using System.Text;
using EllipticBit.Services.Cryptography;

using Microsoft.Extensions.DependencyInjection;

namespace Cryptography.NetFramework.UnitTests
{
	[TestClass]
	public class UnitTests
	{
		private ICryptographyHash hash;
		private ICryptographyMac mac;
		private ICryptographyKdf kdf;
		private ICryptographySymmetric symmetric;

		[TestInitialize]
		public void Setup() {
			var sc = new ServiceCollection();
			sc.AddCryptographyServices();
			var scc = sc.BuildServiceProvider();
			hash = scc.GetRequiredService<ICryptographyHash>();
			mac = scc.GetRequiredService<ICryptographyMac>();
			kdf = scc.GetRequiredService<ICryptographyKdf>();
			symmetric = scc.GetRequiredService<ICryptographySymmetric>();
		}

		[TestMethod]
		public async Task TestHashService()
		{
			var vec1 = await hash.Hash(Encoding.UTF8.GetBytes(""), HashAlgorithm.SHA2_384);
			var vec2 = await hash.Hash(Encoding.UTF8.GetBytes("abc"), HashAlgorithm.SHA2_384);
			var vec3 = await hash.Hash(Encoding.UTF8.GetBytes("abcdbcdecdefdefgefghfghighijhijkijkljklmklmnlmnomnopnopq"), HashAlgorithm.SHA2_384);
			var vec4 = await hash.Hash(Encoding.UTF8.GetBytes("The quick brown fox jumps over the lazy dog."), HashAlgorithm.SHA2_384);

			var vr1 = BitConverter.ToString(vec1).Replace("-", string.Empty);
			Assert.AreEqual("38b060a751ac96384cd9327eb1b1e36a21fdb71114be07434c0cc7bf63f6e1da274edebfe76f65fbd51ad2f14898b95b".ToUpperInvariant(), vr1);
			Assert.AreEqual("cb00753f45a35e8bb5a03d699ac65007272c32ab0eded1631a8b605a43ff5bed8086072ba1e7cc2358baeca134c825a7".ToUpperInvariant(), BitConverter.ToString(vec2).Replace("-", string.Empty));
			Assert.AreEqual("3391fdddfc8dc7393707a65b1b4709397cf8b1d162af05abfe8f450de5f36bc6b0455a8520bc4e6f5fe95b1fe3c8452b".ToUpperInvariant(), BitConverter.ToString(vec3).Replace("-", string.Empty));
			Assert.AreEqual("ed892481d8272ca6df370bf706e4d7bc1b5739fa2177aae6c50e946678718fc67a7af2819a021c2fc34e91bdb63409d7".ToUpperInvariant(), BitConverter.ToString(vec4).Replace("-", string.Empty));
		}

		[TestMethod]
		public async Task TestMacService()
		{
			byte[] key = new byte[48] {0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9, 0xA, 0xB, 0xC, 0xD, 0xE, 0xF,
				0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9, 0xA, 0xB, 0xC, 0xD, 0xE, 0xF,
				0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9, 0xA, 0xB, 0xC, 0xD, 0xE, 0xF };
			var vec1 = await mac.Hmac(key, Encoding.UTF8.GetBytes(""), HashAlgorithm.SHA2_384);
			var vec2 = await mac.Hmac(key, Encoding.UTF8.GetBytes("abc"), HashAlgorithm.SHA2_384);
			var vec3 = await mac.Hmac(key, Encoding.UTF8.GetBytes("abcdbcdecdefdefgefghfghighijhijkijkljklmklmnlmnomnopnopq"), HashAlgorithm.SHA2_384);

			var vr1 = BitConverter.ToString(vec1).Replace("-", string.Empty);
			Assert.AreEqual("440b0d5f59c32cbee090c3d9f524b81a9b9708e9b65a46bbc189842b0ab0759d3bf118acca58eda0813fd346e8ccfde4".ToUpperInvariant(), vr1);
			Assert.AreEqual("cb5da1048feb76fd75752dc1b699caba124090feac21adb5b4c0f6600e7b626e08d7415660aa0ee79ca5b83e56669a60".ToUpperInvariant(), BitConverter.ToString(vec2).Replace("-", string.Empty));
			Assert.AreEqual("460b59c0bd8ae48133431185a4583376738be3116cafce47aff7696bd19501b0cf1f1850c3e5fa2992882997493d1c99".ToUpperInvariant(), BitConverter.ToString(vec3).Replace("-", string.Empty));
		}

