
using System;
using System.Threading.Tasks;
using User;

class Program
{
    static async Task Main()
    {
        
        await Console.Out.WriteLineAsync("Username...");
        Conn.Username = Console.ReadLine();
        await Console.Out.WriteLineAsync("Password...");
        Conn.Password = Console.ReadLine();

        Conn conn = new Conn();
        while (!Conn.Acces)
        {
            Thread.Sleep(1000);
        }
        if (Conn.Acces)
        {
           
            
            while (true)
            {
                await conn.SendMessageToServerAsync(Console.ReadLine());
            }
        }
        else
        {
            await Console.Out.WriteLineAsync("Sorry you are not one of our users");
            Thread.Sleep(2000);

        }
        
       
    }
}
