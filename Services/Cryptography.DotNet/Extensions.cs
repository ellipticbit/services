//----------------------------------------------------------------
// Copyright (c) 2017-2023 EllipticBit, LLC - All Rights Reserved.
//----------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;

namespace EllipticBit.Services.Cryptography
{
	public static class Extensions
	{
		public static IServiceCollection AddCryptographyService(this IServiceCollection services, DotNetCryptographyServiceOptions options = null) {
			services.AddSingleton(options ?? new DotNetCryptographyServiceOptions());
			services.AddTransient<ICryptographyService, DotNetCryptographyService>();
			return services;
		}
	}
}
