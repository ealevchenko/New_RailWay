using EFKIS.Abstract;
using EFKIS.Concrete;
using EFKIS.Entities;
using MessageLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Web_RailWay.Controllers.api
{
    [RoutePrefix("api/kis/num_vag")]
    public class KisNumVagController : ApiController
    {
        private eventID eventID = eventID.Web_API_KisNumVagController;

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

        // GET: api/kis/num_vag/station/name
        [Route("station/name")]
        [ResponseType(typeof(Option))]
        public IHttpActionResult GetNumVagStations()
        {
            List<Option> list = new List<Option>();
            try
            {
                this.rep_kis.GetNumVagStations().ToList().ForEach(s => list.Add(new Option() { value = s.K_STAN, text = s.NAME }));
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetNumVagStations()"), eventID);
                return NotFound();
            }
            if (list == null || list.Count() == 0)
            {
                return NotFound();
            }
            return Ok(list);
        }

        // GET: api/kis/num_vag/station/4/name
        [Route("station/{id:int}/name")]
        [ResponseType(typeof(string))]
        public string GetNumVagStations(int id)
        {
            NumVagStan kstan = this.rep_kis.GetNumVagStations(id);
            return kstan != null ? kstan.NAME : id.ToString();
        }

    }
}
