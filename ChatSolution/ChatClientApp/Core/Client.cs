using User;
namespace ChatClientApp.Core;


public class Client
{
    private static Conn Con;

    static Client()
    {
        Con = new Conn();
    }
    
    public static Conn GetConnectionInstance()
    {
        return Con;
    }
    
    
}