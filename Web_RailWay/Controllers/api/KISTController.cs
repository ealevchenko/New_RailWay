using EFKIS.Abstract;
using EFKIS.Concrete;
using EFKIS.Entities;
using KIS;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Resources;
using System.Web.Http;
using System.Web.Http.Description;
using Web_RailWay.App_LocalResources;

namespace Web_RailWay.Controllers.api
{
    [RoutePrefix("api/kis/transfer")]
    public class KISTController : ApiController
    {
        protected ITKIS rep_kist;

        public KISTController()
        {
            this.rep_kist = new EFTKIS();
        }

        /// Вернуть текстовое сообщение из ресурса
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private string GetResource(string key)
        {
            ResourceManager rmLog = new ResourceManager(typeof(KISResource));
            return rmLog.GetString(key, CultureInfo.CurrentCulture);
        }

        // GET: api/kis/transfer/status
        [Route("status")]
        [ResponseType(typeof(Option))]
        public IHttpActionResult GetStatus()
        {
            List<Option> list = new List<Option>();
            foreach (statusSting status in Enum.GetValues(typeof(statusSting)))
            {
                list.Add(new Option() { value = (int)status, text = ((statusSting)status).ToString() });
            }
            if (list == null && list.Count() == 0)
            {
                return NotFound();
            }
            return Ok(list);
        }

        // GET: api/kis/transfer/status/1
        [Route("status/{status:int}")]
        [ResponseType(typeof(string))]
        public string GetStatus(int status)
        {
            return ((statusSting)status).ToString();
        }

        // GET: api/kis/transfer/status/name
        [Route("status/name")]
        [ResponseType(typeof(Option))]
        public IHttpActionResult GetStatusName()
        {
            List<Option> list = new List<Option>();
            foreach (statusSting status in Enum.GetValues(typeof(statusSting)))
            {
                list.Add(new Option() { value = (int)status, text = GetResource(((statusSting)status).ToString()) });
            }
            if (list == null && list.Count() == 0)
            {
                return NotFound();
            }
            return Ok(list);
        }

        // GET: api/kis/transfer/status/1/name
        [Route("status/{status:int}/name")]
        [ResponseType(typeof(string))]
        public string GetStatusName(int status)
        {
            return GetResource(((statusSting)status).ToString());
        }

        // GET: api/kis/transfer/error
        [Route("error")]
        [ResponseType(typeof(Option))]
        public IHttpActionResult GetErrorTransfer()
        {
            List<Option> list = new List<Option>();
            foreach (errorTransfer errors in Enum.GetValues(typeof(errorTransfer)))
            {
                list.Add(new Option() { value = (int)errors, text = ((errorTransfer)errors).ToString() });
            }
            if (list == null && list.Count() == 0)
            {
                return NotFound();
            }
            return Ok(list);
        }

        // GET: api/kis/transfer/error/1
        [Route("error/{error:int}")]
        [ResponseType(typeof(string))]
        public string GetErrorTransfer(int error)
        {
            return ((statusSting)error).ToString();
        }

        #region BufferArrivalSostav

        // GET: api/kis/transfer/bas/2018-01-13T22:00:00.000Z/2018-01-15T21:59:59.000Z
        [Route("bas/{start:datetime}/{stop:datetime}")]
        [ResponseType(typeof(RCBufferArrivalSostav))]
        public IHttpActionResult GetBufferArrivalSostav(DateTime start, DateTime stop)
        {
            List<RCBufferArrivalSostav> list = this.rep_kist.GetRCBufferArrivalSostav(start, stop)
                                                .OrderByDescending(x => x.datetime)
                                                .ToList();
            if (list == null)
            {
                return NotFound();
            }
            return Ok(list);
        }

        [HttpPost]
        [Route("bas/{id:int}/close")]
        public int CloseBufferArrivalSostav(int id)
        {
            return this.rep_kist.CloseRCBufferArrivalSostav(id, User.Identity.Name);
        }

        #endregion

        #region BufferInputSostav

