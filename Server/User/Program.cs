
using System;
using System.Threading.Tasks;
using User;

class Program
{
    static async Task Main()
    {
        Conn conn = new Conn();

       
        while (true)
        {
            await conn.SendMessageToServerAsync(Console.ReadLine());
        }
    }
}
