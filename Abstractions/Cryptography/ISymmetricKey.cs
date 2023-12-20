//-----------------------------------------------------------------------------
// Copyright (c) 2020-2023 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

namespace EllipticBit.Services.Cryptography
{
	/// <summary>
	/// Opaque interface that contains a cryptographically secure key.
	/// </summary>
	public interface ISymmetricKey
	{
		/// <summary>
		/// Retrieve the key as a byte array.
		/// </summary>
		public byte[] Key { get; }
	}
}
