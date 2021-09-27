using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineShop.Models
{
    public class Inventory
    {
        public Inventory()
        {
            SalesItems = new HashSet<SalesItem>();
        }
        [Key]
        public int InventoryId { get; set; }

        public string ProductName { get; set; }

        public string Category { get; set; }

        public decimal CostPrice { get; set; }

        public decimal Markup { get; set; }

        public decimal SellingPrice { get; set; }

        public int Quantity { get; set; }

        public string ImageName { get; set; }

        public virtual ICollection<SalesItem> SalesItems { get; set; }
    }
}