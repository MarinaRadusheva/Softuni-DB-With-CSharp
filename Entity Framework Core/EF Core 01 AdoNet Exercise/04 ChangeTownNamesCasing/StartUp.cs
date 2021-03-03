using Microsoft.Data.SqlClient;
using System;

namespace _04_ChangeTownNamesCasing
{
    class StartUp
    {
        private const string connectionString = @"Server = DESKTOP-JRG378H\SQLEXPRESS; Database = Minions; Integrated security = true;";
        static void Main(string[] args)
        {
            string countryName = Console.ReadLine();
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string updateTownsQuery = @"UPDATE Towns
                                    SET [Name] = UPPER([Name]) WHERE CountryCode = (SELECT Id FROM Countries C WHERE C.[Name] = @countryName)";
            SqlCommand updateTowns = new SqlCommand(updateTownsQuery, connection);
            updateTowns.Parameters.AddWithValue("@countryName", countryName);
            int townsUpdated = updateTowns.ExecuteNonQuery();
            if (townsUpdated == 0)
            {
                Console.WriteLine("No town names were affected.");
            }
            else
            {
                string updatedTownsQuery = @"SELECT STRING_AGG(t.[Name], ', ') FROM Towns t JOIN Countries c on t.CountryCode = c.Id WHERE c.[Name] =  @country";
                SqlCommand getUpdatedTowns = new SqlCommand(updatedTownsQuery, connection);
                getUpdatedTowns.Parameters.AddWithValue("@country", countryName);
                using SqlDataReader reader = getUpdatedTowns.ExecuteReader();
                reader.Read();
                string result = reader[0].ToString();
                Console.WriteLine($"{townsUpdated} town names were affected.");
                Console.WriteLine($"[{result}]");
            }
            
        }
    }
}
