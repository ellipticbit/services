//-----------------------------------------------------------------------------
// Copyright (c) 2020-2024 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using System;
using System.Net.Http;
using SendGrid;
using SendGrid.Helpers.Reliability;

namespace EllipticBit.Services.Email
{
	public sealed class SendGridEmailServiceOptions : EmailServiceOptions
	{
		public string ApiKey { get; }
		public string HttpClientName { get; set; } = null;
		public string Host { get; set; } = null;
		public int MaxRetries { get; set; } = 3;
		public TimeSpan MinimumBackOff { get; set; } = TimeSpan.FromSeconds(10);
		public TimeSpan MaximumBackOff { get; set; } = TimeSpan.FromSeconds(1);
		public TimeSpan DeltaBackOff { get; set; } = TimeSpan.FromSeconds(1);

		public SendGridEmailServiceOptions(string apiKey, string fromAddress, string fromName)
		: base(typeof(SendGridEmailService), new EmailAddress(fromAddress, fromName)) {
			this.ApiKey = apiKey;
		}

		internal SendGridClient GetSendGridClient(IHttpClientFactory httpClientFactory) {
			var sgco = new SendGridClientOptions() {
				ApiKey = ApiKey,
				ReliabilitySettings = new ReliabilitySettings(MaxRetries, MinimumBackOff, MaximumBackOff, DeltaBackOff),
			};
			if (!string.IsNullOrWhiteSpace(this.Host)) sgco.Host = this.Host;
			var http = string.IsNullOrWhiteSpace(HttpClientName) ? httpClientFactory.CreateClient() : httpClientFactory.CreateClient(HttpClientName);
			return new SendGridClient(http, sgco);
		}
	}
}
