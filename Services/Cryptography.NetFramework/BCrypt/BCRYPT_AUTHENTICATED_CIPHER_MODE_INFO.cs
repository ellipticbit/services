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
/*
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
*/
