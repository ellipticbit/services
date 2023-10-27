using System;
using System.Collections.Immutable;
using Microsoft.Extensions.DependencyInjection;

namespace EllipticBit.Services.Email
{
	internal class EmailServiceFactory : IEmailServiceFactory , IEmailServiceBuilder
	{
		internal static ImmutableDictionary<string, EmailServiceOptions> options = ImmutableDictionary<string, EmailServiceOptions>.Empty;
		internal static EmailServiceOptions defaultOptions;

		private readonly IServiceProvider provider;

		internal EmailServiceFactory() { }

		public EmailServiceFactory(IServiceProvider provider) {
			this.provider = provider;
		}

		public IEmailService Create(string name = null) {
			if (string.IsNullOrWhiteSpace(name)) {
				return (IEmailService)ActivatorUtilities.CreateInstance(provider, defaultOptions.EmailServiceType, defaultOptions);
			}
			if (options.TryGetValue(name, out EmailServiceOptions eso)) {
				return (IEmailService)ActivatorUtilities.CreateInstance(provider, eso.EmailServiceType, eso);
			}

			throw new ArgumentOutOfRangeException(nameof(name), $"Unable to locate Email Service: {name}");
		}

		public IEmailTemplateService CreateTemplate(string name) {
			if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));

			if (options.TryGetValue(name, out EmailServiceOptions eso)) {
				if (eso.ServiceImplementation == EmailServiceImplementation.Service) throw new ArgumentOutOfRangeException(nameof(name), $"Email Service '{name}' does not implement 'IEmailTemplateService'.");
				return (IEmailTemplateService)ActivatorUtilities.CreateInstance(provider, eso.EmailServiceType, eso);
			}

			throw new ArgumentOutOfRangeException(nameof(name), $"Unable to locate Email Service: {name}");
		}

		public IEmailScheduleService CreateSchedule(string name) {
			if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));

			if (options.TryGetValue(name, out EmailServiceOptions eso))
			{
				if (eso.ServiceImplementation == EmailServiceImplementation.Service || eso.ServiceImplementation == EmailServiceImplementation.Template) throw new ArgumentOutOfRangeException(nameof(name), $"Email Service '{name}' does not implement 'IEmailScheduleService'.");
				return (IEmailScheduleService)ActivatorUtilities.CreateInstance(provider, eso.EmailServiceType, eso);
			}

			throw new ArgumentOutOfRangeException(nameof(name), $"Unable to locate Email Service: {name}");
		}

		//
		// IEmailServiceBuilder implementation
		//

		public IEmailServiceBuilder AddEmailService(string name, EmailServiceOptions value) {
			if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
			if (value == null) throw new ArgumentNullException(nameof(value));
			EmailServiceFactory.options = EmailServiceFactory.options.Add(name, value);
			return this;
		}
	}
}
