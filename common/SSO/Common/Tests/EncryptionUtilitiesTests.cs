using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NetSteps.SSO.Common.Tests
{
	[TestClass]
	public class EncryptionUtilitiesTests
	{
		[TestMethod]
		public void EncryptionUtilities_Encrypt_Decrypt_String_SameKey_SameResult()
		{
			//Arrange
			byte[] keyBytes = new byte[] { 5, 3, 1, 7 };
			byte[] bytesToEncrypt = new byte[] { 1, 11, 95, 134, 16 };

			//Act
			byte[] results = EncryptionUtilities.Encrypt(bytesToEncrypt, keyBytes);
			results = EncryptionUtilities.Decrypt(results, keyBytes);

			string convertedNew = Convert.ToBase64String(results);
			string convertedOriginal = Convert.ToBase64String(bytesToEncrypt);

			//Assert
			Assert.AreEqual(convertedNew, convertedOriginal);
		}


		[TestMethod]
		public void EncryptionUtilities_Encrypt_Decrypt_String_DifferentKeys_DifferentResults()
		{
			//Arrange
			byte[] keyBytes = new byte[] { 5, 3, 1, 7 };
			byte[] secondKeyBytes = new byte[] { 1, 2, 3, 4, 5 };
			byte[] bytesToEncrypt = new byte[] { 1, 11, 95, 134, 16 };
			bool failed = false;

			//Act
			try
			{
				byte[] results = EncryptionUtilities.Encrypt(bytesToEncrypt, keyBytes);
				results = EncryptionUtilities.Decrypt(results, secondKeyBytes);
			}
			catch (Exception ex)
			{
				if (ex is System.Security.Cryptography.CryptographicException)
					failed = true;
			}

			//Assert
			Assert.IsFalse(failed);
		}



		[TestMethod]
		public void EncryptionUtilities_EncryptTripleDES_String_DecryptTripleDES_SameResult()
		{
			byte[] key = new byte[] { 5, 3, 1, 7 };
			string message = "message";
			string salt = "salt";

			//Act
			string encrypted = EncryptionUtilities.EncryptTripleDES(key, message, salt);
			string result = EncryptionUtilities.DecryptTripleDES(key, encrypted, salt);

			//Assert
			Assert.AreEqual(result, message);
		}



		[TestMethod]
		public void EncryptionUtilities_EncryptTripleDES_String_DecryptTripleDES_DifferentKey_DifferentResult()
		{
			byte[] key1 = new byte[] { 5, 3, 1, 7 };
			byte[] key2 = new byte[] { 1, 2, 3, 4 };
			string message = "message";
			string salt = "salt";

			//Act
			string encrypted = EncryptionUtilities.EncryptTripleDES(key1, message, salt);
			string result = EncryptionUtilities.DecryptTripleDES(key2, encrypted, salt);

			//Assert
			Assert.AreNotEqual(result, message);
		}



		[TestMethod]
		public void EncryptionUtilities_EncryptTripleDES_DecryptTripleDES_String_DifferentSalt_DifferentResult()
		{
			byte[] key = new byte[] { 5, 3, 1, 7 };
			string message = "message";
			string salt1 = "salt";
			string salt2 = "salty";

			//Act
			string encrypted = EncryptionUtilities.EncryptTripleDES(key, message, salt1);
			string result = EncryptionUtilities.DecryptTripleDES(key, encrypted, salt2);

			//Assert
			Assert.AreNotEqual(result, message);
		}



		[TestMethod]
		public void EncryptionUtilities_EncryptTripleDES_Object_DecryptTripleDES_SameResult()
		{
			byte[] key = new byte[] { 5, 3, 1, 7 };
			TestObject message = new TestObject() { SomeString = "Hi mom.", SomeDate = DateTime.Now };
			string salt = "salt";

			//Act
			string encrypted = EncryptionUtilities.EncryptTripleDES(key, message, salt);
			TestObject result = EncryptionUtilities.DecryptTripleDES<TestObject>(key, encrypted, salt);

			//Assert
			Assert.AreEqual(result, message);
		}



		[TestMethod]
		public void EncryptionUtilities_EncryptTripleDES_Object_DecryptTripleDES_DifferentKey_DifferentResult()
		{
			byte[] key1 = new byte[] { 5, 3, 1, 7 };
			byte[] key2 = new byte[] { 1, 2, 3, 4 };
			TestObject message = new TestObject() { SomeString = "Hi mom.", SomeDate = DateTime.Now };
			string salt = "salt";

			//Act
			string encrypted = EncryptionUtilities.EncryptTripleDES(key1, message, salt);
			TestObject result = EncryptionUtilities.DecryptTripleDES<TestObject>(key2, encrypted, salt);

			//Assert
			Assert.AreNotEqual(result, message);
		}



		[TestMethod]
		public void EncryptionUtilities_EncryptTripleDES_DecryptTripleDES_Object_DifferentSalt_DifferentResult()
		{
			byte[] key = new byte[] { 5, 3, 1, 7 };
			TestObject message = new TestObject() { SomeString = "Hi mom.", SomeDate = DateTime.Now };
			string salt1 = "salt";
			string salt2 = "salty";

			//Act
			string encrypted = EncryptionUtilities.EncryptTripleDES(key, message, salt1);
			TestObject result = EncryptionUtilities.DecryptTripleDES<TestObject>(key, encrypted, salt2);

			//Assert
			Assert.AreNotEqual(result, message);
		}



		[TestMethod]
		public void EncryptionUtitilies_EncryptTripleDES_DecryptTripleDES_Object_DifferentTypeRequestedForDecryption()
		{
			byte[] key = new byte[] { 5, 3, 1, 7 };
			TestObject message = new TestObject() { SomeString = "Hi mom.", SomeDate = DateTime.Now };
			string salt = "salt";

			//Act
			string encrypted = EncryptionUtilities.EncryptTripleDES(key, message, salt);
			string result = EncryptionUtilities.DecryptTripleDES<string>(key, encrypted, salt);
			Foo result2 = EncryptionUtilities.DecryptTripleDES<Foo>(key, encrypted, salt);

			//Assert
			Assert.AreNotEqual(result, message);
			Assert.AreNotEqual(result2, message);
		}



		private class TestObject
		{
			public string SomeString { get; set; }

			public DateTime SomeDate { get; set; }

			public override bool Equals(object obj)
			{
				return obj is TestObject && this.Equals((TestObject)obj);
			}

			public override int GetHashCode()
			{
				return SomeString.GetHashCode() ^ SomeDate.GetHashCode();
			}

			public bool Equals(TestObject obj)
			{
				return this.SomeDate == obj.SomeDate && this.SomeString == obj.SomeString;
			}
		}

		private class Foo
		{
		}
	}
}
