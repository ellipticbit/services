﻿//-----------------------------------------------------------------------------
// Copyright (c) 2020-2023 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using System;
using System.IO;
using System.Text;

namespace EllipticBit.Services.Cryptography
{
	/// <summary>
	/// The result of the Password Verification
	/// </summary>
	public enum VerifyPasswordResult
	{
		/// <summary>
		/// The password verification was successful.
		/// </summary>
		Success,
		/// <summary>
		/// The password verification failed.
		/// </summary>
		Failure,
		/// <summary>
		/// The password was successfully verified, but needs to be rehashed to use updated hashing parameters.
		/// </summary>
		Rehash,
	}

	/// <summary>
	/// The Cryptography service interface.
	/// </summary>
	public interface ICryptographyService
	{
		/// <summary>
		/// Generates a cryptographically secure n-length array of bytes.
		/// </summary>
		/// <param name="bytes">The number of bytes to generate.</param>
		/// <returns>The generated bytes.</returns>
		byte[] RandomBytes(int bytes);

		/// <summary>
		/// Calculates a hash of the provided byte array.
		/// </summary>
		/// <param name="data">The data stream to hash.</param>
		/// <param name="algorithm">Optional. Used to specify which Hashing algorithm to use.</param>
		/// <returns>The hash value as an array of bytes.</returns>
		byte[] Hash(Stream data, HashAlgorithm algorithm = HashAlgorithm.Default);

		/// <summary>
		/// Calculates a cryptographically secure hash of the provided byte array using the provided key material.
		/// </summary>
		/// <param name="key">The key used to hash the data.</param>
		/// <param name="data">The data stream to hash.</param>
		/// <param name="algorithm">Optional. Used to specify which Hashing algorithm to use.</param>
		/// <returns>The hash value as an array of bytes.</returns>
		byte[] Hmac(ICryptographyKey key, Stream data, HashAlgorithm algorithm = HashAlgorithm.Default);

		/// <summary>
		/// Securely hashes the provided password for storage.
		/// </summary>
		/// <param name="password">The password to store.</param>
		/// <param name="pepper">A pepper value to secure the password during derivation.</param>
		/// <param name="associatedData">Unencrypted associated data to be stored with the secured password.</param>
		/// <returns></returns>
		string SecurePassword(string password, byte[] pepper = null, byte[] associatedData = null);

		/// <summary>
		/// Verifies a user supplied password with a previously secured password.
		/// </summary>
		/// <param name="storedPassword">The hashed password to verify.</param>
		/// <param name="suppliedPassword">The password from the user.</param>
		/// <param name="pepper">The pepper value used to secure the password during derivation.</param>
		/// <param name="associatedData">Unencrypted associated data stored with the secured password.</param>
		/// <returns></returns>
		VerifyPasswordResult VerifyPassword(string storedPassword, string suppliedPassword, byte[] pepper = null, byte[] associatedData = null);

		/// <summary>
		/// Derives key material from a password and salt.
		/// </summary>
		/// <param name="password">The password to derive the key from.</param>
		/// <param name="salt">A salt value to secure the password during derivation.</param>
		/// <param name="algorithm">The symmetric encryption algorithm this key is intended to be used with. Use EncryptionAlgorithm.None to specify a key for non-symmetric operations.</param>
		/// <returns>An ICryptographyKey type containing the key.</returns>
		ICryptographyKey DeriveKey(byte[] password, out byte[] salt, EncryptionAlgorithm algorithm);

		/// <summary>
		/// Generates a cryptographically secure random key.
		/// </summary>
		/// <param name="algorithm">The symmetric encryption algorithm this key is intended to be used with. Use EncryptionAlgorithm.None to specify a key for non-symmetric operations.</param>
		/// <returns>An ICryptographyKey type containing the key.</returns>
		ICryptographyKey GenerateKey(EncryptionAlgorithm algorithm);

		/// <summary>
		/// Initializes a ICryptographyKey with pre-existing key value.
		/// </summary>
		/// <param name="keyBytes">The key bytes.</param>
		/// <param name="algorithm">The symmetric encryption algorithm this key is intended to be used with. Use EncryptionAlgorithm.None to specify a key for non-symmetric operations.</param>
		/// <returns>An ICryptographyKey type containing the key.</returns>
		ICryptographyKey InitializeKey(byte[] keyBytes, EncryptionAlgorithm algorithm);

		/// <summary>
		///	Encrypts data securely for storage.
		/// </summary>
		/// <param name="key">The key used to encrypt the data.</param>
		/// <param name="data">The data to encrypt.</param>
		/// <param name="associatedData">The associated data to use during encryption.</param>
		/// <returns>Returns a byte array with the decrypted plaintext.</returns>
		EncryptedData Encrypt(ICryptographyKey key, byte[] data, byte[] associatedData = null);

		/// <summary>
		///	Decrypts data previously encrypted with the Encrypt method.
		/// </summary>
		/// <param name="key">The key used to decrypt the data.</param>
		/// <param name="data">The data to decrypt.</param>
		/// <param name="associatedData">The associated data to use during encryption.</param>
		/// <returns>Returns a byte array with the decrypted plaintext.</returns>
		byte[] Decrypt(ICryptographyKey key, EncryptedData data, byte[] associatedData = null);
	}

	/// <summary>
	/// Extensions for ICryptographyService.
	/// </summary>
	public static class CryptographyServiceExtensions
	{
		/// <summary>
		/// Calculates a hash of the provided byte array.
		/// </summary>
		/// <param name="service">The ICryptographyService interface.</param>
		/// <param name="data">The data to hash.</param>
		/// <param name="algorithm">Optional. Used to specify which Hashing algorithm to use.</param>
		/// <returns>The hash value as an array of bytes.</returns>
		public static byte[] Hash(this ICryptographyService service, byte[] data, HashAlgorithm algorithm = HashAlgorithm.Default) {
			using var ms = new MemoryStream(data);
			return service.Hash(ms, algorithm);
		}

