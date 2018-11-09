using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libClass;
using MessageLog;
using EFMT.Entities;
using EFMT.Concrete;
using EFRW.Entities1;
using EFRW.Concrete;

namespace RW
{
    public class RWOperations : IRWOperations
    {
        private eventID eventID = eventID.RW_RWOperations;
        protected service servece_owner = service.Null;
        bool log_detali = true;                            // Признак детального логирования
        private bool reference_kis = true;                  // Использовать справочники КИС

        EFRailWay1 ef_rw = new EFRailWay1();
        RWReference rw_ref;

        public RWOperations()
        {
            rw_ref = new RWReference(servece_owner, reference_kis);
        }

        public RWOperations(service servece_owner)
        {
            this.servece_owner = servece_owner;
            rw_ref = new RWReference(servece_owner, reference_kis);
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
                //RWReference rw_ref = new RWReference(servece_owner, reference_kis);
                Stations station_uz = rw_ref.GetStationsUZ(car.CompositionIndex, true);

                CarOperations current_operation = GetCurrentOperation(car.Num);
                if (!current_operation.IsLessDateTime(dt_set)) {
                    if (log_detali)
                    {
                        string mess = String.Format("(Подсистема учета и контроля ж.д. транспорта на АМКР). Операция ТСП на УЗ (вагон №{0}, id_arrival={1}, id_sostav={2}, dt_set={3}) – Отменена, время текущей операция id = {4} больше операции ТСП"
                            , car.Num, car.ArrivalSostav.IDArrival, car.IDSostav, dt_set, current_operation.id);
                        mess.WriteWarning(servece_owner, eventID);
                    }
                    return 0; // Пропущен
                }
                CarsInpDelivery delivery = CreateCarsInpDelivery(car);
                // Операция нет или операция закрыта?
                if (current_operation==null || !current_operation.IsOpenAll()) {
                    // Создадим новую строку Car
                    current_operation = CreateCars(current_operation, car.IDSostav, station_uz.id, dt_set, delivery);
                }
                // Выполним операцию ТСП на УЗ
                CarOperations new_current_operation = TSPOnUZ(current_operation, station_uz.id, dt_set);
                //Вернем id current_operation если били изменения иначе 0 - операция не изменилась -1 - ошибка
                return new_current_operation!=null ? new_current_operation.id != current_operation.id ? new_current_operation.id : 0 : -1;
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
        /// Выполнить транзит по УЗ (Вагон отправлен на УЗ Оператором или автоматически)
        /// </summary>
        /// <param name="current_operation"></param>
        /// <param name="id_station_uz"></param>
        /// <param name="dt_set"></param>
        /// <returns></returns>
        public CarOperations PassingOnUZ(CarOperations current_operation, int id_station_uz, DateTime dt_set)
        {
            try
            {
                int id_status = 17; // Транзит по УЗ Кривого Рога
                return ManeuversArrivalAMKRToSendingUZ(current_operation, id_station_uz, dt_set, id_status, null);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("PassingOnUZ(current_operation={0}, id_station_uz={1}, id_station_uz={2})",
                    current_operation.GetFieldsAndValue(), id_station_uz, dt_set), servece_owner, eventID);
                return null;
            }
        }
        /// <summary>
        /// Выполнить транзит по УЗ (Вагон отправлен на УЗ Оператором или автоматически)
        /// </summary>
        /// <param name="current_operation"></param>
        /// <param name="id_station_uz"></param>
        /// <param name="dt_set"></param>
        /// <returns></returns>
        public CarOperations OperatorReturnOnUZ(CarOperations current_operation, int id_station_uz, DateTime dt_set)
        {
            try
            {
                int id_status = 18; // Оператор вернул вагон на УЗ (АМКР не принял вагон)
                return ManeuversArrivalAMKRToSendingUZ(current_operation, id_station_uz, dt_set, id_status, null);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("OperatorReturnOnUZ(current_operation={0}, id_station_uz={1}, id_station_uz={2})",
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
        /// <summary>
        /// Выполнить операцию Прибытие на УЗ Кривого Рога ( Автоматически при обновлении состава на УЗ Кривого Рога)
        /// </summary>
        /// <param name="car"></param>
        /// <returns></returns>
        public int ArrivalOnUZ(ArrivalCars car)
        {
            try
            {
                // Определим станцию
                //RWReference rw_ref = new RWReference(servece_owner, reference_kis);
                Stations station_uz = rw_ref.GetStationsUZ(car.CompositionIndex, true);
                // Определим текущую операцию
                CarOperations current_operation = GetCurrentOperation(car.Num);
                // Определим входящую поставку
                CarsInpDelivery delivery = CreateCarsInpDelivery(car);
                // Создадим Cars и вернем текущую операцию
                current_operation = CreateCars(current_operation, car.IDSostav, station_uz.id, car.DateOperation, delivery);

                return current_operation!=null ? current_operation.id_car : 0;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("ArrivalOnUZ(car={0})",
                    car.GetFieldsAndValue()), servece_owner, eventID);
                return -1; // Ошибка глобальная
            }
        }

        /// <summary>
        /// Создать Cars
        /// </summary>
        /// <param name="current_operation"></param>
        /// <param name="id_sostav"></param>
        /// <param name="id_station_uz"></param>
        /// <param name="dt_set"></param>
        /// <param name="delivery"></param>
        /// <returns></returns>
        public CarOperations CreateCars(CarOperations current_operation, int id_sostav, int id_station_uz,  DateTime dt_set, CarsInpDelivery delivery)
        {
            try
            {
                int? parent_id_car = null;
                // Текущая операция есть (открытая или закрытая)?
                if (current_operation != null)
                {
                    // Это новое ТСП или новый состав
                    if (current_operation.Cars.id_arrival == delivery.id_arrival)
                    {
                        // Это новое или старое ТСП?
                        if (current_operation.Cars.id_sostav >= id_sostav)
                        {
                            // Это старое ТСП
                            // Сообщение "Операция отменена «В систему этот вагон заходил. ТСП -устарело»"
                            if (log_detali) {
                                string mess = String.Format("(Подсистема учета и контроля ж.д. транспорта на АМКР). Операция создания новой строки Cars (вагон №{0}, id_arrival={1}, id_sostav={2}, dt_set={3}, id текущей операции = {4}) – Отменена, ТСП устарело."
                                    ,delivery.num_car,delivery.id_arrival,id_sostav,dt_set,current_operation.id);
                                mess.WriteWarning(servece_owner, eventID);
                            }
                            return null; // Ошибка Операция отменена
                        }
                        else { 
                            // Обновим входящую поставку
                            int res = CarsUpdateSave(current_operation.Cars.id, id_sostav, dt_set);
                            if (res > 0)
                            {
                                int id_new_delivery = AddCarsInpDeliverySave(current_operation.Cars.id, delivery);
                                int res_close_mt = CloseArrivalCars(current_operation.Cars.id);
                                return GetOperation(current_operation.id); // Вернем текушую операцию
                            }
                            return null; // Ошибка нет обновления
                        }
                    }
                    else { 
                        // Это новый состав
                        // Проверим этот состав уже заходил
                        Cars car = ef_rw.GetCarsOfArrivalNum(delivery.id_arrival, delivery.num_car);
                        if (car != null)
                        {
                            // Сообщение "Операция отменена «В систему этот вагон c id_arrival заходил и был закрыт.»"
                            if (log_detali)
                            {
                                string mess = String.Format("(Подсистема учета и контроля ж.д. транспорта на АМКР). Операция создания новой строки Cars (вагон №{0}, id_arrival={1}, id_sostav={2}, dt_set={3}, id текущей операции = {4}) – Отменена, строка была создана ранее id_car={5} и была закрыта."
                                    , delivery.num_car, delivery.id_arrival, id_sostav, dt_set, current_operation.id,car.id);
                                mess.WriteWarning(servece_owner, eventID);
                            }
                            // Обновим старую запись входящей поставку
                            int res = CarsUpdateSave(car.id, id_sostav, dt_set);
                            if (res > 0)
                            {
                                int id_new_delivery = AddCarsInpDeliverySave(car.id, delivery);
                                int res_close_mt = CloseArrivalCars(car.id);
                                //return GetOperation(current_operation.id); // Вернем текушую операцию
                            }
                            return null; // Ошибка нет обновления
                        }
                        else
                        {
                            // Закроем старый CARS
                            parent_id_car = CloseCars(current_operation, id_station_uz, dt_set);
                        }
                    }
                }
                else { 
                    // нет это первый заход
                    //RWReference rw_ref = new RWReference(servece_owner, reference_kis);
                    // Проверим наличие вагона в справочнике если нет создадим + если есть из КИС перенесем аренды и владельца
                    ReferenceCars ref_car = rw_ref.GetReferenceCarsOfNum(delivery.num_car, delivery.id_arrival, dt_set, (int)delivery.id_country, true, true);
                }
                //Создадим новую строку Cars
                int id_new_car = CarsCreateSave(delivery.num_car, id_sostav, delivery.id_arrival, dt_set, parent_id_car);
                if (id_new_car>0){
                    // Поставим вагон в ожидание прибытия с УЗ
                    //RWReference rw_ref = new RWReference(servece_owner, reference_kis);
                    int way_inp = rw_ref.GetIDWayOfStation(id_station_uz, "1");
                    current_operation = OperationSetWayInEnd(id_new_car, way_inp, dt_set, 15, null);  
                    // Входяшие поставки
                    int id_new_delivery = AddCarsInpDeliverySave(id_new_car, delivery);
                    return GetOperation(current_operation.id); // Вернем текушую операцию
                }
                return null; // Ошибка не создан car
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CreateCars(current_operation={0},  id_sostav={1},  id_station_uz={2},  dt_set={3},  delivery={4})",
                    current_operation.GetFieldsAndValue(), id_sostav, id_station_uz, dt_set, delivery.GetFieldsAndValue()), servece_owner, eventID);
                return null; // Ошибка глобальная
            }
        }
        /// <summary>
        /// Закрыть Cars (Вагон стоит на пути приема из АМКР)
        /// </summary>
        /// <param name="current_operation"></param>
        /// <param name="id_station_uz"></param>
        /// <param name="dt_set"></param>
        /// <returns></returns>
        public int CloseCars(CarOperations current_operation, int id_station_uz, DateTime dt_set)
        {
            try
            {
                //RWReference rw_ref = new RWReference(servece_owner, reference_kis);
                // Получим станции УЗ по которым можно получать вагоны на указаную станцию
                //List<Stations> list_station_arrival_uz_to_station = ef_rw.GetStations().Where(s => s.station_uz == true).ToList();

                List<Ways> list_arrival_ways_uz = ef_rw.GetWays().Where(w => w.Stations.station_uz == true & w.num == "1").ToList();
                List<Ways> list_sending_ways_uz = ef_rw.GetWays().Where(w => w.Stations.station_uz == true & w.num == "2").ToList();
                //int way_inp = rw_ref.GetIDWayOfStation(id_station_uz, "1");
                //int way_out = rw_ref.GetIDWayOfStation(id_station_uz, "2");
                //if (!current_operation.IsSetWay(way_out)) {
                // Не стоит на пути отправки из любой станции УЗ
                if (current_operation.IsSetWay(list_sending_ways_uz.Select(w => w.id).ToArray())==0)
                {
                    //if (current_operation.IsSetWay(way_inp))
                    if (current_operation.IsSetWay(list_arrival_ways_uz.Select(w => w.id).ToArray())>0)
                    {
                        // Вагон стоит на пути сделаем маневр
                        // Поставим вагон в конец состава
                        current_operation = PassingOnUZ(current_operation, id_station_uz, dt_set);
                    }
                    else
                    {
                        // Сообщение "Ошибка «Вагон не стоит на путях УЗ»"
                        current_operation = null;
                    }                
                }
                // Закрыть путь
                //int res_close = OperationCloseCorrectPositionSave(current_operation, dt_set);
                int res_close = OperationCloseSave(current_operation, dt_set);
                return res_close > 0 ? CarsCloseSave(current_operation, dt_set) : res_close;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CloseCars(current_operation={0}, id_station_uz={1}, dt_set={2})", current_operation.GetFieldsAndValue(), id_station_uz, dt_set), servece_owner, eventID);
                return -1;
            }
        }
        #endregion




