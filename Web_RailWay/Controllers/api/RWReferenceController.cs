using EFRW.Abstract;
using EFRW.Entities1;
using RW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Web_RailWay.Controllers.api
{
    [RoutePrefix("api/rw/reference")]
    public class RWReferenceController : ApiController
    {
        protected IReference rep_ref;

        #region cargo
        public RWReferenceController()
        {
            this.rep_ref = new EFRW.Concrete.EFReference();
        }

        // GET: api/rw/reference/cargo
        [Route("cargo/{id:int?}")]
        public IEnumerable<ReferenceCargo> GetCargo()
        {
            return this.rep_ref.GetReferenceCargo();
        }
        // GET: api/rw/reference/cargo/id/5
        [Route("cargo/id/{id:int?}")]
        [ResponseType(typeof(ReferenceCargo))]
        public IHttpActionResult GetCargo(int id)
        {
            ReferenceCargo cargo = this.rep_ref.GetReferenceCargo(id);
            if (cargo == null)
            {
                return NotFound();
            }

            return Ok(cargo);
        }

        // GET: api/rw/reference/cargo/code/32203
        [Route("cargo/code/{id:int}")]
        [ResponseType(typeof(ReferenceCargo))]
        public IHttpActionResult GetCargoOfCode(int id)
        {
            ReferenceCargo cargo = this.rep_ref.GetCorectReferenceCargo(id);
            if (cargo == null)
            {
                return NotFound();
            }

            return Ok(cargo);
        }

        // GET: api/rw/reference/cargo/code/etsng/0/all
        [Route("cargo/code/etsng/{code:int}/all")]
        [ResponseType(typeof(ReferenceCargo))]
        public IHttpActionResult GetDefaultReferenceCargo(int code)
        {
            RWReference rw_ref = new RWReference(true);
            int id = rw_ref.GetIDReferenceCargoOfCorrectCodeETSNG(code);
            ReferenceCargo cargo = rep_ref.ReferenceCargo
                .Where(c => c.id == id)
                .ToList()
                .Select(c => new ReferenceCargo
                {
                    id = c.id,
                    name_ru = c.name_ru,
                    name_en = c.name_en,
                    name_full_ru = c.name_full_ru,
                    name_full_en = c.name_full_en,
                    etsng = c.etsng,
                    id_type = c.id_type,
                    create_dt = c.create_dt,
                    create_user = c.create_user,
                    change_dt = c.change_dt,
                    change_user = c.change_user,
                    //CarsInpDelivery = null,
                    //CarsOutDelivery = null,
                    ReferenceTypeCargo = new ReferenceTypeCargo
                    {
                        id = c.ReferenceTypeCargo.id,
                        id_group = c.ReferenceTypeCargo.id_group,
                        type_name_en = c.ReferenceTypeCargo.type_name_en,
                        type_name_ru = c.ReferenceTypeCargo.type_name_ru,
                        //ReferenceCargo = null,
                        //ReferenceGroupCargo = null
                    },
                })
                .FirstOrDefault();
            if (cargo == null)
            {
                return NotFound();
            }
            return Ok(cargo);
        }

        // GET: api/rw/reference/cargo/all
        [Route("cargo/all")]
        [ResponseType(typeof(ReferenceCargo))]
        public IHttpActionResult GetReferenceCargo()
        {
            List<ReferenceCargo> list_cargo = rep_ref.ReferenceCargo
                .ToList()
                .Select(c => new ReferenceCargo
                {
                    id = c.id,
                    name_ru = c.name_ru,
                    name_en = c.name_en,
                    name_full_ru = c.name_full_ru,
                    name_full_en = c.name_full_en,
                    etsng = c.etsng,
                    id_type = c.id_type,
                    create_dt = c.create_dt,
                    create_user = c.create_user,
                    change_dt = c.change_dt,
                    change_user = c.change_user,
                })
                .ToList();
            if (list_cargo == null)
            {
                return NotFound();
            }
            return Ok(list_cargo);
        }
        #endregion

        #region ReferenceCountry
        // GET: api/rw/reference/country/all
        [Route("country/all")]
        [ResponseType(typeof(ReferenceCountry))]
        public IHttpActionResult GetReferenceCountry()
        {
            List<ReferenceCountry> list_country = rep_ref.ReferenceCountry
                .ToList()
                .Select(c => new ReferenceCountry
                {
                    id = c.id,
                    country_ru = c.country_ru,
                    country_en = c.country_en,
                    code = c.code,
                })
                .ToList();
            if (list_country == null)
            {
                return NotFound();
            }
            return Ok(list_country);
        }
        #endregion

        #region ReferenceConsignee
        // GET: api/rw/reference/consignee/all
        [Route("consignee/all")]
        [ResponseType(typeof(ReferenceConsignee))]
        public IHttpActionResult GetReferenceConsignee()
        {
            List<ReferenceConsignee> list_consignee = rep_ref.ReferenceConsignee
                .ToList()
                .Select(c => new ReferenceConsignee
                {
                    id = c.id,
                    name_ru = c.name_ru,
                    name_en = c.name_en,
                    name_full_ru = c.name_full_ru,
                    name_full_en = c.name_full_en,
                    name_abr_ru = c.name_abr_ru,
                    name_abr_en = c.name_abr_en,
                    id_shop = c.id_shop,
                    create_dt = c.create_dt,
                    create_user = c.create_user,
                    change_dt = c.change_dt,
                    change_user = c.change_user,
                    id_kis = c.id_kis,
                    //Shops = c.Shops,
                })
                .ToList();
            if (list_consignee == null)
            {
                return NotFound();
            }
            return Ok(list_consignee);
        }
        #endregion

        #region ReferenceStation
        // GET: api/rw/reference/station/all
        [Route("station/all")]
        [ResponseType(typeof(ReferenceStation))]
        public IHttpActionResult GetReferenceStation()
        {
            List<ReferenceStation> list_station = rep_ref.ReferenceStation
                .ToList()
                .Select(c => new ReferenceStation
                {
                    id = c.id,
                    name = c.name,
                    station = c.station,
                    internal_railroad = c.internal_railroad,
                    ir_abbr = c.ir_abbr,
                    name_network = c.name_network,
                    nn_abbr = c.nn_abbr,
                    code_cs = c.code_cs,
                })
                .ToList();
            if (list_station == null)
            {
                return NotFound();
            }
            return Ok(list_station);
        }
        #endregion

    }
}
