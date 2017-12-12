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

        //private void SaveCookie(string key, string val) {
        //    HttpCookie cookie = Request.Cookies[key];
        //    if (cookie != null)
        //    {
        //        cookie.Value = val;
        //    }
        //    else
        //    {
        //        cookie = new HttpCookie(key);
        //        cookie.HttpOnly = false;
        //        cookie.Value = val;
        //        cookie.Expires = DateTime.Now.AddYears(1);
        //    }
        //    Response.Cookies.Add(cookie);
        //}

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
            //ViewBag.view_sostav_id = list.FirstOrDefault();
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

        public PartialViewResult DetaliSostavOperation(int id)
        {
            // Сохраняем выбранную культуру в куки
            HttpCookie cookie = Request.Cookies["view-cars"];
            //if (cookie == null)
            //{
            //    cookie = new HttpCookie("view-cars");
            //    cookie.HttpOnly = false;
            //    cookie.Value = "0";
            //    cookie.Expires = DateTime.Now.AddYears(1);
            //    Response.Cookies.Add(cookie);
            //}
            //HttpCookie cookie_id = Request.Cookies["id-operation"];
            //if (id == null & cookie_id != null)
            //{
            //    id = int.Parse(cookie_id.Value);
            //}

            ViewBag.view_cars = int.Parse(cookie.Value);
            ArrivalSostav sostav = this.ef_mt.GetArrivalSostav(id);
            return PartialView(sostav);
        }

        [View]
        public PartialViewResult ButtonCloseArrivalCar(int id_sostav, int? id)
        {
            if (id == null) return null;
            ViewBag.id_sostav = id_sostav;
            return PartialView(id);
        }

        public RedirectToRouteResult CloseArrivalCrs(int id_sostav, int id_car)
        {
            //ars.CloseArrivalSostav(IDOrcSostav);
            //ViewBag.Result = "Ок";
            return RedirectToAction("DetaliSostavOperation", "MTArrival", new { id_sostav });
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