using ChatProtocol;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

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
        public byte[] storePacket { get; set; } 

        public PacketReader(byte[] packetData, TcpClient client){}

        public void ReadPacketIdAndlenght(byte[] binPacket)
        {
            int offset = 0;
            PacketContainer packetContainer = new PacketContainer();
            packetContainer.Id = Convert.ToInt32(GetSpecificBYtes(binPacket, offset,4));
            offset+= 4;
            packetContainer.PayloadLenght = Convert.ToInt32(GetSpecificBYtes(binPacket, offset, 4));
            offset+= 4;
            packetContainer.Payload = Encoding.UTF8.GetString(GetSpecificBYtes(binPacket, offset, binPacket.Length))

        } 

        private async Task Case1(TcpClient client)
        {
            string json = PacketMethods.DeleteFirstAndLastCharFromString(Json).Result;
            string result = PacketMethods.ReturnDbQuerryAnswer(Json).Result;
            PacketWriter packet = new PacketWriter(result, "LoginRequest");
            await packet.AssemblePacket();
            var tcpStream = client.GetStream();
            await tcpStream.WriteAsync(packet.PacketReadyToSent, 0, packet.PacketReadyToSent.Length);
            AddUserToDictionary(Json, client);
        }

        private async Task AddUserToDictionary(string json , TcpClient client)
        {
            PacketMethods.GetUsernameAndPassword(json , out string username,out string password);
            TcpServer._clients.Add(username, client);
        }

        private byte[] GetSpecificBYtes(byte[] array, int offset, int arrayLenght)
        {

            byte[] data = new byte[arrayLenght];

            for (int i = 0; i < arrayLenght; i++)
            {
                data[i] = array[offset + i];
            }
            return data;
        }
    }
}