//-----------------------------------------------------------------------------
// Copyright (c) 2020 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using System.Security.Cryptography;
using System.Threading.Tasks;

namespace EllipticBit.Services.Secret
{
	public interface IKeyService
	{
		Task<RSA> CreateRsa(string keyId, CreateKeyProperties properties);
		Task<ECDsa> CreateEcc(string keyId, CreateKeyProperties properties);
		Task<Aes> CreateSymmetric(string keyId, CreateKeyProperties properties);
		Task<RSA> GetRsa(string keyId);
		Task<ECDsa> GetEcc(string keyId);
		Task<Aes> GetSymmetric(string keyId);
		Task<KeyProperties> GetProperties(string keyId);
		Task<IKeyCryptographyService> GetKeyCryptographyService(string keyId);
		Task Delete(string keyId);
	}
}
