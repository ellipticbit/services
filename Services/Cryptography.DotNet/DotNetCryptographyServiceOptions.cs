//----------------------------------------------------------------
// Copyright (c) 2017-2023 EllipticBit, LLC - All Rights Reserved.
//----------------------------------------------------------------

using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace EllipticBit.Services.Cryptography
{
	public sealed class DotNetCryptographyServiceOptions
	{
		public EncryptionAlgorithm EncryptionAlgorithm { get; set; }
		public HashAlgorithm HashAlgorithm { get; set; }
		public PasswordAlgorithm PasswordAlgorithm { get; set; }
		public PasswordAlgorithm KeyDerivationAlgorithm { get; set; }

		public byte[] Pepper { get; set; }

		public int PasswordParametersVersion { get; set; }
		public KeyDerivationPrf PBKDF2Algorithm { get; set; }
		public int PBKDF2Iterations { get; set; }
		public int PBKDF2OutputLength { get; set; }
		public int BCryptWorkFactor { get; set; }
		public int Argon2Parallelism { get; set; }
		public int Argon2MemorySize { get; set; }
		public int Argon2Iterations { get; set; }
		public int Argon2OutputLength { get; set; }

		public DotNetCryptographyServiceOptions() {
			EncryptionAlgorithm = EncryptionAlgorithm.AES256GCM;
			HashAlgorithm = HashAlgorithm.SHA384;
			PasswordAlgorithm = PasswordAlgorithm.Argon2;
			KeyDerivationAlgorithm = PasswordAlgorithm.Argon2;

			Pepper = null;

			PasswordParametersVersion = 1;
			PBKDF2Iterations = 100000;
			PBKDF2Algorithm = KeyDerivationPrf.HMACSHA512;
			PBKDF2OutputLength = 64;
			BCryptWorkFactor = 11;
			Argon2Parallelism = 2;
			Argon2MemorySize = 8192;
			Argon2Iterations = 3;
			Argon2OutputLength = 64;
		}
	}
}
