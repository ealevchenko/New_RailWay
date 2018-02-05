using EFKIS.Abstract;
using EFKIS.Concrete;
using EFKIS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_RailWay.Infrastructure;

namespace Web_RailWay.Areas.KIS.Controllers
{
    public class TransferController : Controller
    {
        ITKIS ef_kis;

        public TransferController(ITKIS kis)
        {
            this.ef_kis = kis;
        }

        // GET: KIST
        [Access(LogVisit = true)]
        public ActionResult Index()
        {
            return View();
        }
    }
}