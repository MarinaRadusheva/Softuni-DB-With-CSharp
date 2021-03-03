using P03_FootballBetting.Data;
using System;

namespace P03_FootballBetting
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            FootballBettingContext dbContext = new FootballBettingContext();
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
        }
    }
}
