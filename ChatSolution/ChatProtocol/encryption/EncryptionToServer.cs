// EncryptionToServer.cs

using System.Security.Cryptography;
using System.Text;

namespace Server;

public class EncryptionToServer
{
    public static string EncryptionKey => "KluczZabezpiecza"; //TODO change this

    public static async Task<byte[]> EncryptMessage(string message)
    {
        byte[] cipheredText;
        var simpletext = string.Empty;
        using (var aesAlg = Aes.Create())
        {
            aesAlg.Key = Encoding.UTF8.GetBytes(EncryptionKey);
            aesAlg.IV = new byte[16]; // Poprawiono długość IV na 16 bajtów (128 bitów)

            var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (var msEncrypt = new MemoryStream())
            {
                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (var swEncrypt = new StreamWriter(csEncrypt))
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

    public static async Task<string> DecryptMessage(byte[] cipheredText)
    {
        var simpleText = string.Empty;
        using (var aesAlg = Aes.Create())
        {
            aesAlg.Key = Encoding.UTF8.GetBytes(EncryptionKey);
            aesAlg.IV = new byte[16]; // Ensure this matches the IV used for encryption.

            var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (var msDecrypt = new MemoryStream(cipheredText))
            {
                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (var srDecrypt = new StreamReader(csDecrypt))
                    {
                        simpleText = srDecrypt.ReadLine();
                        srDecrypt.Close();
                    }
                }
            }
        }

        return simpleText;
    }
}