using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web_RailWay.Areas.RW.Controllers
{
    public class HomeController : Controller
    {
        // GET: RW
        [Access(LogVisit = true)]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Список ссылок на отчеты администрирования
        /// </summary>
        /// <returns></returns>
        public PartialViewResult LinkRWControl()
        {
            return PartialView();
        }
    }
}