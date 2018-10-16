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
                
                return 0;//!!!!!!!!!!!!!!!
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("TSPOnUZ(car={0})",
                    car.GetFieldsAndValue()), servece_owner, eventID);
                return -1;
            }
        }
        #endregion

        #region ПУТЬ ОТПРАВКИ НА АМКР

        #endregion

        #endregion

        #region
        /// <summary>
        /// Вернуть последнюю открытую операцию c коррекцией старых открытых операций (correct = true - закрыть старые открытые операции)
        /// </summary>
        /// <param name="num_car"></param>
        /// <param name="correct"></param>
        /// <returns></returns>
        public CarOperations GetOpenOperation(int num_car, bool correct)
        {

            EFRailWay ef_rw = new EFRailWay();
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
            if (last_station_operation != null && last_sending_operation != null)
            {
                if (last_station_operation.dt_inp_way >= last_sending_operation.send_dt_inp_way)
                {
                    last_open_operation = last_station_operation;
                }
                else
                {
                    last_open_operation = last_sending_operation;
                };
            };
            list_open_operation.Remove(last_open_operation);
            // Закрыть операции, выполнить коррекции
            if (correct)
            {
                // TODO: реализовать код
            }
            return last_open_operation;
        }
        /// <summary>
        /// Вернуть последнюю открытую операцию (автоматически закрыть старые открытые опреации)
        /// </summary>
        /// <param name="num_car"></param>
        /// <returns></returns>
        public CarOperations GetOpenOperation(int num_car) {
            return GetOpenOperation(num_car, true);
        }

        #endregion
    }
}
