using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace User
{
    internal class DecryptionFromServer
    {
        public async static Task<byte[]> EncryptMessage(string message, string key)
        {
            byte[] cipheredText;
            String simpletext = string.Empty;
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.IV = new byte[16]; // Poprawiono długość IV na 16 bajtów (128 bitów)

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(message);
                            swEncrypt.Close(); // Zamknij StreamWriter przed zamknięciem CryptoStream
                        }
                    }
                    cipheredText = msEncrypt.ToArray();
                }
            }
            return cipheredText;
        }

        public async static Task<string> DecryptMessage(byte[] cipheredText, string key)
        {
            string simpleText = String.Empty;
            using (Aes aesAlg = Aes.Create())
            {

                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.IV = new byte[16]; // Ensure this matches the IV used for encryption.

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipheredText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            simpleText = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return simpleText;
        }
    }
}