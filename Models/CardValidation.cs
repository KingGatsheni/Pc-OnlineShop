using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineShop.Models
{
    public class CardValidation
    {
        [Required(ErrorMessage = "Card Number is required")]
        [RegularExpression(@"[\d]{13,16}",
                           ErrorMessage = "Card number Must be 14 Digits long")]
        public int CardNumber { get; set; }
        [StringLength(4, ErrorMessage = "Expiry Date Can't Exceed 4 Digit")]
        public string Expiry { get; set; }

        [Required(ErrorMessage = "Cvv is required")]
        [StringLength(3, ErrorMessage = "Cvv Can't Exceed 3 Digit")]
        public string Cvv { get; set; }
    }
}
