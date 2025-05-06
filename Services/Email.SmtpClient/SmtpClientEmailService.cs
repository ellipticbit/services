//-----------------------------------------------------------------------------
// Copyright (c) 2024-2025 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace EllipticBit.Services.Email
{
	public sealed class SmtpClientEmailService : IEmailService
	{
		private readonly SmtpClientEmailServiceOptions options;

		public SmtpClientEmailService(SmtpClientEmailServiceOptions options)
		{
			this.options = options;
		}

		public async Task<IEmailResult> Send(IEnumerable<EmailAddress> to, string subject, string text, string html = null, EmailAddress from = null, DateTimeOffset? sendAt = null, IEnumerable<EmailAddress> cc = null, IEnumerable<EmailAddress> bcc = null, IEnumerable<EmailAttachment> attachments = null) {
			if (sendAt != null) throw new ArgumentOutOfRangeException(nameof(sendAt), "SmtpClient does not support delayed send.");

			using (var client = options.GetSmtpClient()) {
				//Create the message
				var msg = new MailMessage() {
					From = from?.ToEmailAddress() ?? options.GetDefaultAddress(),
					Subject = subject,
				};

				//Add the To/CC/BCC addresses
				foreach (var a in to) {
					msg.To.Add(a.ToEmailAddress());
				}

				if (cc != null) {
					foreach (var a in cc) {
						msg.CC.Add(a.ToEmailAddress());
					}
				}

				if (bcc != null) {
					foreach (var a in bcc) {
						msg.Bcc.Add(a.ToEmailAddress());
					}
				}

				//Add the attachments
				var emailAttachments = attachments as EmailAttachment[] ?? attachments?.ToArray();
				if (emailAttachments != null) {
					foreach (var a in emailAttachments) {
						msg.Attachments.Add(a.ToAttachment());
					}
				}

				//Add any content attachment links
				var alinks = emailAttachments?.Where(a => !string.IsNullOrWhiteSpace(a.Id)).Select(a => new LinkedResource(a.FileName) { ContentId = a.Id });

				//Set the body content
				if (!string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(html)) {
					msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, Encoding.UTF8, "text/plain"));
					var hv = AlternateView.CreateAlternateViewFromString(html, Encoding.UTF8, "text/html");
					if (alinks != null) {
						foreach (var a in alinks) {
							hv.LinkedResources.Add(a);
						}
					}
					msg.AlternateViews.Add(hv);
				}
				else if (string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(html)) {
					msg.Body = html;
					msg.IsBodyHtml = true;
				}
				else if (!string.IsNullOrWhiteSpace(text) && string.IsNullOrWhiteSpace(html)) {
					msg.Body = text;
					msg.IsBodyHtml = false;
				}
				else {
					msg.Body = string.Empty;
					msg.IsBodyHtml = false;
				}

				await client.SendMailAsync(msg);
			}

			return new SmtpClientResult(true);
		}

		public Task<IEmailResult> Send<TTemplate>(IEnumerable<TTemplate> templateData, EmailAddress from = null) where TTemplate : EmailTemplateBase {
			throw new NotImplementedException();
		}
	}
}
