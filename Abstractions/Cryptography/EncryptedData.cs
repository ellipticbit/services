//-----------------------------------------------------------------------------
// Copyright (c) 2020-2023 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using System;
using System.IO;

namespace EllipticBit.Services.Cryptography
{
	/// <summary>
	/// Contains the encrypted data and metadata.
	/// </summary>
	public class EncryptedData
	{
		private const int StreamBufferSize = 1048576;

		/// <summary>
		/// A stream of the encrypted data.
		/// </summary>
		public byte[] CipherText { get; }

		/// <summary>
		/// The IV used to initialize the Cipher.
		/// </summary>
		public byte[] IV { get; }

		/// <summary>
		/// The authentication tag generated on the ciphertext.
		/// </summary>
		public byte[] AuthTag { get; }

		/// <summary>
		/// The encryption algorithm to use.
		/// </summary>
		public EncryptionAlgorithm EncryptionAlgorithm { get; }

		/// <summary>
		/// The hash algorithm to use when calculating the HMAC on non-AEAD ciphertexts.
		/// </summary>
		public HashAlgorithm HashAlgorithm { get; }

		internal EncryptedData(ReadOnlySpan<byte> cipherText, ReadOnlySpan<byte> iv, ReadOnlySpan<byte> authTag, EncryptionAlgorithm encryptionAlgorithm, HashAlgorithm hashAlgorithm) {
			CipherText = cipherText.ToArray();
			IV = iv.ToArray();
			AuthTag = authTag.ToArray();
			EncryptionAlgorithm = encryptionAlgorithm;
			HashAlgorithm = hashAlgorithm;
		}

		/// <summary>
		/// Generates a Base64 encoded string containing the ciphertext and the encryption metadata.
		/// </summary>
		/// <returns>The Base64 encoded string.</returns>
		public override string ToString() {
			return Convert.ToBase64String(ToBytes());
		}

		/// <summary>
		/// Returns the ciphertext as a byte array.
		/// </summary>
		/// <returns>The byte array containing the ciphertext.</returns>
		public byte[] ToBytes() {
			using var ms = new MemoryStream();
			ms.Write(IV, 0, IV.Length);
			ms.Write(CipherText, 0, CipherText.Length);
			ms.Write(AuthTag, 0, AuthTag.Length);
			return ms.ToArray();
		}

		/// <summary>
		/// Returns the ciphertext as a stream.
		/// </summary>
		/// <returns>The MemoryStream containing the ciphertext.</returns>
		public Stream ToStream() {
			var ms = new MemoryStream();
			ms.Write(IV, 0, IV.Length);
			ms.Write(CipherText, 0, CipherText.Length);
			ms.Write(AuthTag, 0, AuthTag.Length);
			ms.Position = 0;
			return ms;
		}

		/// <summary>
		/// Constructs the EncryptedData class from the raw ciphertext bytes and the encryption metadata.
		/// </summary>
		/// <param name="cipherText">The raw ciphertext bytes.</param>
		/// <param name="algorithm">The encryption algorithm to use.</param>
		/// <param name="hash">The hash algorithm to use when calculating the HMAC on non-AEAD ciphertexts.</param>
		public static EncryptedData FromBytes(byte[] cipherText, EncryptionAlgorithm algorithm = EncryptionAlgorithm.Default, HashAlgorithm hash = HashAlgorithm.Default) {
			ReadOnlySpan<byte> iv = new ReadOnlySpan<byte>(cipherText, 0, algorithm.GetCipherIVLength());
			ReadOnlySpan<byte> ct = new ReadOnlySpan<byte>(cipherText, algorithm.GetCipherIVLength(), cipherText.Length - algorithm.GetCipherIVLength() - algorithm.GetAuthLength(hash));
			ReadOnlySpan<byte> at = new ReadOnlySpan<byte>(cipherText, cipherText.Length - algorithm.GetAuthLength(hash), algorithm.GetAuthLength(hash));
			return new EncryptedData(ct, iv, at, algorithm, hash);
		}

		/// <summary>
		/// Constructs the EncryptedData class from a Base64 encoded string.
		/// </summary>
		/// <param name="encoded">The Base64 encoded string containing both the ciphertext and the encryption metadata.</param>
		/// <param name="algorithm">The encryption algorithm to use.</param>
		/// <param name="hash">The hash algorithm to use when calculating the HMAC on non-AEAD ciphertexts.</param>
		public static EncryptedData FromBase64(string encoded, EncryptionAlgorithm algorithm = EncryptionAlgorithm.Default, HashAlgorithm hash = HashAlgorithm.Default) {
			return FromBytes(Convert.FromBase64String(encoded), algorithm, hash);
		}

		/// <summary>
		/// Constructs the EncryptedData class from the raw ciphertext stream and the encryption metadata.
		/// </summary>
		/// <param name="cipherText">The raw ciphertext stream.</param>
		/// <param name="algorithm">The encryption algorithm to use.</param>
		/// <param name="hash">The hash algorithm to use when calculating the HMAC on non-AEAD ciphertexts.</param>
		public static EncryptedData FromStream(Stream cipherText, EncryptionAlgorithm algorithm = EncryptionAlgorithm.Default, HashAlgorithm hash = HashAlgorithm.Default)
		{
			using (var ms = new MemoryStream()) {
				cipherText.CopyTo(ms);
				return FromBytes(ms.ToArray(), algorithm, hash);
			}
		}
	}
}
