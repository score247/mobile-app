﻿using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace LiveScore.Common.Helpers
{
    /// <summary>
    /// Key is hex string random.
    /// </summary>
    public interface ICryptographyHelper
    {
        string Encrypt(string message, string key, string salt = "382af5535cba45839752a452c16bc618");

        string Decrypt(string encryptedMessage, string key, string salt = "382af5535cba45839752a452c16bc618");
    }

    public class CryptographyHelper : ICryptographyHelper
    {
        public string Decrypt(string encryptedMessage, string key, string salt = "382af5535cba45839752a452c16bc618")
        {
            // Check arguments.
            if (encryptedMessage == null || encryptedMessage.Length <= 0)
            {
                throw new ArgumentNullException(nameof(encryptedMessage));
            }

            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (salt == null || salt.Length <= 0)
            {
                throw new ArgumentNullException(nameof(salt));
            }

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an RijndaelManaged object
            // with the specified key and IV.
            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = ConvertHexStringToByte(key);
                rijAlg.IV = ComputeSalt(salt);

                // Create a decrytor to perform the stream transform.
                var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(encryptedMessage)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }

        public string Encrypt(string message, string key, string salt = "382af5535cba45839752a452c16bc618")
        {
            // Check arguments.
            if (message == null || message.Length <= 0)
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (salt == null || salt.Length <= 0)
            {
                throw new ArgumentNullException(nameof(salt));
            }

            byte[] encrypted;
            // Create an RijndaelManaged object
            // with the specified key and IV.
            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = ConvertHexStringToByte(key);
                rijAlg.IV = ComputeSalt(salt);

                // Create a decryptor to perform the stream transform.
                var encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(message);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return Convert.ToBase64String(encrypted);
        }

        private static byte[] ConvertHexStringToByte(string hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
#pragma warning disable S109 // Magic numbers should not be used
            for (int i = 0; i < NumberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
#pragma warning restore S109 // Magic numbers should not be used
            return bytes;
        }

        private static byte[] ComputeSalt(string salt)
        {
#pragma warning disable S2070 // SHA-1 and Message-Digest hash algorithms should not be used in secure contexts
            var md5 = MD5.Create();
#pragma warning restore S2070 // SHA-1 and Message-Digest hash algorithms should not be used in secure contexts
            byte[] source = Encoding.Default.GetBytes(salt);
            return md5.ComputeHash(source);
        }
    }
}