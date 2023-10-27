//----------------------------------------------------------------
// Copyright (c) 2017-2023 EllipticBit, LLC - All Rights Reserved.
//----------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;

namespace EllipticBit.Services.Cryptography
{
	public static class Extensions
	{
		public static IServiceCollection AddCryptographyService(this IServiceCollection services, NetStandardCryptographyServiceOptions options = null) {
			services.AddSingleton(options ?? new NetStandardCryptographyServiceOptions());
			services.AddTransient<ICryptographyService, NetStandardCryptographyService>();
			return services;
		}
	}
}
