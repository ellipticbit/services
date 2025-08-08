//-----------------------------------------------------------------------------
// Copyright (c) 2020-2025 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using System.Transactions;

namespace EllipticBit.Services.Database
{
	public abstract class EfCoreDatabaseService<TDatabase> : DbContext
		where TDatabase : EfCoreDatabaseService<TDatabase>
	{
		protected EfCoreDatabaseService(DbContextOptions options) : base(options) { }

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
	}
}
