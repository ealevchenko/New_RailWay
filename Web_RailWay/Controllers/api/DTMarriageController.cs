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

        public DTMarriageController(IRepository<MarriageWork> mw)
        {
            this.ef_mw = mw;
        }

        // GET: api/dt/marriage_work
        [Route("marriage_work")]
        [ResponseType(typeof(MarriageWork))]
        public IHttpActionResult GetCards()
        {
            try
            {
                List<MarriageWork> list = this.ef_mw.Get().ToList();
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
    }
}
