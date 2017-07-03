using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace NetSteps.Silverlight.Encryption
{
    public class Encryption
    {
        public const string key = "N3tst3ps_2008";
        public const string salt = "&netsteps_salt&";

        // Encryption Method
        public static string EncryptAES(string dataToEncrypt)
        {
            return EncryptAES(dataToEncrypt, Encryption.key, Encryption.salt);
        }
        public static string EncryptAES(string dataToEncrypt, string hashKey, string salt)
        {
            // Initialize
            AesManaged encryptor = new AesManaged();

            // Set the key
            byte[] key = GetHashKey(hashKey, salt);
            encryptor.Key = key;
            encryptor.IV = key;

            // Create a memory stream
            using (MemoryStream encryptionStream = new MemoryStream())
            {
                // Create the crypto stream
                using (CryptoStream encrypt = new CryptoStream(encryptionStream, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    // Encrypt
                    byte[] utfD1 = UTF8Encoding.UTF8.GetBytes(dataToEncrypt);
                    encrypt.Write(utfD1, 0, utfD1.Length);
                    encrypt.FlushFinalBlock();
                    encrypt.Close();

                    // Return the encrypted data
                    return Convert.ToBase64String(encryptionStream.ToArray());
                }
            }
        }

        // Decryption Method 
        public static string DecryptAES(string encryptedString)
        {
            return DecryptAES(encryptedString, Encryption.key, Encryption.salt);
        }
        public static string DecryptAES(string encryptedString, string hashKey, string salt)
        {
            // Initialize
            AesManaged decryptor = new AesManaged();
            byte[] encryptedData = Convert.FromBase64String(encryptedString);

            // Set the key
            byte[] key = GetHashKey(hashKey, salt);
            decryptor.Key = key;
            decryptor.IV = key;

            // create a memory stream
            using (MemoryStream decryptionStream = new MemoryStream())
            {
                // Create the crypto stream
                using (CryptoStream decrypt = new CryptoStream(decryptionStream, decryptor.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    // Encrypt
                    decrypt.Write(encryptedData, 0, encryptedData.Length);
                    decrypt.Flush();
                    decrypt.Close();

                    // Return the unencrypted data
                    byte[] decryptedData = decryptionStream.ToArray();
                    return UTF8Encoding.UTF8.GetString(decryptedData, 0, decryptedData.Length);
                }
            }
        }

        private static byte[] GetHashKey(string hashKey, string salt)
        {
            // Initialize
            UTF8Encoding encoder = new UTF8Encoding();

            // Get the salt
            byte[] saltBytes = encoder.GetBytes(salt);

            // Setup the hasher
            Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(hashKey, saltBytes);

            // Return the key
            return rfc.GetBytes(16);
        }

    }

}
