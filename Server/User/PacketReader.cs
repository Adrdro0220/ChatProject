using Newtonsoft.Json;
using Server;
using System;
using System.Collections.Generic;
using System.Text;
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
        public PacketReader(byte[] packetData )
        {
           this.PacketData = packetData;

            IdBytes = ByteReader.GetSpecificBYtes(packetData, Offset, 4).Result;
            Id = ByteReader.GetId(IdBytes).Result;
            Offset += 4;
            PayloadLenghtBytes = ByteReader.GetSpecificBYtes(packetData, Offset, 4).Result;
            PayloadLength = ByteReader.GetPayloadLength(PayloadLenghtBytes).Result;

            Offset += 4;

            PayloadByte = ByteReader.GetSpecificBYtes(packetData, Offset, PayloadLength).Result;
            
            Json = DecryptionFromServer.DecryptMessage(PayloadByte).Result;
            Console.WriteLine(Json);
            Object = JsonConvert.DeserializeObject<object>(Json);
            
           
        }  
    }
}
