//----------------------------------------------------------------
// Copyright (c) 2024 EllipticBit, LLC - All Rights Reserved.
//----------------------------------------------------------------

using System.Runtime.InteropServices;

using static Vanara.PInvoke.BCrypt;
using Vanara.PInvoke;

// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

// ReSharper disable once CheckNamespace
namespace EllipticBit.Services.Cryptography
{
	internal static class PInvoke
	{
		[DllImport(Lib.Bcrypt, SetLastError = false, ExactSpelling = true)]
		[PInvokeData("bcrypt.h", MSDNShortId = "69fe4530-4b7c-40db-a85c-f9dc458735e7")]
		//public static extern unsafe NTStatus BCryptEncrypt(BCRYPT_KEY_HANDLE hKey, IntPtr pbInput, uint cbInput, [Optional] void* pPaddingInfo, [Optional] IntPtr pbIV, [Optional] uint cbIV, [Optional] IntPtr pbOutput, [Optional] uint cbOutput, out uint pcbResult, uint dwFlags);
		public static unsafe extern NTStatus BCryptEncrypt(
			BCRYPT_KEY_HANDLE hKey,
			byte* pbInput,
			int cbInput,
			void* pPaddingInfo,
			byte* pbIV,
			int cbIV,
			byte* pbOutput,
			int cbOutput,
			out int pcbResult,
			uint dwFlags);

		[DllImport(Lib.Bcrypt, SetLastError = false, ExactSpelling = true)]
		[PInvokeData("bcrypt.h", MSDNShortId = "62286f6b-0d57-4691-83fc-2b9a9740af71")]
		//public static extern unsafe NTStatus BCryptDecrypt(BCRYPT_KEY_HANDLE hKey, SafeAllocatedMemoryHandle pbInput, uint cbInput, void* pPaddingInfo, SafeAllocatedMemoryHandle pbIV, uint cbIV, [Optional] IntPtr pbOutput, [Optional] uint cbOutput, out uint pcbResult, EncryptFlags dwFlags);
		public static unsafe extern NTStatus BCryptDecrypt(
			BCRYPT_KEY_HANDLE hKey,
			byte* pbInput,
			int cbInput,
			void* pPaddingInfo,
			byte* pbIV,
			int cbIV,
			byte* pbOutput,
			int cbOutput,
			out int pcbResult,
			uint dwFlags);
	}
}
