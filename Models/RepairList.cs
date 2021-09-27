using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineShop.Models
{
    public class RepairList
    {
        [Key]
        public int RepairListId { get; set; }

        public int RepairId { get; set; }

        public int Quantity { get; set; }

        public decimal RepairPrice { get; set; }

        public string ItemName { get; set; }

        public string ItemFault { get; set; }

        public virtual Repair Repair { get; set; }
    }
}