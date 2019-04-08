using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace E_commercesite.Models
{
    public class User
    {

        [Required]
        public String username { get; set; }
        [Required]
        [StringLength(15, MinimumLength =6)]
        public String password { get; set; }
        [Required]
        [Compare("password")]
        public String confirm { get; set; }

    }
}
