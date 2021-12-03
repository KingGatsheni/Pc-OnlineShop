using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShop.ViewModels
{
    public class WishListViewModel
    {
        public int ItemId { get; set; }
        public string UserId { get; set; }
        public string ItemName { get; set; }
        public string ItemImage { get; set; }
        public double ItemPrice { get; set; }
    }
}