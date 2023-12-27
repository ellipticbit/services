//-----------------------------------------------------------------------------
// Copyright (c) 2020-2023 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using System;
using System.Linq;
using System.Text;

namespace EllipticBit.Services.Cryptography
{
	/// <summary>
	/// Contains methods for performing key derivations.
	/// </summary>
	public interface ICryptographyKdf
	{
		/// <summary>
		/// Creates a cryptographic secure encryption key from the provided password using PBKDF2.
		/// </summary>
		/// <param name="password">The password from which to generate the key.</param>
		/// <param name="salt">The salt used by the KDF.</param>
		/// <param name="outputLen">The required numbers of key bytes to generate.</param>
		/// <param name="iterations">The number of iterations to run the key derivation. A minimum of 1,000,000 iterations is recommended.</param>
		/// <param name="func">The hashing function to use.</param>
		/// <returns>A byte array containing the encryption key.</returns>
		byte[] PBKDF2(string password, byte[] salt, int outputLen, int iterations, HashAlgorithm func);

		/// <summary>
		/// Creates a cryptographic secure encryption key from the provided password using HKDF.
		/// </summary>
		/// <param name="key">The key bytes from which to generate the key.</param>
		/// <param name="salt">The salt used by the KDF.</param>
		/// <param name="info">Optional info to use during the key derivation.</param>
		/// <param name="outputLen">The required numbers of key bytes to generate.</param>
		/// <param name="func">The hashing function to use.</param>
		/// <returns>A byte array containing the encryption key.</returns>
		byte[] HKDF(byte[] key, byte[] salt, byte[] info, int outputLen, HashAlgorithm func);


		/// <summary>
		/// Creates a cryptographic secure encryption key from the provided password using SCrypt.
		/// </summary>
		/// <param name="password">The password from which to generate the key.</param>
		/// <param name="salt">The salt used by the KDF.</param>
		/// <param name="outputLen">The required numbers of key bytes to generate.</param>
		/// <param name="n">The number of iterations of SCrypt to run. A minimum of 1,000,000 iterations is recommended.</param>
		/// <param name="r">The SCrypt R value. A minimum of 8 is recommended. </param>
		/// <param name="p">The SCrypt P value. The recommended value is 1.</param>
		/// <returns>A byte array containing the encryption key.</returns>
		byte[] SCrypt(string password, byte[] salt, int outputLen, int n, int r, int p);

		/// <summary>
		/// Creates a cryptographic secure encryption key from the provided password using Argon2 with the default parameters.
		/// </summary>
		/// <param name="password">The password from which to generate the key.</param>
		/// <param name="salt">The salt used by the KDF.</param>
		/// <param name="pepper">A well-known key value to use during derivation.</param>
		/// <param name="info">Additional information to include in derivation.</param>
		/// <param name="outputLen">The required numbers of key bytes to generate.</param>
		/// <param name="iterations">The number of iterations to perform. The recommended minimum is 2.</param>
		/// <param name="threads">The number of threads to use. The recommended minimum is 8.</param>
		/// <param name="memcost">The memory cost. The recommended minimum is 1048576 bytes.</param>
		/// <returns>A byte array containing the encryption key.</returns>
		byte[] Argon2(string password, byte[] salt, byte[] pepper, byte[] info, int outputLen, int iterations, int threads, int memcost);
	}

	/// <summary>
	/// Extensions for ICryptographyKdf.
	/// </summary>
	public static class CryptographyKdfExtensions {
		/// <summary>
		/// Creates a cryptographic secure encryption key from the provided password using PBKDF2 with the default parameters.
		/// </summary>
		/// <param name="kdf">The IKdfService interface.</param>
		/// <param name="password">The password from which to generate the key.</param>
		/// <param name="salt">The salt used by the KDF.</param>
		/// <param name="pepper">The pepper value used by the KDF.</param>
		/// <param name="outputLen">The required numbers of key bytes to generate.</param>
		/// <returns>A byte array containing the encryption key.</returns>
		public static byte[] PBKDF2(this ICryptographyKdf kdf, string password, byte[] salt, byte[] pepper, int outputLen) {
			if (salt == null || salt.Length == 0) throw new ArgumentNullException(nameof(salt));
			if (pepper == null || pepper.Length == 0) throw new ArgumentNullException(nameof(pepper));
			var combined = salt.Concat(pepper);
			return kdf.PBKDF2(password, combined.ToArray(), outputLen, 1048576, HashAlgorithm.Default);
		}

		/// <summary>
		/// Creates a cryptographic secure encryption key from the provided password using HKDF with the default parameters.
		/// </summary>
		/// <param name="kdf">The IKdfService interface.</param>
		/// <param name="key">The key bytes from which to generate the key.</param>
		/// <param name="salt">The salt used by the KDF.</param>
		/// <param name="info">Optional info to use during the key derivation.</param>
		/// <param name="outputLen">The required numbers of key bytes to generate.</param>
		/// <returns>A byte array containing the encryption key.</returns>
		public static byte[] HKDF(this ICryptographyKdf kdf, byte[] key, byte[] salt, string info, int outputLen) {
			return kdf.HKDF(key, salt, Encoding.UTF8.GetBytes(info), outputLen, HashAlgorithm.Default);
		}

		/// <summary>
		/// Creates a cryptographic secure encryption key from the provided password using SCrypt with the default parameters.
		/// </summary>
		/// <param name="kdf">The IKdfService interface.</param>
		/// <param name="password">The password from which to generate the key.</param>
		/// <param name="salt">The salt used by the KDF.</param>
		/// <param name="pepper">The pepper value used by the KDF.</param>
		/// <param name="outputLen">The required numbers of key bytes to generate.</param>
		/// <returns>A byte array containing the encryption key.</returns>
		public static byte[] SCrypt(this ICryptographyKdf kdf, string password, byte[] salt, byte[] pepper, int outputLen) {
			if (salt == null || salt.Length == 0) throw new ArgumentNullException(nameof(salt));
			if (pepper == null || pepper.Length == 0) throw new ArgumentNullException(nameof(pepper));
			var combined = salt.Concat(pepper);
			return kdf.SCrypt(password, combined.ToArray(), outputLen, 1048576, 8, 1);
		}

		/// <summary>
		/// Creates a cryptographic secure encryption key from the provided password using Argon2 with the default parameters.
		/// </summary>
		/// <param name="kdf">The IKdfService interface.</param>
		/// <param name="password">The password from which to generate the key.</param>
		/// <param name="salt">The salt used by the KDF.</param>
		/// <param name="pepper">The pepper value used by the KDF.</param>
		/// <param name="info">Optional info to use during the key derivation.</param>
		/// <param name="outputLen">The required numbers of key bytes to generate.</param>
		/// <returns>A byte array containing the encryption key.</returns>
		public static byte[] Argon2(this ICryptographyKdf kdf, string password, byte[] salt, byte[] pepper, byte[] info, int outputLen) {
			if (salt == null || salt.Length == 0) throw new ArgumentNullException(nameof(salt));
			if (pepper == null || pepper.Length == 0) throw new ArgumentNullException(nameof(pepper));
			return kdf.Argon2(password, salt, pepper, info, outputLen, 2, 8, 1048576);
		}
	}
}
