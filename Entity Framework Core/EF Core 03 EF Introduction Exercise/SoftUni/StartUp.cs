using System;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SoftUni.Data;
using SoftUni.Models;

namespace SoftUni
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            SoftUniContext context = new SoftUniContext();
            Console.WriteLine(GetDepartmentsWithMoreThan5Employees(context));
        }
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            var allEmployees = context.Employees.OrderBy(x => x.EmployeeId);
            foreach (var employee in allEmployees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} {employee.MiddleName} {employee.JobTitle} {employee.Salary:f2}");
            }
            return sb.ToString().TrimEnd();
        }
        
        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            var employees = context.Employees.OrderBy(x => x.FirstName).
                Select(x => new { x.FirstName, x.Salary }).
                Where(x => x.Salary > 50000).ToArray();
            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} - {employee.Salary:f2}");
            }
            return sb.ToString().TrimEnd();
        }
        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            var employees = context.Employees.OrderBy(x => x.Salary).
                ThenByDescending(x => x.Salary).
                Where(x=>x.Department.Name == "Research and Development").
                Select(x => new { x.FirstName, x.LastName, x.Department.Name, x.Salary }).
                ToArray();
            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} from {employee.Name} - ${employee.Salary:f2}");

            }
            return sb.ToString().TrimEnd();
        }
        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            Employee employee = context.Employees.FirstOrDefault(x => x.LastName == "Nakov");
            employee.Address = new Address { AddressText = "Vitoshka 15", TownId = 4, };
            context.SaveChanges();
            var allAddresses = context.Addresses.OrderByDescending(x => x.AddressId).Select(x => x.AddressText).Take(10).ToArray();
            StringBuilder sb = new StringBuilder();
            foreach (var item in allAddresses)
            {
                sb.AppendLine(item);
            }
            return sb.ToString().TrimEnd();
        }
        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            var employees = context.Employees.
                Include(x => x.EmployeesProjects).
                ThenInclude(x => x.Project).
                Where(x => x.EmployeesProjects.Any(x => x.Project.StartDate.Year >= 2001 && x.Project.StartDate.Year <= 2003)).
                Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    manFirstName = e.Manager.FirstName,
                    manLastName = e.Manager.LastName,
                    Projects = e.EmployeesProjects.Select(p => new
                    {
                        ProjectName = p.Project.Name,
                        StartDate = p.Project.StartDate,
                        EndDate = p.Project.EndDate,
                    })
                }).
                Take(10).
                ToArray();
            StringBuilder sb = new StringBuilder();
            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} - Manager: {employee.manFirstName} {employee.manLastName}");
                foreach (var item in employee.Projects)
                {

                    string endDateString = item.EndDate.HasValue ? item.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture) : "not finished";
                    sb.AppendLine($"--{item.ProjectName} - {item.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)} - {endDateString}");
                }
            }
            return sb.ToString().TrimEnd();
        }
        public static string GetAddressesByTown(SoftUniContext context)
        {
            var addresses = context.Addresses.
                Include(x => x.Employees).
                Include(x => x.Town).
                Select(a => new
                {
                    AddressText = a.AddressText,
                    TownName = a.Town.Name,
                    EmpCount = a.Employees.Count(),
                }).
                OrderByDescending(e => e.EmpCount).
                ThenBy(t => t.TownName).
                ThenBy(a => a.AddressText).
                Take(10).
                ToArray();
            StringBuilder sb = new StringBuilder();
            foreach (var add in addresses)
            {
                sb.AppendLine($"{add.AddressText}, {add.TownName} - {add.EmpCount} employees");
            }
            return sb.ToString().TrimEnd();
        }
        public static string GetEmployee147(SoftUniContext context)
        {
            var employee = context.Employees.Include(x => x.EmployeesProjects).ThenInclude(p => p.Project).Select(x => new
            {
                EmployeeId = x.EmployeeId,
                EmployeeName = x.FirstName,
                EmployeeLastName = x.LastName,
                EmployeeJob = x.JobTitle,
                Projects = x.EmployeesProjects.Select(p=>p.Project.Name),
            }).FirstOrDefault(x=>x.EmployeeId==147);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{employee.EmployeeName} {employee.EmployeeLastName} - {employee.EmployeeJob}");
            foreach (var item in employee.Projects.OrderBy(x=>x))
            {
                sb.AppendLine(item);
            }
            return sb.ToString().TrimEnd();
        }
        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            var departments = context.Departments.
                Include(x => x.Employees).
                Include(x=>x.Manager).
                Where(x=>x.Employees.Count>5).
                OrderBy(x => x.Employees.Count).
                ThenBy(x => x.Name).
                Select(x=>new
                {
                    DepartmentName = x.Name,
                    ManagerFirstName = x.Manager.FirstName,
                    ManagerLastName = x.Manager.LastName,
                    Employees = x.Employees.Select(e=> new
                    {
                        EmployeeFirstname = e.FirstName,
                        EmployeeLastName = e.LastName,
                        EmployeeJob = e.JobTitle,
                    }).OrderBy(n=>n.EmployeeFirstname).ThenBy(n=>n.EmployeeLastName).ToArray(),
                }).
                ToArray();
            StringBuilder sb = new StringBuilder();
            foreach (var dept in departments)
            {
                sb.AppendLine($"{dept.DepartmentName} - {dept.ManagerFirstName} {dept.ManagerLastName}");
                foreach (var employee in dept.Employees)
                {
                    sb.AppendLine($"{employee.EmployeeFirstname} {employee.EmployeeLastName} - {employee.EmployeeJob}");
                }
            }
            return sb.ToString().TrimEnd();
        }
        public static string GetLatestProjects(SoftUniContext context)
        {
            var latestProjects = context.Projects.OrderByDescending(x => x.StartDate).Select(x=> new 
            {
                Name=x.Name,
                Description = x.Description,
                Date = x.StartDate,
            }).Take(10).OrderBy(x=>x.Name).ToArray();
            StringBuilder sb = new StringBuilder();
            foreach (var project in latestProjects)
            {
                sb.AppendLine($"{project.Name}");
                sb.AppendLine($"{project.Description }");
                sb.AppendLine($"{project.Date.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture) }");
            }
            return sb.ToString().TrimEnd();
        }
        public static string IncreaseSalaries(SoftUniContext context)
        {
            string[] departments = new string[] { "Engineering", "Tool Design", "Marketing", "Information Services" };
            var employees = context.Employees.
                Where(x => departments.Contains(x.Department.Name)).
                OrderBy(x=>x.FirstName).
                ThenBy(x=>x.LastName).
                ToArray();
            foreach (var employee in employees)
            {
                employee.Salary *= 1.12m;
            }
            context.SaveChanges();
            StringBuilder sb = new StringBuilder();
            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} (${employee.Salary:F2})");
            }
            return sb.ToString().TrimEnd();
        }
        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            string[] startsWith = new string[] { "SA", "Sa", "sA", "sa" };
            var employees = context.Employees.Where(x => startsWith.Contains(x.FirstName.Substring(0, 2))).OrderBy(x=>x.FirstName).ThenBy(x=>x.LastName).ToArray();
            StringBuilder sb = new StringBuilder();
            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle} - (${employee.Salary:f2})");
            }
            return sb.ToString().TrimEnd();
        }
        public static string DeleteProjectById(SoftUniContext context)
        {
            var projectToDelete = context.Projects.Find(2);
            var projectEmployeeToRemove = context.EmployeesProjects.Where(x => x.ProjectId == projectToDelete.ProjectId).ToArray();
            context.EmployeesProjects.RemoveRange(projectEmployeeToRemove);
            context.SaveChanges();
            context.Projects.Remove(projectToDelete);
            context.SaveChanges();
            var tenProjects = context.Projects.Take(10).ToArray();
            StringBuilder sb = new StringBuilder();
            foreach (var project in tenProjects)
            {
                sb.AppendLine(project.Name);
            }
            return sb.ToString().TrimEnd();
        }
        public static string RemoveTown(SoftUniContext context)
        {
            string townName = "Seattle";
            Town townToDelete = context.Towns.FirstOrDefault(x => x.Name == townName);
            Address[] addressesToDelete = context.Addresses.Where(x => x.TownId == townToDelete.TownId).ToArray();
            int townsCount = addressesToDelete.Count();
            foreach (var employee in context.Employees.Where(x=>addressesToDelete.Contains(x.Address)))
            {
                employee.AddressId = null;
            }
            context.SaveChanges();
            context.Addresses.RemoveRange(addressesToDelete);
            context.SaveChanges();
            context.Towns.Remove(townToDelete);
            context.SaveChanges();
            return $"{townsCount} addresses in {townName} were deleted";
        }
    }
}
