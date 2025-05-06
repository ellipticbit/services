//-----------------------------------------------------------------------------
// Copyright (c) 2024-2025 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using System.Net;
using System.Net.Mail;

namespace EllipticBit.Services.Email
{
	public class SmtpClientEmailServiceOptions : EmailServiceOptions<SmtpClientEmailService>
	{
		public int Port { get; set; }
		public string Host { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }

		public bool EnableSsl { get; set; } = true;
		public bool UseUtf8 { get; set; } = true;

		public SmtpClientEmailServiceOptions(EmailAddress fromAddress, int port, string host, string username, string password) : base(fromAddress) {
			Port = port;
			Host = host;
			Username = username;
			Password = password;
		}

		internal SmtpClient GetSmtpClient() {
			return new SmtpClient(Host, Port) {
				Credentials = new NetworkCredential(Username, Password),
				DeliveryFormat = UseUtf8 ? SmtpDeliveryFormat.International : SmtpDeliveryFormat.SevenBit,
				DeliveryMethod = SmtpDeliveryMethod.Network,
				EnableSsl = this.EnableSsl,
			};
		}

		internal MailAddress GetDefaultAddress() {
			return new MailAddress(Username);
		}
	}
}
