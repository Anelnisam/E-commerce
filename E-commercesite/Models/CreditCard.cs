using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace E_commercesite.Models
{
    public class CreditCard
{
        [Required]
        public String number { get; set; }
        [Required]
        public int expMonth { get; set; }
        [Required]
        public int expYear { get; set; }
        [Required]
        public String CCV { get; set; }


}
}
