//-----------------------------------------------------------------------------
// Copyright (c) 2020-2025 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

namespace EllipticBit.Services.Address
{
	/// <summary>
	/// Represents a city-region pairing.
	/// </summary>
	public sealed class CityRegion
	{
		/// <summary>
		/// The city in which the address is located
		/// </summary>
		public string City { get; set; } = null;

		/// <summary>
		/// The region or state.
		/// </summary>
		public string Region { get; set; } = null;

		/// <summary>
		/// The Postal Code of the address.
		/// </summary>
		public string PostalCode { get; set; } = null;
	}
}
