// See https://aka.ms/new-console-template for more information
using Server;

Console.WriteLine("Hello, World!");
TcpServer tcpServer = new TcpServer();

while (true)
{
    tcpServer.BroadcastToClients();
}
Console.ReadLine();