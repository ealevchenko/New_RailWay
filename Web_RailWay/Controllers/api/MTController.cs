using EFMT.Abstract;
using EFMT.Concrete;
using EFMT.Entities;
using MessageLog;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
//using System.Web.Mvc;
using WebApiClient;

namespace Web_RailWay.Controllers.api
{
    [RoutePrefix("api/mt")]
    public class MTController : ApiController
    {
        private eventID eventID = eventID.Web_API_MTController;

        protected IMT rep_MT;
        public MTController()
        {
            this.rep_MT = new EFMetallurgTrans();
        }

        protected ApproachesCars CreateCorectApproachesCars(ApproachesCars app_car)
        {
            return new ApproachesCars()
                {
                    ID = app_car.ID,
                    IDSostav = app_car.IDSostav,
                    CompositionIndex = app_car.CompositionIndex,
                    Num = app_car.Num,
                    CountryCode = app_car.CountryCode,
                    Weight = app_car.Weight,
                    CargoCode = app_car.CargoCode,
                    TrainNumber = app_car.TrainNumber,
                    Operation = app_car.Operation,
                    DateOperation = app_car.DateOperation,
                    CodeStationFrom = app_car.CodeStationFrom,
                    CodeStationOn = app_car.CodeStationOn,
                    CodeStationCurrent = app_car.CodeStationCurrent,
                    CountWagons = app_car.CountWagons,
                    SumWeight = app_car.SumWeight,
                    FlagCargo = app_car.FlagCargo,
                    Route = app_car.Route,
                    Owner = app_car.Owner,
                    NumDocArrival = app_car.NumDocArrival,
                    Arrival = app_car.Arrival,
                    ParentID = app_car.ParentID,
                    ApproachesSostav = null,
                    UserName = app_car.UserName,
                };
        }

        public class awasArrivalCars
        {
            //            ac.[ID] as [id_oper],
            //ac.[Position] as num_vag_on_way,
            //ac.[Num] as num,
            //v.rod, 
            //o.abr as owner_, 
            //c.name as country, 
            //rcountry.Country as wagon_country,
            //cond = null,
            //ac.[Cargo] as gruz,
            //rcargo.Name as CargoName,
            //shop = null,
            //cond2 = N'ожидаем прибытия с УЗ',
            //ac.[DateOperation] as dt_uz,
            //dt_amkr = null,
            //v.st_otpr, --  ?
            //gruz_amkr = null,
            //ac.[Weight] as weight_gruz,
            //p.date_mail, 
            //p.n_mail, 
            //p.[text], 
            //p.nm_stan, 
            //p.nm_sobstv,
            //gdstait = null,
            //note = null,
            //nazn_country = null,
            //tupik = null,
            //grvu_SAP = null,
            //ngru_SAP = null,
            //ac.[DateOperation] as dt_on_way,
            //sapis.NumNakl,
            //sapis.WeightDoc,
            //sapis.DocNumReweighing,
            //sapis.DocDataReweighing,
            //sapis.WeightReweighing,
            //sapis.DateTimeReweighing,
            //sapis.CodeMaterial,
            //sapis.NameMaterial,
            //sapis.CodeStationShipment,
            //sapis.NameStationShipment,
            //sapis.CodeShop,
            //sapis.NameShop,
            //sapis.CodeNewShop,
            //sapis.NameNewShop,
            //sapis.PermissionUnload

