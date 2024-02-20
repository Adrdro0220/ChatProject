using Newtonsoft.Json;
using Org.BouncyCastle.Bcpg;
using Server;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using User;

namespace ConsoleApp1
{
    class PacketReader
    {
        public byte[] PacketData { get; set; }
        public int Id { get; set; }
        public byte[] IdBytes { get; set; }
        public byte[] PayloadLenghtBytes { get; set; }
        public int PayloadLength { get; set; }
        public byte[] binJsonEncrypted { get; set; }
        public string Json { get; set; }
        public object Object { get; set; }
        public int Offset { get; set; } = 0;
        public byte[] PayloadByte { get; set; }
        public byte[] DirectionInput { get; set; }
        public byte[] Direction { get { return BitConverter.GetBytes(0); } }

        public PacketReader(byte[] packetData , TcpClient client)
        {
            this.PacketData = packetData;

            IdBytes = ByteReader.GetSpecificBYtes(packetData, Offset, 4).Result;
            Id = ByteReader.GetId(IdBytes).Result;
            Offset += 4;
            PayloadLenghtBytes = ByteReader.GetSpecificBYtes(packetData, Offset, 4).Result;
            PayloadLength = ByteReader.GetPayloadLength(PayloadLenghtBytes).Result;

            Offset += 4;

            PayloadByte = ByteReader.GetSpecificBYtes(packetData, Offset, PayloadLength).Result;

            Json = EncryptionToServer.DecryptMessage(PayloadByte).Result;

            //Json = PacketMethods.RemoveCharacterPairs(Json, "\a");
            switch (Id)
            {
                case 0:
                  
                    break;
                
                case 1:
                    Case1(client);
                    break;
            }
        }
        private async Task Case1(TcpClient client)
        {
            string json = PacketMethods.DeleteFirstAndLastCharFromString(Json).Result;
            string result = PacketMethods.ReturnDbQuerryAnswer(Json).Result;
            PacketWriter packet = new PacketWriter(result, "SentMessage");
            await packet.AssemblePacket();
            var tcpStream = client.GetStream();
            await tcpStream.WriteAsync(packet.PacketReadyToSent, 0, packet.PacketReadyToSent.Length);
        }
    }
}
