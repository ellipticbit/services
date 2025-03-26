//-----------------------------------------------------------------------------
// Copyright (c) 2025 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

namespace EllipticBit.Services.Address
{
	using System;
	using System.Collections.Generic;
	using System.Text.Json.Serialization;
	using System.Threading.Tasks;
	using EllipticBit.Coalescence.Shared;
	using EllipticBit.Coalescence.Shared.Request;

	internal class UspsAuthenticationHandler : ICoalescenceAuthentication
	{
		private readonly ICoalescenceRequestFactory requests;
		private readonly UspsAddressServiceOptions options;

		private string token = string.Empty;
		private DateTimeOffset expiresAt = DateTimeOffset.MinValue;

		public UspsAuthenticationHandler(ICoalescenceRequestFactory requests, UspsAddressServiceOptions options) {
			this.requests = requests;
			this.options = options;
		}

		public async Task<string> Get(string tenantId = null) {
			var credential = await options.GetCredential(tenantId);

			if (expiresAt >= DateTimeOffset.Now) {
				return token;
			}

			var tokenResponse = await requests.CreateRequest().Post().Path("oauth2", "v3", "token").FormUrlEncoded(new Dictionary<string, string>() {
				{ "grant_type", "client_credentials" },
				{ "client_id", credential.ConsumerKey },
				{ "client_secret", credential.ConsumerSecret }
			}).Send();

			var tokenData = await tokenResponse.ThrowOnFailureResponse().AsDeserialized<UspsAuthenticationResponse>();

			expiresAt = DateTimeOffset.FromUnixTimeMilliseconds(tokenData.IssuedAt).AddSeconds(tokenData.ExpiresIn).AddMinutes(-10);
			token = tokenData.Token;

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
