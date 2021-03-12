

using PassCode.Models.BL.Interfaces;
using PassCode.Models.BO;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace PassCode.Models.BL
{
    public class AesCoder : ICoder
    {
        public string DecryptFromBytes(byte[] dataForDecrypt, string key)
        {
            byte[] initVector = InitVector();
            var keyInBytes = KeyToBytes(key);

            try
            {

                //var res = DecryptStringFromBytesAes(Encoding.UTF8.GetBytes(dataForDecrypt), keyInBytes, initVector);
                return DecryptStringFromBytesAes(dataForDecrypt, keyInBytes, initVector);
            }
            catch
            {
                throw new CommandHandleException("не удалось расшифровать, возможно неверный пароль");
            }
            //return res;
        }

        public byte[] EncryptWithByte(string dataForEncrypt, string key)
        {
            byte[] initVector = InitVector();
            var keyInBytes = KeyToBytes(key);


            var encr = EncryptStringToBytesAes(dataForEncrypt, keyInBytes, initVector);
            return encr;
            //return Encoding.UTF8.GetString(encr);
        }


        public string AddRandomizeToString(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return string.Empty;
            }

            return GenerateRandomPrefix() + str;
        }

        public string RemoveRandomizeFromString(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return string.Empty;
            }

            return str[1..];
        }



        public string BytesToCustomString(byte[] bytes)
        {
            return string.Join('.', bytes);
        }

        public byte[] CustomStringToBytes(string str)
        {
            try
            {
                return str.Split(".", StringSplitOptions.RemoveEmptyEntries).Select(x => byte.Parse(x)).ToArray();
            }
            catch
            {
                throw new CommandHandleException("передана неверная входное слово");
            }
        }




        private byte[] InitVector()
        {
            return new byte[16];
        }

        private byte[] KeyToBytes(string key)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] keyBytesGood = new byte[32];
            var j = 0;
            for (var i = 0; i < keyBytesGood.Length; ++i, ++j)
            {
                if(j >= keyBytes.Length)
                { 
                    j = 0;
                }

                keyBytesGood[i] = keyBytes[j];
            }

            return keyBytesGood;
        }

        private byte[] EncryptStringToBytesAes(string plainText, byte[] Key, byte[] IV)
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
            using Aes aesAlg = Aes.Create();

            aesAlg.Key = Key;
            aesAlg.IV = IV;

            // Create an encryptor to perform the stream transform.
            using (ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV))
            {

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(plainText);

                            //swEncrypt.Flush();
                        }


                        encrypted = msEncrypt.ToArray();
                    }
                }
            }


            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }


        private string DecryptStringFromBytesAes(byte[] cipherText, byte[] Key, byte[] IV)
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
                using (ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV))
                {
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
            }

            return plaintext;
        }

        

        private string GenerateRandomPrefix()
        {
            Random rnd = new Random();
            return ((char)rnd.Next(33, 123)).ToString();
        }

        
    }
}
