//-----------------------------------------------------------------------------
// Copyright (c) 2023-2024 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;

namespace EllipticBit.Services.Email
{
	public static class ServiceCollectionExtensions
	{
		public static IEmailServiceBuilder AddEmailServices(this IServiceCollection services) {
			services.AddTransient<IEmailServiceFactory, EmailServiceFactory>();
			return new EmailServiceFactory();
		}
	}
}
