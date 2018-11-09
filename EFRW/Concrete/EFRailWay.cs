using EFRW.Abstract;
using EFRW.Entities;
using MessageLog;
using libClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFRW.Concrete
{
    public class EFRailWay : IRailWay
    {
        private eventID eventID = eventID.EFRW_EFRailWay;

        protected EFDbContext context = new EFDbContext();

        #region ДВИЖЕНИЕ ВАГОНОВ

        #region CarsInternal

        public IQueryable<CarsInternal> CarsInternal
        {
            get { return context.CarsInternal; }
        }

        public IQueryable<CarsInternal> GetCarsInternal()
        {
            try
            {
                return CarsInternal;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetCarsInternal()"), eventID);
                return null;
            }
        }

        public CarsInternal GetCarsInternal(int id)
        {
            try
            {
                return context.CarsInternal.Where(с => с.id == id).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetCarsInternal(id={0})", id), eventID);
                return null;
            }
        }

        public int SaveCarsInternal(CarsInternal CarsInternal)
        {
            CarsInternal dbEntry;
            try
            {
                if (CarsInternal.id == 0)
                {
                    dbEntry = new CarsInternal()
                    {
                        id = 0,
                        id_sostav = CarsInternal.id_sostav,
                        id_arrival = CarsInternal.id_arrival,
                        num = CarsInternal.num,
                        dt_uz = CarsInternal.dt_uz,
                        dt_inp_amkr = CarsInternal.dt_inp_amkr,
                        dt_out_amkr = CarsInternal.dt_out_amkr,
                        natur_kis_inp = CarsInternal.natur_kis_inp,
                        natur_kis_out = CarsInternal.natur_kis_out,
                        natur_rw = CarsInternal.natur_rw,
                        dt_create = CarsInternal.dt_create != DateTime.Parse("01.01.0001") ? CarsInternal.dt_create : DateTime.Now,
                        user_create = CarsInternal.user_create != null ? CarsInternal.user_create : System.Environment.UserDomainName + @"\" + System.Environment.UserName,
                        //user_close = CarsInternal.user_close,
                        //dt_close = CarsInternal.dt_close,
                        parent_id = CarsInternal.parent_id,
                        CarInboundDelivery = CarsInternal.CarInboundDelivery,
                        CarOutboundDelivery = CarsInternal.CarOutboundDelivery,
                        CarOperations = CarsInternal.CarOperations,
                        Directory_Cars = CarsInternal.Directory_Cars,
                        CarsInternal1 = CarsInternal.CarsInternal1,
                        CarsInternal2 = CarsInternal.CarsInternal2,
                    };

                    context.CarsInternal.Add(dbEntry);
                }
                else
                {
                    dbEntry = context.CarsInternal.Find(CarsInternal.id);
                    if (dbEntry != null)
                    {
                        dbEntry.id_sostav = CarsInternal.id_sostav;
                        dbEntry.id_arrival = CarsInternal.id_arrival;
                        dbEntry.num = CarsInternal.num;
                        dbEntry.dt_uz = CarsInternal.dt_uz;
                        dbEntry.dt_inp_amkr = CarsInternal.dt_inp_amkr;
                        dbEntry.dt_out_amkr = CarsInternal.dt_out_amkr;
                        dbEntry.natur_kis_inp = CarsInternal.natur_kis_inp;
                        dbEntry.natur_kis_out = CarsInternal.natur_kis_out;
                        dbEntry.natur_rw = CarsInternal.natur_rw;
                        dbEntry.user_close = CarsInternal.user_close;
                        dbEntry.dt_close = CarsInternal.dt_close;
                        dbEntry.parent_id = CarsInternal.parent_id;
                        dbEntry.CarInboundDelivery = CarsInternal.CarInboundDelivery;
                        dbEntry.CarOutboundDelivery = CarsInternal.CarOutboundDelivery;
                        dbEntry.CarOperations = CarsInternal.CarOperations;
                        dbEntry.Directory_Cars = CarsInternal.Directory_Cars;
                        dbEntry.CarsInternal1 = CarsInternal.CarsInternal1;
                        dbEntry.CarsInternal2 = CarsInternal.CarsInternal2;  
                    }
                }
                context.SaveChanges();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveCarsInternal(CarsInternal={0})", CarsInternal.GetFieldsAndValue()), eventID);
                return -1;
            }
            return dbEntry.id;
        }

        public CarsInternal DeleteCarsInternal(int id)
        {

            CarsInternal dbEntry = context.CarsInternal.Find(id);
            if (dbEntry != null)
            {
                try
                {
                    context.CarsInternal.Remove(dbEntry);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    e.WriteErrorMethod(String.Format("DeleteCarsInternal(id={0})", id), eventID);
                    return null;
                }
            }
            return dbEntry;
        }
        
        #endregion
        #endregion

        #region СПРАВОЧНИКИ СИСТЕМЫ RAILWAY
        #region Directory_Cars СПРАВОЧНИК ВАГОНОВ

        public IQueryable<Directory_Cars> Directory_Cars
        {
            get { return context.Directory_Cars; }
        }

        public IQueryable<Directory_Cars> GetDirectory_Cars()
        {
            try
            {
                return Directory_Cars;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetDirectory_Cars()"), eventID);
                return null;
            }
        }

        public Directory_Cars GetDirectory_Cars(int num)
        {
            try
            {
                return context.Directory_Cars.Where(с => с.num == num).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetDirectory_Cars(num={0})", num), eventID);
                return null;
            }
        }

        public int SaveDirectory_Cars(Directory_Cars Directory_Cars)
        {
            try
            {
                Directory_Cars dbEntry = context.Directory_Cars.Find(Directory_Cars.num);
                if (dbEntry == null)
                {
                    dbEntry = new Directory_Cars()
                    {
                        num = Directory_Cars.num,
                        id_type = Directory_Cars.id_type,
                        sap = Directory_Cars.sap,
                        note = Directory_Cars.note,
                        lifting_capacity = Directory_Cars.lifting_capacity,
                        tare = Directory_Cars.tare,
                        id_country = Directory_Cars.id_country,
                        count_axles = Directory_Cars.count_axles,
                        is_output_uz = Directory_Cars.is_output_uz,
                        user_create = Directory_Cars.user_create != null ? Directory_Cars.user_create : System.Environment.UserDomainName + @"\" + System.Environment.UserName,
                        dt_create = Directory_Cars.dt_create != DateTime.Parse("01.01.0001") ? Directory_Cars.dt_create : DateTime.Now,
                        //user_edit = Directory_Cars.user_edit,
                        //dt_edit = Directory_Cars.dt_edit,
                        Directory_TypeCars = Directory_Cars.Directory_TypeCars,
                        Directory_OwnerCars = Directory_Cars.Directory_OwnerCars,
                        CarsInternal = Directory_Cars.CarsInternal,
                    };
                    context.Directory_Cars.Add(dbEntry);
                }
                else
                {
                    if (dbEntry != null)
                    {
                        dbEntry.id_type = Directory_Cars.id_type;
                        dbEntry.sap = Directory_Cars.sap;
                        dbEntry.note = Directory_Cars.note;
                        dbEntry.lifting_capacity = Directory_Cars.lifting_capacity;
                        dbEntry.tare = Directory_Cars.tare;
                        dbEntry.id_country = Directory_Cars.id_country;
                        dbEntry.count_axles = Directory_Cars.count_axles;
                        dbEntry.is_output_uz = Directory_Cars.is_output_uz;
                        //dbEntry.user_create = Directory_Cars.user_create;
                        //dbEntry.dt_create = Directory_Cars.dt_create;
                        dbEntry.user_edit = Directory_Cars.user_edit != null ? Directory_Cars.user_edit : System.Environment.UserDomainName + @"\" + System.Environment.UserName;
                        dbEntry.dt_edit = Directory_Cars.dt_edit != null ? Directory_Cars.dt_edit : DateTime.Now;
                        dbEntry.Directory_TypeCars = Directory_Cars.Directory_TypeCars;
                        dbEntry.Directory_OwnerCars = Directory_Cars.Directory_OwnerCars;
                        dbEntry.CarsInternal = Directory_Cars.CarsInternal;
                    }
                }
                context.SaveChanges();
                return dbEntry.num;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveDirectory_Cars(Directory_Cars={0})", Directory_Cars.GetFieldsAndValue()), eventID);
                return -1;
            }
        }

        public Directory_Cars DeleteDirectory_Cars(int num)
        {

            Directory_Cars dbEntry = context.Directory_Cars.Find(num);
            if (dbEntry != null)
            {
                try
                {
                    context.Directory_Cars.Remove(dbEntry);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    e.WriteErrorMethod(String.Format("DeleteDirectory_Cars(num={0})", num), eventID);
                    return null;
                }
            }
            return dbEntry;
        }
        #endregion
        #endregion




    }
}
