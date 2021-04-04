using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace TeisterMask.DataProcessor.ExportDto
{
    [XmlType("Project")]
    public class ProjectXmlDto
    {
        [XmlAttribute]
        public int TasksCount { get; set; }
        public string ProjectName { get; set; }
        public string HasEndDate { get; set; }
        [XmlArray]
        public TaskXmlDto[] Tasks { get; set; }

    }
}
