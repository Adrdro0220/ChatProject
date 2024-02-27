using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatProtocol
{
    public interface IPacket
    {
        int PacketId();
        string Serialize();
    }
}
