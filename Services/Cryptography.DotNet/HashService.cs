//----------------------------------------------------------------
// Copyright (c) 2023 EllipticBit, LLC - All Rights Reserved.
//----------------------------------------------------------------

using System;
using System.IO;
using System.Threading.Tasks;
#if !NETFRAMEWORK
using System.Security.Cryptography;
#endif

namespace EllipticBit.Services.Cryptography
{
	internal class HashService : ICryptographyHash
	{
		public Task<byte[]> Hash(Stream data, HashAlgorithm func) {
			switch (func) {
				case HashAlgorithm.SHA2_256:
					return SHA256.HashDataAsync(data).AsTask();
				case HashAlgorithm.SHA2_384:
					return SHA384.HashDataAsync(data).AsTask();
				case HashAlgorithm.SHA2_512:
					return SHA512.HashDataAsync(data).AsTask();
				case HashAlgorithm.SHA3_256:
					return SHA3_256.HashDataAsync(data).AsTask();
				case HashAlgorithm.SHA3_384:
					return SHA3_384.HashDataAsync(data).AsTask();
				case HashAlgorithm.SHA3_512:
					return SHA3_512.HashDataAsync(data).AsTask();
				default:
					throw new NotSupportedException("Function 'None' is not supported.");
			}
		}
	}
}
