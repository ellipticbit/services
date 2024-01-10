//----------------------------------------------------------------
// Copyright (c) 2024 EllipticBit, LLC - All Rights Reserved.
//----------------------------------------------------------------

using System.Linq;
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

// ReSharper disable once CheckNamespace
namespace System.Security.Cryptography
{
	internal static class HKDF
	{
		public static byte[] DeriveKey(HashAlgorithmName algorithm, byte[] key, int outputLen, byte[] salt, byte[] info = null) {
			//Perform extraction
			var extractMac = HMAC.Create("HMAC" + algorithm.Name);
			extractMac.Key = salt;
			byte[] extract = extractMac.ComputeHash(key);

			//Perform expansion
			var expandMac = HMAC.Create("HMAC" + algorithm.Name);
			expandMac.Key = extract;

			int outputIndex = 0;
			byte count = 1;
			byte[] output = Array.Empty<byte>();
			byte[] result = Array.Empty<byte>();

			while (outputIndex < outputLen) {
				byte[] buffer = result.Concat(info ?? Array.Empty<byte>()).Concat(new byte[] { count++ }).ToArray();
				result = expandMac.ComputeHash(buffer);
				int bytesToCopy = Math.Min(outputLen - outputIndex, result.Length);
				output = output.Concat(result.Take(bytesToCopy).ToArray()).ToArray();
				outputIndex += bytesToCopy;
			}

			return output;
		}

		private static EllipticBit.Services.Cryptography.HashAlgorithm ToFunc(this HashAlgorithmName func) {
			if (func == HashAlgorithmName.SHA256) return EllipticBit.Services.Cryptography.HashAlgorithm.SHA2_256;
			if (func == HashAlgorithmName.SHA384) return EllipticBit.Services.Cryptography.HashAlgorithm.SHA2_384;
			if (func == HashAlgorithmName.SHA512) return EllipticBit.Services.Cryptography.HashAlgorithm.SHA2_512;
			if (func.Name == "SHA3-256") return EllipticBit.Services.Cryptography.HashAlgorithm.SHA3_256;
			if (func.Name == "SHA3-384") return EllipticBit.Services.Cryptography.HashAlgorithm.SHA3_384;
			if (func.Name == "SHA3-512") return EllipticBit.Services.Cryptography.HashAlgorithm.SHA3_512;
			return EllipticBit.Services.Cryptography.HashAlgorithm.None;
		}
	}
}
