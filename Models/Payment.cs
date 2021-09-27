using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineShop.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }

        public int EmployeeId { get; set; }

        public string PaymentMethod { get; set; }

        public int? SaleId { get; set; }

        public decimal TotalAmount { get; set; }

        public int? RepairId { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual Repair Repair { get; set; }

        public virtual Sale Sale { get; set; }
    }
}