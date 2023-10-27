//-----------------------------------------------------------------------------
// Copyright (c) 2020 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace EllipticBit.Services.Storage
{
	public static class Extensions
	{
		public static IServiceCollection AddLocalFileStorage(this IServiceCollection services, IConfiguration configuration) {
			var rp = configuration.GetSection("Storage").GetSection("File").GetSection("RootPath");

			if (!rp.Exists()) {
				throw new ArgumentException("Unable to locate configuration key: Storage:File:RootPath");
			}

			if (!Directory.Exists(rp.Value))
			{
				Directory.CreateDirectory(rp.Value);
			}

			services.AddTransient<ILocalStorageService, LocalFileStorageService>();

			return services;
		}
	}
}
