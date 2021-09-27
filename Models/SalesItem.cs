using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineShop.Models
{
    public class SalesItem
    {
        [Key]
        public int SaleItemId { get; set; }

        public int SaleId { get; set; }

        public int InventoryId { get; set; }

        public int Quantity { get; set; }

        public decimal ItemPrice { get; set; }

        public virtual Inventory Inventory { get; set; }

        public virtual Sale Sale { get; set; }
    }
}