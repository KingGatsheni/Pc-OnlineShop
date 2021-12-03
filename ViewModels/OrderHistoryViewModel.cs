using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShop.ViewModels
{
    public class OrderHistoryViewModel
    {
        public int SaleId { get; set; }
        public DateTime PurchasedOn { get; set; }
        public decimal Total { get; set; }
        public ICollection<SalesItem> SalesItems { get; set; }
    }
}