using EFReference.Abstract;
using EFReference.Concrete;
using EFReference.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace API_RailWay.Controllers
{
    [RoutePrefix("api/reference")]
    public class ReferenceController : ApiController
    {
        IReference rep_ref;

        public ReferenceController()
        {
            this.rep_ref = new EFReference.Concrete.EFReference();
        }        
        
        #region cargo
        // GET: api/reference/cargo
        [Route("cargo/{id:int?}")]
        public IEnumerable<Cargo> GetCargo()
        {
            return this.rep_ref.GetCargo();
        }
        // GET: api/reference/cargo/id/5
        [Route("cargo/id/{id:int?}")]
        [ResponseType(typeof(Cargo))]
        public IHttpActionResult GetCargo(int id)
        {
            Cargo cargo = this.rep_ref.GetCargo(id);
            if (cargo == null)
            {
                return NotFound();
            }

            return Ok(cargo);
        }

        // GET: api/reference/cargo/etsng_code/32203
        [Route("cargo/etsng_code/{id:int}")]
        [ResponseType(typeof(Cargo))]
        public IHttpActionResult GetCargoOfCode(int id)
        {
            Cargo cargo = this.rep_ref.GetCorrectCargo(id);
            if (cargo == null)
            {
                return NotFound();
            }

            return Ok(cargo);
        }
        #endregion

        #region Stations

        protected Stations CreateStations(Stations stations)
        {
            return new Stations()
            {
                id = stations.id,
                code = stations.code,
                code_cs = stations.code_cs,
                station = stations.station,
                id_ir = stations.id_ir,
                InternalRailroad = null,
            };
        }

        // GET: api/reference/stations
        [Route("stations/{id:int?}")]
        [ResponseType(typeof(Stations))]
        public IEnumerable<Stations> GetStations()
        {
            List<Stations> new_station = new List<Stations>();
            this.rep_ref.GetStations().ToList().ForEach(c => new_station.Add(CreateStations(c)));
            return new_station;
        }
        // GET: api/reference/stations/id/5
        [Route("stations/id/{id:int?}")]
        [ResponseType(typeof(Stations))]
        public IHttpActionResult GetStations(int id)
        {
            Stations stations = this.rep_ref.GetStations(id);
            if (stations == null)
            {
                return NotFound();
            }

            return Ok(CreateStations(stations));
        }

        // GET: api/reference/stations/code/46700
        [Route("stations/code/{id:int}")]
        [ResponseType(typeof(Stations))]
        public IHttpActionResult GetStationsOfCode(int id)
        {
            Stations stations = this.rep_ref.GetStationsOfCode(id);
            if (stations == null)
            {
                return NotFound();
            }
            return Ok(CreateStations(stations));
        }
        #endregion
    }
}
