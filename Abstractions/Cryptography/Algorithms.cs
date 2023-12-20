//-----------------------------------------------------------------------------
// Copyright (c) 2020-2023 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using System.Security.Cryptography;

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
		Aes128Cbc = 1,
		/// <summary>
		/// Use AES-192 in CBC Mode.
		/// </summary>
		Aes192Cbc = 2,
		/// <summary>
		/// Use AES-256 in CBC Mode.
		/// </summary>
		Aes256Cbc = 3,

		/// <summary>
		/// Use AES-128 in CFB Mode.
		/// </summary>
		Aes128Cfb = 4,
		/// <summary>
		/// Use AES-192 in CFB Mode.
		/// </summary>
		Aes192Cfb = 5,
		/// <summary>
		/// Use AES-256 in CFB Mode.
		/// </summary>
		Aes256Cfb = 6,

		/// <summary>
		/// Use AES-128 in GCM Mode.
		/// </summary>
		Aes128Gcm = 7,
		/// <summary>
		/// Use AES-192 in GCM Mode.
		/// </summary>
		Aes192Gcm = 8,
		/// <summary>
		/// Use AES-256 in GCM Mode.
		/// </summary>
		Aes256Gcm = 9,

		/// <summary>
		/// Use ChaCha20 with the Poly1305 authentication mode.
		/// </summary>
		ChaCha20Poly1305 = 10,

		/// <summary>
		/// The Default cryptography function is AES-256-GCM.
		/// </summary>
		Default = Aes256Gcm,
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
		Sha2256 = 1,
		/// <summary>
		/// Use the SHA2-384 hash function.
		/// </summary>
		Sha2384 = 2,
		/// <summary>
		/// Use the SHA2-512 hash function.
		/// </summary>
		Sha2512 = 3,

		/// <summary>
		/// Use the SHA3-256 hash function.
		/// </summary>
		Sha3256 = 4,
		/// <summary>
		/// Use the SHA3-384 hash function.
		/// </summary>
		Sha3384 = 5,
		/// <summary>
		/// Use the SHA3-512 hash function.
		/// </summary>
		Sha3512 = 6,

		/// <summary>
		/// The Default Hash algorithm is SHA2-384.
		/// </summary>
		Default = Sha2384,
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
				case EncryptionAlgorithm.Aes128Cbc:
				case EncryptionAlgorithm.Aes128Cfb:
				case EncryptionAlgorithm.Aes128Gcm:
					return 16;
				case EncryptionAlgorithm.Aes192Cbc:
				case EncryptionAlgorithm.Aes192Cfb:
				case EncryptionAlgorithm.Aes192Gcm:
					return 24;
				case EncryptionAlgorithm.Aes256Cbc:
				case EncryptionAlgorithm.Aes256Cfb:
				case EncryptionAlgorithm.Aes256Gcm:
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
				case EncryptionAlgorithm.Aes128Gcm:
				case EncryptionAlgorithm.Aes192Gcm:
				case EncryptionAlgorithm.Aes256Gcm:
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
				case EncryptionAlgorithm.Aes128Gcm:
				case EncryptionAlgorithm.Aes192Gcm:
				case EncryptionAlgorithm.Aes256Gcm:
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
				case EncryptionAlgorithm.Aes128Gcm:
				case EncryptionAlgorithm.Aes192Gcm:
				case EncryptionAlgorithm.Aes256Gcm:
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
				case HashAlgorithm.Sha2256: return 32;
				case HashAlgorithm.Sha2384: return 48;
				case HashAlgorithm.Sha2512: return 64;
				case HashAlgorithm.Sha3256: return 32;
				case HashAlgorithm.Sha3384: return 48;
				case HashAlgorithm.Sha3512: return 64;
				default:
					throw new CryptographicException($"Hash Function '{func}' is not supported.");
			}
		}
	}
}
