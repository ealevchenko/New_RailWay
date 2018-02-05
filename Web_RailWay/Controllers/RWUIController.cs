using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web_RailWay.Controllers
{
    public class RWUIController : Controller
    {
        // GET: RWUI
        public ActionResult Index(int id=6)
        {
            return View(id);
        }
    }
}