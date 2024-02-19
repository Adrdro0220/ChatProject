// EncryptionToServer.cs
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class EncryptionToServer
    {
        public static string EncryptionKey { get { return "KluczZabezpiecza"; } }
        public async static Task<byte[]> EncryptMessage(string message)
        {
            byte[] cipheredText;
            String simpletext = string.Empty;
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(EncryptionKey);
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

        public async static Task<string> DecryptMessage(byte[] cipheredText)
        {
            string simpleText = String.Empty;
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(EncryptionKey);
                aesAlg.IV = new byte[16]; // Ensure this matches the IV used for encryption.
                aesAlg.Padding = PaddingMode.None; // Ustaw dopełnienie na None

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipheredText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            simpleText = srDecrypt.ReadToEnd();
                            srDecrypt.Close();
                        }
                    }
                }
            }
            return simpleText;
        }


    }
}
