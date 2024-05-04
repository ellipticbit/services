//-----------------------------------------------------------------------------
// Copyright (c) 2020-2024 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;

namespace EllipticBit.Services.Email
{
	public interface IEmailService
	{
		Task<TResult> Send<TResult>(EmailAddress to, string subject, string text, string html = null, EmailAddress from = null, IEnumerable<EmailAttachment> attachments = null);
		Task<TResult> Send<TResult>(IEnumerable<EmailAddress> to, string subject, string text, string html = null, EmailAddress from = null, IEnumerable<EmailAddress> cc = null, IEnumerable<EmailAddress> bcc = null, IEnumerable<EmailAttachment> attachments = null);
	}
}
