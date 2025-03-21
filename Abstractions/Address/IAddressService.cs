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
		/// <returns>The best match for the provided Address.</returns>
		Task<Address> Validate(Address address);
	}
}
