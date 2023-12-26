//-----------------------------------------------------------------------------
// Copyright (c) 2020-2023 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

using System;

namespace EllipticBit.Services.Cryptography
{
	/// <summary>
	/// A representation of a hashed password.
	/// </summary>
	public class HashedPassword
	{
		/// <summary>
		/// The hashing algorithm used to secure the password.
		/// </summary>
		public PasswordAlgorithm Algorithm { get; }
		/// <summary>
		/// The version of the hash parameters used to secure the password.
		/// </summary>
		public short ParameterVersion { get; }
		/// <summary>
		/// The salt used by the hashing function.
		/// </summary>
		public byte[] Salt { get; }
		/// <summary>
		/// The hashed password
		/// </summary>
		public byte[] Derived { get; }

		/// <summary>
		/// Constructs a HashedPassword object from the provided hashing parameters.
		/// </summary>
		/// <param name="derived">The hashed password.</param>
		/// <param name="salt">The salt used by the hashing function.</param>
		/// <param name="algorithm">The hashing algorithm used to secure the password.</param>
		/// <param name="paramVersion">The version of the hash parameters used to secure the password.</param>
		public HashedPassword(byte[] derived, byte[] salt, PasswordAlgorithm algorithm, short paramVersion)
		{
			Algorithm = algorithm;
			ParameterVersion = paramVersion;
			Salt = salt;
			Derived = derived;
		}

		/// <summary>
		/// Creates string containing the encoded password from the HashedPassword.
		/// </summary>
		/// <returns>The encoded password string</returns>
		public override string ToString() {
			return $"{Convert.ToInt16(Algorithm)}.{ParameterVersion}.{Convert.ToBase64String(Salt)}.{Convert.ToBase64String(Derived)}";
		}

		/// <summary>
		/// Creates a HashedPassword from an encoded string.
		/// </summary>
		/// <param name="encoded">The encoded string.</param>
		/// <returns>A HashedPassword object containing the decoded string values.</returns>
		/// <exception cref="ArgumentOutOfRangeException">The provided string is invalid.</exception>
		public static HashedPassword FromBase64(string encoded) {
			var parts = encoded.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
			if (parts.Length != 4) throw new ArgumentOutOfRangeException(nameof(encoded), "Invalid string provided.");
			return new HashedPassword(Convert.FromBase64String(parts[3]), Convert.FromBase64String(parts[2]), (PasswordAlgorithm)Convert.ToInt16(parts[0]), Convert.ToInt16(parts[1]));
		}
	}
}
