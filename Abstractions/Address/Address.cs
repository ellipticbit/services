//-----------------------------------------------------------------------------
// Copyright (c) 2020-2023 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace EllipticBit.Services.Address
{
	public sealed class Address
	{

		[JsonPropertyName("firm")]
		[DataMember(Name = "firm")] 
		public string FirmName { get; set; } = string.Empty;

		[JsonPropertyName("streetAddress")]
		[DataMember(Name = "streetAddress")]
		public string Address1 { get; set; } = string.Empty;

		[JsonPropertyName("secondaryAddress")]
		[DataMember(Name = "secondaryAddress")]
		public string Address2 { get; set; } = string.Empty;

		[JsonPropertyName("city")]
		[DataMember(Name = "city")]
		public string City { get; set; } = string.Empty;

		[JsonPropertyName("state")]
		[DataMember(Name = "state")]
		public string Region { get; set; } = string.Empty;

		[JsonPropertyName("ZIPCode")]
		[DataMember(Name = "ZIPCode")]
		public string Zip { get; set; } = string.Empty;

		[JsonPropertyName("ZIPPlus4")]
		[DataMember(Name = "ZIPPlus4")]
		public string Zip4 { get; set; } = string.Empty;
	}
}
