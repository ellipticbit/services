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
	internal class MacService : ICryptographyMac
	{
		public Task<byte[]> Hmac(byte[] key, Stream data, HashAlgorithm func) {
			if (key == null) throw new ArgumentNullException(nameof(key));
			switch (func) {
				case HashAlgorithm.SHA2_256:
					return HMACSHA256.HashDataAsync(key, data).AsTask();
				case HashAlgorithm.SHA2_384:
					return HMACSHA384.HashDataAsync(key, data).AsTask();
				case HashAlgorithm.SHA2_512:
					return HMACSHA512.HashDataAsync(key, data).AsTask();
				case HashAlgorithm.SHA3_256:
					return HMACSHA3_256.HashDataAsync(key, data).AsTask();
				case HashAlgorithm.SHA3_384:
					return HMACSHA3_384.HashDataAsync(key, data).AsTask();
				case HashAlgorithm.SHA3_512:
					return HMACSHA3_512.HashDataAsync(key, data).AsTask();
				default:
					throw new NotSupportedException("Function 'None' is not supported.");
			}
		}
	}
}
