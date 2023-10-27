//-----------------------------------------------------------------------------
// Copyright (c) 2020-2021 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using System;
using System.Threading.Tasks;

namespace EllipticBit.Services.Database
{
	public interface IDatabaseService<out TDatabase>
		where TDatabase : class
	{
		TDatabase GetDatabase(IDatabaseServiceOptions options, IDatabaseConflictDetection detector, IDatabaseConflictResolver resolver);
	}
}
