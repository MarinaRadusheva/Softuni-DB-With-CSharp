using System;
using System.Collections.Generic;
using System.Text;

namespace TeisterMask.DataProcessor.ExportDto
{
    public class EmployeeJsonDto
    {
        public string Username { get; set; }
        public ICollection<TaskJsonDto> Tasks { get; set; }
    }
}
