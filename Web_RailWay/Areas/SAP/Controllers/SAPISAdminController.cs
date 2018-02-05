using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Web_RailWay.Controllers
{
    public class SAPISAdminController : Controller
    {
        // GET: SAPISAdmin
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Сервис коррекции справочника вх. поставка (коррекия вагонов принятых вручную отсутсвующих в системе МТ)
        /// </summary>
        /// <returns></returns>
        public ActionResult CorrectArrival()
        {
            ViewBag.dt_start = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? DateTime.Now.AddDays(-1).Date.ToString("MM/dd/yyyy 00:00:00") : DateTime.Now.Date.ToString("dd.MM.yyyy 00:00:00");
            ViewBag.dt_stop = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? DateTime.Now.AddDays(1).Date.AddSeconds(-1).ToString("MM/dd/yyyy 23:59:00") : DateTime.Now.AddDays(1).Date.AddSeconds(-1).ToString("dd.MM.yyyy 23:59:00");
            //ViewBag.dt_start = new DateTime(2017,4,4,0,40,0).ToString("dd.MM.yyyy HH:mm:ss"); 
            //ViewBag.dt_stop= new DateTime(2017,4,4,4,59,0).ToString("dd.MM.yyyy HH:mm:ss");
            return View();
        }
    }
}