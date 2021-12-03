using Microsoft.AspNet.Identity;
using OnlineShop.Models;
using OnlineShop.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;
using OnlineShop.Notifications;
using System.Data.SqlClient;
using System.Configuration;

namespace OnlineShop.Controllers
{
    public class StoreController : Controller
    {
        private ApplicationDbContext context; // declare database instance
        private List<CartViewModel> listOfShoppingCartModels;
        private List<WishListViewModel> listOfWishListsModel;
        public static string connectionString = ConfigurationManager.ConnectionStrings["Model1"].ConnectionString;
        public StoreController()
        {
            this.context = new ApplicationDbContext();
            listOfShoppingCartModels = new List<CartViewModel>();
            listOfWishListsModel = new List<WishListViewModel>();
        }
        public ActionResult Index(string searching, int? page)
        {
            IEnumerable<ProductViewModel> listOfProducts = GetProducts();

            if (Session["listOfProducts"] == null)
            {
                Session["listOfProducts"] = listOfProducts;
            }


            if (!String.IsNullOrEmpty(searching))
            {
                listOfProducts = listOfProducts.Where(s => s.ProductName.IndexOf(searching, StringComparison.OrdinalIgnoreCase) >= 0  || s.CategoryName.IndexOf(searching, StringComparison.OrdinalIgnoreCase) >= 0);
               
            }
            return View(listOfProducts.ToPagedList(page ?? 1, 8));
        }
        
        [HttpPost]
        public JsonResult Index(string ItemId, bool? isUsed)
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
            SaveCartTotalToSession(listOfShoppingCartModels);
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
            if (cart == null)
            {
                Session["CartItem"] = null;
            }
            SaveCartTotalToSession(cart);

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
            SaveCartTotalToSession(cart);
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
            SaveCartTotalToSession(cart);
            return RedirectToAction("ShoppingCart", "Store");
        }

