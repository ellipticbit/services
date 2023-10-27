//-----------------------------------------------------------------------------
// Copyright (c) 2020-2023 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using System;
using System.Net.Http;
using SendGrid;
using SendGrid.Helpers.Reliability;

namespace EllipticBit.Services.Email
{
	public sealed class SendGridEmailServiceOptions : EmailServiceOptions
	{
		private readonly string HttpClientName;
		private readonly string Host;
		private readonly string ApiKey;
		private readonly int MaxRetries;
		private readonly TimeSpan MinimumBackOff;
		private readonly TimeSpan MaximumBackOff;
		private readonly TimeSpan DeltaBackOff;

		public SendGridEmailServiceOptions(string apiKey, string fromAddress, string fromName, string httpClientName = null, string host = null, int maxRetries = 3, TimeSpan? maxBackOff = null, TimeSpan? minBackOff = null, TimeSpan? deltaBackOff = null)
		: base(typeof(SendGridEmailService), new EmailAddress(fromAddress, fromName)) {
			this.HttpClientName = httpClientName;
			this.Host = host;
			this.ApiKey = apiKey;
			this.MaxRetries = maxRetries;
			this.MaximumBackOff = maxBackOff ?? TimeSpan.FromSeconds(10);
			this.MinimumBackOff = minBackOff ?? TimeSpan.FromSeconds(1);
			this.DeltaBackOff = deltaBackOff ?? TimeSpan.FromSeconds(1);
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
