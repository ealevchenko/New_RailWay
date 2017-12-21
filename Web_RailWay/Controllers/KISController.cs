using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web_RailWay.Controllers
{
    public class KISController : Controller
    {
        // GET: KIS
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Список ссылок на отчеты копирования КИС
        /// </summary>
        /// <returns></returns>
        public PartialViewResult LinkKISControl()
        {
            return PartialView();
        }
    }
}