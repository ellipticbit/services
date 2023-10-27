namespace EllipticBit.Services.Address {
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Net.Http;
	using System.Net.Http.Formatting;
	using System.Net.Http.Headers;
	using System.Threading.Tasks;
	using System.Xml;
	using System.Xml.Serialization;

	public sealed class AddressSanitizer : IAddressService {
		private readonly IHttpClientFactory _httpClient;
		private readonly string _uspsUser;

		public AddressSanitizer(IHttpClientFactory httpClient, string uspsUser) {
			this._httpClient = httpClient;
			this._uspsUser = uspsUser;
		}

		public async Task<List<Address>> Validate(Address address) {
			var xmlser = new XmlSerializer(typeof(AddressValidateRequest));

			var ns = new XmlSerializerNamespaces();
			ns.Add("", "");
			using (var httpClient = this._httpClient.CreateClient())
			using (var sw = new StringWriter())
			using (var xmlw = XmlWriter.Create(sw, new XmlWriterSettings() { Indent = false, OmitXmlDeclaration = true, NewLineHandling = NewLineHandling.None })) {
				xmlser.Serialize(
					xmlw,
					new AddressValidateRequest { Address = new List<Address> { address }, UserId = _uspsUser },
					ns);

				var uri = $"https://secure.shippingapis.com/ShippingAPI.dll?API=Verify&XML={sw}";
				var rm = new HttpRequestMessage(HttpMethod.Get, new Uri(uri));
				rm.Headers.Accept.Clear();
				rm.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
				var rr = await httpClient.SendAsync(rm, HttpCompletionOption.ResponseHeadersRead);
				var res = await rr.Content.ReadAsStringAsync();
				var ret = await rr.Content.ReadAsAsync<AddressValidateResponse>(new MediaTypeFormatter[] { new XmlMediaTypeFormatter() { UseXmlSerializer = true, Indent = false } });
				return ret.Address;
			}
		}
	}
}
