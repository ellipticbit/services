//-----------------------------------------------------------------------------
// Copyright (c) 2020-2024 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using System;

namespace EllipticBit.Services.Email
{

	public abstract class EmailServiceOptions
	{
		internal Type ImplementationType;

		public EmailAddress FromAddress { get; }

		protected EmailServiceOptions(Type implementationType, EmailAddress fromAddress) {
			this.ImplementationType = implementationType;
			this.FromAddress = fromAddress;
		}
	}
}
