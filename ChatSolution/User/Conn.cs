using System.Net.Sockets;
using ChatProtocol;

namespace User;

internal class Conn
{
    public static bool Acces = false;
    private  Guid _guid = new();
    private Handler _handler;

    public Conn()
    {
        Client = new TcpClient("127.0.0.1", 13000);
        Stream = Client.GetStream();
        _handler = new();
        _handler.MessagePacket.Guid = _guid;
        _handler.NetworkStream = Stream;
        _handler.LoginRequest.Username = Username;
        _handler.LoginRequest.Password = Password;
        _handler.LoginRequest.Guid = _guid;
        _handler.PacketWriter.WritePacket(_handler.LoginRequest);
        _handler.PacketWriter.Flush(Stream);

        _handler.Guid = _guid;
         Task.Run(async () => await ReceiveMessagesAsync()); // Uruchomienie asynchronicznej metody do odbierania wiadomości
        
    }

    public static string Username { get; set; }
    public static string Password { get; set; }
    public static TcpClient Client { get; set; }
    public static NetworkStream Stream { get; set; }

    public async Task SendMessageToServerAsync(string message)
    {
        _handler.MessagePacket.Message = message;
        _handler.PacketWriter.WritePacket(_handler.MessagePacket);
       await _handler.PacketWriter.Flush(Stream);
    }

    private async Task ReceiveMessagesAsync()
    {
        while (true)
        {
            Console.WriteLine("Odbieranie wiadomości");
            var buffer = new byte[1024];
            var bytesRead = await Stream.ReadAsync(buffer, 0, buffer.Length);
            _handler.PacketRead = _handler.PacketReader.ReadPacket(buffer);

            if (_handler.PacketRead is LoginResponse)
            {
                if (_handler.HandleLoginResponse(_handler.PacketRead) == "Accept")
                {
                    Console.WriteLine("Acces granted");
                    Acces = true;
                }
                else
                {
                    Acces = false;
                }
            }
        }
    }
}