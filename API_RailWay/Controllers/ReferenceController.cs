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
    [RoutePrefix("api/reference")]
    public class ReferenceController : ApiController
    {
        IReference rep_ref;

        #region cargo
        public ReferenceController()
        {
            this.rep_ref = new EFReference();
        }

        // GET: api/reference/cargo
        [Route("cargo/{id:int?}")]
        public IEnumerable<ReferenceCargo> GetCargo()
        {
            return this.rep_ref.GetReferenceCargo();
        }
        // GET: api/reference/cargo/5
        [Route("cargo/{id:int?}")]
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

        // GET: api/reference/cargo/code/32203
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
