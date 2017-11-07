using EFMT.Abstract;
using EFMT.Concrete;
using MT.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace API_RailWay.Controllers
{
    public class ApproachesCarsController : ApiController
    {
        protected IMT rep_MT;

        public ApproachesCarsController()
        {
            this.rep_MT = new EFMetallurgTrans();
        }
        
        // GET: api/ApproachesCars
        public IEnumerable<ApproachesCars> Get()
        {
            return this.rep_MT.GetApproachesCars().ToArray();
        }

        // GET: api/ApproachesCars/5
        [ResponseType(typeof(ApproachesCars))]
        public IHttpActionResult Get(int id)
        {
            ApproachesCars cars = this.rep_MT.GetApproachesCars(id);
            if (cars == null)
            {
                return NotFound();
            }
            cars.ApproachesSostav = null;
            return Ok(cars);
        }


        // POST: api/ApproachesCars
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/ApproachesCars/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ApproachesCars/5
        public void Delete(int id)
        {
        }
    }
}
