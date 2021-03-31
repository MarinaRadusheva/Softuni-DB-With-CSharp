namespace SoftJail.DataProcessor
{

    using Data;
    using Newtonsoft.Json;
    using SoftJail.Data.Models;
    using SoftJail.Data.Models.Enums;
    using SoftJail.DataProcessor.ImportDto;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    public class Deserializer
    {
        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            StringBuilder output = new StringBuilder();
            var departments = JsonConvert.DeserializeObject<IEnumerable<DepartmentJsonDto>>(jsonString);
            foreach (var dept in departments)
            {
                if (!IsValid(dept) || !dept.Cells.Any() || !dept.Cells.All(IsValid))
                {
                    output.AppendLine("Invalid Data");
                    continue;
                }
                var department = new Department
                {
                    Name = dept.Name,
                };
                context.Departments.Add(department);
                context.SaveChanges();
                foreach (var deptCell in dept.Cells)
                {
                    department.Cells.Add(new Cell 
                    { CellNumber = deptCell.CellNumber,
                        HasWindow = deptCell.HasWindow 
                    });
                    
                }
                context.SaveChanges();
                output.AppendLine($"Imported {department.Name} with {department.Cells.Count()} cells");
            }
            return output.ToString().TrimEnd();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            StringBuilder output = new StringBuilder();
            var prisoners = JsonConvert.DeserializeObject<IEnumerable<PrisonerJsonDto>>(jsonString);
            foreach (var prsn in prisoners)
            {
                if (!IsValid(prsn) || !prsn.Mails.All(IsValid))
                {
                    output.AppendLine("Invalid Data");
                    continue;
                }
                DateTime? releaseDate = null;
                var parsedReleasecDate = DateTime.TryParseExact(prsn.ReleaseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var releaseDateNotNull);
                if (parsedReleasecDate)
                {
                    releaseDate = releaseDateNotNull;
                }
                var prisoner = new Prisoner
                {
                    FullName = prsn.FullName,
                    Nickname = prsn.Nickname,
                    Age = prsn.Age,
                    IncarcerationDate = DateTime.ParseExact(prsn.IncarcerationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    ReleaseDate = releaseDate,
                    Bail = prsn.Bail,
                    CellId = prsn.CellId,
                };
                context.Prisoners.Add(prisoner);
                context.SaveChanges();
                foreach (var mail in prsn.Mails)
                {
                    prisoner.Mails.Add(new Mail
                    {
                        Description = mail.Description,
                        Sender = mail.Sender,
                        Address = mail.Address
                    });
                }
                context.SaveChanges();
                output.AppendLine($"Imported {prisoner.FullName} {prisoner.Age} years old");
            }
            return output.ToString().TrimEnd();
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            StringBuilder output = new StringBuilder();
            var xmlserializer = new XmlSerializer(typeof(OfficerXmlDto[]), new XmlRootAttribute("Officers"));
            StringReader reader = new StringReader(xmlString);
            var officers = (OfficerXmlDto[])xmlserializer.Deserialize(reader);
            foreach (var offcr in officers)
            {
                if (!IsValid(offcr))
                {

                    output.AppendLine("Invalid Data");
                    continue;
                }
                if (!Enum.IsDefined(typeof(Position),offcr.Position) || !Enum.IsDefined(typeof(Weapon), offcr.Weapon))
                {
                    output.AppendLine("Invalid Data");
                    continue;
                }
                var officer = new Officer
                {
                    FullName = offcr.Name,
                    Salary = offcr.Money,
                    Position = (Position)Enum.Parse(typeof(Position), offcr.Position),
                    Weapon = (Weapon)Enum.Parse(typeof(Weapon), offcr.Weapon),
                    DepartmentId = offcr.DepartmentId,
                };
                context.Officers.Add(officer);
                context.SaveChanges();
                foreach (var prsn in offcr.Prisoners)
                {
                    officer.OfficerPrisoners.Add(new OfficerPrisoner { PrisonerId = prsn.Id });
                }

                context.SaveChanges();
                output.AppendLine($"Imported {officer.FullName} ({officer.OfficerPrisoners.Count()} prisoners)");
            }
            return output.ToString().TrimEnd();
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult, true);
            return isValid;
        }
    }
}