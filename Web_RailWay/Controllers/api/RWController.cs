using EFRW.Abstract;
using EFRW.Concrete;
using EFRW.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Resources;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Description;
using Web_RailWay.App_LocalResources;

namespace Web_RailWay.Controllers.api
{
    [RoutePrefix("api/rw")]
    public class RWController : ApiController
    {
        protected IRailWay rep_rw;
        public RWController()
        {
            this.rep_rw = new EFRailWay();  
        }

        /// Вернуть текстовое сообщение из ресурса
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private string GetResource(string key)
        {
            ResourceManager rmMT = new ResourceManager(typeof(RWResource));
            return rmMT.GetString(key, CultureInfo.CurrentCulture);
        }

        #region StationsNodes
        [HttpPut]
        [Route("stations_nodes/{id:int}")]
        public int EditStationsNodes(int id, [FromBody]StationsNodes StationsNodes)
        {
            return this.rep_rw.SaveStationsNodes(StationsNodes);
        }

        [HttpDelete]
        [Route("stations_nodes/{id:int}")]
        public int DeleteStationsNodes(int id)
        {
            StationsNodes sn = this.rep_rw.DeleteStationsNodes(id);
            return sn!=null ? sn.id : -1;
        }

        // GET: api/rw/stations_nodes/send/station/1
        [Route("stations_nodes/send/station/{id:int}")]
        [ResponseType(typeof(StationsNodes))]
        public IHttpActionResult GetSendStationsNodes(int id)
        {
            List<StationsNodes> nodes = this.rep_rw.GetSendStationsNodes(id).ToList();
            if (nodes == null)
            {
                return NotFound();
            }
            return Ok(nodes);
        }

        // GET: api/rw/stations_nodes/arrival/station/1
        [Route("stations_nodes/arrival/station/{id:int}")]
        [ResponseType(typeof(StationsNodes))]
        public IHttpActionResult GetArrivalStationsNodes(int id)
        {
            List<StationsNodes> nodes = this.rep_rw.GetArrivalStationsNodes(id).ToList();
            if (nodes == null)
            {
                return NotFound();
            }
            return Ok(nodes);
        }

        // GET: api/rw/stations_nodes/id/196
        [Route("stations_nodes/id/{id:int}")]
        [ResponseType(typeof(StationsNodes))]
        public IHttpActionResult GetStationsNodes(int id)
        {
            StationsNodes node = this.rep_rw.GetStationsNodes(id);
            if (node == null)
            {
                return NotFound();
            }
            return Ok(node);
        }
        #endregion

        #region Stations
        // GET: api/rw/station_name/1
        [Route("station_name/{id:int}")]
        [ResponseType(typeof(string))]
        public string GetStationName(int id)
        {
            Stations station = this.rep_rw.GetStations(id);
            if (station == null)
            {
                return id.ToString();
            }
            return Thread.CurrentThread.CurrentUICulture.Name == "en-US" ? station.name_en : station.name_ru;
        }

        // GET: api/rw/stations/view/amkr
        [Route("stations/view/amkr")]
        [ResponseType(typeof(Stations))]
        public IHttpActionResult GetStationsOfViewAMKR()
        {
            List<Stations> stations = this.rep_rw.GetStationsOfViewAMKR().ToList();
            if (stations == null)
            {
                return NotFound();
            }
            return Ok(stations);
        }
        #endregion

        #region Option
        // GET: api/rw/send_transfer/type
        [Route("send_transfer/type")]
        [ResponseType(typeof(Option))]
        public IHttpActionResult GetTypeSendTransfer()
        {
            List<Option> options = new List<Option>();
            List<Option> list = this.rep_rw.GetTypeSendTransfer();
            if (list == null)
            {
                return NotFound();
            }
            list.ForEach(c => options.Add(new Option() {  value = c.value, text = GetResource(c.text)}));
            return Ok(options);
        }   

        // GET: api/rw/send_transfer/type/0
        [Route("send_transfer/type/{type:int}")]
        [ResponseType(typeof(string))]
        public string GetTypeSendTransfer(int type)
        {
            return GetResource(this.rep_rw.GetTypeSendTransfer(type));
        }

        // GET: api/rw/side
        [Route("side")]
        [ResponseType(typeof(Option))]
        public IHttpActionResult GetSide()
        {
            List<Option> options = new List<Option>();
            List<Option> list = this.rep_rw.GetSide();
            if (list == null)
            {
                return NotFound();
            }
            list.ForEach(c => options.Add(new Option() {  value = c.value, text = GetResource(c.text)}));
            return Ok(options);
        }   
     
        // GET: api/rw/side/0
        [Route("side/{side:int}")]
        [ResponseType(typeof(string))]
        public string GetSide(int side)
        {
            return GetResource(this.rep_rw.GetSide(side));
        }
        #endregion

    }
}
