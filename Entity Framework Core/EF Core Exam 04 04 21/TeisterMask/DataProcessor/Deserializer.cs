namespace TeisterMask.DataProcessor
{
    using System;
    using System.Collections.Generic;

    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using TeisterMask.Data.Models;
    using TeisterMask.Data.Models.Enums;
    using TeisterMask.DataProcessor.ImportDto;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedProject
            = "Successfully imported project - {0} with {1} tasks.";

        private const string SuccessfullyImportedEmployee
            = "Successfully imported employee - {0} with {1} tasks.";

        public static string ImportProjects(TeisterMaskContext context, string xmlString)
        {
            StringBuilder output = new StringBuilder();
            XmlSerializer serializer = new XmlSerializer(typeof(ProjectXmlDto[]), new XmlRootAttribute("Projects"));
            List<Project> allprojects = new List<Project>();
            using (var reader = new StringReader(xmlString))
            {
                var projects = (ProjectXmlDto[])serializer.Deserialize(reader);
                foreach (var project in projects)
                {

                    if (!IsValid(project))
                    {
                        output.AppendLine(ErrorMessage);
                        continue;
                    }
                    DateTime openDate;
                    var parsedOpenDate = DateTime.TryParseExact(project.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out openDate);
                    if (!parsedOpenDate)
                    {
                        output.AppendLine(ErrorMessage);
                        continue;
                    }

                    DateTime? dueDate;
                    if (!string.IsNullOrEmpty(project.DueDate))
                    {
                        DateTime dueDateActual;
                        var parsedDueDate = DateTime.TryParseExact(project.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dueDateActual);
                        if (!parsedDueDate)
                        {
                            output.AppendLine(ErrorMessage);
                            continue;
                        }
                        dueDate = dueDateActual;
                    }
                    else
                    {
                        dueDate = null;
                    }

                    var newProject = new Project
                    {
                        Name = project.Name,
                        OpenDate = openDate,
                        DueDate = dueDate,
                    };
                    foreach (var task in project.Tasks)
                    {
                        if (!IsValid(task))
                        {
                            output.AppendLine(ErrorMessage);
                            continue;
                        }
                        DateTime taskOpenDate;
                        var taskOpenDateParsed = DateTime.TryParseExact(task.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out  taskOpenDate);
                        
                        if (!taskOpenDateParsed)
                        {
                            output.AppendLine(ErrorMessage);
                            continue;
                        }
                        if (taskOpenDate < newProject.OpenDate)
                        {
                            output.AppendLine(ErrorMessage);
                            continue;
                        }
                        DateTime taskDueDate;
                        var taskDueDateParsed = DateTime.TryParseExact(task.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out taskDueDate);
                        if (!taskDueDateParsed)
                        {
                            output.AppendLine(ErrorMessage);
                            continue;
                        }
                        if (newProject.DueDate.HasValue && taskDueDate > newProject.DueDate)
                        {
                            output.AppendLine(ErrorMessage);
                            continue;
                        }
                        var newTask = new Task
                        {
                            Name = task.Name,
                            OpenDate = taskOpenDate,
                            DueDate = taskDueDate,
                            ExecutionType = (ExecutionType)task.ExecutionType,
                            LabelType = (LabelType)task.LabelType,
                        };
                        newProject.Tasks.Add(newTask);
                    }
                    allprojects.Add(newProject);
                    output.AppendLine(string.Format(SuccessfullyImportedProject, newProject.Name, newProject.Tasks.Count()));
                }
                context.Projects.AddRange(allprojects);
                context.SaveChanges();
            }
            return output.ToString().TrimEnd();
        }

        public static string ImportEmployees(TeisterMaskContext context, string jsonString)
        {
            StringBuilder output = new StringBuilder();
            var employees = JsonConvert.DeserializeObject<List<EmployeeJsonDto>>(jsonString);
            List<Employee> allemployees = new List<Employee>();
            foreach (var employee in employees)
            {
                if (!IsValid(employees))
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }
                if (!usernameValid(employee.Username))
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }
                var newEmployee = new Employee
                {
                    Username = employee.Username,
                    Email = employee.Email,
                    Phone = employee.Phone,
                };
                
                foreach (var taskId in employee.Tasks.Distinct())
                {
                    Task task = context.Tasks
                         .FirstOrDefault(t => t.Id == taskId);

                    if (task == null)
                    {
                        output.AppendLine(ErrorMessage);
                        continue;
                    }

                    newEmployee.EmployeesTasks.Add(new EmployeeTask()
                    {
                        Employee = newEmployee,
                        Task = task
                    });
                }
                allemployees.Add(newEmployee);
                output.AppendLine(string.Format(SuccessfullyImportedEmployee, newEmployee.Username, newEmployee.EmployeesTasks.Count()));
            }
            context.Employees.AddRange(allemployees);
            context.SaveChanges();
            return output.ToString().TrimEnd();
        }

        private static bool usernameValid(string username)
        {
            bool isvalid = true;
            foreach (var ch in username)
            {
                if (!char.IsLetterOrDigit(ch))
                {
                    isvalid = false;
                    break;
                }
                
            }
            return isvalid;
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}