using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VaporStore.DataProcessor.Dto.Import
{
    public class UserImportDto
	{ 
		[Required]
		[RegularExpression(@"^[A-Z][a-z]+[ ][A-Z][a-z]+$")]
		public string FullName { get; set; }

		[Required]
		[StringLength(20,MinimumLength =3)]
        public string Username { get; set; }

		[Required]
        [EmailAddress]
        public string Email { get; set; }

		[Range(3,103)]
        public int Age { get; set; }

        public IEnumerable<CardImportDto> Cards { get; set; }
		
    }
}
