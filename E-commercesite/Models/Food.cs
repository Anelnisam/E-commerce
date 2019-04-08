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
        public String Name { get; set; }
        public float Price { get; set; }


    }
}
