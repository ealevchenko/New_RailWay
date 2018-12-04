using EFRW.Concrete;
using EFRW.Concrete.EFCars;
using EFRW.Entities;
using MessageLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW
{
    
    /// <summary>
    /// Класс Вагоны систмы RailWay
    /// </summary>
    public class RWCars
    {
        private eventID eventID = eventID.RW_RWCars;
        private service servece_owner;
        private EFDbContext db;
        private bool log_detali = false;


        public RWCars()
        {
            this.db = new EFDbContext();
            this.servece_owner = service.Null;
        }

        public RWCars(EFDbContext db)
        {
            this.db = db;
            this.servece_owner = service.Null;
        }

        public RWCars(service servece_owner)
        {
            this.servece_owner = servece_owner;
            this.db = new EFDbContext();
        }

        public RWCars(EFDbContext db, service servece_owner)
        {
            this.servece_owner = servece_owner;
            this.db = db;
        }

        #region
        /// <summary>
        /// Вернуть последнюю открытую операцию
        /// </summary>
        /// <param name="num_car"></param>
        /// <returns></returns>
        public CarOperations GetOpenOperation(int num_car)
        {
            try
            {
                EFCarOperations ef_operation = new EFCarOperations(this.db);
                List<CarOperations> list_open_operation = ef_operation.Get().Where(o => o.CarsInternal.num == num_car).IsStatusOperatio(RWHelpers.IsOpen);
                if (list_open_operation == null) return null;
                return list_open_operation.OrderByDescending(o => o.dt_inp).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetOpenOperation(num_car={0})", num_car), servece_owner, eventID);
                return null;
            }
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
                EFCarOperations ef_operation = new EFCarOperations(this.db);
                List<CarOperations> list_open_operation = ef_operation.Get().Where(o => o.CarsInternal.num == num_car).ToList();
                if (list_open_operation == null) return null;
                return list_open_operation.OrderByDescending(o => o.dt_inp).FirstOrDefault();
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
