//----------------------------------------------------------------
// Copyright (c) 2024 EllipticBit, LLC - All Rights Reserved.
//----------------------------------------------------------------

using System.Linq;
using System.Text;
using EllipticBit.Services.Cryptography;
using Vanara.PInvoke;

namespace System.Security.Cryptography
{
	internal class AesGcm : IDisposable
	{
		private readonly BCrypt.BCRYPT_ALG_HANDLE phAlgorithm;
		private readonly BCrypt.BCRYPT_KEY_HANDLE phKey;

		private readonly int tagSizeInBytes;

		public AesGcm(ReadOnlySpan<byte> key, int tagSizeInBytes) {
			this.tagSizeInBytes = tagSizeInBytes;
			BCrypt.BCryptOpenAlgorithmProvider(out BCrypt.SafeBCRYPT_ALG_HANDLE pa, "AES", BCrypt.KnownProvider.MS_PRIMITIVE_PROVIDER);
			this.phAlgorithm = pa;

			byte[] bcm = Encoding.Unicode.GetBytes(BCrypt.ChainingMode.BCRYPT_CHAIN_MODE_GCM);
			BCrypt.BCryptSetProperty(new BCrypt.BCRYPT_HANDLE(phAlgorithm.DangerousGetHandle()), "ChainingMode", bcm, (uint)bcm.Length);

			BCrypt.BCryptGenerateSymmetricKey(phAlgorithm, out BCrypt.SafeBCRYPT_KEY_HANDLE pk, new IntPtr(), 0, key.ToArray(), (uint)key.Length);
			this.phKey = pk;
		}

		public void Encrypt(ReadOnlySpan<byte> iv, ReadOnlySpan<byte> plainText, Span<byte> cipherText, Span<byte> tag, ReadOnlySpan<byte> associatedData = default) {
			if (tag.Length != tagSizeInBytes) throw new ArgumentOutOfRangeException(nameof(tag), "Tag buffer is not 16 bytes.");
			byte[] output = new byte[plainText.Length];
			NTStatus result;

			unsafe
			{
				int outputLen = 0;
				fixed (byte* pIv = iv)
				fixed (byte* pOutput = output)
				fixed (byte* pPlain = plainText)
				fixed (byte* pTag = tag)
				fixed (byte* pAd = associatedData) {
					var info = new BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO() {
						cbSize = sizeof(BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO),
						dwInfoVersion = 1,
						pbNonce = pIv,
						cbNonce = iv.Length,
						pbAuthData = pAd,
						cbAuthData = associatedData.Length,
						pbTag = pTag,
						cbTag = tag.Length,
					};
					result = PInvoke.BCryptEncrypt(phKey, pPlain, plainText.Length, &info, null, 0, pOutput, output.Length, out outputLen, 0);
				}

				result.ThrowIfFailed();
				var to = outputLen == output.Length ? output : output.Take(outputLen).ToArray();
				to.CopyTo(cipherText);
			}
		}

		public void Decrypt(ReadOnlySpan<byte> iv, ReadOnlySpan<byte> cipherText, ReadOnlySpan<byte> tag, Span<byte> plainText, ReadOnlySpan<byte> associatedData = default) {
			if (tag.Length != tagSizeInBytes) throw new ArgumentOutOfRangeException(nameof(tag), "Tag buffer is not 16 bytes.");
			byte[] output = new byte[plainText.Length];
			NTStatus result;

			unsafe
			{
				int outputLen = 0;
				fixed (byte* pIv = iv)
				fixed (byte* pOutput = output)
				fixed (byte* pCipher = cipherText)
				fixed (byte* pTag = tag)
				fixed (byte* pAd = associatedData) {
					var info = new BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO() {
						cbSize = sizeof(BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO),
						dwInfoVersion = 1,
						pbNonce = pIv,
						cbNonce = iv.Length,
						pbAuthData = pAd,
						cbAuthData = associatedData.Length,
						pbTag = pTag,
						cbTag = tag.Length,
					};
					result = PInvoke.BCryptDecrypt(phKey, pCipher, plainText.Length, &info, null, 0, pOutput, output.Length, out outputLen, 0);
				}

				result.ThrowIfFailed();
				var to = outputLen == output.Length ? output : output.Take(outputLen).ToArray();
				to.CopyTo(plainText);
			}
		}

		private void ReleaseUnmanagedResources() {
			BCrypt.BCryptDestroyKey(phKey);
			BCrypt.BCryptCloseAlgorithmProvider(phAlgorithm, 0);
		}

		public void Dispose() {
			ReleaseUnmanagedResources();
			GC.SuppressFinalize(this);
		}

		~AesGcm() {
			ReleaseUnmanagedResources();
		}
	}
}
