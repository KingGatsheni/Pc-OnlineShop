using OnlineShop.Models;
using OnlineShop.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Controllers
{
    public class StoreController : Controller
    {
        private ApplicationDbContext context; // declare database instance
        private List<CartViewModel> listOfShoppingCartModels;
        public StoreController()
        {
            this.context = new ApplicationDbContext();
            listOfShoppingCartModels = new List<CartViewModel>();
        }
        public ActionResult Index()
        {
            IEnumerable<ProductViewModel> listOfProducts = (from itemObj in context.Inventories
                                                        select new ProductViewModel()
                                                        {
                                                            ProductId = itemObj.InventoryId,
                                                            ProductName = itemObj.ProductName,
                                                            CategoryName = itemObj.Category,
                                                            ProductPrice = itemObj.SellingPrice,
                                                            Image = itemObj.ImageName
                                                        }).ToList();

          
            return View(listOfProducts);
        }

        [HttpPost]
        public JsonResult Index(string ItemId)
        {
            CartViewModel objShoppingCartModel = new CartViewModel();
            Inventory objPro = context.Inventories.Single(model => model.InventoryId.ToString() == ItemId);
            if (Session["CartCounter"] != null)
            {
                listOfShoppingCartModels = Session["CartItem"] as List<CartViewModel>;
            }
            if (listOfShoppingCartModels.Any(model => model.ItemId == ItemId))
            {
                objShoppingCartModel = listOfShoppingCartModels.Single(model => model.ItemId == ItemId);
                objShoppingCartModel.Quantity = objShoppingCartModel.Quantity + 1;
                objShoppingCartModel.Total = objShoppingCartModel.Quantity * objShoppingCartModel.UnitPrice;
            }
            else
            {
                objShoppingCartModel.ItemId = ItemId;
                objShoppingCartModel.ItemName = objPro.ProductName;
                objShoppingCartModel.Quantity = 1;
                objShoppingCartModel.Total = objPro.SellingPrice;
                objShoppingCartModel.UnitPrice = objPro.SellingPrice;
                listOfShoppingCartModels.Add(objShoppingCartModel);
            }

            Session["CartCounter"] = listOfShoppingCartModels.Count;
            Session["CartItem"] = listOfShoppingCartModels;
            return Json(new { Success = true, Counter = listOfShoppingCartModels.Count }, JsonRequestBehavior.AllowGet);
        }

       
        public ActionResult ShoppingCart()
        {
            listOfShoppingCartModels = Session["CartItem"] as List<CartViewModel>;
            return View(listOfShoppingCartModels);
        }

        public ActionResult EmptyCart()
        {
            return View();
        }

    }
}