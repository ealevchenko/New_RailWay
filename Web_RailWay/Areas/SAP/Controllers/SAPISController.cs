using EFRC.Abstract;
using EFRC.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web_RailWay.Controllers
{
    public class SAPISController : Controller
    {
        ISAP ef_sap;

        public SAPISController(ISAP sap)
        {
            this.ef_sap = sap;
        }

        private int GetNumDocOfIndex(string index)
        {
            if (String.IsNullOrWhiteSpace(index)) return 0;
            int doc = index.IndexOf("N:");
            int dt = index.IndexOf(" D:");
            if (doc >= 0 & dt >= 0)
            {
                return int.Parse(index.Substring(doc + 2, dt - 1));
            }
            return 0;
        }

        // GET: SAPIS
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Получить список из таблиц САП входящие поставки созданные в ручную (нет подтверждения захода по данным МТ)
        /// </summary>
        /// <param name="date_start"></param>
        /// <param name="date_stop"></param>
        /// <returns></returns>
        public PartialViewResult ListManualArrival(DateTime date_start, DateTime date_stop)
        {
            List<IGrouping<string, SAPIncSupply>> list = new List<IGrouping<string, SAPIncSupply>>();
            list = this.ef_sap.SAPIncSupply
                .Where(x => x.DateTime >= date_start & x.DateTime <= date_stop & x.IDMTSostav < 0)
                .OrderBy(x => x.DateTime)
                .GroupBy(x => x.CompositionIndex)
                .ToList();
            return PartialView(list);
        }
        /// <summary>
        /// Получить список составов
        /// </summary>
        /// <param name="date_start"></param>
        /// <param name="date_stop"></param>
        /// <returns></returns>
        public PartialViewResult ListManualSostavArrival(DateTime date_start, DateTime date_stop)
        {
            List<string> list = new List<string>();
            list = this.ef_sap.SAPIncSupply
                .Where(x => x.DateTime >= date_start & x.DateTime <= date_stop & x.IDMTSostav < 0)
                .OrderBy(x => x.DateTime)
                .GroupBy(x => x.CompositionIndex)
                .Select(x => x.Key)
                .ToList();
            return PartialView(list);
        }

        public PartialViewResult ListCorrectCars(string index, bool manual)
        {
            ViewBag.natur = GetNumDocOfIndex(index);
            ViewBag.index = index;
            ViewBag.manual = manual;
            List<SAPIncSupply> list = new List<SAPIncSupply>();
            list = this.ef_sap.SAPIncSupply
                .Where(x => x.CompositionIndex == index)
                .OrderBy(x => x.Position)
                 .ToList();
            if (manual) {
                list = list.Where(x => x.IDMTSostav < 0).ToList();
            }
            return PartialView(list);
        }
        /// <summary>
        /// Вернуть номер вагона
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int GetCarriageNumberCar(int id)
        {
            SAPIncSupply sapis = this.ef_sap.SAPIncSupply.Where(s=>s.ID == id).FirstOrDefault();
            return sapis != null ? sapis.CarriageNumber : 0;
        }

        public int Correct(int id)
        {
            return id;
        }

    }
}