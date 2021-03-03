using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace _06_PrintMinions
{
    class StartUp
    {
        private const string connectionString = @"Server = DESKTOP-JRG378H\SQLEXPRESS; Database = Minions; Integrated Security = true;";
        static void Main(string[] args)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string allMinionsQuery = @"SELECT [Name] FROM Minions";
            SqlCommand getAllMinions = new SqlCommand(allMinionsQuery, connection);
            using SqlDataReader reader = getAllMinions.ExecuteReader();
            List<string> minions = new List<string>();
            while (reader.Read())
            {
                minions.Add(reader[0].ToString());
            }
            StringBuilder output = new StringBuilder();
            
            
            for (int i = 0; i < minions.Count / 2; i++)
            {
                Console.WriteLine(minions[i]);
                Console.WriteLine(minions[minions.Count - 1 - i]);

            }

            if (minions.Count % 2 != 0)
            {
                
                Console.WriteLine(minions[minions.Count/2]);
            }
        }
    }
}
