using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web_RailWay.Areas.TD.Controllers
{
    public class HomeController : Controller
    {
        // GET: TD/Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Marriage()
        {
            return View();
        }
    }
}