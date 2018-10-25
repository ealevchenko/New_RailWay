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

        public RWOperations()
        {

        }

        public RWOperations(service servece_owner)
        {
            this.servece_owner = servece_owner;
        }

        #region МАНЕВРЫ НА ПУТЯХ УЗ

        #region ПУТЬ ПРИЕМКИ ИЗ АМКР
        /// <summary>
        /// Выполнить операцию ТСП на УЗ ( Автоматически при обновлении состава на УЗ Кривого Рога)
        /// </summary>
        /// <param name="car"></param>
        /// <returns></returns>
        public int TSPOnUZ(ArrivalCars car)
        {
            try
            {
                EFReference.Concrete.EFReference ef_reference = new EFReference.Concrete.EFReference();

                EFRailWay ef_rw = new EFRailWay();


                int codeon = int.Parse(car.CompositionIndex.Substring(9, 4));
                EFReference.Entities.Stations corect_station_uz = ef_reference.GetCorrectStationsOfCode(codeon, false);
                int code_station_uz = corect_station_uz != null ? (int)corect_station_uz.code_cs : codeon;
                Stations station_uz = ef_rw.GetStationsOfCodeUZ(code_station_uz);

                CarOperations current_operation = GetCurrentOperation(car.Num);
                // Операция нет или операция закрыта?
                if (current_operation==null || !current_operation.IsOpenAll()) {
                    // Создадим новую строку Car

                }
                // Выполним операцию ТСП на УЗ
                TSPOnUZ(current_operation, station_uz.id);
                return 0;//!!!!!!!!!!!!!!!
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("TSPOnUZ(car={0})",
                    car.GetFieldsAndValue()), servece_owner, eventID);
                return -1;
            }
        }

        public CarOperations TSPOnUZ(CarOperations current_operation, int id_station_uz)
        {
            try
            {


                return null;//!!!!!!!!!!!!!!!
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("TSPOnUZ(current_operation={0}, id_station_uz={1})",
                    current_operation.GetFieldsAndValue(), id_station_uz), servece_owner, eventID);
                return null;
            }
        }

        #endregion

        #region ПУТЬ ОТПРАВКИ НА АМКР

        #endregion

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
    }
}
