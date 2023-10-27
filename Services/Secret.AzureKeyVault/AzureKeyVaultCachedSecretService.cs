//-----------------------------------------------------------------------------
// Copyright (c) 2021 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using Azure.Security.KeyVault.Secrets;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Internal;

using System;
using System.Threading.Tasks;

namespace EllipticBit.Services.Secret
{
	public class AzureKeyVaultCachedSecretService : ISecretService
	{
		private readonly SecretClient secrets;
		private readonly MemoryCache cache;
		private readonly TimeSpan cacheItemTimeout;

		public AzureKeyVaultCachedSecretService(SecretClient secrets, IConfiguration config, ISystemClock clock) {
			var akvcc = config.GetSection("Secret").GetSection("AzureKeyVault").GetSection("Cache");
			cacheItemTimeout = TimeSpan.FromSeconds(Convert.ToInt32(akvcc.GetSection("ItemTimeout").Value ?? "3600"));

			this.secrets = secrets;
			this.cache = new MemoryCache(new MemoryCacheOptions() {
				Clock = clock,
				CompactionPercentage = 25,
				ExpirationScanFrequency = TimeSpan.FromSeconds(Convert.ToInt32(akvcc.GetSection("ScanInterval").Value ?? "300")),
				SizeLimit = Convert.ToInt64(akvcc.GetSection("ItemLimit").Value ?? "1024"),
			});
		}

		public Task Delete(string secretId) {
			cache.Remove(secretId);
			return secrets.StartDeleteSecretAsync(secretId);
		}

		public async Task<string> Get(string secretId) {
			if (cache.TryGetValue(secretId, out string value)) {
				return value;
			} else {
				var result = await secrets.GetSecretAsync(secretId);
				cache.Set(secretId, result.Value.Value, cacheItemTimeout);
				return result.Value.Value;
			}
		}

		public Task Set(string secretId, string value) {
			cache.Set(secretId, value, cacheItemTimeout);
			return secrets.SetSecretAsync(new KeyVaultSecret(secretId, value));
		}

		public async Task Set(string secretId, string value, DateTimeOffset expiration) {
			cache.Set(secretId, value, cacheItemTimeout);
			await secrets.SetSecretAsync(new KeyVaultSecret(secretId, value));
			await secrets.UpdateSecretPropertiesAsync(new SecretProperties(secretId) { ExpiresOn = expiration });
		}
	}
}
