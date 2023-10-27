//-----------------------------------------------------------------------------
// Copyright (c) 2020 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using Azure.Security.KeyVault.Keys.Cryptography;
using System.IO;
using System.Threading.Tasks;

namespace EllipticBit.Services.Secret
{
	class AzureKeyVaultKeyCryptographyService : IKeyCryptographyService
	{
		private readonly CryptographyClient cryptographyClient;

		public AzureKeyVaultKeyCryptographyService(CryptographyClient cryptographyClient) {
			this.cryptographyClient = cryptographyClient;
		}

		public async Task<byte[]> Decrypt(EncryptionAlgorithm algorithm, byte[] ciphertext) {
			var ea = algorithm == EncryptionAlgorithm.Rsa15 ? Azure.Security.KeyVault.Keys.Cryptography.EncryptionAlgorithm.Rsa15 :
				algorithm == EncryptionAlgorithm.RsaOaep ? Azure.Security.KeyVault.Keys.Cryptography.EncryptionAlgorithm.RsaOaep :
				Azure.Security.KeyVault.Keys.Cryptography.EncryptionAlgorithm.RsaOaep256;
			var result = await this.cryptographyClient.DecryptAsync(ea, ciphertext);
			return result.Plaintext;
		}

		public async Task<byte[]> Encrypt(EncryptionAlgorithm algorithm, byte[] plaintext) {
			var ea = algorithm == EncryptionAlgorithm.Rsa15 ? Azure.Security.KeyVault.Keys.Cryptography.EncryptionAlgorithm.Rsa15 :
				algorithm == EncryptionAlgorithm.RsaOaep ? Azure.Security.KeyVault.Keys.Cryptography.EncryptionAlgorithm.RsaOaep :
				Azure.Security.KeyVault.Keys.Cryptography.EncryptionAlgorithm.RsaOaep256;
			var result = await this.cryptographyClient.EncryptAsync(ea, plaintext);
			return result.Ciphertext;
		}

		public async Task<byte[]> Sign(SignatureAlgorithm algorithm, byte[] data) {
			var sa = algorithm == SignatureAlgorithm.EccP256 ? Azure.Security.KeyVault.Keys.Cryptography.SignatureAlgorithm.ES256 :
				algorithm == SignatureAlgorithm.EccP256K ? Azure.Security.KeyVault.Keys.Cryptography.SignatureAlgorithm.ES256K :
				algorithm == SignatureAlgorithm.EccP384 ? Azure.Security.KeyVault.Keys.Cryptography.SignatureAlgorithm.ES384 :
				algorithm == SignatureAlgorithm.EccP512 ? Azure.Security.KeyVault.Keys.Cryptography.SignatureAlgorithm.ES512 :
				algorithm == SignatureAlgorithm.Rsa256 ? Azure.Security.KeyVault.Keys.Cryptography.SignatureAlgorithm.RS256 :
				algorithm == SignatureAlgorithm.Rsa384 ? Azure.Security.KeyVault.Keys.Cryptography.SignatureAlgorithm.RS384 :
				algorithm == SignatureAlgorithm.Rsa512 ? Azure.Security.KeyVault.Keys.Cryptography.SignatureAlgorithm.RS512 :
				algorithm == SignatureAlgorithm.RsaSsaPss256 ? Azure.Security.KeyVault.Keys.Cryptography.SignatureAlgorithm.PS256 :
				algorithm == SignatureAlgorithm.RsaSsaPss384 ? Azure.Security.KeyVault.Keys.Cryptography.SignatureAlgorithm.PS384 :
				Azure.Security.KeyVault.Keys.Cryptography.SignatureAlgorithm.PS512;

			var result = await this.cryptographyClient.SignAsync(sa, data);
			return result.Signature;
		}

		public async Task<byte[]> SignData(SignatureAlgorithm algorithm, byte[] data) {
			var sa = algorithm == SignatureAlgorithm.EccP256 ? Azure.Security.KeyVault.Keys.Cryptography.SignatureAlgorithm.ES256 :
				algorithm == SignatureAlgorithm.EccP256K ? Azure.Security.KeyVault.Keys.Cryptography.SignatureAlgorithm.ES256K :
				algorithm == SignatureAlgorithm.EccP384 ? Azure.Security.KeyVault.Keys.Cryptography.SignatureAlgorithm.ES384 :
				algorithm == SignatureAlgorithm.EccP512 ? Azure.Security.KeyVault.Keys.Cryptography.SignatureAlgorithm.ES512 :
				algorithm == SignatureAlgorithm.Rsa256 ? Azure.Security.KeyVault.Keys.Cryptography.SignatureAlgorithm.RS256 :
				algorithm == SignatureAlgorithm.Rsa384 ? Azure.Security.KeyVault.Keys.Cryptography.SignatureAlgorithm.RS384 :
				algorithm == SignatureAlgorithm.Rsa512 ? Azure.Security.KeyVault.Keys.Cryptography.SignatureAlgorithm.RS512 :
				algorithm == SignatureAlgorithm.RsaSsaPss256 ? Azure.Security.KeyVault.Keys.Cryptography.SignatureAlgorithm.PS256 :
				algorithm == SignatureAlgorithm.RsaSsaPss384 ? Azure.Security.KeyVault.Keys.Cryptography.SignatureAlgorithm.PS384 :
				Azure.Security.KeyVault.Keys.Cryptography.SignatureAlgorithm.PS512;

			var result = await this.cryptographyClient.SignDataAsync(sa, data);
			return result.Signature;
		}

