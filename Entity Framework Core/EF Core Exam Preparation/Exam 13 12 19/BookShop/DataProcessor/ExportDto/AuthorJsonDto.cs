using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.DataProcessor.ExportDto
{
    public class AuthorJsonDto
    {

        public string AuthorName { get; set; }
        public IEnumerable<BookJsonDto> Books { get; set; }

    }
}
