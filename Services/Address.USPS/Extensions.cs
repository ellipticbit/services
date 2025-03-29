//-----------------------------------------------------------------------------
// Copyright (c) 2020-2025 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

namespace EllipticBit.Services.Address
{
	using System;
	using System.Text.Json;
	using EllipticBit.Coalescence.Request;
	using EllipticBit.Coalescence.Shared;
	using Microsoft.Extensions.DependencyInjection.Extensions;
	using Microsoft.Extensions.DependencyInjection;

	public static class Extensions
	{
		public static void AddAddressUsps(this IServiceCollection services, UspsAddressServiceOptions options) {
			string endpoint = "https://apis.usps.com/";
			if (options.UseTestingEndpoint) endpoint = "https://apis-tem.usps.com/";

			services.AddHttpClient("USPS").ConfigureHttpClient((client) => {
				client.BaseAddress = new Uri(endpoint);
			});
			services.AddCoalescenceServices().AddCoalescenceRequestOptions("USPS", new CoalescenceRequestOptions("USPS", "USPS", JsonSerializerOptions.Web));
			services.AddCoalescenceRequestServices();

			services.TryAddTransient<IAddressService, UspsAddressService>();
			services.AddTransient<ICoalescenceAuthentication, UspsAuthenticationHandler>();
			services.TryAddSingleton(options);
		}
	}
}