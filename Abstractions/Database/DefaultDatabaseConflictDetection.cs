//-----------------------------------------------------------------------------
// Copyright (c) 2020-2022 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using System;
using System.Linq;

namespace EllipticBit.Services.Database
{
	public sealed class DefaultDatabaseConflictDetection : IDatabaseConflictDetection
	{
		public bool IsConflicted(bool? local, bool? remote) => local != remote;

		public bool IsConflicted(byte? local, byte? remote) => local != remote;

		public bool IsConflicted(short? local, short? remote) => local != remote;

		public bool IsConflicted(int? local, int? remote) => local != remote;

		public bool IsConflicted(long? local, long? remote) => local != remote;

		public bool IsConflicted(ushort? local, ushort? remote) => local != remote;

		public bool IsConflicted(uint? local, uint? remote) => local != remote;

		public bool IsConflicted(ulong? local, ulong? remote) => local != remote;

		public bool IsConflicted(float? local, float? remote) => local != remote;

		public bool IsConflicted(double? local, double? remote) => local != remote;

		public bool IsConflicted(decimal? local, decimal? remote) => local != remote;

		public bool IsConflicted(Guid? local, Guid? remote) => local != remote;

		public bool IsConflicted(TimeSpan? local, TimeSpan? remote) => local != remote;

		public bool IsConflicted(DateTime? local, DateTime? remote) => local != remote;

		public bool IsConflicted(DateTimeOffset? local, DateTimeOffset? remote) => local != remote;

		public bool IsConflicted(string local, string remote) {
			if (local == null && remote == null) return false;
			if (local != null && remote == null) return true;
			return local == null || !local.Equals(remote, StringComparison.Ordinal);
		}

		public bool IsConflicted(byte[] local, byte[] remote) {
			if (local == null && remote == null) return false;
			if (local != null && remote == null) return true;
			return local == null || !local.SequenceEqual(remote);
		}

		public bool IsConflicted(DataValue local, DataValue remote) {
			if (local.IsNullable) {
				switch (local.DataType) {
					case DatabaseValueType.Bool:
						return IsConflicted((bool?)local.Value, (bool?)remote.Value);
					case DatabaseValueType.Byte:
						return IsConflicted((byte?)local.Value, (byte?)remote.Value);
					case DatabaseValueType.Short:
						return IsConflicted((short?)local.Value, (short?)remote.Value);
					case DatabaseValueType.UShort:
						return IsConflicted((ushort?)local.Value, (ushort?)remote.Value);
					case DatabaseValueType.Int:
						return IsConflicted((int?)local.Value, (int?)remote.Value);
					case DatabaseValueType.UInt:
						return IsConflicted((uint?)local.Value, (uint?)remote.Value);
					case DatabaseValueType.Long:
						return IsConflicted((long?)local.Value, (long?)remote.Value);
					case DatabaseValueType.ULong:
						return IsConflicted((ulong?)local.Value, (ulong?)remote.Value);
					case DatabaseValueType.Float:
						return IsConflicted((float?)local.Value, (float?)remote.Value);
					case DatabaseValueType.Double:
						return IsConflicted((double?)local.Value, (double?)remote.Value);
					case DatabaseValueType.Decimal:
						return IsConflicted((decimal?)local.Value, (decimal?)remote.Value);
					case DatabaseValueType.ByteArray:
						return IsConflicted((byte[])local.Value, (byte[])remote.Value);
					case DatabaseValueType.String:
						return IsConflicted((string)local.Value, (string)remote.Value);
					case DatabaseValueType.Guid:
						return IsConflicted((Guid?)local.Value, (Guid?)remote.Value);
					case DatabaseValueType.TimeSpan:
						return IsConflicted((TimeSpan?)local.Value, (TimeSpan?)remote.Value);
					case DatabaseValueType.DateTime:
						return IsConflicted((DateTime?)local.Value, (DateTime?)remote.Value);
					case DatabaseValueType.DateTimeOffset:
						return IsConflicted((DateTimeOffset?)local.Value, (DateTimeOffset?)remote.Value);
				}
			}
			else {
				switch (local.DataType) {
					case DatabaseValueType.Bool:
						return IsConflicted((bool)local.Value, (bool)remote.Value);
					case DatabaseValueType.Byte:
						return IsConflicted((byte)local.Value, (byte)remote.Value);
					case DatabaseValueType.Short:
						return IsConflicted((short)local.Value, (short)remote.Value);
					case DatabaseValueType.UShort:
						return IsConflicted((ushort)local.Value, (ushort)remote.Value);
					case DatabaseValueType.Int:
						return IsConflicted((int)local.Value, (int)remote.Value);
					case DatabaseValueType.UInt:
						return IsConflicted((uint)local.Value, (uint)remote.Value);
					case DatabaseValueType.Long:
						return IsConflicted((long)local.Value, (long)remote.Value);
					case DatabaseValueType.ULong:
						return IsConflicted((ulong)local.Value, (ulong)remote.Value);
					case DatabaseValueType.Float:
						return IsConflicted((float)local.Value, (float)remote.Value);
					case DatabaseValueType.Double:
						return IsConflicted((double)local.Value, (double)remote.Value);
					case DatabaseValueType.Decimal:
						return IsConflicted((decimal)local.Value, (decimal)remote.Value);
					case DatabaseValueType.ByteArray:
						return IsConflicted((byte[])local.Value, (byte[])remote.Value);
					case DatabaseValueType.String:
						return IsConflicted((string)local.Value, (string)remote.Value);
					case DatabaseValueType.Guid:
						return IsConflicted((Guid)local.Value, (Guid)remote.Value);
					case DatabaseValueType.TimeSpan:
						return IsConflicted((TimeSpan)local.Value, (TimeSpan)remote.Value);
					case DatabaseValueType.DateTime:
						return IsConflicted((DateTime)local.Value, (DateTime)remote.Value);
					case DatabaseValueType.DateTimeOffset:
						return IsConflicted((DateTimeOffset)local.Value, (DateTimeOffset)remote.Value);
				}
			}

			return false;
		}
	}
}
