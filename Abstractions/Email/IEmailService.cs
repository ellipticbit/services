//-----------------------------------------------------------------------------
// Copyright (c) 2020-2023 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;

namespace EllipticBit.Services.Email
{
	public interface IEmailService
	{
		Task Send(EmailAddress to, string subject, string text, string html = null, EmailAddress from = null, List<EmailAttachment> attachments = null);
		Task Send(IEnumerable<EmailAddress> to, string subject, string text, string html = null, EmailAddress from = null, IEnumerable<EmailAddress> cc = null, IEnumerable<EmailAddress> bcc = null, List<EmailAttachment> attachments = null);
	}
}
