using Server;

Console.WriteLine("Hello, World!");

var tcpServer = new TcpServer();

while (true) tcpServer.BroadcastToClients();
Console.ReadLine();