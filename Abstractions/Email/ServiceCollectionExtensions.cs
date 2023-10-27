using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.Extensions.DependencyInjection;

namespace EllipticBit.Services.Email
{
	public static class ServiceCollectionExtensions
	{
		public static IEmailServiceBuilder AddEmailServices(this IServiceCollection services, EmailServiceOptions defaultServiceOptions) {
			services.AddTransient<IEmailServiceFactory, EmailServiceFactory>();
			EmailServiceFactory.defaultOptions = defaultServiceOptions;
			return new EmailServiceFactory();
		}
	}
}
