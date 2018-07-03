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
            public int id_sostav { get; set; }
            public int id_arrival { get; set; }
            public string index { get; set; }
            public DateTime dt_inp_station { get; set; }
            public int id_station { get; set; }
            public string name_ru { get; set; }
            public string name_en { get; set; }
            public int id_way { get; set; }
            public int cars { get; set; }
        }

        public class CarsOnWay
        {
            public int id { get; set; }
            public string num { get; set; }
            public string name_ru { get; set; }
            public string name_en { get; set; }
            public int cars { get; set; }
            public int? capacity { get; set; }
        }

        public class CarsDetails
        {
            public int id_operations { get; set; }
            public int? operations_parent_id { get; set; }
            public int position { get; set; }
            public int id_cars { get; set; }
            public int? parent_id_cars { get; set; }
            public int id_sostav { get; set; }
            public int id_arrival { get; set; }
            public int num { get; set; }
            public DateTime dt_uz { get; set; }
            public DateTime? dt_inp_amkr { get; set; }
            public DateTime? dt_out_amkr { get; set; }
            public int? natur_kis { get; set; }
            public int? natur { get; set; }
            public int id_group_cars { get; set; }
            public string group_cars_ru { get; set; }
            public string group_cars_en { get; set; }
            public int id_type_cars { get; set; }
            public string type_cars_ru { get; set; }
            public string type_cars_en { get; set; }
            public string type_cars_abr_ru { get; set; }
            public string type_cars_abr_en { get; set; }
            public decimal? lifting_capacity { get; set; }
            public decimal? tare { get; set; }
            public int id_country { get; set; }
            public string country_ru { get; set; }
            public string country_en { get; set; }
            public int country_code_sng { get; set; }
            public int country_code_iso { get; set; }
            public int? count_axles { get; set; }
            public bool? is_output_uz { get; set; }
            public int id_owners { get; set; }
            public string owner_name { get; set; }
            public string owner_abr { get; set; }
            public DateTime? start_lease { get; set; }
            public DateTime? end_lease { get; set; }
            public int? id_status { get; set; }
            public string status_ru { get; set; }
            public string status_en { get; set; }
            public int? id_status_next { get; set; }
            public int? id_conditions { get; set; }
            public string conditions_ru { get; set; }
            public string conditions_en { get; set; }
            public int? id_station { get; set; }
            public DateTime? dt_inp_station { get; set; }
            public DateTime? dt_out_station { get; set; }
            public int? id_way { get; set; }
            public DateTime? dt_inp_way { get; set; }
            public DateTime? dt_out_way { get; set; }
            //public int position {get;set;}
            public int? send_id_station { get; set; }
            public int? send_id_overturning { get; set; }
            public int? send_id_shop { get; set; }
            public DateTime? send_dt_inp_way { get; set; }
            public DateTime? send_dt_out_way { get; set; }
            public int? send_id_position { get; set; }
            public int? send_train1 { get; set; }
            public int? send_train2 { get; set; }
            public int? send_side { get; set; }
            public string edit_user { get; set; }
            public DateTime? edit_dt { get; set; }
            public string inp_sostav_index { get; set; }
            public string inp_num_nakl_sap { get; set; }
            public decimal? inp_weight_cargo { get; set; }
            public int? inp_num_doc_reweighing_sap { get; set; }
            public DateTime? inp_dt_doc_reweighing_sap { get; set; }
            public decimal? inp_weight_reweighing_sap { get; set; }
            public DateTime? inp_dt_reweighing_sap { get; set; }
            public int? inp_post_reweighing_sap { get; set; }
            public int inp_cargo_code { get; set; }
            public int inp_id_group_cargo { get; set; }
            public string inp_group_cargo_ru { get; set; }
            public string inp_group_cargo_en { get; set; }
            public int inp_id_type_cargo { get; set; }
            public string inp_type_cargo_ru { get; set; }
            public string inp_type_cargo_en { get; set; }
            public int inp_id_cargo { get; set; }
            public string inp_cargo_ru { get; set; }
            public string inp_cargo_en { get; set; }
            public string inp_cargo_full_ru { get; set; }
            public string inp_cargo_full_en { get; set; }
            public int inp_cargo_etsng { get; set; }
            public string inp_material_code_sap { get; set; }
            public string inp_material_name_sap { get; set; }
            public string inp_station_shipment { get; set; }
            public string inp_station_shipment_code_sap { get; set; }
            public string inp_station_shipment_name_sap { get; set; }
            public int inp_consignee { get; set; }
            public int? inp_id_consignee { get; set; }
            public string inp_consignee_name_ru { get; set; }
            public string inp_consignee_name_en { get; set; }
            public string inp_consignee_name_full_ru { get; set; }
            public string inp_consignee_name_full_en { get; set; }
            public string inp_consignee_name_abr_ru { get; set; }
            public string inp_consignee_name_abr_en { get; set; }
            public int? inp_id_shop { get; set; }
            public int? inp_shop_code_amkr { get; set; }
            public string inp_shop_code_sap { get; set; }
            public string inp_shop_name_sap { get; set; }
            public string inp_new_shop_code_sap { get; set; }
            public string inp_new_shop_name_sap { get; set; }
            public bool? inp_permission_unload_sap { get; set; }
            public bool? inp_step1_sap { get; set; }
            public bool? inp_step2_sap { get; set; }
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
            try
            {
                List<StationsNodes> nodes = this.rep_rw.StationsNodes
                    .Where(n => n.id_station_from == id)
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
                e.WriteErrorMethod(String.Format("GetSendStationsNodes(id={0})", id), eventID);
                return InternalServerError(e);
            }
        }

        // GET: api/rw/stations_nodes/arrival/station/1
        [Route("stations_nodes/arrival/station/{id:int}")]
        [ResponseType(typeof(StationsNodes))]
        public IHttpActionResult GetArrivalStationsNodes(int id)
        {
            try
            {
                List<StationsNodes> nodes = this.rep_rw.StationsNodes
                    .Where(n => n.id_station_on == id)
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
                e.WriteErrorMethod(String.Format("GetArrivalStationsNodes(id={0})", id), eventID);
                return InternalServerError(e);
            }
        }


        // GET: api/rw/stations_nodes
        [Route("stations_nodes")]
        [ResponseType(typeof(StationsNodes))]
        public IHttpActionResult GetStationsNodes()
        {
            try
            {
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
            try
            {
                StationsNodes node = this.rep_rw.StationsNodes
                    .Where(n => n.id == id)
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
                    }).FirstOrDefault();
                if (node == null)
                {
                    return NotFound();
                }
                return Json(node);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetStationsNodes(id={0})", id), eventID);
                return InternalServerError(e);
            }
            
            //StationsNodes node = this.rep_rw.GetStationsNodes(id);
            //if (node == null)
            //{
            //    return NotFound();
            //}
            //return Ok(node);
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
                                            //Stations = n.Stations, 
                                            //Stations1 = n.Stations1
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
                                            //Stations = n.Stations, 
                                            //Stations1 = n.Stations1
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
            try
            {
                List<Stations> stations = this.rep_rw.Stations
                    .Where(s => s.view == true & s.station_uz == false)
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
                               //Stations = n.Stations, 
                               //Stations1 = n.Stations1
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
                               //Stations = n.Stations, 
                               //Stations1 = n.Stations1
                           }).ToList(),
                    }).ToList();
                if (stations == null)
                {
                    return NotFound();
                }
                return Json(stations);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetStationsOfViewAMKR()"), eventID);
                return InternalServerError(e);
            }
        }

        // GET: api/rw/stations/view
        [Route("stations/view")]
        [ResponseType(typeof(Stations))]
        public IHttpActionResult GetStationsOfView()
        {
            try
            {
                List<Stations> stations = this.rep_rw.Stations
                    .Where(s => s.view == true)
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
                               //Stations = n.Stations, 
                               //Stations1 = n.Stations1
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
                               //Stations = n.Stations, 
                               //Stations1 = n.Stations1
                           }).ToList(),
                    }).ToList();
                if (stations == null)
                {
                    return NotFound();
                }
                return Json(stations);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetStationsOfViewAMKR()"), eventID);
                return InternalServerError(e);
            }



        }

        // GET: api/rw/stations
        [Route("stations")]
        [ResponseType(typeof(Stations))]
        public IHttpActionResult GetStations()
        {
            try
            {
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

        #region CarConditions
        // GET: api/rw/car/conditions
        [Route("car/conditions")]
        [ResponseType(typeof(CarConditions))]
        public IHttpActionResult GetCarConditions()
        {
            try
            {
                List<CarConditions> car_conditions = this.rep_rw.CarConditions
                    .ToList()
                    .Select(c => new CarConditions
                    {
                        id = c.id,
                        name_ru = c.name_ru,
                        name_en = c.name_en,
                    })
             .ToList();

                if (car_conditions == null)
                {
                    return NotFound();
                }
                return Ok(car_conditions);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetStations()"), eventID);
                return InternalServerError(e);
            }
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

        // GET: api/rw/arrival/sostav/stations/16,15
        [Route("arrival/sostav/stations/{list}")]
        [ResponseType(typeof(ArrivalSostav))]
        public IHttpActionResult GetArrivalSostavOfStationsUZ(string list)
        {
            try
            {
                SqlParameter s_list = new SqlParameter("@stations", list);
                List<ArrivalSostav> arr_cars = this.rep_rw.Database.SqlQuery<ArrivalSostav>("EXEC [RailWay].[GetArrivalSostavOfStationsUZ] @stations", s_list).ToList();
                if (arr_cars == null)
                {
                    return NotFound();
                }
                return Ok(arr_cars);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetArrivalSostavOfStationsUZ(list={0})", list), eventID);
                return InternalServerError(e);
            }
        }


        // GET: api/rw/cars_on_way/station/6
        [Route("cars_on_way/station/{id:int}")]
        [ResponseType(typeof(CarsOnWay))]
        public IHttpActionResult GetCountCarsOnWayOfStation(int id)
        {
            try
            {
                SqlParameter i_id = new SqlParameter("@id", id);
                List<CarsOnWay> arr_cars = this.rep_rw.Database.SqlQuery<CarsOnWay>("EXEC [RailWay].[GetCountCarsOnWayOfStation] @id", i_id).ToList();
                if (arr_cars == null)
                {
                    return NotFound();
                }
                return Ok(arr_cars);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetCountCarsOnWayOfStation(id={0})", id), eventID);
                return InternalServerError(e);
            }
        }

        // GET: api/rw/cars_on_way/way/108/side/0
        [Route("cars_on_way/way/{id:int}/side/{side:int}")]
        [ResponseType(typeof(CarsDetails))]
        public IHttpActionResult GetCarsOnWay(int id, int side)
        {
            try
            {
                SqlParameter i_id = new SqlParameter("@idway", id);
                SqlParameter i_side = new SqlParameter("@side", side);
                List<CarsDetails> arr_cars = this.rep_rw.Database.SqlQuery<CarsDetails>("EXEC [RailWay].[GetCarsOnWay] @idway, @side", i_id, i_side).ToList();
                if (arr_cars == null)
                {
                    return NotFound();
                }
                return Ok(arr_cars);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetCarsOnWay(id={0}, side={1})", id, side), eventID);
                return InternalServerError(e);
            }
        }

        // GET: api/rw/cars_on_way/way/294/arrival/2700/side/0
        [Route("cars_on_way/way/{id:int}/arrival/{id_arrival:int}/side/{side:int}")]
        [ResponseType(typeof(CarsDetails))]
        public IHttpActionResult GetCarsOnWayUZ(int id, int id_arrival, int side)
        {
            try
            {
                SqlParameter i_id = new SqlParameter("@idway", id);
                SqlParameter i_idarrival = new SqlParameter("@idarrival", id_arrival);
                SqlParameter i_side = new SqlParameter("@side", id);
                List<CarsDetails> arr_cars = this.rep_rw.Database.SqlQuery<CarsDetails>("EXEC [RailWay].[GetCarsOnWayUZ] @idway, @idarrival, @side", i_id, i_idarrival, i_side).ToList();
                if (arr_cars == null)
                {
                    return NotFound();
                }
                return Ok(arr_cars);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetCarsOnWay(id={0}, id_arrival={1}, side={2})", id, id_arrival, side), eventID);
                return InternalServerError(e);
            }
        }
    }
}
