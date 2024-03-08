using System.Reflection.Metadata;
using ChatProtocol;
using Newtonsoft.Json;
using Server;

namespace ConsoleApp1;

public class PacketReader
{
    private Handler _handler;
    private int _indicator;
    public PacketContainer packetContainer;
    public IPacket ObjectRead;
    public PacketReader(Handler handler)
    {
        _handler = handler;
        packetContainer = new PacketContainer
        {
            Payload = Array.Empty<byte>(),
            PayloadLenght = 0,
            Id = 0
        };
    }

    public IPacket ReadPacket(byte[] binPacket)
    {
        Console.WriteLine("reading packet");
        if (_indicator == 0)
        {
            var offset = 0;
            try
            {
                packetContainer.Id = BitConverter.ToInt32(GetSpecificBYtes(binPacket, offset, 4));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            offset += 4;
            packetContainer.PayloadLenght = BitConverter.ToInt32(GetSpecificBYtes(binPacket, offset, 4));
            offset += 4;
            try
            {
                packetContainer.Payload = GetSpecificBYtes(binPacket, offset, binPacket.Length - 8);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        if (_indicator == 1)
        {
            var offset = 0;
            packetContainer.Payload = packetContainer.Payload
                .Concat(GetSpecificBYtes(binPacket, offset, binPacket.Length)).ToArray();
        }

        if (packetContainer.PayloadLenght == 0)
        {
            _indicator = 0;
            throw new Exception("Packet was empty");

            return null;
        }

        if (packetContainer.PayloadLenght > packetContainer.Payload.Length)
        {
            //letting know my object that next packet will be just next part of the same packet 
            _indicator = 1;
            return null;
        }

        //Convert payload to object 
        ObjectRead = AssemblePacket(packetContainer.Payload);
        Console.WriteLine(ObjectRead);
        _handler.Handle();

        //Prepare object for next packet to come 
        _indicator = 0;
        packetContainer = new PacketContainer { Payload = new byte[0] };
        return ObjectRead;
    }

    private IPacket AssemblePacket(byte[] binPacket)
    {
        var json = EncryptionToServer.DecryptMessage(binPacket).Result;
        json = TrimJson(json);
        switch (packetContainer.Id)
        {
            case 0:
                return JsonConvert.DeserializeObject<MessagePacket>(json);

                break;

            case 1:
                try
                {
                    return JsonConvert.DeserializeObject<LoginRequest>(json);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                break;
            case 2:

                return JsonConvert.DeserializeObject<LoginResponse>(json);
                break;
            case 3:
                return JsonConvert.DeserializeObject<RegisterRequest>(json);
                break;
        }

        throw new Exception("Convertion Exception. Packet not found!");
        return null;
    }

    private byte[] GetSpecificBYtes(byte[] array, int offset, int arrayLenght)
    {
        var data = new byte[arrayLenght];

        for (var i = 0; i < arrayLenght; i++) data[i] = array[offset + i];
        return data;
    }

    private string TrimJson(string json)
    {
        var index = json.IndexOf('}') + 1; 
        if (index < json.Length)
            json = json.Substring(0, index); 
        return json;
    }
}