//-----------------------------------------------------------------------------
// Copyright (c) 2020-2025 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using System.Threading.Tasks;

namespace EllipticBit.Services.Address
{
	/// <summary>
	/// Address validation service.
	/// </summary>
	public interface IAddressService
	{
		/// <summary>
		/// Provides the closest match to the provided address.
		/// </summary>
		/// <param name="address">The Address to validate. Can be a partial address.</param>
		/// <param name="tenantId">An optional parameter to pass a tenant ID to the underlying authentication system to enable use in multi-tenant systems.</param>
		/// <returns>The best matching Address for the provided Address.</returns>
		Task<Address> GetAddress(Address address, string tenantId = null);

		/// <summary>
		/// Get the city/region pairing that corresponds to specified ZIP code.
		/// </summary>
		/// <param name="postalCode">The postal code to look up.</param>
		/// <param name="tenantId">An optional parameter to pass a tenant ID to the underlying authentication system to enable use in multi-tenant systems.</param>
		/// <returns>A city/region pairing.</returns>
		Task<CityRegion> GetCityRegion(string postalCode, string tenantId = null);

		/// <summary>
		/// Gets the Postal Code of the specified address.
		/// </summary>
		/// <param name="streetAddress">The primary street address.</param>
		/// <param name="city">The city of the postal code.</param>
		/// <param name="region">The region or state of the postal code.</param>
		/// <param name="includeSuffix">Whether to include the Postal Code Suffix in the result. Defaults to 'true'.</param>
		/// <param name="tenantId">An optional parameter to pass a tenant ID to the underlying authentication system to enable use in multi-tenant systems.</param>
		/// <returns>The postal code, including the suffix is desired. Note that the suffix will not be included if one is not found.</returns>
		Task<string> GetPostalCode(string streetAddress, string city, string region, bool includeSuffix = true, string tenantId = null);
	}
}
