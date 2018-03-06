using EFMT.Concrete;
using EFMT.Entities;
using EFRW.Concrete;
using EFRW.Entities;
using MessageLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libClass;

namespace RW
{

    public enum operation : int
    {
        close = 0,
        set_station = 1,
        set_way,
        send_station,
        send_shop,
        send_wo
    }

    public interface IOperation
    {
        operation operation { get; set; }
        int? id_station { get; set; }
        int? id_way { get; set; }
        int position { get; set; }
        int? side { get; set; }
        DateTime dt_set { get; set; }
        int? id_status { get; set; }
    }

    public class OperationSetStation : IOperation
    {
        public operation operation { get; set; }
        public int? id_station { get; set; }
        public int? id_way { get; set; }
        public int position { get; set; }
        public int? side { get; set; }
        public DateTime dt_set { get; set; }
        public int? id_status { get; set; }

        public OperationSetStation(int id_station, int id_way, int position, DateTime dt_set, int? id_status)
        {
            this.operation = operation.set_station;
            this.id_station = id_station;
            this.id_way = id_way;
            this.position = position;
            this.side = null;
            this.dt_set = dt_set;
            this.id_status = id_status;
        }
    }

    public class OperationSendingStation : IOperation
    {
        public operation operation { get; set; }
        public int? id_station { get; set; }
        public int? id_way { get; set; }
        public int position { get; set; }
        public int? side { get; set; }
        public DateTime dt_set { get; set; }
        public int? id_status { get; set; }

        public OperationSendingStation(int id_station, int position, DateTime dt_set, int? id_status)
        {
            this.operation = operation.send_station;
            this.id_station = id_station;
            this.id_way = null;
            this.position = position;
            this.side = null;
            this.dt_set = dt_set;
            this.id_status = id_status;
        }
    }


    public class OperationClose : IOperation
    {
        public operation operation { get; set; }
        public int? id_station { get; set; }
        public int? id_way { get; set; }
        public int position { get; set; }
        public int? side { get; set; }
        public DateTime dt_set { get; set; }
        public int? id_status { get; set; }

        public OperationClose(DateTime dt_close)
        {
            this.operation = operation.close;
            this.id_station = null;
            this.id_way = null;
            this.position = -1;
            this.side = null;
            this.dt_set = dt_close;
            this.id_status = null;
        }
    }

    public class RWOperation
    {
        private eventID eventID = eventID.RW_RWOperation;
        protected service servece_owner = service.Null;

        EFRailWay ef_rw = new EFRailWay();
        RWReference rw_ref = new RWReference(true);
        EFMetallurgTrans ef_mt = new EFMetallurgTrans();
        EFReference.Concrete.EFReference ef_reference = new EFReference.Concrete.EFReference();

        public RWOperation()
        {

        }

        public RWOperation(service servece_owner)
        {
            this.servece_owner = servece_owner;
        }

