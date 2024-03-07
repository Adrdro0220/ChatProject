using System.Net.Sockets;
using ChatProtocol;
using Server;

namespace ChatProtocol.Writer;

public class PacketWriter
{
    private Handler _handler;

    public PacketWriter(Handler handler)
    {
        this._handler = handler;
    }

    public byte[]? PacketReadyToSent { get; set; }

    public void WritePacket(IPacket packet)
    {
        if (packet is PacketContainer)
        {
            var packetContainer = (PacketContainer)packet;
            // Id pakietu
            var bufferId = BitConverter.GetBytes(packetContainer.PacketId());
            PacketReadyToSent = bufferId;

            // Długość ładunku pakietu
            var bufferLength =
                BitConverter.GetBytes(packetContainer.PayloadLenght); // Zmienione, aby prawidłowo obsłużyć długość

            // Składanie pakiet
            PacketReadyToSent = AppendByteArrays(PacketReadyToSent, bufferLength);
            PacketReadyToSent = AppendByteArrays(PacketReadyToSent, packetContainer.Payload);
            return;
        }

        var rawPacket1 = ToPacketContainer(packet);

        // Id pakietu
        var bufferId1 = BitConverter.GetBytes(rawPacket1.PacketId());
        PacketReadyToSent = bufferId1;

        // Długość ładunku pakietu
        var bufferLength1 = BitConverter.GetBytes(rawPacket1.PayloadLenght);

        // Składanie pakiet
        PacketReadyToSent = AppendByteArrays(PacketReadyToSent, bufferLength1);
        PacketReadyToSent = AppendByteArrays(PacketReadyToSent, rawPacket1.Payload);
    }

    public async Task Flush(NetworkStream stream)
    {
       await stream.WriteAsync(PacketReadyToSent);
    }

    private PacketContainer ToPacketContainer(IPacket packet)
    {
        var packetContainer = new PacketContainer
        {
            Id = packet.PacketId(),
            Payload = EncryptionToServer.EncryptMessage(packet.Serialize())
                .Result
        };
        packetContainer.PayloadLenght = packetContainer.Payload.Length;
        return packetContainer;
    }

    private byte[] AppendByteArrays(byte[] array1, byte[] array2)
    {
        var result = new byte[array1.Length + array2.Length];
        Buffer.BlockCopy(array1, 0, result, 0, array1.Length);
        Buffer.BlockCopy(array2, 0, result, array1.Length, array2.Length);
        return result;
    }
}