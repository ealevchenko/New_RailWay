using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web_RailWay.Areas.KIS.Controllers
{
    public class HomeController : Controller
    {
        // GET: KIS
        public ActionResult Index()
        {
            return View();
        }

        // Оборот вагонов 
        public ActionResult Turnover()
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