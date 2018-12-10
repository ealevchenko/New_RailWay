using EFMT.Entities;
using EFRW.Concrete;
using EFRW.Concrete.EFCars;
using EFRW.Entities;
using MessageLog;
using libClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW
{
    
    /// <summary>
    /// Класс Вагоны систмы RailWay
    /// </summary>
    public class RWCars
    {
        private eventID eventID = eventID.RW_RWCars;
        private service servece_owner;
        private EFDbContext db;
        private bool log_detali = false;


        public RWCars()
        {
            this.db = new EFDbContext();
            this.servece_owner = service.Null;
        }

        public RWCars(EFDbContext db)
        {
            this.db = db;
            this.servece_owner = service.Null;
        }

        public RWCars(service servece_owner)
        {
            this.servece_owner = servece_owner;
            this.db = new EFDbContext();
        }

        public RWCars(EFDbContext db, service servece_owner)
        {
            this.servece_owner = servece_owner;
            this.db = db;
        }

        #region CarOperations ОПЕРАЦИИ НАД ВАГОНАМИ
        /// <summary>
        /// Вернуть последнюю открытую операцию
        /// </summary>
        /// <param name="num_car"></param>
        /// <returns></returns>
        public CarOperations GetOpenOperation(int num_car)
        {
            try
            {
                EFCarOperations ef_operation = new EFCarOperations(this.db);
                return  ef_operation.Get().Where(o => o.CarsInternal.num == num_car).GetLastOpenOperation();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetOpenOperation(num_car={0})", num_car), servece_owner, eventID);
                return null;
            }
        }
        /// <summary>
        /// Вернуть последнюю операцию
        /// </summary>
        /// <param name="num_car"></param>
        /// <returns></returns>
        public CarOperations GetLastOperation(int num_car)
        {
            try
            {
                EFCarOperations ef_operation = new EFCarOperations(this.db);
                List<CarOperations> list_open_operation = ef_operation.Get().Where(o => o.CarsInternal.num == num_car).ToList();
                if (list_open_operation == null) return null;
                return list_open_operation.GetLastOperation();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetLastOperation(num_car={0})", num_car), servece_owner, eventID);
                return null;
            }
        }
        /// <summary>
        /// Показать операцию по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CarOperations GetOperation(int id)
        {
            try
            {
                EFCarOperations ef_operation = new EFCarOperations(this.db);
                return ef_operation.Get(id);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetOperation(id={0})", id), servece_owner, eventID);
                return null;
            }
        }

        /// <summary>
        /// Выполнить операцию транзит УЗ
        /// </summary>
        /// <param name="current_operation"></param>
        /// <param name="dt_inp"></param>
        /// <returns></returns>
        public int CarOperationTransitUZ(CarOperations current_operation, DateTime dt_inp) {
            try
            {
                if (current_operation == null) return 0; // Текущая операция не определена
                RWDirectory rw_directory = new RWDirectory(this.db, servece_owner);
                // Это станция УЗ
                Directory_Ways way_current = rw_directory.GetWay((int)current_operation.id_way);
                if (way_current == null) return -1; // Текущий путь не определен
                Directory_InternalStations is_current = rw_directory.GetInternalStations(way_current.id_station);
                if (is_current == null) return -1; // Текущая станция не определена
                if (is_current != null && is_current.station_uz)
                {
                    Directory_Ways way_send = rw_directory.GetWaysOfStation(is_current.id, "2"); // Путь транзита на АМКР
                    return CreateCarOperations(current_operation.id_car_internal, current_operation.id_car_conditions, 17, dt_inp, way_send.id, (int)current_operation.position, current_operation.train1, current_operation.train2, null, current_operation.id);
                }
                else return  -1; // Текущая станция не УЗ
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CarOperationTransitUZ(current_operation={0}, dt_inp={1})",
                    current_operation, dt_inp), servece_owner, eventID);
                return -1;
            }
        }

        /// <summary>
        /// Добавить новую операцию
        /// </summary>
        /// <param name="id_car_internal"></param>
        /// <param name="id_car_conditions"></param>
        /// <param name="id_car_status"></param>
        /// <param name="dt_inp"></param>
        /// <param name="id_way"></param>
        /// <param name="position"></param>
        /// <param name="train1"></param>
        /// <param name="train2"></param>
        /// <param name="side"></param>
        /// <returns></returns>
        public int CreateCarOperations(int id_car_internal, int? id_car_conditions, int? id_car_status, DateTime dt_inp, int id_way, int position, int? train1, int? train2, int? side, int? parent_id)
        {
            try
            {
                EFCarOperations co = new EFCarOperations(this.db);
                // Закроем старую 
                if (parent_id!=null) {
                    CarOperations old_co = co.Get((int)parent_id);
                    old_co.dt_out = dt_inp;
                    co.Update(old_co);
                }
                // Создадим новую
                CarOperations new_co = new CarOperations()
                {
                    id_car_internal = id_car_internal,
                    id_car_conditions = id_car_conditions,
                    id_car_status = id_car_status,
                    dt_inp = dt_inp,
                    id_way = id_way,
                    position = position,
                    train1 = train1,
                    train2 = train2,
                    side = side,
                    parent_id = parent_id,
                };
                co.Add(new_co);
                co.Save();
                return new_co.id;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CreateCarOperations(id_car_internal={0}, id_car_conditions={1}, id_car_status={2}, dt_inp={3}, id_way={4}, position={5}, train1={6}, train2={7}, side={8}, parent_id={9})",
                    id_car_internal, id_car_conditions, id_car_status, dt_inp, id_way, position, train1, train2,side, parent_id), servece_owner, eventID);
                return -1;
            }
        }

        #endregion

        #region CarsInternal - ВНУТРЕНЕЕ ПЕРЕМЕЩЕНИЕ ВАГОНОВ
        public int NewCarsInternal(ArrivalCars car_mt, int? parent_id)
        {
            try
            {
                RWDirectory rw_directory = new RWDirectory(this.db, servece_owner);
                Directory_InternalStations internal_stations = rw_directory.GetInternalStationsUZ(car_mt.CompositionIndex, true);
                if (internal_stations != null)
                {
                    Directory_Ways way_uz = rw_directory.GetWaysOfStation(internal_stations.id, "1"); // Путь отправки на АМКР
                    if (way_uz != null)
                    {
                        return NewCarsInternal(car_mt.IDSostav,
                            car_mt.ArrivalSostav.IDArrival,
                            car_mt.Num,
                            car_mt.CompositionIndex,
                            null,
                            15,
                            car_mt.DateOperation,
                            way_uz.id,
                            car_mt.Position, car_mt.TrainNumber, null, null, car_mt.CountryCode, car_mt.CargoCode, car_mt.Weight, car_mt.Consignee, parent_id);
                    }
                }
                // TODO: Доработать возврат кодов ошибок
                return -1;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("NewCarsInternal(car_mt={0},parent_id={1})", car_mt.GetFieldsAndValue(), parent_id), servece_owner, eventID);
                return -1;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_sostav"></param>
        /// <param name="id_arrival"></param>
        /// <param name="num"></param>
        /// <param name="index"></param>
        /// <param name="id_car_conditions"></param>
        /// <param name="id_car_status"></param>
        /// <param name="dt_inp"></param>
        /// <param name="id_way"></param>
        /// <param name="position"></param>
        /// <param name="train1"></param>
        /// <param name="train2"></param>
        /// <param name="side"></param>
        /// <param name="CountryCode"></param>
        /// <param name="CargoCode"></param>
        /// <param name="Weight"></param>
        /// <param name="Consignee"></param>
        /// <param name="parent_id"></param>
        /// <returns></returns>
        public int NewCarsInternal(
            int id_sostav,
            int id_arrival,
            int num,
            string index,
            int? id_car_conditions,
            int? id_car_status,
            DateTime dt_inp,
            int id_way,
            int position,
            int? train1,
            int? train2,
            int? side,
            int CountryCode,
            int CargoCode,
            float Weight,
            int Consignee,
            int? parent_id)
        {

            try
            {
                // Проверим в справочнике вагонов
                RWDirectory rw_directory = new RWDirectory(this.db);
                Directory_Cars car_directory = rw_directory.GetCarsOfNum(num, id_arrival, dt_inp, rw_directory.GetIDCountryOfCodeSNG(CountryCode), null, null, true, true);
                // Создадим новю строку внутренего перемещения вагона
                int new_id_operation = 0;
                int new_delivery = 0;
                int new_id_car_internal = CreateCarsInternal(id_sostav, id_arrival, num, dt_inp, parent_id);
                if (new_id_car_internal > 0)
                {
                    new_id_operation = CreateCarOperations(new_id_car_internal, id_car_conditions, id_car_status, dt_inp, id_way, position, train1, train2, side, null);
                    new_delivery = CreateCarInboundDelivery(new_id_car_internal, num, id_arrival, dt_inp, index, position, CountryCode, CargoCode, Weight, Consignee);
                }
                // TODO: Доработать возврат кодов ошибок
                return new_id_car_internal > 0 && new_id_operation > 0 && new_delivery > 0 ? new_id_car_internal : -1;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("NewCarsInternal(id_sostav={0},id_arrival={1},num={2},index={3},id_car_conditions={4},id_car_status={5},dt_inp={6},id_way={7},position={8},train1={9},train2={10},side={11},CountryCode={12},CargoCode={13},Weight={14},Consignee={15},parent_id={16})",
                    id_sostav, id_arrival, num, index, id_car_conditions, id_car_status, dt_inp, id_way, position, train1, train2, side, CountryCode, CargoCode, Weight, Consignee, parent_id), servece_owner, eventID);
                return -1;
            }
        }
        /// <summary>
        /// Создать новую строку внутренего перемещения вагона.
        /// </summary>
        /// <param name="id_sostav"></param>
        /// <param name="id_arrival"></param>
        /// <param name="num"></param>
        /// <param name="dt_uz"></param>
        /// <param name="parent_id"></param>
        /// <returns></returns>
        public int CreateCarsInternal(int id_sostav, int id_arrival, int num, DateTime dt_uz, int? parent_id) {
            try
            {
                EFCarsInternal ef_ci = new EFCarsInternal(this.db);

                if (parent_id != null) {
                    // получим предыдущее перемещение 
                    CarsInternal old_ci = ef_ci.Get((int)parent_id);
                    // Закроем последнюю операцию предыдущего перемещения
                    CarOperations last_old_operation = old_ci.CarOperations.GetLastOpenOperation();
                    if (last_old_operation != null) {
                        last_old_operation.dt_out = dt_uz;
                    }
                    // закроем предыдущее перемещение
                    old_ci.dt_close = dt_uz;
                    old_ci.user_close = System.Environment.UserDomainName + @"\" + System.Environment.UserName;
                }
                RWDirectory rw_directory = new RWDirectory(this.db);
                CarsInternal new_ci = new CarsInternal()
                {
                    id_sostav = id_sostav,
                    id_arrival = id_arrival,
                    num = num,
                    dt_uz = dt_uz,
                    parent_id = parent_id
                };
                ef_ci.Add(new_ci);
                ef_ci.Save();
                return new_ci.id;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CreateCarsInternal(id_sostav={0},id_arrival={1},num={2},dt_uz={3},parent_id={4})", id_sostav,id_arrival,num,dt_uz,parent_id), servece_owner, eventID);
                return -1;
            }
        }

        #endregion

        #region CarInboundDelivery - ВХОДЯЩИЕ ПОСТАВКИ
        /// <summary>
        /// Получить входящую поставку для указаного внутреннего перемещения
        /// </summary>
        /// <param name="id_car_internal"></param>
        /// <returns></returns>
        public CarInboundDelivery GetCarInboundDeliveryOfCarsInternal(int id_car_internal)
        {
            try
            {
                EFCarInboundDelivery ef_cid = new EFCarInboundDelivery(this.db);
                return ef_cid.Get().Where(i => i.id_car_internal == id_car_internal).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetCarInboundDeliveryOfCarsInternal(id_car_internal={0})", id_car_internal), servece_owner, eventID);
                return null;
            }
        }
        /// <summary>
        /// Создать входящую поставку для внутренего премещения
        /// </summary>
        /// <param name="id_car_internal"></param>
        /// <param name="car_mt"></param>
        /// <returns></returns>
        public int CreateCarInboundDelivery(int id_car_internal, ArrivalCars car_mt)
        {
            try
            {
                CarInboundDelivery delivery = GetCarInboundDeliveryOfCarsInternal(id_car_internal);
                if (delivery == null)
                {
                    return AddCarInboundDelivery(id_car_internal, car_mt.Num, car_mt.ArrivalSostav.IDArrival, car_mt.DateOperation, car_mt.CompositionIndex, car_mt.Position, car_mt.CountryCode, car_mt.CargoCode, car_mt.Weight, car_mt.Consignee);
                }
                else {
                    return UpdateCarInboundDelivery(delivery, car_mt.DateOperation, car_mt.CompositionIndex, car_mt.Position, car_mt.CountryCode, car_mt.CargoCode, car_mt.Weight, car_mt.Consignee);
                }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CreateCarInboundDelivery(id_car_internal={0}, car_mt={1})", id_car_internal, car_mt.GetFieldsAndValue()), servece_owner, eventID);
                return -1;
            }
        }
        /// <summary>
        /// Создать входящую поставку для внутренего премещения 
        /// </summary>
        /// <param name="id_car_internal"></param>
        /// <param name="num"></param>
        /// <param name="id_arrival"></param>
        /// <param name="dt_operation"></param>
        /// <param name="index"></param>
        /// <param name="position"></param>
        /// <param name="CountryCode"></param>
        /// <param name="CargoCode"></param>
        /// <param name="Weight"></param>
        /// <param name="Consignee"></param>
        /// <returns></returns>
        public int CreateCarInboundDelivery(int id_car_internal, int num, int id_arrival, DateTime dt_operation, string index, int position, int CountryCode, int CargoCode, float Weight, int Consignee)
        {
            try
            {
                CarInboundDelivery delivery = GetCarInboundDeliveryOfCarsInternal(id_car_internal);
                if (delivery == null)
                {
                    return AddCarInboundDelivery(id_car_internal, num, id_arrival, dt_operation, index, position, CountryCode, CargoCode, Weight, Consignee);
                }
                else {
                    return UpdateCarInboundDelivery(delivery, dt_operation, index, position, CountryCode, CargoCode, Weight, Consignee);
                }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CreateCarInboundDelivery(id_car_internal={0}, num={1}, id_arrival={2}, dt_operation={3}, index={4}, position={5}, CountryCode={6}, CargoCode={7}, Weight={8}, Consignee={9})"
                    , id_car_internal, num, id_arrival, dt_operation, index, position, CountryCode, CargoCode, Weight, Consignee), servece_owner, eventID);
                return -1;
            }
        }
        /// <summary>
        /// Создать входящую поставку
        /// </summary>
        /// <param name="id_car_internal"></param>
        /// <param name="num"></param>
        /// <param name="id_arrival"></param>
        /// <param name="dt_operation"></param>
        /// <param name="index"></param>
        /// <param name="position"></param>
        /// <param name="CountryCode"></param>
        /// <param name="CargoCode"></param>
        /// <param name="Weight"></param>
        /// <param name="Consignee"></param>
        /// <returns></returns>
        public int AddCarInboundDelivery(int id_car_internal, int num, int id_arrival, DateTime dt_operation, string index, int position, int CountryCode, int CargoCode, float Weight, int Consignee)
        {
            try
            {
                EFCarInboundDelivery ef_cid = new EFCarInboundDelivery(this.db);
                RWDirectory rw_directory = new RWDirectory(this.db, servece_owner);
                CarInboundDelivery new_car = new CarInboundDelivery()
                {
                    id_car_internal = id_car_internal,
                    id_arrival = id_arrival,
                    num_car = num,
                    num_nakl_sap = null,
                    num_doc_reweighing_sap = null,
                    dt_doc_reweighing_sap = null,
                    weight_reweighing_sap = null,
                    dt_reweighing_sap = null,
                    post_reweighing_sap = null,
                    material_code_sap = null,
                    material_name_sap = null,
                    station_shipment = null,
                    station_shipment_code_sap = null,
                    station_shipment_name_sap = null,
                    id_consignee = null,
                    shop_code_sap = null,
                    shop_name_sap = null,
                    new_shop_code_sap = null,
                    new_shop_name_sap = null,
                    permission_unload_sap = null,
                    step1_sap = null,
                    step2_sap = null,
                    datetime = dt_operation,
                    composition_index = index,
                    position = position,
                    country_code = CountryCode,
                    id_country = rw_directory.GetIDCountryOfCodeSNG(CountryCode),
                    weight_cargo = (decimal)Weight,
                    cargo_code = CargoCode,
                    id_cargo = rw_directory.GetIDCargoOfCodeETSNG(CargoCode),
                    consignee = Consignee,
                };
                ef_cid.Add(new_car);
                ef_cid.Save();
                return new_car.id;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("AddCarInboundDelivery(id_car_internal={0}, num={1}, id_arrival={2}, dt_operation={3}, index={4}, position={5}, CountryCode={6}, CargoCode={7}, Weight={8}, Consignee={9})"
                    ,id_car_internal, num, id_arrival, dt_operation, index, position, CountryCode, CargoCode, Weight, Consignee), servece_owner, eventID);
                return -1;
            }
        }
        /// <summary>
        /// Обновить входящую поставку для внутренего премещения
        /// </summary>
        /// <param name="delivery"></param>
        /// <param name="dt_operation"></param>
        /// <param name="index"></param>
        /// <param name="position"></param>
        /// <param name="CountryCode"></param>
        /// <param name="CargoCode"></param>
        /// <param name="Weight"></param>
        /// <param name="Consignee"></param>
        /// <returns></returns>
        public int UpdateCarInboundDelivery(CarInboundDelivery delivery, DateTime dt_operation, string index, int position, int CountryCode, int CargoCode, float Weight, int Consignee)
        {
            try
            {
                EFCarInboundDelivery ef_cid = new EFCarInboundDelivery(this.db);
                RWDirectory rw_directory = new RWDirectory(this.db, servece_owner);

                delivery = new CarInboundDelivery()
                {
                    datetime = dt_operation,
                    composition_index = index,
                    position = position,
                    country_code = CountryCode,
                    id_country = rw_directory.GetIDCountryOfCodeSNG(CountryCode),
                    weight_cargo = (decimal)Weight,
                    cargo_code = CargoCode,
                    id_cargo = rw_directory.GetIDCargoOfCodeETSNG(CargoCode),
                    consignee = Consignee,
                };
                ef_cid.Update(delivery);
                ef_cid.Save();
                return delivery.id;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("UpdateCarInboundDelivery(delivery={0}, dt_operation={1}, index={2}, position={3}, CountryCode={4}, CargoCode={5}, Weight={6}, Consignee={7})"
                    , delivery.GetFieldsAndValue(), dt_operation, index, position, CountryCode, CargoCode, Weight, Consignee), servece_owner, eventID);
                return -1;
            }
        }
        #endregion

    }
}
