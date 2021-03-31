using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace SoftJail.DataProcessor.ExportDto
{
    [XmlType("Prisoner")]
    public class PrisonerXmlDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string IncarcerationDate { get; set; }
        [XmlArray]
        public MailXmlDto[] EncryptedMessages { get; set; }
    }
}