            public int id_oper { get; set; }
            public DateTime? dt_amkr { get; set; }
            public int? n_natur { get; set; }
            //public int? id_vagon { get; set; }
            //public int? id_stat { get; set; }
            //public DateTime? dt_from_stat { get; set; }
            //public DateTime? dt_on_stat { get; set; }
            //public int? id_way { get; set; }
            //public DateTime? dt_from_way { get; set; }
            public DateTime? dt_on_way { get; set; }
            public int? num_vag_on_way { get; set; }
            //public int? is_present { get; set; }
            //public int? id_locom { get; set; }
            //public int? id_locom2 { get; set; }
            //public int? id_cond2 { get; set; }
            //public int? id_gruz { get; set; }
            //public int? id_gruz_amkr { get; set; }
            //public int? id_shop_gruz_for { get; set; }
            public Single weight_gruz { get; set; }
            //public int? id_tupik { get; set; }
            //public int? id_nazn_country { get; set; }
            //public int? id_gdstait { get; set; }
            //public int? id_cond { get; set; }
            public string note { get; set; }
            //public int? is_hist { get; set; }
            //public int? id_oracle { get; set; }
            //public int? lock_id_way { get; set; }
            //public int? lock_order { get; set; }
            //public int? lock_side { get; set; }
            //public int? lock_id_locom { get; set; }
            //public int? st_lock_id_stat { get; set; }
            //public int? st_lock_order { get; set; }
            //public int? st_lock_train { get; set; }
            //public int? st_lock_side { get; set; }
            //public int? st_gruz_front { get; set; }
            //public int? st_shop { get; set; }
            //public int? oracle_k_st { get; set; }
            //public int? st_lock_locom1 { get; set; }
            //public int? st_lock_locom2 { get; set; }
            //public int? id_oper_parent { get; set; }
            public string grvu_SAP { get; set; }
            public string ngru_SAP { get; set; }
            //public int? id_ora_23_temp { get; set; }
            //public string edit_user { get; set; }
            //public DateTime? edit_dt { get; set; }
            public int? IDSostav { get; set; }
            //public int? num_vagon { get; set; }
            public DateTime? dt_uz { get; set; }
            //public DateTime? dt_out_amkr { get; set; }
            public string owner_ { get; set; }
            public string country { get; set; }
            public string cond { get; set; }
            public string cond2 { get; set; }
            //public int? id_cond_after { get; set; }
            public string gruz { get; set; }
            public string gruz_amkr { get; set; }
            public int? num { get; set; }
            public string rod { get; set; }
            public string st_otpr { get; set; }
            public string shop { get; set; }
            public string tupik { get; set; }
            public string gdstait { get; set; }
            public string nazn_country { get; set; }
            public DateTime? date_mail { get; set; }
            public string n_mail { get; set; }
            public string text { get; set; }
            public string nm_stan { get; set; }
            public string nm_sobstv { get; set; }
            public string NumNakl { get; set; }
            //public int? CountryCode { get; set; }
            public string wagon_country { get; set; }
            public decimal? WeightDoc { get; set; }
            public int? DocNumReweighing { get; set; }
            public DateTime? DocDataReweighing { get; set; }
            public decimal? WeightReweighing { get; set; }
            public DateTime? DateTimeReweighing { get; set; }
            //public int? PostReweighing { get; set; }
            //public int? CodeCargo { get; set; }
            public string CargoName { get; set; }
            public string CodeMaterial { get; set; }
            public string NameMaterial { get; set; }
            public string CodeStationShipment { get; set; }
            public string NameStationShipment { get; set; }
            public string CodeShop { get; set; }
            public string NameShop { get; set; }
            public string CodeNewShop { get; set; }
            public string NameNewShop { get; set; }
            public bool? PermissionUnload { get; set; }
            //public bool? Step1 { get; set; }
            //public bool? Step2 { get; set; }
        }

        public class DBWagonsTracking
        {
            public int id { get; set; }
            public int nvagon { get; set; }
            public string type_car_ru { get; set; }
            public string type_car_en { get; set; }
            public DateTime? dt { get; set; }
            public int? st_disl { get; set; }
            public string nst_disl { get; set; }
            public int? kodop { get; set; }
            public string nameop { get; set; }
            public string full_nameop { get; set; }
            public int? st_form { get; set; }
            public string nst_form { get; set; }
            public int? idsost { get; set; }
            public string nsost { get; set; }
            public string index { get; set; }
            public int? st_nazn { get; set; }
            public string nst_nazn { get; set; }
            public int? ntrain { get; set; }
            public int? st_end { get; set; }
            public string nst_end { get; set; }
            public int? kgr { get; set; }
            public string nkgr { get; set; }
            public int id_cargo { get; set; }
            public string cargo_ru { get; set; }
            public string cargo_en { get; set; }
            public string type_cargo_ru { get; set; }
            public string type_cargo_en { get; set; }
            public string group_cargo_ru { get; set; }
            public string group_cargo_en { get; set; }
            public int? kgrp { get; set; }
            public decimal? ves { get; set; }
            public DateTime? updated { get; set; }
            public int? kgro { get; set; }
            public int? km { get; set; }
        }

        public class CurentWagonTrackingAndDateTime
        {
            public DateTime? dt_last  { get; set; }            
            public List<CurentWagonTracking> list  { get; set; }
        }

        #region WT