		[TestMethod]
		public void TestPBKDF2()
		{
			byte[] salt = new byte[48] {0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9, 0xA, 0xB, 0xC, 0xD, 0xE, 0xF,
				0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9, 0xA, 0xB, 0xC, 0xD, 0xE, 0xF,
				0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9, 0xA, 0xB, 0xC, 0xD, 0xE, 0xF };
			var vec1 = kdf.PBKDF2("", salt, 32, 25000, HashAlgorithm.SHA2_384);
			var vec2 = kdf.PBKDF2("abc", salt, 32, 25000, HashAlgorithm.SHA2_384);
			var vec3 = kdf.PBKDF2("abcdbcdecdefdefgefghfghighijhijkijkljklmklmnlmnomnopnopq", salt, 32, 25000, HashAlgorithm.SHA2_384);

			var vr1 = BitConverter.ToString(vec1).Replace("-", string.Empty);
			Assert.AreEqual("b0ddf56b90903d638ec8d07a4205ba2bcfa944955d553e1ef3f91cba84e8e3bd".ToUpperInvariant(), vr1);
			Assert.AreEqual("b0a5e09a38bee3eb2b84d477d5259ef7bebf0e48d9512178f7e26cc330278ff4".ToUpperInvariant(), BitConverter.ToString(vec2).Replace("-", string.Empty));
			Assert.AreEqual("d1aacafea3a9fdf3ee6236b1b45527974ea01539b4a7cc493bba56e15e14d520".ToUpperInvariant(), BitConverter.ToString(vec3).Replace("-", string.Empty));
		}

		[TestMethod]
		public void TestHKDF()
		{
			byte[] salt = new byte[48] {0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9, 0xA, 0xB, 0xC, 0xD, 0xE, 0xF,
				0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9, 0xA, 0xB, 0xC, 0xD, 0xE, 0xF,
				0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9, 0xA, 0xB, 0xC, 0xD, 0xE, 0xF };
			var vec1 = kdf.HKDF("abc"u8.ToArray(), salt, Encoding.UTF8.GetBytes(string.Empty), 64, HashAlgorithm.SHA2_384);
			var vec2 = kdf.HKDF("abcdbcdecdefdefgefghfghighijhijkijkljklmklmnlmnomnopnopq"u8.ToArray(), salt, "test"u8.ToArray(), 64, HashAlgorithm.SHA2_384);

			var vr1 = BitConverter.ToString(vec1).Replace("-", string.Empty);
			Assert.AreEqual("65e464a5d7026678a3af78bf0282592472f85ccd7d1040e2dea5cea9218276a960367d418154a1e95019182a3c857286860aa0711955829e896b5bcdb1224794".ToUpperInvariant(), vr1);
			Assert.AreEqual("12a82466f85ead03f50bb502475b47ec50e7224a90f0219955bf09846ed72791206f6e713a529a0082bf7229093f2b4e6c6b467119518a2579a5b091ebe8ba12".ToUpperInvariant(), BitConverter.ToString(vec2).Replace("-", string.Empty));
		}

		[TestMethod]
		public void TestAes()
		{
			byte[] key = new byte[32] {0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9, 0xA, 0xB, 0xC, 0xD, 0xE, 0xF,
				0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9, 0xA, 0xB, 0xC, 0xD, 0xE, 0xF };
			byte[] iv = new byte[12] { 0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9, 0xA, 0xB };
			byte[] data = "The quick brown fox jumps over the lazy dog."u8.ToArray();

			var cipherText = symmetric.Encrypt(data, null, key, iv, out byte[] tag, EncryptionAlgorithm.AES256GCM, HashAlgorithm.None);
			var plainText = symmetric.Decrypt(cipherText, null, key, iv, tag, EncryptionAlgorithm.AES256GCM, HashAlgorithm.None);

			Assert.IsTrue(data.SequenceEqual(plainText));
		}

		// Disable test as it does not appear to be available in the CNG Provider.
		//[TestMethod]
		public void TestChaCha20()
		{
			byte[] key = new byte[32] {0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9, 0xA, 0xB, 0xC, 0xD, 0xE, 0xF,
				0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9, 0xA, 0xB, 0xC, 0xD, 0xE, 0xF };
			byte[] iv = new byte[16] { 0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9, 0xA, 0xB, 0xC, 0xD, 0xE, 0xF };
			byte[] data = "The quick brown fox jumps over the lazy dog."u8.ToArray();

			var cipherText = symmetric.Encrypt(data, null, key, iv, out byte[] tag, EncryptionAlgorithm.ChaCha20Poly1305, HashAlgorithm.None);
			var plainText = symmetric.Decrypt(cipherText, null, key, iv, tag, EncryptionAlgorithm.ChaCha20Poly1305, HashAlgorithm.None);

			Assert.IsTrue(data.SequenceEqual(plainText));
		}
	}
}