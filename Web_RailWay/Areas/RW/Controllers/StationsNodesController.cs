using EFRW.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_RailWay.Infrastructure;

namespace Web_RailWay.Areas.RW.Controllers
{
    public class StationsNodesController : Controller
    {
        IRailWay ef_rw;

        public StationsNodesController(IRailWay rw)
        {
                    this.ef_rw = rw;
        }
        
        // GET: RWStationsNodes
        [Access(LogVisit = true)]
        public ActionResult Index()
        {
            return View();
        }
    }
}