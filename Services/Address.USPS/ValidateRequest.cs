namespace EllipticBit.Services.Address
{
	using System.Collections.Generic;
	using System.Xml.Serialization;

	public class AddressValidateRequest
	{
		[XmlAttribute("USERID")] public string UserId { get; set; }
		[XmlElement("Address")] public List<Address> Address { get; set; } = new List<Address>();
	}
}
