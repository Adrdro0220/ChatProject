using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace User
{
    internal class Conn
    {
        public string EncryptionKey { get { return "KluczZabezpiecza"; } }
        public TcpClient _client { get; set; }
        public NetworkStream _stream { get; set; }
        public Conn()
        {
            _client = new TcpClient("127.0.0.1", 13000);
            _stream = _client.GetStream();
            Task.Run(async () => await ReceiveMessagesAsync()); // Uruchomienie asynchronicznej metody do odbierania wiadomości
        }

        public async Task SendMessageToServerAsync(string message)
        {
            var encryptedData = DecryptionFromServer.EncryptMessage(message,EncryptionKey);
            await _stream.WriteAsync(encryptedData.Result, 0, encryptedData.Result.Length);
        }

        private async Task ReceiveMessagesAsync()
        {
            while (true)
            {
                byte[] data = new byte[256];
                int bytesRead = await _stream.ReadAsync(data, 0, data.Length);

                if (bytesRead > 0)
                {
                   
                    string decryptedStream = await DecryptionFromServer.DecryptMessage(data, EncryptionKey);
                    Console.WriteLine($"Received: {decryptedStream}");
                }
            }
        }



    }
}
