//-----------------------------------------------------------------------------
// Copyright (c) 2020-2021 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using System.Threading.Tasks;

namespace EllipticBit.Services.Database
{
	public interface IDatabaseMergeable<in T> where T : IDatabaseMergeable<T>
	{
		DataValue[] GetMergeableValues();
		void ApplyConflictResolutions(DataConflictResolution[] resolutions);
	}
}
