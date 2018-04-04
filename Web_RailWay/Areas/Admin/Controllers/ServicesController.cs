using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_RailWay.Infrastructure;

namespace Web_RailWay.Areas.Admin.Controllers
{
    public class ServicesController : Controller
    {
        // GET: Services
        [Access(LogVisit = true)]
        public ActionResult Index()
        {
            return View();
        }
    }
}