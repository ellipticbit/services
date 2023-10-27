namespace EllipticBit.Services.Address
{
	using System.Net.Http;
	using System.Threading.Tasks;

	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;

	public static class Extensions
	{
		public static async Task AddAddressUsps(this IServiceCollection services, IConfiguration config)
		{
			var section = config.GetSection("Address").GetSection("USPS");
			var uspsUser = section.GetSection("User").Value;

			services.AddTransient<IAddressService>((icc) => new AddressSanitizer(icc.GetRequiredService<IHttpClientFactory>(), uspsUser));
		}
	}
}