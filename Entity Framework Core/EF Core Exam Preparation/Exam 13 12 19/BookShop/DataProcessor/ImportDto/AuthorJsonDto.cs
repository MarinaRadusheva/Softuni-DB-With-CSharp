﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BookShop.DataProcessor.ImportDto
{
    public class AuthorJsonDto
    {
        [Required]
        [MaxLength(30)]
        [MinLength(3)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(30)]
        [MinLength(3)]
        public string LastName { get; set; }
        [Required]
        [RegularExpression(@"^[0-9]{3}-[0-9]{3}-[0-9]{4}$")]
        public string Phone { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public IEnumerable<BookIdDto> Books { get; set; }



    }
}
