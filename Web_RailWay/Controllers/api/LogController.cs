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
    public class Option
    {
        public int value { get; set; }
        public string text { get; set; }
    }
    
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

        // GET: api/log/service/name
        [Route("service/name")]
        [ResponseType(typeof(Option))]
        public IHttpActionResult GetTypeSendTransfer()
        {
            List<Option> list = new List<Option>();
            foreach (service ser in Enum.GetValues(typeof(service))) {
                list.Add(new Option() { value = (int)ser, text = ((service)ser).ToString() });
            }
            if (list == null && list.Count() ==0)
            {
                return NotFound();
            }
            return Ok(list);
        }

        // GET: api/log/service/name/110
        [Route("service/name/{service:int}")]
        [ResponseType(typeof(string))]
        public string GetTypeSendTransfer(int service)
        {
            return ((service)service).ToString();
            //return GetResource(((service)service).ToString());
        }

        // GET: api/log/services/110/2018-01-13T22:00:00.000Z/2018-01-15T21:59:59.000Z
        [Route("services/{service:int}/{start:datetime}/{stop:datetime}")]
        [ResponseType(typeof(LogServices))]
        public IHttpActionResult GetLogServices(int service, DateTime start, DateTime stop)
        {
            List<LogServices> status = this.rep_serv.GetLogServices(service, start, stop).ToList();
            if (status == null)
            {
                return NotFound();
            }
            return Ok(status);
        }

        // GET: api/log/services/2018-01-13T22:00:00.000Z/2018-01-15T21:59:59.000Z
        [Route("services/{start:datetime}/{stop:datetime}")]
        [ResponseType(typeof(LogServices))]
        public IHttpActionResult GetLogServices(DateTime start, DateTime stop)
        {
            List<LogServices> status = this.rep_serv.GetLogServices(start, stop).ToList();
            if (status == null)
            {
                return NotFound();
            }
            return Ok(status);
        }

    }
}
