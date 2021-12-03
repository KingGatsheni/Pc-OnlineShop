using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShop.ViewModels
{
    public class RepairViewModel
    {
        public int RepairId { get; set; }
        public int Quantity { get; set; }
        public string ItemName { get; set; }
        public decimal RepairPrice { get; set; }
        public bool RepairStatus { get; set; }
        public DateTime BookedOn { get; set; }
    }
}