//----------------------------------------------------------------
// Copyright (c) 2024 EllipticBit, LLC - All Rights Reserved.
//----------------------------------------------------------------

using System;
using System.Security.Cryptography;
using Vanara.PInvoke;

// ReSharper disable once CheckNamespace
namespace EllipticBit.Services.Cryptography
{
	internal class Rfc2898DeriveBytes
	{
		public static byte[] Pbkdf2(byte[] password, byte[] salt, int iterations, HashAlgorithmName func, int outputLen) {
			var result = new byte[outputLen];
			unsafe
			{
				fixed (byte* pResult = result)
				fixed (byte* pPassword = password)
				fixed (byte* pSalt = salt)
				{
					var tr = new IntPtr(pResult);
					BCrypt.BCryptOpenAlgorithmProvider(out BCrypt.SafeBCRYPT_ALG_HANDLE pAlgorithm, func.Name, BCrypt.KnownProvider.MS_PRIMITIVE_PROVIDER, 0);
					try
					{
						BCrypt.BCryptDeriveKeyPBKDF2(pAlgorithm, new IntPtr(pPassword), (uint)password.Length, new IntPtr(pSalt), (uint)salt.Length, (ulong)iterations, new IntPtr(pResult), (uint)outputLen, 0);
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
