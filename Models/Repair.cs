using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnlineShop.Models
{
    public class Repair
    {
        public Repair()
        {
            Payments = new HashSet<Payment>();
            RepairLists = new HashSet<RepairList>();
        }
        [Key]
        public int RepairId { get; set; }

        public int CustomerId { get; set; }

        public int EmployeeId { get; set; }

        public bool RepairStatus { get; set; }

        public decimal RepairTotal { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime Bookon { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }

        public virtual ICollection<RepairList> RepairLists { get; set; }
    }
}