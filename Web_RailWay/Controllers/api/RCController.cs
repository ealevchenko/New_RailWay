using EFRC.Abstract;
using EFRC.Concrete;
using EFRC.Entities;
using MessageLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Web_RailWay.Controllers.api
{
    [RoutePrefix("api/rc")]
    public class RCController : ApiController
    {
        private eventID eventID = eventID.Web_API_RCController;
        protected IRC rep_rc;
        public RCController()
        {
            this.rep_rc = new EFRailCars();  
        }

        public class WaysStation { 

            public int id_way {get;set;}
            public string num {get;set;}
            public string name {get;set;}
            public int? vag_capacity {get;set;}
            public int? bind_id_cond {get;set;}
            public int? vag_amount {get;set;}
            public string cond_name {get;set;}
            public int? id_cond_after {get;set;}
        }

        public class ShopsStation {

            public int id_shop { get; set; }
            public string name {get;set;}
            public int? vag_amount { get; set; }
        }

        public class WagonOverturnsStation {

            public int id_gruz_front { get; set; }
            public string name {get;set;}
            public int? vag_amount { get; set; }
        }

        public class ArrivalAMKRStation {
            public int id_stat { get; set; }
            public string stat {get;set;}
            public int st_lock_id_stat { get; set; }
            public int st_lock_train { get; set; }
            public string dt_from_stat { get; set; }
            public int? vag_amount { get; set; }
        }

        public class ArrivalAMKRStation1 {
            public int id_stat { get; set; }
            public string stat {get;set;}
            public int st_lock_id_stat { get; set; }
            public int st_lock_train { get; set; }
            public DateTime dt_from_stat { get; set; }
            public int? vag_amount { get; set; }
        }

        public class SendingStation
        {
            public int st_lock_id_stat { get; set; }
            public string stat {get;set;}
            public int id_stat { get; set; }
            public int st_lock_train { get; set; }
            public DateTime dt_from_stat { get; set; }
            public int? vag_amount { get; set; }
        }

        public class CarInfo { 
            public int id_oper {get;set;}
            public DateTime? dt_amkr {get;set;}
            public int? n_natur {get;set;}
            public int? id_vagon {get;set;}
            public int? id_stat {get;set;}
            public DateTime? dt_from_stat {get;set;}
            public DateTime? dt_on_stat {get;set;}
            public int? id_way {get;set;}
            public DateTime? dt_from_way {get;set;}
            public DateTime? dt_on_way {get;set;}
            public int? num_vag_on_way {get;set;}
            public int? is_present {get;set;}
            public int? id_locom {get;set;}
            public int? id_locom2 {get;set;}
            public int? id_cond2 {get;set;}
            public int? id_gruz {get;set;}
            public int? id_gruz_amkr {get;set;}
            public int? id_shop_gruz_for {get;set;}
            public decimal? weight_gruz {get;set;}
            public int? id_tupik {get;set;}
            public int? id_nazn_country {get;set;}
            public int? id_gdstait {get;set;}
            public int? id_cond {get;set;}
            public string note {get;set;}
            public int? is_hist {get;set;}
            public int? id_oracle {get;set;}
            public int? lock_id_way {get;set;}
            public int? lock_order {get;set;}
            public int? lock_side {get;set;}
            public int? lock_id_locom {get;set;}
            public int? st_lock_id_stat {get;set;}
            public int? st_lock_order {get;set;}
            public int? st_lock_train {get;set;}
            public int? st_lock_side {get;set;}
            public int? st_gruz_front {get;set;}
            public int? st_shop {get;set;}
            public int? oracle_k_st {get;set;}
            public int? st_lock_locom1 {get;set;}
            public int? st_lock_locom2 {get;set;}
            public int? id_oper_parent {get;set;}
            public string grvu_SAP {get;set;}
            public string ngru_SAP {get;set;}
            public int? id_ora_23_temp {get;set;}
            public string edit_user {get;set;}
            public DateTime? edit_dt {get;set;}
            public int? IDSostav {get;set;}
            public int? num_vagon {get;set;}
            public DateTime? dt_uz {get;set;}
            public DateTime? dt_out_amkr {get;set;}
            public string owner_ {get;set;}
            public string country {get;set;}
            public string cond {get;set;}
            public string cond2 {get;set;}
            public int? id_cond_after {get;set;}
            public string gruz {get;set;}
            public string gruz_amkr {get;set;}
            public int? num {get;set;}
            public string rod {get;set;}
            public string st_otpr {get;set;}
            public string shop {get;set;}
            public string tupik {get;set;}
            public string gdstait {get;set;}
            public string nazn_country {get;set;}
            public DateTime? date_mail {get;set;}
            public string n_mail {get;set;}
            public string text {get;set;}
            public string nm_stan {get;set;}
            public string nm_sobstv {get;set;}
            public string NumNakl {get;set;}
            public int? CountryCode {get;set;}
            public string wagon_country {get;set;}
            public decimal? WeightDoc {get;set;}
            public int? DocNumReweighing {get;set;}
            public DateTime?  DocDataReweighing {get;set;}
            public decimal? WeightReweighing {get;set;}
            public DateTime? DateTimeReweighing {get;set;}
            public int? PostReweighing {get;set;}
            public int? CodeCargo {get;set;}
            public string CargoName {get;set;}
            public string CodeMaterial {get;set;}
            public string NameMaterial {get;set;}
            public string CodeStationShipment {get;set;}
            public string NameStationShipment {get;set;}
            public string CodeShop {get;set;}
            public string NameShop {get;set;}
            public string CodeNewShop {get;set;}
            public string NameNewShop {get;set;}
            public bool? PermissionUnload {get;set;}
            public bool? Step1 {get;set;}
            public bool? Step2 {get;set;}
        }

        protected SHOPS CreateSHOPS(SHOPS shops)
        {
            return new SHOPS()
            {
                id_shop = shops.id_shop,
                name = shops.name,
                name_full = shops.name_full,
                id_stat = shops.id_stat,
                id_ora = shops.id_ora,
            };
        }

        protected ArrivalAMKRStation1 CreateArrivalAMKRStation(ArrivalAMKRStation aas)
        {
            return new ArrivalAMKRStation1()
            {
                id_stat = aas.id_stat,
                stat = aas.stat,
                st_lock_id_stat = aas.st_lock_id_stat,
                st_lock_train = aas.st_lock_train,
                dt_from_stat = DateTime.Parse(aas.dt_from_stat),
                vag_amount = aas.vag_amount,
            };
        }

        // GET: api/rc/ways/station/4/rospusk/false
        [Route("ways/station/{id:int}/rospusk/{rosp:bool}")]
        [ResponseType(typeof(WaysStation))]
        public IHttpActionResult GetWaysOfStation(int id, bool rosp)
        {
            try
            {
                SqlParameter i_id = new SqlParameter("@idstation", id);
                SqlParameter b_rosp = new SqlParameter("@rospusk", rosp);
                List<WaysStation> ws = this.rep_rc.Database.SqlQuery<WaysStation>("EXEC [RailCars].[GetWays] @idstation, @rospusk", i_id, b_rosp).ToList();
                if (ws == null)
                {
                    return NotFound();
                }
                return Ok(ws);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetWaysOfStation(id={0}, rosp={1})", id, rosp), eventID);
                return InternalServerError(e);
            }
        }

        // GET: api/rc/shops/station/5/cars
        [Route("shops/station/{id:int}/cars")]
        [ResponseType(typeof(ShopsStation))]
        public IHttpActionResult GetShopsCarsOfStation(int id)
        {
            try
            {
                SqlParameter i_id = new SqlParameter("@idstation", id);
                List<ShopsStation> ss = this.rep_rc.Database.SqlQuery<ShopsStation>("EXEC [RailCars].[GetShop] @idstation", i_id).ToList();
                if (ss == null)
                {
                    return NotFound();
                }
                return Ok(ss);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetShopsOfStation(id={0})", id), eventID);
                return InternalServerError(e);
            }
        }

        // GET: api/rc/wagon_overturns/station/5/cars
        [Route("wagon_overturns/station/{id:int}/cars")]
        [ResponseType(typeof(WagonOverturnsStation))]
        public IHttpActionResult GetWagonOverturnsCarsOfStation(int id)
        {
            try
            {
                SqlParameter i_id = new SqlParameter("@idstation", id);
                List<WagonOverturnsStation> ss = this.rep_rc.Database.SqlQuery<WagonOverturnsStation>("EXEC [RailCars].[GetGruzFront] @idstation", i_id).ToList();
                if (ss == null)
                {
                    return NotFound();
                }
                return Ok(ss);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetWagonOverturnsOfStation(id={0})", id), eventID);
                return InternalServerError(e);
            }
        }

        // GET: api/rc/arrival_amkr/station/4/cars
        [Route("arrival_amkr/station/{id:int}/cars")]
        [ResponseType(typeof(ArrivalAMKRStation1))]
        public IHttpActionResult GetArrivalAMKRCarsOfStation(int id)
        {
            try
            {
                int type = 0;
                SqlParameter i_id = new SqlParameter("@idstation", id);
                SqlParameter i_type = new SqlParameter("@type", type);
                List<ArrivalAMKRStation> ss = this.rep_rc.Database.SqlQuery<ArrivalAMKRStation>("EXEC [RailCars].[GetAdmissTrains] @idstation, @type", i_id, i_type).ToList();
                if (ss == null)
                {
                    return NotFound();
                }
                List<ArrivalAMKRStation1> list = new List<ArrivalAMKRStation1>();
                ss.ForEach(s=>list.Add(CreateArrivalAMKRStation(s)));
                return Ok(list);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetArrivalAMKRCarsOfStation(id={0})", id), eventID);
                return InternalServerError(e);
            }
        }

        // GET: api/rc/sending/station/4/cars
        [Route("sending/station/{id:int}/cars")]
        [ResponseType(typeof(SendingStation))]
        public IHttpActionResult GetSendingCarsOfStation(int id)
        {
            try
            {
                SqlParameter i_id = new SqlParameter("@idstation", id);
                List<SendingStation> ss = this.rep_rc.Database.SqlQuery<SendingStation>("EXEC [RailCars].[GetRemoveTrains] @idstation", i_id).ToList();
                if (ss == null)
                {
                    return NotFound();
                }
                return Ok(ss);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetSendingCarsOfStation(id={0})", id), eventID);
                return InternalServerError(e);
            }
        }

        // GET: api/rc/cars/way/52/side/0
        [Route("cars/way/{id:int}/side/{side:int}")]
        [ResponseType(typeof(CarInfo))]
        public IHttpActionResult GetCarsOfWay(int id, int side)
        {
            try
            {
                SqlParameter i_id = new SqlParameter("@idway", id);
                SqlParameter i_side = new SqlParameter("@side", side);
                List<CarInfo> ws = this.rep_rc.Database.SqlQuery<CarInfo>("EXEC [RailCars].[GetOnStatWagons] @idway, @side", i_id, i_side).ToList();
                if (ws == null)
                {
                    return NotFound();
                }
                return Ok(ws);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetCarsOfWay(id={0}, side={1})", id, side), eventID);
                return InternalServerError(e);
            }
        }

        // GET: api/rc/cars/wagon_overturn/2
        [Route("cars/wagon_overturn/{id:int}")]
        [ResponseType(typeof(CarInfo))]
        public IHttpActionResult GetCarsOfWagonOverturns(int id)
        {
            try
            {
                SqlParameter i_id = new SqlParameter("@idwo", id);
                List<CarInfo> ws = this.rep_rc.Database.SqlQuery<CarInfo>("EXEC [RailCars].[GetOnStatGruzFrontWagons] @idwo", i_id).ToList();
                if (ws == null)
                {
                    return NotFound();
                }
                return Ok(ws);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetCarsOfWagonOverturns(id={0})", id), eventID);
                return InternalServerError(e);
            }
        }

        // GET: api/rc/cars/shop/2
        [Route("cars/shop/{id:int}")]
        [ResponseType(typeof(CarInfo))]
        public IHttpActionResult GetCarsOfShop(int id)
        {
            try
            {
                SqlParameter i_id = new SqlParameter("@idshop", id);
                List<CarInfo> ws = this.rep_rc.Database.SqlQuery<CarInfo>("EXEC [RailCars].[GetOnStatShopWagons] @idshop", i_id).ToList();
                if (ws == null)
                {
                    return NotFound();
                }
                return Ok(ws);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetCarsOfShop(id={0})", id), eventID);
                return InternalServerError(e);
            }
        }

        // GET: api/rc/arrival_amkr/4/30697/2018-02-11T08:26:05/0
        [Route("arrival_amkr/{station:int}/{train:int}/{dt:datetime}/{side:int}")]
        [ResponseType(typeof(CarInfo))]
        public IHttpActionResult GetCarsOfArrivalAMKR(int station, int train, DateTime dt, int side)
        {
            try
            {
                SqlParameter i_station = new SqlParameter("@idstation", station);
                SqlParameter i_train = new SqlParameter("@trainNum", train);
                SqlParameter d_dt = new SqlParameter("@dt", dt);
                SqlParameter i_shop = new SqlParameter();
                i_shop.ParameterName = "@shop";
                i_shop.SqlDbType = SqlDbType.Int;
                i_shop.SqlValue = DBNull.Value;
                SqlParameter i_gf = new SqlParameter();
                i_gf.ParameterName = "@gf";
                i_gf.SqlDbType = SqlDbType.Int;
                i_gf.SqlValue = DBNull.Value;
                SqlParameter i_side = new SqlParameter("@side", side);
                List<CarInfo> ws = this.rep_rc.Database.SqlQuery<CarInfo>("EXEC [RailCars].[GetAdmissWagons] @idstation, @trainNum, @dt, @shop, @gf, @side", i_station, i_train, d_dt, i_shop, i_gf, i_side).ToList();
                if (ws == null)
                {
                    return NotFound();
                }
                return Ok(ws);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetCarsOfArrivalAMKR(station={0}, train={1}, dt={2}, side={3})", station, train, dt, side), eventID);
                return InternalServerError(e);
            }
        }

        // GET: api/rc/shops/station/5
        [Route("shops/station/{id:int}")]
        [ResponseType(typeof(SHOPS))]
        public IHttpActionResult GetShopsOfStation(int id)
        {
            try
            {
                List<SHOPS> list = this.rep_rc.GetShopsOfStation(id).ToList();
                if (list == null)
                {
                    return NotFound();
                }
                List<SHOPS> list_correct = new List<SHOPS>();
                list.ForEach(s => list_correct.Add(CreateSHOPS(s)));
                return Ok(list_correct);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetCarsOfShop(id={0})", id), eventID);
                return InternalServerError(e);
            }
        }

        // GET: api/rc/wagon_overturns/station/5
        [Route("wagon_overturns/station/{id:int}")]
        [ResponseType(typeof(GRUZ_FRONTS))]
        public IHttpActionResult GetWagonOverturnsOfStation(int id)
        {
            try
            {
                List<GRUZ_FRONTS> list = this.rep_rc.GetGRUZ_FRONTSOfStation(id).ToList();
                if (list == null)
                {
                    return NotFound();
                }
                return Ok(list);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetWagonOverturnsOfStation(id={0})", id), eventID);
                return InternalServerError(e);
            }
        }

    }
}
