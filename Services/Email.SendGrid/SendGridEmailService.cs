//-----------------------------------------------------------------------------
// Copyright (c) 2020-2023 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using Newtonsoft.Json.Linq;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace EllipticBit.Services.Email
{
	internal sealed class SendGridEmailService : IEmailScheduleService
	{
		private readonly SendGridClient client;
		private readonly SendGridEmailServiceOptions options;

		public SendGridEmailService(IHttpClientFactory httpFactory, SendGridEmailServiceOptions options) {
			client = options.GetSendGridClient(httpFactory);
			this.options = options;
		}

		public Task Send(EmailAddress to, string subject, string text, string html = null, EmailAddress from = null, List<EmailAttachment> attachments = null) {
			var msg = BuildMessage(new[] { to }, subject, text, html, from, null, null, attachments);
			return client.SendEmailAsync(msg);
		}

		public Task Send(IEnumerable<EmailAddress> to, string subject, string text, string html = null, EmailAddress from = null, IEnumerable<EmailAddress> cc = null, IEnumerable<EmailAddress> bcc = null, List<EmailAttachment> attachments = null) {
			var msg = BuildMessage(to, subject, text, html, from, cc, bcc, attachments);
			return client.SendEmailAsync(msg);
		}

		public async Task<string> SendTemplate<T>(string templateId, List<EmailData<T>> emailData, EmailAddress from = null, List<EmailAttachment> attachments = null) where T : class {
			var batchId = await GetBatchId();
			var ml = BuildTemplateMessage(templateId, emailData, batchId, from, attachments);
			foreach (var msg in ml) {
				msg.SetBatchId(batchId);
				await client.SendEmailAsync(msg);
			}
			return batchId;
		}

		public async Task SendTemplate<T>(string templateId, EmailData<T> emailData, EmailAddress from = null, List<EmailAttachment> attachments = null) where T : class {
			var ml = BuildTemplateMessage(templateId, new[] { emailData }, null, from, attachments);
			foreach (var msg in ml)
			{
				await client.SendEmailAsync(msg);
			}
		}

		public Task Send(EmailAddress to, DateTimeOffset sendAt, string subject, string text, string html = null, EmailAddress from = null, List<EmailAttachment> attachments = null) {
			var msg = BuildMessage(new[] { to }, subject, text, html, from, null, null, attachments);
			msg.SendAt = sendAt.ToUnixTimeSeconds();
			return client.SendEmailAsync(msg);
		}

		public Task Send(IEnumerable<EmailAddress> to, DateTimeOffset sendAt, string subject, string text, string html = null, EmailAddress from = null, IEnumerable<EmailAddress> cc = null, IEnumerable<EmailAddress> bcc = null, List<EmailAttachment> attachments = null) {
			var msg = BuildMessage(to, subject, text, html, from, cc, bcc, attachments);
			msg.SendAt = sendAt.ToUnixTimeSeconds();
			return client.SendEmailAsync(msg);
		}

		public async Task<string> SendTemplate<T>(string templateId, List<EmailData<T>> emailData, DateTimeOffset sendAt, EmailAddress from = null, List<EmailAttachment> attachments = null) where T : class {
			var batchId = await GetBatchId();
			var ml = BuildTemplateMessage(templateId, emailData, batchId, from, attachments);
			foreach (var msg in ml)
			{
				msg.SendAt = sendAt.ToUnixTimeSeconds();
				msg.SetBatchId(batchId);
				await client.SendEmailAsync(msg);
			}
			return batchId;
		}

		public async Task SendTemplate<T>(string templateId, EmailData<T> emailData, DateTimeOffset sendAt, EmailAddress from = null, List<EmailAttachment> attachments = null) where T : class {
			var ml = BuildTemplateMessage(templateId, new[] { emailData }, null, from, attachments);
			foreach (var msg in ml) {
				msg.SendAt = sendAt.ToUnixTimeSeconds();
				await client.SendEmailAsync(msg);
			}
		}

		private SendGridMessage BuildMessage(IEnumerable<EmailAddress> to, string subject, string text, string html = null, EmailAddress from = null, IEnumerable<EmailAddress> cc = null, IEnumerable<EmailAddress> bcc = null, List<EmailAttachment> attachments = null)
		{
			if (string.IsNullOrWhiteSpace(html) && string.IsNullOrWhiteSpace(text)) throw new ArgumentNullException(nameof(html), "No HTML or Text body content provided. Please provide at least one type of email body content.");
			var tl = to.ToArray();
			if (tl == null || !tl.Any()) throw new ArgumentNullException(nameof(to));

			var msg = new SendGridMessage();
			msg.SetFrom(from?.ToEmailAddress() ?? options.FromAddress.ToEmailAddress());
			msg.AddTos(tl.Select(a => a.ToEmailAddress()).ToList());
			if (cc != null) msg.AddCcs(cc.Select(a => a.ToEmailAddress()).ToList());
			if (bcc != null) msg.AddCcs(bcc.Select(a => a.ToEmailAddress()).ToList());
			msg.Subject = subject;
			if (!string.IsNullOrEmpty(html)) msg.HtmlContent = html;
			if (!string.IsNullOrEmpty(text)) msg.PlainTextContent = text;
			msg.Attachments = attachments?.Select(a => a.ToAttachment()).ToList();
			return msg;
		}

		private IEnumerable<SendGridMessage> BuildTemplateMessage<T>(string templateId, IEnumerable<EmailData<T>> emailData, string batchId = null, EmailAddress from = null, List<EmailAttachment> attachments = null) where T : class {
			if (string.IsNullOrWhiteSpace(templateId)) throw new ArgumentNullException(nameof(templateId));
			var ed = emailData?.ToArray();
			if (ed == null || !ed.Any()) throw new ArgumentNullException(nameof(emailData));
			if (ed.Length > 1 && string.IsNullOrWhiteSpace(batchId)) throw new ArgumentNullException(nameof(batchId));

			var rl = new List<SendGridMessage>();
			foreach (var data in ed) {
				var msg = new SendGridMessage();
				msg.SetFrom(from?.ToEmailAddress() ?? options.FromAddress.ToEmailAddress());
				msg.AddTo(data.Address.ToEmailAddress());
				msg.SetTemplateId(templateId);
				msg.SetTemplateData(data.TemplateData);
				msg.Attachments = attachments?.Select(a => a.ToAttachment()).ToList();
				rl.Add(msg);
			}
			return rl;
		}

		private async Task<string> GetBatchId() {
			var response = await client.RequestAsync(method: SendGridClient.Method.POST, urlPath: "mail/batch");
			if (response.StatusCode != HttpStatusCode.Created) {
				throw new HttpRequestException("Unable to generate SendGrid batch ID.");
			}
			var json = await response.Body.ReadAsStringAsync();
			return JObject.Parse(json)["batch_id"].Value<string>();
		}
	}
}
