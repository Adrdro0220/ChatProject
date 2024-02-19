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
        public int PayloadLength { get; set; }
        public byte[] binJsonEncrypted { get; set; }
        public string Json { get; set; }
        public object Object { get; set; }

        public PacketReader(byte[] packetData , byte[]id ,int payloadLength)
        {
            this.PacketData = packetData;

            ParsePacket();

            byte[] idArray = new byte[1];

            Buffer.BlockCopy(packetData, 1, idArray, 0, idArray.Length);

            Id = id[0];

            PayloadLength = payloadLength;
        }

        
        private void ParsePacket()
        {
            // Extract encrypted JSON
            binJsonEncrypted = new byte[PayloadLength];
            Buffer.BlockCopy(PacketData, 5, binJsonEncrypted, 0, PayloadLength); // Zmiana indeksu na 5

            // Decrypt JSON
            Json = DecryptionFromServer.DecryptMessage(binJsonEncrypted).Result;

            // Deserialize object
            Object = JsonConvert.DeserializeObject(Json);
        }

    }
}
