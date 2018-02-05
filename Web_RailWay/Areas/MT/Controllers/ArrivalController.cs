using EFMT.Abstract;
using EFMT.Concrete;
using EFMT.Entities;
using EFRC.Abstract;
using EFRC.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Web_RailWay.Infrastructure;

namespace Web_RailWay.Areas.MT.Controllers
{
    public class ArrivalController : Controller
    {
        IMT ef_mt;

        public ArrivalController(IMT mt)
        {
            this.ef_mt = mt;
        }

        // GET: Arrival

        public ActionResult Index()
        {
            return View();
        }

        [Access(LogVisit = true)]
        public ActionResult Sostav()
        {
            ViewBag.station = 0;
            return View();
        }
        /// <summary>
        /// Показать меню составов
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
        /// <summary>
        /// Показать меню операций
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Показать детальную информацию о вагонах
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PartialViewResult DetaliSostavOperation(int id)
        {
            // Сохраняем выбранную культуру в куки
            HttpCookie cookie = Request.Cookies["view-cars"];
            ViewBag.view_cars = int.Parse(cookie.Value);
            ArrivalSostav sostav = this.ef_mt.GetArrivalSostav(id);
            return PartialView(sostav);
        }
        /// <summary>
        /// Показать кнопку закрыть вагон
        /// </summary>
        /// <param name="id_sostav"></param>
        /// <param name="id"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Показать вагоны
        /// </summary>
        /// <returns></returns>
        [Access(LogVisit = true)]
        public ActionResult Cars()
        {
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
        /// <summary>
        /// Показать список вагонов не принятых на АМКР
        /// </summary>
        /// <param name="date_start"></param>
        /// <param name="date_stop"></param>
        /// <param name="station"></param>
        /// <returns></returns>
        public PartialViewResult ListNotArrival(DateTime date_start, DateTime date_stop, int? station)
        {
            List<IGrouping<string, ArrivalCars>> list = new List<IGrouping<string, ArrivalCars>>();
            station = station != null ? station : 0;
            if (station != 0)
            {
                list = this.ef_mt.GetArrivalCarsOfConsignees(this.ef_mt.GetConsigneeToCodes(mtConsignee.AMKR))
                    .Where(x => x.DateOperation >= date_start & x.DateOperation <= date_stop & x.NumDocArrival == null & x.StationCode == station)
                    .OrderByDescending(x => x.DateOperation)
                    .GroupBy(x => x.CompositionIndex)
                    .ToList();
            }
            else
            {
                list = this.ef_mt.GetArrivalCarsOfConsignees(this.ef_mt.GetConsigneeToCodes(mtConsignee.AMKR))
                    .Where(x => x.DateOperation >= date_start & x.DateOperation <= date_stop & x.NumDocArrival == null)
                    .OrderByDescending(x => x.DateOperation)
                    .GroupBy(x => x.CompositionIndex)
                    .ToList();
            }
            return PartialView(list);
        }

        /// <summary>
        /// Показать результат быстрого решения 
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public PartialViewResult QuickCorrectArrivalCars(int num)
        {
            ArrivalCars car = this.ef_mt.ArrivalCars
                .Where(x => x.Num ==  num & x.NumDocArrival == null)
                    .OrderByDescending(x => x.ID)
                    .FirstOrDefault();
            if (car == null) return null;
            return PartialView(car);
        }
        /// <summary>
        /// Показать результат детального решения 
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public PartialViewResult DetailedCorrectArrivalCars(int num)
        {
            List<ArrivalCars> cars = this.ef_mt.ArrivalCars
                    .Where(x => x.Num == num)
                    .OrderByDescending(x => x.ID)
                    .ToList();
             return PartialView(cars);
        }

        /// <summary>
        /// Показать список прибывших вагонов
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public PartialViewResult ListArrivalCars(List<ArrivalCars> list)
        {
            return PartialView(list);
        }
        /// <summary>
        /// Показать вагон
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public PartialViewResult ArrivalCar(ArrivalCars car)
        {
            return PartialView(car);
        }
    }
}