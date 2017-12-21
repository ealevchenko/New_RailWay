using EFKIS.Abstract;
using EFKIS.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
        public ActionResult TransferArrival()
        {
            return View();
        }
    }
}