        // GET: api/kis/transfer/bis/2018-01-13T22:00:00.000Z/2018-01-15T21:59:59.000Z
        [Route("bis/{start:datetime}/{stop:datetime}")]
        [ResponseType(typeof(RCBufferInputSostav))]
        public IHttpActionResult GetBufferInputSostav(DateTime start, DateTime stop)
        {
            List<RCBufferInputSostav> list = this.rep_kist.GetRCBufferInputSostav(start, stop)
                                                .OrderByDescending(x => x.datetime)
                                                .ToList();
            if (list == null)
            {
                return NotFound();
            }
            return Ok(list);
        }
        #endregion

        #region BufferOutputSostav

        // GET: api/kos/transfer/bis/2018-01-13T22:00:00.000Z/2018-01-15T21:59:59.000Z
        [Route("bos/{start:datetime}/{stop:datetime}")]
        [ResponseType(typeof(RCBufferOutputSostav))]
        public IHttpActionResult GetBufferOutputSostav(DateTime start, DateTime stop)
        {
            List<RCBufferOutputSostav> list = this.rep_kist.GetRCBufferOutputSostav(start, stop)
                                                .OrderByDescending(x => x.datetime)
                                                .ToList();
            if (list == null)
            {
                return NotFound();
            }
            return Ok(list);
        }
        #endregion

        #region RAILWAY
        #region BufferArrivalSostav

        // GET: api/kis/transfer/arrival/2018-01-13T22:00:00.000Z/2018-01-15T21:59:59.000Z
        [Route("arrival/{start:datetime}/{stop:datetime}")]
        [ResponseType(typeof(RWBufferArrivalSostav))]
        public IHttpActionResult GetRWBufferArrivalSostav(DateTime start, DateTime stop)
        {
            List<RWBufferArrivalSostav> list = this.rep_kist.GetRWBufferArrivalSostav(start, stop)
                                                .OrderByDescending(x => x.datetime)
                                                .ToList();
            if (list == null)
            {
                return NotFound();
            }
            return Ok(list);
        }

        // GET: api/kis/transfer/arrival/id/2513
        [Route("arrival/id/{id:int}")]
        [ResponseType(typeof(RWBufferArrivalSostav))]
        public IHttpActionResult GetRWBufferArrivalSostav(int id)
        {
            RWBufferArrivalSostav bas = this.rep_kist.GetRWBufferArrivalSostav(id);
            if (bas == null)
            {
                return NotFound();
            }
            return Ok(bas);
        }

        [HttpPost]
        [Route("arrival/{id:int}/close")]
        public int CloseRWBufferArrivalSostav(int id)
        {
            return this.rep_kist.CloseRWBufferArrivalSostav(id, User.Identity.Name);
        }

        #endregion

        #region BufferSendingSostav

        // GET: api/kis/transfer/sending/2018-01-13T22:00:00.000Z/2018-01-15T21:59:59.000Z
        [Route("sending/{start:datetime}/{stop:datetime}")]
        [ResponseType(typeof(RWBufferSendingSostav))]
        public IHttpActionResult GetRWBufferSendingSostav(DateTime start, DateTime stop)
        {
            List<RWBufferSendingSostav> list = this.rep_kist.GetRWBufferSendingSostav(start, stop)
                                                .OrderByDescending(x => x.datetime)
                                                .ToList();
            if (list == null)
            {
                return NotFound();
            }
            return Ok(list);
        }

        // GET: api/kis/transfer/sending/id/2513
        [Route("sending/id/{id:int}")]
        [ResponseType(typeof(RWBufferSendingSostav))]
        public IHttpActionResult GetRWBufferSendingSostav(int id)
        {
            RWBufferSendingSostav bas = this.rep_kist.GetRWBufferSendingSostav(id);
            if (bas == null)
            {
                return NotFound();
            }
            return Ok(bas);
        }

        [HttpPost]
        [Route("sending/{id:int}/close")]
        public int CloseRWBufferSendinglSostav(int id)
        {
            return this.rep_kist.CloseRWBufferSendingSostav(id, User.Identity.Name);
        }

        #endregion

        #endregion

    }
}
