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
            string dupa = "dupa";

           dupa = EncryptionToServer.EncryptMessage(dupa).Result;
            byte[] dupa1 = Encoding.UTF8.GetBytes(dupa); 
            dupa = EncryptionToServer.DecryptMessage(dupa1).Result;
            if(dupa == "dupa")
            {
                Assert.Pass();
            }

        }
    }
}
