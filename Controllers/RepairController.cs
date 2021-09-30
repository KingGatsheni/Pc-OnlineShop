using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Controllers
{
    [Authorize]
    public class RepairController : Controller
    {
        // GET: Repair
        public ActionResult Index()
        {
            return View();
        }
    }
}