using System;

namespace EllipticBit.Services.Email
{

	public abstract class EmailServiceOptions
	{
		public EmailAddress FromAddress { get; }

		protected EmailServiceOptions(EmailAddress fromAddress) {
			FromAddress = fromAddress;
		}
	}
}
