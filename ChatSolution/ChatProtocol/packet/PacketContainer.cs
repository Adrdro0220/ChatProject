namespace ChatProtocol;

public class PacketContainer : IPacket
{
    public required byte[] Payload { get; set; }

    public int PayloadLenght { get; set; }

    public int Id { set; get; }

    public int PacketId()
    {
        return Id;
    }

    string IPacket.Serialize()
    {
        throw new Exception("Cannot serialize packet");
    }
}