using OnlineShop.Models;
using System;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace OnlineShop.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
        ApplicationDbContext context;

        public ProfileController()
        {
            this.context = new ApplicationDbContext();
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Customer customer) {
            var profile = new Customer() {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = User.Identity.GetUserName(),
                CellNo = customer.CellNo,
                Address = customer.Address,
                ApplicationUserId = User.Identity.GetUserId(),
            };

            context.Customers.Add(profile);
            context.SaveChanges();

            return RedirectToAction("Index", "Home");

        }

    }
}