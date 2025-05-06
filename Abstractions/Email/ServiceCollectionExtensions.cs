//-----------------------------------------------------------------------------
// Copyright (c) 2023-2025 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EllipticBit.Services.Email
{
	public static class ServiceCollectionExtensions
	{
		public static IEmailServiceBuilder<T> AddEmailServices<T>(this IServiceCollection services)
			where T : EmailServiceOptionsBase
		{
			services.TryAddTransient<IEmailServiceFactory, EmailServiceFactory>();
			return new EmailServiceBuilder<T>();
		}
	}
}
