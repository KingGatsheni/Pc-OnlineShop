using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineShop.Models
{
    public class Customer
    {
        public Customer()
        {
            Repairs = new HashSet<Repair>();
            Sales = new HashSet<Sale>();
        }

        [Key]
        public int CustomerId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string CellNo { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string ZipCode { get; set; }

        [StringLength(450)]
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

      
        public virtual ICollection<Repair> Repairs { get; set; }

        
        public virtual ICollection<Sale> Sales { get; set; }
    }
}