using System;
using System.Threading.Tasks;
using EllipticBit.Services.Address;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
	[TestClass]
	public sealed class USPS
	{
		private IAddressService _addressService;

		[TestInitialize]
		public void TestInit()
		{
			var sc = new ServiceCollection();
			sc.AddAddressUsps(new UspsAddressServiceOptions((tenantId) => {
				return Task.FromResult(new UspsCredential(Environment.GetEnvironmentVariable("USPS_KEY"), Environment.GetEnvironmentVariable("USPS_SECRET")));
			}));
			var scc = sc.BuildServiceProvider();
			_addressService = scc.GetRequiredService<IAddressService>();
		}

		[TestMethod]
		public async Task TestAddressCorrection()
		{
			var result = await _addressService.GetAddress(new Address()
			{
				Address1 = "3000 Poberezny Rd",
				City = "Oshkosh",
				Region = "WI"
			});

			Assert.IsNotNull(result);
			Assert.AreEqual("OSHKOSH", result.City);
			Assert.AreEqual("54902", result.PostalCode);
		}

		[TestMethod]
		public async Task TestGetPostalCode()
		{
			var result = await _addressService.GetPostalCode("3000 Poberezny Rd", "Oshkosh", "WI");

			Assert.IsNotNull(result);
			Assert.AreEqual("54902-8939", result);
		}

		[TestMethod]
		public async Task TestCityRegion()
		{
			var result = await _addressService.GetCityRegion("54902");

			Assert.IsNotNull(result);
			Assert.AreEqual("OSHKOSH", result.City);
			Assert.AreEqual("WI", result.Region);
			Assert.AreEqual("54902", result.PostalCode);
		}
	}
}
