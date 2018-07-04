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

namespace Web_RailWay.Controllers.api
{
    [RoutePrefix("api/kis/prom")]
    public class KisPromController : ApiController
    {
        protected IKIS rep_kis;
        public KisPromController()
        {
            this.rep_kis = new EFWagons();
        }

        #region PROM.GRUZ_SP
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
        #endregion

        #region PROM.SOSTAV
        //// GET: api/kis/prom/sostav/start/2018-06-20T00:00:00/stop/2018-06-20T23:00:00
        //[Route("sostav/start/{start:datetime}/stop/{stop:datetime}")]
        //[ResponseType(typeof(Prom_Sostav))]
        //public IHttpActionResult GetPromSostav(DateTime start, DateTime stop)
        //{
        //    List<Prom_Sostav> ls_all = this.rep_kis.GetProm_Sostav().Where(s => s.D_PR_YY >= start.Year & s.D_MM >= start.Month).ToList();
        //    List<Prom_Sostav> list = ls_all.Where(s => s.DT_PR >= start & s.DT_PR <= stop).ToList();
        //    if (list == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(list);
        //}
        // GET: api/kis/prom/sostav/start/2018-06-20T00:00:00/stop/2018-06-20T23:00:00
        [Route("sostav/start/{start:datetime}/stop/{stop:datetime}")]
        [ResponseType(typeof(Prom_SostavAndCount))]
        public IHttpActionResult GetPromSostav(DateTime start, DateTime stop)
        {
            //List<Prom_Sostav> ls_all = this.rep_kis.GetProm_Sostav().Where(s => s.D_PR_YY >= start.Year & s.D_MM >= start.Month).ToList();
            List<Prom_SostavAndCount> list = this.rep_kis.GetProm_SostavAndCount(start, stop).ToList();
            if (list == null)
            {
                return NotFound();
            }
            return Ok(list);
        }

        // GET: api/kis/prom/sostav/natur/4201/day/3/month/7/year/2018/hour/14/minute/35
        [Route("sostav/natur/{natur:int}/day/{day:int}/month/{month:int}/year/{year:int}/hour/{hour:int}/minute/{minute:int}")]
        [ResponseType(typeof(Prom_SostavAndCount))]
        public IHttpActionResult GetPromSostav(int natur, int day, int month, int year, int hour, int minute)
        {
            List<Prom_SostavAndCount> list = this.rep_kis.GetProm_SostavAndCount(
                natur != -1 ? (int?)natur : null,
                day != -1 ? (int?)day : null,
                month != -1 ? (int?)month : null,
                year != -1 ? (int?)year : null,
                hour != -1 ? (int?)hour : null,
                minute != -1 ? (int?)minute : null).ToList();
            if (list == null)
            {
                return NotFound();
            }
            return Ok(list);
        }        
        #endregion



        #region PROM.Vagon
        // GET: api/kis/prom/vagon/arrival/natur/4023/day/27/month/6/year/2018/hour/4/minute/50
        [Route("vagon/arrival/natur/{natur:int}/day/{day:int}/month/{month:int}/year/{year:int}/hour/{hour:int}/minute/{minute:int}")]
        [ResponseType(typeof(Prom_Vagon))]
        public IHttpActionResult GetPRPromVagon(int natur, int day, int month, int year, int hour, int minute)
        {
            List<Prom_Vagon> list = this.rep_kis.GetPRProm_Vagon(natur, day, month, year, hour, minute).ToList();
            if (list == null)
            {
                return NotFound();
            }
            return Ok(list);
        }

        // GET: api/kis/prom/vagon/sending/natur/4024/day/27/month/6/year/2018/hour/5/minute/30
        [Route("vagon/sending/natur/{natur:int}/day/{day:int}/month/{month:int}/year/{year:int}/hour/{hour:int}/minute/{minute:int}")]
        [ResponseType(typeof(Prom_Vagon))]
        public IHttpActionResult GetSDPromVagon(int natur, int day, int month, int year, int hour, int minute)
        {
            List<Prom_Vagon> list = this.rep_kis.GetSDProm_Vagon(natur, day, month, year, hour, minute).ToList();
            if (list == null)
            {
                return NotFound();
            }
            return Ok(list);
        }        
        #endregion

        #region PROM.Nat_Hist
        // GET: api/kis/prom/nat_hist/arrival/natur/4023/day/27/month/6/year/2018/hour/4/minute/50
        [Route("nat_hist/arrival/natur/{natur:int}/day/{day:int}/month/{month:int}/year/{year:int}/hour/{hour:int}/minute/{minute:int}")]
        [ResponseType(typeof(Prom_NatHist))]
        public IHttpActionResult GetPRPromNatHist(int natur, int day, int month, int year, int hour, int minute)
        {
            List<Prom_NatHist> list = this.rep_kis.GetPRProm_NatHist(natur, day, month, year, hour, minute).ToList();
            if (list == null)
            {
                return NotFound();
            }
            return Ok(list);
        }

        // GET: api/kis/prom/nat_hist/sending/natur/4024/day/27/month/6/year/2018/hour/5/minute/30
        [Route("nat_hist/sending/natur/{natur:int}/day/{day:int}/month/{month:int}/year/{year:int}/hour/{hour:int}/minute/{minute:int}")]
        [ResponseType(typeof(Prom_NatHist))]
        public IHttpActionResult GetSDPromNatHist(int natur, int day, int month, int year, int hour, int minute)
        {
            List<Prom_NatHist> list = this.rep_kis.GetSDProm_NatHist(natur, day, month, year, hour, minute).ToList();
            if (list == null)
            {
                return NotFound();
            }
            return Ok(list);
        }        
        #endregion

        #region PROM.CEX
        // GET: api/kis/prom/cex
        [Route("cex")]
        [ResponseType(typeof(PromCex))]
        public IHttpActionResult GetPromCex()
        {
            List<PromCex> list = this.rep_kis.GetCex().ToList();
            if (list == null)
            {
                return NotFound();
            }
            return Ok(list);
        }
        #endregion
    }
}
