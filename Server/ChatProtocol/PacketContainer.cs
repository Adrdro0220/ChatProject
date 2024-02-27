using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatProtocol
{
    public class PacketContainer : IPacket
    {
        private int _id;
        private int _payloadLen;
        private byte[] _payload;
        public int PacketId()
        {
            return _id;
        }

        public byte[] Payload
        {
            get { return _payload; }
            set { _payload = value; }
        }

        public int PayloadLenght
        {
            get { return _payloadLen; }
            set { _payloadLen = value; }
        }
        public int Id {  
            set
            {
                _id = value;
            }
        }

        string IPacket.Serialize()
        {
            throw new Exception("Cannot serialize packet");
        }
    }
}
