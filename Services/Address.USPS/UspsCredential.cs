//-----------------------------------------------------------------------------
// Copyright (c) 2025 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

namespace EllipticBit.Services.Address
{
	using System.Text.Json.Serialization;

	public class UspsCredential
	{
		[JsonPropertyName("key")]
		public string ConsumerKey { get; set; }

		[JsonPropertyName("secret")]
		public string ConsumerSecret { get; set; }

		public UspsCredential(string consumerKey, string consumerSecret)
		{
			this.ConsumerKey = consumerKey;
			this.ConsumerSecret = consumerSecret;
		}
	}
}
