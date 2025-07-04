//-----------------------------------------------------------------------------
// Copyright (c) 2020-2025 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using System;
using Microsoft.Extensions.DependencyInjection;
using SendGrid.Helpers.Mail;

namespace EllipticBit.Services.Email
{
	public static class Extensions
	{
		public static IEmailServiceBuilder<SendGridEmailServiceOptions> AddSendGridEmailServices(this IServiceCollection services) {
			return services.AddEmailServices<SendGridEmailServiceOptions>();
		}

		internal static SendGrid.Helpers.Mail.EmailAddress ToEmailAddress(this EmailAddress address) {
			return new SendGrid.Helpers.Mail.EmailAddress(address.Email, address.Name);
		}

		internal static Attachment ToAttachment(this IEmailAttachment attachment) {
			return new Attachment() {
				Content = Convert.ToBase64String(attachment.Content),
				ContentId = attachment.Id,
				Disposition = attachment.Id != null ? "inline" : "attachment",
				Filename = attachment.FileName
			};
		}
	}
}