        // GET: api/mt/wagons_tracking
        [Route("wagons_tracking")]
        public IHttpActionResult GetWagonsTracking()
        {
            WebApiClientMetallurgTrans client = new WebApiClientMetallurgTrans();
            string result = client.GetJSONWagonsTracking();
            if (result == null || result.ToList().Count() == 0)
            {
                return NotFound();
            }
            return Ok(JsonConvert.DeserializeObject(result));
        }

        // GET: api/mt/wagons_tracking_arhiv
        [Route("wagons_tracking_arhiv")]
        [ResponseType(typeof(DBWagonsTracking))]
        public IHttpActionResult GetDBWagonsTracking()
        {
            try
            {
                List<DBWagonsTracking> arr_cars = this.rep_MT.Database.SqlQuery<DBWagonsTracking>("EXEC [MT].[GetDBWagonsTracking]").ToList();
                if (arr_cars == null)
                {
                    return NotFound();
                }
                return Ok(arr_cars);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetDBWagonsTracking()"), eventID);
                return InternalServerError(e);
            }
        }

        // GET: api/mt/wagons_tracking_arhiv/report/2
        [Route("wagons_tracking_arhiv/report/{id_report:int}")]
        [ResponseType(typeof(DBWagonsTracking))]
        public IHttpActionResult GetDBWagonsTrackingOfCarsReports(int id_report)
        {
            try
            {
                SqlParameter i_id_report = new SqlParameter("@idreport", id_report);
                List<DBWagonsTracking> arr_cars = this.rep_MT.Database.SqlQuery<DBWagonsTracking>("EXEC [MT].[GetDBWagonsTrackingOfCarsReports] @idreport", i_id_report).ToList();
                if (arr_cars == null)
                {
                    return NotFound();
                }
                return Ok(arr_cars);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetDBWagonsTrackingOfCarsReports(id_report={0})", id_report), eventID);
                return InternalServerError(e);
            }
        }

        // GET: api/mt/wagons_tracking_arhiv/car/52921004/start/2018-03-29T00:00:00.000Z/stop/2018-03-29T23:59:59.000Z
        [Route("wagons_tracking_arhiv/car/{num:int}/start/{start:datetime}/stop/{stop:datetime}")]
        [ResponseType(typeof(DBWagonsTracking))]
        public IHttpActionResult GetDBCarWagonsTracking(int num, DateTime start, DateTime stop)
        {
            try
            {
                SqlParameter i_num = new SqlParameter("@num", num);
                SqlParameter dt_start = new SqlParameter("@start", start);
                SqlParameter dt_stop = new SqlParameter("@stop", stop);
                List<DBWagonsTracking> arr_cars = this.rep_MT.Database.SqlQuery<DBWagonsTracking>("EXEC [MT].[GetDBCarWagonsTracking] @num, @start, @stop", i_num, dt_start, dt_stop).ToList();
                if (arr_cars == null)
                {
                    return NotFound();
                }
                return Ok(arr_cars);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetDBCarWagonsTracking(num={0}, start={1}, stop={2})", num, start, stop), eventID);
                return InternalServerError(e);
            }
        }

        // GET: api/mt/wagons_tracking_arhiv/last/report/2/start/2018-04-01T00:00:00.000Z/stop/2018-04-30T23:59:59.000Z
        [Route("wagons_tracking_arhiv/last/report/{id_report:int}/start/{start:datetime}/stop/{stop:datetime}")]
        [ResponseType(typeof(CurentWagonTracking))]
        public IHttpActionResult GetLastWagonTrackingOfReports(int id_report, DateTime start, DateTime stop)
        {
            try
            {
                List<CurentWagonTracking> list = this.rep_MT.GetLastWagonTrackingOfReports(id_report,start,stop);
                if (list == null)
                {
                    return NotFound();
                }
                return Ok(list);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetLastWagonTrackingOfReports(id_report={0}, start={1}, stop={2})", id_report, start, stop), eventID);
                return InternalServerError(e);
            }
        }

