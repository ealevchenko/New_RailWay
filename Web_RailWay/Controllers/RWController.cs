using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web_RailWay.Controllers
{
    public class RWController : Controller
    {
        // GET: RW
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