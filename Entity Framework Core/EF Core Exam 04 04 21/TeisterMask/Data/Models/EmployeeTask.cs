﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TeisterMask.Data.Models
{
    public class EmployeeTask
    {
        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }
        public int TaskId { get; set; }
        public virtual Task Task { get; set; }
    }
}
