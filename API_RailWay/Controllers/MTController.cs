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
    [RoutePrefix("api/mt")]
    public class MTController : ApiController
    {
        protected IMT rep_MT;
        public MTController() {
            this.rep_MT = new EFMetallurgTrans();  
        }

        protected ApproachesCars CreateCorectApproachesCars(ApproachesCars app_car)
        {
            return new ApproachesCars()
                {
                    ID = app_car.ID,
                    IDSostav = app_car.IDSostav,
                    CompositionIndex = app_car.CompositionIndex,
                    Num = app_car.Num,
                    CountryCode = app_car.CountryCode,
                    Weight = app_car.Weight,
                    CargoCode = app_car.CargoCode,
                    TrainNumber = app_car.TrainNumber,
                    Operation = app_car.Operation,
                    DateOperation = app_car.DateOperation,
                    CodeStationFrom = app_car.CodeStationFrom,
                    CodeStationOn = app_car.CodeStationOn,
                    CodeStationCurrent = app_car.CodeStationCurrent,
                    CountWagons = app_car.CountWagons,
                    SumWeight = app_car.SumWeight,
                    FlagCargo = app_car.FlagCargo,
                    Route = app_car.Route,
                    Owner = app_car.Owner,
                    NumDocArrival = app_car.NumDocArrival,
                    Arrival = app_car.Arrival,
                    //ParentID = app_car.ParentID,
                    ApproachesSostav = null
                };
        }

        // GET: api/mt/approaches/sostav
        //[Route("approaches/sostav")]
        //public IEnumerable<ApproachesSostav> GetApproachesSostav()
        //{
        //    return this.rep_MT.GetApproachesSostav();
        //}

        // GET: api/mt/approaches/sostav/id/12
        [Route("approaches/sostav/id/{id:int?}")]
        [ResponseType(typeof(ApproachesSostav))]
        public IHttpActionResult GetApproachesSostav(int id)
        {
            ApproachesSostav app_sostav = this.rep_MT.GetApproachesSostav(id); ;
            if (app_sostav == null)
            {
                return NotFound();
            }
            List<ApproachesCars> cars = new List<ApproachesCars>();
            app_sostav.ApproachesCars.ToList().ForEach(c => cars.Add(CreateCorectApproachesCars(c)));
            app_sostav.ApproachesCars = cars.ToArray();
            return Ok(app_sostav);
        }

        // GET: api/mt/approaches/cars/id/12
        [Route("approaches/cars/id/{id:int?}")]
        [ResponseType(typeof(ApproachesCars))]
        public IHttpActionResult GetApproachesCars(int id)
        {
            ApproachesCars cars = this.rep_MT.GetApproachesCars(id);
            if (cars == null)
            {
                return NotFound();
            }
            cars.ApproachesSostav = null;
            ApproachesCars new_cars = CreateCorectApproachesCars(cars);
            return Ok(new_cars);
        }

        // GET: api/mt/approaches/cars/sostav/12
        [Route("approaches/cars/sostav/{id:int?}")]
        [ResponseType(typeof(ApproachesCars))]
        public IHttpActionResult GetApproachesCarsOfSostav(int id)
        {
            IEnumerable<ApproachesCars> cars = this.rep_MT.GetApproachesCarsOfSostav(id);
            if (cars == null)
            {
                return NotFound();
            }
            List<ApproachesCars> new_cars = new List<ApproachesCars>();
            cars.ToList().ForEach(c => new_cars.Add(CreateCorectApproachesCars(c)));
            return Ok(new_cars);
        }
    }
}
