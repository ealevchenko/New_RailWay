using EFRW.Abstract;
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
    [RoutePrefix("api/rw/reference")]
    public class RWReferenceController : ApiController
    {
        IReference rep_ref;

        #region cargo
        public RWReferenceController()
        {
            this.rep_ref = new EFRW.Concrete.EFReference();
        }

        // GET: api/rw/reference/cargo
        [Route("cargo/{id:int?}")]
        public IEnumerable<ReferenceCargo> GetCargo()
        {
            return this.rep_ref.GetReferenceCargo();
        }
        // GET: api/rw/reference/cargo/id/5
        [Route("cargo/id/{id:int?}")]
        [ResponseType(typeof(ReferenceCargo))]
        public IHttpActionResult GetCargo(int id)
        {
            ReferenceCargo cargo = this.rep_ref.GetReferenceCargo(id);
            if (cargo == null)
            {
                return NotFound();
            }

            return Ok(cargo);
        }

        // GET: api/rw/reference/cargo/code/32203
        [Route("cargo/code/{id:int}")]
        [ResponseType(typeof(ReferenceCargo))]
        public IHttpActionResult GetCargoOfCode(int id)
        {
            ReferenceCargo cargo = this.rep_ref.GetCorectReferenceCargo(id);
            if (cargo == null)
            {
                return NotFound();
            }

            return Ok(cargo);
        }
        #endregion

    }
}
