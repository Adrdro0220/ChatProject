using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace ChatProtocol
{
    public class Class1
    {

        public struct Message
        {
            public string Mess;
        }

        public struct UserTmp
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }

    public class MessagePacket : IPacket
    {
        public string message;
        public Guid Guid;

        public int PacketId()
        {
            return 0;
        }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

}

   
