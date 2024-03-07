namespace ChatProtocol;

public interface IPacket
{
    int PacketId();
    string Serialize();
}