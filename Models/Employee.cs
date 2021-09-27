using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineShop.Models
{
    public class Employee
    {
        public Employee()
        {
            Accounts = new HashSet<Account>();
            Payments = new HashSet<Payment>();
            Repairs = new HashSet<Repair>();
            Sales = new HashSet<Sale>();
        }

        [Key]
        public int EmployeeId { get; set; }

        public long StuffId { get; set; }

        public string Firstname { get; set; }

        public string Id { get; set; }

        public string CellNo { get; set; }

        public string Email { get; set; }

        public string EmployeeRole { get; set; }

        public string Address { get; set; }

        public string LastName { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }


        public virtual ICollection<Payment> Payments { get; set; }

      
        public virtual ICollection<Repair> Repairs { get; set; }

       
        public virtual ICollection<Sale> Sales { get; set; }
    }
}