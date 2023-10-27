//-----------------------------------------------------------------------------
// Copyright (c) 2020-2021 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

namespace EllipticBit.Services.Cryptography
{
	public enum VerifyPasswordResult
	{
		Success,
		Failure,
		Rehash,
	}

	public interface ICryptographyService
	{
		bool ConstantTimeEquality(byte[] a, byte[] b);
		byte[] RandomBytes(int bytes);
		byte[] Hash(byte[] data, HashAlgorithm algorithm = HashAlgorithm.Default);
		byte[] Hash(byte[] key, byte[] data, HashAlgorithm algorithm = HashAlgorithm.Default);
		string Encrypt(byte[] key, byte[] data);
		string Encrypt(string key, byte[] data, byte[] salt = null);
		byte[] Decrypt(byte[] key, string data);
		byte[] Decrypt(string key, string data, byte[] salt = null);
		string SecurePasssword(string password, byte[] pepper = null, byte[] associatedData = null);
		VerifyPasswordResult VerifyPasssword(string storedPassword, string suppliedPassword, byte[] pepper = null, byte[] associatedData = null);
		byte[] DeriveKey(string password, byte[] salt, int requiredBytes);
	}
}
