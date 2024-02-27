﻿using ChatProtocol;
using k8s.KubeConfigModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using User;
using static System.Net.Mime.MediaTypeNames;


namespace Server
{

    internal class PacketMethods
    {

        public static async Task<string> ReturnDbQuerryAnswer(string Json)
        {
            string Username;
            string Password;

            GetUsernameAndPassword(Json, out Username, out Password);

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


        public static void GetUsernameAndPassword(string json, out string username, out string password)
        {
            try
            {
                json = json.Replace("\\", "");
                json = json.Substring(1, json.Length - 2);
                Console.WriteLine($"got json {json}");
<<<<<<< HEAD
                dynamic credentials = JsonConvert.DeserializeObject<Class1.Message>(json);
=======
                dynamic credentials = JsonConvert.DeserializeObject<UserTmp>(json);
>>>>>>> 3ff75ecd2dff62f53cf8ba41dc4ab8d81890bf36

                if (credentials != null)
                {
                    username = credentials.Username;
                    password = credentials.Password;
                }
                else
                {
                    // Obsługa przypadku, gdy deserializacja zwraca null
                    username = password = "Deserializacja zwróciła null";

                }
            }
            catch (JsonException ex)
            {
                // Obsługa błędu deserializacji
                Console.WriteLine($"Błąd deserializacji: {ex.Message}");
                username = password = "Błąd deserializacji";
            }

        }


        public static async Task<string> DeleteFirstAndLastCharFromString(string json)
        {
            string modifiedString = json.Substring(1, json.Length - 2);
            Console.WriteLine(modifiedString);
            return modifiedString;
        }
        public static string RemoveCharacterPairs(string input, string pairToRemove)
        {
            return input.Replace(pairToRemove, string.Empty);
        }
    }

}