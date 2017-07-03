using System;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace NetSteps.Sql.Security
{
    public class Encryption
    {
        // TODO: We need to change these for the new Architecture - JHE
        // We should probably have a different set of these for each client. - JHE
        #region Constants
        private const string key = "N3tst3ps_2008";
        public const string salt = "&netsteps_salt&";
        #endregion

        /// <summary>
        /// This is a salt that will we valid for the same day that it is created.
        /// Used to encrypt a value like Single-Sign-On with so that it can only be decrypted successfully on the same day.
        /// Disallows users to bookmark Single-Sign-On token URLs for later use. - JHE
        /// </summary>
        public static string SingleSignOnSalt
        {
            get
            {
                return DateTime.Today.ToString("dddd, MMMM dd, yyyy", new CultureInfo("en-US"));
            }
        }

        #region DES
        public static string EncryptTripleDES(string plainText)
        {
            return EncryptTripleDES(plainText, Encryption.key, Encryption.salt);
        }
        public static string EncryptTripleDES(string plainText, string salt)
        {
            return EncryptTripleDES(plainText, Encryption.key, salt);
        }
        public static string EncryptTripleDES(string plainText, string key, string salt)
        {
            plainText = (plainText == null) ? string.Empty : plainText.Trim();
            if (string.IsNullOrEmpty(plainText))
                return string.Empty;

            if (salt.Length > 0)
                plainText += salt;
            byte[] buffer = new byte[0];
            System.Security.Cryptography.TripleDESCryptoServiceProvider DES = new System.Security.Cryptography.TripleDESCryptoServiceProvider();
            System.Security.Cryptography.MD5CryptoServiceProvider hashMD5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            DES.Key = hashMD5.ComputeHash(System.Text.ASCIIEncoding.ASCII.GetBytes(key));
            DES.Mode = System.Security.Cryptography.CipherMode.ECB;
            System.Security.Cryptography.ICryptoTransform DESEncrypt = DES.CreateEncryptor();
            buffer = System.Text.ASCIIEncoding.ASCII.GetBytes(plainText);

            string tripleDES = Convert.ToBase64String(DESEncrypt.TransformFinalBlock(buffer, 0, buffer.Length));
            return tripleDES;
        }

        // Decryption Method 
        public static string DecryptTripleDES(string base64Text)
        {
            return DecryptTripleDES(base64Text, Encryption.key, Encryption.salt);
        }
        public static string DecryptTripleDES(string base64Text, string salt)
        {
            return DecryptTripleDES(base64Text, Encryption.key, salt);
        }
        public static string DecryptTripleDES(string base64Text, string key, string salt)
        {
            if (string.IsNullOrEmpty(base64Text))
                return string.Empty;

            byte[] buffer = new byte[0];
            System.Security.Cryptography.TripleDESCryptoServiceProvider DES = new System.Security.Cryptography.TripleDESCryptoServiceProvider();
            System.Security.Cryptography.MD5CryptoServiceProvider hashMD5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            DES.Key = hashMD5.ComputeHash(System.Text.ASCIIEncoding.ASCII.GetBytes(key));
            DES.Mode = System.Security.Cryptography.CipherMode.ECB;
            System.Security.Cryptography.ICryptoTransform DESDecrypt = DES.CreateDecryptor();
            buffer = Convert.FromBase64String(base64Text);

            string decTripleDES = System.Text.ASCIIEncoding.ASCII.GetString(DESDecrypt.TransformFinalBlock(buffer, 0, buffer.Length));

            if (salt.Length > 0 && decTripleDES.EndsWith(salt))
                decTripleDES = decTripleDES.Substring(0, decTripleDES.Length - salt.Length);

            return decTripleDES;
        }
        #endregion

        #region AES
        public static string EncryptAES(string dataToEncrypt)
        {
            return EncryptAES(dataToEncrypt, Encryption.key, Encryption.salt);
        }
        public static string EncryptAES(string dataToEncrypt, string hashKey, string salt)
        {
            // Initialise
            AesManaged encryptor = new AesManaged();

            // Set the key
            byte[] key = GetHashKey(hashKey, salt);
            encryptor.Key = key;
            encryptor.IV = key;

            // create a memory stream
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

        public static string DecryptAES(string encryptedString)
        {
            return DecryptAES(encryptedString, Encryption.key, Encryption.salt);
        }
        public static string DecryptAES(string encryptedString, string hashKey, string salt)
        {
            // Initialise
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
        #endregion

        #region Helper Methods
        private static byte[] GetHashKey(string hashKey, string salt)
        {
            // Initialise
            UTF8Encoding encoder = new UTF8Encoding();

            // Get the salt
            byte[] saltBytes = encoder.GetBytes(salt);

            // Setup the hasher
            Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(hashKey, saltBytes);

            // Return the key
            return rfc.GetBytes(16);
        }
        #endregion
    }
}
