//-----------------------------------------------------------------------------
// Copyright (c) 2020 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Keys.Cryptography;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Linq;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace EllipticBit.Services.Secret
{
	public static class Extensions
	{
		public static IServiceCollection AddAzureKeyVault(this IServiceCollection services, IConfiguration config) {
			var akv = config.GetSection("Secret").GetSection("AzureKeyVault");
			if (!akv.Exists()) throw new InvalidCredentialException("Unable to locate Credential configuration for Azure KeyVault");

			var uri = new Uri(akv.GetSection("VaultUri").Value);
			TokenCredential kvcreds = akv.GetSection("IsCertificate").Value.Equals("true", StringComparison.OrdinalIgnoreCase) ?
				(TokenCredential)new ClientCertificateCredential(akv.GetSection("TenantId").Value, akv.GetSection("ClientId").Value, GetKeyVaultCertificate(akv.GetSection("Key").Value)) :
				(TokenCredential)new ClientSecretCredential(akv.GetSection("TenantId").Value, akv.GetSection("ClientId").Value, akv.GetSection("Key").Value);

			services.TryAddSingleton(new SecretClient(uri, kvcreds, new SecretClientOptions(SecretClientOptions.ServiceVersion.V7_1)));
			services.TryAddTransient<ISecretService, AzureKeyVaultSecretService>();

			services.TryAddSingleton(new KeyClient(uri, kvcreds, new KeyClientOptions(KeyClientOptions.ServiceVersion.V7_1)));
			services.TryAddTransient<IKeyService, AzureKeyVaultKeyService>();

			services.TryAddSingleton(new CryptographyClient(uri, kvcreds, new CryptographyClientOptions(CryptographyClientOptions.ServiceVersion.V7_1)));
			services.TryAddTransient<IKeyCryptographyService, AzureKeyVaultKeyCryptographyService>();

			return services;
		}

		public static IServiceCollection AddAzureKeyVaultCached(this IServiceCollection services, IConfiguration config) {
			var akv = config.GetSection("Secret").GetSection("AzureKeyVault");
			if (!akv.Exists()) throw new InvalidCredentialException("Unable to locate Credential configuration for Azure KeyVault");

			var uri = new Uri(akv.GetSection("VaultUri").Value);
			TokenCredential kvcreds = akv.GetSection("IsCertificate").Value.Equals("true", StringComparison.OrdinalIgnoreCase) ?
				(TokenCredential)new ClientCertificateCredential(akv.GetSection("TenantId").Value, akv.GetSection("ClientId").Value, GetKeyVaultCertificate(akv.GetSection("Key").Value)) :
				(TokenCredential)new ClientSecretCredential(akv.GetSection("TenantId").Value, akv.GetSection("ClientId").Value, akv.GetSection("Key").Value);

			services.TryAddSingleton(new SecretClient(uri, kvcreds, new SecretClientOptions(SecretClientOptions.ServiceVersion.V7_1)));
			services.TryAddTransient<ISecretService, AzureKeyVaultCachedSecretService>();

			services.TryAddSingleton(new KeyClient(uri, kvcreds, new KeyClientOptions(KeyClientOptions.ServiceVersion.V7_1)));
			services.TryAddTransient<IKeyService, AzureKeyVaultKeyService>();

			services.TryAddSingleton(new CryptographyClient(uri, kvcreds, new CryptographyClientOptions(CryptographyClientOptions.ServiceVersion.V7_1)));
			services.TryAddTransient<IKeyCryptographyService, AzureKeyVaultKeyCryptographyService>();

			return services;
		}

		private static X509Certificate2 GetKeyVaultCertificate(string thumbprint) {
			using (X509Store certStore = new X509Store(StoreName.My, StoreLocation.CurrentUser)) {
				certStore.Open(OpenFlags.ReadOnly);

				X509Certificate2Collection certCollection = certStore.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, true);
				X509Certificate2 cert = certCollection.OfType<X509Certificate2>().FirstOrDefault();

				if (cert is null) {
					throw new ArgumentException($"Certificate with thumbprint {thumbprint} was not found", nameof(thumbprint));
				}

				return cert;
			}
		}
	}
}
