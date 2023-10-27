//-----------------------------------------------------------------------------
// Copyright (c) 2020 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using Azure.Identity;
using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Keys.Cryptography;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace EllipticBit.Services.Secret
{
	public class AzureKeyVaultKeyService : IKeyService
	{
		private readonly KeyClient keyClient;

		public AzureKeyVaultKeyService(KeyClient keyClient) {
			this.keyClient = keyClient;
		}

		public async Task<ECDsa> CreateEcc(string keyId, CreateKeyProperties properties) {
			var ko = new CreateEcKeyOptions(keyId, properties.HardwareProtected) { ExpiresOn = properties.ExpiresOn, NotBefore = properties.NotBefore };

			if (properties.Curve == KeyCurve.P256) ko.CurveName = KeyCurveName.P256;
			else if (properties.Curve == KeyCurve.P256K) ko.CurveName = KeyCurveName.P256K;
			else if (properties.Curve == KeyCurve.P384) ko.CurveName = KeyCurveName.P384;
			else if (properties.Curve == KeyCurve.P521) ko.CurveName = KeyCurveName.P521;
			else throw new ArgumentException("Invalid Curve selected.", "Curve");

			if (properties.Operations.HasFlag(KeyOperation.Encrypt)) ko.KeyOperations.Add(Azure.Security.KeyVault.Keys.KeyOperation.Encrypt);
			else if (properties.Operations.HasFlag(KeyOperation.Decrypt)) ko.KeyOperations.Add(Azure.Security.KeyVault.Keys.KeyOperation.Decrypt);
			else if (properties.Operations.HasFlag(KeyOperation.Sign)) ko.KeyOperations.Add(Azure.Security.KeyVault.Keys.KeyOperation.Sign);
			else if (properties.Operations.HasFlag(KeyOperation.Verify)) ko.KeyOperations.Add(Azure.Security.KeyVault.Keys.KeyOperation.Verify);
			else if (properties.Operations.HasFlag(KeyOperation.Wrap)) ko.KeyOperations.Add(Azure.Security.KeyVault.Keys.KeyOperation.WrapKey);
			else if (properties.Operations.HasFlag(KeyOperation.Unwrap)) ko.KeyOperations.Add(Azure.Security.KeyVault.Keys.KeyOperation.UnwrapKey);

			var key = await keyClient.CreateEcKeyAsync(ko);
			return key.Value.Key.ToECDsa(true);
		}

		public async Task<RSA> CreateRsa(string keyId, CreateKeyProperties properties) {
			var ko = new CreateRsaKeyOptions(keyId, properties.HardwareProtected) { ExpiresOn = properties.ExpiresOn, NotBefore = properties.NotBefore, KeySize = properties.KeyLength };

			if (properties.Operations.HasFlag(KeyOperation.Encrypt)) ko.KeyOperations.Add(Azure.Security.KeyVault.Keys.KeyOperation.Encrypt);
			else if (properties.Operations.HasFlag(KeyOperation.Decrypt)) ko.KeyOperations.Add(Azure.Security.KeyVault.Keys.KeyOperation.Decrypt);
			else if (properties.Operations.HasFlag(KeyOperation.Sign)) ko.KeyOperations.Add(Azure.Security.KeyVault.Keys.KeyOperation.Sign);
			else if (properties.Operations.HasFlag(KeyOperation.Verify)) ko.KeyOperations.Add(Azure.Security.KeyVault.Keys.KeyOperation.Verify);
			else if (properties.Operations.HasFlag(KeyOperation.Wrap)) ko.KeyOperations.Add(Azure.Security.KeyVault.Keys.KeyOperation.WrapKey);
			else if (properties.Operations.HasFlag(KeyOperation.Unwrap)) ko.KeyOperations.Add(Azure.Security.KeyVault.Keys.KeyOperation.UnwrapKey);

			var key = await keyClient.CreateRsaKeyAsync(ko);
			return key.Value.Key.ToRSA(true);
		}

		public async Task<Aes> CreateSymmetric(string keyId, CreateKeyProperties properties) {
			var ko = new CreateKeyOptions() { ExpiresOn = properties.ExpiresOn, NotBefore = properties.NotBefore };

			if (properties.Operations.HasFlag(KeyOperation.Encrypt)) ko.KeyOperations.Add(Azure.Security.KeyVault.Keys.KeyOperation.Encrypt);
			else if (properties.Operations.HasFlag(KeyOperation.Decrypt)) ko.KeyOperations.Add(Azure.Security.KeyVault.Keys.KeyOperation.Decrypt);
			else if (properties.Operations.HasFlag(KeyOperation.Sign)) ko.KeyOperations.Add(Azure.Security.KeyVault.Keys.KeyOperation.Sign);
			else if (properties.Operations.HasFlag(KeyOperation.Verify)) ko.KeyOperations.Add(Azure.Security.KeyVault.Keys.KeyOperation.Verify);
			else if (properties.Operations.HasFlag(KeyOperation.Wrap)) ko.KeyOperations.Add(Azure.Security.KeyVault.Keys.KeyOperation.WrapKey);
			else if (properties.Operations.HasFlag(KeyOperation.Unwrap)) ko.KeyOperations.Add(Azure.Security.KeyVault.Keys.KeyOperation.UnwrapKey);

			var key = await keyClient.CreateKeyAsync(keyId, KeyType.Oct, ko);
			return key.Value.Key.ToAes();
		}

		public Task Delete(string keyId) {
			return keyClient.StartDeleteKeyAsync(keyId);
		}

		public async Task<ECDsa> GetEcc(string keyId) {
			var key = await keyClient.GetKeyAsync(keyId);
			return key.Value.Key.ToECDsa(true);
		}

		public async Task<IKeyCryptographyService> GetKeyCryptographyService(string keyId) {
			var key = await keyClient.GetKeyAsync(keyId);
			return new AzureKeyVaultKeyCryptographyService(new CryptographyClient(key.Value.Id, new DefaultAzureCredential(), new CryptographyClientOptions(CryptographyClientOptions.ServiceVersion.V7_1)));
		}

		public async Task<KeyProperties> GetProperties(string keyId) {
			var key = await keyClient.GetKeyAsync(keyId);
			return new AzureKeyVaultKeyProperties(key.Value.Properties);
		}

		public async Task<RSA> GetRsa(string keyId) {
			var key = await keyClient.GetKeyAsync(keyId);
			return key.Value.Key.ToRSA(true);
		}

		public async Task<Aes> GetSymmetric(string keyId) {
			var key = await keyClient.GetKeyAsync(keyId);
			return key.Value.Key.ToAes();
		}
	}
}
