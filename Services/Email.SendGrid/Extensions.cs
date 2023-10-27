//-----------------------------------------------------------------------------
// Copyright (c) 2020-2023 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using SendGrid.Helpers.Mail;

namespace EllipticBit.Services.Email
{
	public static class Extensions
	{
		public static IEmailServiceBuilder AddSendGridEmailService(this IServiceCollection services, EmailServiceOptions defaultOptions) {
			services.AddTransient<IEmailScheduleService, SendGridEmailService>();
			return services.AddEmailServices(defaultOptions);
		}

		internal static SendGrid.Helpers.Mail.EmailAddress ToEmailAddress(this EmailAddress address) {
			return new SendGrid.Helpers.Mail.EmailAddress(address.Email, address.Name);
		}

		internal static Attachment ToAttachment(this EmailAttachment attachment) {
			return new Attachment() {
				Content = attachment.Content,
				ContentId = attachment.Id,
				Disposition = attachment.Id != null ? "inline" : "attachment",
				Filename = attachment.FileName
			};
		}
	}
}
