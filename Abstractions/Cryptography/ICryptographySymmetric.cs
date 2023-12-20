//-----------------------------------------------------------------------------
// Copyright (c) 2020-2023 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using System;

namespace EllipticBit.Services.Cryptography
{
	/// <summary>
	/// Contains methods for performing symmetric data encryption.
	/// </summary>
	public interface ICryptographySymmetric
	{
		/// <summary>
		/// Encrypts the provided data using the specified encryption parameters.
		/// </summary>
		/// <param name="data">The plaintext to encrypt.</param>
		/// <param name="associatedData">Any associated data to process during encryption.</param>
		/// <param name="key">The encryption key to use. The length of the key must match the length required by the chosen cipher.</param>
		/// <param name="iv">The IV/Nonce using to initialize the cipher.</param>
		/// <param name="authTag">The resulting Authentication Code.</param>
		/// <param name="algorithm">The symmetric encryption algorithm to use.</param>
		/// <param name="hash"></param>
		/// <returns>The encrypted ciphertext as a byte array.</returns>
		byte[] Encrypt(ReadOnlySpan<byte> data, ReadOnlySpan<byte> associatedData, ReadOnlySpan<byte> key, ReadOnlySpan<byte> iv, out byte[] authTag, EncryptionAlgorithm algorithm, HashAlgorithm hash);

		/// <summary>
		/// Decrypts the provided data using the specified decryption parameters.
		/// </summary>
		/// <param name="data">The plaintext to encrypt.</param>
		/// <param name="associatedData">Any associated data to process during encryption.</param>
		/// <param name="key">The encryption key to use. The length of the key must match the length required by the chosen cipher.</param>
		/// <param name="iv">The IV/Nonce using to initialize the cipher.</param>
		/// <param name="authTag">The resulting Authentication Code.</param>
		/// <param name="algorithm">The symmetric encryption algorithm to use.</param>
		/// <param name="hash"></param>
		/// <returns>The decrypted plaintext as a byte array.</returns>
		byte[] Decrypt(ReadOnlySpan<byte> data, ReadOnlySpan<byte> associatedData, ReadOnlySpan<byte> key, ReadOnlySpan<byte> iv, ReadOnlySpan<byte> authTag, EncryptionAlgorithm algorithm, HashAlgorithm hash);
	}

	/// <summary>
	/// Extensions methods for ICryptographySymmetric.
	/// </summary>
	public static class CryptographySymmetricExtensions
	{
		/// <summary>
		/// Encrypts the provided data using the specified encryption parameters.
		/// </summary>
		/// <param name="symmetric">The ICryptographySymmetric interface.</param>
		/// <param name="data">The plaintext to encrypt.</param>
		/// <param name="associatedData">Any associated data to process during encryption.</param>
		/// <param name="key">The encryption key to use. The length of the key must match the length required by the chosen cipher.</param>
		/// <param name="iv">The IV/Nonce using to initialize the cipher.</param>
		/// <param name="algorithm">The symmetric encryption algorithm to use.</param>
		/// <param name="hash"></param>
		/// <returns>The encrypted ciphertext as a byte array.</returns>
		public static EncryptedData Encrypt(this ICryptographySymmetric symmetric, ReadOnlySpan<byte> data, ReadOnlySpan<byte> associatedData, ReadOnlySpan<byte> key, ReadOnlySpan<byte> iv, EncryptionAlgorithm algorithm, HashAlgorithm hash) {
			byte[] cipherText = symmetric.Encrypt(data, associatedData, key, iv, out var authTag, algorithm, hash);
			return new EncryptedData(cipherText, iv, authTag, algorithm, hash);
		}

		/// <summary>
		/// Decrypts the provided data using the specified decryption parameters.
		/// </summary>
		/// <param name="symmetric">The ICryptographySymmetric interface.</param>
		/// <param name="data">The plaintext to encrypt.</param>
		/// <param name="associatedData">Any associated data to process during encryption.</param>
		/// <param name="key">The encryption key to use. The length of the key must match the length required by the chosen cipher.</param>
		/// <returns>The decrypted plaintext as a byte array.</returns>
		public static byte[] Decrypt(this ICryptographySymmetric symmetric, EncryptedData data, ReadOnlySpan<byte> associatedData, ReadOnlySpan<byte> key) {
			return symmetric.Decrypt(data.CipherText, associatedData, key, data.IV, data.AuthTag, data.EncryptionAlgorithm, data.HashAlgorithm);
		}
	}
}
