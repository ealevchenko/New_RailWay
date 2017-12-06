using EFMT.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Web_RailWay.Controllers
{
    public class MTArrivalController : Controller
    {
        IMT ef_mt;

        public MTArrivalController(IMT mt)
        {
            this.ef_mt = mt;
        }

        // GET: MTArrival
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Sostav()
        {
            ViewBag.dt_start = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? DateTime.Now.Date.ToString("MM/dd/yyyy 00:00") : DateTime.Now.Date.ToString("dd.MM.yyyy 00:00");
            ViewBag.dt_stop = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? DateTime.Now.AddDays(1).Date.AddSeconds(-1).ToString("MM/dd/yyyy 23:59") : DateTime.Now.AddDays(1).Date.AddSeconds(-1).ToString("dd.MM.yyyy 23:59");
            return View();
        }

        /// <summary>
        /// Сформировать ajax отчет
        /// </summary>
        /// <param name="date_start"></param>
        /// <param name="date_stop"></param>
        /// <returns></returns>
        public PartialViewResult ListSostav(DateTime date_start, DateTime date_stop)
        {
            List<int> list = new List<int>();
            list = this.ef_mt.ArrivalSostav
                .Where(x => x.ParentID == null & x.DateTime >= date_start & x.DateTime <= date_stop)
                .OrderByDescending(x => x.DateTime)
                .Select(x => x.ID)
                .ToList();
            return PartialView(list);
        }
    }
}