//----------------------------------------------------------------
// Copyright (c) 2017-2023 EllipticBit, LLC - All Rights Reserved.
//----------------------------------------------------------------

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace EllipticBit.Services.Cryptography
{
	public static class Extensions
	{
		public static IServiceCollection AddPasswordHashing<TUser>(this IServiceCollection services) where TUser : class {
			services.AddTransient<IPasswordHasher<TUser>, AspNetCorePasswordHasher<TUser>>();
			return services;
		}
	}
}
