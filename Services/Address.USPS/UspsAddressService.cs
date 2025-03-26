//-----------------------------------------------------------------------------
// Copyright (c) 2025 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

namespace EllipticBit.Services.Address {
	using EllipticBit.Coalescence.Shared.Request;

	using System.Threading.Tasks;

	public sealed class UspsAddressService : IAddressService {
		private readonly ICoalescenceRequestFactory requests;

		public UspsAddressService(ICoalescenceRequestFactory requests) {
			this.requests = requests;
		}

		public async Task<Address> GetAddress(Address address, string tenantId = null) {
			var request = requests.CreateRequest("USPS", tenantId).Get().Path("addresses", "v3", "address").Authentication("USPS")
				.Query("streetAddress", address.Address1).Query("state", address.Region);

			if (!string.IsNullOrWhiteSpace(address.Address2)) request.Query("secondaryAddress", address.Address2);
			if (!string.IsNullOrWhiteSpace(address.City)) request.Query("city", address.City);
			if (!string.IsNullOrWhiteSpace(address.SubRegion)) request.Query("urbanization", address.SubRegion);
			if (!string.IsNullOrWhiteSpace(address.PostalCode)) request.Query("ZIPCode", address.PostalCode);
			if (!string.IsNullOrWhiteSpace(address.PostalCodeSuffix)) request.Query("ZIPPlus4", address.PostalCodeSuffix);

			var response = await request.Send();
			var addr = await response.ThrowOnFailureResponse().AsDeserialized<UspsAddressResponse>();
			return addr.Address?.ToAddress();
		}

		public async Task<CityRegion> GetCityRegion(string postalCode, string tenantId = null) {
			var response = await requests.CreateRequest("USPS", tenantId).Get()
				.Path("addresses", "v3", "address").Query("ZIPCode", postalCode)
				.Authentication("USPS").Send();

			var result = await response.ThrowOnFailureResponse().AsDeserialized<UspsCityRegion>();
			return result.ToCityRegion();
		}

		public async Task<string> GetPostalCode(string streetAddress, string city, string region, bool includeSuffix = true, string tenantId = null) {
			var request = requests.CreateRequest("USPS", tenantId).Get().Path("addresses", "v3", "address").Authentication("USPS")
				.Query("streetAddress", streetAddress).Query("city", city).Query("state", region);

			var response = await request.Send();
			var addr = await response.ThrowOnFailureResponse().AsDeserialized<UspsAddressResponse>();

			return includeSuffix ? $"{addr.Address.PostalCode}-{addr.Address.PostalCodeSuffix}" : addr.Address?.PostalCode;
		}
	}
}
