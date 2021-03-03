using Microsoft.Data.SqlClient;
using System;
using System.Text;

namespace _03_MinionsNames
{
    class StartUp
    {
        private const string ConnectionString = @"Server = DESKTOP-JRG378H\SQLEXPRESS; Database = Minions; Integrated Security = true;";
        static void Main(string[] args)
        {
            StringBuilder sb = new StringBuilder();
            using SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            sqlConnection.Open();
            int villainId = int.Parse(Console.ReadLine());
            string getVillainNameQuery = @"SELECT [NAME] FROM Villains WHERE Id = @villainId";
            SqlCommand getVillainName = new SqlCommand(getVillainNameQuery, sqlConnection);
            getVillainName.Parameters.AddWithValue("@villainId", villainId);
            var result = getVillainName.ExecuteScalar()?.ToString();
            if (result==null)
            {
                sb.AppendLine($"No villain with ID { villainId} exists in the database.");
            }
            else
            {
                sb.AppendLine($"Villain: {result}");
                string getMinionsInfoQuery = @"SELECT M.Name, M.Age FROM Villains V
                                            LEFT JOIN MinionsVillains MV ON V.Id = MV.VillainId
                                            LEFT JOIN Minions M ON MV.MinionId = M.Id
                                            WHERE V.Id = @villainId
                                            ORDER BY M.Name";
                SqlCommand getMinionsInfo = new SqlCommand(getMinionsInfoQuery, sqlConnection);
                getMinionsInfo.Parameters.AddWithValue("@villainId", villainId);
                using SqlDataReader reader = getMinionsInfo.ExecuteReader();          
                int rowNum = 0;
                while (reader.Read())
                    {
                        string minionName = reader["Name"]?.ToString();
                        string minionAge = reader["Age"]?.ToString();
                    if (minionName == "")
                        break;
                        sb.AppendLine($"{rowNum+1}. {minionName} {minionAge}");
                        rowNum++;
                    }
                if(rowNum==0)
                {
                    sb.AppendLine("(no minions)");
                }
            }
            Console.WriteLine(sb.ToString());
        }
    }
}
