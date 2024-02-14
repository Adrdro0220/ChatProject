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
                Console.WriteLine("Klient został połączony");

                // Start asynchronous data reading for the new client
                Task.Run(() => HandleClientAsync(client));
            }
        }

        private async Task HandleClientAsync(TcpClient client)
        {
            string uniqueId = Guid.NewGuid().ToString();
            _stream = client.GetStream();
            if (_stream != null)
            {
                while (true)
                {
                    await Console.Out.WriteLineAsync($"Klient o id {uniqueId} połączył się ");
                    byte[] buffer = new byte[256];
                    int bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length);

                    if (bytesRead > 0)
                    {
                        byte[] data = new byte[bytesRead];
                        Array.Copy(buffer, data, bytesRead);
                        string decryptedStream = await EncryptionToServer.DecryptMessage(data, EncryptionKey);
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
                byte[] tempMessage = await EncryptionToServer.EncryptMessage(message, EncryptionKey);
                SendToClient(connectedClient, tempMessage);
            }
        }

        private void SendToClient(TcpClient client, byte[] message)
        {
            try
            {
                var tcpStream = client.GetStream();
                tcpStream.Write(message, 0, message.Length);
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., client disconnects)
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }
    }
}
