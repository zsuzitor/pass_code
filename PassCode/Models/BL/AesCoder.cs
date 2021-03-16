

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
            var keyInBytes = KeyToBytes(key);

            try
            {
                return DecryptStringFromBytesAes(dataForDecrypt, keyInBytes);
            }
            catch
            {
                throw new CommandHandleException("не удалось расшифровать, возможно неверный пароль");
            }
        }

        public byte[] EncryptWithByte(string dataForEncrypt, string key)
        {
            var keyInBytes = KeyToBytes(key);
            var encr = EncryptStringToBytesAes(dataForEncrypt, keyInBytes);
            return encr;
        }



        public string BytesToCustomString(byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
            //return string.Join('.', bytes);
        }

        public byte[] CustomStringToBytes(string str)
        {
            return Convert.FromBase64String(str);
            //try
            //{
            //    return str.Split(".", StringSplitOptions.RemoveEmptyEntries).Select(x => byte.Parse(x)).ToArray();
            //}
            //catch
            //{
            //    throw new CommandHandleException("переданное зашифрованное слово в неверном формате");
            //}
        }

        public string EncryptWithString(string dataForEncrypt, string key)
        {
            return BytesToCustomString(EncryptWithByte(dataForEncrypt, key));
        }

        public string DecryptFromString(string dataForDecrypt, string key)
        {
            return DecryptFromBytes(CustomStringToBytes(dataForDecrypt), key);
        }


        private byte[] KeyToBytes(string key)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] keyBytesGood = new byte[32];
            var j = 0;
            for (var i = 0; i < keyBytesGood.Length; ++i, ++j)
            {
                if (j >= keyBytes.Length)
                {
                    j = 0;
                }

                keyBytesGood[i] = keyBytes[j];
            }

            return keyBytesGood;
        }

        private byte[] EncryptStringToBytesAes(string plainText, byte[] Key)
        {
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");

            byte[] encrypted;

            var input = Encoding.UTF8.GetBytes(plainText);

            using Aes aesAlg = Aes.Create();

            aesAlg.Key = Key;
            aesAlg.GenerateIV();

            // Create an encryptor to perform the stream transform.
            using (ICryptoTransform encryptor = aesAlg.CreateEncryptor())
            {
                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var tBinaryWriter = new BinaryWriter(csEncrypt))
                        {
                            msEncrypt.Write(aesAlg.IV);
                            tBinaryWriter.Write(input);
                            csEncrypt.FlushFinalBlock();
                        }

                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return encrypted;
        }


        private string DecryptStringFromBytesAes(byte[] cipherText, byte[] Key)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");

            string plaintext = null;

            var iv = new byte[16];
            var txt_ = new byte[cipherText.Length - 16];
            Array.Copy(cipherText, 0, iv, 0, iv.Length);
            Array.Copy(cipherText, 16, txt_, 0, cipherText.Length - 16);

            using (Aes aesAlg = Aes.Create())
            {

                aesAlg.Key = Key;
                aesAlg.IV = iv;

                using (ICryptoTransform decryptor = aesAlg.CreateDecryptor())
                {
                    using (MemoryStream msDecrypt = new MemoryStream(txt_))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {
                                plaintext = srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }
            }

            return plaintext;
        }

        
    }
}
