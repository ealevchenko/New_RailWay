using EFKIS.Abstract;
using EFKIS.Concrete;
using EFKIS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace API_RailWay.Controllers
{
    [RoutePrefix("api/kis/num_vag")]
    public class KisNumVagController : ApiController
    {
        protected IKIS rep_kis;
        public KisNumVagController()
        {
            this.rep_kis = new EFWagons();
        }

        // GET: api/kis/num_vag/stpr1gr
        [Route("stpr1gr")]
        [ResponseType(typeof(NumVagStpr1Gr))]
        public IHttpActionResult GetSTPR1GR()
        {
            List<NumVagStpr1Gr> list = this.rep_kis.GetSTPR1GR().ToList();
            if (list == null || list.Count() == 0)
            {
                return NotFound();
            }
            return Ok(list);
        }

        // GET: api/kis/num_vag/stpr1gr/kod/291
        [Route("stpr1gr/kod/{kod:int}")]
        [ResponseType(typeof(NumVagStpr1Gr))]
        public IHttpActionResult GetSTPR1GR(int kod)
        {
            NumVagStpr1Gr gsp = this.rep_kis.GetSTPR1GR(kod);
            if (gsp == null)
            {
                return NotFound();
            }
            return Ok(gsp);
        }
    }
}
