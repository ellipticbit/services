using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EllipticBit.Services.Email
{
	public interface IEmailScheduleService : IEmailTemplateService
	{
		Task Send(EmailAddress to, DateTimeOffset sendAt, string subject, string text, string html = null, EmailAddress from = null, List<EmailAttachment> attachments = null);
		Task Send(IEnumerable<EmailAddress> to, DateTimeOffset sendAt, string subject, string text, string html = null, EmailAddress from = null, IEnumerable<EmailAddress> cc = null, IEnumerable<EmailAddress> bcc = null, List<EmailAttachment> attachments = null);
		Task<string> SendTemplate<T>(string templateId, List<EmailData<T>> emailData, DateTimeOffset sendAt, EmailAddress from = null, List<EmailAttachment> attachments = null) where T : class;
		Task SendTemplate<T>(string templateId, EmailData<T> emailData, DateTimeOffset sendAt, EmailAddress from = null, List<EmailAttachment> attachments = null) where T : class;
	}
}
