using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_RailWay.Infrastructure;

namespace Web_RailWay.Areas.MT.Controllers
{
    public class HomeController : Controller
    {
        // GET: MettalurgTrans
        [Access(LogVisit = true)]
        public ActionResult Index()
        {
            return View();
        }
    }
}