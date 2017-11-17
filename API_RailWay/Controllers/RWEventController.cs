using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TransferRailCars;

namespace API_RailWay.Controllers
{
    [RoutePrefix("api/rw/event")]
    public class RWEventController : ApiController
    {
        public RWEventController() { 
        
        }

        // GET: api/rw/event/arrival/sostav/id/4113
        [Route("arrival/sostav/id/{id:int}")]
        public int GetArrivalSostav(int id)
        {
            TRailCars trc = new TRailCars();
            return trc.ArrivalToRailCars(id);
        }
    }
}
