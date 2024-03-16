using System.Net.Sockets;
using ChatProtocol;
using ChatProtocol.ChatHistory;

namespace User;

public class Conn
{
    public static bool Acces { get; set; } = false;
    public bool acces = Acces;
    public  Guid _guid = new();
    public Handler _handler;
    public History ChatHistory = new History();
    public int PacketCount = 0;
    
    
    public static string Username { get; set; }
    public static string Password { get; set; }
    public static string Email { get; set;}
    public static TcpClient Client { get; set; }
    public static NetworkStream Stream { get; set; }
    
    
    public Conn()
    {
        Client = new TcpClient("127.0.0.1", 13000);
        Stream = Client.GetStream();
        _handler = new();
        _handler.MessagePacket.Guid = _guid;
        _handler.NetworkStream = Stream;
        _handler.Guid = _guid;
        
        
         Task.Run(async () => await ReceiveMessagesAsync()); // Uruchomienie asynchronicznej metody do odbierania wiadomości
         
    }


    public async Task SendMessageToServerAsync(string message)
    {
         message = FilterMessage(message);
        _handler.MessagePacket.Message = message;
        _handler.PacketWriter.WritePacket(_handler.MessagePacket);
       await _handler.PacketWriter.Flush(Stream);
    }

    private async Task ReceiveMessagesAsync()
    {
        while (true)
        {
            
            var buffer = new byte[1024];
            var bytesRead = await Stream.ReadAsync(buffer, 0, buffer.Length);
            _handler.PacketRead =  _handler.PacketReader.ReadPacket(buffer);
            
            if (_handler.PacketRead is LoginResponse)
            {
                
                if (_handler.HandleLoginResponse(_handler.PacketRead) == "Accept")
                {
                    Console.WriteLine("Acces granted");
                    Acces = true;
                    acces = true;
                }
                else
                {
                    Acces = false;
                    acces = false;
                }
            }
            PacketCount++;
            
            Console.WriteLine("packet Read: " + _handler.PacketRead);
        }
    }
    
    public void SendRegisterRequest(string username, string password, string email)
    {
        _handler.RegisterRequest.Username = username;
        _handler.RegisterRequest.Password = password;
        _handler.RegisterRequest.Email = email;
        _handler.RegisterRequest.Guid = _guid;
        _handler.PacketWriter.WritePacket(_handler.RegisterRequest);
        _handler.PacketWriter.Flush(Stream);
    }
    
    public void SendLoginRequest(string username, string password)
    {
        _handler.LoginRequest.Username = username;
        _handler.LoginRequest.Password = password;
        _handler.LoginRequest.Guid = _guid;
        _handler.PacketWriter.WritePacket(_handler.LoginRequest);
        _handler.PacketWriter.Flush(Stream);
    }
    
    
    private string FilterMessage(string message)
    {
        message = message.Replace("}", "");
        return message;
    }
}