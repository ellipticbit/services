//-----------------------------------------------------------------------------
// Copyright (c) 2020-2024 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

namespace EllipticBit.Services.Email
{
	public abstract class EmailTemplateBase
	{
		public EmailAddress ToAddress { get; }

		protected EmailTemplateBase(EmailAddress toAddress) {
			this.ToAddress = toAddress;
		}
	}
}
