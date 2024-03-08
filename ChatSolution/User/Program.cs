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
        
        Thread.Sleep(3000);
        if (Conn.Acces)
        {
            await Console.Out.WriteLineAsync("You are one of our users Welcome");
    
            // Conn.Password =  Console.ReadLine();
            // Conn.Username =  Console.ReadLine();
            // Conn.Email =  Console.ReadLine();
            //
            // conn.SendRegisterRequest( Conn.Username, Conn.Password, Conn.Email);
            //
            while (true) await conn.SendMessageToServerAsync(Console.ReadLine());
        }
        
        await Console.Out.WriteLineAsync("Sorry you are not one of our users");

        Console.ReadLine();
    }
}