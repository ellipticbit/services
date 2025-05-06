//-----------------------------------------------------------------------------
// Copyright (c) 2025 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using System;

namespace EllipticBit.Services.Email
{
	internal class EmailServiceBuilder<T> : IEmailServiceBuilder<T>
		where T : EmailServiceOptionsBase
	{
		public IEmailServiceBuilder<T> AddEmailService(string name, T value) {
			if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
			if (value == null) throw new ArgumentNullException(nameof(value));
			EmailServiceFactory.options = EmailServiceFactory.options.Add(name, value);
			return this;
		}
	}
}
