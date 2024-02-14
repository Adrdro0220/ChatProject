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
                var tempMessage = await EncryptionToServer.EncryptMessage(message, EncryptionKey);
                SendToClient(connectedClient, tempMessage.ToString());
            }
        }

        private void SendToClient(TcpClient client, string message)
        {
            try
            {
                var tcpStream = client.GetStream();
                System.Threading.Tasks.Task<byte[]> data = EncryptionToServer.EncryptMessage(message, EncryptionKey);
                tcpStream.Write(data.Result, 0, data.Result.Length);

            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., client disconnects)
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }
        
    }
}
