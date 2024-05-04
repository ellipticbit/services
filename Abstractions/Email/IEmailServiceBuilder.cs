//-----------------------------------------------------------------------------
// Copyright (c) 2020-2024 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

namespace EllipticBit.Services.Email
{
	public interface IEmailServiceBuilder
	{
		IEmailServiceBuilder AddEmailService(string name, EmailServiceOptions options);
	}
}
