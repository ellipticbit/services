using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EllipticBit.Services.Email
{
	public interface IEmailTemplateService : IEmailService
	{
		Task<string> SendTemplate<T>(string templateId, List<EmailData<T>> emailData, EmailAddress from = null, List<EmailAttachment> attachments = null) where T : class;
		Task SendTemplate<T>(string templateId, EmailData<T> emailData, EmailAddress from = null, List<EmailAttachment> attachments = null) where T : class;
	}
}
