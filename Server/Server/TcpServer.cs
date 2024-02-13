// TcpServer.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class TcpServer
    {
        public string EncryptionKey { get { return "KluczZabezpiecza"; } }
        public NetworkStream _stream { get; set; }
        public TcpListener _listener { get; set; }
        public List<TcpClient> _clients { get; set; }

        public TcpServer()
        {
            _clients = new List<TcpClient>();
            StartServer();
        }

        private async void StartServer()
        {
            var port = 13000;
            var hostAddress = IPAddress.Parse("127.0.0.1");
            _listener = new TcpListener(hostAddress, port);
            _listener.Start();

            while (true)
            {
                TcpClient client = await _listener.AcceptTcpClientAsync();
                _clients.Add(client);
                await Console.Out.WriteLineAsync("klient został połączony");

                // Start asynchronous data reading for the new client
                await Task.Run(() => HandleClientAsync(client));
            }
        }

        private async Task HandleClientAsync(TcpClient client)
        {
                _stream = client.GetStream();
                if (_stream != null)
                {
                    while (true)
                    {
                    byte[] data = new byte[256];
                    int bytesRead = await _stream.ReadAsync(data, 0, data.Length);

                    if (bytesRead > 0)
                    {
                        string receivedData = Encoding.ASCII.GetString(data, 0, bytesRead);
                        string decryptedStream = await EncryptionToServer.DecryptMessage(receivedData, EncryptionKey);
                        Console.WriteLine($"Received: {decryptedStream}");
                    }
                }
                }
        }

        public async Task BroadcastToClients()
        {
            string message = Console.ReadLine();
            foreach (var connectedClient in _clients)
            {
                var tempMessage = await EncryptionToServer.EncryptMessage(message, EncryptionKey);
                SendToClient(connectedClient, tempMessage.ToString());
            }
        }

        private void SendToClient(TcpClient client, string message)
        {
            try
            {
                var tcpStream = client.GetStream();
                byte[] data = Encoding.UTF8.GetBytes(message);
                tcpStream.Write(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., client disconnects)
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }
        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}
