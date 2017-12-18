using EFMT.Abstract;
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
    public class MTApproachesController : Controller
    {
        IMT ef_mt;

        public MTApproachesController(IMT mt)
        {
            this.ef_mt = mt;
        }

        // GET: MTApproaches
        public ActionResult Index()
        {
            return View();
        }
        [Access(LogVisit = true)]
        public ActionResult Sostav()
        {
            ViewBag.dt_start = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? DateTime.Now.Date.ToString("MM/dd/yyyy 00:00") : DateTime.Now.Date.ToString("dd.MM.yyyy 00:00");
            ViewBag.dt_stop = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? DateTime.Now.AddDays(1).Date.AddSeconds(-1).ToString("MM/dd/yyyy 23:59") : DateTime.Now.AddDays(1).Date.AddSeconds(-1).ToString("dd.MM.yyyy 23:59");
            //ViewBag.dt_start = new DateTime(2017, 8, 10, 0, 40, 0).ToString("dd.MM.yyyy HH:mm:ss");
            //ViewBag.dt_stop = new DateTime(2017, 8, 30, 4, 59, 59).ToString("dd.MM.yyyy HH:mm:ss");
            ViewBag.station = 0;
            return View();
        }
        /// <summary>
        /// Получить список составов
        /// </summary>
        /// <param name="date_start"></param>
        /// <param name="date_stop"></param>
        /// <param name="station"></param>
        /// <returns></returns>
        public PartialViewResult ListSostav(DateTime date_start, DateTime date_stop, int? station)
        {
            List<int> list = new List<int>();
            station = station != null ? station : 0;
            if (station != 0)
            {
                list = this.ef_mt.ApproachesSostav
                .Where(x => x.ParentID == null & x.DateTime >= date_start & x.DateTime <= date_stop & x.CompositionIndex.Substring(9, 4) == station.ToString().Substring(0, 4))
                .OrderByDescending(x => x.DateTime)
                .Select(x => x.ID)
                .ToList();
            }
            else
            {
                list = this.ef_mt.ApproachesSostav
                .Where(x => x.ParentID == null & x.DateTime >= date_start & x.DateTime <= date_stop)
                .OrderByDescending(x => x.DateTime)
                .Select(x => x.ID)
                .ToList();
            }
            return PartialView(list);
        }
        /// <summary>
        /// Получить список истории движения состава
        /// </summary>
        /// <param name="id_sostav"></param>
        /// <param name="route"></param>
        /// <returns></returns>
        public PartialViewResult ListHistoryLocation(int id_sostav, bool route)
        {
            List<int> list = new List<int>();
            list = this.ef_mt.GetApproachesSostavLocation(id_sostav, route)
                    .Select(x => x.ID)
                    .ToList();
            return PartialView(list);
        }
        /// <summary>
        /// Показать детально состав
        /// </summary>
        /// <param name="id_sostav"></param>
        /// <returns></returns>
        public PartialViewResult SostavDetali(int id_sostav)
        {
            ApproachesSostav sostav = this.ef_mt.ApproachesSostav.Where(c => c.ID == id_sostav).FirstOrDefault();
            return PartialView(sostav);
        }
        [Access(LogVisit = true)]
        public ActionResult Cars()
        {
            ViewBag.station = 0;
            return View();
        }
        /// <summary>
        /// Получить сгруппированные даты незакрытых составов
        /// </summary>
        /// <param name="station"></param>
        /// <returns></returns>
        public PartialViewResult ListGroupDateCars(int? station)
        {
            List<DateTime> list = new List<DateTime>();
            list = this.ef_mt.GroupDateApproachesCars();
            return PartialView(list);
        }
        /// <summary>
        /// Получить вагоны на подходах сгруппированные погрузуза указанную дату
        /// </summary>
        /// <param name="date_start"></param>
        /// <param name="date_stop"></param>
        /// <returns></returns>
        public PartialViewResult ListArrivalCarsOfDate(DateTime? date_start, DateTime? date_stop) {
            List<IGrouping<int, ApproachesCars>> list = new List<IGrouping<int, ApproachesCars>>();
            list = this.ef_mt.ApproachesCars
                .Where(x => x.Arrival == null & x.DateOperation >= date_start & x.DateOperation <= date_stop)
                .OrderByDescending(x => x.DateOperation)
                .GroupBy(x => x.CargoCode)
                .ToList();
            return PartialView(list); 
        }

        public PartialViewResult ListCargoArrivalCars(List<IGrouping<int, ApproachesCars>> list) {
            return PartialView(list); 
        }

        //public PartialViewResult ListCarsOfDate(DateTime date)
        //{
        //    List<ApproachesCars> list = new List<ApproachesCars>();
        //    DateTime date_start = date.Date;
        //    DateTime date_stop = date.AddDays(2).AddSeconds(-1);
        //    list = this.ef_mt.ApproachesCars
        //        .Where(x => x.Arrival == null & x.DateOperation >= date_start & x.DateOperation <= date_stop)
        //        .OrderByDescending(x => x.DateOperation)
        //        .ToList();
        //    return PartialView(list);
        //}

    }
}