        [Authorize]
        public ActionResult Checkout()
        {
            return View();
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CheckOut(CardValidation validate)
        {
            if (ModelState.IsValid)
            {
                ViewBag.CardNumber = validate.CardNumber;
                ViewBag.Expiry = validate.Expiry;
                ViewBag.Cvv = validate.Cvv;             
            }
            return View(validate);
        }

        [Authorize]
        public ActionResult Wishlist()
        {
            IEnumerable<WishListViewModel> listOfObjects = from wishObj in context.WishLists
                                                           select new WishListViewModel()
                                                           {
                                                               ItemId = wishObj.ItemId,
                                                               UserId = wishObj.UserId,
                                                               ItemName = wishObj.ItemName,
                                                               ItemImage = wishObj.ItemImage,
                                                               ItemPrice = (double)wishObj.ItemPrice
                                                           };
            var wishListItem = listOfObjects.Where(x => x.UserId == User.Identity.GetUserId());
            if (Session["WishCounter"] != null)
            {
                Session["WishCounter"] = wishListItem.Count();
            }
            return View(wishListItem);
        }

        [HttpGet]
        public ActionResult Getlist()
        {
            listOfWishListsModel = Session["WishList"] as List<WishListViewModel>;
            return PartialView("_WishListModal", listOfWishListsModel);
        }

        [HttpPost]
        public JsonResult AddToWish(string ItemId)
        {
            WishListViewModel wishListModel = new WishListViewModel();
            WishItem wishlist = new WishItem();
            Inventory objPro = context.Inventories.Single(model => model.InventoryId.ToString() == ItemId);
            if (Session["WishCounter"] != null)
            {
                listOfWishListsModel = Session["WishList"] as List<WishListViewModel>;
            }
            //add items to our wishlist 
            wishListModel.ItemName = objPro.ProductName;
            wishListModel.ItemImage = objPro.ImageName;
            wishListModel.ItemPrice = (double)objPro.SellingPrice;
            wishListModel.ItemId = Int32.Parse(ItemId);
            //listOfWishListsModel.Add(wishListModel);

            //Save Item to wishlist table in database
            try
            {
                    wishlist.ItemId = Int32.Parse(ItemId);
                    wishlist.UserId = User.Identity.GetUserId();
                    wishlist.ItemName = objPro.ProductName;
                    wishlist.ItemImage = objPro.ImageName;
                    wishlist.ItemPrice = objPro.SellingPrice;
                    context.WishLists.Add(wishlist);
                    context.SaveChanges(); //save to database
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            if (User.Identity.IsAuthenticated)
            {
                Session["WishCounter"] = listOfWishListsModel.Count;
                Session["WishList"] = listOfWishListsModel;
                ViewBag.Message = "added to wish list";
            }
            return Json(new { Success = true, Counter = listOfWishListsModel.Count }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ProcessOrder()
        {
            List<CartViewModel> listOfOrderToProcess = (List<CartViewModel>)Session["CartItem"];
            Sale sale = new Sale();
            SalesItem salesItem = new SalesItem();
            Payment payment = new Payment();
            Customer customer = new Customer();
            Message message = new Message();
            decimal OrderTotal = 0.00M;
           

            //Saving Sale or Order Data
            foreach(var item in listOfOrderToProcess)
            {
                OrderTotal += item.Total;
            }
            sale.Total = OrderTotal;
            sale.Date = DateTime.Now;
            sale.UserId = User.Identity.GetUserId();
            sale.EmployeeId = null;

            context.Sales.Add(sale);
            context.SaveChanges();

            int id = context.Sales.Max(o => o.SaleId);
            if (listOfOrderToProcess.Count > 0)
            {
               
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                   
                    conn.Open();                  
                    foreach (var item in listOfOrderToProcess)
                    {
                        string query = "insert into SalesItems values(@SaleId,@InventoryId,@Quantity,@ItemPrice)";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@SaleId",id);
                        cmd.Parameters.AddWithValue("@InventoryId",item.ItemId);
                        cmd.Parameters.AddWithValue("@Quantity", item.Quantity);
                        cmd.Parameters.AddWithValue("ItemPrice",item.UnitPrice);
                        cmd.ExecuteNonQuery();
                        
                        SqlCommand cmd2 = new SqlCommand("update Inventories set Quantity = Quantity - '"+item.Quantity+"' where InventoryId = '"+item.ItemId+"'",conn);
                        cmd2.ExecuteNonQuery();

                        cmd.Dispose();
                        cmd2.Dispose();
                    }
                   
                    conn.Close();
                }
               
                
            }
           

            //Save Payment Info
            payment.EmployeeId = null;
            payment.PaymentMethod = "Online Card Payment";
            payment.SaleId = id;
            payment.TotalAmount = OrderTotal;
            payment.RepairId = null;
            context.Payments.Add(payment);
            context.SaveChanges();
           // message.Execute();

            //clear Sessions
            Session["CartItem"] = null;
            Session["CartCounter"] = null;
            Session["CartTotal"] = null;

            ViewBag.Message = "Order Successful";
            return RedirectToAction("TranscationSucess", "Store");
        }

        private void SaveCartTotalToSession(List<CartViewModel> cart)
        {
            decimal a = 0.00M;
            foreach (var item in cart)
            {
                a += item.SubTotal;
            }
            Session["CartTotal"] = a;
        }

        private List<ProductViewModel> GetProducts()
        {
           
              ProductViewModel products = new ProductViewModel();
              var product  = (from itemObj in context.Inventories
                                           select new ProductViewModel {
                                               ProductId = itemObj.InventoryId,
                                               ProductName = itemObj.ProductName,
                                               Qty = itemObj.Quantity,
                                               CategoryName = itemObj.Category,
                                               ProductPrice = itemObj.SellingPrice,
                                               Image = itemObj.ImageName
                                           })
                           . ToList();
                return product;
           
        }
        [HttpPost]
        public JsonResult Wishlist(string ItemId)
        {
            CartViewModel objShoppingCartModel = new CartViewModel();
            WishItem objPro = context.WishLists.Single(model => model.ItemId.ToString() == ItemId);
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
                objShoppingCartModel.ItemName = objPro.ItemName;
                objShoppingCartModel.Quantity = 1;
                objShoppingCartModel.Total = objPro.ItemPrice;
                objShoppingCartModel.UnitPrice = objPro.ItemPrice;
                objShoppingCartModel.SubTotal = objPro.ItemPrice;
                objShoppingCartModel.Image = objPro.ItemImage;
                listOfShoppingCartModels.Add(objShoppingCartModel);
            }

            WishItem del = context.WishLists.Single(m => m.ItemId.ToString() == ItemId);
            context.WishLists.Remove(del);
            context.SaveChanges();

            Session["CartCounter"] = listOfShoppingCartModels.Count;
            Session["CartItem"] = listOfShoppingCartModels;
            SaveCartTotalToSession(listOfShoppingCartModels);
            return Json(new { Success = true, Counter = listOfShoppingCartModels.Count }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RemoveFromWishList(string ItemID)
        {
            WishListViewModel wish = new WishListViewModel();
            var del = context.WishLists.Single(m => m.ItemId.ToString() == ItemID);
            context.WishLists.Remove(del);
            context.SaveChanges();
            return RedirectToAction("WishList","Store");
        }

        public ActionResult TranscationSucess()
        {
            return View();
        }

    }
}