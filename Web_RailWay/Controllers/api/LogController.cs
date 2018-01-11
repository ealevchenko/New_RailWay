using EFServicesLogs.Abstract;
using EFServicesLogs.Concrete;
using EFServicesLogs.Entities;
using MessageLog;
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
    [RoutePrefix("api/log")]
    public class LogController : ApiController
    {
        protected IServicesLogs rep_serv;
        public LogController()
        {
            this.rep_serv = new EFServicesLog(); 
        }

        /// Вернуть текстовое сообщение из ресурса
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private string GetResource(string key)
        {
            ResourceManager rmLog = new ResourceManager(typeof(LogResource));
            return rmLog.GetString(key, CultureInfo.CurrentCulture);
        }



        // GET: api/log/services/status
        [Route("services/status")]
        [ResponseType(typeof(LogStatusServices))]
        public IHttpActionResult GetLogStatusServices()
        {
            List<LogStatusServices> status = this.rep_serv.GetLogStatusServices().ToList();
            if (status == null)
            {
                return NotFound();
            }
            return Ok(status);
        }

        // GET: api/log/services/110
        [Route("services/{service:int}")]
        [ResponseType(typeof(string))]
        public string GetTypeSendTransfer(int service)
        {
            return ((service)service).ToString();
            //return GetResource(((service)service).ToString());
        }

    }
}
