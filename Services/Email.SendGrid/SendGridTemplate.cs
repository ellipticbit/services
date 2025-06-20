//-----------------------------------------------------------------------------
// Copyright (c) 2024-2025 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace EllipticBit.Services.Email
{
	public class SendGridTemplate : EmailTemplateBase
	{
		public string TemplateId { get; }
		public int? UnsubscribeGroupId { get; set; } = null;
		public IEnumerable<int> UnsubscribeGroups { get; set; } = null;
		public DateTimeOffset? SendAt { get; set; } = null;
		public IEnumerable<IEmailAttachment> Attachments { get; set; } = null;
		public object TemplateData { get; set; } = null;

		public SendGridTemplate(EmailAddress toAddress, string templateId) : base(toAddress) {
			this.TemplateId = templateId;
		}
	}
}
