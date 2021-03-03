using MiniORM.App.Data;
using MiniORM.App.Data.Entities;
using System;
using System.Linq;

namespace MiniORM.App
{
    class StartUp
    {
        static void Main(string[] args)
        {
            string connectionString = @"Server = .; Database = MiniORM; Integrated security = true;";
            SoftUniDbContextClass context = new SoftUniDbContextClass(connectionString);
            context.Employees.Add(new Employee
            {
                FirstName = "Gary",
                LastName = "Preston",
                DepartmentId = context.Departments.First().Id,
                IsEmployed = true,
            });
            Employee employee = context.Employees.Last();
            employee.FirstName = "Modified";

            context.SaveChanges();

        }
    }
}
