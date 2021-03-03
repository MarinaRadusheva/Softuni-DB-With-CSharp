using Microsoft.Data.SqlClient;
using System;

namespace _02_VillainNames
{
    class StartUp
    {
        private const string connectionString = @"Server = DESKTOP-JRG378H\SQLEXPRESS; Database = Minions; Integrated Security = true;";
        static void Main(string[] args)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string getVillainsQuery = @"SELECT V.Name, COUNT(MV.MinionId) AS MINIONCOUNT FROM Villains V
                                        JOIN MinionsVillains MV ON V.Id = MV.VillainId
                                        GROUP BY V.Id, V.Name
                                        HAVING COUNT(MV.MinionId) >=3
                                        ORDER BY MINIONCOUNT DESC";
            SqlCommand getVillains = new SqlCommand(getVillainsQuery, connection);
            using SqlDataReader reader = getVillains.ExecuteReader();
            while (reader.Read())
            {
                string villainName = reader["Name"]?.ToString();
                string minionsCount = reader["MINIONCOUNT"]?.ToString();
                Console.WriteLine($"{villainName} - {minionsCount}");
            }
        }
    }
}
