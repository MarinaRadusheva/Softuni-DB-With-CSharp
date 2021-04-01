namespace TeisterMask.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    using Data;
    using System.Text;
    using System.Xml.Serialization;
    using TeisterMask.DataProcessor.ImportDto;
    using System.IO;
    using TeisterMask.Data.Models;
    using System.Globalization;
    using TeisterMask.Data.Models.Enums;
    using System.Linq;
    using Newtonsoft.Json;

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
            StringReader reader = new StringReader(xmlString);
            var projectDtos = (ProjectXmlDto[])serializer.Deserialize(reader);
            foreach (var proj in projectDtos)
            {
                if (!IsValid(proj))
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }
                DateTime? dueDate = null;
                var parsedDueDate = DateTime.TryParseExact(proj.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date);
                if (parsedDueDate)
                {
                    dueDate = date;
                }
                var project = new Project
                {
                    Name = proj.Name,
                    OpenDate = DateTime.ParseExact(proj.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None),
                    DueDate = dueDate,
                };
                foreach (var projtask in proj.Tasks)
                {
                    if (!IsValid(projtask))
                    {
                        output.AppendLine(ErrorMessage);
                        continue;
                    }
                    DateTime taskOpenDate = DateTime.ParseExact(projtask.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);
                    DateTime taskDueDate = DateTime.ParseExact(projtask.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);
                    if (taskOpenDate<project.OpenDate || (project.DueDate.HasValue && taskDueDate>project.DueDate.Value))
                    {
                        output.AppendLine(ErrorMessage);
                        continue;
                    }
                    var task = new Task
                    {
                        Name = projtask.Name,
                        OpenDate = taskOpenDate,
                        DueDate = taskDueDate,
                        ExecutionType = (ExecutionType)projtask.ExecutionType,
                        LabelType = (LabelType)projtask.LabelType,
                    };
                    project.Tasks.Add(task);
                }
                context.Projects.Add(project);
                context.SaveChanges();
                output.AppendLine(string.Format(SuccessfullyImportedProject, project.Name, project.Tasks.Count()));
            }
            return output.ToString().TrimEnd();
        }

        public static string ImportEmployees(TeisterMaskContext context, string jsonString)
        {
            StringBuilder output = new StringBuilder();
            var employeeDtos = JsonConvert.DeserializeObject<IEnumerable<EmployeeJsonDto>>(jsonString);
            foreach (var empl in employeeDtos)
            {
                if (!IsValid(empl))
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }
                var employee = new Employee
                {
                    Username = empl.Username,
                    Email = empl.Email,
                    Phone = empl.Phone,
                };
                context.Employees.Add(employee);
                context.SaveChanges();
                foreach (var task in empl.Tasks.Distinct())
                {
                    if (context.Tasks.Any(x=>x.Id==task))
                    {
                        employee.EmployeesTasks.Add(new EmployeeTask { TaskId = task });
                    }
                    else
                    {
                        output.AppendLine(ErrorMessage);
                    }
                }
                context.SaveChanges();
                output.AppendLine(string.Format(SuccessfullyImportedEmployee, employee.Username, employee.EmployeesTasks.Count()));
            }
            return output.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}