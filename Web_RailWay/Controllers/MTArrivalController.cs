using EFMT.Abstract;
using EFMT.Concrete;
using EFMT.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Web_RailWay.Infrastructure;

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

        [Access(LogVisit = true)]
        public ActionResult Sostav()
        {
            ViewBag.dt_start = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? DateTime.Now.Date.ToString("MM/dd/yyyy 00:00") : DateTime.Now.Date.ToString("dd.MM.yyyy 00:00");
            ViewBag.dt_stop = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? DateTime.Now.AddDays(1).Date.AddSeconds(-1).ToString("MM/dd/yyyy 23:59") : DateTime.Now.AddDays(1).Date.AddSeconds(-1).ToString("dd.MM.yyyy 23:59");
            ViewBag.station = 0;
            return View();
        }
        /// <summary>
        /// Сформировать ajax отчет
        /// </summary>
        /// <param name="date_start"></param>
        /// <param name="date_stop"></param>
        /// <returns></returns>
        public PartialViewResult ListSostav(DateTime date_start, DateTime date_stop, int? station)
        {
            List<int> list = new List<int>();
            station = station != null ? station : 0;
            if (station != 0)
            {
                list = this.ef_mt.ArrivalSostav
                .Where(x => x.ParentID == null & x.DateTime >= date_start & x.DateTime <= date_stop & x.CompositionIndex.Substring(9, 4) == station.ToString().Substring(0, 4))
                .OrderByDescending(x => x.DateTime)
                .Select(x => x.ID)
                .ToList();
            }
            else
            {
                list = this.ef_mt.ArrivalSostav
                .Where(x => x.ParentID == null & x.DateTime >= date_start & x.DateTime <= date_stop)
                .OrderByDescending(x => x.DateTime)
                .Select(x => x.ID)
                .ToList();
            }


            return PartialView(list);
        }

        public PartialViewResult ListSostavOperation(int id)
        {
            List<int> list = new List<int>();
            int id_arrival = this.ef_mt.GetArrivalSostav(id).IDArrival;
            list = this.ef_mt.ArrivalSostav
                .Where(x => x.IDArrival == id_arrival)
                .OrderBy(x => x.ID)
                .Select(x => x.ID)
                .ToList();
            return PartialView(list);
        }

        public PartialViewResult DetaliSostavOperation(int id, int? cars)
        {
            
            ViewBag.cars = cars!=null ? cars: 0;
            ArrivalSostav sostav = this.ef_mt.GetArrivalSostav(id);
            return PartialView(sostav);
        }
        /// <summary>
        /// Показать вагоны
        /// </summary>
        /// <returns></returns>
        [Access(LogVisit = true)]
        public ActionResult Cars()
        {
            ViewBag.dt_start = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? DateTime.Now.AddDays(-1).Date.ToString("MM/dd/yyyy 00:00") : DateTime.Now.Date.ToString("dd.MM.yyyy 00:00");
            ViewBag.dt_stop = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? DateTime.Now.AddDays(1).Date.AddSeconds(-1).ToString("MM/dd/yyyy 23:59") : DateTime.Now.AddDays(1).Date.AddSeconds(-1).ToString("dd.MM.yyyy 23:59");
            ViewBag.station = 0;
            return View();
        }
        /// <summary>
        /// Показать список вагонов по составам
        /// </summary>
        /// <param name="date_start"></param>
        /// <param name="date_stop"></param>
        /// <param name="station"></param>
        /// <returns></returns>
        public PartialViewResult ListSostavArrival(DateTime date_start, DateTime date_stop, int? station)
        {
            List<int> list = new List<int>();
            station = station != null ? station : 0;
            if (station != 0)
            {
                list = this.ef_mt.GetArrivalCarsOfConsignees(this.ef_mt.GetConsigneeToCodes(mtConsignee.AMKR))
                    .Where(x => x.DateOperation >= date_start & x.DateOperation <= date_stop & x.NumDocArrival == null & x.StationCode == station)
                    .GroupBy(x => x.IDSostav)
                    .OrderByDescending(x => x.Key)
                    .Select(x => x.Key)
                    .ToList();
            }
            else { 
                list = this.ef_mt.GetArrivalCarsOfConsignees(this.ef_mt.GetConsigneeToCodes(mtConsignee.AMKR))
                    .Where(x => x.DateOperation >= date_start & x.DateOperation <= date_stop & x.NumDocArrival == null)
                    .GroupBy(x => x.IDSostav)
                    .OrderByDescending(x => x.Key)
                    .Select(x => x.Key)
                    .ToList();            
            }
            return PartialView(list);
        }

    }
}