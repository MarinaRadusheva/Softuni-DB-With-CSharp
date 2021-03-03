using Microsoft.Data.SqlClient;
using System;
using System.Linq;

namespace _07_IncreaseMinionAge
{
    class StartUp
    {
        private const string connectionString = @"Server = DESKTOP-JRG378H\SQLEXPRESS; Database = Minions; Integrated Security = true;";
        static void Main(string[] args)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            int[] minionIds = Console.ReadLine().Split().Select(int.Parse).ToArray();
            UpdateMinions(minionIds, connection);
            PrintMinions(connection);
        }

        private static void PrintMinions(SqlConnection connection)
        {
            string allMinions = @"SELECT Name,Age FROM Minions";
            SqlCommand getAllMinions = new SqlCommand(allMinions, connection);
            using SqlDataReader reader = getAllMinions.ExecuteReader();
            while (reader.Read())
            {
                string name = reader[0].ToString();
                string age = reader[1]?.ToString();
                Console.WriteLine($"{name} {age}");
            }
        }

        private static void UpdateMinions(int[] minionIds, SqlConnection connection)
        {
            string updateMinionsQuery = @"UPDATE Minions
            SET Name = UPPER(LEFT(Name, 1))+RIGHT(Name, LEN(NAME)-1), Age+=1  WHERE Id = @ID";

            for (int i = 0; i < minionIds.Length; i++)
            {
                SqlCommand updateMinions = new SqlCommand(updateMinionsQuery, connection);
                updateMinions.Parameters.AddWithValue("@ID", minionIds[i]);
                updateMinions.ExecuteNonQuery();
            }
        }
    }
}
