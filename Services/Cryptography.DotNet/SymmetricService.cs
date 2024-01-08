//----------------------------------------------------------------
// Copyright (c) 2023 EllipticBit, LLC - All Rights Reserved.
//----------------------------------------------------------------

using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace EllipticBit.Services.Cryptography
{
	internal class SymmetricService : ICryptographySymmetric
	{
		private readonly ICryptographyMac macService;

		public SymmetricService(ICryptographyMac macService) {
			this.macService = macService;
		}

		public byte[] Encrypt(ReadOnlySpan<byte> data, ReadOnlySpan<byte> associatedData, ReadOnlySpan<byte> key, ReadOnlySpan<byte> iv, out byte[] authTag, EncryptionAlgorithm algorithm, HashAlgorithm hash) {
			if (key.Length != 16 && (algorithm == EncryptionAlgorithm.AES128CBC || algorithm == EncryptionAlgorithm.AES128CFB || algorithm == EncryptionAlgorithm.AES128GCM)) throw new ArgumentOutOfRangeException("Invalid key size. Key must be 16 bytes.");
			if (key.Length != 24 && (algorithm == EncryptionAlgorithm.AES192CBC || algorithm == EncryptionAlgorithm.AES192CFB || algorithm == EncryptionAlgorithm.AES192GCM)) throw new ArgumentOutOfRangeException("Invalid key size. Key must be 24 bytes.");
			if (key.Length != 32 && (algorithm == EncryptionAlgorithm.AES256CBC || algorithm == EncryptionAlgorithm.AES256CFB || algorithm == EncryptionAlgorithm.AES256GCM || algorithm == EncryptionAlgorithm.ChaCha20Poly1305)) throw new ArgumentOutOfRangeException("Invalid key size. Key must be 32 bytes.");


			if (algorithm == EncryptionAlgorithm.AES128GCM || algorithm == EncryptionAlgorithm.AES192GCM || algorithm == EncryptionAlgorithm.AES256GCM)
			{
				var enc = new byte[data.Length];
				var tag = new byte[16];

				using var alg = new AesGcm(key, 16);
				alg.Encrypt(iv, data, enc, tag, associatedData);

				authTag = tag;
				return enc;
			}
			if (algorithm == EncryptionAlgorithm.ChaCha20Poly1305) {
				var enc = new byte[data.Length];
				var tag = new byte[16];

				using var alg = new ChaCha20Poly1305(key);
				alg.Encrypt(iv, data, enc, tag, associatedData);

				authTag = tag;
				return enc;
			}
			if (algorithm != EncryptionAlgorithm.None) {
				byte[] tiv = iv.ToArray();

				using var alg = Aes.Create();
				alg.Mode = CipherMode.CBC;
				if (algorithm == EncryptionAlgorithm.AES128CFB || algorithm == EncryptionAlgorithm.AES192CFB || algorithm == EncryptionAlgorithm.AES256CFB) alg.Mode = CipherMode.CFB;
				alg.KeySize = key.Length * 8;
				alg.Key = key.ToArray();
				alg.IV = tiv;
				alg.Padding = PaddingMode.PKCS7;

				using var crypt = alg.CreateEncryptor(alg.Key, alg.IV);
				using var ms = new MemoryStream();
				using (var cs = new CryptoStream(ms, crypt, CryptoStreamMode.Write))
				{
					cs.Write(data.ToArray(), 0, data.Length);
					cs.FlushFinalBlock();
				}

				var enc = ms.ToArray();
				var ad = associatedData.ToArray();

				var tagTask = Task.Run(async () => {
					var adhash = macService.Hmac(tiv, ad, hash);
					var enchash = macService.Hmac(tiv, enc, hash);
					await Task.WhenAll(adhash, enchash);
					return await macService.Hmac(tiv, enchash.Result.Concat(adhash.Result).ToArray(), hash);
				});

				authTag = tagTask.Result;
				return enc;
			}

			throw new NotSupportedException("Unsupported Encryption Algorithm specified.");
		}

		public byte[] Decrypt(ReadOnlySpan<byte> data, ReadOnlySpan<byte> associatedData, ReadOnlySpan<byte> key, ReadOnlySpan<byte> iv, ReadOnlySpan<byte> authTag, EncryptionAlgorithm algorithm, HashAlgorithm hash) {
			if (key.Length != 16 && (algorithm == EncryptionAlgorithm.AES128CBC || algorithm == EncryptionAlgorithm.AES128CFB || algorithm == EncryptionAlgorithm.AES128GCM)) throw new ArgumentOutOfRangeException("Invalid key size. Key must be 16 bytes.");
			if (key.Length != 24 && (algorithm == EncryptionAlgorithm.AES192CBC || algorithm == EncryptionAlgorithm.AES192CFB || algorithm == EncryptionAlgorithm.AES192GCM)) throw new ArgumentOutOfRangeException("Invalid key size. Key must be 24 bytes.");
			if (key.Length != 32 && (algorithm == EncryptionAlgorithm.AES256CBC || algorithm == EncryptionAlgorithm.AES256CFB || algorithm == EncryptionAlgorithm.AES256GCM || algorithm == EncryptionAlgorithm.ChaCha20Poly1305)) throw new ArgumentOutOfRangeException("Invalid key size. Key must be 32 bytes.");

			if (algorithm == EncryptionAlgorithm.AES128GCM || algorithm == EncryptionAlgorithm.AES192GCM || algorithm == EncryptionAlgorithm.AES256GCM)
			{
				var dec = new byte[data.Length];

				using var alg = new AesGcm(key, 16);
				alg.Decrypt(iv, data, authTag, dec, associatedData);

				return dec;
			}
			if (algorithm == EncryptionAlgorithm.ChaCha20Poly1305)
			{
				var dec = new byte[data.Length];

				using var alg = new ChaCha20Poly1305(key);
				alg.Decrypt(iv, data, authTag, dec, associatedData);

				return dec;
			}
			if (algorithm != EncryptionAlgorithm.None) {
				byte[] tiv = iv.ToArray();
				var ad = associatedData.ToArray();
				var enc = data.ToArray();

				var tagTask = Task.Run(async () => {
					var adhash = macService.Hmac(tiv, ad, hash);
					var enchash = macService.Hmac(tiv, enc, hash);
					await Task.WhenAll(adhash, enchash);
					return await macService.Hmac(tiv, enchash.Result.Concat(adhash.Result).ToArray(), hash);
				});
				var calcTag = tagTask.Result;

				if (!calcTag.ConstantTimeEquality(authTag.ToArray())) {
					throw new CryptographicException("Authentication Tag verification failed.");
				}

				using var alg = Aes.Create();
				alg.Mode = CipherMode.CBC;
				if (algorithm == EncryptionAlgorithm.AES128CFB || algorithm == EncryptionAlgorithm.AES192CFB || algorithm == EncryptionAlgorithm.AES256CFB) alg.Mode = CipherMode.CFB;
				alg.KeySize = key.Length * 8;
				alg.Key = key.ToArray();
				alg.IV = tiv;
				alg.Padding = PaddingMode.PKCS7;

				using var crypt = alg.CreateDecryptor(alg.Key, alg.IV);
				using var rms = new MemoryStream(enc);
				using var oms = new MemoryStream();
				using var cs = new CryptoStream(rms, crypt, CryptoStreamMode.Read);

				cs.CopyTo(oms);
				return oms.ToArray();
			}

			throw new NotSupportedException("Unsupported Encryption Algorithm specified.");
		}
	}
}
