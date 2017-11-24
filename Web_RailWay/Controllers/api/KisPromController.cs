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

namespace Web_RailWay.Controllers
{
    [RoutePrefix("api/kis/prom")]
    public class KisPromController : ApiController
    {
        protected IKIS rep_kis;
        public KisPromController()
        {
            this.rep_kis = new EFWagons();
        }

        // GET: api/kis/prom/gruz_sp
        [Route("gruz_sp")]
        [ResponseType(typeof(PromGruzSP))]
        public IHttpActionResult GetGruzSP()
        {
            List<PromGruzSP> list = this.rep_kis.GetGruzSP().ToList();
            if (list == null || list.Count() == 0)
            {
                return NotFound();
            }
            return Ok(list);
        }

        // GET: api/kis/prom/gruz_sp/kod_gr/307
        [Route("gruz_sp/kod_gr/{kod:int}")]
        [ResponseType(typeof(PromGruzSP))]
        public IHttpActionResult GetGruzSP(int kod)
        {
            PromGruzSP gsp = this.rep_kis.GetGruzSP(kod);
            if (gsp == null)
            {
                return NotFound();
            }
            return Ok(gsp);
        }

        // GET: api/kis/prom/gruz_sp/tar_gr/12502/true
        [Route("gruz_sp/tar_gr/{kod:int?}/{corect:bool}")]
        [ResponseType(typeof(PromGruzSP))]
        public IHttpActionResult GetGruzSPToTarGR(int? kod, bool corect)
        {
            PromGruzSP gsp = this.rep_kis.GetGruzSPToTarGR(kod, corect);
            if (gsp == null)
            {
                return NotFound();
            }
            return Ok(gsp);
        }
    }
}
