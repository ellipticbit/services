//----------------------------------------------------------------
// Copyright (c) 2024 EllipticBit, LLC - All Rights Reserved.
//----------------------------------------------------------------

using System.Security.Cryptography;

// ReSharper disable once CheckNamespace
namespace EllipticBit.Services.Cryptography
{
	internal static class RandomNumberGenerator
	{
		public static byte[] GetBytes( int bytes)
		{
			var ret = new byte[bytes];
			using var tmp = new RNGCryptoServiceProvider();
			tmp.GetBytes(ret);
			return ret;
		}
	}
}
