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

namespace API_RailWay.Controllers
{
    public class ReferenceCargoController : ApiController
    {
        IReference rep_ref;

        public ReferenceCargoController()
        {
            this.rep_ref = new EFReference();
        }
        // GET: api/ReferenceCargo
        public IEnumerable<ReferenceCargo> Get()
        {
            return this.rep_ref.GetReferenceCargo();
        }

        // GET: api/ReferenceCargo/5
        [ResponseType(typeof(ReferenceCargo))]
        public IHttpActionResult Get(int id)
        {
            ReferenceCargo cargo = this.rep_ref.GetReferenceCargo(id);
            if (cargo == null)
            {
                return NotFound();
            }

            return Ok(cargo);
        }

        // POST: api/ReferenceCargo
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/ReferenceCargo/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ReferenceCargo/5
        public void Delete(int id)
        {
        }
    }
}
