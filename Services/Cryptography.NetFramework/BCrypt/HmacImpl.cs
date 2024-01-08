//----------------------------------------------------------------
// Copyright (c) 2024 EllipticBit, LLC - All Rights Reserved.
//----------------------------------------------------------------

using System;
using System.IO;
using System.Threading.Tasks;

using Vanara.PInvoke;

// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

// ReSharper disable once CheckNamespace
namespace EllipticBit.Services.Cryptography
{
	internal static class HMACSHA256
	{
		public static ValueTask<byte[]> HashDataAsync(byte[] key, Stream data) {
			using var hmac = new System.Security.Cryptography.HMACSHA256(key);
			return new ValueTask<byte[]>(hmac.ComputeHash(data));
		}
	}

	internal static class HMACSHA384
	{
		public static ValueTask<byte[]> HashDataAsync(byte[] key, Stream data)
		{
			using var hmac = new System.Security.Cryptography.HMACSHA384(key);
			return new ValueTask<byte[]>(hmac.ComputeHash(data));
		}
	}

	internal static class HMACSHA512
	{
		public static ValueTask<byte[]> HashDataAsync(byte[] key, Stream data)
		{
			using var hmac = new System.Security.Cryptography.HMACSHA512(key);
			return new ValueTask<byte[]>(hmac.ComputeHash(data));
		}
	}

	internal static class HMACSHA3_256
	{
		public static async ValueTask<byte[]> HashDataAsync(byte[] key, Stream data)
		{
			using var ms = new MemoryStream();
			await data.CopyToAsync(ms);
			return HmacImpl.Hmac(key, ms.ToArray(), EllipticBit.Services.Cryptography.HashAlgorithm.SHA3_256);
		}
	}

	internal static class HMACSHA3_384
	{
		public static async ValueTask<byte[]> HashDataAsync(byte[] key, Stream data)
		{
			using var ms = new MemoryStream();
			await data.CopyToAsync(ms);
			return HmacImpl.Hmac(key, ms.ToArray(), EllipticBit.Services.Cryptography.HashAlgorithm.SHA3_384);
		}
	}

	internal static class HMACSHA3_512
	{
		public static async ValueTask<byte[]> HashDataAsync(byte[] key, Stream data)
		{
			using var ms = new MemoryStream();
			await data.CopyToAsync(ms);
			return HmacImpl.Hmac(key, ms.ToArray(), EllipticBit.Services.Cryptography.HashAlgorithm.SHA3_512);
		}
	}

	internal static class HmacImpl
	{
		public static byte[] Hmac(byte[] key, byte[] data, EllipticBit.Services.Cryptography.HashAlgorithm func)
		{
			var result = new byte[func.GetHashLength()];
			unsafe
			{
				fixed (byte* pKey = key)
				fixed (byte* pResult = result)
				fixed (byte* pData = data)
				{
					var tr = new IntPtr(pResult);
					BCrypt.BCryptOpenAlgorithmProvider(out BCrypt.SafeBCRYPT_ALG_HANDLE pAlgorithm, func.ToAlgId(), BCrypt.KnownProvider.MS_PRIMITIVE_PROVIDER, 0);
					try
					{
						BCrypt.BCryptHash(pAlgorithm, new IntPtr(pKey), (uint)key.Length, new IntPtr(pData), (uint)data.Length, tr, (uint)result.Length);
					}
					finally
					{
						BCrypt.BCryptCloseAlgorithmProvider(pAlgorithm);
					}
				}
			}

			return result;
		}
	}
}
