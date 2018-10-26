using libClass;
using EFMT.Entities;
using MessageLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFRW.Entities;
using EFRW.Concrete;

namespace RW
{
    public class RWOperations : IRWOperations
    {
        private eventID eventID = eventID.RW_RWOperations;
        protected service servece_owner = service.Null;
        bool log_detali = false;                            // Признак детального логирования
        private bool reference_kis = true;                  // Использовать справочники КИС

        public RWOperations()
        {

        }

        public RWOperations(service servece_owner)
        {
            this.servece_owner = servece_owner;
        }

        #region Вспомогательные методы

        #endregion

        #region МАНЕВРЫ НА ПУТЯХ УЗ

        #region ПУТЬ ПРИЕМКИ ИЗ АМКР
        /// <summary>
        /// Выполнить операцию ТСП на УЗ ( Автоматически при обновлении состава на УЗ Кривого Рога)
        /// </summary>
        /// <param name="car"></param>
        /// <returns></returns>
        public int TSPOnUZ(ArrivalCars car, DateTime dt_set)
        {
            try
            {
                RWReference rw_ref = new RWReference(servece_owner, reference_kis);
                Stations station_uz = rw_ref.GetStationsUZ(car.CompositionIndex, true);

                CarOperations current_operation = GetCurrentOperation(car.Num);
                // Операция нет или операция закрыта?
                if (current_operation==null || !current_operation.IsOpenAll()) {
                    // Создадим новую строку Car

                }
                // Выполним операцию ТСП на УЗ
                TSPOnUZ(current_operation, station_uz.id, dt_set);
                return 0;//!!!!!!!!!!!!!!!
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("TSPOnUZ(car={0}, dt_set={1})",
                    car.GetFieldsAndValue(), dt_set), servece_owner, eventID);
                return -1;
            }
        }
        /// <summary>
        /// Выполнить ТСП по УЗ Кривого Рога
        /// </summary>
        /// <param name="current_operation"></param>
        /// <param name="id_station_uz"></param>
        /// <returns></returns>
        public CarOperations TSPOnUZ(CarOperations current_operation, int id_station_uz, DateTime dt_set)
        {
            try
            {
                int id_status = 19; // ТСП по УЗ Кривого Рога
                return ManeuversArrivalAMKRToSendingUZ(current_operation, id_station_uz, dt_set, id_status, null);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("TSPOnUZ(current_operation={0}, id_station_uz={1}, id_station_uz={2})",
                    current_operation.GetFieldsAndValue(), id_station_uz, dt_set), servece_owner, eventID);
                return null;
            }
        }
        /// <summary>
        /// Выполнить маневр "на путь приема из АМКР"
        /// </summary>
        /// <param name="current_operation"></param>
        /// <param name="id_station_uz"></param>
        /// <param name="id_status"></param>
        /// <returns></returns>
        public CarOperations ManeuversArrivalAMKRToSendingUZ(CarOperations current_operation, int id_station_uz, DateTime dt_set, int id_status, int? id_conditions)
        {
            try
            {
                RWReference rw_ref = new RWReference(servece_owner, reference_kis);
                int way_inp = rw_ref.GetIDWayOfStation(id_station_uz, "1");
                int way_out = rw_ref.GetIDWayOfStation(id_station_uz, "2");
                if (!current_operation.IsSetWay(way_out)) {
                    if (current_operation.IsSetWay(way_inp))
                    {
                        // Вагон стоит на пути сделаем маневр
                        // Поставим вагон в конец состава
                        return OperationSetWayInEnd(current_operation, way_out, dt_set, id_status, id_conditions);
                    }
                    else { 
                        // Сообщение об ошибке!
                        current_operation = null;
                    }                    
                }
                return current_operation;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("ManeuversArrivalAMKRToSendingUZ(current_operation={0}, id_station_uz={1}, dt_set={2}, id_status=={3}, id_conditions=={4})",
                    current_operation.GetFieldsAndValue(), id_station_uz, dt_set, id_status, id_conditions), servece_owner, eventID);
                return null;
            }            
        }

        #endregion

        #region ПУТЬ ОТПРАВКИ НА АМКР

        #endregion
        //public CreateCars(CarOperations current_operation, int id_station_uz){
        //    //if(current_operation!=null)
        //}
        #endregion

        #region Операции
        /// <summary>
        /// Вернуть последнюю открытую операцию c коррекцией старых открытых операций (correct = true - закрыть старые открытые операции)
        /// </summary>
        /// <param name="num_car"></param>
        /// <param name="correct"></param>
        /// <returns></returns>
        public CarOperations GetOpenOperation(int num_car, bool correct)
        {
            try
            {
                EFRailWay ef_rw = new EFRailWay();
                List<CarOperations> list_open_operation = ef_rw.query_GetOpenOperationOfNumCar(num_car).ToList();
                if (list_open_operation == null) return null;
                if (list_open_operation.Count() == 1) return list_open_operation.FirstOrDefault();
                CarOperations last_open_operation = null;
                // Сравнить и вернуть последнюю открытую
                last_open_operation = list_open_operation.IsOpenOperation();
                // Удалить 
                list_open_operation.Remove(last_open_operation);
                // Закрыть операции, выполнить коррекции
                if (correct)
                {
                    // TODO: реализовать код
                }
                return last_open_operation;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetOpenOperation(num_car={0},  correct={1})",num_car,correct), servece_owner, eventID);
                return null;
            }
        }
        /// <summary>
        /// Вернуть последнюю открытую операцию (автоматически закрыть старые открытые опреации)
        /// </summary>
        /// <param name="num_car"></param>
        /// <returns></returns>
        public CarOperations GetOpenOperation(int num_car) {
            return GetOpenOperation(num_car, true);
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
                EFRailWay ef_rw = new EFRailWay();
                List<CarOperations> list_operation = ef_rw.GetCarOperationsOfNumCar(num_car).ToList();
                if (list_operation == null) return null;
                if (list_operation.Count() == 1) return list_operation.FirstOrDefault();
                // Сравнить и вернуть последнюю
                return list_operation.IsLastOperation();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetLastOperation(num_car={0})", num_car), servece_owner, eventID);
                return null;
            }
        }
        /// <summary>
        /// Вернуть текущую операцию
        /// </summary>
        /// <param name="num_car"></param>
        /// <returns></returns>
        public CarOperations GetCurrentOperation(int num_car)
        {
            try
            {
                CarOperations current_operation = GetOpenOperation(num_car);
                if (current_operation == null)
                {
                    current_operation = GetLastOperation(num_car);
                    if (log_detali && current_operation != null)
                    {
                        // Сообщение Ошибка «Если вагон есть в системе по нему не может быть закрыты все операции» 
                        String.Format("[GetCurrentOperation]. Вагон №{0}, id_car={1} - нет открытых операций, но есть закрытая операция id={2}.", num_car, current_operation.id_car, current_operation.id).WriteWarning(servece_owner, this.eventID);
                    }
                }
                return current_operation;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetLastOperation(num_car={0})", num_car), servece_owner, eventID);
                return null;
            }
        }
        
        #endregion

        #region
        /// <summary>
        /// Поставить вагон на путь в конец состава
        /// </summary>
        /// <param name="current_operation"></param>
        /// <param name="id_way"></param>
        /// <param name="dt_set"></param>
        /// <param name="id_status"></param>
        /// <param name="id_conditions"></param>
        /// <returns></returns>
        public CarOperations OperationSetWayInEnd(CarOperations current_operation, int id_way, DateTime dt_set, int? id_status, int? id_conditions)
        {
            try
            {
                if (current_operation == null) return null; // Не указана последняя операия      
                EFRailWay ef_rw = new EFRailWay();
                // Определим путь
                Ways way = ef_rw.GetWays(id_way);
                if (way == null) return null; // Путь не оределен
                // Закроем последнюю операцию
                int res_close = OperationCloseSave(current_operation, dt_set);
                if (res_close > 0)
                {
                    // Скорректируем на пути нумерацию
                    CorrectPositionStationWaySave((int)current_operation.id_way, 1, 1);
                    int? position = way.CarOperations.IsOpenOperation(Filters.IsOpenAll).Max(o => o.position);
                    int res_set = OperationSetWaySave(current_operation.id_car, 
                        current_operation.id, 
                        id_way, way.id_station, 
                        dt_set, 
                        position != null ? 1 : (int)++position, 
                        id_status,
                        id_conditions != null ? id_conditions : current_operation.id_car_conditions);
                    return res_set > 0 ? ef_rw.GetCarOperations(res_set) : null;
                }
                return null;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("OperationSetWay(current_operation={0}, id_way={1}, dt_set={2}, id_status={3}, id_conditions={4})",
                    current_operation, id_way,dt_set,id_status,id_conditions), servece_owner, eventID);
                return null;
            }
        }

        /// <summary>
        /// Поставить вагон на путь станции
        /// </summary>
        /// <param name="id_car"></param>
        /// <param name="parent_id"></param>
        /// <param name="id_way"></param>
        /// <param name="id_station"></param>
        /// <param name="dt_set"></param>
        /// <param name="position"></param>
        /// <param name="id_status"></param>
        /// <param name="id_сonditions"></param>
        /// <returns></returns>
        public CarOperations OperationSetWay(int id_car, int? parent_id, int id_way, int id_station, DateTime dt_set, int position, int? id_status, int? id_сonditions)
        {
            try
            {
                EFRailWay ef_rw = new EFRailWay();
                Ways way = ef_rw.GetWays(id_way);
                return new CarOperations()
                {
                    id = 0,
                    id_car = id_car,
                    id_car_conditions = id_сonditions,
                    id_car_status = id_status!=null ? id_status : way.id_car_status, // Если статус не указан тогда согласно пути
                    id_station = id_station,
                    dt_inp_station = dt_set,
                    dt_out_station = null,
                    id_way = id_way,
                    dt_inp_way = dt_set,
                    dt_out_way = null,
                    position = position,
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
                e.WriteErrorMethod(String.Format("OperationSetWay(id_car={0}, parent_id={1}, id_way={2}, id_station={3}, dt_set={4}, position={5}, id_status={6}, id_сonditions={7})",
                    id_car, parent_id, id_way, id_station, dt_set, position, id_status, id_сonditions), servece_owner, eventID);
                return null;
            }
        }
        /// <summary>
        /// Создать операцию вагон стоит на пути станции и сохранить
        /// </summary>
        /// <param name="id_car"></param>
        /// <param name="parent_id"></param>
        /// <param name="id_way"></param>
        /// <param name="id_station"></param>
        /// <param name="dt_set"></param>
        /// <param name="position"></param>
        /// <param name="id_status"></param>
        /// <param name="id_сonditions"></param>
        /// <returns></returns>
        public int OperationSetWaySave(int id_car, int? parent_id, int id_way, int id_station, DateTime dt_set, int position, int? id_status, int? id_сonditions)
        {
            try
            {
                EFRailWay ef_rw = new EFRailWay();
                CarOperations new_operation = OperationSetWay(id_car, parent_id, id_way, id_station, dt_set, position, id_status, id_сonditions);
                return ef_rw.SaveCarOperations(new_operation);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("OperationSetWaySave(id_car={0}, parent_id={1}, id_way={2}, id_station={3}, dt_set={4}, position={5}, id_status={6}, id_сonditions={7})",
                    id_car, parent_id, id_way, id_station, dt_set, position, id_status, id_сonditions), servece_owner, eventID);
                return -1;
            }
        }
        /// <summary>
        /// Вернуть закрытую операцию (на пути позиция вагонов не корректируется)
        /// </summary>
        /// <param name="current_operation"></param>
        /// <param name="dt_close"></param>
        /// <returns></returns>
        public CarOperations OperationClose(CarOperations current_operation, DateTime dt_close)
        {
            if (current_operation == null) return null;
            // Проверяем открытые
            if (current_operation.dt_inp_station != null & current_operation.dt_out_station == null)
            {
                current_operation.dt_out_station = dt_close;
            }
            if (current_operation.dt_inp_way != null & current_operation.dt_out_way == null)
            {
                current_operation.dt_out_way = dt_close;
            }
            if (current_operation.send_dt_inp_way != null & current_operation.send_dt_out_way == null)
            {
                current_operation.send_dt_out_way = dt_close;
            }
            return current_operation;
        }
        /// <summary>
        /// Вернуть закрытую операцию и сохранить (на пути позиция вагонов не корректируется)
        /// </summary>
        /// <param name="current_operation"></param>
        /// <param name="dt_close"></param>
        /// <returns></returns>
        public int OperationCloseSave(CarOperations current_operation, DateTime dt_close)
        {
            try
            {
                EFRailWay ef_rw = new EFRailWay();
                CarOperations close_operation = OperationClose(current_operation, dt_close);
                return ef_rw.SaveCarOperations(close_operation);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("OperationCloseSave(current_operation={0}, dt_close={1})",
                    current_operation, dt_close), servece_owner, eventID);
                return -1;
            }
        }
        /// <summary>
        /// Вернуть скорректированую позицию вагонов на пути
        /// </summary>
        /// <param name="id_way"></param>
        /// <param name="start_position_way"></param>
        /// <param name="start_position"></param>
        /// <returns></returns>
        public List<CarOperations> CorrectPositionStationWay(int id_way, int start_position_way, int start_position)
        {
            try
            {
                EFRailWay ef_rw = new EFRailWay();
                Ways way = ef_rw.GetWays(id_way);
                if (way == null || way.CarOperations == null) return null;
                List<CarOperations> list = way.CarOperations.IsOpenOperation(Filters.IsOpenWay).OrderBy(w => w.position).ToList();
                foreach (CarOperations operation in list)
                {
                    if (operation.position >= start_position_way)
                    {
                        operation.position = start_position;
                        start_position++;
                    }
                }
                return list;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CorrectPositionStationWay(id_way={0}, start_position_way={1}, start_position={2})",
                    id_way, start_position_way, start_position), servece_owner, eventID);
                return null;
            }
        }
        /// <summary>
        /// Вернуть скорректированую позицию вагонов на пути
        /// </summary>
        /// <param name="id_way"></param>
        /// <param name="start_position_way"></param>
        /// <param name="start_position"></param>
        /// <returns></returns>
        public int CorrectPositionStationWaySave(int id_way, int start_position_way, int start_position)
        {
            try
            {
                int count = 0;
                List<CarOperations> list_operation = CorrectPositionStationWay(id_way, start_position_way, start_position);
                if (list_operation == null) return count;
                EFRailWay ef_rw = new EFRailWay();
                foreach (CarOperations operation in list_operation)
                {
                    int res = ef_rw.SaveCarOperations(operation);
                    count += res > 0 ? 1 : 0;
                }
                return count;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CorrectPositionStationWaySave(id_way={0}, start_position_way={1}, start_position={2})",
                    id_way, start_position_way, start_position), servece_owner, eventID);
                return -1;
            }
        }

        #endregion
    }
}
