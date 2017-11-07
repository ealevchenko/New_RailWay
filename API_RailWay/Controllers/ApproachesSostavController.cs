using EFMT.Abstract;
using EFMT.Concrete;
using MT.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace API_RailWay.Controllers
{
    //[Authorize]
    public class ApproachesSostavController : ApiController
    {
        protected IMT rep_MT;

        public ApproachesSostavController() { 
            this.rep_MT = new EFMetallurgTrans();        
        }
        
        public ApproachesSostavController(IMT rep) {
            this.rep_MT = rep;
        }

        // GET: api/ApproachesSostav
        //public IQueryable<ApproachesSostav> Get()
        //{
        //    return this.rep_MT.GetApproachesSostav();
        //}

        // GET: api/ApproachesSostav/12
        [ResponseType(typeof(ApproachesSostav))]
        public IHttpActionResult Get(int id)
        {
            ApproachesSostav app_s = this.rep_MT.GetApproachesSostav(id); ;
            if (app_s == null)
            {
                return NotFound();
            }
            List<ApproachesCars> cars = new List<ApproachesCars>();
            app_s.ApproachesCars.ToList().ForEach(c => cars.Add(new ApproachesCars() {
                ID = c.ID,
                IDSostav = c.IDSostav,
                CompositionIndex = c.CompositionIndex,
                Num = c.Num,
                CountryCode = c.CountryCode,
                Weight = c.Weight,
                CargoCode = c.CargoCode,
                TrainNumber = c.TrainNumber,
                Operation = c.Operation,
                DateOperation = c.DateOperation,
                CodeStationFrom = c.CodeStationFrom,
                CodeStationOn = c.CodeStationOn,
                CodeStationCurrent = c.CodeStationCurrent,
                CountWagons = c.CountWagons,
                SumWeight = c.SumWeight,
                FlagCargo = c.FlagCargo,
                Route = c.Route,
                Owner = c.Owner,
                NumDocArrival = c.NumDocArrival,
                Arrival = c.Arrival,
                //ParentID = ApproachesCars.ParentID,
                ApproachesSostav = null
            }));
            app_s.ApproachesCars = cars.ToArray();
            return Ok(app_s);
        }

    }
}
