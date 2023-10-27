//-----------------------------------------------------------------------------
// Copyright (c) 2020-2022 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace EllipticBit.Services.Database
{
	public abstract class EfCoreDatabaseService<TDatabase> : DbContext
		where TDatabase : EfCoreDatabaseService<TDatabase>
	{
		private readonly IDatabaseConflictDetection detection;
		private readonly IDatabaseConflictResolver resolver;

		protected EfCoreDatabaseService() : base() { }

		protected EfCoreDatabaseService(IDatabaseServiceOptions options, IDatabaseConflictDetection detection, IDatabaseConflictResolver resolver) : base(((EfCoreDatabaseServiceOptions)options).DbContextOptions) {
			this.detection = detection;
			this.resolver = resolver;
		}

		public async Task Transaction(Func<TDatabase, Task> command, IsolationLevel level = IsolationLevel.ReadCommitted) {
			using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = level }, TransactionScopeAsyncFlowOption.Enabled)) {
				await command((TDatabase)this).ConfigureAwait(false);
				scope.Complete();
			}
		}

		public async Task<T> Transaction<T>(Func<TDatabase, Task<T>> command, IsolationLevel level = IsolationLevel.ReadCommitted) {
			using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = level }, TransactionScopeAsyncFlowOption.Enabled)) {
				var ret = await command((TDatabase)this).ConfigureAwait(false);
				scope.Complete();
				return ret;
			}
		}

		public async Task Transaction(Func<TDatabase, IDatabaseConflictDetection, IDatabaseConflictResolver, Task> command, IsolationLevel level = IsolationLevel.ReadCommitted) {
			using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = level }, TransactionScopeAsyncFlowOption.Enabled)) {
				await command((TDatabase)this, detection, resolver).ConfigureAwait(false);
				scope.Complete();
			}
		}

		public async Task<T> Transaction<T>(Func<TDatabase, IDatabaseConflictDetection, IDatabaseConflictResolver, Task<T>> command, IsolationLevel level = IsolationLevel.ReadCommitted) where T : IDatabaseMergeable<T> {
			using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = level }, TransactionScopeAsyncFlowOption.Enabled)) {
				var ret = await command((TDatabase)this, detection, resolver).ConfigureAwait(false);
				scope.Complete();
				return ret;
			}
		}

		public DataConflict[] GetConflicts<T>(T local, T remote) where T : IDatabaseMergeable<T> {
			var lvl = local.GetMergeableValues();
			var rvl = remote.GetMergeableValues();

			var rl = new List<DataConflict>(lvl.Length);
			rl.AddRange(from lv in lvl let rv = rvl.FirstOrDefault(a => a.Name.Equals(lv.Name, StringComparison.Ordinal)) where rv != null where detection.IsConflicted(lv, rv) select new DataConflict(lv.DataType, lv.IsNullable, lv.Name, lv.Value, rv.Value));
			return rl.ToArray();
		}

		public Task<IEnumerable<DataConflictResolution>> GetResolutions(IEnumerable<DataConflict> conflicts) {
			return resolver.Resolve(conflicts);
		}

		public async Task MergeToLocal<T>(T local, T remote) where T : IDatabaseMergeable<T>
		{
			var resolutions = await GetResolutions(GetConflicts(local, remote));
			local.ApplyConflictResolutions(resolutions.ToArray());
		}

		public async Task MergeToRemote<T>(T local, T remote) where T : IDatabaseMergeable<T>
		{
			var resolutions = await GetResolutions(GetConflicts(local, remote));
			remote.ApplyConflictResolutions(resolutions.ToArray());
		}

		public async Task Merge<T>(T local, T remote) where T : IDatabaseMergeable<T>
		{
			var resolutions = await GetResolutions(GetConflicts(local, remote));
			var dataConflictResolutions = resolutions as DataConflictResolution[] ?? resolutions.ToArray();
			local.ApplyConflictResolutions(dataConflictResolutions.ToArray());
			remote.ApplyConflictResolutions(dataConflictResolutions.ToArray());
		}
	}
}
