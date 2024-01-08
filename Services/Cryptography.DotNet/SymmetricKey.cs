//----------------------------------------------------------------
// Copyright (c) 2023 EllipticBit, LLC - All Rights Reserved.
//----------------------------------------------------------------

using System;

namespace EllipticBit.Services.Cryptography
{
	internal class SymmetricKey : ICryptographyKey
	{
		public EncryptionAlgorithm Algorithm { get; }

		public byte[] Key { get; }

		public SymmetricKey(byte[] key, EncryptionAlgorithm algorithm) {
			if (key.Length != 16 && (algorithm == EncryptionAlgorithm.AES128CBC || algorithm == EncryptionAlgorithm.AES128CFB || algorithm == EncryptionAlgorithm.AES128GCM)) throw new ArgumentOutOfRangeException("Invalid key size. Key must be 16 bytes.");
			if (key.Length != 24 && (algorithm == EncryptionAlgorithm.AES192CBC || algorithm == EncryptionAlgorithm.AES192CFB || algorithm == EncryptionAlgorithm.AES192GCM)) throw new ArgumentOutOfRangeException("Invalid key size. Key must be 24 bytes.");
			if (key.Length != 32 && (algorithm == EncryptionAlgorithm.AES256CBC || algorithm == EncryptionAlgorithm.AES256CFB || algorithm == EncryptionAlgorithm.AES256GCM || algorithm == EncryptionAlgorithm.ChaCha20Poly1305)) throw new ArgumentOutOfRangeException("Invalid key size. Key must be 32 bytes.");

			this.Key = key;
			this.Algorithm = algorithm;
		}

		public override string ToString() {
			return Convert.ToBase64String(Key);
		}
	}
}
