using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineShop.Models
{
    public class Account
    {
        [Key]
        public int AccountId { get; set; }

        public int EmployeeId { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public virtual Employee Employee { get; set; }
    }
}