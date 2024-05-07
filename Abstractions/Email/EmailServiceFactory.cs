//-----------------------------------------------------------------------------
// Copyright (c) 2020-2024 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using System;
using System.Collections.Immutable;
using Microsoft.Extensions.DependencyInjection;

namespace EllipticBit.Services.Email
{
	internal class EmailServiceFactory : IEmailServiceFactory , IEmailServiceBuilder
	{
		private static ImmutableDictionary<string, EmailServiceOptions> options = ImmutableDictionary<string, EmailServiceOptions>.Empty;

		private readonly IServiceProvider provider;

		internal EmailServiceFactory() { }

		public EmailServiceFactory(IServiceProvider provider) {
			this.provider = provider;
		}

		public IEmailService Create(string name)
		{
			if (options.TryGetValue(name, out EmailServiceOptions eso))
			{
				return (IEmailService)ActivatorUtilities.CreateInstance(provider, eso.ImplementationType, eso);
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
