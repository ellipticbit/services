//-----------------------------------------------------------------------------
// Copyright (c) 2020 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

namespace EllipticBit.Services.Email
{
	public sealed class EmailAddress
	{
		public string Email { get; }
		public string Name { get; }

		public EmailAddress(string email, string name) {
			this.Email = email;
			this.Name = name;
		}
	}
}
