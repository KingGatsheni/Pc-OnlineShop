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
                objShoppingCartModel.SubTotal = objShoppingCartModel.Quantity * objShoppingCartModel.UnitPrice;

            }
            else
            {
                objShoppingCartModel.ItemId = ItemId;
                objShoppingCartModel.ItemName = objPro.ProductName;
                objShoppingCartModel.Quantity = 1;
                objShoppingCartModel.Total = objPro.SellingPrice;
                objShoppingCartModel.UnitPrice = objPro.SellingPrice;
                objShoppingCartModel.SubTotal = objPro.SellingPrice;
                objShoppingCartModel.Image = objPro.ImageName;
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

        public ActionResult Remove(int id)
        {
            List<CartViewModel> cart = (List<CartViewModel>)Session["CartItem"];
            foreach(var item in cart)
            {
                if(item.ItemId == id.ToString())
                {
                    cart.Remove(item);
                    break;
                }
            }
            Session["CartItem"] = cart;
            Session["CartCounter"] = cart.Count;

            return RedirectToAction("ShoppingCart","Store");
        }

        public ActionResult Plus(int id)
        {
            List<CartViewModel> cart = (List<CartViewModel>)Session["CartItem"];
            foreach (var item in cart)
            {
                if (item.ItemId == id.ToString())
                {
                    item.Quantity = item.Quantity + 1;
                    item.SubTotal = item.Quantity * item.UnitPrice;
                    item.Total = item.Quantity * item.UnitPrice;
                   
                }
            }
            Session["CartItem"] = cart;
            return RedirectToAction("ShoppingCart", "Store");
        }

        public ActionResult Minus(int id)
        {
            List<CartViewModel> cart = (List<CartViewModel>)Session["CartItem"];
            foreach (var item in cart)
            {
                if (item.ItemId == id.ToString() && item.Quantity !=1) 
                {
                    item.Quantity = item.Quantity - 1;
                    item.SubTotal = item.Quantity * item.UnitPrice;
                    item.Total = item.Quantity * item.UnitPrice;
                   
                }
            }
            Session["CartItem"] = cart;
            return RedirectToAction("ShoppingCart", "Store");
        }

        [Authorize]
        public ActionResult Checkout()
        {
            return View();
        }

     
        public ActionResult Search(string searching)
        {
            var productObj = from s in context.Inventories select new ProductViewModel() { ProductId = s.InventoryId, ProductName = s.ProductName, CategoryName = s.Category, ProductPrice = s.SellingPrice, Image = s.ImageName };
            if (!String.IsNullOrEmpty(searching))
            {
                productObj = productObj.Where(s => s.ProductName.Contains(searching));
            }

            return View(productObj.ToList());
        }

    }
}