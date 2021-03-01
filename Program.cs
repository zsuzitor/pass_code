using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
        //https://docs.microsoft.com/ru-ru/dotnet/api/system.security.cryptography.aes?view=netcore-3.1
        //https://docs.microsoft.com/ru-ru/dotnet/api/system.security.cryptography.rijndael?view=net-5.0

            string original = "привет как дела 123аффвыячсмfdasnnnnnnnnnnnnnnnnnnn55555555555555555555---------------------------!!!$$$";
            foreach (var item in Enumerable.Range(0,10))
            {
                original += original;
            }
            string key = "kjopweproifjflsd4949409";
            byte[] initVector = new byte[16];




            //using (Aes myAes = Aes.Create())
            {
                var keyBytes = Encoding.UTF8.GetBytes(key);
                byte[] keyBytesGood = new byte[32];
               for(var i=0;i< keyBytes.Length; ++i)
                {
                    keyBytesGood[i] = keyBytes[i];
                }
                //myAes.Key = [0..32];
                //myAes.Key = keyBytesGood;
                // Encrypt the string to an array of bytes.
                byte[] encrypted = EncryptStringToBytes_Aes(original, keyBytesGood, initVector);
               var strEnc =Encoding.UTF8.GetString(encrypted);
                // Decrypt the bytes to a string.
                string roundtrip = DecryptStringFromBytes_Aes(encrypted, keyBytesGood, initVector);

                //Display the original data and the decrypted data.
                Console.WriteLine("Original:   {0}", original);
                Console.WriteLine("Round Trip: {0}", roundtrip);
            }


            Console.WriteLine("Hello World!");

        }





        static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

        static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
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















        static public byte[] Decrypt(byte[] data, string password)
        {

            using SymmetricAlgorithm sa = Rijndael.Create();
            
            using ICryptoTransform ct = sa.CreateDecryptor(
                (new PasswordDeriveBytes(password, null)).GetBytes(16),//TODO сюда докинуть еще и соль?
                new byte[16]);
            using MemoryStream ms = new MemoryStream();
            using CryptoStream cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
            //cs.Write(data, 0, data.Length);
            cs.Write(data);

            cs.FlushFinalBlock();
            return ms.ToArray();
        }
        static public byte[] Encrypt(byte[] data, string password)
        {
            using SymmetricAlgorithm sa = Rijndael.Create();
            using ICryptoTransform ct = sa.CreateEncryptor(
                (new PasswordDeriveBytes(password, null)).GetBytes(16),//TODO сюда докинуть еще и соль?
                new byte[16]);
            using MemoryStream ms = new MemoryStream();
            using CryptoStream cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
            cs.Write(data, 0, data.Length);
            cs.FlushFinalBlock();
            return ms.ToArray();
        }

        static public string Encrypt(string data, string password)
        {
            var tmp = Encrypt(Encoding.UTF8.GetBytes(data), password);

            return Convert.ToBase64String(tmp);
        }

        static public string Decrypt(string data, string password)
        {
            var tmp = Decrypt(Encoding.UTF8.GetBytes(data), password);

            return Convert.ToBase64String(tmp);
        }


    }
}
