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
    [RoutePrefix("api/kis/kometa")]
    public class KisKometaController : ApiController
    {
        private eventID eventID = eventID.Web_API_KisKometaController;

        protected IKIS rep_kis;
        public KisKometaController()
        {
            this.rep_kis = new EFWagons();  
        }

        #region KometaVagonSob
        // GET: api/kis/kometa/vagon_sob/num_vag/68823137
        [Route("vagon_sob/num_vag/{num:int}")]
        [ResponseType(typeof(KometaVagonSob))]
        public IHttpActionResult GetKometaVagonSob(int num)
        {
            try
            {
                List<KometaVagonSob> list = this.rep_kis.GetVagonsSob(num).ToList();
                if (list == null || list.Count() == 0)
                {
                    return NotFound();
                }
                return Ok(list);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetKometaVagonSob(num={0})", num), eventID);
                return NotFound();
            }
}

        // GET: api/kis/kometa/vagon_sob/num_vag/68823137/2017/11/15/17/00/00
        [Route("vagon_sob/num_vag/{num:int}/{y:int}/{mt:int}/{d:int}/{h:int}/{m:int}/{s:int}")]
        [ResponseType(typeof(KometaVagonSob))]
        public IHttpActionResult GetKometaVagonSob(int num, int y, int mt, int d, int h, int m, int s)
        {
            try
            {
                DateTime dt = new DateTime(y, mt, d, h, m, s);
                KometaVagonSob vs = this.rep_kis.GetVagonsSob(num, dt);
                if (vs == null)
                {
                    return NotFound();
                }
                return Ok(vs);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetKometaVagonSob(num={0},y={1},mt={2},d={3},h={4},m={5},s={6})", num, y ,mt, d, h, m, s), eventID);
                return NotFound();
            }
}
        // GET: api/kis/kometa/vagon_sob/num_vag/68823137/2017-11-15 17:00:00
        // GET: api/kis/kometa/vagon_sob/num_vag/?num=68823137&dt=2017-11-15 17:00:00
        [Route("vagon_sob/num_vag/{num:int}/{dt:DateTime}")]
        [ResponseType(typeof(KometaVagonSob))]
        public IHttpActionResult GetKometaVagonSob(int num, DateTime dt)
        {
            try
            {
                //DateTime dt = DateTime.Parse(dts);
                KometaVagonSob vs = this.rep_kis.GetVagonsSob(num, dt);
                if (vs == null)
                {
                    return NotFound();
                }
                return Ok(vs);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetKometaVagonSob(num={0}, dt={1})", num, dt), eventID);
                return NotFound();
            }
}
        #endregion

        #region KometaSobstvForNakl
        // GET: api/kis/kometa/sobstv_for_nakl
        [Route("sobstv_for_nakl")]
        [ResponseType(typeof(KometaSobstvForNakl))]
        public IHttpActionResult GetSobstvForNakl()
        {
            try
            {
                List<KometaSobstvForNakl> list = this.rep_kis.GetSobstvForNakl().ToList();
                if (list == null || list.Count() == 0)
                {
                    return NotFound();
                }
                return Ok(list);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetSobstvForNakl()"), eventID);
                return NotFound();
            }
}
        
        // GET: api/kis/kometa/sobstv_for_nakl/sob/10
        [Route("sobstv_for_nakl/sob/{kod:int}")]
        [ResponseType(typeof(KometaSobstvForNakl))]
        public IHttpActionResult GetSobstvForNakl(int kod)
        {
            try
            {
                KometaSobstvForNakl nakl = this.rep_kis.GetSobstvForNakl(kod);
                if (nakl == null)
                {
                    return NotFound();
                }
                return Ok(nakl);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetSobstvForNakl(kod={0})",kod), eventID);
                return NotFound();
            }
}
        #endregion

        #region KometaStan
        // GET: api/kis/kometa/station/name
        [Route("station/name")]
        [ResponseType(typeof(Option))]
        public IHttpActionResult GetKometaStan()
        {
            List<Option> list = new List<Option>();
            try
            {
                this.rep_kis.GetKometaStan().ToList().ForEach(s => list.Add(new Option() { value = s.K_STAN, text = s.NAME }));
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetKometaStan()"), eventID);
                return NotFound();
            }
            if (list == null || list.Count()==0)
            {
                return NotFound();
            }
            return Ok(list);
        }

        // GET: api/kis/kometa/station/10/name
        [Route("station/{id:int}/name")]
        [ResponseType(typeof(string))]
        public string GetKometaStan(int id)
        {
            try
            {
                KometaStan kstan = this.rep_kis.GetKometaStan(id);
                return kstan != null ? kstan.NAME : id.ToString();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetKometaStan(id={0})", id), eventID);
                return null;
            }
}
        #endregion

        #region KometaStrana
        // GET: api/kis/kometa/strana
        [Route("strana")]
        [ResponseType(typeof(KometaStrana))]
        public IHttpActionResult GetKometaStrana()
        {
            try
            {
                List<KometaStrana> list = this.rep_kis.GetKometaStrana().ToList();
                if (list == null || list.Count() == 0)
                {
                    return NotFound();
                }
                return Ok(list);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetKometaStrana()"), eventID);
                return NotFound();
            }
}
        #endregion

        #region

        #endregion
    }
}
