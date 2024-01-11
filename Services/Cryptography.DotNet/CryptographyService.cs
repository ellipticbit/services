//----------------------------------------------------------------
// Copyright (c) 2023 EllipticBit, LLC - All Rights Reserved.
//----------------------------------------------------------------

using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace EllipticBit.Services.Cryptography
{
	internal class CryptographyService : ICryptographyService
	{
		private readonly ICryptographyHash hashService;
		private readonly ICryptographyMac macService;
		private readonly ICryptographyKdf kdfService;
		private readonly ICryptographySymmetric symmetricService;

		public CryptographyService(ICryptographyHash hashService, ICryptographyMac macService, ICryptographyKdf kdfService, ICryptographySymmetric symmetricService) {
			this.hashService = hashService;
			this.macService = macService;
			this.kdfService = kdfService;
			this.symmetricService = symmetricService;
		}

		public byte[] RandomBytes(int bytes) {
			return RandomNumberGenerator.GetBytes(bytes);
		}

		public Task<byte[]> Hash(Stream data, HashAlgorithm algorithm = HashAlgorithm.SHA2_384) {
			return hashService.Hash(data, algorithm);
		}

		public Task<byte[]> Hmac(ICryptographyKey key, Stream data, HashAlgorithm algorithm = HashAlgorithm.SHA2_384) {
			var tk = key as SymmetricKey;
			if (tk == null) throw new ArgumentNullException(nameof(key));
			return macService.Hmac(tk.Key, data, algorithm);
		}

		public HashedPassword SecurePassword(string password, byte[] pepper, byte[] associatedData = null, PasswordAlgorithm algorithm = PasswordAlgorithm.Default) {
			if (algorithm == PasswordAlgorithm.Hkdf) throw new NotSupportedException("HKDF is not supported for password security.");
			if (algorithm == PasswordAlgorithm.Pbkdf2) {
				byte[] salt = RandomNumberGenerator.GetBytes(32);
				return new HashedPassword(kdfService.PBKDF2(password, salt, pepper, 64), salt, algorithm, 1);
			}
			if (algorithm == PasswordAlgorithm.SCrypt) {
				byte[] salt = RandomNumberGenerator.GetBytes(32);
				return new HashedPassword(kdfService.SCrypt(password, salt, pepper, 64), salt, algorithm, 1);
			}
			if (algorithm == PasswordAlgorithm.Argon2) {
				byte[] salt = RandomNumberGenerator.GetBytes(32);
				return new HashedPassword(kdfService.Argon2(password, salt, pepper, associatedData, 64), salt, algorithm, 1);
			}

			throw new NotSupportedException("PasswordAlgorithm.None is not supported for password security.");
		}

		public VerifyPasswordResult VerifyPassword(string suppliedPassword, HashedPassword storedPassword, byte[] pepper, byte[] associatedData = null) {
			if (storedPassword.Algorithm == PasswordAlgorithm.Pbkdf2 && storedPassword.ParameterVersion == 1)
			{
				var supplied = kdfService.PBKDF2(suppliedPassword, storedPassword.Salt, pepper, 64);
				if (supplied.ConstantTimeEquality(storedPassword.Derived)) return VerifyPasswordResult.Success;
			}
			else if (storedPassword.Algorithm == PasswordAlgorithm.Pbkdf2 && storedPassword.ParameterVersion == 0)
			{
				var supplied = kdfService.PBKDF2_V0(suppliedPassword, storedPassword.Salt, storedPassword.Derived.Length);
				if (supplied.ConstantTimeEquality(storedPassword.Derived)) return VerifyPasswordResult.Rehash;
			}

			if (storedPassword.Algorithm == PasswordAlgorithm.SCrypt && storedPassword.ParameterVersion == 1) {
				var supplied = kdfService.SCrypt(suppliedPassword, storedPassword.Salt, pepper, 64);
				if (supplied.ConstantTimeEquality(storedPassword.Derived)) return VerifyPasswordResult.Success;
			}

			if (storedPassword.Algorithm == PasswordAlgorithm.Argon2 && storedPassword.ParameterVersion == 1)
			{
				var supplied = kdfService.Argon2(suppliedPassword, storedPassword.Salt, pepper, associatedData, 64);
				if (supplied.ConstantTimeEquality(storedPassword.Derived)) return VerifyPasswordResult.Success;
			}
			else if (storedPassword.Algorithm == PasswordAlgorithm.Argon2 && storedPassword.ParameterVersion == 0)
			{
				var supplied = kdfService.Argon2_V0(suppliedPassword, storedPassword.Salt, pepper, associatedData, storedPassword.Derived.Length);
				if (supplied.ConstantTimeEquality(storedPassword.Derived)) return VerifyPasswordResult.Rehash;
			}

			return VerifyPasswordResult.Failure;
		}

		public ICryptographyKey DeriveKey(byte[] password, byte[] salt, EncryptionAlgorithm algorithm) {
			return new SymmetricKey(kdfService.HKDF(password, salt, null, algorithm.GetCipherKeyLength()), algorithm);
		}

		public ICryptographyKey GenerateKey(EncryptionAlgorithm algorithm) {
			return new SymmetricKey(RandomNumberGenerator.GetBytes(algorithm.GetCipherKeyLength()), algorithm);
		}

		public ICryptographyKey InitializeKey(byte[] keyBytes, EncryptionAlgorithm algorithm) {
			if (algorithm != EncryptionAlgorithm.None) {
				if (keyBytes.Length != 16 && (algorithm == EncryptionAlgorithm.AES128CBC || algorithm == EncryptionAlgorithm.AES128CFB || algorithm == EncryptionAlgorithm.AES128GCM)) throw new ArgumentOutOfRangeException("Invalid key size. Key must be 16 bytes.");
				if (keyBytes.Length != 24 && (algorithm == EncryptionAlgorithm.AES192CBC || algorithm == EncryptionAlgorithm.AES192CFB || algorithm == EncryptionAlgorithm.AES192GCM)) throw new ArgumentOutOfRangeException("Invalid key size. Key must be 24 bytes.");
				if (keyBytes.Length != 32 && (algorithm == EncryptionAlgorithm.AES256CBC || algorithm == EncryptionAlgorithm.AES256CFB || algorithm == EncryptionAlgorithm.AES256GCM || algorithm == EncryptionAlgorithm.ChaCha20Poly1305)) throw new ArgumentOutOfRangeException("Invalid key size. Key must be 32 bytes.");
			}

			return new SymmetricKey(keyBytes, algorithm);
		}

		public EncryptedData Encrypt(ICryptographyKey key, byte[] data, byte[] associatedData = null) {
			var tk = key as SymmetricKey;
			if (tk == null) throw new ArgumentNullException(nameof(key));
			byte[] iv = RandomNumberGenerator.GetBytes(EncryptionAlgorithm.Default.GetCipherIVLength());
			return symmetricService.Encrypt(data, associatedData, tk.Key, iv, tk.Algorithm, HashAlgorithm.Default);
		}

		public byte[] Decrypt(ICryptographyKey key, EncryptedData data, byte[] associatedData = null) {
			var tk = key as SymmetricKey;
			if (tk == null) throw new ArgumentNullException(nameof(key));
			return symmetricService.Decrypt(data, associatedData, tk.Key);
		}
	}
}
