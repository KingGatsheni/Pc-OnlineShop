using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnlineShop.Models
{
    public class Sale
    {
        public Sale()
        {
            Payments = new HashSet<Payment>();
            SalesItems = new HashSet<SalesItem>();
        }

        [Key]
        public int SaleId { get; set; }

        public decimal Total { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime Date { get; set; }

        public int? CustomerId { get; set; }

        public int? EmployeeId { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual Employee Employee { get; set; }

      
        public virtual ICollection<Payment> Payments { get; set; }

        public virtual ICollection<SalesItem> SalesItems { get; set; }
    }
}