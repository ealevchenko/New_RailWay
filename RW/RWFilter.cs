using EFRW.Concrete;
using EFRW.Entities;
using MessageLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW
{
    public static class Filters
    {
        private static eventID eventID = eventID.RW_RWFilters;

        public delegate bool IsFilterOpen(CarOperations operation);

        public static bool IsOpenStation(this CarOperations operation)
        {
            if (operation.id_station != null & operation.dt_inp_station != null & operation.dt_out_station == null)
            {
                return true;
            }
            return false;
        }

        public static bool IsOpenWay(this CarOperations operation)
        {
            if (operation.id_way != null & operation.dt_inp_way != null & operation.dt_out_way == null)
            {
                return true;
            }
            return false;
        }

        public static bool IsOpenSending(this CarOperations operation)
        {
            if ((operation.send_id_station != null | operation.send_id_shop != null | operation.send_id_overturning != null)
                & operation.send_dt_inp_way != null & operation.send_dt_out_way == null)
            {
                return true;
            }
            return false;
        }

        public static bool IsOpenAll(this CarOperations operation)
        {
            return IsOpenStation(operation) | IsOpenWay(operation) | IsOpenSending(operation) ? true : false;
        }

        public static CarOperations IsOpenOperation(this Cars car, IsFilterOpen filter)
        {
            CarOperations last_operations = car.GetLastOperations();
            return (last_operations != null && filter(last_operations)) ? last_operations : null;
        }

        public static List<CarOperations> IsOpenOperation(this IEnumerable<CarOperations> list, IsFilterOpen filter)
        {
            List<CarOperations> result = new List<CarOperations>();
            foreach (CarOperations oper in list) {
                if (filter(oper)) {
                    result.Add(oper);
                }
            }
            return result;
        }

        /// <summary>
        /// Вернуть последнюю открытую операцию на пути станции или пути отправки
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static CarOperations IsOpenOperation(this IEnumerable<CarOperations> list)
        {
            if (list == null || list.Count() == 0) return null;
            // Открытые операции на станции
            CarOperations last_station_operation = list.IsOpenOperation(Filters.IsOpenWay).OrderByDescending(o => o.dt_inp_way).FirstOrDefault();
            // Открытые операции отправленее вагоны            
            CarOperations last_sending_operation = list.IsOpenOperation(Filters.IsOpenSending).OrderByDescending(o => o.send_dt_inp_way).FirstOrDefault();

            return IsLastOperation(last_station_operation, last_sending_operation);
        }

        /// <summary>
        /// Вернуть последнюю операцию на пути станции
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static CarOperations IsLastWayOperation(this IEnumerable<CarOperations> list)
        {
            if (list == null || list.Count() == 0) return null;
            // Открытые операции на станции
            CarOperations last_station_operation = list.Where(o => o.dt_inp_way != null).OrderByDescending(o => o.dt_inp_way).FirstOrDefault();
            return last_station_operation;

        }
        /// <summary>
        /// Вернуть последнюю операцию на пути отправки
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static CarOperations IsLastSendingOperation(this IEnumerable<CarOperations> list)
        {
            if (list == null || list.Count() == 0) return null;
            // Открытые операции на станции
            CarOperations last_sending_operation = list.Where(o => o.send_dt_inp_way!= null).OrderByDescending(o => o.send_dt_inp_way).FirstOrDefault();
            return last_sending_operation;
        }
        /// <summary>
        /// Вернуть последнюю операцию на пути станции или отправки
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static CarOperations IsLastOperation(this IEnumerable<CarOperations> list)
        {
            if (list == null || list.Count() == 0) return null;
            // Открытые операции на станции
            CarOperations last_station_operation = list.IsLastWayOperation();
            // Открытые операции отправленее вагоны            
            CarOperations last_sending_operation = list.IsLastSendingOperation();

            return IsLastOperation(last_station_operation, last_sending_operation);
        }

        /// <summary>
        /// Вернуть последнюю операцию между операциями путь станции и путь отправки
        /// </summary>
        /// <param name="station_operation"></param>
        /// <param name="sending_operation"></param>
        /// <returns></returns>
        public static CarOperations IsLastOperation(CarOperations station_operation, CarOperations sending_operation)
        {
            if (station_operation == null && sending_operation != null) return sending_operation;
            if (station_operation != null && sending_operation == null) return station_operation;
            if (station_operation != null && sending_operation != null)
            {
                if (station_operation.dt_inp_way >= sending_operation.send_dt_inp_way)
                {
                    return station_operation;
                }
                else
                {
                    return sending_operation;
                };
            };
            return null;
        }


        /// <summary>
        /// Вернуть последнюю операцию
        /// </summary>
        /// <param name="car"></param>
        /// <returns></returns>
        public static CarOperations GetLastOperations(this Cars car)
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


        #region Набор функций проверок операций над вагонами

        /// <summary>
        /// Проверка опирации, стоит(стоял set=false) вагон на указаном пути (по указаному времени)
        /// </summary>
        /// <param name="oper"></param>
        /// <param name="id_way"></param>
        /// <param name="dt"></param>
        /// <param name="set"></param>
        /// <returns></returns>
        public static bool IsSetOperation(this CarOperations oper, int id_way, DateTime? dt, bool set)
        {
            if (dt == null)
            {
                if (oper != null && (oper.id_way == id_way))
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
                if (oper != null && (oper.id_way == id_way))
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
        /// Операция, вагон стоит на указном пути
        /// </summary>
        /// <param name="oper"></param>
        /// <param name="id_way"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static bool IsSetWay(this CarOperations oper, int id_way, DateTime? dt)
        {
            return IsSetOperation(oper, id_way, dt, true);
        }
        /// <summary>
        /// Операция, вагон проходил на указном пути
        /// </summary>
        /// <param name="oper"></param>
        /// <param name="id_way"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static bool IsPassWay(this CarOperations oper, int id_way, DateTime? dt)
        {
            bool result = IsSetOperation(oper, id_way, dt, false);
            if (result) { return true; }
            return false;
        }
        /// <summary>
        /// Операция, вагон стоит на одном из указных путей
        /// </summary>
        /// <param name="oper"></param>
        /// <param name="id_way"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static int IsSetWay(this CarOperations oper, int[] id_way, DateTime? dt)
        {
            foreach (int id in id_way)
            {
                if (IsSetOperation(oper, id, dt, true))
                {
                    return id;
                }
            }
            return 0;

        }
        /// <summary>
        /// Операция, вагон стоял на одном из указных путей
        /// </summary>
        /// <param name="oper"></param>
        /// <param name="id_way"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static int IsPassWay(this CarOperations oper, int[] id_way, DateTime? dt)
        {
            foreach (int id in id_way)
            {
                if (IsSetOperation(oper, id, dt, false))
                {
                    return id;
                }
            }
            return 0;
        }

        /// <summary>
        /// Проверим текущее положение вагона - вагон стоит на указанном пути за указанное время
        /// </summary>
        /// <param name="car"></param>
        /// <param name="id_station"></param>
        /// <param name="id_way"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static bool IsSetWay(this Cars car, int id_way, DateTime? dt)
        {
            CarOperations last_operation = car.GetLastOperations();
            return IsSetWay(last_operation, id_way, dt);
        }
        /// <summary>
        /// Проходил вагон указаный путь за указанное время 
        /// </summary>
        /// <param name="car"></param>
        /// <param name="id_station"></param>
        /// <param name="id_way"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static bool IsPassWay(this Cars car, int id_way, DateTime? dt)
        {
            foreach (CarOperations oper in car.CarOperations.OrderByDescending(o => o.id).ToList())
            {
                bool result = IsPassWay(oper, id_way, dt);
                if (result) { return true; }
            }
            return false;
        }
        /// <summary>
        /// Проверим текущее положение вагона - вагон стоит на одном из пути за указанное время, если да возвращает id пути
        /// </summary>
        /// <param name="car"></param>
        /// <param name="id_way"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static int IsSetWay(this Cars car, int[] id_way, DateTime? dt)
        {
            CarOperations last_operation = car.GetLastOperations();
            return IsSetWay(last_operation, id_way, dt);

        }

        /// <summary>
        /// Проверка опирации, стоит(стоял set=false) вагон на указаной станции и пути (по указаному времени)
        /// </summary>
        /// <param name="oper"></param>
        /// <param name="id_station"></param>
        /// <param name="id_way"></param>
        /// <param name="dt"></param>
        /// <param name="set"></param>
        /// <returns></returns>
        public static bool IsSetOperation(this CarOperations oper, int id_station, int id_way, DateTime? dt, bool set)
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
        public static bool IsSetStationWay(this Cars car, int id_station, int id_way, DateTime? dt)
        {
            CarOperations last_operation = car.GetLastOperations();
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
        public static bool IsPassStationWay(this Cars car, int id_station, int id_way, DateTime? dt)
        {
            //if (IsSetStationWay(car, id_station, id_way, dt)) return false; // Вагон не проходил он сейчас стоит на станции.
            foreach (CarOperations oper in car.CarOperations.OrderByDescending(o => o.id).ToList())
            {
                bool result = IsSetOperation(oper, id_station, id_way, dt, false);
                if (result) { return true; }
            }
            return false;
        }

        public static List<CarOperations> GetPassStationWay(this Cars car, int id_station, int id_way)
        {
            List<CarOperations> list_result = new List<CarOperations>();
            foreach (CarOperations oper in car.CarOperations.OrderByDescending(o => o.id).ToList())
            {
                bool result = IsSetOperation(oper, id_station, id_way, null, false);
                if (result & oper.dt_out_station != null & oper.dt_out_way != null)
                {
                    // нужная операция и закрыта
                    list_result.Add(oper);
                }
            }
            return list_result;
        }


        #endregion

        #region Набор функций закрытия операций над вагонами
        public static CarOperations CloseOperations(this CarOperations operations, DateTime? dt_close, bool rewrite)
        {
            try
            {
                List<CarOperations> list = new List<CarOperations>();
                if (operations == null) return null;
                // Проверяем открытые
                if (operations.dt_inp_station != null & operations.dt_out_station == null) { 
                    operations.dt_out_station = dt_close != null ? dt_close : DateTime.Now; }
                if (operations.dt_inp_way != null & operations.dt_out_way == null) { 
                    operations.dt_out_way = dt_close != null ? dt_close : DateTime.Now; 
                    list = operations.Ways.CarOperations.Where(w=>w.dt_inp_way != null & w.dt_out_way == null).OrderBy(w=>w.position).ToList();
                }
                if (operations.send_dt_inp_way != null & operations.send_dt_out_way == null) { 
                    operations.send_dt_out_way = dt_close != null ? dt_close : DateTime.Now; 
                    list = operations.Ways.CarOperations.Where(w=>w.send_dt_inp_way != null & w.send_dt_out_way == null).OrderBy(w=>w.position).ToList();                
                }
                if (rewrite)
                {
                    // Проверяем закрытые перезаписать время закрытия операции
                    if (operations.dt_inp_station != null & operations.dt_out_station != null) { operations.dt_out_station = dt_close != null ? dt_close : operations.dt_out_station; }
                    if (operations.dt_inp_way != null & operations.dt_out_way != null) { operations.dt_out_way = dt_close != null ? dt_close : operations.dt_out_way; }
                    if (operations.send_dt_inp_way != null & operations.send_dt_out_way != null) { operations.send_dt_out_way = dt_close != null ? dt_close : operations.send_dt_out_way; }
                }
                int position = 1;
                foreach (CarOperations operation in list)
                {
                    operation.position = position;
                    position++;
                }

                return operations;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CloseOperations(operations={0}, dt_close={1}, rewrite={2})", operations, dt_close, rewrite), eventID);
                return null;
            }
        }

        public static CarOperations CloseOperations(this Cars car, DateTime? dt_close, bool rewrite)
        {
            try
            {
                return car.GetLastOperations().CloseOperations(dt_close, rewrite);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CloseOperations(car={0}, dt_close={1}, rewrite={2})", car, dt_close, rewrite), eventID);
                return null;
            }
        }

        public static List<CarOperations> CloseOperations(this IEnumerable<Cars> cars, DateTime? dt_close, bool rewrite)
        {
            try
            {
                List<CarOperations> result = new List<CarOperations>();
                foreach (Cars car in cars) { 
                    CarOperations operation_result = car.GetLastOperations().CloseOperations(dt_close, rewrite);
                    if (operation_result!=null){result.Add(operation_result);}
                }
                return result;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CloseOperations(cars={0}, dt_close={1}, rewrite={2})", cars, dt_close, rewrite), eventID);
                return null;
            }
        }

        public static Cars CloseCar(this Cars car, DateTime dt_close, bool rewrite)
        {
            if (car == null) return null;
            car.GetLastOperations().CloseOperations(dt_close, rewrite);
            if ((car.dt_close != null & rewrite) | (car.dt_close == null))
            {
                car.dt_close = dt_close;
                car.user_close = System.Environment.UserDomainName + @"\" + System.Environment.UserName;
            }
            return car;
        }

        public static Cars CloseCar(this Cars car, DateTime dt_close_operation, DateTime dt_close_car, bool rewrite)
        {
            if (car == null) return null;
            car.GetLastOperations().CloseOperations(dt_close_operation, rewrite);
            if ((car.dt_close != null & rewrite) | (car.dt_close == null))
            {
                car.dt_close = dt_close_car;
                car.user_close = System.Environment.UserDomainName + @"\" + System.Environment.UserName;
            }
            return car;
        }

        public static Cars CloseCar(this CarOperations operations, DateTime dt_close, bool rewrite)
        {
            if (operations == null) return null;
            Cars car = operations.Cars;
            return car.CloseCar(dt_close, rewrite);
        }        

        public static Cars CloseCar(this CarOperations operations, DateTime dt_close_operation, DateTime dt_close_car, bool rewrite)
        {
            if (operations == null) return null;
            Cars car = operations.Cars;
            return car.CloseCar(dt_close_operation, dt_close_car, rewrite);
        }        
        #endregion

        #region Набор функций сброса закрытия операций над вагонами
        public static CarOperations ClearCloseOperations(this CarOperations operations)
        {
            try
            {
                //List<CarOperations> list = null;
                if (operations == null) return null;
                if (operations.dt_inp_station != null & operations.dt_out_station != null) { operations.dt_out_station = null; }
                if (operations.dt_inp_way != null & operations.dt_out_way != null) { operations.dt_out_way = null; }
                if (operations.send_dt_inp_way != null & operations.send_dt_out_way != null) {  operations.send_dt_out_way = null; }
                return operations;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("ClearCloseOperations(operations={0})", operations), eventID);
                return null;
            }
        }

        public static Cars ClearCloseCar(this Cars car)
        {
            if (car == null) return null;
            car.GetLastOperations().ClearCloseOperations();
            car.dt_close = null;
            car.user_close = null;
            return car;
        }        
        #endregion

        public static Cars SetCar(this Cars car, int? natur_kis, int? natur, DateTime? dt_inp_amkr)
        {
            if (car == null) return null;
            car.dt_inp_amkr = dt_inp_amkr;
            if (car.natur_kis == null & natur_kis != null) 
            { car.natur_kis = natur_kis; }
            if (car.natur == null & natur != null) 
            { car.natur = natur; }
            return car;
        }
        /// <summary>
        /// Закрыть вагон
        /// </summary>
        /// <param name="car"></param>
        /// <param name="dt_out_amkr"></param>
        /// <returns></returns>
        public static Cars SetCar(this Cars car, DateTime? dt_out_amkr, int? natur_kis_out)
        {
            if (car == null) return null;
            car.dt_out_amkr = dt_out_amkr != null ? dt_out_amkr : car.dt_out_amkr; ;
            car.natur_kis_out = natur_kis_out!= null ? natur_kis_out : car.natur_kis_out;
            return car;
        }
        /// <summary>
        /// Добавить или обновить исходяшую поставку
        /// </summary>
        /// <param name="car"></param>
        /// <param name="delivery"></param>
        /// <returns></returns>
        public static Cars SetCar(this Cars car, CarsOutDelivery delivery)
        {
            if (car == null) return null;
            // Входяшие поставки
            if (car.CarsOutDelivery == null || car.CarsOutDelivery.Count() == 0)
            {
                // если нет добавим
                car.CarsOutDelivery.Add(delivery);
            }
            else
            {
                // Обновим 
                car.CarsOutDelivery.ToList()[0] = delivery;
            }
            return car;
        }


        /// <summary>
        /// Присвоить грузополучателя и готовность
        /// </summary>
        /// <param name="car"></param>
        /// <param name="id_consignee"></param>
        /// <param name="id_status"></param>
        /// <returns></returns>
        //public static Cars SetCar(this Cars car, int? id_consignee, int? id_status)
        //{
        //    if (car == null) return null;
            
            
            
        //    car.CarsInpDelivery = dt_inp_amkr;
        //    if (car.natur_kis == null & natur_kis != null) 
        //    { car.natur_kis = natur_kis; }
        //    if (car.natur == null & natur != null) 
        //    { car.natur = natur; }
        //    return car;
        //}


        //public delegate bool flOperation<T>(T ps, int id_station);

        //public static T[] FilterOperation<T>(this IEnumerable<T> source, flOperation<T> filter, int id)
        //{
        //    ArrayList aList = new ArrayList();
        //    foreach (T s in source)
        //    {
        //        if (filter(s, id))
        //        {
        //            aList.Add(s);
        //        }
        //    }
        //    return ((T[])aList.ToArray(typeof(T)));
        //}

        //public static bool IsSendingStation(Cars car, int id)
        //{
        //    EFRailWay ef_rw = new EFRailWay();
        //    CarOperations operation = ef_rw.GetCurrentCarOperationsOfCar(car.id);
        //    return operation != null && operation.send_id_station == id ? true:false;
        //}
    }
}
