using System.Security.Cryptography;
using System.Text;

namespace NetSteps.Security
{
	public class Hashing
	{
		public static string Md5Encrypt(string plainText)
		{
			UTF8Encoding encoder = new UTF8Encoding();
			MD5CryptoServiceProvider md5hasher = new MD5CryptoServiceProvider();
			byte[] hashedDataBytes = md5hasher.ComputeHash(encoder.GetBytes(plainText));
			return ByteArrayToString(hashedDataBytes);
		}

		public static string Sha1Encrypt(string plainText)
		{
			UTF8Encoding encoder = new UTF8Encoding();
			SHA1CryptoServiceProvider sha1hasher = new SHA1CryptoServiceProvider();
			byte[] hashedDataBytes = sha1hasher.ComputeHash(encoder.GetBytes(plainText));
			return ByteArrayToString(hashedDataBytes);
		}

		public static string Sha256Encrypt(string plainText)
		{
			UTF8Encoding encoder = new UTF8Encoding();
			SHA256Managed sha256hasher = new SHA256Managed();
			byte[] hashedDataBytes = sha256hasher.ComputeHash(encoder.GetBytes(plainText));
			return ByteArrayToString(hashedDataBytes);
		}

		public static string Sha384Encrypt(string plainText)
		{
			UTF8Encoding encoder = new UTF8Encoding();
			SHA384Managed sha384hasher = new SHA384Managed();
			byte[] hashedDataBytes = sha384hasher.ComputeHash(encoder.GetBytes(plainText));
			return ByteArrayToString(hashedDataBytes);
		}

		public static string Sha512Encrypt(string plainText)
		{
			UTF8Encoding encoder = new UTF8Encoding();
			SHA512Managed sha512hasher = new SHA512Managed();
			byte[] hashedDataBytes = sha512hasher.ComputeHash(encoder.GetBytes(plainText));
			return ByteArrayToString(hashedDataBytes);
		}

		public static string ByteArrayToString(byte[] inputArray)
		{
			StringBuilder output = new StringBuilder("");
			for (int i = 0; i < inputArray.Length; i++)
			{
				output.Append(inputArray[i].ToString("X2"));
			}
			return output.ToString();
		}
	}
}
