//-----------------------------------------------------------------------------
// Copyright (c) 2020-2025 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using System;

namespace EllipticBit.Services.Email
{

	public abstract class EmailServiceOptionsBase
	{
		internal Type ImplementationType;

		public EmailAddress FromAddress { get; }

		private protected EmailServiceOptionsBase(Type emailServiceType, EmailAddress fromAddress) {
			this.ImplementationType = emailServiceType;
			this.FromAddress = fromAddress;
		}
	}

	public abstract class EmailServiceOptions<T> : EmailServiceOptionsBase
		where T : class, IEmailService
	{
		protected EmailServiceOptions(EmailAddress fromAddress) : base(typeof(T), fromAddress) { }
	}
}
