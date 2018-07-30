using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_RailWay.Infrastructure;

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

        // Поиск вагонов по номеру 
        [Access(LogVisit = true)]
        public ActionResult Vagon(int? num, DateTime? dt_uz, DateTime? dt_inp, DateTime? dt_out) //int? day, int? month, int? year, int? hour, int? minute, int? second
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

        public ActionResult PluginTest() { 
                return View();
        }
    }
}