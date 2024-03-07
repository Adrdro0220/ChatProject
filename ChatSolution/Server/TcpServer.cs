// TcpServer.cs

using System.Net;
using System.Net.Sockets;
using ChatProtocol;

namespace Server;

public class TcpServer
{
    public static Dictionary<Guid, TcpClient> Clients = new();
    private Handler _handler = new Handler();
    private int _id;

    public TcpServer()
    {
       
        StartServer();
        
    }

    public NetworkStream Stream { get; set; }
    public TcpListener Listener { get; set; }

    private async void StartServer()
    {

        var port = 13000;
        var hostAddress = IPAddress.Parse("127.0.0.1");
        Listener = new TcpListener(hostAddress, port);
        Listener.Start();
            

        while (true)
        {
            var client = await Listener.AcceptTcpClientAsync();
            Clients.Add(Guid.NewGuid(), client);
            Console.WriteLine("Klient został połączonys");

            // Start asynchronous data reading for the new client
            Task.Run(() => HandleClientAsync(client));
        }
    }

    private async Task HandleClientAsync(TcpClient client)
    {
        var clientStream = client.GetStream();
        var buffer = new byte[1024];

        while (true)
        {
            Console.WriteLine("Odbieranie wiadomości");

            var bytesRead = await clientStream.ReadAsync(buffer, 0, buffer.Length);
            if (bytesRead > 0) {_handler.PacketRead = _handler.PacketReader.ReadPacket(buffer);}
           
        }
    }


    public async Task BroadcastToClients()
    {
        var message = Console.ReadLine();
        foreach (var connectedClient in Clients) await SendToClient(connectedClient.Value, message);
    }


    private async Task SendToClient(TcpClient client, string message)
    {
        try
        {
            _handler.MessagePacket.Message = message;
            _handler.PacketWriter.WritePacket(_handler.MessagePacket);
            await  _handler.PacketWriter.Flush(client.GetStream());
        }
        
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
        }
    }
}