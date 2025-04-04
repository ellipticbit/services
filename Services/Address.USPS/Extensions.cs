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

		public static bool IsValidUspsZipCode(this Address addr) => (addr.PostalCode?.Trim().Length ?? 0) == 5 && int.TryParse(addr.PostalCode?.Trim(), out var tpc);

		public static bool IsCheckableUspsAddress(this Address addr) {
			if (addr.IsValidUspsZipCode()) {
				return !string.IsNullOrWhiteSpace(addr.Address1) && !string.IsNullOrWhiteSpace(addr.City);
			}

			return !string.IsNullOrWhiteSpace(addr.Address1) && !string.IsNullOrWhiteSpace(addr.City) && !string.IsNullOrWhiteSpace(addr.Region) && addr.Region.Trim().Length == 2;
		}
	}
}