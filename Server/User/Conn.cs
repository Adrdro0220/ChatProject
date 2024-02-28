using ChatProtocol;
using ConsoleApp1;
using Newtonsoft.Json;
using Server;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace User
{
    internal class Conn
    {
        static public string Username { get; set; }
        static public string Password { get; set; }

        public static bool Acces = true;
        public static TcpClient _client { get; set; }
        public static NetworkStream _stream { get; set; }
        private Guid Guid = new Guid();
        MessagePacket messagePacket = new MessagePacket();
        LoginRequest loginRequest = new LoginRequest();
        PacketWriter packetWriter = new PacketWriter();
        PacketReader packetReader = new PacketReader();
        public Conn()
        {
            _client = new TcpClient("127.0.0.1", 13000);
            _stream = _client.GetStream();
            messagePacket.guid = this.Guid;
            loginRequest.Username = Username;
            loginRequest.Password = Password;
            loginRequest.guid = Guid;
            packetWriter.WritePacket(loginRequest);
            packetWriter.Flush(_stream);
            Task.Run(async () => await ReceiveMessagesAsync()); // Uruchomienie asynchronicznej metody do odbierania wiadomości
        }

        public async Task SendMessageToServerAsync(string message)
        {
            messagePacket.message = message;
            packetWriter.WritePacket(messagePacket);
            packetWriter.Flush(_stream);
        }

        private async Task ReceiveMessagesAsync()
        {
            while (true)
            {

                byte[] buffer = new byte[1024];
                int bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length);
                packetReader.ReadPacket(buffer);
            }
        }
    }
}