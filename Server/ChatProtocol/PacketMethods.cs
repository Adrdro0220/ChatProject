using ChatProtocol;
using System.Data;
using System.Data.SqlClient;


namespace Server
{

    internal class PacketMethods
    {

        public static async Task<string> ReturnDbQuerryAnswer(LoginRequest obj)
        {
            string Username;
            string Password;

            GetUsernameAndPassword(obj, out Username, out Password);

            SqlConnection ConnectionToDB = new SqlConnection(@"Data Source = ADI\SQLEXPRESS;Initial Catalog=RegisterToChatProject;Integrated Security=True");
            DataTable dtable = new DataTable();
            try
            {
                ConnectionToDB.Open();
                string query = "SELECT id FROM Users WHERE UserN =  '" + Username + "' and PassW = '" + Password + "'";
                SqlDataAdapter SDA = new SqlDataAdapter(query, ConnectionToDB);
                SDA.SelectCommand.ExecuteNonQuery();
                SDA.Fill(dtable);

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            finally
            {
                ConnectionToDB.Close();
            }

            if (dtable.Rows.Count == 0)
            {
                Console.WriteLine("Connection to DB failed");
                return "Reject";
            }
            else
            {

                Console.WriteLine("Successfully connected to DB");
                return "Accept";
            }
        }


        public static void GetUsernameAndPassword(LoginRequest obj, out string username, out string password)
        {
            username = obj.Username;
            password = obj.Password;
        }


    }

}