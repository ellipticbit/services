using System;

namespace EllipticBit.Services.Email
{
	internal enum EmailServiceImplementation {
		Service,
		Template,
		Schedule,
	}

	public abstract class EmailServiceOptions
	{
		internal Type EmailServiceType { get; }
		internal EmailServiceImplementation ServiceImplementation { get; }

		public EmailAddress FromAddress { get; }

		protected EmailServiceOptions(Type emailServiceType, EmailAddress fromAddress) {
			EmailServiceType = emailServiceType;
			FromAddress = fromAddress;

			var ess = EmailServiceType.GetInterface("IEmailScheduleService");
			var ets = EmailServiceType.GetInterface("IEmailTemplateService");
			var es = EmailServiceType.GetInterface("IEmailService");
			if (ess != null) {
				ServiceImplementation = EmailServiceImplementation.Schedule;
			}
			else if (ets != null) {
				ServiceImplementation = EmailServiceImplementation.Template;
			}
			else if (es != null) {
				ServiceImplementation = EmailServiceImplementation.Service;}
			else {
				throw new ArgumentNullException(nameof(emailServiceType), "Email Service Type does not implement an appropriate interface.");
			}
		}
	}
}
