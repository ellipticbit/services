//-----------------------------------------------------------------------------
// Copyright (c) 2020-2025 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using System;
using System.Collections.Immutable;
using Microsoft.Extensions.DependencyInjection;

namespace EllipticBit.Services.Email
{
	internal class EmailServiceFactory : IEmailServiceFactory
	{
		internal static ImmutableDictionary<string, EmailServiceOptionsBase> options = ImmutableDictionary<string, EmailServiceOptionsBase>.Empty;

		private readonly IServiceProvider provider;

		internal EmailServiceFactory() { }

		public EmailServiceFactory(IServiceProvider provider) {
			this.provider = provider;
		}

		public IEmailService Create(string name) {
			if (options.TryGetValue(name, out EmailServiceOptionsBase eso)) {
				return (IEmailService)ActivatorUtilities.CreateInstance(provider, eso.ImplementationType, eso);
			}

			throw new ArgumentOutOfRangeException(nameof(name), $"Unable to locate Email Service: {name}");
		}
	}
}
