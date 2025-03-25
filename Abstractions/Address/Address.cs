//-----------------------------------------------------------------------------
// Copyright (c) 2020-2025 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

namespace EllipticBit.Services.Address
{
	/// <summary>
	/// A data structure containing all necessary address information.
	/// </summary>
	public sealed class Address
	{

		/// <summary>
		/// The name of the business, if any, at this address.
		/// </summary>
		public string BusinessName { get; set; } = null;

		/// <summary>
		/// The primary street address.
		/// </summary>
		public string Address1 { get; set; } = null;

		/// <summary>
		/// The secondary address.
		/// </summary>
		public string Address2 { get; set; } = null;

		/// <summary>
		/// The city in which the address is located
		/// </summary>
		public string City { get; set; } = null;

		/// <summary>
		/// The sub-region/urbanization.
		/// </summary>
		public string SubRegion { get; set; } = null;

		/// <summary>
		/// The region or state.
		/// </summary>
		public string Region { get; set; } = null;

		/// <summary>
		/// The Postal Code of the address.
		/// </summary>
		public string PostalCode { get; set; } = null;

		/// <summary>
		/// The Postal Code suffix.
		/// </summary>
		public string PostalCodeSuffix { get; set; } = null;
	}
}
