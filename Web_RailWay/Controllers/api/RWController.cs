using EFMT.Abstract;
using EFRW.Abstract;
using EFRW.Concrete;
using EFRW.Entities;
using MessageLog;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
        private eventID eventID = eventID.Web_API_RWController;
        protected IRailWay rep_rw;

        public class ArrivalSostav
        { 
            public int id_sostav {get;set;}           
            public int id_arrival {get;set;}
            public string index {get;set;}
            public DateTime dt_inp_station {get;set;}
            public int cars {get;set;}  
        }

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
            return sn != null ? sn.id : -1;
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


        // GET: api/rw/stations_nodes
        [Route("stations_nodes")]
        [ResponseType(typeof(StationsNodes))]
        public IHttpActionResult GetStationsNodes()
        {
            try
            {
                //List<StationsNodes> nodes = this.rep_rw.GetStationsNodes().ToList();
                List<StationsNodes> nodes = this.rep_rw.StationsNodes
                    .ToList()
                    .Select(n => new StationsNodes
                        {
                            id = n.id,
                            nodes = n.nodes,
                            id_station_from = n.id_station_from,
                            side_station_from = n.side_station_from,
                            id_station_on = n.id_station_on,
                            side_station_on = n.side_station_on,
                            transfer_type = n.transfer_type,
                        }).ToList();
                if (nodes == null)
                {
                    return NotFound();
                }
                return Json(nodes);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetStationsNodes()"), eventID);
                return InternalServerError(e);
            }
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

        // GET: api/rw/station/1
        [Route("station/{id:int}")]
        [ResponseType(typeof(Stations))]
        public IHttpActionResult GetStation(int id)
        {
            try
            {
                Stations station = this.rep_rw.Stations
                    .Where(s => s.id == id)
                    .ToList()
                    .Select(c => new Stations
                         {
                             id = c.id,
                             name_ru = c.name_ru,
                             name_en = c.name_en,
                             view = c.view,
                             exit_uz = c.exit_uz,
                             station_uz = c.station_uz,
                             id_rs = c.id_rs,
                             id_kis = c.id_kis,
                             default_side = c.default_side,
                             code_uz = c.code_uz,
                             Ways = c.Ways
                                .ToList()
                                .Select(w => new Ways
                                        {
                                            id = w.id,
                                            id_station = w.id_station,
                                            num = w.num,
                                            name_ru = w.name_ru,
                                            name_en = w.name_en,
                                            position = w.position,
                                            capacity = w.capacity,
                                            id_car_status = w.id_car_status,
                                            tupik = w.tupik,
                                            dissolution = w.dissolution,
                                            defrosting = w.defrosting,
                                            overturning = w.overturning,
                                            pto = w.pto,
                                            cleaning = w.cleaning,
                                            rest = w.rest,
                                            id_rc = w.id_rc
                                        }).ToList(),
                             CarOperations = null,
                             StationsNodes = c.StationsNodes
                                .ToList()
                                .Select(n => new StationsNodes
                                        {
                                            id = n.id,
                                            nodes = n.nodes,
                                            id_station_from = n.id_station_from,
                                            side_station_from = n.side_station_from,
                                            id_station_on = n.id_station_on,
                                            side_station_on = n.side_station_on,
                                            transfer_type = n.transfer_type,
                                        }).ToList(),
                             StationsNodes1 = c.StationsNodes1
                                .ToList()
                                .Select(n => new StationsNodes
                                        {
                                            id = n.id,
                                            nodes = n.nodes,
                                            id_station_from = n.id_station_from,
                                            side_station_from = n.side_station_from,
                                            id_station_on = n.id_station_on,
                                            side_station_on = n.side_station_on,
                                            transfer_type = n.transfer_type,
                                        }).ToList(),
                         })
                         .FirstOrDefault();
                if (station == null)
                {
                    return NotFound();
                }
                return Json(station);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetStation(id={0})", id), eventID);
                return InternalServerError(e);
            }
        }
        //public IHttpActionResult GetStation(int id)
        //{
        //    Stations station = this.rep_rw.GetStations(id);
        //    if (station == null)
        //    {
        //        return NotFound();
        //    }

        //    //Stations st = new Stations() {
        //    //    id = station.id,
        //    //    name_ru = station.name_ru,
        //    //    name_en = station.name_en,
        //    //    view = station.view,
        //    //    exit_uz = station.exit_uz,
        //    //    station_uz = station.station_uz,
        //    //    id_rs = station.id_rs,
        //    //    id_kis = station.id_kis,
        //    //    default_side = station.default_side,
        //    //    code_uz = station.code_uz,
        //    //    //Ways = station.Ways,
        //    //    //CarOperations = station.CarOperations,
        //    //    //StationsNodes = station.StationsNodes,
        //    //    //StationsNodes1 = station.StationsNodes1 
        //    //};

        //    return Ok(station);
        //}

        //// GET: api/rw/station_name/1
        //[Route("station_name/{id:int}")]
        //[ResponseType(typeof(string))]
        //public string GetStationName(int id)
        //{
        //    Stations station = this.rep_rw.GetStations(id);
        //    if (station == null)
        //    {
        //        return id.ToString();
        //    }
        //    return Thread.CurrentThread.CurrentUICulture.Name == "en-US" ? station.name_en : station.name_ru;
        //}

        // GET: api/rw/station/1/name
        [Route("station/{id:int}/name")]
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

        // GET: api/rw/stations
        [Route("stations")]
        [ResponseType(typeof(Stations))]
        public IHttpActionResult GetStations()
        {
            try
            {
                //List<Stations> stations = this.rep_rw.GetStations().ToList();
                List<Stations> stations = this.rep_rw.Stations
        .ToList()
        .Select(c => new Stations
        {
            id = c.id,
            name_ru = c.name_ru,
            name_en = c.name_en,
            view = c.view,
            exit_uz = c.exit_uz,
            station_uz = c.station_uz,
            id_rs = c.id_rs,
            id_kis = c.id_kis,
            default_side = c.default_side,
            code_uz = c.code_uz,
            Ways = c.Ways
               .ToList()
               .Select(w => new Ways
               {
                   id = w.id,
                   id_station = w.id_station,
                   num = w.num,
                   name_ru = w.name_ru,
                   name_en = w.name_en,
                   position = w.position,
                   capacity = w.capacity,
                   id_car_status = w.id_car_status,
                   tupik = w.tupik,
                   dissolution = w.dissolution,
                   defrosting = w.defrosting,
                   overturning = w.overturning,
                   pto = w.pto,
                   cleaning = w.cleaning,
                   rest = w.rest,
                   id_rc = w.id_rc
               }).ToList(),
            CarOperations = null,
            StationsNodes = c.StationsNodes
               .ToList()
               .Select(n => new StationsNodes
               {
                   id = n.id,
                   nodes = n.nodes,
                   id_station_from = n.id_station_from,
                   side_station_from = n.side_station_from,
                   id_station_on = n.id_station_on,
                   side_station_on = n.side_station_on,
                   transfer_type = n.transfer_type,
               }).ToList(),
            StationsNodes1 = c.StationsNodes1
               .ToList()
               .Select(n => new StationsNodes
               {
                   id = n.id,
                   nodes = n.nodes,
                   id_station_from = n.id_station_from,
                   side_station_from = n.side_station_from,
                   id_station_on = n.id_station_on,
                   side_station_on = n.side_station_on,
                   transfer_type = n.transfer_type,
               }).ToList(),
                })
             .ToList();

                if (stations == null)
                {
                    return NotFound();
                }
                return Ok(stations);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetStations()"), eventID);
                return InternalServerError(e);
            }
        }
        #endregion

        #region Option
        // GET: api/rw/send_transfer/type
        [Route("send_transfer/type")]
        [ResponseType(typeof(Option))]
        public IHttpActionResult GetTypeSendTransfer()
        {
            List<EFRW.Concrete.Option> options = new List<EFRW.Concrete.Option>();
            List<EFRW.Concrete.Option> list = this.rep_rw.GetTypeSendTransfer();
            if (list == null)
            {
                return NotFound();
            }
            list.ForEach(c => options.Add(new EFRW.Concrete.Option() { value = c.value, text = GetResource(c.text) }));
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
            try
            {
                List<Option> sides = new List<Option>();
                foreach (EFRailWay.Side side in Enum.GetValues(typeof(EFRailWay.Side)))
                {
                    sides.Add(new Option() { value = (int)side, text = side.ToString() });
                }
                if (sides == null)
                {
                    return NotFound();
                }
                return Json(sides);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetSide()"), eventID);
                return InternalServerError(e);
            }
        }
        //public IHttpActionResult GetSide()
        //{
        //    List<EFRW.Concrete.Option> options = new List<EFRW.Concrete.Option>();
        //    List<EFRW.Concrete.Option> list = this.rep_rw.GetSide();
        //    if (list == null)
        //    {
        //        return NotFound();
        //    }
        //    list.ForEach(c => options.Add(new EFRW.Concrete.Option() { value = c.value, text = GetResource(c.text) }));
        //    return Ok(options);
        //}
        // GET: api/rw/side/0
        [Route("side/{side:int}")]
        [ResponseType(typeof(string))]
        public string GetSide(int side)
        {
            return GetResource(this.rep_rw.GetSide(side));
        }
        #endregion

        // GET: api/rw/arrival/sostav/station/16
        [Route("arrival/sostav/station/{id:int}")]
        [ResponseType(typeof(ArrivalSostav))]
        public IHttpActionResult GetArrivalSostavOfStationUZ(int id)
        {
            try
            {
                SqlParameter i_id = new SqlParameter("@id", id);
                List<ArrivalSostav> arr_cars = this.rep_rw.Database.SqlQuery<ArrivalSostav>("EXEC [RailWay].[GetArrivalSostavOfStationUZ] @id", i_id).ToList();
                if (arr_cars == null)
                {
                    return NotFound();
                }
                return Ok(arr_cars);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetArrivalSostavOfStationUZ(id={0})", id), eventID);
                return InternalServerError(e);
            }
        }

    }
}
