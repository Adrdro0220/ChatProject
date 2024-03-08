using Newtonsoft.Json;

namespace ChatProtocol;

public class MessagePacket : IPacket
{
    public Guid Guid;
    public required string Message;

    public int PacketId()
    {
        return 0;
    }

    public string Serialize()
    {
        return JsonConvert.SerializeObject(this);
    }
}

public class LoginRequest : IPacket
{
    public Guid Guid;
    public required string Username { get; set; }
    public required string Password { get; set; }

    public int PacketId()
    {
        return 1;
    }

    public string Serialize()
    {
        return JsonConvert.SerializeObject(this);
    }
}

public class LoginResponse : IPacket
{
    public Guid Guid;
    public required string Message;
    
    public int PacketId()
    {
        return 2;
    }

    public string Serialize()
    {
        return JsonConvert.SerializeObject(this);
    }
   
}

public class RegisterRequest : IPacket
{
    public Guid Guid;
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string Email { get; set; }

    public int PacketId()
    {
        return 3;
    }

    public string Serialize()
    {
        return JsonConvert.SerializeObject(this);
    }
}
public class RegisterResponse : IPacket
{
    public Guid Guid;
    public required string Message;
    
    public int PacketId()
    {
        return 4;
    }

    public string Serialize()
    {
        return JsonConvert.SerializeObject(this);
    }
}