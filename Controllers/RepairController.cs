using OnlineShop.Models;
using OnlineShop.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;

namespace OnlineShop.Controllers
{
    [Authorize]
    public class RepairController : Controller
    {
        ApplicationDbContext db;
        List<RepairViewModel> repairs;
        public RepairController()
        {
            this.db = new ApplicationDbContext();
            repairs = new List<RepairViewModel>();

        }
        // GET: Repair
        public ActionResult Index()
        {
            return View(GetRepairItems());
        }

        private IEnumerable<RepairViewModel> GetRepairItems()
        {

            
                try
                {
                    string userId = User.Identity.GetUserId().ToString();
                    int Id = db.Customers.First(o => o.ApplicationUserId == userId).CustomerId;
                    IEnumerable<RepairViewModel> repairList = from r in db.Repairs
                                                              join rp in db.RepairLists on r.RepairId equals rp.RepairId
                                                              where r.CustomerId == Id
                                                              select new RepairViewModel
                                                              {
                                                                  RepairId = r.RepairId,
                                                                  Quantity = rp.Quantity,
                                                                  ItemName = rp.ItemName,
                                                                  RepairPrice = rp.RepairPrice,
                                                                  RepairStatus = r.RepairStatus,
                                                                  BookedOn = r.Bookon                                             
                                                              };
                    return repairList;
                }
                catch (Exception s)
                {

                }
            return null;
        }
    }
}