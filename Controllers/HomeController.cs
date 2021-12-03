using OnlineShop.Models;
using OnlineShop.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Controllers
{
    
    public class HomeController : Controller
    {
        ApplicationDbContext db;
        public HomeController()
        {
            this.db = new ApplicationDbContext();
        }
        public ActionResult Index()
        {
            IEnumerable<ProductViewModel> product = from s in db.Inventories
                          select new ProductViewModel()
                          {
                              ProductId = s.InventoryId,
                              ProductName = s.ProductName,
                              ProductPrice = s.SellingPrice,
                              CategoryName = s.Category,
                              Image = s.ImageName
                          };
            return View(product.OrderByDescending(x => x.ProductId).Take(4).ToList());
        }
      
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

      
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }

        public ActionResult Help()
        {
            return View();
        }
    }
}