using AppSettings;
using EFSettings.Abstract;
using EFSettings.Concrete;
using EFSettings.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Web_RailWay.Controllers.api
{
    [RoutePrefix("api/setting")]
    public class SettingController : ApiController
    {
        protected ISettings rep_setting;


        public SettingController()
        {
            this.rep_setting = new EFSetting(true);
        }

        // GET: api/setting/type_value
        [Route("type_value")]
        [ResponseType(typeof(Option))]
        public IHttpActionResult GetModuleTransfer()
        {
            List<Option> list = new List<Option>();
            foreach (st_value type in Enum.GetValues(typeof(st_value)))
            {
                list.Add(new Option() { value = (int)type, text = ((st_value)type).ToString() });
            }
            if (list == null && list.Count() == 0)
            {
                return NotFound();
            }
            return Ok(list);
        }

        // GET: api/setting/type_value/1
        [Route("type_value/{type:int}")]
        [ResponseType(typeof(string))]
        public string GetModuleTransfer(int type)
        {
            return ((st_value)type).ToString();
        }

        #region Settings
        // GET: api/setting/service/110
        [Route("service/{service:int}")]
        [ResponseType(typeof(Settings))]
        public IHttpActionResult GetSettingsOfServices(int service)
        {
            List<Settings> setting = this.rep_setting.GetSettingsOfServices(service).ToList();
            if (setting == null)
            {
                return NotFound();
            }
            return Ok(setting);
        }

        // GET: api/setting
        [Route("")]
        [ResponseType(typeof(Settings))]
        public IHttpActionResult GetSettings()
        {
            List<Settings> setting = this.rep_setting.GetSettings().ToList();
            if (setting == null)
            {
                return NotFound();
            }
            return Ok(setting);
        }
        #endregion

    }
}
