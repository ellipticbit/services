//-----------------------------------------------------------------------------
// Copyright (c) 2025 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

namespace EllipticBit.Services.Address
{
	using System.Text.Json.Serialization;

	internal class UspsAddressResponse
	{
		[JsonPropertyName("firm")]
		public string BusinessName { get; set; } = null;

		[JsonPropertyName("address")]
		public UspsAddress Address { get; set; } = null;
	}

	internal class UspsAddress
	{
		[JsonPropertyName("streetAddress")]
		public string Address1 { get; set; } = null;

		[JsonPropertyName("secondaryAddress")]
		public string Address2 { get; set; } = null;

		[JsonPropertyName("city")]
		public string City { get; set; } = null;

		[JsonPropertyName("state")]
		public string Region { get; set; } = null;

		[JsonPropertyName("urbanization")]
		public string SubRegion { get; set; } = null;

		[JsonPropertyName("ZIPCode")]
		public string PostalCode { get; set; } = null;

		[JsonPropertyName("ZIPPlus4")]
		public string PostalCodeSuffix { get; set; } = null;

		public UspsAddress(Address fromAddress) {
			this.Address1 = fromAddress.Address1;
			this.Address2 = fromAddress.Address2;
			this.City = fromAddress.City;
			this.Region = fromAddress.Region;
			this.SubRegion = fromAddress.SubRegion;
			this.PostalCode = fromAddress.PostalCode;
			this.PostalCodeSuffix = fromAddress.PostalCodeSuffix;
		}

		public Address ToAddress() {
			return new Address {
				Address1 = this.Address1,
				Address2 = this.Address2,
				City = this.City,
				Region = this.Region,
				SubRegion = this.SubRegion,
				PostalCode = this.PostalCode,
				PostalCodeSuffix = this.PostalCodeSuffix
			};
		}
	}

	internal class UspsCityRegion
	{
		[JsonPropertyName("city")]
		public string City { get; set; } = null;

		[JsonPropertyName("state")]
		public string Region { get; set; } = null;

		[JsonPropertyName("ZIPCode")]
		public string PostalCode { get; set; } = null;

		public UspsCityRegion(CityRegion fromCityRegion)
		{
			this.City = fromCityRegion.City;
			this.Region = fromCityRegion.Region;
			this.PostalCode = fromCityRegion.PostalCode;
		}

		public CityRegion ToCityRegion()
		{
			return new CityRegion
			{
				City = this.City,
				Region = this.Region,
				PostalCode = this.PostalCode,
			};
		}
	}
}
