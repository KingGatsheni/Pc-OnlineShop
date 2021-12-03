using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnlineShop.Models
{
    public class WishItem
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ItemId { get; set; }
        public string UserId { get; set; }
        public string ItemName { get; set; }
        public string ItemImage { get; set; }
        public decimal ItemPrice { get; set; }
    }
}