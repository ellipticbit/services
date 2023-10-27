//-----------------------------------------------------------------------------
// Copyright (c) 2020-2022 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

namespace EllipticBit.Services.Database
{
	public sealed class DataConflict
	{
		public DatabaseValueType DataType { get; }
		public bool IsNullable { get; }
		public string Name { get; }
		public object Local { get; }
		public object Remote { get; }

		public DataConflict(DatabaseValueType dataType, bool isNullable, string name, object local, object remote) {
			this.DataType = dataType;
			this.IsNullable = isNullable;
			this.Name = name;
			this.Local = local;
			this.Remote = remote;
		}

		public DataConflictResolution Resolve(object resolved) {
			return new DataConflictResolution(this.Name, resolved);
		}
	}
}
