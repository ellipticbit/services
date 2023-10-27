//-----------------------------------------------------------------------------
// Copyright (c) 2020 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using System;

namespace EllipticBit.Services.Database
{
	public interface IDatabaseConflictDetection
	{
		bool IsConflicted(bool? local, bool? remote);
		bool IsConflicted(byte? local, byte? remote);
		bool IsConflicted(short? local, short? remote);
		bool IsConflicted(int? local, int? remote);
		bool IsConflicted(long? local, long? remote);
		bool IsConflicted(float? local, float? remote);
		bool IsConflicted(double? local, double? remote);
		bool IsConflicted(decimal? local, decimal? remote);
		bool IsConflicted(byte[] local, byte[] remote);
		bool IsConflicted(string local, string remote);
		bool IsConflicted(Guid? local, Guid? remote);
		bool IsConflicted(TimeSpan? local, TimeSpan? remote);
		bool IsConflicted(DateTime? local, DateTime? remote);
		bool IsConflicted(DateTimeOffset? local, DateTimeOffset? remote);
		bool IsConflicted(DataValue local, DataValue remote);
	}
}
