using ChatProtocol;
using k8s;
using Newtonsoft.Json;
using Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class PacketWriter
    {

        public byte[] PacketReadyToSent { get; set; }

        public PacketWriter()
        {
        }

        static public Dictionary<int, string> Dict = new Dictionary<int, string>()
        {
            {  0,"SentMessage" },
            {   1 ,"LoginRequest" }
        };

        public void WritePacket(IPacket packet)
        {
            if (packet is PacketContainer)
            {
                
                var packetContainer = (PacketContainer)packet;
                // Id pakietu
                byte[] bufferId = BitConverter.GetBytes(packetContainer.PacketId());
                PacketReadyToSent = bufferId;

                // Długość ładunku pakietu
                byte[] bufferLength = BitConverter.GetBytes(packetContainer.PayloadLenght); // Zmienione, aby prawidłowo obsłużyć długość

                // Składanie pakiet
                PacketReadyToSent =  AppendByteArrays(PacketReadyToSent, bufferLength);
                PacketReadyToSent =  AppendByteArrays(PacketReadyToSent, packetContainer.Payload);
                return ;
            }
            var rawPacket1  = ToPacketContainer(packet);
            
            // Id pakietu
            byte[] bufferId1 = BitConverter.GetBytes(rawPacket1.PacketId());
            PacketReadyToSent = bufferId1;

            // Długość ładunku pakietu
            byte[] bufferLength1 = BitConverter.GetBytes(rawPacket1.PayloadLenght);

            // Składanie pakiet
            PacketReadyToSent = AppendByteArrays(PacketReadyToSent, bufferLength1);
            PacketReadyToSent = AppendByteArrays(PacketReadyToSent, rawPacket1.Payload);
            return;
        }

        public async Task Flush(NetworkStream networkStream)
        {
            await networkStream.WriteAsync(PacketReadyToSent);
            PacketReadyToSent = new byte[0];
        }

        private PacketContainer ToPacketContainer(IPacket packet)
        {
            PacketContainer packetContainer = new PacketContainer();
            packetContainer.Id = packet.PacketId();
            packetContainer.Payload = EncryptionToServer.EncryptMessage(packet.Serialize()).Result;
            packetContainer.PayloadLenght = packetContainer.Payload.Length;
            return packetContainer;
        }

        private byte[] AppendByteArrays(byte[] array1, byte[] array2)
        {
            byte[] result = new byte[array1.Length + array2.Length];
            Buffer.BlockCopy(array1, 0, result, 0, array1.Length);
            Buffer.BlockCopy(array2, 0, result, array1.Length, array2.Length);
            return result;
        }
    }
}
