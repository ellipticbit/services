//----------------------------------------------------------------
// Copyright (c) 2024 EllipticBit, LLC - All Rights Reserved.
//----------------------------------------------------------------

using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

using Vanara.PInvoke;

// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

// ReSharper disable once CheckNamespace
namespace EllipticBit.Services.Cryptography
{
	internal static class SHA256
	{
		public static ValueTask<byte[]> HashDataAsync(Stream data) {
			using var hash = new SHA256Cng();
			return new ValueTask<byte[]>(hash.ComputeHash(data));
		}
	}

	internal static class SHA384
	{
		public static ValueTask<byte[]> HashDataAsync(Stream data)
		{
			using var hash = new SHA384Cng();
			return new ValueTask<byte[]>(hash.ComputeHash(data));
		}
	}

	internal static class SHA512
	{
		public static ValueTask<byte[]> HashDataAsync(Stream data)
		{
			using var hash = new SHA512Cng();
			return new ValueTask<byte[]>(hash.ComputeHash(data));
		}
	}

	internal static class SHA3_256
	{
		public static async ValueTask<byte[]> HashDataAsync(Stream data)
		{
			using var ms = new MemoryStream();
			await data.CopyToAsync(ms);
			return HashImpl.Hash(ms.ToArray(), HashAlgorithm.SHA3_256);
		}
	}

	internal static class SHA3_384
	{
		public static async ValueTask<byte[]> HashDataAsync(Stream data)
		{
			using var ms = new MemoryStream();
			await data.CopyToAsync(ms);
			return HashImpl.Hash(ms.ToArray(), HashAlgorithm.SHA3_384);
		}
	}

	internal static class SHA3_512
	{
		public static async ValueTask<byte[]> HashDataAsync(Stream data)
		{
			using var ms = new MemoryStream();
			await data.CopyToAsync(ms);
			return HashImpl.Hash(ms.ToArray(), HashAlgorithm.SHA3_512);
		}
	}

	internal static class HashImpl
	{
		public static byte[] Hash(byte[] data, HashAlgorithm func) {
			var result = new byte[func.GetHashLength()];
			unsafe {
				fixed (byte* pResult = result)
				fixed (byte* pData = data) {
					var tr = new IntPtr(pResult);
					var r = BCrypt.BCryptOpenAlgorithmProvider(out BCrypt.SafeBCRYPT_ALG_HANDLE pAlgorithm, func.ToAlgId(), BCrypt.KnownProvider.MS_PRIMITIVE_PROVIDER, 0);
					if (r!= NTStatus.STATUS_SUCCESS) throw new CryptographicException($"{func.ToAlgId()} is not supported on this Operating System.");

					try
					{
						BCrypt.BCryptHash(pAlgorithm, default(IntPtr), 0, new IntPtr(pData), (uint)data.Length, tr, (uint)result.Length);
					}
					finally {
						BCrypt.BCryptCloseAlgorithmProvider(pAlgorithm);
					}
				}
			}

			return result;
		}

		public static string ToAlgId(this EllipticBit.Services.Cryptography.HashAlgorithm func) {
			switch (func) {
				case HashAlgorithm.SHA2_256:
					return "SHA256";
				case HashAlgorithm.SHA2_384:
					return "SHA384";
				case HashAlgorithm.SHA2_512:
					return "SHA512";
				case HashAlgorithm.SHA3_256:
					return "SHA3-256";
				case HashAlgorithm.SHA3_384:
					return "SHA3-384";
				case HashAlgorithm.SHA3_512:
					return "SHA3-512";
			}
			return String.Empty;
		}
	}
}