        // GET: api/mt/wagons_tracking_arhiv/last_dt/report/2/start/2018-04-01T00:00:00.000Z/stop/2018-04-30T23:59:59.000Z
        [Route("wagons_tracking_arhiv/last_dt/report/{id_report:int}/start/{start:datetime}/stop/{stop:datetime}")]
        [ResponseType(typeof(CurentWagonTrackingAndDateTime))]
        public IHttpActionResult GetLastWagonTrackingDTOfReports(int id_report, DateTime start, DateTime stop)
        {
            try
            {
                List<CurentWagonTracking> list = this.rep_MT.GetLastWagonTrackingOfReports(id_report,start,stop);
                WagonsTracking wt = this.rep_MT.WagonsTracking.Where(w=>w.dt <= stop).OrderByDescending(w=>w.dt).FirstOrDefault();
                if (list == null)
                {
                    return NotFound();
                }
                
                return Ok(new CurentWagonTrackingAndDateTime() { list = list, dt_last = wt !=null ? wt.dt : null});
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetLastWagonTrackingOfReports(id_report={0}, start={1}, stop={2})", id_report, start, stop), eventID);
                return InternalServerError(e);
            }
        }


        // GET: api/mt/wagons_tracking/reports
        [Route("wagons_tracking/reports")]
        [ResponseType(typeof(WTReports))]
        public IHttpActionResult GetWTReports()
        {
            try
            {
                List<WTReports> list = this.rep_MT.WTReports
                    .ToList()
                    .Select(r => new WTReports
                    {
                        id = r.id,
                        Report = r.Report,
                        Description = r.Description
                    }).ToList();
                if (list == null)
                {
                    return NotFound();
                }
                return Ok(list);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetWTReports()"), eventID);
                return InternalServerError(e);
            }
        }
        #endregion

        #region Arrival

        // GET: api/mt/arrival/cars/station_uz/4670
        [Route("arrival/cars/station_uz/{code:int?}")]
        [ResponseType(typeof(CountCarsOfSostav))]
        public IHttpActionResult GetNoCloseArrivalCarsOfStationUZ(int code)
        {
            List<CountCarsOfSostav> cars = this.rep_MT.GetNoCloseArrivalCarsOfStationUZ(code);
            if (cars == null || cars.ToList().Count() == 0)
            {
                return NotFound();
            }
            return Ok(cars);
        }

        // GET: api/mt/arrival/cars/sostav/7109/close/True
        [Route("arrival/cars/sostav/{id_sostav:int}/close/{close:bool}")]
        [ResponseType(typeof(awasArrivalCars))]
        public IHttpActionResult GetArrivalCarsOfSostav(int id_sostav, bool close)
        {
            try
            {
                SqlParameter i_id = new SqlParameter("@idsostav", id_sostav);
                SqlParameter b_close = new SqlParameter("@close", close);
                List<awasArrivalCars> arr_cars = this.rep_MT.Database.SqlQuery<awasArrivalCars>("EXEC [MT].[GetArrivalCarsOfSostav] @idsostav, @close", i_id, b_close).ToList();
                if (arr_cars == null)
                {
                    return NotFound();
                }
                return Ok(arr_cars);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetArrivalCarsOfSostav(id_sostav={0}, close={1})", id_sostav, close), eventID);
                return InternalServerError(e);
            }
        }
        #endregion

        #region Approaches

        // GET: api/mt/approaches/sostav
        //[Route("approaches/sostav")]
        //public IEnumerable<ApproachesSostav> GetApproachesSostav()
        //{
        //    return this.rep_MT.GetApproachesSostav();
        //}

        // GET: api/mt/approaches/sostav/id/12
        [Route("approaches/sostav/id/{id:int?}")]
        [ResponseType(typeof(ApproachesSostav))]
        public IHttpActionResult GetApproachesSostav(int id)
        {
            ApproachesSostav app_sostav = this.rep_MT.GetApproachesSostav(id); ;
            if (app_sostav == null)
            {
                return NotFound();
            }
            List<ApproachesCars> cars = new List<ApproachesCars>();
            app_sostav.ApproachesCars.ToList().ForEach(c => cars.Add(CreateCorectApproachesCars(c)));
            app_sostav.ApproachesCars = cars.ToArray();
            return Ok(app_sostav);
        }

        // GET: api/mt/approaches/cars/id/12
        [Route("approaches/cars/id/{id:int?}")]
        [ResponseType(typeof(ApproachesCars))]
        public IHttpActionResult GetApproachesCars(int id)
        {
            ApproachesCars cars = this.rep_MT.GetApproachesCars(id);
            if (cars == null)
            {
                return NotFound();
            }
            cars.ApproachesSostav = null;
            ApproachesCars new_cars = CreateCorectApproachesCars(cars);
            return Ok(new_cars);
        }
        #endregion



    }
}
