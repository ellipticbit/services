//-----------------------------------------------------------------------------
// Copyright (c) 2020-2024 EllipticBit, LLC All Rights Reserved.
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
	public sealed class SendGridEmailService : IEmailService
	{
		private readonly SendGridClient client;
		private readonly SendGridEmailServiceOptions options;

		public SendGridEmailService(IHttpClientFactory httpFactory, SendGridEmailServiceOptions options) {
			client = options.GetSendGridClient(httpFactory);
			this.options = options;
		}

		public async Task<IEmailResult> Send(IEnumerable<EmailAddress> to, string subject, string text, string html, EmailAddress from, DateTimeOffset? sendAt, IEnumerable<EmailAddress> cc, IEnumerable<EmailAddress> bcc, IEnumerable<EmailAttachment> attachments)
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
			msg.SendAt = sendAt?.ToUnixTimeSeconds();

			var res = await client.SendEmailAsync(msg);
			return new SendGridResult(res);
		}

		public async Task<IEmailResult> Send<TTemplate>(IEnumerable<TTemplate> templateData, EmailAddress from = null)
			where TTemplate : EmailTemplateBase
		{
			var ed = templateData?.OfType<SendGridTemplate>().ToArray();
			if (ed == null || !ed.Any()) throw new ArgumentNullException(nameof(templateData));

			var batchId = await GetBatchId();
			Response sgr = null;
			uint pc = 0;

			foreach (var data in ed) {
				var msg = new SendGridMessage();
				msg.BatchId = batchId;
				msg.SetFrom(from?.ToEmailAddress() ?? options.FromAddress.ToEmailAddress());
				msg.AddTo(data.ToAddress.ToEmailAddress());
				msg.SetTemplateId(data.TemplateId);
				msg.SetTemplateData(data.TemplateData);
				if (data.UnsubscribeGroupId.HasValue) msg.SetAsm(data.UnsubscribeGroupId.Value, data.UnsubscribeGroups.ToList());
				msg.Attachments = data.Attachments?.Select(a => a.ToAttachment()).ToList();
				msg.SendAt = data.SendAt?.ToUnixTimeSeconds();
				sgr = await client.SendEmailAsync(msg);
				if (!sgr.IsSuccessStatusCode) break;
				pc++;
			}

			return new SendGridResult(sgr, batchId, pc);
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
