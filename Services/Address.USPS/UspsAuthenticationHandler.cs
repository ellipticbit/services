//-----------------------------------------------------------------------------
// Copyright (c) 2025 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

namespace EllipticBit.Services.Address
{
	using System;
	using System.Collections.Generic;
	using System.Net.Http;
	using System.Net.Http.Json;
	using System.Text.Json.Serialization;
	using System.Threading;
	using System.Threading.Tasks;
	using EllipticBit.Coalescence.Shared;

	internal class UspsAuthenticationHandler : ICoalescenceAuthentication
	{
		private static SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

		private readonly IHttpClientFactory factory;
		private readonly UspsAddressServiceOptions options;

		private string token = null;
		private DateTimeOffset expiresAt = DateTimeOffset.MinValue;

		public UspsAuthenticationHandler(IHttpClientFactory factory, UspsAddressServiceOptions options) {
			this.factory = factory;
			this.options = options;
		}

		public async Task<string> Get(string tenantId = null) {
			if (expiresAt >= DateTimeOffset.Now) {
				return token;
			}

			await semaphore.WaitAsync();
			try {
				//If we ended up waiting for the semaphore, and it completed, then we already have a new token and should just return it.
				if (expiresAt >= DateTimeOffset.Now) {
					return token;
				}

				var credential = await options.GetCredential(tenantId);

				var content = new FormUrlEncodedContent(new Dictionary<string, string>() {
					{ "grant_type", "client_credentials" },
					{ "client_id", credential.ConsumerKey },
					{ "client_secret", credential.ConsumerSecret }
				});

				using var http = factory.CreateClient("USPS");
				using var response = await http.PostAsync(new Uri("oauth2/v3/token", UriKind.Relative), content);
				response.EnsureSuccessStatusCode();
				var tokenData = await response.Content.ReadFromJsonAsync<UspsAuthenticationResponse>();

				token = tokenData.Token;
				expiresAt = DateTimeOffset.FromUnixTimeMilliseconds(tokenData.IssuedAt).AddSeconds(tokenData.ExpiresIn).AddMinutes(-10);
			}
			finally
			{
				semaphore.Release();
			}

			return token;
		}

		public string Name => "USPS";
		public string Scheme => "Bearer";
		public bool ContinueOnFailure => false;
	}

	internal class UspsAuthenticationResponse
	{
		[JsonPropertyName("access_token")]
		public string Token { get; set; }

		[JsonPropertyName("token_type")]
		public string TokenType { get; set; }

		[JsonPropertyName("issued_at")]
		public long IssuedAt { get; set; }

		[JsonPropertyName("expires_in")]
		public long ExpiresIn { get; set; }
	}
}
