//----------------------------------------------------------------
// Copyright (c) 2024 EllipticBit, LLC - All Rights Reserved.
//----------------------------------------------------------------

// ReSharper disable InconsistentNaming
// ReSharper disable once CheckNamespace
namespace System.Security.Cryptography
{
	internal unsafe struct BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO
	{
		public int cbSize;
		public int dwInfoVersion;
		public byte* pbNonce;
		public int cbNonce;
		public byte* pbAuthData;
		public int cbAuthData;
		public byte* pbTag;
		public int cbTag;
		public byte* pbMacContext;
		public int cbMacContext;
		public int cbAAD;
		public long cbData;
		public int dwFlags;
	}
}
