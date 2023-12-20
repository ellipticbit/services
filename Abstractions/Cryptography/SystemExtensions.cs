using System;
using System.Collections.Generic;
using System.Text;

namespace EllipticBit.Services.Cryptography
{
	/// <summary>
	/// Extensions for System types.
	/// </summary>
	public static class SystemExtensions
	{
		/// <summary>
		/// Calculates the equality of an array of bytes in the constant time.
		/// </summary>
		/// <param name="a">The first byte array to compare.</param>
		/// <param name="b">The second byte array to compare.</param>
		/// <returns>Value indicating if the values are equal.</returns>
		public static bool ConstantTimeEquality(this byte[] a, byte[] b) {
			if (a == null || b == null ||
			    (a.Length != b.Length))
				return false;

			int differentbits = 0;
			for (int i = 0; i < a.Length; ++i)
			{
				differentbits |= a[i] ^ b[i];
			}

			return differentbits == 0;
		}
	}
}
