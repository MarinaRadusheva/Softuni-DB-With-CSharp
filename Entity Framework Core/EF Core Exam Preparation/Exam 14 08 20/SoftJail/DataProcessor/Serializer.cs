namespace SoftJail.DataProcessor
{

    using Data;
    using Newtonsoft.Json;
    using SoftJail.DataProcessor.ExportDto;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportPrisonersByCells(SoftJailDbContext context, int[] ids)
        {
            var prisoners = context.Prisoners.ToList()
                .Where(x => ids.Contains(x.Id))
                .Select(x => new PrisonerJsonDto
                {
                    Id = x.Id,
                    Name = x.FullName,
                    CellNumber = x.Cell.CellNumber,
                    Officers = x.PrisonerOfficers
                    .Select(o => new OfficerJsonDto
                    {
                        OfficerName = o.Officer.FullName,
                        Department = o.Officer.Department.Name,
                    })
                    .OrderBy(o=>o.OfficerName)
                    .ToList(),
                    TotalOfficerSalary=x.PrisonerOfficers.Sum(p=>p.Officer.Salary)
                })
                .OrderBy(x=>x.Name)
                .ThenBy(x=>x.Id)
                .ToList();
            return JsonConvert.SerializeObject(prisoners, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

        }

        public static string ExportPrisonersInbox(SoftJailDbContext context, string prisonersNames)
        {
            var prisoners = context.Prisoners.ToList()
                .Where(x => prisonersNames.Contains(x.FullName))
                .Select(x => new PrisonerXmlDto
                {
                    Id = x.Id,
                    Name = x.FullName,
                    IncarcerationDate = x.IncarcerationDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    EncryptedMessages = x.Mails.Select(m => new MailXmlDto
                    {
                        Description = ReverseMessage(m.Description)
                    }).ToArray()
                })
                .OrderBy(x=>x.Name)
                .ThenBy(x=>x.Id)
                .ToArray();
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");
            var serializer = new XmlSerializer(typeof(PrisonerXmlDto[]), new XmlRootAttribute("Prisoners"));
            serializer.Serialize(writer, prisoners, namespaces);
            return sb.ToString().TrimEnd();
        }
        static string ReverseMessage(string message)
        {
            char[] charArray = message.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }
}