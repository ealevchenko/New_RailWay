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
    public class ReportNC {
        public string name { get; set; }        
        public int count { get; set; }
    }

    public class DinamicClassification
    {
        public string month { get; set; }
        public int classification1 { get; set; }
        public int classification2 { get; set; }
        public int classification3 { get; set; }
        public int classification4 { get; set; }
        public int classification5 { get; set; }
        public int classification6 { get; set; }
        public int classification7 { get; set; }
        public int classification8 { get; set; }
        public int classification9 { get; set; }
        public int classification10 { get; set; }
    }
    
    [RoutePrefix("api/dt")]
    public class DTMarriageController : ApiController
    {

        protected IRepository<MarriageWork> ef_mw;
        protected IRepository<MarriageDistrict> ef_md;
        protected IRepository<MarriageDistrictObject> ef_mdo;
        protected IRepository<MarriageClassification> ef_mc;
        protected IRepository<MarriageNature> ef_mn;
        protected IRepository<MarriageCause> ef_mcs;
        protected IRepository<MarriageSubdivision> ef_ms;

        public DTMarriageController(IRepository<MarriageWork> mw, IRepository<MarriageDistrict> md, IRepository<MarriageDistrictObject> mdo, IRepository<MarriageClassification> mc, IRepository<MarriageNature> mn, IRepository<MarriageCause> mcs, IRepository<MarriageSubdivision> ms)
        {
            this.ef_mw = mw;
            this.ef_md = md;
            this.ef_mdo = mdo;
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
                        id_district_object = c.id_district_object,
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
                        MarriageDistrictObject = new MarriageDistrictObject
                        {
                            id = c.MarriageDistrictObject.id, 
                            id_district = c.MarriageDistrictObject.id_district, 
                            district_object = c.MarriageDistrictObject.district_object,
                            type_object = c.MarriageDistrictObject.type_object,
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
                        id_district_object = c.id_district_object,
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
                        MarriageDistrictObject = new MarriageDistrictObject
                        {
                            id = c.MarriageDistrictObject.id,
                            id_district = c.MarriageDistrictObject.id_district,
                            district_object = c.MarriageDistrictObject.district_object,
                            type_object = c.MarriageDistrictObject.type_object,
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
                        id_district_object = c.id_district_object,
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
                        MarriageDistrictObject = new MarriageDistrictObject
                        {
                            id = c.MarriageDistrictObject.id,
                            id_district = c.MarriageDistrictObject.id_district,
                            district_object = c.MarriageDistrictObject.district_object,
                            type_object = c.MarriageDistrictObject.type_object,
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

        // POST api/dt/marriage_work
        [HttpPost]
        [Route("marriage_work")]
        public int PostMarriageWork([FromBody]MarriageWork value)
        {
            try
            {
                this.ef_mw.Add(value);
                this.ef_mw.Save();
                return value.id;
            }
            catch (Exception e)
            {
                return -1;
            }
        }

        // PUT api/dt/marriage_work/66
        [HttpPut]
        [Route("marriage_work/{id:int}")]
        public int PutMarriageWork(int id, [FromBody]MarriageWork value)
        {
            try
            {
                this.ef_mw.Update(value);
                return this.ef_mw.Save();
            }
            catch (Exception e)
            {
                return -1;
            }
        }

        // PUT api/dt/marriage_work/66
        [HttpDelete]
        [Route("marriage_work/{id:int}")]
        public int DeleteMarriageWork(int id)
        {
            try
            {
                this.ef_mw.Delete(id);
                return this.ef_mw.Save();
            }
            catch (Exception e)
            {
                return -1;
            }
        }

        #endregion

        #region MarriageDistrictObject
        // GET: api/dt/marriage_district_object
        [Route("marriage_district_object")]
        [ResponseType(typeof(MarriageDistrictObject))]
        public IHttpActionResult GetMarriageDistrictObject()
        {
            try
            {
                List<MarriageDistrictObject> list = this.ef_mdo.Get().ToList()
                    .Select(c => new MarriageDistrictObject
                    {
                        id = c.id,
                        id_district = c.id_district,
                        district_object = c.district_object,
                        type_object = c.type_object,
                        MarriageDistrict = new MarriageDistrict
                        {
                            id = c.MarriageDistrict.id,
                            district = c.MarriageDistrict.district
                        }
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
                List<MarriageSubdivision> list = this.ef_ms.Get().ToList().OrderBy(s=>s.subdivision)
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

        #region Report
        // GET: api/dt/marriage_work/cause_count/start/2019-04-15T00:00:00/stop/2019-04-16T06:59:59
        [Route("marriage_work/cause_count/start/{start:datetime}/stop/{stop:datetime}")]
        [ResponseType(typeof(ReportNC))]
        public IHttpActionResult GetReportCauseCount(DateTime start, DateTime stop)
        {

            try
            {
                string sql = "SELECT c.cause as name, COUNT(mw.id) AS count FROM TD.MarriageWork as mw INNER JOIN  TD.MarriageCause as c ON mw.id_cause = c.id " +
                "where mw.date_start >= CONVERT(datetime,'" + start.ToString("yyyy-MM-dd HH:mm:ss") + "',120) and  mw.date_start <= CONVERT(datetime,'" + stop.ToString("yyyy-MM-dd HH:mm:ss") + "',120) " +
                "GROUP BY c.cause order by c.cause";
                List<ReportNC> list = this.ef_mw.Database.SqlQuery<ReportNC>(sql).ToList();
                if (list == null)
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

        // GET: api/dt/marriage_work/district_count/start/2019-04-15T00:00:00/stop/2019-04-16T06:59:59
        [Route("marriage_work/district_count/start/{start:datetime}/stop/{stop:datetime}")]
        [ResponseType(typeof(ReportNC))]
        public IHttpActionResult GetReportDistrictCount(DateTime start, DateTime stop)
        {

            try
            {
                string sql = "SELECT d.district as name, COUNT(mw.id) AS count FROM TD.MarriageWork as mw INNER JOIN TD.MarriageDistrictObject as do ON mw.id_district_object = do.id INNER JOIN TD.MarriageDistrict as d ON do.id_district = d.id " +
                "where mw.date_start >= CONVERT(datetime,'" + start.ToString("yyyy-MM-dd HH:mm:ss") + "',120) and  mw.date_start <= CONVERT(datetime,'" + stop.ToString("yyyy-MM-dd HH:mm:ss") + "',120) " +
                "GROUP BY d.district order by d.district";
                List<ReportNC> list = this.ef_mw.Database.SqlQuery<ReportNC>(sql).ToList();
                if (list == null)
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

        // GET: api/dt/marriage_work/dinamic_count/classification/10/start/2019-01-01T00:00:00/stop/2019-06-30T23:59:59
        [Route("marriage_work/dinamic_count/classification/{id:int}/start/{start:datetime}/stop/{stop:datetime}")]
        [ResponseType(typeof(ReportNC))]
        public IHttpActionResult GetReportDinamicCount(int id, DateTime start, DateTime stop)
        {

            try
            {
                string sql = "SELECT DATENAME(month,max(date_start)) + ' ' + DATENAME(year, max(date_start)) as name , COUNT(DATEPART(month,date_start)) AS count FROM TD.MarriageWork " +
                "where (id_classification = "+ id.ToString() + ") and  date_start >= CONVERT(datetime,'" + start.ToString("yyyy-MM-dd HH:mm:ss") + "',120) and  date_start <= CONVERT(datetime,'" + stop.ToString("yyyy-MM-dd HH:mm:ss") + "',120) " +
                "GROUP BY DATEPART(year, date_start), DATEPART(month, date_start) order by DATEPART(year, date_start), DATEPART(month, date_start)";
                List<ReportNC> list = this.ef_mw.Database.SqlQuery<ReportNC>(sql).ToList();
                if (list == null)
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

        // GET: api/dt/marriage_work/dinamic/classification/year/2019
        [Route("marriage_work/dinamic/classification/year/{year:int}")]
        [ResponseType(typeof(DinamicClassification))]
        public IHttpActionResult GetReportDinamicClassification(int year)
        {

            try
            {
                string sql = "SELECT DATENAME(month,max(date_start)) + ' ' + DATENAME(year, max(date_start)) as 'month' , " +
                    "Sum(CASE id_classification WHEN 1 THEN 1 ELSE 0 END) as 'classification1', " +
                    "Sum(CASE id_classification WHEN 2 THEN 1 ELSE 0 END) as 'classification2', " +
                    "Sum(CASE id_classification WHEN 3 THEN 1 ELSE 0 END) as 'classification3', " +
                    "Sum(CASE id_classification WHEN 4 THEN 1 ELSE 0 END) as 'classification4', " +
                    "Sum(CASE id_classification WHEN 5 THEN 1 ELSE 0 END) as 'classification5', " +
                    "Sum(CASE id_classification WHEN 6 THEN 1 ELSE 0 END) as 'classification6', " +
                    "Sum(CASE id_classification WHEN 7 THEN 1 ELSE 0 END) as 'classification7', " +
                    "Sum(CASE id_classification WHEN 8 THEN 1 ELSE 0 END) as 'classification8', " +
                    "Sum(CASE id_classification WHEN 9 THEN 1 ELSE 0 END) as 'classification9', " +
                    "Sum(CASE id_classification WHEN 10 THEN 1 ELSE 0 END) as 'classification10' " +
	                "FROM TD.MarriageWork "+
                    "where DATEPART(year, date_start) = " + year.ToString() + " GROUP BY DATEPART(year, date_start), DATEPART(month, date_start) order by DATEPART(year, date_start), DATEPART(month, date_start)";
                List<DinamicClassification> list = this.ef_mw.Database.SqlQuery<DinamicClassification>(sql).ToList();
                if (list == null)
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
