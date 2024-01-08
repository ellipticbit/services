//----------------------------------------------------------------
// Copyright (c) 2024 EllipticBit, LLC - All Rights Reserved.
//----------------------------------------------------------------

using System.Runtime.InteropServices;

// ReSharper disable once CheckNamespace
// ReSharper disable once InconsistentNaming
namespace System.Security.Cryptography
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO
	{
		public uint cbSize;
		public uint dwInfoVersion;
		public IntPtr pbNonce;
		public uint cbNonce;
		public IntPtr pbAuthData;
		public uint cbAuthData;
		public IntPtr pbTag;
		public uint cbTag;
		public IntPtr pbMacContext;
		public uint cbMacContext;
		public uint cbAAD;
		public ulong cbData;
		public uint dwFlags;
	}
}