        #region Набор функций с операциями над вагонами
        /// <summary>
        /// Выполнить операцию
        /// </summary>
        /// <param name="car"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        public delegate CarOperations SetOperation(Cars car, IOperation operation);
        /// <summary>
        /// Опрерация поставить на путь станции.
        /// </summary>
        /// <param name="car"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        public CarOperations OperationSetWayStation(Cars car, IOperation operation)
        {
            try
            {
                Ways way = ef_rw.GetWays((int)operation.id_way);
                CarOperations last_operation = OperationClose(car, new OperationClose(operation.dt_set));
                int? parent_id = last_operation != null ? (int?)last_operation.id : null;
                CarOperations new_car_operation = new CarOperations()
                {
                    id = 0,
                    id_car = car.id,
                    id_car_conditions = null,
                    id_car_status = operation.id_status != null ? operation.id_status : way.id_car_status,
                    id_station = operation.id_station,
                    dt_inp_station = operation.dt_set,
                    dt_out_station = null,
                    id_way = operation.id_way,
                    dt_inp_way = operation.dt_set,
                    dt_out_way = null,
                    position = operation.position,
                    send_id_station = null,
                    send_id_overturning = null,
                    send_id_shop = null,
                    send_dt_inp_way = null,
                    send_dt_out_way = null,
                    send_id_position = null,
                    send_train1 = null,
                    send_train2 = null,
                    send_side = null,
                    parent_id = parent_id,
                    edit_dt = DateTime.Now,
                    edit_user = System.Environment.UserDomainName + @"\" + System.Environment.UserName,
                };
                //car.CarOperations.Add(new_car_operation);
                return new_car_operation;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("OperationSetWayStation(car={0}, operation={1})",
                    car, operation), servece_owner, eventID);
                return null;
            }
        }
        /// <summary>
        /// Опрерация поставить на путь станции.
        /// </summary>
        /// <param name="car"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        public CarOperations OperationSendingStation(Cars car, IOperation operation)
        {
            try
            {
                CarOperations last_operation = OperationClose(car, new OperationClose(operation.dt_set));
                int? parent_id = last_operation != null ? (int?)last_operation.id : null;
                CarOperations new_car_operation = new CarOperations()
                {
                    id = 0,
                    id_car = car.id,
                    id_car_conditions = last_operation!=null ? last_operation.id_car_conditions : null,
                    id_car_status = operation.id_status != null ? operation.id_status : last_operation.id_car_status,
                    id_station = last_operation != null ? last_operation.id_station : null,
                    dt_inp_station = last_operation != null ? last_operation.dt_inp_station : null,
                    dt_out_station = last_operation != null ? last_operation.dt_out_station : null,
                    id_way = last_operation != null ? last_operation.id_way : null,
                    dt_inp_way = last_operation != null ? last_operation.dt_inp_way : null,
                    dt_out_way = last_operation != null ? last_operation.dt_out_way : null,
                    position = operation.position,
                    send_id_station = operation.id_station,
                    send_id_overturning = null,
                    send_id_shop = null,
                    send_dt_inp_way = operation.dt_set,
                    send_dt_out_way = null,
                    send_id_position = null,
                    send_train1 = null,
                    send_train2 = null,
                    send_side = null,
                    parent_id = parent_id,
                    edit_dt = DateTime.Now,
                    edit_user = System.Environment.UserDomainName + @"\" + System.Environment.UserName, 
                    Cars = car,
                };
                //car.CarOperations.Add(new_car_operation);
                return new_car_operation;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("OperationSetWayStation(car={0}, operation={1})",
                    car, operation), servece_owner, eventID);
                return null;
            }
        }
        /// <summary>
        /// Операция закрыть операцию
        /// </summary>
        /// <param name="car"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        public CarOperations OperationClose(Cars car, IOperation operation)
        {
            try
            {
                CarOperations last_operation = GetLastOperations(car);
                if (last_operation != null)
                {
                    last_operation = CloseOperations(last_operation, operation.dt_set);
                    return last_operation;
                }
                return null;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("OperationClose(car={0}, operation={1})",
                    car, operation), servece_owner, eventID);
                return null;
            }
        }

        #endregion

        /// <summary>
        /// Проверка опирации, стоит(стоял set=false) вагон указаной станции и пути (по указаному времени)
        /// </summary>
        /// <param name="oper"></param>
        /// <param name="id_station"></param>
        /// <param name="id_way"></param>
        /// <param name="dt"></param>
        /// <param name="set"></param>
        /// <returns></returns>
        public bool IsSetOperation(CarOperations oper, int id_station, int id_way, DateTime? dt, bool set)
        {
            if (dt == null)
            {
                if (oper != null && (oper.id_station == id_station &
                        oper.id_way == id_way))
                {
                    if ((set & oper.dt_inp_way != null & oper.dt_out_way == null) | 
                        (!set & oper.dt_inp_way != null & oper.dt_out_way != null))
                    {
                        return true;
                    }
                }
            }
            else
            {
                if (oper != null && (oper.id_station == id_station &
                        oper.id_way == id_way))
                {
                    if ((set & oper.dt_inp_way == dt & oper.dt_out_way == null) |
                        (!set & oper.dt_inp_way <= dt & oper.dt_out_way >= dt))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// Проверим текущее положение вагона - вагон стоит на указанной станции и пути за указанное время
        /// </summary>
        /// <param name="car"></param>
        /// <param name="id_station"></param>
        /// <param name="id_way"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool IsSetStationWay(Cars car, int id_station, int id_way, DateTime? dt)
        {
            CarOperations last_operation = GetLastOperations(car);
            return IsSetOperation(last_operation, id_station, id_way, dt, true);
        }
        /// <summary>
        /// Проходил вагон указаную станцию за указанное время 
        /// </summary>
        /// <param name="car"></param>
        /// <param name="id_station"></param>
        /// <param name="id_way"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool IsPassStationWay(Cars car, int id_station, int id_way, DateTime? dt)
        {
            //if (IsSetStationWay(car, id_station, id_way, dt)) return false; // Вагон не проходил он сейчас стоит на станции.
            foreach (CarOperations oper in car.CarOperations.OrderByDescending(o => o.id).ToList())
            {
                bool result = IsSetOperation(oper, id_station, id_way, dt, false);
                if (result) { return true; }
            }
            return false;
        }

        public List<CarOperations> GetPassStationWay(Cars car, int id_station, int id_way)
        {
            List<CarOperations> list_result = new List<CarOperations>();
            foreach (CarOperations oper in car.CarOperations.OrderByDescending(o => o.id).ToList())
            {
                bool result = IsSetOperation(oper, id_station, id_way, null, false);
                if (result & oper.dt_out_station!= null & oper.dt_out_way !=null) { 
                    // нужная операция и закрыта
                    list_result.Add(oper); 
                }
            }
            return list_result;
        }

        #region Методы выполнения операций над вагонами
        /// <summary>
        /// Вернуть последнюю запись операции по указаному вагону
        /// </summary>
        /// <param name="car"></param>
        /// <returns></returns>
        public CarOperations GetLastOperations(Cars car)
        {
            try
            {
                if (car.CarOperations != null && car.CarOperations.Count() > 0)
                {
                    CarOperations last_operation = car.CarOperations.OrderByDescending(o => o.id).FirstOrDefault();
                    return last_operation;
                }
                return null;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetLastOperations(car={0})", car), eventID);
                return null;
            }
        }
        /// <summary>
        /// Закрыть последнюю операцию
        /// </summary>
        /// <param name="operations"></param>
        /// <param name="dt_close"></param>
        /// <returns></returns>
        public CarOperations CloseOperations(CarOperations operations, DateTime? dt_close)
        {
            try
            {
                if (operations == null) return null;
                if (operations.dt_inp_station != null & operations.dt_out_station == null) { operations.dt_out_station = dt_close != null ? dt_close : DateTime.Now; }
                if (operations.dt_inp_way != null & operations.dt_out_way == null) { operations.dt_out_way = dt_close != null ? dt_close : DateTime.Now; }
                if (operations.send_dt_inp_way != null & operations.send_dt_out_way == null) { operations.send_dt_out_way = dt_close != null ? dt_close : DateTime.Now; }
                return operations;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CloseOperations(operations={0}, dt_close={1})", operations, dt_close), eventID);
                return null;
            }
        }
        /// <summary>
        /// Выполнить операцию над вагоном
        /// </summary>
        /// <param name="car"></param>
        /// <param name="filter"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        public CarOperations ExecOperation(Cars car, SetOperation filter, IOperation operation)
        {
            try
            {
                return filter(car, operation);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("ExecOperation(car={0}, filter={1}, operation={2})",
                    car, filter, operation), servece_owner, eventID);
                return null;
            }
        }
        /// <summary>
        /// Выполнить операцию по группе вагонов
        /// </summary>
        /// <param name="id_arrival"></param>
        /// <param name="nums"></param>
        /// <param name="filter"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        public List<CarOperations> ExecOperation(int id_arrival, int[] nums, SetOperation filter, IOperation operation)
        {
            try
            {
                List<Cars> list_not_cars = ef_rw.GetCarsOfArrivalNum(id_arrival, nums);
                return ExecOperation(list_not_cars, filter, operation);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("ExecOperation(id_arrival={0}, nums={1}, filter={2}, operation={3})",
                    id_arrival, nums, filter, operation), servece_owner, eventID);
                return null;
            }
        }
        /// <summary>
        /// Выполнить операцию по группе вагонов
        /// </summary>
        /// <param name="cars"></param>
        /// <param name="filter"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        public List<CarOperations> ExecOperation(List<Cars> cars, SetOperation filter, IOperation operation)
        {
            try
            {
                List<CarOperations> list_result = new List<CarOperations>();
                foreach (Cars car in cars)
                {
                    list_result.Add(filter(car, operation));
                    operation.position++;
                }
                return list_result;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("ExecOperation(cars={0}, filter={1}, operation={2})",
                    cars, filter, operation), servece_owner, eventID);
                return null;
            }
        }
        #endregion

        #region Создать Вагоны в системе RailWay
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_arrival"></param>
        /// <param name="id_sostav"></param>
        /// <param name="num"></param>
        /// <param name="country_code"></param>
        /// <param name="set_operation"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        public Cars CreateCars(int id_arrival, int id_sostav, int num, int country_code, SetOperation set_operation, IOperation operation)
        {
            try
            {
                // Проверка наличия вагона в системе
                Cars new_car = ef_rw.GetCarsOfArrivalNum(id_arrival, num);
                if (new_car != null && new_car.id_sostav >= id_sostav) return new_car; // Вагон из этого состава уже стоит или номер состава меньше чем уже стоит(старый состав)
                if (new_car == null)
                {
                    // Получим код страны
                    int id_country = rw_ref.GetIDReferenceCountryOfCodeSNG(country_code);
                    // Проверим наличие вагона в справочнике если нет создадим + если есть из КИС перенесем аренды и владельца
                    ReferenceCars ref_car = rw_ref.GetReferenceCarsOfNum(num, id_arrival, operation.dt_set, id_country, true, true);
                    // Создадим строку 
                    new_car = new Cars()
                    {
                        id = 0,
                        id_arrival = id_arrival,
                        num = num,

                    };
                }
                new_car.id_sostav = id_sostav;
                new_car.dt_uz = operation.dt_set;
                new_car.dt_inp_amkr = null;
                new_car.dt_out_amkr = null;
                new_car.natur = null;

                // Создадим или изменяем входящую поставку
                //CarsInpDelivery cid = AddCarsInpDelivery(new_car);

                //List<CarsInpDelivery> cid = new List<CarsInpDelivery>();
                //cid.Add(AddCarsInpDelivery(new_car));
                //new_car.CarsInpDelivery = cid;

                List<CarsInpDelivery> cid = new List<CarsInpDelivery>();
                cid.Add(AddCarsInpDelivery(new_car));
                new_car.CarsInpDelivery = cid;

                // Закрываем старую, создаем новую операцию

                bool is_set = IsSetStationWay(new_car, (int)operation.id_station, (int)operation.id_way, null);
                bool is_pass = IsPassStationWay(new_car, (int)operation.id_station, (int)operation.id_way, null);

                if ((is_set) | (!is_set & !is_pass))
                {
                    CarOperations oper = set_operation(new_car, operation);
                    new_car.CarOperations.Add(oper);
                }
                else { 
                
                }
                return new_car;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CreateCars(id_arrival={0}, id_sostav={1}, num={2}, country_code={3}, set_operation={4}, operation={5})",
                    id_arrival, id_sostav, num, country_code, set_operation, operation), servece_owner, eventID);
                return null;
            }
        }
        /// <summary>
        /// Создать вагон в системе RailWay по данным MT и выполнить по нему операциию 
        /// </summary>
        /// <param name="arr_car"></param>
        /// <param name="set_operation"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        public Cars CreateCars(ArrivalCars arr_car, SetOperation set_operation, IOperation operation)
        {
            try
            {
                return CreateCars(arr_car.ArrivalSostav.IDArrival, arr_car.IDSostav, arr_car.Num, arr_car.CountryCode, set_operation, operation);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CreateCars(arr_car={0}, set_operation={1}, operation={2})", arr_car.GetFieldsAndValue(), set_operation, operation), servece_owner, eventID);
                return null;
            }
        }
        /// <summary>
        /// Создать вагон на станции УЗ системы RailWay по данным MT
        /// </summary>
        /// <param name="arr_car"></param>
        /// <returns></returns>
        public Cars CreateCarsMTToRailway(ArrivalCars arr_car)
        {
            try
            {
                // Определим код станции по справочникам
                EFReference.Entities.Stations station_in = ef_reference.GetStationsOfCode(int.Parse(arr_car.CompositionIndex.Substring(9, 4)) * 10);
                int codecs_in = station_in != null ? (int)station_in.code_cs : int.Parse(arr_car.CompositionIndex.Substring(9, 4)) * 10;
                //EFReference.Entities.Stations station_from = ef_reference.GetStationsOfCode(int.Parse(sost.CompositionIndex.Substring(0, 4)) * 10);
                //int? codecs_from = station_from != null ? station_from.code_cs : int.Parse(sost.CompositionIndex.Substring(0, 4)) * 10;
                Stations station = rw_ref.GetStationsUZ(codecs_in, true);
                Ways way = ef_rw.GetWaysOfArrivalUZ(station.id);
                return CreateCars(arr_car, OperationSetWayStation, new OperationSetStation(station.id, way.id, arr_car.Position, arr_car.DateOperation, null));
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CreateCarsMTToRailway(arr_car={0})", arr_car.GetFieldsAndValue()), servece_owner, eventID);
                return null;
            }
        }
        #endregion

        #region Закрыть вагоны в системе RailWay
        /// <summary>
        /// Закрыть вагоны предыдущего состава которых нет в новом составе 
        /// </summary>
        /// <param name="sostav"></param>
        /// <returns></returns>
        //public List<Cars> CloseCarsOldMTSostav(ArrivalSostav sostav)
        //{
        //    List<Cars> list_result = new List<Cars>();
        //    // Получим список отцепленных вагонов
        //    List<int> not_nums = ef_mt.GetNotCarsOfOldArrivalSostav(sostav);
        //    if (not_nums != null && not_nums.Count() > 0)
        //    {
        //        List<Cars> list_not_cars = ef_rw.GetCarsOfArrivalNum(sostav.IDArrival, not_nums.ToArray());
        //        List<CarOperations> list_operations = ExecOperation(list_not_cars, OperationClose, new OperationClose(sostav.DateTime));
        //        //int res = ef_rw.SaveCars(list_not_cars);
        //        list_result = list_not_cars;
        //    }
        //    return list_result;
        //}
        #endregion

        #region Перенос вагонов из МТ в систему RailWay
        /// <summary>
        /// Перенести состав МТ с учетом указанной операции
        /// </summary>
        /// <param name="sostav"></param>
        /// <param name="filter"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        public int TransferArrivalSostavToRailWay(ArrivalSostav sostav, SetOperation filter, IOperation operation)
        {
            foreach (ArrivalCars car in sostav.ArrivalCars.ToList())
            {
                //Cars car_new = rw_oper.CreateCarsInWay(car);
                //Cars car_new = rw_oper.CreateCars(car);
                //int res = ef_rw.SaveChanges();
            }
            return 0;
        }
        /// <summary>
        /// Перенос прибывшего на УЗ КР состава, на станции АМКР системы Railway
        /// </summary>
        /// <param name="sostav"></param>
        /// <returns></returns>
        public int TransferArrivalSostavToRailWay(ArrivalSostav sostav)
        {
            try
            {
                int count = 0;
                int transfer = 0;
                int error = 0;
                string message = null;
                //int close = 0;
                // Получим список отцепленных вагонов и закроем их
                List<int> not_nums = ef_mt.GetNotCarsOfOldArrivalSostav(sostav);

                int result_close = 0;
                if (not_nums != null && not_nums.Count() > 0)
                {
                    List<Cars> list_not_cars = ef_rw.GetCarsOfArrivalNum(sostav.IDArrival, not_nums.ToArray());
                    List<CarOperations> list_operations = ExecOperation(list_not_cars, OperationClose, new OperationClose(sostav.DateTime));
                    result_close = SaveChanges(list_operations);
                }
                // Поставим новые
                List<Cars> list_result = new List<Cars>();
                foreach (ArrivalCars car in sostav.ArrivalCars.ToList())
                {
                    message += car.Num.ToString() + " - ";
                    Cars car_new = CreateCarsMTToRailway(car);
                    //int res = rw_oper.SaveCars(car_new);
                    if (car_new != null) { 
                        list_result.Add(car_new);
                        transfer++;
                        message += car_new.id.ToString();
                    }
                    else { error++; message += "-1"; }
                    message += "; ";
                }
                count = sostav.ArrivalCars != null ? sostav.ArrivalCars.Count() : 0;                
                int result = SaveChanges(list_result);

                string mess = String.Format("Перенос состава из базы данных [MT.Arrival] (id состава: {0}, id прибытия {1}, индекс: {2}, дата операции: {3}) в систему RailWay. Определенно для переноса {4} вагона(ов), перенесено {5}, ошибок переноса {6}, закрыто по ТСП {7}, сохранено в системе {8}.",
                    sostav.ID, sostav.IDArrival, sostav.CompositionIndex, sostav.DateTime, count, transfer, error, result_close, result);
                mess.WriteInformation(servece_owner, eventID);
                if (error > 0 | result < 0) { mess.WriteEvents(message, servece_owner, eventID); }
                return result;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("TransferArrivalSostavToRailWay(sostav={0})", sostav.GetFieldsAndValue()), servece_owner, eventID);
                return -1;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_sostav"></param>
        /// <returns></returns>
        public int TransferArrivalSostavToRailWay(int id_sostav)
        {
            try
            {
                ArrivalSostav sost = ef_mt.GetArrivalSostav(id_sostav);
                return TransferArrivalSostavToRailWay(sost);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("TransferArrivalSostavToRailWay(id_sostav={0})", id_sostav), servece_owner, eventID);
                return -1;
            }
        }
        #endregion

        #region Входящие поставки
        /// <summary>
        /// Добавить или обновить входящую поставку
        /// </summary>
        /// <param name="car"></param>
        /// <returns></returns>
        public CarsInpDelivery AddCarsInpDelivery(Cars car)
        {
            try
            {
                ArrivalCars car_mt = ef_mt.GetArrivalCarsOfSostavNum(car.id_sostav, car.num);
                CarsInpDelivery inp_deliver = car.CarsInpDelivery.FirstOrDefault();
                if (car.CarsInpDelivery == null || car.CarsInpDelivery.Count() == 0)
                {
                    inp_deliver = new CarsInpDelivery()
                    {
                        id = 0,
                        id_car = car.id,
                        id_arrival = car.id_arrival,
                        num_car = car.num,
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
                        shop_code_sap = null,
                        shop_name_sap = null,
                        new_shop_code_sap = null,
                        new_shop_name_sap = null,
                        permission_unload_sap = null,
                        step1_sap = null,
                        step2_sap = null,
                    };
                }
                inp_deliver.datetime = car_mt.DateOperation;
                inp_deliver.composition_index = car_mt.CompositionIndex;
                inp_deliver.position = car_mt.Position;
                inp_deliver.country_code = car_mt.CountryCode;
                inp_deliver.id_country = rw_ref.GetIDReferenceCountryOfCodeSNG(car_mt.CountryCode);
                inp_deliver.weight_cargo = (decimal)car_mt.Weight;
                inp_deliver.cargo_code = car_mt.CargoCode;
                inp_deliver.id_cargo = rw_ref.GetIDReferenceCargoOfCodeETSNG(car_mt.CargoCode);
                inp_deliver.consignee = car_mt.Consignee;
                return inp_deliver;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("AddCarsInpDelivery(car={0})", car.GetFieldsAndValue()), servece_owner, eventID);
                return null;
            }
        }
        
        //public CarsInpDelivery AddCarsInpDelivery(Cars car)
        //{
        //    try
        //    {
        //        ArrivalCars car_mt = ef_mt.GetArrivalCarsOfSostavNum(car.id_sostav, car.num);
        //        if (car.CarsInpDelivery == null || car.CarsInpDelivery.Count() == 0)
        //        {
        //            CarsInpDelivery inp_deliver = new CarsInpDelivery()
        //            {
        //                id = 0,
        //                id_car = car.id,
        //                id_arrival = car.id_arrival,
        //                num_car = car.num,
        //                num_nakl_sap = null,
        //                num_doc_reweighing_sap = null,
        //                dt_doc_reweighing_sap = null,
        //                weight_reweighing_sap = null,
        //                dt_reweighing_sap = null,
        //                post_reweighing_sap = null,
        //                material_code_sap = null,
        //                material_name_sap = null,
        //                station_shipment = null,
        //                station_shipment_code_sap = null,
        //                station_shipment_name_sap = null,
        //                shop_code_sap = null,
        //                shop_name_sap = null,
        //                new_shop_code_sap = null,
        //                new_shop_name_sap = null,
        //                permission_unload_sap = null,
        //                step1_sap = null,
        //                step2_sap = null,
        //            };
        //            car.CarsInpDelivery.Add(inp_deliver);
        //        }
        //        car.CarsInpDelivery.ToList()[0].datetime = car_mt.DateOperation;
        //        car.CarsInpDelivery.ToList()[0].composition_index = car_mt.CompositionIndex;
        //        car.CarsInpDelivery.ToList()[0].position = car_mt.Position;
        //        car.CarsInpDelivery.ToList()[0].country_code = car_mt.CountryCode;
        //        car.CarsInpDelivery.ToList()[0].id_country = rw_ref.GetIDReferenceCountryOfCodeSNG(car_mt.CountryCode);
        //        car.CarsInpDelivery.ToList()[0].weight_cargo = (decimal)car_mt.Weight;
        //        car.CarsInpDelivery.ToList()[0].cargo_code = car_mt.CargoCode;
        //        car.CarsInpDelivery.ToList()[0].id_cargo = rw_ref.GetIDReferenceCargoOfCodeETSNG(car_mt.CargoCode);
        //        car.CarsInpDelivery.ToList()[0].consignee = car_mt.Consignee;
        //        return car.CarsInpDelivery.ToList()[0];
        //    }
        //    catch (Exception e)
        //    {
        //        e.WriteErrorMethod(String.Format("AddCarsInpDelivery(car={0})", car.GetFieldsAndValue()), servece_owner, eventID);
        //        return null;
        //    }
        //}


        #endregion

        #region Сохранить изменения
        /// <summary>
        /// Сохранить изменения
        /// </summary>
        /// <param name="Cars"></param>
        /// <returns></returns>
        public int SaveChanges(List<Cars> Cars)
        {
            try
            {
                int result = 0;
                foreach (Cars car in Cars)
                {
                    int res = ef_rw.SaveCars(car);
                    if (res > 0) { result++; }
                }
                return result;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveChanges(Cars={0})", Cars), eventID);
                return -1;
            }

        }

        public int SaveChanges(List<CarOperations> CarOperations)
        {
            try
            {
                int result = 0;
                foreach (CarOperations oper in CarOperations)
                {
                    int res = ef_rw.SaveCarOperations(oper);
                    if (res > 0) { result++; }
                }
                return result;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveChanges(CarOperations={0})", CarOperations), eventID);
                return -1;
            }

        }
        #endregion



        ///// <summary>
        ///// Создать вагон новый вагон на указвной станции на указаном пути
        ///// </summary>
        ///// <param name="id_arrival"></param>
        ///// <param name="id_sostav"></param>
        ///// <param name="num"></param>
        ///// <param name="country_code"></param>
        ///// <param name="id_station"></param>
        ///// <param name="id_way"></param>
        ///// <param name="position"></param>
        ///// <param name="dt_set"></param>
        ///// <returns></returns>
        //public Cars CreateCarsInWay(int id_arrival, int id_sostav, int num, int country_code, int id_station, int id_way, int position, DateTime dt_set)
        //{
        //    // Проверка наличия вагона в системе
        //    Cars new_car = ef_rw.GetCarsOfArrivalNum(id_arrival, num);
        //    //int id_car = new_car != null ? new_car.id : 0;
        //    if (new_car == null)
        //    {
        //        // Получим код страны
        //        int id_country = rw_ref.GetIDReferenceCountryOfCodeSNG(country_code);
        //        // Проверим наличие вагона в справочнике если нет создадим + если есть из КИС перенесем аренды и владельца
        //        ReferenceCars ref_car = rw_ref.GetReferenceCarsOfNum(num, id_arrival, dt_set, id_country, true, true);
        //        // Создадим строку 
        //        new_car = new Cars()
        //        {
        //            id = 0,
        //            id_arrival = id_arrival,
        //            num = num,

        //        };
        //    }
        //    new_car.id_sostav = id_sostav;
        //    new_car.dt_uz = dt_set;
        //    new_car.dt_inp_amkr = null;
        //    new_car.dt_out_amkr = null;
        //    new_car.natur = null;
        //    // Создадим или изменяем входящую поставку
        //    CarsInpDelivery cid = AddCarsInpDelivery(new_car);
        //    // Закрываем старую, создаем новую операцию
        //    CarOperations oper = AddOperationSetWayStation(new_car, id_station, id_way, dt_set, position, null);
        //    // Сохраним изменения в системе
        //    int id_car = ef_rw.SaveCars(new_car);
        //    if (id_car > 0)
        //    {
        //        return ef_rw.GetCars(id_car);
        //    }
        //    return null;
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="arr_car"></param>
        ///// <param name="save"></param>
        ///// <returns></returns>
        //public Cars CreateCarsInWay(ArrivalCars arr_car)
        //{
        //    // Определим код станции по справочникам
        //    EFReference.Entities.Stations station_in = ef_reference.GetStationsOfCode(int.Parse(arr_car.CompositionIndex.Substring(9, 4)) * 10);
        //    int codecs_in = station_in != null ? (int)station_in.code_cs : int.Parse(arr_car.CompositionIndex.Substring(9, 4)) * 10;
        //    //EFReference.Entities.Stations station_from = ef_reference.GetStationsOfCode(int.Parse(sost.CompositionIndex.Substring(0, 4)) * 10);
        //    //int? codecs_from = station_from != null ? station_from.code_cs : int.Parse(sost.CompositionIndex.Substring(0, 4)) * 10;
        //    Stations station = rw_ref.GetStationsUZ(codecs_in, true);
        //    Ways way = ef_rw.GetWaysOfArrivalUZ(station.id);
        //    return CreateCarsInWay(arr_car.ArrivalSostav.IDArrival, arr_car.IDSostav, arr_car.Num, arr_car.CountryCode, station.id, way.id, arr_car.Position, arr_car.DateOperation);
        //}









        ///// <summary>
        ///// Поставить на путь системы RailWay новый вагон
        ///// </summary>
        ///// <param name="id_arrival"></param>
        ///// <param name="id_sostav"></param>
        ///// <param name="num"></param>
        ///// <param name="country_code"></param>
        ///// <param name="id_station"></param>
        ///// <param name="id_way"></param>
        ///// <param name="position"></param>
        ///// <param name="dt_set"></param>
        ///// <returns></returns>
        //public Cars AddCarToRaylWay(int id_arrival, int id_sostav, int num, int country_code, int id_station, int id_way, int position, DateTime dt_set)
        //{
        //    // Проверка наличия вагона в системе
        //    Cars new_car = ef_rw.GetCarsOfArrivalNum(id_arrival, num);
        //    //int id_car = new_car != null ? new_car.id : 0;
        //    if (new_car == null)
        //    {
        //        // Получим код страны
        //        int id_country = rw_ref.GetIDReferenceCountryOfCodeSNG(country_code);
        //        // Проверим наличие вагона в справочнике если нет создадим + если есть из КИС перенесем аренды и владельца
        //        ReferenceCars ref_car = rw_ref.GetReferenceCarsOfNum(num, id_arrival, dt_set, id_country, true, true);
        //        // Создадим строку 
        //        new_car = new Cars()
        //        {
        //            id = 0,
        //            id_arrival = id_arrival,
        //            num = num,

        //        };
        //    }
        //    new_car.id_sostav = id_sostav;
        //    new_car.dt_uz = dt_set;
        //    new_car.dt_inp_amkr = null;
        //    new_car.dt_out_amkr = null;
        //    new_car.natur = null;
        //    // Создадим или изменяем входящую поставку
        //    CarsInpDelivery cid = AddCarsInpDelivery(new_car);
        //    // Закрываем старую, создаем новую операцию
        //    CarOperations oper = AddOperationSetWayStation(new_car, id_station, id_way, dt_set, position, null);
        //    // Сохраним изменения в системе
        //    int id_car = ef_rw.SaveCars(new_car);
        //    if (id_car > 0)
        //    {
        //        return ef_rw.GetCars(id_car);
        //    }
        //    return null;
        //}
        ///// <summary>
        ///// Поставить на путь системы RailWay новый вагон по данным МеталлургТранса
        ///// </summary>
        ///// <param name="arr_car"></param>
        ///// <returns></returns>
        //public Cars AddCarToRaylWay(ArrivalCars arr_car)
        //{
        //    //EFRailWay ef_rw = new EFRailWay();
        //    //RWReference rw_ref = new RWReference(true);
        //    //EFReference.Concrete.EFReference ef_reference = new EFReference.Concrete.EFReference();
        //    // Определим станцию и путь
        //    // Определим код станции по справочникам
        //    //Reference api_reference = new Reference();
        //    EFReference.Entities.Stations station_in = ef_reference.GetStationsOfCode(int.Parse(arr_car.CompositionIndex.Substring(9, 4)) * 10);
        //    int codecs_in = station_in != null ? (int)station_in.code_cs : int.Parse(arr_car.CompositionIndex.Substring(9, 4)) * 10;
        //    //EFReference.Entities.Stations station_from = ef_reference.GetStationsOfCode(int.Parse(sost.CompositionIndex.Substring(0, 4)) * 10);
        //    //int? codecs_from = station_from != null ? station_from.code_cs : int.Parse(sost.CompositionIndex.Substring(0, 4)) * 10;

        //    Stations station = rw_ref.GetStationsUZ(codecs_in, true);
        //    Ways way = ef_rw.GetWaysOfArrivalUZ(station.id);
        //    return AddCarToRaylWay(arr_car.ArrivalSostav.IDArrival, arr_car.IDSostav, arr_car.Num, arr_car.CountryCode, station.id, way.id, arr_car.Position, arr_car.DateOperation);
        //}

        //public CarsInpDelivery AddCarsInpDelivery(ArrivalCars arr_car, int id_car, int id_arrival)
        //{
        //    EFRailWay ef_rw = new EFRailWay();
        //    RWReference rw_ref = new RWReference(true);
        //    CarsInpDelivery inp_deliver = ef_rw.GetCarsInpDeliveryOfNumArrival(arr_car.Num, id_arrival);
        //    if (inp_deliver != null)
        //    {
        //        inp_deliver = new CarsInpDelivery()
        //        {
        //            id = 0,
        //            id_car = id_car,
        //            datetime = arr_car.DateOperation,
        //            composition_index = arr_car.CompositionIndex,
        //            id_arrival = id_arrival,
        //            num_car = arr_car.Num,
        //            position = arr_car.Position,
        //            num_nakl_sap = null,
        //            country_code = arr_car.CountryCode,
        //            id_country = rw_ref.GetIDReferenceCountryOfCodeSNG(arr_car.CountryCode),
        //            weight_cargo = (decimal)arr_car.Weight,
        //            num_doc_reweighing_sap = null,
        //            dt_doc_reweighing_sap = null,
        //            weight_reweighing_sap = null,
        //            dt_reweighing_sap = null,
        //            post_reweighing_sap = null,
        //            cargo_code = arr_car.CargoCode,
        //            id_cargo = rw_ref.GetIDReferenceCargoOfCodeETSNG(arr_car.CargoCode),
        //            material_code_sap = null,
        //            material_name_sap = null,
        //            station_shipment = null,
        //            station_shipment_code_sap = null,
        //            station_shipment_name_sap = null,
        //            consignee = arr_car.Consignee,
        //            shop_code_sap = null,
        //            shop_name_sap = null,
        //            new_shop_code_sap = null,
        //            new_shop_name_sap = null,
        //            permission_unload_sap = null,
        //            step1_sap = null,
        //            step2_sap = null,
        //            Cars = null,
        //            ReferenceCargo = null,
        //        };
        //        int res_id = ef_rw.SaveCarsInpDelivery(inp_deliver);
        //        if (res_id > 0)
        //        {
        //            inp_deliver = ef_rw.GetCarsInpDelivery(res_id);
        //        }
        //    }
        //    return inp_deliver;
        //}
        ///// <summary>
        ///// Добавить строку входящая поставка
        ///// </summary>
        ///// <param name="id_car"></param>
        ///// <returns></returns>
        //public CarsInpDelivery AddCarsInpDelivery(int id_car)
        //{
        //    //EFRailWay ef_rw = new EFRailWay();
        //    //EFMetallurgTrans ef_mt = new EFMetallurgTrans();
        //    //RWReference rw_ref = new RWReference(true);
        //    Cars car = ef_rw.GetCars(id_car);
        //    if (car == null) return null;
        //    ArrivalCars car_mt = ef_mt.GetArrivalCarsOfSostavNum(car.id_sostav, car.num);
        //    //CarsInpDelivery inp_deliver = ef_rw.GetCarsInpDeliveryOfNumArrival(car.num, car.id_arrival);
        //    CarsInpDelivery inp_deliver = ef_rw.GetCarsInpDeliveryOfCar(id_car);
        //    if (inp_deliver == null)
        //    {
        //        inp_deliver = new CarsInpDelivery()
        //        {
        //            id = 0,
        //            id_car = id_car,
        //            datetime = car_mt.DateOperation,
        //            composition_index = car_mt.CompositionIndex,
        //            id_arrival = car.id_arrival,
        //            num_car = car.num,
        //            position = car_mt.Position,
        //            num_nakl_sap = null,
        //            country_code = car_mt.CountryCode,
        //            id_country = rw_ref.GetIDReferenceCountryOfCodeSNG(car_mt.CountryCode),
        //            weight_cargo = (decimal)car_mt.Weight,
        //            num_doc_reweighing_sap = null,
        //            dt_doc_reweighing_sap = null,
        //            weight_reweighing_sap = null,
        //            dt_reweighing_sap = null,
        //            post_reweighing_sap = null,
        //            cargo_code = car_mt.CargoCode,
        //            id_cargo = rw_ref.GetIDReferenceCargoOfCodeETSNG(car_mt.CargoCode),
        //            material_code_sap = null,
        //            material_name_sap = null,
        //            station_shipment = null,
        //            station_shipment_code_sap = null,
        //            station_shipment_name_sap = null,
        //            consignee = car_mt.Consignee,
        //            shop_code_sap = null,
        //            shop_name_sap = null,
        //            new_shop_code_sap = null,
        //            new_shop_name_sap = null,
        //            permission_unload_sap = null,
        //            step1_sap = null,
        //            step2_sap = null,
        //            //Cars = null,
        //            //ReferenceCargo = null,
        //        };
        //    }
        //    else
        //    {
        //        inp_deliver.datetime = car_mt.DateOperation;
        //        inp_deliver.composition_index = car_mt.CompositionIndex;
        //        inp_deliver.id_arrival = car.id_arrival;
        //        inp_deliver.num_car = car.num;
        //        inp_deliver.position = car_mt.Position;
        //        inp_deliver.country_code = car_mt.CountryCode;
        //        inp_deliver.id_country = rw_ref.GetIDReferenceCountryOfCodeSNG(car_mt.CountryCode);
        //        inp_deliver.weight_cargo = (decimal)car_mt.Weight;
        //        inp_deliver.cargo_code = car_mt.CargoCode;
        //        inp_deliver.id_cargo = rw_ref.GetIDReferenceCargoOfCodeETSNG(car_mt.CargoCode);
        //        inp_deliver.consignee = car_mt.Consignee;
        //    }
        //    int res_id = ef_rw.SaveCarsInpDelivery(inp_deliver);
        //    if (res_id > 0)
        //    {
        //        return ef_rw.GetCarsInpDelivery(res_id);
        //    }
        //    return null;
        //}


        //public CarOperations AddOperationSetWayStation(Cars car, int id_station, int id_way, DateTime dt_set, int position, int? id_status)
        //{
        //    Ways way = ef_rw.GetWays(id_way);
        //    int? parent_id = null;
        //    if (car.CarOperations != null && car.CarOperations.Count() > 0)
        //    {
        //        CarOperations last_operation = car.CarOperations.OrderByDescending(o => o.id).FirstOrDefault();
        //        if (last_operation != null)
        //        {
        //            last_operation = CloseOperations(last_operation, dt_set);
        //            parent_id = last_operation != null ? (int?)last_operation.id : null;
        //        }
        //    }
        //    CarOperations new_inp_deliver = new CarOperations()
        //    {
        //        id = 0,
        //        id_car = car.id,
        //        id_car_conditions = null,
        //        id_car_status = id_status != null ? id_status : way.id_car_status,
        //        id_station = id_station,
        //        dt_inp_station = dt_set,
        //        dt_out_station = null,
        //        id_way = id_way,
        //        dt_inp_way = dt_set,
        //        dt_out_way = null,
        //        position = position,
        //        send_id_station = null,
        //        send_id_overturning = null,
        //        send_id_shop = null,
        //        send_dt_inp_way = null,
        //        send_dt_out_way = null,
        //        send_id_position = null,
        //        send_train1 = null,
        //        send_train2 = null,
        //        send_side = null,
        //        parent_id = null,
        //    };
        //    car.CarOperations.Add(new_inp_deliver);
        //    return new_inp_deliver;
        //}


        ///// <summary>
        ///// Получить id последней операции по id вагону (закрыть последнюю операцию)
        ///// </summary>
        ///// <param name="id_car"></param>
        ///// <param name="close"></param>
        ///// <returns></returns>
        //public int? GetIDLastOperationOfIDCar(int id_car, bool close, DateTime? dt_close)
        //{
        //    //EFRailWay ef_rw = new EFRailWay();
        //    Cars car = ef_rw.GetCars(id_car);
        //    if (car == null) return null;
        //    // Проверим наличие незакрытого вагона в системе RailWay, если есть закроем id операции запомним
        //    return GetIDLastOperationOfNumCar(car.num, close, dt_close);
        //}
        ///// <summary>
        ///// Получить id последней операции по номеру вагона (закрыть последнюю операцию)
        ///// </summary>
        ///// <param name="num"></param>
        ///// <param name="close"></param>
        ///// <returns></returns>
        //public int? GetIDLastOperationOfNumCar(int num, bool close, DateTime? dt_close)
        //{
        //    //EFRailWay ef_rw = new EFRailWay();
        //    // Проверим наличие незакрытого вагона в системе RailWay, если есть закроем id операции запомним
        //    int? id_operation_old = null; // id предыдущей операции
        //    CarOperations old_car_operation = ef_rw.GetLastCarOperationsOfNumCar(num);
        //    if (old_car_operation != null)
        //    {
        //        id_operation_old = old_car_operation.id;
        //        if (close)
        //        {
        //            // Закрыть предыдущее состояние
        //            //TODO: При закрытии вагона на пути сделать смещение последовательности вагонов
        //            if (old_car_operation.dt_inp_station != null & old_car_operation.dt_out_station == null) { old_car_operation.dt_out_station = dt_close != null ? dt_close : DateTime.Now; }
        //            if (old_car_operation.dt_inp_way != null & old_car_operation.dt_out_way == null) { old_car_operation.dt_out_way = dt_close != null ? dt_close : DateTime.Now; }
        //            if (old_car_operation.send_dt_inp_way != null & old_car_operation.send_dt_out_way == null) { old_car_operation.send_dt_out_way = dt_close != null ? dt_close : DateTime.Now; }
        //            ef_rw.SaveCarOperations(old_car_operation);
        //        }
        //    }
        //    return id_operation_old;
        //}


        //public CarOperations OperationSetStationUZ(int id_car, int code_ctation_uz, DateTime dt_set, int position)
        //{
        //    EFRailWay ef_rw = new EFRailWay();
        //    RWReference rw_ref = new RWReference(true);
        //    // Закрыть предыдущую операцию
        //    int? id_operation_old = GetIDLastOperationOfIDCar(id_car, true, null);
        //    Stations station = rw_ref.GetStationsUZ(code_ctation_uz, true);
        //    Ways way = ef_rw.GetWaysOfArrivalUZ(station.id);
        //    CarOperations new_operation = new CarOperations() {
        //        id = 0,
        //        id_car = id_car,
        //        id_car_conditions = null,
        //        id_car_status = 15,
        //        id_station = station!=null ? (int?)station.id : null,
        //        dt_inp_station = dt_arrival,
        //        dt_out_station = null,
        //        id_way = way!=null ? (int?)way.id : null,
        //        dt_inp_way = dt_arrival,
        //        dt_out_way = null,
        //        position = position,
        //        send_id_station = null,
        //        send_id_overturning = null,
        //        send_id_shop = null,
        //        send_dt_inp_way = null,
        //        send_dt_out_way = null,
        //        send_id_position = null,
        //        send_train1 = null,
        //        send_train2 = null,
        //        send_side = null, 
        //        parent_id = id_operation_old
        //    };
        //    int res_oper = ef_rw.SaveCarOperations(new_operation);
        //    if (res_oper > 0) {
        //        new_operation = ef_rw.GetCarOperations(res_oper);
        //    }
        //    return new_operation;
        //}
        ///// <summary>
        ///// Операция "Поставить на путь станции"
        ///// </summary>
        ///// <param name="id_car"></param>
        ///// <param name="id_station"></param>
        ///// <param name="id_way"></param>
        ///// <param name="dt_set"></param>
        ///// <param name="position"></param>
        ///// <param name="id_status"></param>
        ///// <returns></returns>
        //public CarOperations OperationSetWayStation(int id_car, int id_station, int id_way, DateTime dt_set, int position, int? id_status)
        //{
        //    //EFRailWay ef_rw = new EFRailWay();
        //    //// Закрыть предыдущую операцию
        //    int? id_operation_old = GetIDLastOperationOfIDCar(id_car, true, dt_set);
        //    Ways way = ef_rw.GetWays(id_way);
        //    CarOperations new_operation = new CarOperations()
        //    {
        //        id = 0,
        //        id_car = id_car,
        //        id_car_conditions = null,
        //        id_car_status = id_status != null ? id_status : way.id_car_status,
        //        id_station = id_station,
        //        dt_inp_station = dt_set,
        //        dt_out_station = null,
        //        id_way = id_way,
        //        dt_inp_way = dt_set,
        //        dt_out_way = null,
        //        position = position,
        //        send_id_station = null,
        //        send_id_overturning = null,
        //        send_id_shop = null,
        //        send_dt_inp_way = null,
        //        send_dt_out_way = null,
        //        send_id_position = null,
        //        send_train1 = null,
        //        send_train2 = null,
        //        send_side = null,
        //        parent_id = id_operation_old
        //    };
        //    int res_oper = ef_rw.SaveCarOperations(new_operation);
        //    if (res_oper > 0)
        //    {
        //        new_operation = ef_rw.GetCarOperations(res_oper);
        //    }
        //    return new_operation;
        //}
    }
}
