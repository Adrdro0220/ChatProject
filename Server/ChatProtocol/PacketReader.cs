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


namespace ConsoleApp1
{
    public class PacketReader
    {
        private int indicator = 0;
        public PacketContainer packetContainer = new PacketContainer();
        public PacketReader() { }
        public object ReadPacket(byte[] binPacket)
        {
            if (indicator == 0)
            {
                int offset = 0;
                try
                {
                    packetContainer.Id = BitConverter.ToInt32(GetSpecificBYtes(binPacket, offset, 4));

                }catch(Exception ex) { Console.WriteLine(ex.Message); }
                offset += 4;
                packetContainer.PayloadLenght = BitConverter.ToInt32(GetSpecificBYtes(binPacket, offset, 4));
                offset += 4;
                try
                {
                    packetContainer.Payload = GetSpecificBYtes(binPacket, offset, binPacket.Length-8);

                }
                catch (Exception ex) { Console.WriteLine(ex.Message); } 
            }
            if (indicator == 1)
            {
                int offset = 0;
                packetContainer.Payload = packetContainer.Payload.Concat(GetSpecificBYtes(binPacket, offset, binPacket.Length)).ToArray();
            }
            if (packetContainer.PayloadLenght == 0)
                {
                    indicator = 0;
                    throw new Exception("Packet was empty");

                return null;
                }
            
                if (packetContainer.PayloadLenght > packetContainer.Payload.Length)
                {
                    //letting know my object that next packet will be just next part of the same packet 
                    indicator = 1;
                    return null;
                }
                else
                {
                    //Convert payload to object 
                    var Json =  AssemblePacket(packetContainer.Payload);


                    if (Json is MessagePacket)
                    {
                        MessagePacket obj = Json as MessagePacket;
                        Console.WriteLine(obj.message);
                    }

                    if(Json is LoginRequest)
                    {
                    
                        LoginRequest obj = Json as LoginRequest;
                    Console.WriteLine(obj.Username);
                    Console.WriteLine(obj.Password);
                    
                }





                    //Prepare object for next packet to come 
                     indicator = 0;
                    packetContainer = new PacketContainer();
                    return Json;
                }

            

        }

        private object AssemblePacket(byte[] binPacket)
        {
            string _json = EncryptionToServer.DecryptMessage(binPacket).Result;
            _json = TrimJson(_json);
            switch (packetContainer.Id)
            {
                case 0:
                    return JsonConvert.DeserializeObject<MessagePacket>(_json);
                    
                    break;

                case 1:
                    try
                    {
                        return JsonConvert.DeserializeObject<LoginRequest>(_json);
                    }catch (Exception ex) { Console.WriteLine(ex.Message); }
                   
                    break;

            }
            throw new Exception("Convertion Exception. Packet not found!");
            return null;
        }

          //  private async Task Case1(TcpClient client)
          //  {
           //     string result = PacketMethods.ReturnDbQuerryAnswer(Json).Result;
            //    PacketWriter packet = new PacketWriter(result, "LoginRequest");
           //     await packet.AssemblePacket();
           //     var tcpStream = client.GetStream();
           //     await tcpStream.WriteAsync(packet.PacketReadyToSent, 0, packet.PacketReadyToSent.Length);
            //    AddUserToDictionary(Json, client);
          //  }

          //  private async Task AddUserToDictionary(string json, TcpClient client)
          //  {
           //     PacketMethods.GetUsernameAndPassword(json, out string username, out string password);
          //      TcpServer._clients.Add(username, client);
         //   }

            private byte[] GetSpecificBYtes(byte[] array, int offset, int arrayLenght)
            {

                byte[] data = new byte[arrayLenght];

                for (int i = 0; i < arrayLenght; i++)
                {
                    data[i] = array[offset + i];
                }
                return data;
            }

            private string TrimJson(string json)
        {
            int index = json.IndexOf('}') + 1; // znajdź indeks pierwszego nawiasu zamykającego
            if (index < json.Length) // jeśli istnieją dodatkowe znaki
            {
                json = json.Substring(0, index); // usuń dodatkowe znaki
            }
            return json;
        }
        }
    }
