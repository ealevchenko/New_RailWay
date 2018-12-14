//using EFKIS.Concrete;
using EFMT.Concrete;
using EFMT.Entities;
using EFRW.Entities;
using libClass;
using MessageLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW
{
    public class RWOperations
    {
        private eventID eventID = eventID.RW_RWOperations;
        protected service servece_owner = service.Null;
        bool log_detali = true;                             // Признак детального логирования
        private bool directory_kis = true;                  // Использовать справочники из системы транспорт КИС
        private bool data_kis = true;                       // Использовать информацию из системы транспорт КИС
        

        public RWOperations()
        {
            //rw_ref = new RWReference(servece_owner, reference_kis);
        }

        public RWOperations(service servece_owner)
        {
            this.servece_owner = servece_owner;
            //rw_ref = new RWReference(servece_owner, reference_kis);
        }


        ///// <summary>
        ///// Принять состав на станию УЗ
        ///// </summary>
        ///// <param name="sostav"></param>
        ///// <returns></returns>
        //public int CarsArrivesUZ(ArrivalSostav sostav)
        //{
        //    try
        //    {
        //        RWCars RWCars = new RWCars();
        //        EFMetallurgTrans ef_mt = new EFMetallurgTrans();
        //        int transfer_count = 0;
        //        int transfer = 0;
        //        int transfer_error = 0;
        //        int transfer_skip = 0;
        //        int tsp_count = 0;
        //        int tsp = 0;
        //        int tsp_error = 0;
        //        string message = null;
        //        string list_no_set = null;

        //        if (sostav == null) return 0; // Состава по указаному id нет
        //        ArrivalSostav sostav_old = sostav.ParentID != null ? ef_mt.GetArrivalSostav((int)sostav.ParentID) : null;
        //        // Получить новый и предыдущий список вагонов в составе
        //        List<ArrivalCars> list_new_car = sostav.ArrivalCars != null ? sostav.ArrivalCars.ToList() : new List<ArrivalCars>();
        //        List<ArrivalCars> list_old_car = sostav_old != null && sostav_old.ArrivalCars != null ? sostav_old.ArrivalCars.ToList() : new List<ArrivalCars>();
        //        // Провести анализ спсиков и убрать существующие вагоны
        //        ef_mt.RemoveMatchingArrivalCars(ref list_new_car, ref list_old_car);
        //        // Сделать ТСП по УЗ вагонов которые были оцеплены на станции УЗ
        //        tsp_count = list_old_car != null ? list_old_car.Count() : 0;
        //        foreach (ArrivalCars car_old in list_old_car)
        //        {
        //            int res_tsp = RWCars.CarTSPUZ(car_old, sostav.DateTime);
        //            tsp += res_tsp > 0 ? 1 : 0;
        //            tsp_error += res_tsp < 0 ? 1 : 0;
        //        }
        //        // Перенесем новые
        //        List<ArrivalCars> list_car_transfer = sostav.ArrivalCars != null ? sostav.ArrivalCars.ToList() : new List<ArrivalCars>();
        //        transfer_count = list_car_transfer != null ? list_car_transfer.Count() : 0;
        //        foreach (ArrivalCars car in list_car_transfer)
        //        {
        //            DateTime dt_start = DateTime.Now;
        //            message += car.Num.ToString() + ":";
        //            int res_transfer = RWCars.CarArrivesUZ(car);
        //            transfer += res_transfer > 0 ? 1 : 0;
        //            transfer_skip += res_transfer == 0 ? 1 : 0;
        //            transfer_error += res_transfer < 0 ? 1 : 0;
        //            list_no_set += res_transfer < 0 ? car.Num.ToString() + ";" : null;
        //            message += res_transfer.ToString() + ";";
        //            TimeSpan ts = DateTime.Now - dt_start;
        //            Console.WriteLine(String.Format("Перенос вагона №{0}, время выполнения: {1}:{2}:{3}({4})", car.Num, ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds));
        //        }
        //        string mess = String.Format("(Подсистема учета и контроля магистрального парка на УЗ) Перенос состава (id состава: {0}, id прибытия {1}, индекс: {2}, дата операции: {3}) на путь отправки на АМКР системы RailWay. Определенно для ТСП по УЗ {4} вагона(ов), выполнено ТСП {5}, ошибок ТСП {6}. Определенно для переноса {7} вагона(ов), перенесено {8}, пропущено {9}, ошибок переноса {10}.",
        //            sostav.ID, sostav.IDArrival, sostav.CompositionIndex, sostav.DateTime, tsp_count, tsp, tsp_error, transfer_count, transfer, transfer_skip, transfer_error);
        //            mess.WriteInformation(servece_owner, eventID);
        //        if (transfer_error > 0 || tsp_error > 0) { mess.WriteEvents(message, servece_owner, eventID); }

        //        return transfer + transfer_skip; ;
        //    }
        //    catch (Exception e)
        //    {
        //        e.WriteErrorMethod(String.Format("CarsArrivesUZ(sostav={0})", sostav.GetFieldsAndValue()), servece_owner, eventID);
        //        return -1;
        //    }
        //}

        /// <summary>
        /// Выполнить "ТСП на УЗ"
        /// </summary>
        /// <param name="car"></param>
        /// <param name="dt_set"></param>
        /// <returns></returns>
        public int CarTSPUZ(ArrivalCars car, DateTime dt_set)
        {
            try
            {
                return 0;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CarTSPUZ(car={0}, dt_set={1})", car, dt_set), servece_owner, eventID);
                return -1;
            }
        }
        /// <summary>
        /// Выполнить операцию "Вагон прибывает на УЗ"
        /// </summary>
        /// <param name="car"></param>
        /// <returns></returns>
        public int CarArrivesUZ(ArrivalCars car)
        {
            try
            {
                RWCars rw_cars = new RWCars();
                int result = 0;
                List<CarsInternal> list = rw_cars.GetCarsInternalOfNum(car.Num).ToList();
                // Вагон заходил на АМКР?
                if (list != null && list.Count() == 0)
                {
                    // Вагон Не заходил на АМКР.
                   result = rw_cars.NewCarsInternal(car, null);
                }
                else
                {
                    // Вагон Есть в системе RailWay
                    // Вернуть последнюю запись из списка строк "Внутренего перемещения вагона"
                    CarsInternal car_internal = list.GetLastCarsInternal();
                    // Код прибытия совподает?
                    if (car_internal.id_arrival == car.ArrivalSostav.IDArrival)
                    {
                        // Да, код прибытия совподает. Это новая операция ТСП
                        if (car_internal.id_sostav < car.IDSostav)
                        {
                            // Да. Это новое ТСП обновим информацию
                            result = rw_cars.UpdateCarsInternal(car_internal, car);
                            // Закроем в базе МТ
                            EFMetallurgTrans ef_mt = new EFMetallurgTrans();
                            // Закрываем прибытие (по вагону пришло ТСП, а вагон уже принят на АМКР)
                            if (result > 0 && (car_internal.natur_rw != null || car_internal.natur_kis_inp != null))
                            {
                                int res_close_mt = ef_mt.CloseArrivalCars(car.IDSostav, car.Num, car_internal.natur_rw != null ? (int)car_internal.natur_rw : (int)car_internal.natur_kis_inp, (DateTime)car.DateOperation);
                            }
                        }
                        else { 
                            // Нет. Это ТСП уже устарело
                            result = 0; // Пропустить, ТСП уже устарело
                        }
                    }
                    else { 
                        // Нет, код прибытия Не совподает.
                        int id_arr = car.ArrivalSostav.IDArrival;

                        if (car_internal.id_arrival > car.ArrivalSostav.IDArrival)
                        {
                            // Код прибыти меньше чем код прибытия в системе, это старая запись (возможна обработка через КИС)
                            // TODO: Выполнить код поиска по КИС старой операции входа на и выход из АМКР, вставка операций в систему RAILWAY
                            result = -20; // Это старая запись (возможна обработка через КИС)
                        }
                        else { 
                            // Код прибыти больше чем код прибытия в системе, это новая запись. Ставим в прибытие.
                            CarOperations last_operation = car_internal.GetLastOperation();
                            DateTime? data_operation = last_operation.GetDateOperation();
                            DateTime? data_operation_new = car.DateOperation; // тестовая
                            int id = car.IDSostav;
                            // Дата выполняемой операции больше или равно (сразу ТСП после прибытия) текущей операции?
                            if (data_operation != null && data_operation <= car.DateOperation)
                            {
                                // Да, это новая операция
                                //------------------------------------
                                //TODO: Добавить модуль обработки принятие на АМКР и отправка с АМКР по данным КИС (выборка происходит из данных КИС за период data_operation и data_operation_new)
                                //// если включен режим "Переносить из КИС"
                                //if (this.data_kis)
                                //{
                                //    EFWagons ef_wag = new EFWagons();
                                //    List<Prom_NatHistAndSostav> pnh_list = ef_wag.GetProm_NatHistAndSostav(car.Num).Where(h => h.DT >= data_operation & h.DT <= data_operation_new).ToList();

                                //}
                                 //------------------------------------ 
                              
                                // Вагон стоит на пути прибытия с АМКР?
                                RWDirectory rw_directory = new RWDirectory(servece_owner);
                                if (last_operation.IsSetWayOperation(rw_directory.GetOnWaysExternalStationUZ().ToList()))
                                {
                                    // Да, вагон стоит на пути прибытия с АМКР
                                    // TODO:Выполним операцию принять на УЗ и создадим новое внутреннее перемещение 
                                }
                                else { 
                                    // Нет, вагон не стоит на пути прибытия с АМКР
                                    // Вагон стоит на пути отправки на АМКР?
                                    //List<Directory_Ways> list_w = rw_directory.GetSendingWaysStationUZ().ToList();
                                    if (last_operation.IsSetWayOperation(rw_directory.GetSendingWaysStationUZ().ToList()))
                                    {
                                        // Да, вагон стоит на пути отправки на АМКР
                                        // TODO:Выполним операцию "Транзит по УЗ" и создадим новое внутреннее перемещение
                                        int res = rw_cars.PerformOperation_TransitUZ(last_operation, car.DateOperation);
                                        if (res <= 0) return res * 100; 
                                        result = rw_cars.NewCarsInternal(car, car_internal.id);
                                    }
                                    else
                                    {
                                        // Нет, вагон Не стоит на пути отправки на АМКР
                                        // Вагон стоит на пути маневра УЗ?
                                        if (last_operation.IsSetWayOperation(rw_directory.GetManeuversWaysStationUZ().ToList()))
                                        {
                                            // Да, вагон стоит на пути маневра УЗ

                                            // TODO: Создадим новое внутреннее перемещение
                                            result = rw_cars.NewCarsInternal(car, car_internal.id);
                                        }
                                        else
                                        {
                                            // Нет, вагон Не стоит на пути маневра УЗ
                                            // Вагон стоит на АМКР

                                            // TODO: Выведим Ошибку или обработаем в КИС выход на УЗ и Создадим новое внутреннее перемещение     
                                        }

                                    }



                                }


                            }
                            else
                            {
                                // Дата прибыти меньше чем дата прибытия в системе, это старая запись (возможна обработка через КИС)
                                // TODO: Выполнить код поиска по КИС старой операции входа на и выход из АМКР, вставка операций в систему RAILWAY
                                result = -20; // Это старая запись (возможна обработка через КИС)
                            }
                        
                        }
                    
                    }

                }
                return result;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CarArrivesUZ(car={0})", car), servece_owner, eventID);
                return -1;
            }
        }
    }
}
