using EFRW.Abstract;
using EFRW.Entities;
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

        //protected IRailWay rep_rw;

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
        #endregion

    }
}
