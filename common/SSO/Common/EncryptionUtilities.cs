using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace NetSteps.SSO.Common
{
	using System.Security.Cryptography;

	using Newtonsoft.Json;

	/// <summary>
	/// Collection of static methods used to encrypt or decrypt byte[]'s
	/// </summary>
	public static class EncryptionUtilities
	{
		/// <summary>
		/// Deciphers a base 64 encoded string.
		/// </summary>
		/// <param name="key">cipher key</param>
		/// <param name="base64Text">the base 64 encoded text to decipher.</param>
		/// <param name="salt">salt</param>
		/// <returns>The decrypted text, or empty string if failure</returns>
		public static string DecryptTripleDES(byte[] key, string base64Text, string salt)
		{
			Contract.Requires(key != null);
			Contract.Requires(key.Length > 0);
			Contract.Requires(!String.IsNullOrWhiteSpace(base64Text));

			return DecryptTripleDES<string>(key, base64Text, salt);
		}



		/// <summary>
		/// Decryptns a base 64 encrypted string
		/// </summary>
		/// <typeparam name="T">The type of object to be returned</typeparam>
		/// <param name="key">The encryption key</param>
		/// <param name="base64Text">The base 64 encoded text to be decrypted</param>
		/// <param name="salt">The salt to be used</param>
		/// <returns>The decrypted text, or default(T) if failure</returns>
		public static T DecryptTripleDES<T>(byte[] key, string base64Text, string salt) where T : class
		{
			Contract.Requires(key != null);
			Contract.Requires(key.Length > 0);
			Contract.Requires(!String.IsNullOrWhiteSpace(base64Text));

			try
			{
				byte[] textBytes = Convert.FromBase64String(base64Text);
				string decryptedString = System.Text.ASCIIEncoding.ASCII.GetString(Decrypt(textBytes, key));

				if (salt != null && salt.Length > 0 && decryptedString.EndsWith(salt))
					decryptedString = decryptedString.Substring(0, decryptedString.Length - salt.Length);

				if (typeof(T) == typeof(string))
				{
					return decryptedString as T;
				}
				else
				{
					return JsonConvert.DeserializeObject<T>(decryptedString);
				}
			}
			catch (Exception)
			{
				return default(T);
			}
		}



		/// <summary>
		/// Creates cipher text from a plaintext string and base64 encodes the result.
		/// </summary>
		/// <param name="key">Encryption key</param>
		/// <param name="plainText">Text to be encrypted</param>
		/// <param name="salt">Salt to use for encryption</param>
		/// <returns>The ciphered text, empty string if failure</returns>
		public static string EncryptTripleDES(byte[] key, string plainText, string salt)
		{
			Contract.Requires(key != null);
			Contract.Requires(key.Length > 0);
			Contract.Requires(!String.IsNullOrWhiteSpace(plainText));

			return EncryptTripleDES(key, (object)plainText, salt);
		}



		/// <summary>
		/// Creates cipher text from an object serialized to JSON and base64 encodes the result.
		/// </summary>
		/// <param name="key">Encryption key</param>
		/// <param name="obj">Object to be JSON encoded and encrypted</param>
		/// <param name="salt">Salt to use for encryption</param>
		/// <returns>The encrypted text, empty string if failure</returns>
		public static string EncryptTripleDES(byte[] key, object obj, string salt)
		{
			Contract.Requires(key != null);
			Contract.Requires(key.Length > 0);
			Contract.Requires(obj != null);

			try
			{
				string jsonRepresentation = obj is string ? obj as string : JsonConvert.SerializeObject(obj);

				if (!String.IsNullOrEmpty(salt))
					jsonRepresentation += salt;

				byte[] textBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(jsonRepresentation);

				return Convert.ToBase64String(Encrypt(textBytes, key));
			}
			catch (Exception)
			{
				return string.Empty;
			}
		}




		/// <summary>
		/// Encrypts bytesToEncrypt using the provided key
		/// </summary>
		/// <param name="bytesToEncrypt"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public static byte[] Encrypt(byte[] bytesToEncrypt, byte[] key)
		{
			try
			{

				using (TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider())
				using (MD5CryptoServiceProvider hashMD5 = new MD5CryptoServiceProvider())
				{
					DES.Key = hashMD5.ComputeHash(key);
					DES.Mode = CipherMode.ECB;
					ICryptoTransform DESEncrypt = DES.CreateEncryptor();

					return DESEncrypt.TransformFinalBlock(bytesToEncrypt, 0, bytesToEncrypt.Length);
				}
			}
			catch (Exception ex)
			{
				throw new Exception(string.Format("Encryption failed. Original Message: {0}", ex.Message), ex);
			}
		}



		/// <summary>
		/// Decrypts bytesToDecrypt with the provided key
		/// </summary>
		/// <param name="bytesToDecrypt"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public static byte[] Decrypt(byte[] bytesToDecrypt, byte[] key)
		{
			try
			{
				using (TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider())
				using (MD5CryptoServiceProvider hashMD5 = new MD5CryptoServiceProvider())
				{
					DES.Key = hashMD5.ComputeHash(key);
					DES.Mode = System.Security.Cryptography.CipherMode.ECB;
					System.Security.Cryptography.ICryptoTransform DESDecrypt = DES.CreateDecryptor();

					return DESDecrypt.TransformFinalBlock(bytesToDecrypt, 0, bytesToDecrypt.Length);
				}
			}
			catch (Exception ex)
			{
				throw new Exception(string.Format("Decryption failed. Original Message: {0}", ex.Message), ex);
			}
		}
	}
}
