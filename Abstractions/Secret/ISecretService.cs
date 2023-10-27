//-----------------------------------------------------------------------------
// Copyright (c) 2020 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using System;
using System.Threading.Tasks;

namespace EllipticBit.Services.Secret
{
	public interface ISecretService
	{
		Task<string> Get(string secretId);
		Task Set(string secretId, string value);
		Task Set(string secretId, string value, DateTimeOffset expiration);
		Task Delete(string secretId);
	}
}
