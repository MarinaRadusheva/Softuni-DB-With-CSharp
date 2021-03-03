using Microsoft.Data.SqlClient;
using System;
using System.Text;

namespace _03_AddMinion
{
    class StartUp
    {
        private const string connectionString = @"Server = DESKTOP-JRG378H\SQLEXPRESS; Database = Minions; Integrated Security = true";
        static void Main(string[] args)
        {
            using SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            string[] minionInfo = Console.ReadLine().Split();
            string[] villainInfo = Console.ReadLine().Split();
            string minionName = minionInfo[1];
            int minionAge = int.Parse(minionInfo[2]);
            string minionTown = minionInfo[3];
            string villainName = villainInfo[1];
            StringBuilder output = new StringBuilder();
            string minionTownId = GetMinionTownId(minionTown, sqlConnection, output);
            string villainId = AddVillainIfNotExistant(villainName, sqlConnection, output);
            InsertMinionInDB(minionName, minionAge, minionTownId, sqlConnection);
            string minionId = GetMinionId(minionName, sqlConnection);
            AddMinionToVillain(minionId, villainId, sqlConnection);
            output.AppendLine($"Successfully added {minionName} to be minion of {villainName}.");
            Console.WriteLine(output.ToString());
        }

        private static void AddMinionToVillain(string minionId, string villainId, SqlConnection sqlConnection)
        {
            string addMinionToVillainQuery = @"INSERT INTO MinionsVillains VALUES (@minionId, @villainId)";
            SqlCommand addMinionToVillain = new SqlCommand(addMinionToVillainQuery, sqlConnection);
            addMinionToVillain.Parameters.AddWithValue("@minionId", minionId);
            addMinionToVillain.Parameters.AddWithValue("@villainId", villainId);
            addMinionToVillain.ExecuteNonQuery();
            
        }

        private static string GetMinionId(string minionName, SqlConnection sqlConnection)
        {
            string getMinionIdQuery = @"SELECT Id FROM Minions WHERE [Name] = @minionName";
            SqlCommand getMinionId = new SqlCommand(getMinionIdQuery, sqlConnection);
            getMinionId.Parameters.AddWithValue("@minionName", minionName);
            return getMinionId.ExecuteScalar().ToString();
        }

        private static void InsertMinionInDB(string minionName, int minionAge, string minionTownId, SqlConnection sqlConnection)
        {
            string insertMinionQuery = @"INSERT INTO Minions VALUES (@minionName, @minionAge, @townId)";
            SqlCommand insertMinion = new SqlCommand(insertMinionQuery, sqlConnection);
            insertMinion.Parameters.AddWithValue("@minionName", minionName);
            insertMinion.Parameters.AddWithValue("@minionAge", minionAge);
            insertMinion.Parameters.AddWithValue("@townId", minionTownId);
            insertMinion.ExecuteNonQuery();
        }

        private static string AddVillainIfNotExistant(string villainName, SqlConnection sqlConnection, StringBuilder output)
        {
            string getVillainIdQuery = @"SELECT Id FROM Villains WHERE [Name] = @villainName";
            SqlCommand getVillainId = new SqlCommand(getVillainIdQuery, sqlConnection);
            getVillainId.Parameters.AddWithValue("@villainName", villainName);
            string villainId = getVillainId.ExecuteScalar()?.ToString();
            if (villainId==null)
            {
                string addVillainQuery = @"INSERT INTO Villains VALUES (@villainName, 4)";
                SqlCommand insertVillain = new SqlCommand(addVillainQuery, sqlConnection);
                insertVillain.Parameters.AddWithValue("@villainName", villainName);
                insertVillain.ExecuteNonQuery();
                output.AppendLine($"Villain {villainName} was added to the database.");
                villainId = getVillainId.ExecuteScalar().ToString();
            }
            return villainId;
        }

        private static string GetMinionTownId(string minionTown, SqlConnection sqlConnection, StringBuilder output)
        {
            string getTownIdQuery = @"SELECT Id FROM Towns WHERE [Name] = @townName";
            SqlCommand getTownId = new SqlCommand(getTownIdQuery, sqlConnection);
            getTownId.Parameters.AddWithValue("@townName", minionTown);
            string townId = getTownId.ExecuteScalar()?.ToString();
            if (townId == null)
            {
                string insertTownQuery = @"INSERT INTO Towns VALUES (@townName, 1)";
                SqlCommand insertTown = new SqlCommand(insertTownQuery, sqlConnection);
                insertTown.Parameters.AddWithValue("@townName", minionTown);
                insertTown.ExecuteNonQuery();
                townId = getTownId.ExecuteScalar()?.ToString();
                output.AppendLine($"Town {minionTown} was added to the database.");
            }
            return townId;
        }
    }
}
