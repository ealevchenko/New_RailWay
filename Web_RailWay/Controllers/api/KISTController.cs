﻿using EFKIS.Abstract;
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
        [ResponseType(typeof(BufferArrivalSostav))]
        public IHttpActionResult GetBufferArrivalSostav(DateTime start, DateTime stop)
        {
            List<BufferArrivalSostav> list = this.rep_kist.GetBufferArrivalSostav(start,stop)
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
            return this.rep_kist.CloseBufferArrivalSostav(id, User.Identity.Name); 
        }

        #endregion

        #region BufferInputSostav

        // GET: api/kis/transfer/bis/2018-01-13T22:00:00.000Z/2018-01-15T21:59:59.000Z
        [Route("bis/{start:datetime}/{stop:datetime}")]
        [ResponseType(typeof(BufferInputSostav))]
        public IHttpActionResult GetBufferInputSostav(DateTime start, DateTime stop)
        {
            List<BufferInputSostav> list = this.rep_kist.GetBufferInputSostav(start, stop)
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
        [ResponseType(typeof(BufferOutputSostav))]
        public IHttpActionResult GetBufferOutputSostav(DateTime start, DateTime stop)
        {
            List<BufferOutputSostav> list = this.rep_kist.GetBufferOutputSostav(start, stop)
                                                .OrderByDescending(x => x.datetime)
                                                .ToList();
            if (list == null)
            {
                return NotFound();
            }
            return Ok(list);
        }
        #endregion

    }
}