		public async Task<byte[]> SignData(SignatureAlgorithm algorithm, Stream data) {
			var sa = algorithm == SignatureAlgorithm.EccP256 ? Azure.Security.KeyVault.Keys.Cryptography.SignatureAlgorithm.ES256 :
				algorithm == SignatureAlgorithm.EccP256K ? Azure.Security.KeyVault.Keys.Cryptography.SignatureAlgorithm.ES256K :
				algorithm == SignatureAlgorithm.EccP384 ? Azure.Security.KeyVault.Keys.Cryptography.SignatureAlgorithm.ES384 :
				algorithm == SignatureAlgorithm.EccP512 ? Azure.Security.KeyVault.Keys.Cryptography.SignatureAlgorithm.ES512 :
				algorithm == SignatureAlgorithm.Rsa256 ? Azure.Security.KeyVault.Keys.Cryptography.SignatureAlgorithm.RS256 :
				algorithm == SignatureAlgorithm.Rsa384 ? Azure.Security.KeyVault.Keys.Cryptography.SignatureAlgorithm.RS384 :
				algorithm == SignatureAlgorithm.Rsa512 ? Azure.Security.KeyVault.Keys.Cryptography.SignatureAlgorithm.RS512 :
				algorithm == SignatureAlgorithm.RsaSsaPss256 ? Azure.Security.KeyVault.Keys.Cryptography.SignatureAlgorithm.PS256 :
				algorithm == SignatureAlgorithm.RsaSsaPss384 ? Azure.Security.KeyVault.Keys.Cryptography.SignatureAlgorithm.PS384 :
				Azure.Security.KeyVault.Keys.Cryptography.SignatureAlgorithm.PS512;

			var result = await this.cryptographyClient.SignDataAsync(sa, data);
			return result.Signature;
		}

		public async Task<byte[]> Unwrap(KeyWrapAlgorithm algorithm, byte[] ciphertext) {
			var kwa = algorithm == KeyWrapAlgorithm.Aes128 ? Azure.Security.KeyVault.Keys.Cryptography.KeyWrapAlgorithm.A128KW :
				algorithm == KeyWrapAlgorithm.Aes192 ? Azure.Security.KeyVault.Keys.Cryptography.KeyWrapAlgorithm.A192KW :
				algorithm == KeyWrapAlgorithm.Aes256 ? Azure.Security.KeyVault.Keys.Cryptography.KeyWrapAlgorithm.A256KW :
				algorithm == KeyWrapAlgorithm.Rsa15 ? Azure.Security.KeyVault.Keys.Cryptography.KeyWrapAlgorithm.Rsa15 :
				algorithm == KeyWrapAlgorithm.RsaOaep ? Azure.Security.KeyVault.Keys.Cryptography.KeyWrapAlgorithm.RsaOaep :
				Azure.Security.KeyVault.Keys.Cryptography.KeyWrapAlgorithm.RsaOaep256;

			var result = await this.cryptographyClient.UnwrapKeyAsync(kwa, ciphertext);
			return result.Key;
		}

		public async Task<bool> Verify(SignatureAlgorithm algorithm, byte[] data, byte[] signature) {
			var sa = algorithm == SignatureAlgorithm.EccP256 ? Azure.Security.KeyVault.Keys.Cryptography.SignatureAlgorithm.ES256 :
				algorithm == SignatureAlgorithm.EccP256K ? Azure.Security.KeyVault.Keys.Cryptography.SignatureAlgorithm.ES256K :
				algorithm == SignatureAlgorithm.EccP384 ? Azure.Security.KeyVault.Keys.Cryptography.SignatureAlgorithm.ES384 :
				algorithm == SignatureAlgorithm.EccP512 ? Azure.Security.KeyVault.Keys.Cryptography.SignatureAlgorithm.ES512 :
				algorithm == SignatureAlgorithm.Rsa256 ? Azure.Security.KeyVault.Keys.Cryptography.SignatureAlgorithm.RS256 :
				algorithm == SignatureAlgorithm.Rsa384 ? Azure.Security.KeyVault.Keys.Cryptography.SignatureAlgorithm.RS384 :
				algorithm == SignatureAlgorithm.Rsa512 ? Azure.Security.KeyVault.Keys.Cryptography.SignatureAlgorithm.RS512 :
				algorithm == SignatureAlgorithm.RsaSsaPss256 ? Azure.Security.KeyVault.Keys.Cryptography.SignatureAlgorithm.PS256 :
				algorithm == SignatureAlgorithm.RsaSsaPss384 ? Azure.Security.KeyVault.Keys.Cryptography.SignatureAlgorithm.PS384 :
				Azure.Security.KeyVault.Keys.Cryptography.SignatureAlgorithm.PS512;

			var result = await this.cryptographyClient.VerifyAsync(sa, data, signature);
			return result.IsValid;
		}

