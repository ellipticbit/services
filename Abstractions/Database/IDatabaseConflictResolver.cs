//-----------------------------------------------------------------------------
// Copyright (c) 2020 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;

namespace EllipticBit.Services.Database
{
	public interface IDatabaseConflictResolver
	{
		Task<IEnumerable<DataConflictResolution>> Resolve(IEnumerable<DataConflict> conflicts);
	}
}
