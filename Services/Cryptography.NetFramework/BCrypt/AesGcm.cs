//----------------------------------------------------------------
// Copyright (c) 2024 EllipticBit, LLC - All Rights Reserved.
//----------------------------------------------------------------

using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

using Vanara.InteropServices;
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
			BCrypt.BCryptGenerateSymmetricKey(phAlgorithm, out BCrypt.SafeBCRYPT_KEY_HANDLE pk, null, 0, key.ToArray(), (uint)key.Length);
			this.phKey = pk;
			byte[] bcm = Encoding.Unicode.GetBytes(BCrypt.ChainingMode.BCRYPT_CHAIN_MODE_GCM);
			BCrypt.BCryptSetProperty(new BCrypt.BCRYPT_HANDLE(phAlgorithm.DangerousGetHandle()), "ChainingMode", bcm, (uint)bcm.Length);
		}

		public void Encrypt(ReadOnlySpan<byte> iv, ReadOnlySpan<byte> plainText, Span<byte> cipherText, Span<byte> tag, ReadOnlySpan<byte> associatedData = default) {
			if (tag.Length != tagSizeInBytes) throw new ArgumentOutOfRangeException(nameof(tag), "Tag buffer is not 16 bytes.");
			byte[] output = new byte[plainText.Length];
			NTStatus result;

			unsafe
			{
				uint outputLen = 0;
				fixed (byte* pIv = iv)
				fixed (byte* pOutput = output)
				fixed (byte* pPlain = plainText)
				fixed (byte* pTag = tag)
				fixed (byte* pAd = associatedData) {
					BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO sinfo = new BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO() {
						cbSize = (uint)sizeof(BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO),
						dwInfoVersion = 1,
						pbNonce = new IntPtr(pIv),
						cbNonce = (uint)iv.Length,
						pbAuthData = new IntPtr(pAd),
						cbAuthData = (uint)associatedData.Length,
						pbTag = new IntPtr(pTag),
						cbTag = (uint)tag.Length,
						pbMacContext = new IntPtr(0),
						cbMacContext = 0,
						cbAAD = 0,
						cbData = 0,
						dwFlags = 0
					};
					IntPtr info = Marshal.AllocHGlobal(sizeof(BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO));
					Marshal.StructureToPtr(sinfo, info, false);

					result = BCrypt.BCryptEncrypt(phKey, plainText.ToArray(), (uint)plainText.Length, info, new IntPtr(pIv), (uint)iv.Length, new IntPtr(pOutput), (uint)output.Length, out outputLen, 0);

					Marshal.FreeHGlobal(info);
				}

				result.ThrowIfFailed();
				cipherText = outputLen == output.Length ? output : output.Take((int)outputLen).ToArray();
			}
		}

		public void Decrypt(ReadOnlySpan<byte> iv, ReadOnlySpan<byte> cipherText, ReadOnlySpan<byte> tag, Span<byte> plainText, ReadOnlySpan<byte> associatedData = default) {
			if (tag.Length != tagSizeInBytes) throw new ArgumentOutOfRangeException(nameof(tag), "Tag buffer is not 16 bytes.");
			byte[] output = new byte[plainText.Length];
			NTStatus result;

			unsafe
			{
				uint outputLen = 0;
				fixed (byte* pIv = iv)
				fixed (byte* pOutput = output)
				fixed (byte* pPlain = plainText)
				fixed (byte* pTag = tag)
				fixed (byte* pAd = associatedData)
				{
					BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO sinfo = new BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO()
					{
						cbSize = (uint)sizeof(BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO),
						dwInfoVersion = 1,
						pbNonce = new IntPtr(pIv),
						cbNonce = (uint)iv.Length,
						pbAuthData = new IntPtr(pAd),
						cbAuthData = (uint)associatedData.Length,
						pbTag = new IntPtr(pTag),
						cbTag = (uint)tag.Length,
						pbMacContext = new IntPtr(0),
						cbMacContext = 0,
						cbAAD = 0,
						cbData = 0,
						dwFlags = 0
					};
					IntPtr info = Marshal.AllocHGlobal(sizeof(BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO));
					Marshal.StructureToPtr(sinfo, info, false);

					result = BCrypt.BCryptDecrypt(phKey, new SafeByteArray(cipherText.ToArray()), (uint)plainText.Length, info, new SafeByteArray(iv.ToArray()), (uint)iv.Length, new IntPtr(pOutput), (uint)output.Length, out outputLen, 0);

					Marshal.FreeHGlobal(info);
				}

				result.ThrowIfFailed();
				cipherText = outputLen == output.Length ? output : output.Take((int)outputLen).ToArray();
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
