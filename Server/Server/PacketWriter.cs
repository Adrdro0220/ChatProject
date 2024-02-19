using Newtonsoft.Json;
using Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class PacketWriter
    {

        public PacketWriter(object obj, string idName)
        {
            this.Object = obj;

            Json = JsonConvert.SerializeObject(Object);

            binJsonEncrypted = Encoding.UTF8.GetBytes(Json);

            foreach (var item in Dict)
            {
                if (item.Value == idName)
                {
                    Id = item.Key;
                }
            }
        }


        static public Dictionary<int, string> Dict = new Dictionary<int, string>()
        {
            {  0,"SentMessage" }
        };

        public byte[] binJsonEncrypted { get; set; }
        public string Json { get; set; }
        public int Id { get; set; }
        public int PayloadLenght { get; set; }
        public object Object { get; set; }
        public byte[] Direction { get { return BitConverter.GetBytes(1); } }

        public byte[] PacketReadyToSent { get; set; }



        public async Task AssemblePacket()
        {
            // Id pakietu
            byte[] bufferId = BitConverter.GetBytes(Id);
            PacketReadyToSent = bufferId;
            await Console.Out.WriteLineAsync($" PacketReadyToSent {PacketReadyToSent}");

            // Szyfrowanie ładunku
            binJsonEncrypted = EncryptionToServer.EncryptMessage(Json).Result;
           

            foreach (var item in binJsonEncrypted)
            {
                await Console.Out.WriteLineAsync((char)item);
            }

            PayloadLenght = binJsonEncrypted.Length;

            // Długość ładunku pakietu
            byte[] bufferLength = BitConverter.GetBytes(PayloadLenght); // Zmienione, aby prawidłowo obsłużyć długość

            // Składanie pakietu
            PacketReadyToSent = await AppendByteArrays(PacketReadyToSent, bufferLength);
            PacketReadyToSent = await AppendByteArrays(PacketReadyToSent, binJsonEncrypted);
            int pom = BitConverter.ToInt32(bufferLength, 0);
            await Console.Out.WriteLineAsync((char)pom);
        }




        public async Task<byte[]> AppendByteArrays(byte[] array1, byte[] array2)
        {
            byte[] result = new byte[array1.Length + array2.Length];
            Buffer.BlockCopy(array1, 0, result, 0, array1.Length);
            Buffer.BlockCopy(array2, 0, result, array1.Length, array2.Length);
            return result;
        }


 
        public async Task< byte[] > FillUnassignedWithZeros(byte[] originalArray, int newSize)
        {
            byte[] newArray = new byte[newSize];
            Buffer.BlockCopy(originalArray, 0, newArray, 0, Math.Min(originalArray.Length, newSize));
            return newArray;
        }




    }
}
