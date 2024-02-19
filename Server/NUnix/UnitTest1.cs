using System.ComponentModel.DataAnnotations;
using System.Net.Security;
using System.Security.Cryptography;
using System.Text;
using Server;
namespace Server
{
    public class EncryptionToServera
    {

        public EncryptionToServer _encryptionToServer { get; set; } = null;

        [SetUp]

        public void Setup()
        {
            _encryptionToServer = new EncryptionToServer();
        }

        [Test]
        public void EncryptMessage_EqualTest()
        {
            string a = "KluczZabezpiecza";
            string b = "wiadomosc";

            System.Threading.Tasks.Task<byte[]> encryptedMessage = EncryptionToServer.EncryptMessage(b);

            string decryptedMessage = EncryptionToServer.DecryptMessage(encryptedMessage.Result).Result;
            Console.WriteLine(decryptedMessage);
            if (decryptedMessage == b) { Assert.Pass(); }
            else
            {
                Assert.Fail();
            }
        }
    }
}
