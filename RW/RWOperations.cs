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
        bool log_detali = true;                            // Признак детального логирования
        private bool reference_kis = true;                  // Использовать справочники КИС
  

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
                // Вернем последнюю операцию
                CarOperations last_operation = rw_cars.GetLastOperation(car.Num);
                // Вагон заходил на АМКР
                if (last_operation == null)
                {
                    // Вагон не заходил на АМКР

                }
                else { 
                
                }

                return 0;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CarArrivesUZ(car={0})", car), servece_owner, eventID);
                return -1;
            }
        }
    }
}
