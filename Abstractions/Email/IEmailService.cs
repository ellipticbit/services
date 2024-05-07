//-----------------------------------------------------------------------------
// Copyright (c) 2020-2024 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EllipticBit.Services.Email
{
	public interface IEmailService
	{
		Task<IEmailResult> Send(IEnumerable<EmailAddress> to, string subject, string text, string html = null, EmailAddress from = null, DateTimeOffset? sendAt = null, IEnumerable<EmailAddress> cc = null, IEnumerable<EmailAddress> bcc = null, IEnumerable<EmailAttachment> attachments = null);

		Task<IEmailResult> Send<TTemplate>(IEnumerable<TTemplate> templateData, EmailAddress from = null)
			where TTemplate : EmailTemplateBase;
	}

	public static class EmailServiceExtensions
	{
		public static Task<IEmailResult> Send(this IEmailService service, EmailAddress to, string subject, string text, string html = null, EmailAddress from = null,  DateTimeOffset? sendAt = null,IEnumerable<EmailAttachment> attachments = null)
		{
			return service.Send(new [] {to}, subject, text, html, from, sendAt,null, null, attachments);
		}

		public static Task<IEmailResult> Send<TTemplate>(this IEmailService service, TTemplate templateData, EmailAddress from = null)
			where TTemplate : EmailTemplateBase
		{
			return service.Send(new[] { templateData }, from);
		}
	}
}
