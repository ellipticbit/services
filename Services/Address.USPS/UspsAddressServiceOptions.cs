//-----------------------------------------------------------------------------
// Copyright (c) 2025 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

namespace EllipticBit.Services.Address
{
	using System;
	using System.Threading.Tasks;

	public class UspsAddressServiceOptions
	{
		public Func<string, Task<UspsCredential>> GetCredential { get; }

		public UspsAddressServiceOptions(Func<string, Task<UspsCredential>> getCredential) {
			this.GetCredential = getCredential ?? throw new ArgumentNullException(nameof(getCredential));
		}
	}
}
