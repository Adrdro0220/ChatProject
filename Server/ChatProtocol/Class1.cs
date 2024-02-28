using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace ChatProtocol
{


    public class MessagePacket : IPacket
    {
        public string message;
        public Guid guid;

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
        public string Username { get; set; }
        public string Password { get; set; }

        public Guid guid;

        public int PacketId()
        {
            return 1;
        }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }
    }


}

   
