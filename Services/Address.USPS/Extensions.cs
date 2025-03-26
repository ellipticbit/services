//-----------------------------------------------------------------------------
// Copyright (c) 2020-2025 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using System;
using System.Text.Json;
using EllipticBit.Coalescence.Request;
using EllipticBit.Coalescence.Shared;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EllipticBit.Services.Address
{
	using System.Threading.Tasks;

	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;

	public static class Extensions
	{
		public static void AddAddressUsps(this IServiceCollection services, IConfiguration config, UspsAddressServiceOptions options)
		{
			var section = config.GetSection("Address").GetSection("USPS");

			string endpoint = "https://apis.usps.com/";
			var ute = section.GetSection("UseTestingEndpoint").Value;
			if (ute?.Equals("true", StringComparison.OrdinalIgnoreCase) ?? false) endpoint = "https://apis-tem.usps.com/";

			services.AddCoalescenceServices().AddCoalescenceRequestOptions("USPS", new CoalescenceRequestOptions("USPS", "USPS", JsonSerializerOptions.Web));

			services.AddHttpClient("USPS").ConfigureHttpClient((client) => {
				client.BaseAddress = new Uri(endpoint);
			});
			services.TryAddTransient<IAddressService, UspsAddressService>();
			services.TryAddTransient<ICoalescenceAuthentication, UspsAuthenticationHandler>();
			services.TryAddSingleton(options);
		}
	}
}