using Microsoft.Data.SqlClient;
using System;

namespace _05_RemoveVillain
{
    class StartUp
    {
        private const string connectionString = @"Server = DESKTOP-JRG378H\SQLEXPRESS; Database = Minions; Integrated Security = true;";
        static void Main(string[] args)
        {
            int villainId = int.Parse(Console.ReadLine());
            using SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            string villainName = GetVillainName(villainId, sqlConnection);
            if(villainName==null)
            {
                Console.WriteLine("No such villain was found.");
            }
            else
            {
                int minionsReleased = ReleaseMinions(villainId, sqlConnection);
                DeleteVillain(villainId, sqlConnection);
                Console.WriteLine($"{villainName} was deleted.");
                Console.WriteLine($"{minionsReleased} minions were released");
            }
        }

        private static void DeleteVillain(int villainId, SqlConnection sqlConnection)
        {
            string deleteVillainQuery = @"DELETE FROM Villains WHERE Id = @villainId";
            SqlCommand deleteVillain = new SqlCommand(deleteVillainQuery, sqlConnection);
            deleteVillain.Parameters.AddWithValue(@"villainId", villainId);
            deleteVillain.ExecuteNonQuery();
        }

        private static int ReleaseMinions(int villainId, SqlConnection sqlConnection)
        {
            string deleteFromMappingTQuery = @"DELETE FROM MinionsVillains WHERE VillainId = @villainId";
            SqlCommand deleteFromMappingT = new SqlCommand(deleteFromMappingTQuery, sqlConnection);
            deleteFromMappingT.Parameters.AddWithValue("@villainId", villainId);
            return deleteFromMappingT.ExecuteNonQuery();
        }

        private static string GetVillainName(int villainId, SqlConnection sqlConnection)
        {
            string getVillainNameQuery = @"SELECT [Name] FROM Villains WHERE Id = @villainId";
            SqlCommand getVillainName = new SqlCommand(getVillainNameQuery, sqlConnection);
            getVillainName.Parameters.AddWithValue("@villainId", villainId);
            string name = getVillainName.ExecuteScalar()?.ToString();
            return name;
        }
    }
}
