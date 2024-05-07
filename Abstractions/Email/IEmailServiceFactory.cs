//-----------------------------------------------------------------------------
// Copyright (c) 2020-2024 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using System;

namespace EllipticBit.Services.Email
{
	public interface IEmailServiceFactory
	{
		/// <summary>
		/// Create a basic Email Service.
		/// </summary>
		/// <param name="name">The name of the service to create.</param>
		/// <returns cref="IEmailService">An IEmailService interface to the implementation.</returns>
		/// <exception cref="ArgumentNullException">No email service was specified.</exception>
		/// <exception cref="ArgumentOutOfRangeException">No service was registered the implements the IEmailService interface.</exception>
		IEmailService Create(string name);
	}
}
