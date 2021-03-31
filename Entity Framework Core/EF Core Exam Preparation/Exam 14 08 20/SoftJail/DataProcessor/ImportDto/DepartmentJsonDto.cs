using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SoftJail.DataProcessor.ImportDto
{
    class DepartmentJsonDto
    {
            [Required]
            [MinLength(3)]
            [MaxLength(25)]
            public string Name { get; set; } //min length 3 and max length 25 (lic ret quinired)
            public IEnumerable<CellJsonDto> Cells { get; set; }

    }

}
