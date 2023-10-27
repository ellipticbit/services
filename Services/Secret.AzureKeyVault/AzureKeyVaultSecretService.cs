//-----------------------------------------------------------------------------
// Copyright (c) 2020 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using Azure.Security.KeyVault.Secrets;
using System;
using System.Threading.Tasks;

namespace EllipticBit.Services.Secret
{
	public class AzureKeyVaultSecretService : ISecretService
	{
		private readonly SecretClient secrets;

		public AzureKeyVaultSecretService(SecretClient secrets) {
			this.secrets = secrets;
		}

		public Task Delete(string secretId) {
			return secrets.StartDeleteSecretAsync(secretId);
		}

		public async Task<string> Get(string secretId) {
			var result = await secrets.GetSecretAsync(secretId);
			return result.Value.Value;
		}

		public Task Set(string secretId, string value) {
			return secrets.SetSecretAsync(new KeyVaultSecret(secretId, value));
		}

		public async Task Set(string secretId, string value, DateTimeOffset expiration) {
			await secrets.SetSecretAsync(new KeyVaultSecret(secretId, value));
			await secrets.UpdateSecretPropertiesAsync(new SecretProperties(secretId) { ExpiresOn = expiration });
		}
	}
}
