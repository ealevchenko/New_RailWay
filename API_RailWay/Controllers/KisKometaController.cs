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
    [RoutePrefix("api/kis/kometa")]
    public class KisKometaController : ApiController
    {
        protected IKIS rep_kis;
        public KisKometaController()
        {
            this.rep_kis = new EFWagons();  
        }

        // GET: api/kis/kometa/vagon_sob/num_vag/68823137
        [Route("vagon_sob/num_vag/{num:int}")]
        [ResponseType(typeof(KometaVagonSob))]
        public IHttpActionResult GetKometaVagonSob(int num)
        {
            List<KometaVagonSob> list = this.rep_kis.GetVagonsSob(num).ToList();
            if (list == null || list.Count() == 0)
            {
                return NotFound();
            }
            return Ok(list);
        }

        // GET: api/kis/kometa/vagon_sob/num_vag/68823137/2017/11/15/17/00/00
        [Route("vagon_sob/num_vag/{num:int}/{y:int}/{mt:int}/{d:int}/{h:int}/{m:int}/{s:int}")]
        [ResponseType(typeof(KometaVagonSob))]
        public IHttpActionResult GetKometaVagonSob(int num, int y, int mt, int d, int h, int m, int s)
        {
            DateTime dt = new DateTime(y, mt, d, h, m, s);
            KometaVagonSob vs = this.rep_kis.GetVagonsSob(num, dt);
            if (vs == null)
            {
                return NotFound();
            }
            return Ok(vs);
        }
        // GET: api/kis/kometa/vagon_sob/num_vag/68823137/2017-11-15 17:00:00
        // GET: api/kis/kometa/vagon_sob/num_vag/?num=68823137&dt=2017-11-15 17:00:00
        [Route("vagon_sob/num_vag/{num:int}/{dt:DateTime}")]
        [ResponseType(typeof(KometaVagonSob))]
        public IHttpActionResult GetKometaVagonSob(int num, DateTime dt)
        {
            //DateTime dt = DateTime.Parse(dts);
            KometaVagonSob vs = this.rep_kis.GetVagonsSob(num, dt);
            if (vs == null)
            {
                return NotFound();
            }
            return Ok(vs);
        }

        // GET: api/kis/kometa/sobstv_for_nakl
        [Route("sobstv_for_nakl")]
        [ResponseType(typeof(KometaSobstvForNakl))]
        public IHttpActionResult GetSobstvForNakl()
        {
            List<KometaSobstvForNakl> list = this.rep_kis.GetSobstvForNakl().ToList();
            if (list == null || list.Count() == 0)
            {
                return NotFound();
            }
            return Ok(list);
        }

        // GET: api/kis/kometa/sobstv_for_nakl/sob/10
        [Route("sobstv_for_nakl/sob/{kod:int}")]
        [ResponseType(typeof(KometaSobstvForNakl))]
        public IHttpActionResult GetSobstvForNakl(int kod)
        {
            KometaSobstvForNakl nakl = this.rep_kis.GetSobstvForNakl(kod);
            if (nakl == null)
            {
                return NotFound();
            }
            return Ok(nakl);
        }

    }
}
