// TcpServer.cs
using ConsoleApp1;
using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Utilities.Encoders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using User;

namespace Server
{
    internal class TcpServer
    {
        public NetworkStream _stream { get; set; }
        public TcpListener _listener { get; set; }
        static public int maxCliets { get; set; }

        static public List<TcpClient> _clients { get; set; }

        private int id = 0;

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
                //++id;
                //Client _client = new Client(id);
                _clients.Add(client);
                Console.WriteLine("Klient został połączonys");

                // Start asynchronous data reading for the new client
                Task.Run(() => HandleClientAsync(client));
            }
        }

        private async Task HandleClientAsync(TcpClient client)
        {
            
            _stream = client.GetStream();
            if (_stream != null)
            {
                while (true)
                {
                    byte[] buffer = new byte[1024];
                    int bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead > 0)
                    {
                        PacketReader tmp = new PacketReader(buffer);
                    }
                }
            }
        }


        public async Task BroadcastToClients()
        {
            string message = Console.ReadLine();
            foreach (var connectedClient in _clients)
            {
                SendToClient(connectedClient, message);
            }
        }

        private async Task SendToClient(TcpClient client, string message)
        {
            try
            {
                PacketWriter packet = new PacketWriter(message, "SentMessage");
                await packet.AssemblePacket();
                var tcpStream = client.GetStream();
                await tcpStream.WriteAsync(packet.PacketReadyToSent, 0, packet.PacketReadyToSent.Length);
               
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }
        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
    }
}
