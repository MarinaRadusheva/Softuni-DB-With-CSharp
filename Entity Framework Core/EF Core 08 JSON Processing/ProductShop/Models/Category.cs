using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ProductShop.Models
{
    public class Category
    {
        public Category()
        {
            this.CategoryProducts = new HashSet<CategoryProduct>();
        }
        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(15)]
        public string Name { get; set; }
        public virtual ICollection<CategoryProduct> CategoryProducts { get; set; }
    }
}
