//-----------------------------------------------------------------------------
// Copyright (c) 2020 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using EllipticBit.Services.Secret;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Azure.Storage.Blobs;

namespace EllipticBit.Services.Storage
{
	public static class Extensions
	{
		public static async Task<IServiceCollection> AddAzureBlobStorage(this IServiceCollection services, IConfiguration configuration, ISecretService secretService) {
			var abs = configuration.GetSection("Storage").GetSection("AzureBlob");

			var sid = abs.GetSection("SecretId");
			var ccs = abs.GetSection("Connection");
			string cs = null;

			if (sid.Exists() && secretService != null) {
				cs = await secretService.Get(sid.Value);
			} else if (ccs.Exists()) {
				cs = ccs.Value;
			} else {
				throw new ArgumentException("No secret or connection configuration provided.", "Storage:AzureBlob");
			}

			services.AddSingleton(new BlobServiceClient(cs));
			services.AddTransient<IRemoteStorageService, AzureBlobStorageService>();

			return services;
		}
	}
}
