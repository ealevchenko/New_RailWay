using EFMT.Concrete;
using EFMT.Entities;
using EFRW.Concrete;
using EFRW.Entities1;
using MessageLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libClass;
//using EFKIS.Entities;
using System.Globalization;

namespace RW
{

    public enum operation : int
    {
        set_way,        // Поставить на путь 
        arrival_uz_way, // Принять с УЗ на путь АМКР
        sending_uz_way, // Сдать на УЗ с пути АМКР
        send_station,
        send_shop,
        send_wo
    }

    public interface IOperation
    {
        operation operation { get; set; }
        int? id_station { get; set; }
        int? id_way { get; set; }
        int? position { get; set; }
        int? side { get; set; }
        DateTime dt_set { get; set; }
        DateTime? dt_inp_amkr { get; set; }
        DateTime? dt_out_amkr { get; set; }
        int? id_status { get; set; }
        int? id_сonditions { get; set; }
        int? natur_kis { get; set; }
        int? natur_rw { get; set; }
    }
    /// <summary>
    /// Клас данных поставить на путь
    /// </summary>
    public class OperationSetWay : IOperation
    {
        public operation operation { get; set; }
        public int? id_station { get; set; }
        public int? id_way { get; set; }
        public int? position { get; set; }
        public int? side { get; set; }
        public DateTime dt_set { get; set; }
        public DateTime? dt_inp_amkr { get; set; }
        public DateTime? dt_out_amkr { get; set; }
        public int? id_status { get; set; }
        public int? id_сonditions { get; set; }
        public int? natur_kis { get; set; }
        public int? natur_rw { get; set; }

        public OperationSetWay(int id_way, DateTime dt_set, int? id_status, int? id_сonditions)
        {
            this.operation = operation.set_way;
            this.id_station = id_station;
            this.id_way = id_way;
            this.position = null;
            this.side = null;
            this.dt_set = dt_set;
            this.id_status = id_status;
            this.id_сonditions = id_сonditions;

        }

        public OperationSetWay(int id_way, int position, DateTime dt_set, int? id_status, int? id_сonditions)
        {
            this.operation = operation.set_way;
            //this.id_station = id_station;
            this.id_way = id_way;
            this.position = position;
            this.side = null;
            this.dt_set = dt_set;
            this.id_status = id_status;
            this.id_сonditions = id_сonditions;
        }
    }
    /// <summary>
    /// Класс данных отправить на станцию на путь
    /// </summary>
    public class OperationSendingStation : IOperation
    {
        public operation operation { get; set; }
        public int? id_station { get; set; }
        public int? id_way { get; set; }
        public int? position { get; set; }
        public int? side { get; set; }
        public DateTime dt_set { get; set; }
        public DateTime? dt_inp_amkr { get; set; }
        public DateTime? dt_out_amkr { get; set; }
        public int? id_status { get; set; }
        public int? id_сonditions { get; set; }
        public int? natur_kis { get; set; }
        public int? natur_rw { get; set; }

        public OperationSendingStation(int id_station, DateTime dt_set, int? id_status)
        {
            this.operation = operation.send_station;
            this.id_station = id_station;
            this.id_way = null;
            //this.position = position;
            this.side = null;
            this.dt_set = dt_set;
            this.id_status = id_status;
        }
    }
    /// <summary>
    /// Класс данных принять из УЗ на путь станции
    /// </summary>
    public class OperationArrivalUZWay : IOperation
    {
        public operation operation { get; set; }
        public int? id_station { get; set; }
        public int? id_way { get; set; }
        public int? position { get; set; }
        public int? side { get; set; }
        public DateTime dt_set { get; set; }
        public DateTime? dt_inp_amkr { get; set; }
        public DateTime? dt_out_amkr { get; set; }
        public int? id_status { get; set; }
        public int? id_сonditions { get; set; }
        public int? natur_kis { get; set; }
        public int? natur_rw { get; set; }

        public OperationArrivalUZWay(int id_way, int position, DateTime dt_set, DateTime? dt_inp_amkr, int? natur_kis, int? natur, int? id_сonditions)
        {
            this.operation = operation.arrival_uz_way;
            this.id_way = id_way;
            this.position = position;
            //this.side = null;
            this.dt_set = dt_set;
            this.dt_inp_amkr = dt_inp_amkr;
            this.id_status = 13; // Прибыл с УЗ
            this.id_сonditions = id_сonditions;
            this.natur_kis = natur_kis;
            this.natur_rw = natur;
        }

        public OperationArrivalUZWay(int id_way, DateTime dt_set, DateTime? dt_inp_amkr, int? natur_kis, int? natur, int? id_сonditions)
        {
            this.operation = operation.arrival_uz_way;
            this.id_way = id_way;
            //this.position = position;
            //this.side = null;
            this.dt_set = dt_set;
            this.dt_inp_amkr = dt_inp_amkr;
            this.id_status = 13; // Прибыл с УЗ
            this.id_сonditions = id_сonditions;
            this.natur_kis = natur_kis;
            this.natur_rw = natur;
        }
    }

    public class OperationSendingUZWay : IOperation
    {
        public operation operation { get; set; }
        public int? id_station { get; set; }
        public int? id_way { get; set; }
        public int? position { get; set; }
        public int? side { get; set; }
        public DateTime dt_set { get; set; }
        public DateTime? dt_inp_amkr { get; set; }
        public DateTime? dt_out_amkr { get; set; }
        public int? id_status { get; set; }
        public int? id_сonditions { get; set; }
        public int? natur_kis { get; set; }
        public int? natur_rw { get; set; }

        public OperationSendingUZWay(int id_way, int position, DateTime dt_set, DateTime? dt_out_amkr, int? natur_kis, int? natur, int? id_сonditions)
        {
            this.operation = operation.sending_uz_way;
            this.id_way = id_way;
            this.position = position;
            //this.side = null;
            this.dt_set = dt_set;
            this.dt_out_amkr = dt_out_amkr;
            this.id_status = 13; // Прибыл с УЗ
            this.id_сonditions = id_сonditions;
            this.natur_kis = natur_kis;
            this.natur_rw = natur;
        }