		/// <summary>
		/// Calculates a cryptographically secure hash of the provided byte array using the provided key material.
		/// </summary>
		/// <param name="service">The ICryptographyService interface.</param>
		/// <param name="key">The key used to hash the data.</param>
		/// <param name="data">The data to hash.</param>
		/// <param name="algorithm">Optional. Used to specify which Hashing algorithm to use.</param>
		/// <returns>The hash value as an array of bytes.</returns>
		public static byte[] Hmac(this ICryptographyService service, ICryptographyKey key, byte[] data, HashAlgorithm algorithm = HashAlgorithm.Default) {
			using var ms = new MemoryStream(data);
			return service.Hmac(key, ms, algorithm);
		}

		/// <summary>
		///	Encrypts data securely for storage.
		/// </summary>
		/// <param name="service">The ICryptographyService interface.</param>
		/// <param name="key">The key used to encrypt the data.</param>
		/// <param name="data">The data to encrypt.</param>
		/// <param name="associatedData">The associated data to use during encryption.</param>
		/// <returns>Returns a byte array with the decrypted plaintext.</returns>
		public static EncryptedData Encrypt(this ICryptographyService service, ICryptographyKey key, Stream data, byte[] associatedData = null) {
			using var ms = new MemoryStream();
			data.CopyTo(ms);
			return service.Encrypt(key, ms.ToArray(), associatedData);
		}

		/// <summary>
		///	Decrypts data previously encrypted with the Encrypt method.
		/// </summary>
		/// <param name="service">The ICryptographyService interface.</param>
		/// <param name="key">The key used to decrypt the data.</param>
		/// <param name="data">The raw bytes data to decrypt.</param>
		/// <param name="associatedData">The associated data to use during encryption.</param>
		/// <returns>Returns a byte array with the decrypted plaintext.</returns>
		public static byte[] Decrypt(this ICryptographyService service, ICryptographyKey key, byte[] data, byte[] associatedData = null) {
			return service.Decrypt(key, EncryptedData.FromBytes(data), associatedData);
		}

		/// <summary>
		///	Decrypts data previously encrypted with the Encrypt method.
		/// </summary>
		/// <param name="service">The ICryptographyService interface.</param>
		/// <param name="key">The key used to decrypt the data.</param>
		/// <param name="data">The Base64 encoded data to decrypt.</param>
		/// <param name="associatedData">The associated data to use during encryption.</param>
		/// <returns>Returns a byte array with the decrypted plaintext.</returns>
		public static byte[] Decrypt(this ICryptographyService service, ICryptographyKey key, string data, byte[] associatedData = null) {
			return service.Decrypt(key, EncryptedData.FromBase64(data), associatedData);

		}

		/// <summary>
		///	Decrypts data previously encrypted with the Encrypt method.
		/// </summary>
		/// <param name="service">The ICryptographyService interface.</param>
		/// <param name="key">The key used to decrypt the data.</param>
		/// <param name="data">The stream data to decrypt.</param>
		/// <param name="associatedData">The associated data to use during encryption.</param>
		/// <returns>Returns a byte array with the decrypted plaintext.</returns>
		public static byte[] Decrypt(this ICryptographyService service, ICryptographyKey key, Stream data, byte[] associatedData = null) {
			return service.Decrypt(key, EncryptedData.FromStream(data), associatedData);
		}

		/// <summary>
		/// Derives key material from a password and salt.
		/// </summary>
		/// <param name="service">The ICryptographyService interface.</param>
		/// <param name="password">The password to derive the key from.</param>
		/// <param name="salt">A salt value to secure the password during derivation.</param>
		/// <param name="algorithm">Optional. The symmetric encryption algorithm this key is intended to be used with. Use EncryptionAlgorithm.None to specify a key for non-symmetric operations.</param>
		/// <returns>A byte array containing the key material.</returns>
		public static ICryptographyKey DeriveKey(this ICryptographyService service, string password, out byte[] salt, EncryptionAlgorithm algorithm = EncryptionAlgorithm.Default) {
			return service.DeriveKey(Encoding.UTF8.GetBytes(password), out salt, algorithm);
		}

		/// <summary>
		/// Generates a cryptographically secure random key.
		/// </summary>
		/// <param name="service">The ICryptographyService interface.</param>
		/// <param name="algorithm">Optional. The symmetric encryption algorithm this key is intended to be used with. Use EncryptionAlgorithm.None to specify a key for non-symmetric operations.</param>
		/// <returns>A byte array containing the key material.</returns>
		public static ICryptographyKey GenerateKey(this ICryptographyService service, EncryptionAlgorithm algorithm = EncryptionAlgorithm.Default) {
			return service.GenerateKey(algorithm);
		}

		/// <summary>
		/// Initializes a ICryptographyKey with pre-existing key value.
		/// </summary>
		/// <param name="service">The ICryptographyService interface.</param>
		/// <param name="keyString">The Base64 encoded key bytes.</param>
		/// <param name="algorithm">Optional. The symmetric encryption algorithm this key is intended to be used with. Use EncryptionAlgorithm.None to specify a key for non-symmetric operations.</param>
		/// <returns>An ICryptographyKey type containing the key.</returns>
		public static ICryptographyKey InitializeKey(this ICryptographyService service, string keyString, EncryptionAlgorithm algorithm = EncryptionAlgorithm.Default) {
			return service.InitializeKey(Convert.FromBase64String(keyString), algorithm);
		}
	}
}
