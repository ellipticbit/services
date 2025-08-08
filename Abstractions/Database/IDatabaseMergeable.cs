//-----------------------------------------------------------------------------
// Copyright (c) 2020-2025 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EllipticBit.Services.Database
{
	public interface IDatabaseMergeable<in T> where T : class, IDatabaseMergeable<T>
	{
		DataValue[] GetMergeableValues();
		void ApplyConflictResolutions(DataConflictResolution[] resolutions);
	}

	public static class IDatabaseMergeableExtensions
	{
		public static DataConflict[] GetConflicts<T>(this T local, T remote, IDatabaseConflictDetection detection)
			where T : class, IDatabaseMergeable<T>
		{
			var lvl = local.GetMergeableValues();
			var rvl = remote.GetMergeableValues();

			var rl = new List<DataConflict>(lvl.Length);
			rl.AddRange(from lv in lvl let rv = rvl.FirstOrDefault(a => a.Name.Equals(lv.Name, StringComparison.Ordinal)) where rv != null where detection.IsConflicted(lv, rv) select new DataConflict(lv.DataType, lv.IsNullable, lv.Name, lv.Value, rv.Value));
			return rl.ToArray();
		}

		public static Task<IEnumerable<DataConflictResolution>> GetResolutions<T>(this T local, T remote, IDatabaseConflictDetection detection, IDatabaseConflictResolver resolver)
			where T : class, IDatabaseMergeable<T>
		{
			return resolver.Resolve(local.GetConflicts(remote, detection));
		}

		public static async Task MergeTo<T>(this T from, T to, IDatabaseConflictDetection detection, IDatabaseConflictResolver resolver)
			where T : class, IDatabaseMergeable<T>
		{
			var resolutions = await from.GetResolutions(to, detection, resolver);
			to.ApplyConflictResolutions(resolutions.ToArray());
		}

		public static async Task Merge<T>(this T local, T remote, IDatabaseConflictDetection detection, IDatabaseConflictResolver resolver)
			where T : class, IDatabaseMergeable<T>
		{
			var resolutions = await local.GetResolutions(remote, detection, resolver);
			var dataConflictResolutions = resolutions as DataConflictResolution[] ?? resolutions.ToArray();
			local.ApplyConflictResolutions(dataConflictResolutions.ToArray());
			remote.ApplyConflictResolutions(dataConflictResolutions.ToArray());
		}
	}
}
