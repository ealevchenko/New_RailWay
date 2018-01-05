using EFRW.Abstract;
using EFRW.Concrete;
using EFRW.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Web_RailWay.Controllers.api
{
    [RoutePrefix("api/rw")]
    public class RWController : ApiController
    {
        protected IRailWay rep_rw;
        public RWController()
        {
            this.rep_rw = new EFRailWay();  
        }

        // GET: api/rw/stations/view/amkr
        [Route("stations/view/amkr")]
        [ResponseType(typeof(Stations))]
        public IHttpActionResult GetStationsOfViewAMKR()
        {
            List<Stations> stations = this.rep_rw.GetStationsOfViewAMKR().ToList();
            if (stations == null)
            {
                return NotFound();
            }
            return Ok(stations);
        }
    }
}