        #endregion

        #region Операции определения вагона

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
                //EFRailWay ef_rw = new EFRailWay();
                //List<CarOperations> list_open_operation = ef_rw.query_GetOpenOperationOfNumCar(num_car).ToList(); // При использовании нет .Cars
                List<CarOperations> list_open_operation = ef_rw.GetOpenOperationOfNumCar(num_car).ToList();
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
                //EFRailWay ef_rw = new EFRailWay();
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

        public CarOperations GetOperation(int id_operation) {
            //EFRailWay ef_rw = new EFRailWay();
            return ef_rw.GetCarOperations(id_operation);
        }

        #endregion

        #region Операции с вагонами на путях
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
                //EFRailWay ef_rw = new EFRailWay();
                // Определим путь
                Ways way = ef_rw.GetWays(id_way);
                if (way == null) return null; // Путь не оределен
                // Закроем последнюю операцию
                //int res_close = OperationCloseCorrectPositionSave(current_operation, dt_set);
                int res_close = OperationCloseSave(current_operation, dt_set);
                if (res_close > 0)
                {
                    int? position = way.CarOperations.IsOpenOperation(Filters.IsOpenAll).Max(o => o.position);
                    // Создать операцию вагон стоит на пути станции и сохранить
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
        /// Поставить вагон на путь в конец состава (первая операция)
        /// </summary>
        /// <param name="id_car"></param>
        /// <param name="id_way"></param>
        /// <param name="dt_set"></param>
        /// <param name="id_status"></param>
        /// <param name="id_conditions"></param>
        /// <returns></returns>
        public CarOperations OperationSetWayInEnd(int id_car, int id_way, DateTime dt_set, int? id_status, int? id_conditions)
        {
            try
            {
                //EFRailWay ef_rw = new EFRailWay();
                // Определим путь
                Ways way = ef_rw.GetWays(id_way);
                if (way == null) return null; // Путь не оределен

                    int? position = way.CarOperations.IsOpenOperation(Filters.IsOpenAll).Max(o => o.position);
                    // Создать операцию вагон стоит на пути станции и сохранить
                    int res_set = OperationSetWaySave(id_car,
                        null,
                        id_way, way.id_station,
                        dt_set,
                        position != null ? 1 : (int)++position,
                        id_status,
                        id_conditions);
                    return res_set > 0 ? ef_rw.GetCarOperations(res_set) : null;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("OperationSetWay(id_car={0}, id_way={1}, dt_set={2}, id_status={3}, id_conditions={4})",
                    id_car, id_way, dt_set, id_status, id_conditions), servece_owner, eventID);
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
                //EFRailWay ef_rw = new EFRailWay();
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
                //EFRailWay ef_rw = new EFRailWay();
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
                //EFRailWay ef_rw = new EFRailWay();
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
        /// Вернуть закрытую операцию и сохранить (на пути закрытия позиция вагонов корректируется)
        /// </summary>
        /// <param name="current_operation"></param>
        /// <param name="dt_close"></param>
        /// <returns></returns>
        public int OperationCloseCorrectPositionSave(CarOperations current_operation, DateTime dt_close)
        {
            try
            {
                int res_close = OperationCloseSave(current_operation, dt_close);
                if (res_close > 0)
                {
                    // Скорректируем на пути нумерацию
                    int res_corr = CorrectPositionStationWaySave((int)current_operation.id_way, 1, 1);
                    return res_corr >= 0 ? res_close : res_corr;
                }
                return res_close;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("OperationCloseCorrectPositionSave(current_operation={0}, dt_close={1})",
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
                //EFRailWay ef_rw = new EFRailWay();
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
                //EFRailWay ef_rw = new EFRailWay();
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

        #region Операции над CARS
        /// <summary>
        /// Вернуть созданный cars
        /// </summary>
        /// <param name="num"></param>
        /// <param name="id_arrival"></param>
        /// <param name="parent_id_car"></param>
        /// <returns></returns>
        public Cars CarsCreate(int num, int id_sostav, int id_arrival, DateTime dt_uz, int? parent_id_car) {
            Cars car = new Cars()
            {
                id = 0,
                id_arrival = id_arrival,
                id_sostav = id_sostav,
                num = num,
                dt_uz = dt_uz,
                dt_inp_amkr = null,
                dt_out_amkr = null,
                natur = null,
                natur_kis = null,
                parent_id = parent_id_car,
            };
            return car;
        }
        /// <summary>
        /// Вернуть и сохранить созданный cars
        /// </summary>
        /// <param name="num"></param>
        /// <param name="id_arrival"></param>
        /// <param name="parent_id_car"></param>
        /// <returns></returns>
        public int CarsCreateSave(int num, int id_sostav, int id_arrival, DateTime dt_uz, int? parent_id_car)
        {
            Cars car = CarsCreate(num, id_sostav, id_arrival, dt_uz, parent_id_car);
            if (car != null) {
                //EFRailWay ef_rw = new EFRailWay();                
                return ef_rw.SaveCars(car);
            }
            return 0;        
        }
        /// <summary>
        /// Вернуть закрытый Cars
        /// </summary>
        /// <param name="current_operation"></param>
        /// <param name="dt_close"></param>
        /// <returns></returns>
        public Cars CarsClose(CarOperations current_operation, DateTime dt_close)
        {
            if (current_operation == null) return null;
            Cars car = current_operation.Cars;
            if (car != null) {
                car.dt_close = dt_close;
                car.user_close = System.Environment.UserDomainName + @"\" + System.Environment.UserName;
            }
            return car;

        }
        /// <summary>
        /// Вернуть закрытый Cars  и сохранить
        /// </summary>
        /// <param name="current_operation"></param>
        /// <param name="dt_close"></param>
        /// <returns></returns>
        public int CarsCloseSave(CarOperations current_operation, DateTime dt_close)
        {

            Cars car = CarsClose(current_operation, dt_close);
            if (car != null) {
                //EFRailWay ef_rw = new EFRailWay();                
                return ef_rw.SaveCars(car);
            }
            return 0;
        }
        /// <summary>
        /// Вернуть обновленный cars
        /// </summary>
        /// <param name="num"></param>
        /// <param name="id_arrival"></param>
        /// <param name="parent_id_car"></param>
        /// <returns></returns>
        public Cars CarsUpdate(int id_car, int id_sostav, DateTime dt_uz) {
            //EFRailWay ef_rw = new EFRailWay();
            Cars car = ef_rw.GetCars(id_car);
            if (car != null)
            { 
                car.id_sostav = id_sostav;
                car.dt_uz = dt_uz;
            }
            return car;
        }
        /// <summary>
        /// Вернуть обновленный cars и сохранить
        /// </summary>
        /// <param name="id_car"></param>
        /// <param name="id_sostav"></param>
        /// <param name="dt_uz"></param>
        /// <returns></returns>
        public int CarsUpdateSave(int id_car, int id_sostav, DateTime dt_uz) {
            //EFRailWay ef_rw = new EFRailWay();
            Cars car = CarsUpdate(id_car, id_sostav, dt_uz);
            if (car != null)
            {
                return ef_rw.SaveCars(car);
            }
            return 0;
        }

        #endregion

        #region CarsInpDelivery Входящие поставки
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
                //EFRailWay ef_rw = new EFRailWay();
                //RWReference rw_ref = new RWReference(servece_owner, reference_kis);
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
        /// <summary>
        /// Вернуть обновленную или новуювходящую поставку
        /// </summary>
        /// <param name="id_car"></param>
        /// <param name="delivery"></param>
        /// <returns></returns>
        public CarsInpDelivery AddCarsInpDelivery(int id_car, CarsInpDelivery delivery)
        {
            //EFRailWay ef_rw = new EFRailWay();
            Cars car = ef_rw.GetCars(id_car);
            CarsInpDelivery new_delivery = null;
            if (car != null)
            {
                // Входяшие поставки
                if (car.CarsInpDelivery != null && car.CarsInpDelivery.Count() > 0)
                {
                    new_delivery = car.CarsInpDelivery.ToList()[0];
                    new_delivery.datetime = delivery.datetime;
                    new_delivery.position = delivery.position;
                }
                else
                {
                    new_delivery = delivery;
                }
                new_delivery.id_car = id_car;
            }
            return new_delivery;
        }
        /// <summary>
        /// Вернуть и сохранить обновленную или новуювходящую поставку
        /// </summary>
        /// <param name="id_car"></param>
        /// <param name="delivery"></param>
        /// <returns></returns>
        public int AddCarsInpDeliverySave(int id_car, CarsInpDelivery delivery)
        {
            //EFRailWay ef_rw = new EFRailWay();
            CarsInpDelivery new_delivery = AddCarsInpDelivery(id_car, delivery);
            if (new_delivery != null)
            {
                return ef_rw.SaveCarsInpDelivery(new_delivery);
            }
            return 0;
        }
        #endregion

        #region Методы для работы с МетТрансом
        /// <summary>
        /// Закрыть прибытие в MT (по вагону пришло ТСП, а вагон уже принят на АМКР)
        /// </summary>
        /// <param name="id_car"></param>
        /// <returns></returns>
        public int CloseArrivalCars(int id_car)
        {
            //EFRailWay ef_rw = new EFRailWay();
            Cars car = ef_rw.GetCars(id_car);
            if (car != null)
            {
                // Закрываем прибытие (по вагону пришло ТСП, а вагон уже принят на АМКР)
                if (car.natur != null | car.natur_kis != null)
                {
                    EFMetallurgTrans ef_mt = new EFMetallurgTrans();
                    int res_close_mt = ef_mt.CloseArrivalCars(car.id_sostav, car.num, car.natur != null ? (int)car.natur : (int)car.natur_kis, (DateTime)car.dt_uz);
                    return res_close_mt;
                }
            }
            return 0;
        }
        #endregion
    }
}
