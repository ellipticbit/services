//-----------------------------------------------------------------------------
// Copyright (c) 2020-2025 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using System.IO;
using System.Net.Mail;
using System.Net.Mime;

using Microsoft.Extensions.DependencyInjection;

namespace EllipticBit.Services.Email
{
	public static class Extensions
	{
		public static IEmailServiceBuilder<SmtpClientEmailServiceOptions> AddSendGridEmailService(this IServiceCollection services) {
			return services.AddEmailServices<SmtpClientEmailServiceOptions>();
		}

		internal static MailAddress ToEmailAddress(this EmailAddress address) {
			return new MailAddress(address.Email, address.Name);
		}

		internal static Attachment ToAttachment(this EmailAttachment attachment) {
			var ms = new MemoryStream(attachment.Content);
			ms.Position = 0;
			var na =  new Attachment(ms, new ContentType(attachment.Type)) {
				Name = attachment.FileName,
				ContentId = attachment.Id
			};
			na.ContentDisposition.DispositionType = attachment.Id != null ? "inline" : "attachment";
			return na;
		}
	}
}
