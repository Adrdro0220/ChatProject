using System.Data;
using System.Data.SqlClient;
using ChatProtocol;

namespace Server;

internal class PacketMethods
{
    
    public delegate void PacketHandler(IPacket packet, Handler handler);
    public static async Task<string> ReturnDbQuerryAnswer(LoginRequest obj)
    {
        GetUsernameAndPassword(obj, out var username, out var password);

        var query = "SELECT id FROM Users WHERE UserN = @username and PassW = @password";
        var dtable = ExecuteQuery(query, username, password);

        if (dtable.Rows.Count == 0)
        {
            Console.WriteLine("Connection to DB failed");
            return "Reject";
        }

        Console.WriteLine("Successfully connected to DB");
        return "Accept";
    }

    public static DataTable ExecuteQuery(string query, string username, string password)
    {
        var dtable = new DataTable();
        using (var connectionToDb =
               new SqlConnection(
                   @"Data Source = ADI\SQLEXPRESS;Initial Catalog=RegisterToChatProject;Integrated Security=True"))
        {
            try
            {
                connectionToDb.Open();
                using (var command = new SqlCommand(query, connectionToDb))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);
                    using (var sda = new SqlDataAdapter(command))
                    {
                        sda.Fill(dtable);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        return dtable;
    }

    public static void GetUsernameAndPassword(LoginRequest obj, out string username, out string password)
    {
        username = obj.Username;
        password = obj.Password;
    }
}