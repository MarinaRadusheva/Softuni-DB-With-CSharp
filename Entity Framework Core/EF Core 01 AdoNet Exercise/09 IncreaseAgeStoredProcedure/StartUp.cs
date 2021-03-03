using Microsoft.Data.SqlClient;
using System;

namespace _09_IncreaseAgeStoredProcedure
{
    class StartUp
    {
        private const string connectionString = @"Server = DESKTOP-JRG378H\SQLEXPRESS; Database = Minions; Integrated Security = true;";
        static void Main(string[] args)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            int minionId = int.Parse(Console.ReadLine());
            string executeProcedureQuery = @"EXEC dbo.usp_GetOlder @id";
            SqlCommand increseAge = new SqlCommand(executeProcedureQuery, connection);
            increseAge.Parameters.AddWithValue("@id", minionId);
            increseAge.ExecuteNonQuery();
            string selectMinionQuery = @"SELECT Name, Age FROM Minions WHERE Id = @id";
            SqlCommand getMinionInfo = new SqlCommand(selectMinionQuery, connection);
            getMinionInfo.Parameters.AddWithValue("@id", minionId);
            using SqlDataReader reader = getMinionInfo.ExecuteReader();
            reader.Read();
            string name = reader[0].ToString();
            string age = reader[1].ToString();
            Console.WriteLine($"{name} – {age} years old");
        }
    }
}
