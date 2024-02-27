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
        private int id;
        public NetworkStream _stream { get; set; }
        public TcpListener _listener { get; set; }
        public static Dictionary<string, TcpClient> _clients = new Dictionary<string, TcpClient>();
        public int GetId()
        {
            ++id;
            return id;
        }
        public TcpServer()
        {
            
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
                Console.WriteLine("Klient został połączonys");

                // Start asynchronous data reading for the new client
                Task.Run(() => HandleClientAsync(client));
            }
        }

        private async Task HandleClientAsync(TcpClient client)
        {
            Console.WriteLine(_clients);
            _stream = client.GetStream();
            if (_stream != null)
            {
                byte[] buffer = new byte[1024];
                PacketReader packetReader = new PacketReader();
                while (true)
                {
                    int bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead > 0)
                    {
                        packetReader.ReadPacket(buffer);
                    }
                }
            }
        }


        public async Task BroadcastToClients()
        {
            string message = Console.ReadLine();
            foreach (KeyValuePair<string , TcpClient> connectedClient in _clients)
            {
                message = ($"User {connectedClient.Key.ToString()} Said  {message}");
                SendToClient(connectedClient.Value, message);
            }
        }

        private async Task SendToClient(TcpClient client, string message)
        {
            try
            {
                PacketWriter packet = new PacketWriter();

                
               
                var tcpStream = client.GetStream();
                await packet.Flush(tcpStream);
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
