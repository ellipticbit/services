//-----------------------------------------------------------------------------
// Copyright (c) 2020-2025 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

namespace EllipticBit.Services.Email
{
	public interface IEmailServiceBuilder<T>
		where T : EmailServiceOptionsBase
	{
		IEmailServiceBuilder<T> AddEmailService(string name, T options);
	}
}
