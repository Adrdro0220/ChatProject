// TcpServer.cs
using ChatProtocol;
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
        MessagePacket messagePacket = new MessagePacket();
        PacketWriter packetWriter = new PacketWriter();

        public TcpListener _listener { get; set; }
        PacketReader packetReader = new PacketReader();
        public static Dictionary<Guid, TcpClient> _clients = new Dictionary<Guid, TcpClient>();
        
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
                _clients.Add(Guid.NewGuid(), client);
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
                byte[] buffer = new byte[1024];
                
                while (client.Connected)
                {
                    int bytesRead = await _stream.ReadAsync( buffer, 0, buffer.Length);
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
            foreach (KeyValuePair<Guid, TcpClient> connectedClient in _clients)
            {
                SendToClient(connectedClient.Value, message);
            }
        }


        private async Task SendToClient(TcpClient client, string message)
        {
            try
            {
                messagePacket.message = message;
                packetWriter.WritePacket(messagePacket);
                packetWriter.Flush(_stream);
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
