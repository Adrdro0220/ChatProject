using ChatProtocol;
using Newtonsoft.Json;
using Server;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using static ChatProtocol.Class1;

namespace ConsoleApp1
{
    public class PacketReader
    {
        private int indicator = 0;
        public PacketContainer packetContainer = new PacketContainer();
        public PacketReader() { }
        public void ReadPacket(byte[] binPacket)
        {
            if (indicator == 0)
            {
                int offset = 0;
                packetContainer.Id = Convert.ToInt32(GetSpecificBYtes(binPacket, offset, 4));
                offset += 4;
                packetContainer.PayloadLenght = Convert.ToInt32(GetSpecificBYtes(binPacket, offset, 4));
                offset += 4;
                packetContainer.Payload = GetSpecificBYtes(binPacket, offset, binPacket.Length);
            }
            if (indicator == 1)
            {
                int offset = 0;
                packetContainer.Payload = packetContainer.Payload.Concat(GetSpecificBYtes(binPacket, offset, binPacket.Length)).ToArray();

                if (packetContainer.PayloadLenght == 0)
                {
                    indicator = 0;
                    throw new Exception("Packet was empty");
                    return;
                }
                if (packetContainer.PayloadLenght > packetContainer.Payload.Length)
                {
                    //letting know my object that next packet will be just next part of the same packet 
                    indicator = 1;
                    return;
                }
                if (packetContainer.PayloadLenght == packetContainer.Payload.Length)
                {
                    //Convert payload to object 
                    try
                    {
                        var Json = AssemblePacket(packetContainer.Payload);
                    }catch (Exception ex) { Console.WriteLine(ex.Message); }

                    //Prepare object for new packet to come 
                     indicator = 0;
                    packetContainer = new PacketContainer();
                }
            }
        }

        private object AssemblePacket(byte[] binPacket)
        {
            string _json = EncryptionToServer.DecryptMessage(binPacket).Result;
            switch (packetContainer.Id)
            {
                case 0:
                    return JsonConvert.DeserializeObject<Class1.Message>(_json);
                    break;

                case 1:
                    return JsonConvert.DeserializeObject<Class1.UserTmp>(_json);
                    break;

            }
            throw new Exception("convertion exception Packet not found!");
            return null;
        }

            private async Task Case1(TcpClient client)
            {
                string result = PacketMethods.ReturnDbQuerryAnswer(Json).Result;
                PacketWriter packet = new PacketWriter(result, "LoginRequest");
                await packet.AssemblePacket();
                var tcpStream = client.GetStream();
                await tcpStream.WriteAsync(packet.PacketReadyToSent, 0, packet.PacketReadyToSent.Length);
                AddUserToDictionary(Json, client);
            }

            private async Task AddUserToDictionary(string json, TcpClient client)
            {
                PacketMethods.GetUsernameAndPassword(json, out string username, out string password);
                TcpServer._clients.Add(username, client);
            }

            private byte[] GetSpecificBYtes(byte[] array, int offset, int arrayLenght)
            {

                byte[] data = new byte[arrayLenght];

                for (int i = 0; i < arrayLenght; i++)
                {
                    data[i] = array[offset + i];
                }
                return data;
            }
        }
    }
