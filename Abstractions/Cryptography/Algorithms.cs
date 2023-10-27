//-----------------------------------------------------------------------------
// Copyright (c) 2020-2021 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

namespace EllipticBit.Services.Cryptography
{
	public enum EncryptionAlgorithm
	{
		Default,

		AES128CBC,
		AES192CBC,
		AES256CBC,

		AES128GCM,
		AES192GCM,
		AES256GCM,
	}

	public enum HashAlgorithm
	{
		Default,
		None,
		SHA256,
		SHA384,
		SHA512,
	}

	public enum PasswordAlgorithm
	{
		Default = 0,
		PBKDF2 = 1,
		BCrypt = 2,
		Argon2 = 3,
	}
}
