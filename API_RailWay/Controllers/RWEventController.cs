using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API_RailWay.Controllers
{
    [RoutePrefix("api/rw/event")]
    public class RWEventController : ApiController
    {
        public RWEventController() { 
        
        }

        // GET: api/rw/event/arrival/sostav/id/5
        [Route("arrival/sostav/id/{id:int}")]
        public int GetArrivalSostav(int id)
        {
            //Cargo cargo = this.rep_ref.GetCargo(id);
            //if (cargo == null)
            //{
            //    return NotFound();
            //}

            //return Ok(cargo);
            return 1;
        }
    }
}
