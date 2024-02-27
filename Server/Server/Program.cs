<<<<<<< HEAD
﻿
using Server;
using System.Text;
=======
﻿using Server;

>>>>>>> 3ff75ecd2dff62f53cf8ba41dc4ab8d81890bf36

Console.WriteLine("Hello, World!");

TcpServer tcpServer = new TcpServer();

while (true)
{
    tcpServer.BroadcastToClients();
}
Console.ReadLine();