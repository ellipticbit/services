//-----------------------------------------------------------------------------
// Copyright (c) 2020 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using System.IO;
using System.Threading.Tasks;

namespace EllipticBit.Services.Secret
{
	public enum EncryptionAlgorithm
	{
		Rsa15,
		RsaOaep,
		RsaOaep256,
	}

	public enum SignatureAlgorithm
	{
		EccP256,
		EccP256K,
		EccP384,
		EccP512,
		RsaSsaPss256,
		RsaSsaPss384,
		RsaSsaPss512,
		Rsa256,
		Rsa384,
		Rsa512,
	}

	public enum KeyWrapAlgorithm
	{
		Aes128,
		Aes192,
		Aes256,
		Rsa15,
		RsaOaep,
		RsaOaep256,
	}

	public interface IKeyCryptographyService
	{
		Task<byte[]> Encrypt(EncryptionAlgorithm algorithm, byte[] plaintext);
		Task<byte[]> Decrypt(EncryptionAlgorithm algorithm, byte[] ciphertext);

		Task<byte[]> Sign(SignatureAlgorithm algorithm, byte[] data);
		Task<byte[]> SignData(SignatureAlgorithm algorithm, byte[] data);
		Task<byte[]> SignData(SignatureAlgorithm algorithm, Stream data);

		Task<bool> Verify(SignatureAlgorithm algorithm, byte[] data, byte[] signature);
		Task<bool> VerifyData(SignatureAlgorithm algorithm, byte[] data, byte[] signature);
		Task<bool> VerifyData(SignatureAlgorithm algorithm, Stream data, byte[] signature);

		Task<byte[]> Wrap(KeyWrapAlgorithm algorithm, byte[] plaintext);
		Task<byte[]> Unwrap(KeyWrapAlgorithm algorithm, byte[] ciphertext);
	}
}
