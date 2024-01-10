//----------------------------------------------------------------
// Copyright (c) 2024 EllipticBit, LLC - All Rights Reserved.
//----------------------------------------------------------------

using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

using Vanara.InteropServices;
using Vanara.PInvoke;

// ReSharper disable once CheckNamespace
namespace System.Security.Cryptography
{
	internal class ChaCha20Poly1305 : IDisposable
	{
		private readonly BCrypt.BCRYPT_ALG_HANDLE phAlgorithm;
		private readonly BCrypt.BCRYPT_KEY_HANDLE phKey;

		public ChaCha20Poly1305(ReadOnlySpan<byte> key) {
			var result = BCrypt.BCryptOpenAlgorithmProvider(out BCrypt.SafeBCRYPT_ALG_HANDLE pa, "CHACHA20_POLY1305", BCrypt.KnownProvider.MS_PRIMITIVE_PROVIDER);
			this.phAlgorithm = pa;

			if (result != NTStatus.STATUS_NOT_FOUND) throw new CryptographicException("ChaCha20_Poly1305 is not supported on this Operating System.");

			byte[] bcm = Encoding.Unicode.GetBytes(BCrypt.ChainingMode.BCRYPT_CHAIN_MODE_CCM);
			result =BCrypt.BCryptSetProperty(new BCrypt.BCRYPT_HANDLE(phAlgorithm.DangerousGetHandle()), "ChainingMode", bcm, (uint)bcm.Length);

			result = BCrypt.BCryptGenerateSymmetricKey(phAlgorithm, out BCrypt.SafeBCRYPT_KEY_HANDLE pk, new IntPtr(), 0, key.ToArray(), (uint)key.Length);
			this.phKey = pk;
		}

		public void Encrypt(ReadOnlySpan<byte> iv, ReadOnlySpan<byte> plainText, Span<byte> cipherText, Span<byte> tag, ReadOnlySpan<byte> associatedData = default) {
			if (tag.Length != 16) throw new ArgumentOutOfRangeException(nameof(tag), "Tag buffer does not match the specified size.");
			byte[] output = new byte[plainText.Length];
			NTStatus result;

			unsafe {
				uint outputLen = 0;
				fixed (byte* pIv = iv)
				fixed (byte* pOutput = output)
				fixed (byte* pPlain = plainText)
				fixed (byte* pTag = tag)
				fixed (byte* pAd = associatedData) {
					var sinfo = new BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO() {
						cbSize = (uint)sizeof(BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO),
						dwInfoVersion = 1,
						pbNonce = new IntPtr(pIv),
						cbNonce = (uint)iv.Length,
						pbAuthData = new IntPtr(pAd),
						cbAuthData = (uint)associatedData.Length,
						pbTag = new IntPtr(pTag),
						cbTag = (uint)tag.Length,
						pbMacContext = new IntPtr(),
						cbMacContext = 0,
						cbAAD = 0,
						cbData = 0,
						dwFlags = 0
					};
					IntPtr info = Marshal.AllocHGlobal(sizeof(BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO));
					Marshal.StructureToPtr(sinfo, info, false);

					result = BCrypt.BCryptEncrypt(phKey, plainText.ToArray(), (uint)plainText.Length, info, new IntPtr(), 0, new IntPtr(pOutput), (uint)output.Length, out outputLen, 0);

					Marshal.FreeHGlobal(info);
				}

				result.ThrowIfFailed();
				var to = outputLen == output.Length ? output : output.Take((int)outputLen).ToArray();
				to.CopyTo(cipherText);
			}
		}

		public void Decrypt(ReadOnlySpan<byte> iv, ReadOnlySpan<byte> cipherText, ReadOnlySpan<byte> tag, Span<byte> plainText, ReadOnlySpan<byte> associatedData = default) {
			if (tag.Length != 16) throw new ArgumentOutOfRangeException(nameof(tag), "Tag buffer does not match the specified size.");
			byte[] output = new byte[plainText.Length];
			NTStatus result;

			unsafe
			{
				uint outputLen = 0;
				fixed (byte* pIv = iv)
				fixed (byte* pOutput = output)
				fixed (byte* pCipher = cipherText)
				fixed (byte* pTag = tag)
				fixed (byte* pAd = associatedData)
				{
					var sinfo = new BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO()
					{
						cbSize = (uint)sizeof(BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO),
						dwInfoVersion = 1,
						pbNonce = new IntPtr(pIv),
						cbNonce = (uint)iv.Length,
						pbAuthData = new IntPtr(pAd),
						cbAuthData = (uint)associatedData.Length,
						pbTag = new IntPtr(pTag),
						cbTag = (uint)tag.Length,
						pbMacContext = new IntPtr(),
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
				var to = outputLen == output.Length ? output : output.Take((int)outputLen).ToArray();
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

		~ChaCha20Poly1305() {
			ReleaseUnmanagedResources();
		}
	}
}
