using ConsoleApp1;
using Server;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace User
{
    internal class Conn
    {
        static public string Username { get; set; }
        static public string Password { get; set; }

        public string EncryptionKey { get { return "KluczZabezpiecza"; } }
        public TcpClient _client { get; set; }
        public NetworkStream _stream { get; set; }
        public Conn()
        {
            _client = new TcpClient("127.0.0.1", 13000);
            _stream = _client.GetStream();
            Task.Run(async () => await ReceiveMessagesAsync()); // Uruchomienie asynchronicznej metody do odbierania wiadomości
        }

        public async Task SendMessageToServerAsync(string message)
        {
            var encryptedData = DecryptionFromServer.EncryptMessage(message);
            await _stream.WriteAsync(encryptedData.Result, 0, encryptedData.Result.Length);
        }

        private async Task ReceiveMessagesAsync()
        {
            while (true)
            {
                byte[] id = new byte[1];
                await _stream.ReadAsync(id, 0, id.Length);
                byte[] lenght = new byte[4];
                await _stream.ReadAsync(lenght, 0, lenght.Length);
                foreach (var item in lenght)
                {
                    await Console.Out.WriteLineAsync((char)item);
                }
                int len = BitConverter.ToInt32(lenght, 0);
                byte[] data = new byte[len];
                await _stream.ReadAsync(data, 0, len);
                PacketReader temp = new PacketReader(data, id, len);


            }
        }

        public static bool ServerAcces() 
        {
            SqlConnection ConnectionToDB = new SqlConnection(@"Data Source = ADI\SQLEXPRESS;Initial Catalog=RegisterToChatProject;Integrated Security=True");
            DataTable dtable = new DataTable();
            try
            {
                ConnectionToDB.Open();
                string query = "SELECT id FROM Users WHERE UserN =  '"+Username+"' and PassW = '"+Password+"'";
                SqlDataAdapter SDA = new SqlDataAdapter(query, ConnectionToDB);
                SDA.SelectCommand.ExecuteNonQuery();
                SDA.Fill(dtable);

            }
             
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message );
            }
            finally
            {
                ConnectionToDB.Close();
            }
            if (dtable.Rows.Count == 0)
            {
                Console.WriteLine("Connection to server failed");
                return false;
            }
            else
            {
                Console.WriteLine("Successfully connected to server");              
                return true;
            }
        }
    }
}
