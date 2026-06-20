//-----------------------------------------------------------------------------
// Copyright (c) 2020-2026 EllipticBit, LLC All Rights Reserved.
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
			var tenantId = akv.GetSection("TenantId").Value;
			var clientId = akv.GetSection("ClientId").Value;

			TokenCredential kvcreds;
			if (!string.IsNullOrEmpty(akv.GetSection("CertificateFilePath").Value)) {
				kvcreds = new ClientCertificateCredential(tenantId, clientId, new X509Certificate2(akv.GetSection("CertificateFilePath").Value));
			} else if (!string.IsNullOrEmpty(akv.GetSection("KeyFilePath").Value)) {
				kvcreds = new ClientSecretCredential(tenantId, clientId, System.IO.File.ReadAllText(akv.GetSection("KeyFilePath").Value).Trim());
			} else {
				kvcreds = new ClientSecretCredential(tenantId, clientId, akv.GetSection("Key").Value);
			}

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
			var tenantId = akv.GetSection("TenantId").Value;
			var clientId = akv.GetSection("ClientId").Value;

			TokenCredential kvcreds;
			if (!string.IsNullOrEmpty(akv.GetSection("CertificateFilePath").Value)) {
				kvcreds = new ClientCertificateCredential(tenantId, clientId, new X509Certificate2(akv.GetSection("CertificateFilePath").Value));
			} else if (!string.IsNullOrEmpty(akv.GetSection("KeyFilePath").Value)) {
				kvcreds = new ClientSecretCredential(tenantId, clientId, System.IO.File.ReadAllText(akv.GetSection("KeyFilePath").Value).Trim());
			} else {
				kvcreds = new ClientSecretCredential(tenantId, clientId, akv.GetSection("Key").Value);
			}

			services.TryAddSingleton(new SecretClient(uri, kvcreds, new SecretClientOptions(SecretClientOptions.ServiceVersion.V7_1)));
			services.TryAddTransient<ISecretService, AzureKeyVaultCachedSecretService>();

			services.TryAddSingleton(new KeyClient(uri, kvcreds, new KeyClientOptions(KeyClientOptions.ServiceVersion.V7_1)));
			services.TryAddTransient<IKeyService, AzureKeyVaultKeyService>();

			services.TryAddSingleton(new CryptographyClient(uri, kvcreds, new CryptographyClientOptions(CryptographyClientOptions.ServiceVersion.V7_1)));
			services.TryAddTransient<IKeyCryptographyService, AzureKeyVaultKeyCryptographyService>();

			return services;
		}
	}
}
