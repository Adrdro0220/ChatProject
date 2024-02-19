using Newtonsoft.Json;
using Server;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class PacketReader
    {
        public byte[] PacketData { get; set; }
        public int Id { get; set; }
        public int PayloadLength { get; set; }
        public byte[] binJsonEncrypted { get; set; }
        public string Json { get; set; }
        public object Object { get; set; }

        public PacketReader(byte[] packetData)
        {
            this.PacketData = packetData;
            ParsePacket();
        }

        private void ParsePacket()
        {
            // Extract Packet ID
            Id = BitConverter.ToInt32(PacketData, 0);

            // Extract payload length
            PayloadLength = BitConverter.ToInt32(PacketData, 1);

            // Extract encrypted JSON
            binJsonEncrypted = new byte[PayloadLength];
            Buffer.BlockCopy(PacketData, 6, binJsonEncrypted, 0, PayloadLength);

            // Decrypt JSON
            Json = EncryptionToServer.DecryptMessage(binJsonEncrypted).Result;

            // Deserialize object
            Object = JsonConvert.DeserializeObject(Json);
        }
    }
}
