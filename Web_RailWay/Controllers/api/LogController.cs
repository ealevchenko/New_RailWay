using EFLogs.Abstract;
using EFLogs.Concrete;
using EFLogs.Entities;
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
        protected IServicesLogs rep_services;
        protected IDBLog rep_log;


        public LogController()
        {
            this.rep_services = new EFServicesLog(true);
            this.rep_log = new EFLog(true);
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
            List<LogStatusServices> status = this.rep_services.GetLogStatusServices().ToList();
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
        }

        // GET: api/log/module/name
        [Route("module/name")]
        [ResponseType(typeof(Option))]
        public IHttpActionResult GetModuleTransfer()
        {
            List<Option> list = new List<Option>();
            foreach (eventID modul in Enum.GetValues(typeof(eventID)))
            {
                list.Add(new Option() { value = (int)modul, text = ((eventID)modul).ToString() });
            }
            if (list == null && list.Count() ==0)
            {
                return NotFound();
            }
            return Ok(list);
        }

        // GET: api/log/module/name/110
        [Route("module/name/{module:int}")]
        [ResponseType(typeof(string))]
        public string GetModuleTransfer(int module)
        {
            return ((eventID)module).ToString();
        }

        #region LogServices
        // GET: api/log/services/110/2018-01-13T22:00:00.000Z/2018-01-15T21:59:59.000Z
        [Route("services/{service:int}/{start:datetime}/{stop:datetime}")]
        [ResponseType(typeof(LogServices))]
        public IHttpActionResult GetLogServices(int service, DateTime start, DateTime stop)
        {
            List<LogServices> status = this.rep_services.GetLogServices(service, start, stop).ToList();
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
            List<LogServices> status = this.rep_services.GetLogServices(start, stop).ToList();
            if (status == null)
            {
                return NotFound();
            }
            return Ok(status);
        }
        #endregion

        #region LogErrors
        // GET: api/log/errors/110/2018-01-13T22:00:00.000Z/2018-01-15T21:59:59.000Z
        [Route("errors/{service:int}/{start:datetime}/{stop:datetime}")]
        [ResponseType(typeof(LogErrors))]
        public IHttpActionResult GetLogErrors(int service, DateTime start, DateTime stop)
        {
            List<LogErrors> status = this.rep_log.GetLogErrors(service, start, stop).ToList();
            if (status == null)
            {
                return NotFound();
            }
            return Ok(status);
        }

        // GET: api/log/errors/2018-01-13T22:00:00.000Z/2018-01-15T21:59:59.000Z
        [Route("errors/{start:datetime}/{stop:datetime}")]
        [ResponseType(typeof(LogErrors))]
        public IHttpActionResult GetLogErrors(DateTime start, DateTime stop)
        {
            List<LogErrors> status = this.rep_log.GetLogErrors(start, stop).ToList();
            if (status == null)
            {
                return NotFound();
            }
            return Ok(status);
        }
        #endregion

        #region LogEvents
        // GET: api/log/events/110/2018-01-13T22:00:00.000Z/2018-01-15T21:59:59.000Z
        [Route("events/{service:int}/{start:datetime}/{stop:datetime}")]
        [ResponseType(typeof(LogEvents))]
        public IHttpActionResult GetLogEvents(int service, DateTime start, DateTime stop)
        {
            List<LogEvents> status = this.rep_log.GetLogEventsOfServices(start, stop, service).ToList();
            if (status == null)
            {
                return NotFound();
            }
            return Ok(status);
        }

        // GET: api/log/events/2018-01-13T22:00:00.000Z/2018-01-15T21:59:59.000Z
        [Route("events/{start:datetime}/{stop:datetime}")]
        [ResponseType(typeof(LogEvents))]
        public IHttpActionResult GetLogEvents(DateTime start, DateTime stop)
        {
            List<LogEvents> status = this.rep_log.GetLogEvents(start, stop).ToList();
            if (status == null)
            {
                return NotFound();
            }
            return Ok(status);
        }
        #endregion

    }
}
