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

namespace Web_RailWay.Areas.MT.Controllers
{
    public class MTAdminController : Controller
    {
        //IMT ef_mt;

        //public MTAdminController(IMT mt)
        //{
        //    this.ef_mt = mt;
        //}

        // GET: MTAdmin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ReportArrival()
        {
            ViewBag.dt_start = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? DateTime.Now.AddDays(-1).Date.ToString("MM/dd/yyyy 00:00") : DateTime.Now.Date.ToString("dd.MM.yyyy 00:00");
            ViewBag.dt_stop = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? DateTime.Now.AddDays(1).Date.AddSeconds(-1).ToString("MM/dd/yyyy 23:59") : DateTime.Now.AddDays(1).Date.AddSeconds(-1).ToString("dd.MM.yyyy 23:59");
            ViewBag.station = 0;
            return View();
        }

        public PartialViewResult ListReportArrival(DateTime date_start, DateTime date_stop, int station)
        {
            ViewBag.dt_start = date_start;
            ViewBag.dt_stop = date_stop;
            ViewBag.station = station;
            return PartialView();
        }
        ///// <summary>
        ///// Показать список вагонов не принятых на АМКР
        ///// </summary>
        ///// <param name="date_start"></param>
        ///// <param name="date_stop"></param>
        ///// <param name="station"></param>
        ///// <returns></returns>
        //public PartialViewResult ListNotArrival(DateTime date_start, DateTime date_stop, int? station)
        //{
        //    List<IGrouping<string, ArrivalCars>> list = new List<IGrouping<string, ArrivalCars>>();
        //    station = station != null ? station : 0;
        //    if (station != 0)
        //    {
        //        list = this.ef_mt.GetArrivalCarsOfConsignees(this.ef_mt.GetConsigneeToCodes(mtConsignee.AMKR))
        //            .Where(x => x.DateOperation >= date_start & x.DateOperation <= date_stop & x.NumDocArrival == null & x.StationCode == station)
        //            .OrderByDescending(x => x.DateOperation)
        //            .GroupBy(x => x.CompositionIndex)
        //            .ToList();
        //    }
        //    else
        //    {
        //        list = this.ef_mt.GetArrivalCarsOfConsignees(this.ef_mt.GetConsigneeToCodes(mtConsignee.AMKR))
        //            .Where(x => x.DateOperation >= date_start & x.DateOperation <= date_stop & x.NumDocArrival == null)
        //            .OrderByDescending(x => x.DateOperation)
        //            .GroupBy(x => x.CompositionIndex)
        //            .ToList();
        //    }
        //    return PartialView(list);
        //}

        [View]
        public PartialViewResult ButtonCloseArrivalCar(int id_sostav, int? id)
        {
            if (id == null) return null;
            ViewBag.id_sostav = id_sostav;
            return PartialView(id);
        }
        /// <summary>
        /// Закрыть вагон
        /// </summary>
        /// <param name="id_sostav"></param>
        /// <param name="id_car"></param>
        /// <returns></returns>
        public RedirectToRouteResult CloseArrivalCrs(int id_sostav, int id_car)
        {
            //ars.CloseArrivalSostav(IDOrcSostav);
            //ViewBag.Result = "Ок";
            return RedirectToAction("DetaliSostavOperation", "Arrival", new { id_sostav });
        }
    }
}