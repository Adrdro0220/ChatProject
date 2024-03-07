using User;

internal class Program
{
    private static async Task Main()
    {
        await Console.Out.WriteLineAsync("Username...");
        Conn.Username = Console.ReadLine();
        await Console.Out.WriteLineAsync("Password...");
        Conn.Password = Console.ReadLine();

        var conn = new Conn();
        while (!Conn.Acces) Thread.Sleep(1000);
        
        if (Conn.Acces)
        {
            while (true) await conn.SendMessageToServerAsync(Console.ReadLine());
        }

        await Console.Out.WriteLineAsync("Sorry you are not one of our users");
        Thread.Sleep(2000);
    }
}