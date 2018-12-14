using EFRW.Entities;
using MessageLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW
{
    public static class RWHelpers
    {
        private static eventID eventID = eventID.RW_RWHelpers;

        public delegate bool IsFilterStatusOperation(CarOperations operation);

        /// <summary>
        /// Фильтр операция открыта
        /// </summary>
        /// <param name="operation"></param>
        /// <returns></returns>
        public static bool IsOpen(this CarOperations operation)
        {
            return operation.dt_inp != null && operation.dt_out == null ? true : false;
        }
        /// <summary>
        /// Фильтр операция закрыта
        /// </summary>
        /// <param name="operation"></param>
        /// <returns></returns>
        public static bool IsClose(this CarOperations operation)
        {
            return operation.dt_inp != null && operation.dt_out != null ? true : false;
        }
        /// <summary>
        /// Выбрать опрерации согласно фильтра
        /// </summary>
        /// <param name="list"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static List<CarOperations> IsStatusOperatio(this IEnumerable<CarOperations> list, IsFilterStatusOperation filter)
        {
            List<CarOperations> result = new List<CarOperations>();
            foreach (CarOperations oper in list)
            {
                if (filter(oper))
                {
                    result.Add(oper);
                }
            }
            return result;
        }

        /// <summary>
        /// Вернуть последнюю операцию из списка операций
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static CarOperations GetLastOperation(this IEnumerable<CarOperations> list)
        {
            try
            {
                if (list == null) return null;
                return list.OrderByDescending(o => o.dt_inp).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetLastOperation(list={0})", list), eventID);
                return null;
            }
        }
        /// <summary>
        /// Вернуть последнюю операцию из строки "Внутренего перемещения вагона"
        /// </summary>
        /// <param name="car_internal"></param>
        /// <returns></returns>
        public static CarOperations GetLastOperation(this CarsInternal car_internal)
        {
            return car_internal.CarOperations.GetLastOperation();
        }

        public static CarOperations GetLastOpenOperation(this IEnumerable<CarOperations> list)
        {
            try
            {
                if (list == null) return null;
                return list.IsStatusOperatio(RWHelpers.IsOpen).OrderByDescending(o => o.dt_inp).GetLastOperation();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetLastOpenOperation(list={0})", list), eventID);
                return null;
            }
        }
        /// <summary>
        /// Вернуть дату и время операции (если не закрыта берется дата прибытия, если закрыта берется дата отправки)
        /// </summary>
        /// <param name="operation"></param>
        /// <returns></returns>
        public static DateTime? GetDateOperation(this CarOperations operation) {
            try
            {
                if (operation == null) return null;
                return operation.dt_out != null ? operation.dt_out : operation.dt_inp;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetDateOperation(operation={0})", operation), eventID);
                return null;
            }
        }

        /// <summary>
        /// Последняя операция
        /// </summary>
        /// <param name="operation"></param>
        /// <returns></returns>
        public static bool IsEndOperation(this CarOperations operation) {
            return operation.CarOperations1.Count() == 0 ? true : false;
        }
        /// <summary>
        /// Первая операция
        /// </summary>
        /// <param name="operation"></param>
        /// <returns></returns>
        public static bool IsStartOperation(this CarOperations operation) {
            return operation.CarOperations2 == null ? true : false;
        }
        /// <summary>
        /// Определить задвоение операций (Ошибка)
        /// </summary>
        /// <param name="operation"></param>
        /// <returns></returns>
        public static bool IsErrorOperation(this CarOperations operation) {
            return operation.CarOperations1.Count() >1 ? true : false;
        }

        /// <summary>
        /// Операция пренадлежит указаному пути
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="id_way"></param>
        /// <returns></returns>
        public static bool IsSetWayOperation(this CarOperations operation, int id_way) {
            return operation.id_way == id_way ? true : false;
        }
        /// <summary>
        /// Операция пренадлежит списку путей
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="ways"></param>
        /// <returns></returns>
        public static bool IsSetWayOperation(this CarOperations operation, List<Directory_Ways> ways) {
            foreach (Directory_Ways way in ways) {
                if (way.id == operation.id_way) return true;
            }
            return  false;
        }

        /// <summary>
        /// Операция пренадлежит указанной станции
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="id_internal_stations"></param>
        /// <returns></returns>
        public static bool IsSetStationOperation(this CarOperations operation, int id_internal_stations)
        {
            return operation.Directory_Ways.id_station == id_internal_stations ? true : false;
        }
        /// <summary>
        /// Операция пренадлежит станции УЗ
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="id_internal_stations"></param>
        /// <returns></returns>
        public static bool IsSetStationOperationUZ(this CarOperations operation)
        {
            return operation.Directory_Ways.Directory_InternalStations.station_uz ? true : false;
        }

        /// <summary>
        /// Вернуть последнюю запись из списка строк "Внутренего перемещения вагона"
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static CarsInternal GetLastCarsInternal(this IEnumerable<CarsInternal> list) {
            return list.OrderByDescending(c => c.id_arrival).FirstOrDefault();
        }


    }
}
