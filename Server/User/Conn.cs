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
        public static bool Acces = false;
        public string EncryptionKey { get { return "KluczZabezpiecza"; } }
        public static TcpClient _client { get; set; }
        public static NetworkStream _stream { get; set; }
        public Conn()
        {
            _client = new TcpClient("127.0.0.1", 13000);
            _stream = _client.GetStream();

            UserTmp user = new UserTmp();
            user.Username = Username;
            user.Password = Password;
            string Json = JsonConvert.SerializeObject(user);

            dynamic dJson = JsonConvert.DeserializeObject(Json);
            Console.WriteLine(dJson);


            PacketWriter packet = new PacketWriter(Json, "LoginRequest");
            packet.AssemblePacket();
            _stream.Write(packet.PacketReadyToSent, 0, packet.PacketReadyToSent.Length);


            Task.Run(async () => await ReceiveMessagesAsync()); // Uruchomienie asynchronicznej metody do odbierania wiadomości
        }

        public async Task SendMessageToServerAsync(string message)
        {
            PacketWriter packet = new PacketWriter(message, "SentMessage");
            await packet.AssemblePacket();
            var tcpStream = _client.GetStream();
            await tcpStream.WriteAsync(packet.PacketReadyToSent, 0, packet.PacketReadyToSent.Length);
        }

        private async Task ReceiveMessagesAsync()
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
}