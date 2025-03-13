//-----------------------------------------------------------------------------
// Copyright (c) 2025 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

namespace System.IO
{
	public static class IOExtensions
	{
		public static byte[] ReadAllBytes(this Stream input) {
			input.Position = 0;
			using (MemoryStream ms = new MemoryStream()) {
				input.CopyTo(ms);
				return ms.ToArray();
			}
		}
	}
}
