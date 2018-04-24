using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_RailWay.Infrastructure;

namespace Web_RailWay.Areas.MT.Controllers
{
    public class AdminController : Controller
    {
        // GET: MT/Admin
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Получить список натурных листов полученных от МТ
        /// </summary>
        /// <returns></returns>
        [Access(LogVisit = true)]
        public ActionResult Natur()
        {
            return View();
        }
    }
}