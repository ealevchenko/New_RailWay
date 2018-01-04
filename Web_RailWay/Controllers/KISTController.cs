using EFKIS.Abstract;
using EFKIS.Concrete;
using EFKIS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_RailWay.Infrastructure;

namespace Web_RailWay.Controllers
{
    public class KISTController : Controller
    {
        ITKIS ef_kis;

        public KISTController(ITKIS kis)
        {
            this.ef_kis = kis;
        }

        // GET: KIST
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Показать процесс переноса вагонов по прибытию
        /// </summary>
        /// <returns></returns>
        [Access(LogVisit = true)]
        public ActionResult TransferArrival()
        {
            return View();
        }

        public PartialViewResult ListBufferArrivalSostavOfDate(DateTime date_start, DateTime date_stop)
        {
            List<BufferArrivalSostav> list = new List<BufferArrivalSostav>();
            list = this.ef_kis.BufferArrivalSostav
                .Where(b => b.datetime >= date_start & b.datetime <= date_stop)
                .OrderByDescending(x => x.datetime)
                .ToList();
            return PartialView(list);
        }

        public PartialViewResult ListBufferArrivalSostav(List<BufferArrivalSostav> list)
        {
            return PartialView(list);
        }

        public PartialViewResult BufferArrivalSostav(BufferArrivalSostav bas)
        {
            return PartialView(bas);
        }

        public PartialViewResult ListCarsBufferArrivalSostav(int id)
        {
            BufferArrivalSostav bas = this.ef_kis.GetBufferArrivalSostav(id);
            return PartialView(bas);
        }
        /// <summary>
        /// Показать кнопку
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [View]
        public PartialViewResult ButtonCloseBufferArrivalSostav(int id)
        {
            return PartialView(id);
        }
        /// <summary>
        /// Закрыть строку состава
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PartialViewResult CloseBufferArrivalSostav(int id)
        {
            this.ef_kis.CloseBufferArrivalSostav(id);
            BufferArrivalSostav bas = this.ef_kis.GetBufferArrivalSostav(id);
            return PartialView(bas);
        }


    }
}