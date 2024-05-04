//-----------------------------------------------------------------------------
// Copyright (c) 2020-2024 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;

namespace EllipticBit.Services.Email
{
	public interface IEmailService { }

	public interface IEmailService<TResult> : IEmailService
	{
		Task<TResult> Send(EmailAddress to, string subject, string text, string html = null, EmailAddress from = null, IEnumerable<EmailAttachment> attachments = null);
		Task<TResult> Send(IEnumerable<EmailAddress> to, string subject, string text, string html = null, EmailAddress from = null, IEnumerable<EmailAddress> cc = null, IEnumerable<EmailAddress> bcc = null, IEnumerable<EmailAttachment> attachments = null);
	}
}
