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
		Task<Address> Validate(Address address, string tenantId = null);
	}
}
