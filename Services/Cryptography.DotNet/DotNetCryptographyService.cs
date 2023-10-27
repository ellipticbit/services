//----------------------------------------------------------------
// Copyright (c) 2017-2023 EllipticBit, LLC - All Rights Reserved.
//----------------------------------------------------------------

using Konscious.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace EllipticBit.Services.Cryptography
{
	public class DotNetCryptographyService : ICryptographyService
	{
		private readonly DotNetCryptographyServiceOptions options;

		public DotNetCryptographyService(DotNetCryptographyServiceOptions options) {
			this.options = options;
		}

		public bool ConstantTimeEquality(byte[] a, byte[] b) {
			if (a == null || b == null ||
				(a.Length != b.Length))
				return false;

			int differentbits = 0;
			for (int i = 0; i < a.Length; ++i) {
				differentbits |= a[i] ^ b[i];
			}

			return differentbits == 0;
		}

		public byte[] Decrypt(string key, string data, byte[] salt = null)
		{
			int rb = (options.EncryptionAlgorithm == EncryptionAlgorithm.AES256GCM || options.EncryptionAlgorithm == EncryptionAlgorithm.AES256CBC) ? 32 :
				(options.EncryptionAlgorithm == EncryptionAlgorithm.AES192CBC || options.EncryptionAlgorithm == EncryptionAlgorithm.AES192GCM) ? 24 : 16;
			return Decrypt(DeriveKey(key, salt ?? Encoding.UTF8.GetBytes(key), rb), data);
		}

		public byte[] Decrypt(byte[] key, string data) {
			var dataParts = data.Split(new[] { "." }, StringSplitOptions.None);
			var encryptionAlgorithm = (EncryptionAlgorithm)Convert.ToInt32(dataParts[0]);
			var hashAlgorithm = (HashAlgorithm)Convert.ToInt32(dataParts[1]);
			var hash = dataParts[2] != null ? Convert.FromBase64String(dataParts[2]) : null;
			var nonce = Convert.FromBase64String(dataParts[3]);
			var ciphertext = Convert.FromBase64String(dataParts[4]);

			if (key.Length != 16 && (encryptionAlgorithm == EncryptionAlgorithm.AES128CBC || encryptionAlgorithm == EncryptionAlgorithm.AES128GCM)) throw new ArgumentOutOfRangeException("Invalid key size. Key must be 16 bytes.");
			if (key.Length != 24 && (encryptionAlgorithm == EncryptionAlgorithm.AES192CBC || encryptionAlgorithm == EncryptionAlgorithm.AES192GCM)) throw new ArgumentOutOfRangeException("Invalid key size. Key must be 24 bytes.");
			if (key.Length != 32 && (encryptionAlgorithm == EncryptionAlgorithm.AES256CBC || encryptionAlgorithm == EncryptionAlgorithm.AES256GCM)) throw new ArgumentOutOfRangeException("Invalid key size. Key must be 32 bytes.");

			//Verify hash
			if (hash != null && hashAlgorithm != HashAlgorithm.None) {
				if (!ConstantTimeEquality(Hash(key, ciphertext, hashAlgorithm), hash)) {
					throw new CryptographicException("Provided hash did not match calculated hash.");
				}
			} else if ((hash == null || hash.Length == 0) && hashAlgorithm != HashAlgorithm.None) {
				throw new CryptographicException("No hash provided to compare using specified algorithm.");
			} else if ((hash != null || hash.Length != 0) && hashAlgorithm == HashAlgorithm.None) {
				throw new CryptographicException("No hash algorithm specified.");
			}

			if (encryptionAlgorithm == EncryptionAlgorithm.AES128CBC || encryptionAlgorithm == EncryptionAlgorithm.AES192CBC || encryptionAlgorithm == EncryptionAlgorithm.AES256CBC) {
				using var alg = new AesCryptoServiceProvider();
				alg.KeySize = key.Length * 8;
				alg.Key = key;
				alg.IV = nonce;
				alg.Padding = PaddingMode.PKCS7;

				using var cryptor = alg.CreateDecryptor(alg.Key, alg.IV);
				using var rms = new MemoryStream(ciphertext);
				using var oms = new MemoryStream();
				using (var cs = new CryptoStream(rms, cryptor, CryptoStreamMode.Read))
				{
					cs.CopyTo(oms);
				}

				return oms.ToArray();
			}
			else if (encryptionAlgorithm == EncryptionAlgorithm.AES128GCM || encryptionAlgorithm == EncryptionAlgorithm.AES192GCM || encryptionAlgorithm == EncryptionAlgorithm.AES256GCM) {
				using var alg = new AesGcm(key);
				var dec = new byte[ciphertext.Length];
				alg.Decrypt(nonce, ciphertext, hash, dec);
				return dec;
			}

			throw new NotSupportedException("Unsupported Encryption Algorithm specified.");
		}

		public string Encrypt(string key, byte[] data, byte[] salt = null)
		{
			int rb = (options.EncryptionAlgorithm == EncryptionAlgorithm.AES256GCM || options.EncryptionAlgorithm == EncryptionAlgorithm.AES256CBC) ? 32 :
				(options.EncryptionAlgorithm == EncryptionAlgorithm.AES192CBC || options.EncryptionAlgorithm == EncryptionAlgorithm.AES192GCM) ? 24 : 16;
			return Encrypt(DeriveKey(key, salt ?? Encoding.UTF8.GetBytes(key), rb), data);
		}

		public string Encrypt(byte[] key, byte[] data) {
			var encryptionAlgorithm = options.EncryptionAlgorithm;
			var hashAlgorithm = options.HashAlgorithm;

			if (key.Length != 16 && (encryptionAlgorithm == EncryptionAlgorithm.AES128CBC || encryptionAlgorithm == EncryptionAlgorithm.AES128GCM)) throw new ArgumentOutOfRangeException("Invalid key size. Key must be 16 bytes.");
			if (key.Length != 24 && (encryptionAlgorithm == EncryptionAlgorithm.AES192CBC || encryptionAlgorithm == EncryptionAlgorithm.AES192GCM)) throw new ArgumentOutOfRangeException("Invalid key size. Key must be 24 bytes.");
			if (key.Length != 32 && (encryptionAlgorithm == EncryptionAlgorithm.AES256CBC || encryptionAlgorithm == EncryptionAlgorithm.AES256GCM)) throw new ArgumentOutOfRangeException("Invalid key size. Key must be 32 bytes.");

			if (encryptionAlgorithm == EncryptionAlgorithm.AES128CBC || encryptionAlgorithm == EncryptionAlgorithm.AES192CBC || encryptionAlgorithm == EncryptionAlgorithm.AES256CBC) {
				using var alg = new AesCryptoServiceProvider();
				alg.KeySize = key.Length * 8;
				alg.Key = key;
				alg.IV = RandomBytes(16);
				alg.Padding = PaddingMode.PKCS7;

				using var cryptor = alg.CreateEncryptor(alg.Key, alg.IV);
				using var ms = new MemoryStream();
				using (var cs = new CryptoStream(ms, cryptor, CryptoStreamMode.Write)) {
					cs.Write(data, 0, data.Length);
					cs.FlushFinalBlock();
				}
				var enc = ms.ToArray();

				var th = Convert.ToBase64String(Hash(key, enc, hashAlgorithm));
				var ti = Convert.ToBase64String(alg.IV);
				var tc = Convert.ToBase64String(enc);
				return string.Join(".", new[] { Convert.ToInt32(encryptionAlgorithm).ToString(), Convert.ToInt32(hashAlgorithm).ToString(), th, ti, tc });
			} else if (encryptionAlgorithm == EncryptionAlgorithm.AES128GCM || encryptionAlgorithm == EncryptionAlgorithm.AES192GCM || encryptionAlgorithm == EncryptionAlgorithm.AES256GCM) {
				using var alg = new AesGcm(key);
				var nonce = RandomBytes(12);
				var enc = new byte[data.Length];
				var tag = new byte[16];
				alg.Encrypt(nonce, data, enc, tag);

				var th = Convert.ToBase64String(tag);
				var ti = Convert.ToBase64String(nonce);
				var tc = Convert.ToBase64String(enc);
				return string.Join(".", new[] { Convert.ToInt32(encryptionAlgorithm).ToString(), Convert.ToInt32(hashAlgorithm).ToString(), th, ti, tc });
			}

			throw new NotSupportedException("Unsupported Encryption Algorithm specified.");
		}

		public byte[] Hash(byte[] data, HashAlgorithm algorithm = HashAlgorithm.Default) {
			if (algorithm == HashAlgorithm.Default) {
				return Hash(data, options.HashAlgorithm);
			}
			if (algorithm == HashAlgorithm.SHA256) {
				using (var hash = new SHA256CryptoServiceProvider()) {
					return hash.ComputeHash(data);
				}
			}
			if (algorithm == HashAlgorithm.SHA384) {
				using (var hash = new SHA384CryptoServiceProvider()) {
					return hash.ComputeHash(data);
				}
			}
			if (algorithm == HashAlgorithm.SHA512) {
				using (var hash = new SHA512CryptoServiceProvider()) {
					return hash.ComputeHash(data);
				}
			}

			throw new NotSupportedException("Unsupported Hash Algorithm specified.");
		}

		public byte[] Hash(byte[] key, byte[] data, HashAlgorithm algorithm = HashAlgorithm.Default) {
			if (algorithm == HashAlgorithm.Default) {
				return Hash(data, key, options.HashAlgorithm);
			}
			if (algorithm == HashAlgorithm.SHA256) {
				using (var hash = new HMACSHA256(key)) {
					return hash.ComputeHash(data);
				}
			}
			if (algorithm == HashAlgorithm.SHA384) {
				using (var hash = new HMACSHA384(key)) {
					return hash.ComputeHash(data);
				}
			}
			if (algorithm == HashAlgorithm.SHA512) {
				using (var hash = new HMACSHA512(key)) {
					return hash.ComputeHash(data);
				}
			}

			throw new NotSupportedException("Unsupported Hash Algorithm specified.");
		}

		public byte[] RandomBytes(int bytes) {
			using (var rng = new RNGCryptoServiceProvider()) {
				var bl = new byte[bytes];
				rng.GetBytes(bl);
				return bl;
			}
		}

		public string SecurePasssword(string password, byte[] pepper = null, byte[] associatedData = null) {
			if (options.PasswordAlgorithm == PasswordAlgorithm.PBKDF2) {
				var salt = RandomBytes(32); //TODO: This may need to be changed to match the HMAC key width.
				var passwordHash = KeyDerivation.Pbkdf2(password, salt, options.PBKDF2Algorithm, options.PBKDF2Iterations, options.PBKDF2OutputLength);
				var parameters = string.Join(",", ((int)options.PBKDF2Algorithm).ToString(), options.PBKDF2Iterations.ToString());
				return string.Join(".", options.PasswordParametersVersion.ToString(), ((int)options.PasswordAlgorithm).ToString(), parameters, Convert.ToBase64String(salt), Convert.ToBase64String(passwordHash));
			} else if (options.PasswordAlgorithm == PasswordAlgorithm.BCrypt) {
				return string.Join(".", options.PasswordParametersVersion.ToString(), ((int)options.PasswordAlgorithm).ToString(), BCrypt.Net.BCrypt.EnhancedHashPassword(password, options.BCryptWorkFactor));
			} else if (options.PasswordAlgorithm == PasswordAlgorithm.Argon2) {
				var salt = RandomBytes(32);
				var argon = new Argon2id(System.Text.Encoding.UTF8.GetBytes(password)) {
					KnownSecret = pepper ?? options.Pepper,
					Salt = salt,
					AssociatedData = associatedData,
					DegreeOfParallelism = options.Argon2Parallelism,
					MemorySize = options.Argon2MemorySize,
					Iterations = options.Argon2Iterations
				};
				var parameters = string.Join(",", options.Argon2Parallelism.ToString(), options.Argon2MemorySize.ToString(), options.Argon2Iterations.ToString());
				var passwordHash = argon.GetBytes(options.Argon2OutputLength);
				return string.Join(".", options.PasswordParametersVersion.ToString(), ((int)options.PasswordAlgorithm).ToString(), parameters, Convert.ToBase64String(salt), Convert.ToBase64String(passwordHash));
			} else {
				throw new NotSupportedException("Unsupported Hash Algorithm specified.");
			}
		}

		public VerifyPasswordResult VerifyPasssword(string storedPassword, string suppliedPassword, byte[] pepper = null, byte[] associatedData = null) {
			var dataParts = storedPassword.Split(new[] { "." }, StringSplitOptions.None);
			var version = Convert.ToInt32(dataParts[0]);
			var passwordAlgorithm = (PasswordAlgorithm)Convert.ToInt32(dataParts[1]);

			if (options.PasswordAlgorithm == PasswordAlgorithm.PBKDF2) {
				var parameters = dataParts[2].Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(a => Convert.ToInt32(a)).ToArray();
				var salt = Convert.FromBase64String(dataParts[3]);
				var ciphertext = Convert.FromBase64String(dataParts[4]);
				var prf = (KeyDerivationPrf)parameters[0];
				var iterations = parameters[1];
				var pwhashstr = BitConverter.ToString(Hash(pepper ?? options.Pepper ?? salt, System.Text.Encoding.UTF8.GetBytes(suppliedPassword))).Replace("-", "").ToUpperInvariant();
				var suppliedHash = KeyDerivation.Pbkdf2(pwhashstr, salt, prf, iterations, ciphertext.Length);
				if (!ConstantTimeEquality(suppliedHash, ciphertext)) return VerifyPasswordResult.Failure;
			} else if (options.PasswordAlgorithm == PasswordAlgorithm.BCrypt) {
				if (!BCrypt.Net.BCrypt.EnhancedVerify(suppliedPassword, dataParts[2])) return VerifyPasswordResult.Failure;
			} else if (options.PasswordAlgorithm == PasswordAlgorithm.Argon2) {
				var parameters = dataParts[2].Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(a => Convert.ToInt32(a)).ToArray();
				var salt = Convert.FromBase64String(dataParts[3]);
				var ciphertext = Convert.FromBase64String(dataParts[4]);
				var argon = new Argon2id(System.Text.Encoding.UTF8.GetBytes(suppliedPassword)) {
					KnownSecret = pepper ?? options.Pepper,
					Salt = salt,
					AssociatedData = associatedData,
					DegreeOfParallelism = parameters[0],
					MemorySize = parameters[1],
					Iterations = parameters[2]
				};
				if (!ConstantTimeEquality(argon.GetBytes(ciphertext.Length), ciphertext)) return VerifyPasswordResult.Failure;
			} else {
				throw new NotSupportedException("Unsupported Hash Algorithm specified.");
			}

			//WARNING! Do Not Move This Line!
			if (version != options.PasswordParametersVersion) return VerifyPasswordResult.Rehash;
			return VerifyPasswordResult.Success;
		}

		public byte[] DeriveKey(string password, byte[] salt, int requiredBytes) {
			if (options.PasswordAlgorithm == PasswordAlgorithm.PBKDF2) {
				return KeyDerivation.Pbkdf2(password, salt, options.PBKDF2Algorithm, options.PBKDF2Iterations, requiredBytes);
			} else if (options.PasswordAlgorithm == PasswordAlgorithm.Argon2) {
				var argon = new Argon2id(System.Text.Encoding.UTF8.GetBytes(password)) {
					Salt = salt,
					DegreeOfParallelism = options.Argon2Parallelism,
					MemorySize = options.Argon2MemorySize,
					Iterations = options.Argon2Iterations
				};
				return argon.GetBytes(requiredBytes);
			} else {
				throw new NotSupportedException("Unsupported Hash Algorithm specified.");
			}
		}
	}
}
