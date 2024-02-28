
﻿
using Server;
using System.Text;

﻿using Server;



Console.WriteLine("Hello, World!");

TcpServer tcpServer = new TcpServer();

while (true)
{
    tcpServer.BroadcastToClients();
}
Console.ReadLine();