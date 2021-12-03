using OnlineShop.Models;
using System;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Data.SqlClient;

namespace OnlineShop.Controllers
{
    [Authorize]
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
        public ActionResult Index(Customer customer)
        {
            var profile = new Customer()
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = User.Identity.GetUserName(),
                CellNo = customer.CellNo,
                Address = customer.Address,
                ApplicationUserId = User.Identity.GetUserId(),
                City = customer.City,
                Province = customer.Province,
                ZipCode = customer.ZipCode
            };

            context.Customers.Add(profile);
            context.SaveChanges();
            ViewBag.Message = $"Registered Sucessfully, {customer.FirstName} {customer.LastName}";
            return RedirectToAction("Index", "Home");

        }

       public ActionResult EditProfile()
        {           
            return View();
        }
        [HttpPost]
        public ActionResult EditProfile(Customer  customer)
        {
            string userId = User.Identity.GetUserId().ToString();
           using(SqlConnection conn = new SqlConnection(StoreController.connectionString))
            {
                conn.Open();
                string query = "update Customers set FirstName = @FirstName, LastName=@LastName,CellNo=@CellNo,Address=@Address,City=@City,Province=@Province,ZipCode=@ZipCode where ApplicationUserId = @ApplicationUserId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@FirstName", customer.FirstName);
                cmd.Parameters.AddWithValue("@LastName", customer.LastName);
                cmd.Parameters.AddWithValue("@CellNo", customer.CellNo);
                cmd.Parameters.AddWithValue("@Address", customer.Address);
                cmd.Parameters.AddWithValue("@City", customer.City);
                cmd.Parameters.AddWithValue("@Province", customer.Province);
                cmd.Parameters.AddWithValue("@ZipCode", customer.ZipCode);
                cmd.Parameters.AddWithValue("@ApplicationUserId", userId);

                cmd.ExecuteNonQuery();
                cmd.Dispose();
                conn.Close();

            }          
          
            ViewBag.Message = "Customer info was Updated";
            return RedirectToAction("Index", "Home");
        }
    }
}