		public async Task<bool> VerifyData(SignatureAlgorithm algorithm, byte[] data, byte[] signature) {
			var sa = algorithm == SignatureAlgorithm.EccP256 ? Azure.Security.KeyVault.Keys.Cryptography.SignatureAlgorithm.ES256 :
				algorithm == SignatureAlgorithm.EccP256K ? Azure.Security.KeyVault.Keys.Cryptography.SignatureAlgorithm.ES256K :
				algorithm == SignatureAlgorithm.EccP384 ? Azure.Security.KeyVault.Keys.Cryptography.SignatureAlgorithm.ES384 :
				algorithm == SignatureAlgorithm.EccP512 ? Azure.Security.KeyVault.Keys.Cryptography.SignatureAlgorithm.ES512 :
				algorithm == SignatureAlgorithm.Rsa256 ? Azure.Security.KeyVault.Keys.Cryptography.SignatureAlgorithm.RS256 :
				algorithm == SignatureAlgorithm.Rsa384 ? Azure.Security.KeyVault.Keys.Cryptography.SignatureAlgorithm.RS384 :
				algorithm == SignatureAlgorithm.Rsa512 ? Azure.Security.KeyVault.Keys.Cryptography.SignatureAlgorithm.RS512 :
				algorithm == SignatureAlgorithm.RsaSsaPss256 ? Azure.Security.KeyVault.Keys.Cryptography.SignatureAlgorithm.PS256 :
				algorithm == SignatureAlgorithm.RsaSsaPss384 ? Azure.Security.KeyVault.Keys.Cryptography.SignatureAlgorithm.PS384 :
				Azure.Security.KeyVault.Keys.Cryptography.SignatureAlgorithm.PS512;

			var result = await this.cryptographyClient.VerifyDataAsync(sa, data, signature);
			return result.IsValid;
		}

		public async Task<bool> VerifyData(SignatureAlgorithm algorithm, Stream data, byte[] signature) {
			var sa = algorithm == SignatureAlgorithm.EccP256 ? Azure.Security.KeyVault.Keys.Cryptography.SignatureAlgorithm.ES256 :
				algorithm == SignatureAlgorithm.EccP256K ? Azure.Security.KeyVault.Keys.Cryptography.SignatureAlgorithm.ES256K :
				algorithm == SignatureAlgorithm.EccP384 ? Azure.Security.KeyVault.Keys.Cryptography.SignatureAlgorithm.ES384 :
				algorithm == SignatureAlgorithm.EccP512 ? Azure.Security.KeyVault.Keys.Cryptography.SignatureAlgorithm.ES512 :
				algorithm == SignatureAlgorithm.Rsa256 ? Azure.Security.KeyVault.Keys.Cryptography.SignatureAlgorithm.RS256 :
				algorithm == SignatureAlgorithm.Rsa384 ? Azure.Security.KeyVault.Keys.Cryptography.SignatureAlgorithm.RS384 :
				algorithm == SignatureAlgorithm.Rsa512 ? Azure.Security.KeyVault.Keys.Cryptography.SignatureAlgorithm.RS512 :
				algorithm == SignatureAlgorithm.RsaSsaPss256 ? Azure.Security.KeyVault.Keys.Cryptography.SignatureAlgorithm.PS256 :
				algorithm == SignatureAlgorithm.RsaSsaPss384 ? Azure.Security.KeyVault.Keys.Cryptography.SignatureAlgorithm.PS384 :
				Azure.Security.KeyVault.Keys.Cryptography.SignatureAlgorithm.PS512;

			var result = await this.cryptographyClient.VerifyDataAsync(sa, data, signature);
			return result.IsValid;
		}

		public async Task<byte[]> Wrap(KeyWrapAlgorithm algorithm, byte[] plaintext) {
			var kwa = algorithm == KeyWrapAlgorithm.Aes128 ? Azure.Security.KeyVault.Keys.Cryptography.KeyWrapAlgorithm.A128KW :
				algorithm == KeyWrapAlgorithm.Aes192 ? Azure.Security.KeyVault.Keys.Cryptography.KeyWrapAlgorithm.A192KW :
				algorithm == KeyWrapAlgorithm.Aes256 ? Azure.Security.KeyVault.Keys.Cryptography.KeyWrapAlgorithm.A256KW :
				algorithm == KeyWrapAlgorithm.Rsa15 ? Azure.Security.KeyVault.Keys.Cryptography.KeyWrapAlgorithm.Rsa15 :
				algorithm == KeyWrapAlgorithm.RsaOaep ? Azure.Security.KeyVault.Keys.Cryptography.KeyWrapAlgorithm.RsaOaep :
				Azure.Security.KeyVault.Keys.Cryptography.KeyWrapAlgorithm.RsaOaep256;

			var result = await this.cryptographyClient.UnwrapKeyAsync(kwa, plaintext);
			return result.Key;
		}
	}
}
