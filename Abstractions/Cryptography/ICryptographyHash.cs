//-----------------------------------------------------------------------------
// Copyright (c) 2020-2023 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using System.IO;
using System.Threading.Tasks;

namespace EllipticBit.Services.Cryptography
{
	/// <summary>
	/// Provides methods for hashing data.
	/// </summary>
	public interface ICryptographyHash
	{
		/// <summary>
		/// Calculates a hash on the specified data.
		/// </summary>
		/// <param name="data">A stream containing the data to hash.</param>
		/// <param name="func">The has function to use.</param>
		/// <returns>A byte array containing the calculated hash value.</returns>
		Task<byte[]> Hash(Stream data, HashAlgorithm func);
	}

	/// <summary>
	/// Extensions methods for ICryptographyHash.
	/// </summary>
	public static class CryptographyHashExtensions
	{
		/// <summary>
		/// Calculates a hash on an in-memory byte array.
		/// </summary>
		/// <param name="hash">The Hash service interface.</param>
		/// <param name="data">A byte array containing the data to hash.</param>
		/// <param name="func">The has function to use.</param>
		/// <returns>A byte array containing the calculated hash value.</returns>
		public static async Task<byte[]> Hash(this ICryptographyHash hash, byte[] data, HashAlgorithm func) {
			using var stream = new MemoryStream(data);
			return await hash.Hash(stream, func);
		}

		/// <summary>
		/// Calculates a hash on the specified file.
		/// </summary>
		/// <param name="hash">The Hash service interface.</param>
		/// <param name="path">A string containing a path to the file to hash.</param>
		/// <param name="func">The has function to use.</param>
		/// <returns>A byte array containing the calculated hash value.</returns>
		public static async Task<byte[]> Hash(this ICryptographyHash hash, string path, HashAlgorithm func) {
			using var stream = new FileStream(path, FileMode.Open);
			return await hash.Hash(stream, func);
		}
	}
}
