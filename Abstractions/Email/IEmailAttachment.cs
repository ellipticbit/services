//-----------------------------------------------------------------------------
// Copyright (c) 2025 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

namespace EllipticBit.Services.Email
{
	public interface IEmailAttachment
	{
		string Id { get; }
		byte[] Content { get; }
		string FileName { get; }
		string Type { get; }
	}
}
