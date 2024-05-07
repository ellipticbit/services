//-----------------------------------------------------------------------------
// Copyright (c) 2023-2024 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EllipticBit.Services.Email
{
	public static class ServiceCollectionExtensions
	{
		public static IEmailServiceBuilder AddEmailServices(this IServiceCollection services) {
			services.TryAddTransient<IEmailServiceFactory, EmailServiceFactory>();
			return new EmailServiceFactory();
		}
	}
}
