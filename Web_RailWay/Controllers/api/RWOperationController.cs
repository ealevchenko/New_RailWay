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
    [RoutePrefix("api/rw/operation")]
    public class RWOperationController : ApiController
    {
        protected IRWOperation rw_oper;

        public RWOperationController()
        {
            this.rw_oper = new RWOperation();
        }


        // DELETE: api/rw/operation/cars/delete/79381
        [HttpDelete]
        [Route("cars/delete/{id:int}")]
        public int DeleteSaveCar(int id)
        {
            return this.rw_oper.DeleteSaveCar(id);
        }

    }
}
