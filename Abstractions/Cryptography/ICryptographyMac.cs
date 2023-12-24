//-----------------------------------------------------------------------------
// Copyright (c) 2020-2023 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using System.IO;
using System.Threading.Tasks;

namespace EllipticBit.Services.Cryptography
{
	/// <summary>
	/// Contains methods for generating Message Authentication Codes.
	/// </summary>
	public interface ICryptographyMac
	{
		/// <summary>
		/// Generate an HMAC-based authentication code on the data in the specified stream.
		/// </summary>
		/// <param name="key">The key used to MAC the data.</param>
		/// <param name="data">The data to generate an HMAC for.</param>
		/// <param name="func">The Hash function used to generate the MAC.</param>
		/// <returns>A byte array containing the Authentication Code.</returns>
		Task<byte[]> Hmac(byte[] key, Stream data, HashAlgorithm func);
	}

	/// <summary>
	/// Extensions for ICryptographyMac.
	/// </summary>
	public static class CryptographyMacExtensions
	{
		/// <summary>
		/// Generate an HMAC-based authentication code on the data in a byte array.
		/// </summary>
		/// <param name="mac">The IMacService interface.</param>
		/// <param name="key">The key material used to generate the MAC.</param>
		/// <param name="data">A byte array containing the data to generate the MAC.</param>
		/// <param name="func">The Hash function used to generate the MAC.</param>
		/// <returns>A byte array containing the Authentication Code.</returns>
		public static async Task<byte[]> Hmac(this ICryptographyMac mac, byte[] key, byte[] data, HashAlgorithm func) {
			using var stream = new MemoryStream(data);
			return await mac.Hmac(key, stream, func);
		}

		/// <summary>
		/// Generate an HMAC-based authentication code on the data in a file.
		/// </summary>
		/// <param name="mac">The IMacService interface.</param>
		/// <param name="key">The key material used to generate the MAC.</param>
		/// <param name="path">A path to the file to generate the MAC.</param>
		/// <param name="func">The Hash function used to generate the MAC.</param>
		/// <returns>A byte array containing the Authentication Code.</returns>
		public static async Task<byte[]> Hmac(this ICryptographyMac mac, byte[] key, string path, HashAlgorithm func) {
			using var stream = new FileStream(path, FileMode.Open);
			return await mac.Hmac(key, stream, func);
		}
	}
}
