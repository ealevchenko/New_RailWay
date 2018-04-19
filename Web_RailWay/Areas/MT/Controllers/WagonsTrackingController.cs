using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_RailWay.Infrastructure;

namespace Web_RailWay.Areas.MT.Controllers
{
    public class WagonsTrackingController : Controller
    {
        // GET: MT/WagonsTracking
        [Access(LogVisit = true)]
        public ActionResult Index()
        {
            return View();
        }
        [Access(LogVisit = true)]
        public ActionResult ReportOnline()
        {
            return View();
        }
        [Access(LogVisit = true)]
        public ActionResult ReportArhive()
        {
            return View();
        }

        [Access(LogVisit = true)]
        public ActionResult Routes()
        {
            return View();
        }

        [Access(LogVisit = true)]
        public ActionResult Operations()
        {
            return View();
        }
    }
}