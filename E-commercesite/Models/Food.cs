using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace E_commercesite.Models
{
    public class Food
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [StringLength(50)]
        public String Name { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public IFormFile Image { get; set; }
        public String ImageString { get; set; }

    }
}
