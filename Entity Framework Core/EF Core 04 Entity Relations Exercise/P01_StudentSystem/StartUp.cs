using P01_StudentSystem.Data;
using System;

namespace P01_StudentSystem
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            StudentSystemContext dbContext = new StudentSystemContext();
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
        }
    }
}
