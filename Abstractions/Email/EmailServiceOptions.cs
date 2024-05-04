using System;

namespace EllipticBit.Services.Email
{
	internal enum EmailServiceImplementation {
		Service,
		Template,
	}

	public abstract class EmailServiceOptions
	{
		internal Type EmailServiceType { get; }
		internal EmailServiceImplementation ServiceImplementation { get; }

		protected internal EmailServiceOptions(Type emailServiceType)
		{
			EmailServiceType = emailServiceType;

			var ets = EmailServiceType.GetInterface("IEmailTemplateService");
			var es = EmailServiceType.GetInterface("IEmailService");
			if (ets != null)
			{
				ServiceImplementation = EmailServiceImplementation.Template;
			}
			else if (es != null)
			{
				ServiceImplementation = EmailServiceImplementation.Service;
			}
			else
			{
				throw new ArgumentNullException(nameof(emailServiceType), "Email Service Type does not implement an appropriate interface.");
			}
		}
	}

	public abstract class EmailServiceOptions<T> : EmailServiceOptions
		where T : IEmailService
	{
		public EmailAddress FromAddress { get; }

		protected EmailServiceOptions(EmailAddress fromAddress) : base(typeof(T)) {
			FromAddress = fromAddress;
		}
	}
}
