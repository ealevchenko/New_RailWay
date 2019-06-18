using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_RailWay.Infrastructure;

namespace Web_RailWay.Areas.TD.Controllers
{
    public class AdminController : Controller
    {
        // GET: TD/Admin
        public ActionResult Index()
        {
            return View();
        }
        [Access(LogVisit = true)]
        public ActionResult Marriage()
        {
            return View();
        }

    }
}