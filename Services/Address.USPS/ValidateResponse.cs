namespace EllipticBit.Services.Address
{
	using System.Collections.Generic;
	using System.Xml.Serialization;

	public class AddressValidateResponse
	{
		[XmlElement("Address")] public List<Address> Address { get; set; } = new List<Address>();
	}
}
