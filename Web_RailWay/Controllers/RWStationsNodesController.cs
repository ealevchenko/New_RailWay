using EFRW.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web_RailWay.Controllers
{
    public class RWStationsNodesController : Controller
    {
        IRailWay ef_rw;

        public RWStationsNodesController(IRailWay rw)
        {
                    this.ef_rw = rw;
        }
        
        // GET: RWStationsNodes
        public ActionResult Index()
        {
            return View();
        }
    }
}