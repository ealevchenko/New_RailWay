using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web_RailWay.Areas.MT.Controllers
{
    public class WagonsTrackingController : Controller
    {
        // GET: MT/WagonsTracking
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ReportOnline()
        {
            return View();
        }

        public ActionResult ReportArhive()
        {
            return View();
        }
    }
}