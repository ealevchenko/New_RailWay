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

        // Поиск составов по натурке 
        public ActionResult Natur(int? natur, int? day, int? month, int? year, int? hour, int? minute)
        {
            return View();
        }

        // Поиск вагонов по номеру 
        public ActionResult Vagon(int? num)
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