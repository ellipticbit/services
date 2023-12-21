//-----------------------------------------------------------------------------
// Copyright (c) 2020-2023 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using System.Security.Cryptography;
// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo

namespace EllipticBit.Services.Cryptography
{
	/// <summary>
	/// The available symmetric cryptography algorithms.
	/// </summary>
	public enum EncryptionAlgorithm
	{
		/// <summary>
		/// Use AES-128 in CBC Mode.
		/// </summary>
		AES128CBC = 1,
		/// <summary>
		/// Use AES-192 in CBC Mode.
		/// </summary>
		AES192CBC = 2,
		/// <summary>
		/// Use AES-256 in CBC Mode.
		/// </summary>
		AES256CBC = 3,

		/// <summary>
		/// Use AES-128 in CFB Mode.
		/// </summary>
		AES128CFB = 4,
		/// <summary>
		/// Use AES-192 in CFB Mode.
		/// </summary>
		AES192CFB = 5,
		/// <summary>
		/// Use AES-256 in CFB Mode.
		/// </summary>
		AES256CFB = 6,

		/// <summary>
		/// Use AES-128 in GCM Mode.
		/// </summary>
		AES128GCM = 7,
		/// <summary>
		/// Use AES-192 in GCM Mode.
		/// </summary>
		AES192GCM = 8,
		/// <summary>
		/// Use AES-256 in GCM Mode.
		/// </summary>
		AES256GCM = 9,

		/// <summary>
		/// Use ChaCha20 with the Poly1305 authentication mode.
		/// </summary>
		ChaCha20Poly1305 = 10,

		/// <summary>
		/// The Default cryptography function is AES-256-GCM.
		/// </summary>
		Default = AES256GCM,
	}

	/// <summary>
	/// The available hash functions.
	/// </summary>
	public enum HashAlgorithm
	{
		/// <summary>
		/// Do not use a hash function.
		/// </summary>
		None = 0,

		/// <summary>
		/// Use the SHA2-256 hash function.
		/// </summary>
		SHA2_256 = 1,
		/// <summary>
		/// Use the SHA2-384 hash function.
		/// </summary>
		SHA2_384 = 2,
		/// <summary>
		/// Use the SHA2-512 hash function.
		/// </summary>
		SHA2_512 = 3,

		/// <summary>
		/// Use the SHA3-256 hash function.
		/// </summary>
		SHA3_256 = 4,
		/// <summary>
		/// Use the SHA3-384 hash function.
		/// </summary>
		SHA3_384 = 5,
		/// <summary>
		/// Use the SHA3-512 hash function.
		/// </summary>
		SHA3_512 = 6,

		/// <summary>
		/// The Default Hash algorithm is SHA2-384.
		/// </summary>
		Default = SHA2_384,
	}

	/// <summary>
	/// The available key derivation algorithms.
	/// </summary>
	public enum KeyDerivationAlgorithm
	{
		/// <summary>
		/// Do not use a KDF.
		/// </summary>
		None = 0,
		/// <summary>
		/// Use the PBKDF2 algorithm.
		/// </summary>
		Pbkdf2 = 1,
		/// <summary>
		/// Use the HKDF algorithm.
		/// </summary>
		Hkdf = 2,
		/// <summary>
		/// Use the SCrypt algorithm.
		/// </summary>
		SCrypt = 3,
		/// <summary>
		/// Use the Argon2 algorithm.
		/// </summary>
		Argon2 = 4,

		/// <summary>
		/// The default KDF algorithm is Argon2.
		/// </summary>
		Default = Argon2,
	}

	/// <summary>
	/// Extension methods for Algorithms types.
	/// </summary>
	public static class AlgorithmsExtensions
	{
		/// <summary>
		/// Gets the key length required by the specified algorithm.
		/// </summary>
		/// <param name="algorithm">The encryption algorithm.</param>
		/// <returns>An int with the required key length.</returns>
		public static int GetCipherKeyLength(this EncryptionAlgorithm algorithm) {
			switch (algorithm) {
				case EncryptionAlgorithm.AES128CBC:
				case EncryptionAlgorithm.AES128CFB:
				case EncryptionAlgorithm.AES128GCM:
					return 16;
				case EncryptionAlgorithm.AES192CBC:
				case EncryptionAlgorithm.AES192CFB:
				case EncryptionAlgorithm.AES192GCM:
					return 24;
				case EncryptionAlgorithm.AES256CBC:
				case EncryptionAlgorithm.AES256CFB:
				case EncryptionAlgorithm.AES256GCM:
				case EncryptionAlgorithm.ChaCha20Poly1305:
					return 32;
				default:
					return 0;
			}
		}

		/// <summary>
		/// Gets the IV length required by the specified algorithm.
		/// </summary>
		/// <param name="algorithm">The encryption algorithm.</param>
		/// <returns>An int with the required IV length.</returns>
		public static int GetCipherIVLength(this EncryptionAlgorithm algorithm) {
			switch (algorithm)
			{
				case EncryptionAlgorithm.AES128GCM:
				case EncryptionAlgorithm.AES192GCM:
				case EncryptionAlgorithm.AES256GCM:
				case EncryptionAlgorithm.ChaCha20Poly1305:
					return 12;
				default:
					return 16;
			}
		}

		/// <summary>
		/// Determines if the algorithm is an AEAD algorithm.
		/// </summary>
		/// <param name="algorithm">The encryption algorithm.</param>
		/// <returns>A bool indicating if the specified algorithm is an AEAD algorithm.</returns>
		public static bool IsAeadCipher(this EncryptionAlgorithm algorithm)
		{
			switch (algorithm)
			{
				case EncryptionAlgorithm.AES128GCM:
				case EncryptionAlgorithm.AES192GCM:
				case EncryptionAlgorithm.AES256GCM:
				case EncryptionAlgorithm.ChaCha20Poly1305:
					return true;
				default:
					return false;
			}
		}

		/// <summary>
		/// Gets the expected number of bytes in the algorithms authentication tag.
		/// </summary>
		/// <param name="algorithm">The encryption algorithm.</param>
		/// <param name="hash">Optional. The hash function to use when a non-AEAD cipher is used.</param>
		/// <returns>An int with the number of authentication bytes.</returns>
		public static int GetAuthLength(this EncryptionAlgorithm algorithm, HashAlgorithm hash = HashAlgorithm.None)
		{
			switch (algorithm)
			{
				case EncryptionAlgorithm.AES128GCM:
				case EncryptionAlgorithm.AES192GCM:
				case EncryptionAlgorithm.AES256GCM:
				case EncryptionAlgorithm.ChaCha20Poly1305:
					return 16;
				default:
					return hash.GetHashLength();
			}
		}

		/// <summary>
		/// Gets the number of bytes produced by the specified hash function.
		/// </summary>
		/// <param name="func">The hash function.</param>
		/// <returns>An int with the number of bytes produced by the hash function.</returns>
		/// <exception cref="CryptographicException">The hash function specified is not supported.</exception>
		public static int GetHashLength(this HashAlgorithm func)
		{
			switch (func)
			{
				case HashAlgorithm.None: return 0;
				case HashAlgorithm.SHA2_256: return 32;
				case HashAlgorithm.SHA2_384: return 48;
				case HashAlgorithm.SHA2_512: return 64;
				case HashAlgorithm.SHA3_256: return 32;
				case HashAlgorithm.SHA3_384: return 48;
				case HashAlgorithm.SHA3_512: return 64;
				default:
					throw new CryptographicException($"Hash Function '{func}' is not supported.");
			}
		}
	}
}