        public OperationSendingUZWay(int id_way, DateTime dt_set, DateTime? dt_out_amkr, int? natur_kis, int? natur, int? id_сonditions)
        {
            this.operation = operation.sending_uz_way;
            this.id_way = id_way;
            //this.position = position;
            //this.side = null;
            this.dt_set = dt_set;
            this.dt_out_amkr = dt_out_amkr;
            this.id_status = 16; // Сдан на УЗ
            this.id_сonditions = id_сonditions;
            this.natur_kis = natur_kis;
            this.natur_rw = natur;
        }

    }

    public class RWOperation : IRWOperation
    {
        private eventID eventID = eventID.RW_RWOperation;
        protected service servece_owner = service.Null;
        bool log_detali = false;

        EFRailWay1 ef_rw = new EFRailWay1();
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

        #region Набор функций для операций над вагонами
        /// <summary>
        /// Выполнить операцию
        /// </summary>
        /// <param name="car"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        public delegate CarOperations SetCarOperation(Cars car, IOperation operation);
        public delegate CarOperations SetOperation(CarOperations car_operation, IOperation operation);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="last_car_operation"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        public CarOperations OperationSetWay(CarOperations last_car_operation, IOperation operation)
        {
            try
            {
                if (last_car_operation != null)
                {
                    CarOperations close_operation = last_car_operation.CloseOperations(operation.dt_set, true);
                    //int res = ef_rw.SaveCarOperations(close_operation);
                    return OperationSetWay(last_car_operation.id_car, last_car_operation.id, operation);
                }
                return null;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("OperationSetWay(last_car_operation={0}, operation={1})",
                    last_car_operation, operation), servece_owner, eventID);
                return null;
            }
        }
        /// <summary>
        /// Создать операцию
        /// </summary>
        /// <param name="id_car"></param>
        /// <param name="parent_id"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        public CarOperations OperationSetWay(int id_car, int? parent_id, IOperation operation)
        {
            try
            {
                Ways way = ef_rw.GetWays((int)operation.id_way);
                int? position = way.CarOperations.IsOpenOperation(Filters.IsOpenAll).Max(o => o.position);

                return new CarOperations()
                {
                    id = 0,
                    id_car = id_car,
                    id_car_conditions = operation.id_сonditions,
                    id_car_status = operation.id_status != null ? operation.id_status : way.id_car_status,
                    id_station = way.id_station,
                    dt_inp_station = operation.dt_set,
                    dt_out_station = null,
                    id_way = operation.id_way,
                    dt_inp_way = operation.dt_set,
                    dt_out_way = null,
                    position = operation.position != null ? operation.position : position == null ? 1 : ++position,
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
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CreateOperationSetWay(id_car={0}, parent_id={1} operation={2})",
                    id_car, parent_id, operation), servece_owner, eventID);
                return null;
            }
        }
        /// <summary>
        /// Опрерация поставить на путь станции.
        /// </summary>
        /// <param name="car"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        public CarOperations OperationSetWay(Cars car, IOperation operation)
        {
            try
            {
                CarOperations last_operation = car.GetLastOperations();
                CarOperations new_operation = last_operation == null ? OperationSetWay(car.id, null, operation) : OperationSetWay(last_operation, operation);
                //int res = ef_rw.SaveCarOperations(new_operation);
                car.CarOperations.Add(new_operation);
                return new_operation;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("OperationSetWayStation(car={0}, operation={1})",
                    car, operation), servece_owner, eventID);
                return null;
            }
        }
        /// <summary>
        /// Опрерация принять с УЗ на путь.
        /// </summary>
        /// <param name="car"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        public CarOperations OperationArrivalUZWay(Cars car, IOperation operation)
        {
            try
            {
                
                CarOperations last_operation = car.GetLastOperations();
                CarOperations new_operation = last_operation == null ? OperationSetWay(car.id, null, operation) : OperationSetWay(last_operation, operation);
                car.CarOperations.Add(new_operation);
                car.SetCar(operation.natur_kis, operation.natur_rw, operation.dt_inp_amkr);
                return new_operation;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("OperationArrivalUZWay(car={0}, operation={1})",
                    car, operation), servece_owner, eventID);
                return null;
            }
        }
        /// <summary>
        /// Опрерация сдать на УЗ с пути.
        /// </summary>
        /// <param name="car"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        public CarOperations OperationSendingUZWay(Cars car, IOperation operation)
        {
            try
            {
                
                CarOperations last_operation = car.GetLastOperations();
                CarOperations new_operation = last_operation == null ? OperationSetWay(car.id, null, operation) : OperationSetWay(last_operation, operation);
                car.CarOperations.Add(new_operation);
                car.SetCar(operation.dt_out_amkr, operation.natur_kis);
                return new_operation;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("OperationSendingUZWay(car={0}, operation={1})",
                    car, operation), servece_owner, eventID);
                return null;
            }
        }


        #endregion

        #region Набор функций проверок стояния вагонаов
        /// <summary>
        /// Вернуть все открытые операции по номеру вагона
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public List<CarOperations> IsOpenAllOperationOfNum(int num)
        {
            return ef_rw.CarOperations.Where(o => o.Cars.num == num).IsOpenOperation(Filters.IsOpenAll);
        }
        /// <summary>
        /// Вернуть операции по номеру вагона с использованием фильтра
        /// </summary>
        /// <param name="num"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<CarOperations> IsOpenOperationOfNum(int num, Filters.IsFilterOpen filter)
        {
            return ef_rw.CarOperations.Where(o => o.Cars.num == num).IsOpenOperation(filter);
        }
        /// <summary>
        /// Вернуть все открытые операции по id_car
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public List<CarOperations> IsOpenAllOperationOfID(int id)
        {
            return ef_rw.CarOperations.Where(o => o.Cars.id == id).IsOpenOperation(Filters.IsOpenAll);
        }
        /// <summary>
        /// Вернуть операции по id_car с использованием фильтра
        /// </summary>
        /// <param name="num"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<CarOperations> IsOpenOperationOfID(int id, Filters.IsFilterOpen filter)
        {
            return ef_rw.CarOperations.Where(o => o.Cars.id == id).IsOpenOperation(filter);
        }

        /// <summary>
        /// Вернуть все открытые операции по номеру вагона и натурке
        /// </summary>
        /// <param name="num"></param>
        /// <param name="natur"></param>
        /// <returns></returns>
        public List<CarOperations> IsOpenAllOperationOfNumNatur(int num, int natur)
        {
            return ef_rw.CarOperations.Where(o => o.Cars.num == num & o.Cars.natur_kis == natur).IsOpenOperation(Filters.IsOpenAll);
        }
        /// <summary>
        /// Вернуть операции по номеру вагона и натурке с использованием фильтра
        /// </summary>
        /// <param name="num"></param>
        /// <param name="natur"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<CarOperations> IsOpenOperationOfNumNatur(int num, int natur, Filters.IsFilterOpen filter)
        {
            return ef_rw.CarOperations.Where(o => o.Cars.num == num & o.Cars.natur_kis == natur).IsOpenOperation(filter);
        }
        /// <summary>
        /// Вернуть все открытые операции по Cars
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public List<CarOperations> IsOpenAllOperationOfCar(Cars car)
        {
            Cars cars = ef_rw.GetCars(car.id);
            return cars!=null ? cars.CarOperations.IsOpenOperation(Filters.IsOpenAll) : null;
        }
        /// <summary>
        /// Вернуть операции по Cars с использованием фильтра
        /// </summary>
        /// <param name="car"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<CarOperations> IsOpenOperationOfCar(Cars car, Filters.IsFilterOpen filter)
        {
            Cars cars = ef_rw.GetCars(car.id);
            return cars != null ? cars.CarOperations.IsOpenOperation(filter) : null;
        }

        #endregion

        /// <summary>
        /// Коррекция вагонов если открытых операций больше чем 1, закрыть все старые операции оставить последнюю 
        /// </summary>
        /// <param name="list"></param>
        /// <param name="close"></param>
        /// <returns></returns>
        public CarOperations GetLastOpenOperation(List<CarOperations> list, bool close)
        {
            if (list == null && list.Count() == 0) return null;
            if (list != null & list.Count() == 1) return list[0];
            return list.OrderByDescending(o => o.id_car).FirstOrDefault(); // вернем последнюю операцию
            //TODO: Доработать коррекцию опреаций (по указанному номеру вагона есть несколько не закрытых операций)
            //List<IGrouping<Cars, CarOperations>> list_cars = list.GroupBy(o => o.Cars).ToList();
            //foreach (IGrouping<Cars, CarOperations> gr in list_cars.OrderByDescending(g => g.Key.id)) { 

            //}
        }

        #region Методы выполнения операций над вагонами
        /// <summary>
        /// Выполнить операцию над вагоном
        /// </summary>
        /// <param name="car"></param>
        /// <param name="filter"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        public CarOperations ExecOperation(Cars car, SetCarOperation filter, IOperation operation)
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
        /// Выполнить операцию над вагоном и сохранить в базе
        /// </summary>
        /// <param name="car"></param>
        /// <param name="filter"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        public int ExecSaveOperation(Cars car, SetCarOperation filter, IOperation operation)
        {
            try
            {
                CarOperations res_operation = ExecOperation(car, filter, operation);
                if (res_operation != null) {
                    return ef_rw.SaveCars(car);
                }
                return 0;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("ExecSaveOperation(car={0}, filter={1}, operation={2})",
                    car, filter, operation), servece_owner, eventID);
                return -1;
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
        public List<CarOperations> ExecOperation(int id_arrival, int[] nums, SetCarOperation filter, IOperation operation)
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
        public List<CarOperations> ExecOperation(List<Cars> cars, SetCarOperation filter, IOperation operation)
        {
            try
            {
                List<CarOperations> list_result = new List<CarOperations>();
                foreach (Cars car in cars)
                {
                    list_result.Add(filter(car, operation));
                    //operation.position++;
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

        /// <summary>
        /// Закрыть и сохранить вагон
        /// </summary>
        /// <param name="car"></param>
        /// <param name="dt_close"></param>
        /// <returns></returns>
        public int CloseSaveCar(Cars car, DateTime dt_close, bool rewrite)
        {
            try
            {
                car.CloseCar(dt_close, rewrite);
                return ef_rw.SaveCars(car);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CloseSaveCar(car={0}, dt_close={1})",
                    car, dt_close), servece_owner, eventID);
                return -1;
            }
        }

        public int CloseSaveCar(int id_car, DateTime dt_close, bool rewrite)
        {
            try
            {
                Cars car = ef_rw.GetCars(id_car);
                car.CloseCar(dt_close, rewrite);
                return ef_rw.SaveCars(car);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CloseSaveCar(id_car={0}, dt_close={1})",
                    id_car, dt_close), servece_owner, eventID);
                return -1;
            }
        }
        /// <summary>
        /// Закрыть и сохранить вагон
        /// </summary>
        /// <param name="car"></param>
        /// <param name="dt_close_operation"></param>
        /// <param name="dt_close_car"></param>
        /// <returns></returns>
        public int CloseSaveCar(Cars car, DateTime dt_close_operation, DateTime dt_close_car, bool rewrite)
        {
            try
            {
                car.CloseCar(dt_close_operation, dt_close_car, rewrite);
                return ef_rw.SaveCars(car);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CloseSaveCar(car={0}, dt_close_operation={1}, dt_close_operation={2})",
                    car, dt_close_operation, dt_close_car), servece_owner, eventID);
                return -1;
            }
        }
        /// <summary>
        /// Применить и сохранить свойства
        /// </summary>
        /// <param name="car"></param>
        /// <param name="natur_kis"></param>
        /// <param name="natur"></param>
        /// <param name="dt_inp_amkr"></param>
        /// <returns></returns>
        public int SetSaveCar(Cars car, int? natur_kis, int? natur, DateTime? dt_inp_amkr) { 
            try
            {
                car.SetCar(natur_kis, natur, dt_inp_amkr);
                return ef_rw.SaveCars(car);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SetSaveCar(car={0}, natur_kis={1}, natur={2}, dt_inp_amkr={3})",
                    car, natur_kis, natur, dt_inp_amkr), servece_owner, eventID);
                return -1;
            }        
        }
        /// <summary>
        /// Применить и сохранить свойства
        /// </summary>
        /// <param name="car"></param>
        /// <param name="dt_out_amkr"></param>
        /// <returns></returns>
        public int SetSaveCar(Cars car, DateTime? dt_out_amkr, int? natur_kis_out) { 
            try
            {
                car.SetCar(dt_out_amkr, natur_kis_out);
                return ef_rw.SaveCars(car);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SetSaveCar(car={0}, dt_out_amkr={1})",
                    car, dt_out_amkr), servece_owner, eventID);
                return -1;
            }        
        }
        /// <summary>
        /// Применить и сохранить свойства
        /// </summary>
        /// <param name="car"></param>
        /// <param name="delivery"></param>
        /// <returns></returns>
        public int SetSaveCar(Cars car, CarsOutDelivery delivery)
        { 
            try
            {
                car.SetCar(delivery);
                return ef_rw.SaveCars(car);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SetSaveCar(car={0}, delivery={1})",
                    car.GetFieldsAndValue(), delivery.GetFieldsAndValue()), servece_owner, eventID);
                return -1;
            }        
        }
        /// <summary>
        /// Сбросить посдеднюю операцию закрытия и сохранить вагон
        /// </summary>
        /// <param name="car"></param>
        /// <returns></returns>
        public int ClearCloseSaveCar(Cars car)
        { 
            try
            {
                car.ClearCloseCar();
                return ef_rw.SaveCars(car);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("ClearCloseCar(car={0})",
                    car.GetFieldsAndValue()), servece_owner, eventID);
                return -1;
            }        
        }
        /// <summary>
        /// Сбросить посдеднюю операцию закрытия и сохранить вагон
        /// </summary>
        /// <param name="id_car"></param>
        /// <returns></returns>
        public int ClearCloseSaveCar(int id_car)
        { 
            try
            {
                Cars car = ef_rw.GetCars(id_car);
                car.ClearCloseCar();
                return ef_rw.SaveCars(car);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("ClearCloseCar(id_car={0})",
                    id_car), servece_owner, eventID);
                return -1;
            }        
        }
        /// <summary>
        /// Удалить последнюю операцию, а предпоследнюю открыть
        /// </summary>
        /// <param name="id_car"></param>
        /// <returns></returns>
        public int DeleteLastOperationSaveCar(int id_car)
        {
            try
            {
                Cars car = ef_rw.GetCars(id_car);
                CarOperations del = car.GetLastOperations();
                CarOperations res_del = ef_rw.DeleteCarOperations(del.id);
                car.GetLastOperations().ClearCloseOperations();
                return ef_rw.SaveCars(car);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("DeleteLastOperationSaveCar(id_car={0})",
                    id_car), servece_owner, eventID);
                return -1;
            }
        }
        /// <summary>
        /// Удалить строку из справочника "Входящий вагон"
        /// </summary>
        /// <param name="id_car"></param>
        /// <returns></returns>
        public int DeleteSaveCar(int id_car)
        {
            try
            {
                Cars delete_car = ef_rw.GetCars(id_car);
                if (delete_car != null)
                {
                    Cars previous_car = delete_car.parent_id != null ? ef_rw.GetCars((int)delete_car.parent_id) : null;
                    Cars next_car = ef_rw.GetCarsOfParentID(delete_car.id);

                    int del_operation = ef_rw.DeleteCarOperationsOfCars(delete_car.id);
                    int del_inp = ef_rw.DeleteCarsInpDeliveryOfCars(delete_car.id);
                    int del_out = ef_rw.DeleteCarsOutDeliveryOfCars(delete_car.id);
                    Cars del = ef_rw.DeleteCars(delete_car.id);
                    int res_next_car = 0;
                    int res_previous_car = 0;
                    if (next_car != null)
                    {
                        next_car.parent_id = previous_car != null ? (int?)previous_car.id : null;
                        res_next_car = ef_rw.SaveCars(next_car);
                        //перезакрыть последнюю операцию previous_car от next_car
                        if (previous_car != null && next_car.dt_uz != null ) {
                            previous_car.CloseCar((DateTime)next_car.dt_uz, true);
                            res_previous_car = ef_rw.SaveCars(previous_car);
                        }
                    }
                    else
                    {
                        if (previous_car != null)
                        {
                            // Открыть последнюю операцию
                            previous_car.ClearCloseCar();
                            res_previous_car = ef_rw.SaveCars(previous_car);
                        }
                    }
                    
                    return (del_operation < 0 || del_inp < 0 || del_out < 0 || del == null || res_next_car < 0 || res_previous_car < 0) ? -1 : del.id;
                }
                return 0;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("DeleteSaveCar(id_car={0})", id_car), eventID);
                return -1;
            }
        }


        /// <summary>
        /// Коррекция номера позиции вагона на пути
        /// </summary>
        /// <param name="id_way"></param>
        /// <returns></returns>
        public List<CarOperations> CorrectPositionCarsOnWay(int id_way) { 
            List<CarOperations> list_position = ef_rw.GetOpenCarOperationsOfWay(id_way).OrderBy(p=>p.position).ToList();
            int position = 1;
            foreach(CarOperations operation in list_position){
                operation.position = position;
                position++;
            }
            return list_position;
        }

        #endregion

        #region Перенос вагонов из МТ в систему RailWay
        /// <summary>
        /// Перенести состав МТ с учетом указанной операции
        /// </summary>
        /// <param name="sostav"></param>
        /// <param name="filter"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        public int TransferArrivalSostavToRailWay(ArrivalSostav sostav, SetCarOperation filter, IOperation operation)
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
                int skip = 0;
                int error = 0;
                string message = null;
                //int close = 0;
                // Получим список отцепленных вагонов и закроем их
                List<int> not_nums = ef_mt.GetNotCarsOfOldArrivalSostav(sostav);

                int result_close = 0;
                if (not_nums != null && not_nums.Count() > 0)
                {
                    List<Cars> list_not_cars = ef_rw.GetCarsOfArrivalNum(sostav.IDArrival, not_nums.ToArray());
                    //List<CarOperations> list_operations = ExecOperation(list_not_cars, OperationClose, new OperationClose(sostav.DateTime));
                    List<CarOperations> list_operations = list_not_cars.CloseOperations(sostav.DateTime, true);
                    result_close = SaveChanges(list_operations);
                }
                // Поставим новые
                // List<Cars> list_result = new List<Cars>();
                foreach (ArrivalCars car in sostav.ArrivalCars.ToList())
                {
                    DateTime dt_start = DateTime.Now;
                    message += car.Num.ToString() + " - ";
                    Cars car_new = SetCarsToRailWay(car);
                    if (car_new != null)
                    {
                        int res = ef_rw.SaveCarsNoDetect(car_new);
                        if (res > 0)
                        {
                            transfer++;
                            message += res.ToString();
                        }
                        else { error++; message += res.ToString(); }
                    }
                    else {
                        skip++;
                        message += "null";
                    }
                    message += "; ";
                    TimeSpan ts = DateTime.Now - dt_start;
                    Console.WriteLine(String.Format("Перенос вагона №{0}, время выполнения: {1}:{2}:{3}({4})", car.Num, ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds));
                }
                count = sostav.ArrivalCars != null ? sostav.ArrivalCars.Count() : 0;
                string mess = String.Format("Перенос состава из базы данных [MT.Arrival] (id состава: {0}, id прибытия {1}, индекс: {2}, дата операции: {3}) в систему RailWay. Определенно для переноса {4} вагона(ов), перенесено {5}, пропущено {6}, ошибок переноса {7}, закрыто по ТСП {8}.",
                    sostav.ID, sostav.IDArrival, sostav.CompositionIndex, sostav.DateTime, count, transfer, skip, error, result_close);
                mess.WriteInformation(servece_owner, eventID);
                if (error > 0) { mess.WriteEvents(message, servece_owner, eventID); }
                return transfer;
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

        #region Входящие вагоны
        ///// <summary>
        ///// Поставить вагон в сисему RailWay по данным ArrivalCars
        ///// </summary>
        ///// <param name="prom_vagon"></param>
        ///// <param name="set_operation"></param>
        ///// <param name="operation"></param>
        ///// <returns></returns>
        //public Cars SetCarsToRailWay(PromVagon prom_vagon, SetCarOperation set_operation, IOperation operation)
        //{
        //    try
        //    {
        //        CarsInpDelivery delivery = CreateCarsInpDelivery(prom_vagon);
        //        return SetCarsToRailWay(prom_vagon.N_VAG, prom_vagon.N_NATUR *-1, delivery, set_operation, operation);
        //    }
        //    catch (Exception e)
        //    {
        //        e.WriteErrorMethod(String.Format("SetCarsToRailWay(prom_vagon={0}, set_operation={1}, operation={2})", prom_vagon.GetFieldsAndValue(), set_operation, operation), servece_owner, eventID);
        //        return null;
        //    }
        //}
        /// <summary>
        /// Поставить вагон в сисему RailWay по данным ArrivalCars
        /// </summary>
        /// <param name="arr_car"></param>
        /// <returns></returns>
        public Cars SetCarsToRailWay(ArrivalCars arr_car)
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
                return SetCarsToRailWay(arr_car, OperationSetWay, new OperationSetWay(way.id, arr_car.Position, arr_car.DateOperation, null, null));
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SetCarsToRailWay(arr_car={0})", arr_car.GetFieldsAndValue()), servece_owner, eventID);
                return null;
            }
        }
        /// <summary>
        /// Поставить вагон в сисему RailWay на указаный путь по данным ArrivalCars
        /// </summary>
        /// <param name="arr_car"></param>
        /// <param name="set_operation"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        public Cars SetCarsToRailWay(ArrivalCars arr_car, SetCarOperation set_operation, IOperation operation)
        {
            try
            {
                CarsInpDelivery delivery = CreateCarsInpDelivery(arr_car);
                return SetCarsToRailWay(arr_car.Num, arr_car.IDSostav, delivery, set_operation, operation);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SetCarsToRailWay(arr_car={0}, set_operation={1}, operation={2})", arr_car.GetFieldsAndValue(), set_operation, operation), servece_owner, eventID);
                return null;
            }
        }
        /// <summary>
        /// Поставить вагон в сисему RailWay на указаный путь 
        /// </summary>
        /// <param name="num"></param>
        /// <param name="id_sostav"></param>
        /// <param name="delivery"></param>
        /// <param name="set_operation"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        public Cars SetCarsToRailWay(int num, int id_sostav, CarsInpDelivery delivery, SetCarOperation set_operation, IOperation operation)
        {
            try
            {
                string mess = String.Format("Выполнение операции {0} по вагону {1} состава {2} ", operation.GetFieldsAndValue(), num, id_sostav);
                Cars car = null;
                Cars car_old = null;
                // Есть по этому вагону открытая операция
                CarOperations last_operation = GetLastOpenOperation(IsOpenAllOperationOfNum(num), true); // проверить вагон в системе 
                if (last_operation != null)
                {
                    // Есть открытая операция
                    if (last_operation.Cars.id_arrival == delivery.id_arrival)
                    {
                        if (last_operation.Cars.id_sostav >= id_sostav)
                        {
                            if (this.log_detali) { (mess += String.Format(" - отменено, в системе RailWay такой вагон существует id_car:{0}", last_operation.Cars.id)).WriteInformation(servece_owner, eventID); }
                            //return last_operation.Cars; // Пытаемся поставить старый вагон. Возвращаем Cars без выполнения операции
                            return null; // Пытаемся поставить старый вагон. Возвращаем Cars без выполнения операции
                        }
                        else
                        {
                            car = last_operation.Cars;
                        }
                    }
                    else
                    {
                        if (this.log_detali)
                        {
                            (mess += String.Format(" - в системе RailWay будет закрыта старая запись 'Входящего вагона' id_car:{0}, и создана новая.", last_operation.Cars.id)).WriteInformation(servece_owner, eventID);
                        }
                        // Открыта операция над старим Входящим вагоном
                        //car_old = last_operation.Cars.CloseCar(DateTime.Now);
                        car_old = last_operation.Cars.CloseCar(operation.dt_set, true);
                        ef_rw.SaveCars(car_old);
                    }
                }
                else
                {
                    // Нет открытых операций
                    // найти последний закрыты
                    car_old = ef_rw.GetLastCarsOfNum(num);
                }
                // Если Входящего вагона с указаным номером и id прибытием нет, создадим 
                if (car == null)
                {
                    // Проверим наличие вагона в справочнике если нет создадим + если есть из КИС перенесем аренды и владельца
                    ReferenceCars ref_car = rw_ref.GetReferenceCarsOfNum(num, delivery.id_arrival, operation.dt_set, (int)delivery.id_country, true, true);
                    // Создадим строку 
                    car = new Cars()
                    {
                        id = 0,
                        id_arrival = delivery.id_arrival,
                        num = num,
                        dt_inp_amkr = null,
                        dt_out_amkr = null,
                        natur = null,
                        natur_kis = null,
                        parent_id = car_old != null ? (int?)car_old.id : null,
                    };
                    if (this.log_detali)
                    {
                        (mess += String.Format(" - в системе RailWay создана новая запись 'Входящего вагона'")).WriteInformation(servece_owner, eventID);
                    }
                }
                // Входяшие поставки
                if (car.CarsInpDelivery == null || car.CarsInpDelivery.Count() == 0)
                { car.CarsInpDelivery.Add(delivery); }
                car.id_sostav = id_sostav;
                car.dt_uz = operation.dt_set;
                // Закрываем прибытие (по вагону пришло ТСП, а вагон уже принят на АМКР)
                if (car.natur != null | car.natur_kis != null)
                {
                    int res_close_mt = ef_mt.CloseArrivalCars(car.id_sostav, car.num, car.natur != null ? (int)car.natur : (int)car.natur_kis, (DateTime)car.dt_uz);
                }
                // Закрываем старую, создаем новую операцию
                bool is_set = car.IsSetWay((int)operation.id_way, null);
                bool is_pass = car.IsPassWay((int)operation.id_way, null);
                if ((is_set) | (!is_set & !is_pass))
                {
                    CarOperations oper = set_operation(car, operation);
                    //car.CarOperations.Add(oper);
                }
                return car;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SetCarsToRailWay(num={0}, id_sostav={1}, delivery={2}, set_operation={3}, operation={4})", num, id_sostav, delivery, set_operation, operation), servece_owner, eventID);
                return null;
            }
        }
        #endregion

        #region Входящие поставки
        /// <summary>
        /// Создать справочник SAP Входящие поставки по данным ArrivalCars
        /// </summary>
        /// <param name="num"></param>
        /// <param name="id_arrival"></param>
        /// <param name="car_mt"></param>
        /// <returns></returns>
        public CarsInpDelivery CreateCarsInpDelivery(ArrivalCars car_mt)
        {
            try
            {
                return CreateCarsInpDelivery(car_mt.Num, car_mt.ArrivalSostav.IDArrival, car_mt.DateOperation, car_mt.CompositionIndex, car_mt.Position, car_mt.CountryCode, car_mt.CargoCode, car_mt.Weight, car_mt.Consignee);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CreateCarsInpDelivery(car_mt={0})", car_mt.GetFieldsAndValue()), servece_owner, eventID);
                return null;
            }
        }
        /// <summary>
        /// Создать справочник SAP Входящие поставки
        /// </summary>
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
        public CarsInpDelivery CreateCarsInpDelivery(int num, int id_arrival, DateTime dt_operation, string index, int position, int CountryCode, int CargoCode, float Weight, int Consignee)
        {
            try
            {
                Cars car = ef_rw.GetCarsOfArrivalNum(id_arrival, num);
                return new CarsInpDelivery()
                {
                    id = 0,
                    id_car = car != null ? car.id : 0,
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
                    id_country = rw_ref.GetIDReferenceCountryOfCodeSNG(CountryCode),
                    weight_cargo = (decimal)Weight,
                    cargo_code = CargoCode,
                    id_cargo = rw_ref.GetIDReferenceCargoOfCodeETSNG(CargoCode),
                    consignee = Consignee,
                };

            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CreateCarsInpDelivery(num={0}, id_arrival={1}, dt_operation={2}, index={3}, position={4}, CountryCode={5}, CargoCode={6}, Weight={7}, Consignee={8})"
                    , num, id_arrival, dt_operation, index, position, CountryCode, CargoCode, Weight, Consignee), servece_owner, eventID);
                return null;
            }
        }
        #endregion

        #region Исходящие вагоны

        //public int SetCarToSendingWayRailWay(int id_car, int natur, DateTime dt_out_amkr, int id_sending_way, CarsOutDelivery delivery)
        //{
        //    try
        //    {
        //        RWOperation rw_oper = new RWOperation(this.servece_owner);
        //        EFRailWay ef_rw = new EFRailWay();
        //        List<Ways> list_sending_ways_uz = ef_rw.GetWays().Where(w => w.Stations.station_uz == true & w.num == "2").ToList();

        //        // Вагон статит на пути станции АМКР, если да отправить на УЗ, обновим исх .поставку
        //        int id_car_res = SetCarSendingWayUZRailWay(id_car, natur, dt_out_amkr, id_sending_way, delivery);
        //        if (id_car_res < 0) return id_car_res; // Ошибка                
        //        if (id_car_res > 0)
        //        {
        //            // Вагон стоял на станции АМКР, и отправлен на УЗ
        //            // получим новую операцию
        //            return id_car_res;
        //        }
        //        else
        //        {
        //            // Вагон НЕ стоял на станции АМКР
        //            // Вагон статит на путях принятия с УЗ?, если да поставить на путь станции и вернуть id CAR
        //            id_car_res = SetCarArrivalWayRailWay(id_car, dt_out_amkr);
        //            if (id_car_res < 0) return id_car_res; // Ошибка
        //            if (id_car_res > 0)
        //            {
        //                // Вагон стоял на путях принятия с УЗ и принят на амкр
        //                // Отправить на УЗ, обновим исх .поставку
        //                id_car_res = SetCarSendingWayUZRailWay(id_car, natur, dt_out_amkr, id_sending_way, delivery);
        //                //if (id_car < 0) return id_car; // Ошибка
        //                return id_car_res;
        //            }
        //            else
        //            {
        //                // Вагон НЕ стотял на путях принятия с УЗ
        //                // Вагон статит на путях принятия с АМКР?
        //                CarOperations last_operation = rw_oper.GetLastOpenOperation(rw_oper.IsOpenAllOperationOfID(id_car), true);
        //                int id_way_car = last_operation.IsSetWay(list_sending_ways_uz.Select(w => w.id).ToArray(), null);
        //                if (id_way_car > 0)
        //                {
        //                    // Вагон статит на путях принятия с АМКР
        //                    //..
        //                    // Обновим исходящие поставки
        //                    // Вагон статит на АМКР
        //                    Cars car = last_operation.Cars; // Определим вагон
        //                    // Определим исходящие поставки
        //                    //CarsOutDelivery delivery = CreateCarsOutDelivery(car.num, natur, dt_out_amkr);
        //                    // Исходящая поставка
        //                    int res_car = rw_oper.SetSaveCar(car, delivery);
        //                    return res_car;
        //                }
        //                else
        //                {
        //                    // Вагон НЕ статит на путях принятия с АМКР
        //                    //..
        //                    // Исключение!! у вагона нет другого варианта
        //                    return -1;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        e.WriteErrorMethod(String.Format("SetCarToWayRailWay()"), servece_owner, eventID);
        //        return -1;
        //    }
        //}

        //public int SetCarSendingWayUZRailWay(int id_car, int natur, DateTime dt_out_amkr, int id_sending_way, CarsOutDelivery delivery)
        //{
        //    try
        //    {
        //        EFRailWay ef_rw = new EFRailWay();
        //        RWOperation rw_oper = new RWOperation(this.servece_owner);
        //        List<Ways> list_all_ways_station_amkr = new List<Ways>();
        //        list_all_ways_station_amkr = ef_rw.GetWaysOfStationAMKR().ToList();

        //        CarOperations last_operation = rw_oper.GetLastOpenOperation(rw_oper.IsOpenAllOperationOfID(id_car), true);
        //        // проверить вагон в системе, стоит он на путях станций АМКР
        //        int id_way_car = last_operation.IsSetWay(list_all_ways_station_amkr.Select(w => w.id).ToArray(), null);
        //        if (id_way_car > 0)
        //        {
        //            // Вагон статит на АМКР
        //            Cars car = last_operation.Cars; // Определим вагон
        //            // Исходящая поставка
        //            car.SetCar(delivery);
        //            int res_car = rw_oper.ExecSaveOperation(car, rw_oper.OperationSendingUZWay, new OperationSendingUZWay(id_sending_way, dt_out_amkr, dt_out_amkr, natur, null, null));
        //            Console.WriteLine("Вагон {0} - cтоит на станции АМКР, и будет сдан на УЗ - результат переноса {1}", car.num, res_car);
        //            return res_car;
        //        } return 0;
        //    }
        //    catch (Exception e)
        //    {
        //        e.WriteErrorMethod(String.Format("SetCarSendingWayUZRailWay()"), servece_owner, eventID);
        //        return -1;
        //    }
        //}

        ///// <summary>
        ///// Найти информацию о прибытии вагона на АМКР, принять вагон на АМКР с путей "для отправки на АМКР" станции УЗ
        ///// </summary>
        ///// <param name="last_operation"></param>
        ///// <param name="pnh_arrival"></param>
        ///// <returns></returns>
        //public int SetCarArrivalWayRailWay(int id_car, DateTime dt_out_amkr)
        //{
        //    try
        //    {
        //        RWReference rw_ref = new RWReference(base.servece_owner, true); // создавать содержимое справочника из данных КИС
        //        EFRailWay ef_rw = new EFRailWay();


        //        Cars car = ef_rw.GetCars(id_car);

        //        int id_stations_rw = rw_ref.GetIDStationsOfKIS(pnh_arrival.K_ST);
        //        if (id_stations_rw <= 0)
        //        {
        //            //String.Format(mess_error_upd_sostav + mess_upd + " - ID станции: {0} не определён в справочнике системы RailWay", bas_sostav.id_station_kis).WriteError(servece_owner, eventID);
        //            return -1;
        //        }
        //        if (id_stations_rw == 26) id_stations_rw = 27; // Коррекция Промышленная Керамет -> 'это промышленная
        //        // Определим путь на станции система RailCars
        //        int id_ways_rw = rw_ref.GetIDDefaultWayOfStation(id_stations_rw, null);
        //        if (id_ways_rw <= 0)
        //        {
        //            //String.Format(mess_error_upd_sostav + mess_upd + " - ID пути: {0} станции: {1} не определён в справочнике системы RailWay", bas_sostav.way_num, bas_sostav.id_station_kis).WriteError(servece_owner, eventID);
        //            return -1;
        //        }
        //        return SetCarArrivalWayRailWay(id_car, pnh_arrival.N_NATUR, (DateTime)pnh_arrival.GetPRDateTime(), id_ways_rw);
        //    }
        //    catch (Exception e)
        //    {
        //        e.WriteErrorMethod(String.Format("SetCarArrivalWayRailWay()"), servece_owner, eventID);
        //        return -1;
        //    }
        //}
        ///// <summary>
        ///// Принять на АМКР с путей "для отправки на АМКР" станции УЗ
        ///// </summary>
        ///// <param name="last_operation"></param>
        //public int SetCarArrivalWayRailWay(int id_car, int natur, DateTime dt_amkr, int id_way)
        //{
        //    try
        //    {
        //        EFRailWay ef_rw = new EFRailWay();
        //        RWOperation rw_oper = new RWOperation(this.servece_owner);
        //        EFMetallurgTrans ef_mt = new EFMetallurgTrans();

        //        CarOperations last_operation = rw_oper.GetLastOpenOperation(rw_oper.IsOpenAllOperationOfID(id_car), true);

        //        List<Ways> list_arrival_ways_uz = ef_rw.GetWays().Where(w => w.Stations.station_uz == true & w.num == "1").ToList();
        //        int id_way_car = last_operation.IsSetWay(list_arrival_ways_uz.Select(w => w.id).ToArray(), null);
        //        if (id_way_car > 0)
        //        {
        //            // Вагон статит на путях "для отправки на АМКР"
        //            Cars car = last_operation.Cars; // Определим вагон
        //            int res_car = rw_oper.ExecSaveOperation(car, rw_oper.OperationArrivalUZWay, new OperationArrivalUZWay(id_way, dt_amkr, dt_amkr, natur, null, null));
        //            if (res_car > 0)
        //            {
        //                // Закрываем прибытие
        //                int res_close_mt = ef_mt.CloseArrivalCars(car.id_sostav, car.num, natur, dt_amkr);
        //            }
        //            Console.WriteLine("Вагон {0} - cтоит в прибытии станции УЗ по которой можно получить вагон на станцию АМКР - результат переноса {1}", car.num, res_car);
        //            return res_car;

        //        } return 0;
        //    }
        //    catch (Exception e)
        //    {
        //        e.WriteErrorMethod(String.Format("SetCarArrivalWayRailWay()"), servece_owner, eventID);
        //        return (int)errorTransfer.global;
        //    }
        //}

        #endregion

        #region Исходящие поставки
        public CarsOutDelivery CreateCarsOutDelivery(int num, int? id_tupik, int? CountryCodeISO, int? StationCode, string note, int CargoCode, float Weight)
        {
            try
            {
                return new CarsOutDelivery()
                {
                    id = 0,
                    id_car = 0,
                    num_nakl_sap = null,
                    id_tupik = id_tupik,
                    id_country_out = CountryCodeISO != null ? rw_ref.GetIDReferenceCountryOfCodeISO((int)CountryCodeISO): CountryCodeISO,
                    id_station_out = StationCode != null ? rw_ref.GetIDGetReferenceStationOfCodecs((int)StationCode) : StationCode, //
                    note = note,
                    cargo_code = CargoCode,
                    id_cargo = rw_ref.GetIDReferenceCargoOfCodeETSNG(CargoCode),
                    weight_cargo = (decimal)Weight,
                    num_doc_reweighing_sap = null,
                    dt_doc_reweighing_sap = null,
                    weight_reweighing_sap = null,
                    dt_reweighing_sap = null,
                    post_reweighing_sap = null,
                };

            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CreateCarsOutDelivery(num={0}, id_tupik={1}, CountryCodeISO={2}, StationCode={3}, note={4}, CargoCode={5}, Weight={6})"
                    , num, id_tupik, CountryCodeISO, StationCode, note, CargoCode, Weight), servece_owner, eventID);
                return null;
            }
        }
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

        public int SaveChanges(Cars car)
        {
            try
            {
                return ef_rw.SaveCars(car);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveChanges(car={0})", car), eventID);
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

        public Cars GetCars(int id_car) {
            return ef_rw.GetCars(id_car);
        }

        public List<Cars> GetCarsOfNum(int num) {
            return ef_rw.GetCarsOfNum(num).ToList();
        }

        public Cars GetNextCars(int id_car) {
            return ef_rw.GetCarsOfParentID(id_car);
        }

        public void Refresh() {
            ef_rw.RefreshAll();
        }

        /// <summary>
        /// Получить последнюю операцию по номеру вагона
        /// </summary>
        /// <param name="num_car"></param>
        /// <returns></returns>
        //public CarOperations GetLastOperation(int num_car)
        //{
        //    EFRailWay ef_rw = new EFRailWay();
        //    List<CarOperations> list_open_operation = ef_rw.query_GetOpenOperationOfNumCar(num_car).ToList();
        //}

        /// <summary>
        /// Получить последнюю открытую операцию по номеру вагона, если их много сделать коррекцию (correct=true - закрыть старые операции)
        /// </summary>
        /// <param name="num_car"></param>
        /// <param name="correct"></param>
        /// <returns></returns>
        public CarOperations GetLastOpenOperation(int num_car, bool correct) {

            EFRailWay1 ef_rw = new EFRailWay1();
            List<CarOperations> list_open_operation = ef_rw.query_GetOpenOperationOfNumCar(num_car).ToList();
            if (list_open_operation == null) return null;
            if (list_open_operation.Count() == 1) return list_open_operation.FirstOrDefault();
            CarOperations last_open_operation = null;            
            // Открытые операции на станции
            CarOperations last_station_operation = list_open_operation.IsOpenOperation(Filters.IsOpenWay).OrderByDescending(o => o.dt_inp_way).FirstOrDefault();
            // Открытые операции отправленее вагоны            
            CarOperations last_sending_operation = list_open_operation.IsOpenOperation(Filters.IsOpenSending).OrderByDescending(o => o.send_dt_inp_way).FirstOrDefault();

            if (last_station_operation == null && last_sending_operation != null) last_open_operation = last_sending_operation;
            if (last_station_operation != null && last_sending_operation == null) last_open_operation = last_station_operation;
            if (last_station_operation != null && last_sending_operation != null) {
                if (last_station_operation.dt_inp_way >= last_sending_operation.send_dt_inp_way)
                {
                    last_open_operation = last_station_operation;
                }
                else {
                    last_open_operation = last_sending_operation;
                };
            };
            list_open_operation.Remove(last_open_operation);
            // Закрыть операции, выполнить коррекции
            if (correct) { 
                // TODO: реализовать код
            }
            return last_open_operation;
        }

        /// <summary>
        /// Выполнить операцию отправки вагона из пути "Прибытия на АМКР" на путь "Отправки на УЗ"
        /// </summary>
        /// <param name="code_station_uz"></param>
        /// <param name="num_car"></param>
        /// <returns></returns>
        public int OperationArrivalUZToSendingUZ(int code_station_uz, int num_car)
        {
            EFRailWay1 ef_rw = new EFRailWay1();
            RWReference rw_ref = new RWReference(true);
            
            
            Stations station = rw_ref.GetStationsUZ(code_station_uz, true);
            Ways way_from = ef_rw.GetWaysOfArrivalUZ(station.id);
            Ways way_on = ef_rw.GetWaysOfSendingUZ(station.id);
            // Найти вагон в системе Railway
            CarOperations last_operation = GetLastOpenOperation(num_car, true);
            if (last_operation == null)
            { 
                
            }
            //Cars car
            return 0;
        }

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
