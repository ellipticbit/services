//----------------------------------------------------------------
// Copyright (c) 2017-2023 EllipticBit, LLC - All Rights Reserved.
//----------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;

namespace EllipticBit.Services.Cryptography
{
	/// <summary>
	/// Registration Extensions.
	/// </summary>
	public static class Extensions
	{
		/// <summary>
		/// Registers the Cryptography Services.
		/// </summary>
		/// <param name="services">The IServiceCollection to register the cryptography services.</param>
		/// <returns>The IServiceCollection.</returns>
		public static IServiceCollection AddCryptographyServices(this IServiceCollection services) {
			services.AddTransient<ICryptographyService, CryptographyService>();
			services.AddTransient<ICryptographyHash, HashService>();
			services.AddTransient<ICryptographyMac, MacService>();
			services.AddTransient<ICryptographyKdf, KdfService>();
			services.AddTransient<ICryptographySymmetric, SymmetricService>();
			return services;
		}
	}
}
