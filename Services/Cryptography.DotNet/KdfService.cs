//----------------------------------------------------------------
// Copyright (c) 2023 EllipticBit, LLC - All Rights Reserved.
//----------------------------------------------------------------

using Konscious.Security.Cryptography;

using System;
using System.Text;
#if !NETFRAMEWORK
using System.Security.Cryptography;
#endif

// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

namespace EllipticBit.Services.Cryptography
{
	internal class KdfService : ICryptographyKdf
	{
		public byte[] PBKDF2(string password, byte[] salt, int outputLen, int iterations, HashAlgorithm func) {
			switch (func) {
				case HashAlgorithm.SHA2_256: return Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(password), salt, iterations, System.Security.Cryptography.HashAlgorithmName.SHA256, outputLen);
				case HashAlgorithm.SHA2_384: return Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(password), salt, iterations, System.Security.Cryptography.HashAlgorithmName.SHA384, outputLen);
				case HashAlgorithm.SHA2_512: return Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(password), salt, iterations, System.Security.Cryptography.HashAlgorithmName.SHA512, outputLen);
				case HashAlgorithm.SHA3_256: return Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(password), salt, iterations, new System.Security.Cryptography.HashAlgorithmName("SHA3-256"), outputLen);
				case HashAlgorithm.SHA3_384: return Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(password), salt, iterations, new System.Security.Cryptography.HashAlgorithmName("SHA3-384"), outputLen);
				case HashAlgorithm.SHA3_512: return Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(password), salt, iterations, new System.Security.Cryptography.HashAlgorithmName("SHA3-512"), outputLen);
				default: throw new NotSupportedException($"Hash Function '{func}' is not supported.");
			}
		}

		public byte[] HKDF(byte[] key, byte[] salt, byte[] info, int outputLen, HashAlgorithm func) {
			if (key == null) throw new ArgumentNullException(nameof(key));
			switch (func) {
				case HashAlgorithm.SHA2_256: return System.Security.Cryptography.HKDF.DeriveKey(System.Security.Cryptography.HashAlgorithmName.SHA256, key, outputLen, salt, info);
				case HashAlgorithm.SHA2_384: return System.Security.Cryptography.HKDF.DeriveKey(System.Security.Cryptography.HashAlgorithmName.SHA384, key, outputLen, salt, info);
				case HashAlgorithm.SHA2_512: return System.Security.Cryptography.HKDF.DeriveKey(System.Security.Cryptography.HashAlgorithmName.SHA512, key, outputLen, salt, info);
				case HashAlgorithm.SHA3_256: return System.Security.Cryptography.HKDF.DeriveKey(new System.Security.Cryptography.HashAlgorithmName("SHA3-256"), key, outputLen, salt, info);
				case HashAlgorithm.SHA3_384: return System.Security.Cryptography.HKDF.DeriveKey(new System.Security.Cryptography.HashAlgorithmName("SHA3-384"), key, outputLen, salt, info);
				case HashAlgorithm.SHA3_512: return System.Security.Cryptography.HKDF.DeriveKey(new System.Security.Cryptography.HashAlgorithmName("SHA3-512"), key, outputLen, salt, info);
				default:
					throw new NotSupportedException($"Hash Function '{func}' is not supported.");
			}
		}

#if NETFRAMEWORK
		public byte[] SCrypt(string password, byte[] salt, int outputLen, int n, int r, int p) {
			return CryptSharp.Utility.SCrypt.ComputeDerivedKey(Encoding.UTF8.GetBytes(password), salt, n, r, p, 1, outputLen);
		}
#else
		public byte[] SCrypt(string password, byte[] salt, int outputLen, int n, int r, int p) {
			return Norgerman.Cryptography.Scrypt.ScryptUtil.Scrypt(password, salt, n, r, p, outputLen);
		}
#endif

		public byte[] Argon2(string password, byte[] salt, byte[] pepper, byte[] info, int outputLen, int iterations, int threads, int memcost) {
			var argon = new Argon2id(System.Text.Encoding.UTF8.GetBytes(password))
			{
				Salt = salt,
				KnownSecret = pepper,
				AssociatedData = info,
				Iterations = iterations,
				DegreeOfParallelism = threads,
				MemorySize = memcost,
			};
			return argon.GetBytes(outputLen);
		}
	}

	internal static class PriorKdfImplementationExtensions
	{
		//For all versions of the library prior to 1.0.5.
		public static byte[] Argon2_V0(this ICryptographyKdf service, string password, byte[] salt, byte[] pepper, byte[] info, int outputLen) {
			return service.Argon2(password, salt, pepper, info, outputLen, 3, 2, 8192);
		}

		//For all versions of the library prior to 1.0.5.
		public static byte[] PBKDF2_V0(this ICryptographyKdf service, string password, byte[] salt, int outputLen) {
			return service.PBKDF2(password, salt, outputLen, 100000, HashAlgorithm.SHA2_512);
		}
	}
}
