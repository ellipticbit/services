//-----------------------------------------------------------------------------
// Copyright (c) 2020-2023 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;

namespace EllipticBit.Services.Address
{
	public interface IAddressService
	{
		/// <summary>
		/// Provides a list of possible matches to the provided address.
		/// </summary>
		/// <param name="address">The Address to validate. May be a partial address.</param>
		/// <returns>A list of possible addresses matching the provided Address.</returns>
		Task<List<Address>> Validate(Address address);
	}
}
