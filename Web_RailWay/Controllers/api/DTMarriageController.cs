using EFTD.Abstract;
using EFTD.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Web_RailWay.Controllers.api
{
    [RoutePrefix("api/dt")]
    public class DTMarriageController : ApiController
    {

        protected IRepository<MarriageWork> ef_mw;
        protected IRepository<MarriagePlace> ef_mp;
        protected IRepository<MarriageClassification> ef_mc;
        protected IRepository<MarriageNature> ef_mn;
        protected IRepository<MarriageCause> ef_mcs;
        protected IRepository<MarriageSubdivision> ef_ms;

        public DTMarriageController(IRepository<MarriageWork> mw, IRepository<MarriagePlace> mp, IRepository<MarriageClassification> mc, IRepository<MarriageNature> mn, IRepository<MarriageCause> mcs, IRepository<MarriageSubdivision> ms)
        {
            this.ef_mw = mw;
            this.ef_mp = mp;
            this.ef_mc = mc;
            this.ef_mn = mn;
            this.ef_mcs = mcs;
            this.ef_ms = ms;
        }

        #region MarriageWork
        // GET: api/dt/marriage_work
        [Route("marriage_work")]
        [ResponseType(typeof(MarriageWork))]
        public IHttpActionResult GetMarriageWork()
        {
            try
            {
                List<MarriageWork> list = this.ef_mw.Get().ToList()
                    .Select(c => new MarriageWork
                    {
                        id = c.id,
                        date_start = c.date_start,
                        date_stop = c.date_stop,
                        id_place = c.id_place,
                        site = c.site,
                        id_classification = c.id_classification,
                        id_nature = c.id_nature,
                        num = c.num,
                        id_cause = c.id_cause,
                        id_type_cause = c.id_type_cause,
                        id_subdivision = c.id_subdivision,
                        akt = c.akt,
                        locomotive_series = c.locomotive_series,
                        driver = c.driver,
                        helper = c.helper,
                        measures = c.measures,
                        note = c.note,
                        create = c.create,
                        create_user = c.create_user,
                        change = c.change,
                        change_user = c.change_user,
                        MarriageCause = new MarriageCause
                        {
                            id = c.MarriageCause.id,
                            cause = c.MarriageCause.cause,
                        },
                        MarriageClassification = new MarriageClassification
                        {
                            id = c.MarriageClassification.id,
                            classification = c.MarriageClassification.classification,
                        },
                        MarriageNature = c.MarriageNature != null ? new MarriageNature
                        {
                            id = c.MarriageNature.id,
                            nature = c.MarriageNature.nature,
                        } : null,
                        MarriagePlace = new MarriagePlace
                        {
                            id = c.MarriagePlace.id,
                            place = c.MarriagePlace.place,
                        },
                        MarriageSubdivision = new MarriageSubdivision
                        {
                            id = c.MarriageSubdivision.id,
                            subdivision = c.MarriageSubdivision.subdivision,
                        },

                    }).ToList();
                if (list == null || list.Count() == 0)
                {
                    return NotFound();
                }
                return Ok(list);
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }

        // GET: api/dt/marriage_work/start/2019-04-02T00:00:00/stop/2019-04-02T23:59:59
        [Route("marriage_work/start/{start:datetime}/stop/{stop:datetime}")]
        [ResponseType(typeof(MarriageWork))]
        public IHttpActionResult GetMarriageWork(DateTime start, DateTime stop)
        {
            try
            {
                List<MarriageWork> list = this.ef_mw
                    .Get()
                    .Where(d=>d.date_start >=start && d.date_start <= stop)
                    .ToList()
                    .Select(c => new MarriageWork
                    {
                        id = c.id,
                        date_start = c.date_start,
                        date_stop = c.date_stop,
                        id_place = c.id_place,
                        site = c.site,
                        id_classification = c.id_classification,
                        id_nature = c.id_nature,
                        num = c.num,
                        id_cause = c.id_cause,
                        id_type_cause = c.id_type_cause,
                        id_subdivision = c.id_subdivision,
                        akt = c.akt,
                        locomotive_series = c.locomotive_series,
                        driver = c.driver,
                        helper = c.helper,
                        measures = c.measures,
                        note = c.note,
                        create = c.create,
                        create_user = c.create_user,
                        change = c.change,
                        change_user = c.change_user,
                        MarriageCause = new MarriageCause
                        {
                            id = c.MarriageCause.id,
                            cause = c.MarriageCause.cause,
                        },
                        MarriageClassification = new MarriageClassification
                        {
                            id = c.MarriageClassification.id,
                            classification = c.MarriageClassification.classification,
                        },
                        MarriageNature = c.MarriageNature != null ? new MarriageNature
                        {
                            id = c.MarriageNature.id,
                            nature = c.MarriageNature.nature,
                        } : null,
                        MarriagePlace = new MarriagePlace
                        {
                            id = c.MarriagePlace.id,
                            place = c.MarriagePlace.place,
                        },
                        MarriageSubdivision = new MarriageSubdivision
                        {
                            id = c.MarriageSubdivision.id,
                            subdivision = c.MarriageSubdivision.subdivision,
                        },

                    }).ToList();
                if (list == null || list.Count() == 0)
                {
                    return NotFound();
                }
                return Ok(list);
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }

        // GET: api/dt/marriage_work/id/66
        [Route("marriage_work/id/{id:int}")]
        [ResponseType(typeof(MarriageWork))]
        public IHttpActionResult GetMarriageWork(int id)
        {
            try
            {
               MarriageWork mw = this.ef_mw
                    .Get()
                    .Where( c=> c.id == id)
                    .Select(c => new MarriageWork
                    {
                        id = c.id,
                        date_start = c.date_start,
                        date_stop = c.date_stop,
                        id_place = c.id_place,
                        site = c.site,
                        id_classification = c.id_classification,
                        id_nature = c.id_nature,
                        num = c.num,
                        id_cause = c.id_cause,
                        id_type_cause = c.id_type_cause,
                        id_subdivision = c.id_subdivision,
                        akt = c.akt,
                        locomotive_series = c.locomotive_series,
                        driver = c.driver,
                        helper = c.helper,
                        measures = c.measures,
                        note = c.note,
                        create = c.create,
                        create_user = c.create_user,
                        change = c.change,
                        change_user = c.change_user,
                        MarriageCause = new MarriageCause
                        {
                            id = c.MarriageCause.id,
                            cause = c.MarriageCause.cause,
                        },
                        MarriageClassification = new MarriageClassification
                        {
                            id = c.MarriageClassification.id,
                            classification = c.MarriageClassification.classification,
                        },
                        MarriageNature = c.MarriageNature != null ? new MarriageNature
                        {
                            id = c.MarriageNature.id,
                            nature = c.MarriageNature.nature,
                        } : null,
                        MarriagePlace = new MarriagePlace
                        {
                            id = c.MarriagePlace.id,
                            place = c.MarriagePlace.place,
                        },
                        MarriageSubdivision = new MarriageSubdivision
                        {
                            id = c.MarriageSubdivision.id,
                            subdivision = c.MarriageSubdivision.subdivision,
                        },

                    }).FirstOrDefault();
                if (mw == null)
                {
                    return NotFound();
                }
                return Ok(mw);
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }
        #endregion

        #region MarriagePlace
        // GET: api/dt/marriage_place
        [Route("marriage_place")]
        [ResponseType(typeof(MarriagePlace))]
        public IHttpActionResult GetMarriagePlace()
        {
            try
            {
                List<MarriagePlace> list = this.ef_mp.Get().ToList()
                    .Select(c => new MarriagePlace
                    {
                        id = c.id,
                        place = c.place,
                        MarriageWork = null

                    }).ToList();
                if (list == null || list.Count() == 0)
                {
                    return NotFound();
                }
                return Ok(list);
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }
        #endregion

        #region MarriageClassification
        // GET: api/dt/marriage_classification
        [Route("marriage_classification")]
        [ResponseType(typeof(MarriageClassification))]
        public IHttpActionResult GetMarriageClassification()
        {
            try
            {
                List<MarriageClassification> list = this.ef_mc.Get().ToList()
                    .Select(c => new MarriageClassification
                    {
                        id = c.id,
                        classification = c.classification,
                        MarriageWork = null

                    }).ToList();
                if (list == null || list.Count() == 0)
                {
                    return NotFound();
                }
                return Ok(list);
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }
        #endregion

        #region MarriageNature
        // GET: api/dt/marriage_nature
        [Route("marriage_nature")]
        [ResponseType(typeof(MarriageNature))]
        public IHttpActionResult GetMarriageNature()
        {
            try
            {
                List<MarriageNature> list = this.ef_mn.Get().ToList()
                    .Select(c => new MarriageNature
                    {
                        id = c.id,
                        nature = c.nature,
                        MarriageWork = null

                    }).ToList();
                if (list == null || list.Count() == 0)
                {
                    return NotFound();
                }
                return Ok(list);
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }
        #endregion

        #region MarriageCause
        // GET: api/dt/marriage_cause
        [Route("marriage_cause")]
        [ResponseType(typeof(MarriageCause))]
        public IHttpActionResult GetMarriageCause()
        {
            try
            {
                List<MarriageCause> list = this.ef_mcs.Get().ToList()
                    .Select(c => new MarriageCause
                    {
                        id = c.id,
                        cause = c.cause,
                        MarriageWork = null

                    }).ToList();
                if (list == null || list.Count() == 0)
                {
                    return NotFound();
                }
                return Ok(list);
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }
        #endregion

        #region MarriageSubdivision
        // GET: api/dt/marriage_subdivision
        [Route("marriage_subdivision")]
        [ResponseType(typeof(MarriageSubdivision))]
        public IHttpActionResult GetMarriageSubdivision()
        {
            try
            {
                List<MarriageSubdivision> list = this.ef_ms.Get().ToList()
                    .Select(c => new MarriageSubdivision
                    {
                        id = c.id,
                        subdivision = c.subdivision,
                        MarriageWork = null

                    }).ToList();
                if (list == null || list.Count() == 0)
                {
                    return NotFound();
                }
                return Ok(list);
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }
        #endregion
    }
}
