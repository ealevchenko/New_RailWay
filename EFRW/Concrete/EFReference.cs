using EFRW.Abstract;
using EFRW.Entities;
using MessageLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libClass;
using System.Data.Entity;
using System.Data.SqlClient;

namespace EFRW.Concrete
{
    public class EFReference : IReference
    {
        private eventID eventID = eventID.EFRW_EFReference;

        protected EFDbContext context = new EFDbContext();

        public Database Database
        {
            get { return context.Database; }
        }

        #region CARGO
        #region ReferenceCargo
        public IQueryable<ReferenceCargo> ReferenceCargo
        {
            get { return context.ReferenceCargo; }
        }

        public int SaveReferenceCargo(ReferenceCargo ReferenceCargo)
        {
            ReferenceCargo dbEntry;
            try
            {
                if (ReferenceCargo.id == 0)
                {
                    dbEntry = new ReferenceCargo()
                    {
                        id = 0,
                        name_ru = ReferenceCargo.name_ru,
                        name_en = ReferenceCargo.name_en,
                        name_full_ru = ReferenceCargo.name_full_ru,
                        name_full_en = ReferenceCargo.name_full_en,
                        etsng = ReferenceCargo.etsng,
                        id_type = ReferenceCargo.id_type,
                        //create_dt = ReferenceCargo.create_dt,
                        //create_user = ReferenceCargo.create_user,
                        //change_dt = ReferenceCargo.change_dt,
                        //change_user = ReferenceCargo.change_user,
                        create_dt = ReferenceCargo.create_dt != DateTime.Parse("01.01.0001") ? ReferenceCargo.create_dt : DateTime.Now,
                        create_user = ReferenceCargo.create_user != null ? ReferenceCargo.create_user : System.Environment.UserDomainName + @"\" + System.Environment.UserName,
                        change_dt = null,
                        change_user = null,
                        CarsInpDelivery = ReferenceCargo.CarsInpDelivery,
                        CarsOutDelivery = ReferenceCargo.CarsOutDelivery,
                        ReferenceTypeCargo = ReferenceCargo.ReferenceTypeCargo
                    };
                    context.ReferenceCargo.Add(dbEntry);
                }
                else
                {
                    dbEntry = context.ReferenceCargo.Find(ReferenceCargo.id);
                    // Сделано для отмены изменения даты создания строки
                    EFDbContext context_real = new EFDbContext();
                    ReferenceCargo old_ReferenceCargo = context_real.ReferenceCargo.Where(c => c.id == ReferenceCargo.id).FirstOrDefault();
                    if (dbEntry != null)
                    {
                        dbEntry.name_ru = ReferenceCargo.name_ru;
                        dbEntry.name_en = ReferenceCargo.name_en;
                        dbEntry.name_full_ru = ReferenceCargo.name_full_ru;
                        dbEntry.name_full_en = ReferenceCargo.name_full_en;
                        dbEntry.etsng = ReferenceCargo.etsng;
                        dbEntry.id_type = ReferenceCargo.id_type;
                        dbEntry.create_dt = old_ReferenceCargo.create_dt;
                        dbEntry.create_user = old_ReferenceCargo.create_user;
                        dbEntry.change_dt = ReferenceCargo.change_dt != null ? ReferenceCargo.change_dt : DateTime.Now;
                        dbEntry.change_user = ReferenceCargo.change_user != null ? ReferenceCargo.change_user : System.Environment.UserDomainName + @"\" + System.Environment.UserName;
                        dbEntry.CarsInpDelivery = ReferenceCargo.CarsInpDelivery;
                        dbEntry.CarsOutDelivery = ReferenceCargo.CarsOutDelivery;
                        dbEntry.ReferenceTypeCargo = ReferenceCargo.ReferenceTypeCargo;
                    }
                }
                context.SaveChanges();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveReferenceCargo(ReferenceCargo={0})", ReferenceCargo.GetFieldsAndValue()), eventID);
                return -1;
            }
            return dbEntry.id;
        }

        public ReferenceCargo DeleteReferenceCargo(int id)
        {
            ReferenceCargo dbEntry = context.ReferenceCargo.Find(id);
            if (dbEntry != null)
            {
                try
                {
                    context.ReferenceCargo.Remove(dbEntry);

                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    e.WriteErrorMethod(String.Format("DeleteReferenceCargo(id={0})", id), eventID);
                    return null;
                }
            }
            return dbEntry;
        }
        /// <summary>
        /// Получить все строки справочника грузов
        /// </summary>
        /// <returns></returns>
        public IQueryable<ReferenceCargo> GetReferenceCargo()
        {
            try
            {
                return ReferenceCargo;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetReferenceCargo()"), eventID);
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_cargo"></param>
        /// <returns></returns>
        public ReferenceCargo GetReferenceCargo(int id)
        {
            try
            {
                return GetReferenceCargo().Where(c => c.id == id).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetReferenceCargo()"), eventID);
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code_etsng"></param>
        /// <returns></returns>
        public ReferenceCargo GetReferenceCargoOfCodeETSNG(int code_etsng)
        {
            return GetReferenceCargo().Where(c => c.etsng == code_etsng).FirstOrDefault();
        }
        /// <summary>
        /// Вернуть список грузов по указаному деапазону кодов ETSNG
        /// </summary>
        /// <param name="code_start"></param>
        /// <param name="code_stop"></param>
        /// <returns></returns>
        public IQueryable<ReferenceCargo> GetReferenceCargoOfCodeETSNG(int code_etsng_start, int code_etsng_stop)
        {
            return GetReferenceCargo().Where(c => c.etsng >= code_etsng_start & c.etsng <= code_etsng_stop);
        }
        /// <summary>
        /// Вернуть уточненую строку ReferenceCargo
        /// </summary>
        /// <param name="code_etsng"></param>
        /// <returns></returns>
        public ReferenceCargo GetCorectReferenceCargo(int code_etsng)
        {
            ReferenceCargo ref_cargo = GetReferenceCargoOfCodeETSNG(code_etsng);
            if (ref_cargo == null)
            {
                ref_cargo = GetReferenceCargoOfCodeETSNG(code_etsng * 10, (code_etsng * 10) + 9).FirstOrDefault();
            }
            return ref_cargo;
        }
        /// <summary>
        /// Вернуть откорректированный код ETSNG
        /// </summary>
        /// <param name="code_etsng"></param>
        /// <returns></returns>
        public int GetCodeCorectReferenceCargo(int code_etsng)
        {
            ReferenceCargo ref_cargo = GetCorectReferenceCargo(code_etsng);
            return ref_cargo != null ? ref_cargo.etsng : code_etsng;
        }
        #endregion

        #region ReferenceTypeCargo
        public IQueryable<ReferenceTypeCargo> ReferenceTypeCargo
        {
            get { return context.ReferenceTypeCargo; }
        }

        public int SaveReferenceTypeCargo(ReferenceTypeCargo ReferenceTypeCargo)
        {
            ReferenceTypeCargo dbEntry;
            try
            {
                if (ReferenceTypeCargo.id == 0)
                {
                    dbEntry = new ReferenceTypeCargo()
                    {
                        id = 0,
                        id_group = ReferenceTypeCargo.id_group,
                        type_name_ru = ReferenceTypeCargo.type_name_ru,
                        type_name_en = ReferenceTypeCargo.type_name_en,
                        ReferenceCargo = ReferenceTypeCargo.ReferenceCargo,
                        ReferenceGroupCargo = ReferenceTypeCargo.ReferenceGroupCargo
                    };
                    context.ReferenceTypeCargo.Add(dbEntry);
                }
                else
                {
                    dbEntry = context.ReferenceTypeCargo.Find(ReferenceTypeCargo.id);
                    if (dbEntry != null)
                    {
                        dbEntry.id_group = ReferenceTypeCargo.id_group;
                        dbEntry.type_name_ru = ReferenceTypeCargo.type_name_ru;
                        dbEntry.type_name_en = ReferenceTypeCargo.type_name_en;
                        dbEntry.ReferenceCargo = ReferenceTypeCargo.ReferenceCargo;
                        dbEntry.ReferenceGroupCargo = ReferenceTypeCargo.ReferenceGroupCargo;
                    }
                }
                context.SaveChanges();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveReferenceTypeCargo(ReferenceTypeCargo={0})", ReferenceTypeCargo.GetFieldsAndValue()), eventID);
                return -1;
            }
            return dbEntry.id;
        }

        public ReferenceTypeCargo DeleteReferenceTypeCargo(int id)
        {
            ReferenceTypeCargo dbEntry = context.ReferenceTypeCargo.Find(id);
            if (dbEntry != null)
            {
                try
                {
                    context.ReferenceTypeCargo.Remove(dbEntry);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    e.WriteErrorMethod(String.Format("DeleteReferenceTypeCargo(id={0})", id), eventID);
                    return null;
                }
            }
            return dbEntry;
        }
        #endregion

        #region ReferenceGroupCargo

        public IQueryable<ReferenceGroupCargo> ReferenceGroupCargo
        {
            get { return context.ReferenceGroupCargo; }
        }

        public int SaveReferenceGroupCargo(ReferenceGroupCargo ReferenceGroupCargo)
        {
            ReferenceGroupCargo dbEntry;
            try
            {
                if (ReferenceGroupCargo.id == 0)
                {
                    dbEntry = new ReferenceGroupCargo()
                    {
                        id = 0,
                        ReferenceTypeCargo = ReferenceGroupCargo.ReferenceTypeCargo,
                        group_name_ru = ReferenceGroupCargo.group_name_ru,
                        group_name_en = ReferenceGroupCargo.group_name_en
                    };
                    context.ReferenceGroupCargo.Add(dbEntry);
                }
                else
                {
                    dbEntry = context.ReferenceGroupCargo.Find(ReferenceGroupCargo.id);
                    if (dbEntry != null)
                    {
                        dbEntry.ReferenceTypeCargo = ReferenceGroupCargo.ReferenceTypeCargo;
                        dbEntry.group_name_ru = ReferenceGroupCargo.group_name_ru;
                        dbEntry.group_name_en = ReferenceGroupCargo.group_name_en;
                    }
                }
                context.SaveChanges();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveReferenceGroupCargo(ReferenceGroupCargo={0})", ReferenceGroupCargo.GetFieldsAndValue()), eventID);
                return -1;
            }
            return dbEntry.id;
        }

        public ReferenceGroupCargo DeleteReferenceGroupCargo(int id)
        {
            ReferenceGroupCargo dbEntry = context.ReferenceGroupCargo.Find(id);
            if (dbEntry != null)
            {
                try
                {
                    context.ReferenceGroupCargo.Remove(dbEntry);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    e.WriteErrorMethod(String.Format("DeleteReferenceGroupCargo(id={0})", id), eventID);
                    return null;
                }
            }
            return dbEntry;
        }
        #endregion
        #endregion

        #region CARS
        #region ReferenceCars
        public IQueryable<ReferenceCars> ReferenceCars
        {
            get { return context.ReferenceCars; }
        }

        public IQueryable<ReferenceCars> GetReferenceCars()
        {

            try
            {
                return ReferenceCars;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetReferenceCars()"), eventID);
                return null;
            }
        }

        public ReferenceCars GetReferenceCars(int num)
        {
            try
            {
                return GetReferenceCars().Where(c => c.num == num).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetReferenceCars(num={0})", num), eventID);
                return null;
            }
        }

        public int SaveReferenceCars(ReferenceCars ReferenceCars)
        {
            try
            {
                EntityState resu = context.Entry(ReferenceCars).State;
                ReferenceCars dbEntry;
                if (context.ReferenceCars.Find(ReferenceCars.num) == null)
                {
                    dbEntry = new ReferenceCars()
                    {
                        num = ReferenceCars.num,
                        id_type = ReferenceCars.id_type,
                        lifting_capacity = ReferenceCars.lifting_capacity,
                        tare = ReferenceCars.tare,
                        id_country = ReferenceCars.id_country,
                        count_axles = ReferenceCars.count_axles,
                        is_output_uz = ReferenceCars.is_output_uz,
                        create_dt = ReferenceCars.create_dt != DateTime.Parse("01.01.0001") ? ReferenceCars.create_dt : DateTime.Now,
                        create_user = ReferenceCars.create_user != null ? ReferenceCars.create_user : System.Environment.UserDomainName + @"\" + System.Environment.UserName,
                        change_dt = null,
                        change_user = null,
                        Cars = ReferenceCars.Cars,
                        ReferenceCountry = ReferenceCars.ReferenceCountry,
                        ReferenceOwnerCars = ReferenceCars.ReferenceOwnerCars,
                        ReferenceTypeCars = ReferenceCars.ReferenceTypeCars
                    };
                    context.ReferenceCars.Add(dbEntry);
                }
                else
                {
                    dbEntry = context.ReferenceCars.Find(ReferenceCars.num);
                    // Сделано для отмены изменения даты создания строки
                    EFDbContext context_real = new EFDbContext();
                    ReferenceCars old_ReferenceCars = context_real.ReferenceCars.Where(c => c.num == ReferenceCars.num).FirstOrDefault();
                    if (dbEntry != null)
                    {
                        dbEntry.id_type = ReferenceCars.id_type;
                        dbEntry.lifting_capacity = ReferenceCars.lifting_capacity;
                        dbEntry.tare = ReferenceCars.tare;
                        dbEntry.id_country = ReferenceCars.id_country;
                        dbEntry.count_axles = ReferenceCars.count_axles;
                        dbEntry.is_output_uz = ReferenceCars.is_output_uz;
                        dbEntry.create_dt = old_ReferenceCars.create_dt;
                        dbEntry.create_user = old_ReferenceCars.create_user;
                        dbEntry.change_dt = ReferenceCars.change_dt != null ? ReferenceCars.change_dt : DateTime.Now;
                        dbEntry.change_user = ReferenceCars.change_user != null ? ReferenceCars.change_user : System.Environment.UserDomainName + @"\" + System.Environment.UserName;
                        dbEntry.Cars = ReferenceCars.Cars;
                        dbEntry.ReferenceCountry = ReferenceCars.ReferenceCountry;
                        dbEntry.ReferenceOwnerCars = ReferenceCars.ReferenceOwnerCars;
                        dbEntry.ReferenceTypeCars = ReferenceCars.ReferenceTypeCars;
                    }
                }
                context.SaveChanges();
                return dbEntry.num;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveReferenceCars(ReferenceCars={0})", ReferenceCars.GetFieldsAndValue()), eventID);
                return -1;
            }
        }

        public ReferenceCars DeleteReferenceCars(int num)
        {
            // Удалим строки справосника "Аренда"
            int res = DeleteReferenceOwnerCarsOfNum(num);
            if (res < 0) return null;
            ReferenceCars dbEntry = context.ReferenceCars.Find(num);
            if (dbEntry != null)
            {
                try
                {
                    context.ReferenceCars.Remove(dbEntry);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    e.WriteErrorMethod(String.Format("DeleteReferenceCars(num={0})", num), eventID);
                    return null;
                }
            }
            return dbEntry;
        }
        #endregion

        #region ReferenceTypeCars
        public IQueryable<ReferenceTypeCars> ReferenceTypeCars
        {
            get { return context.ReferenceTypeCars; }
        }

        public IQueryable<ReferenceTypeCars> GetReferenceTypeCars()
        {
            try
            {
                return ReferenceTypeCars;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetReferenceTypeCars()"), eventID);
                return null;
            }
        }

        public ReferenceTypeCars GetReferenceTypeCars(int id)
        {
            try
            {
                return GetReferenceTypeCars().Where(t => t.id == id).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetReferenceTypeCars(id={0})", id), eventID);
                return null;
            }
        }

        public int SaveReferenceTypeCars(ReferenceTypeCars ReferenceTypeCars)
        {
            ReferenceTypeCars dbEntry;
            try
            {
                if (ReferenceTypeCars.id == 0)
                {
                    dbEntry = new ReferenceTypeCars()
                    {
                        id = 0,
                        id_group = ReferenceTypeCars.id_group,
                        type_cars_ru = ReferenceTypeCars.type_cars_ru,
                        type_cars_en = ReferenceTypeCars.type_cars_en,
                        type_cars_abr_ru = ReferenceTypeCars.type_cars_abr_ru,
                        type_cars_abr_en = ReferenceTypeCars.type_cars_abr_en,
                        ReferenceCars = ReferenceTypeCars.ReferenceCars,
                        ReferenceGroupCars = ReferenceTypeCars.ReferenceGroupCars
                    };
                    context.ReferenceTypeCars.Add(dbEntry);
                }
                else
                {
                    dbEntry = context.ReferenceTypeCars.Find(ReferenceTypeCars.id);
                    if (dbEntry != null)
                    {
                        dbEntry.id_group = ReferenceTypeCars.id_group;
                        dbEntry.type_cars_ru = ReferenceTypeCars.type_cars_ru;
                        dbEntry.type_cars_en = ReferenceTypeCars.type_cars_en;
                        dbEntry.type_cars_abr_ru = ReferenceTypeCars.type_cars_abr_ru;
                        dbEntry.type_cars_abr_en = ReferenceTypeCars.type_cars_abr_en;
                        dbEntry.ReferenceCars = ReferenceTypeCars.ReferenceCars;
                        dbEntry.ReferenceGroupCars = ReferenceTypeCars.ReferenceGroupCars;
                    }
                }
                context.SaveChanges();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveReferenceTypeCars(ReferenceTypeCars={0})", ReferenceTypeCars.GetFieldsAndValue()), eventID);
                return -1;
            }
            return dbEntry.id;
        }

        public ReferenceTypeCars DeleteReferenceTypeCars(int id)
        {
            ReferenceTypeCars dbEntry = context.ReferenceTypeCars.Find(id);
            if (dbEntry != null)
            {
                try
                {
                    context.ReferenceTypeCars.Remove(dbEntry);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    e.WriteErrorMethod(String.Format("DeleteReferenceTypeCars(id={0})", id), eventID);
                    return null;
                }
            }
            return dbEntry;
        }
        /// <summary>
        /// Вернуть строку справочника по абрииатуре
        /// </summary>
        /// <param name="abr"></param>
        /// <returns></returns>
        public ReferenceTypeCars GetReferenceTypeCarsOfAbr(string abr)
        {
            try
            {
                return GetReferenceTypeCars().Where(t => t.type_cars_abr_ru.Trim().ToLower() == abr.Trim().ToLower()).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetReferenceTypeCarsOfAbr(abr={0})", abr), eventID);
                return null;
            }
        }
        #endregion

        #region ReferenceGroupCars

        public IQueryable<ReferenceGroupCars> ReferenceGroupCars
        {
            get { return context.ReferenceGroupCars; }
        }

        public IQueryable<ReferenceGroupCars> GetReferenceGroupCars()
        {
            try
            {
                return ReferenceGroupCars;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetReferenceGroupCars()"), eventID);
                return null;
            }
        }

        public ReferenceGroupCars GetReferenceGroupCars(int id)
        {
            try
            {
                return GetReferenceGroupCars().Where(g => g.id == id).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetReferenceGroupCars(id={0})", id), eventID);
                return null;
            };
        }

        public int SaveReferenceGroupCars(ReferenceGroupCars ReferenceGroupCars)
        {
            ReferenceGroupCars dbEntry;
            try
            {
                if (ReferenceGroupCars.id == 0)
                {
                    dbEntry = new ReferenceGroupCars()
                    {
                        id = 0,
                        group_cars_ru = ReferenceGroupCars.group_cars_ru,
                        group_cars_en = ReferenceGroupCars.group_cars_en,
                        ReferenceTypeCars = ReferenceGroupCars.ReferenceTypeCars
                    };
                    context.ReferenceGroupCars.Add(dbEntry);
                }
                else
                {
                    dbEntry = context.ReferenceGroupCars.Find(ReferenceGroupCars.id);
                    if (dbEntry != null)
                    {
                        dbEntry.group_cars_ru = ReferenceGroupCars.group_cars_ru;
                        dbEntry.group_cars_en = ReferenceGroupCars.group_cars_en;
                        dbEntry.ReferenceTypeCars = ReferenceGroupCars.ReferenceTypeCars;
                    }
                }
                context.SaveChanges();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveReferenceGroupCars(ReferenceGroupCars={0})", ReferenceGroupCars.GetFieldsAndValue()), eventID);
                return -1;
            }
            return dbEntry.id;
        }

        public ReferenceGroupCars DeleteReferenceGroupCars(int id)
        {
            ReferenceGroupCars dbEntry = context.ReferenceGroupCars.Find(id);
            if (dbEntry != null)
            {
                try
                {
                    context.ReferenceGroupCars.Remove(dbEntry);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    e.WriteErrorMethod(String.Format("DeleteReferenceGroupCars(id={0})", id), eventID);
                    return null;
                }
            }
            return dbEntry;
        }
        #endregion
        #endregion

        #region ReferenceCountry

        public IQueryable<ReferenceCountry> ReferenceCountry
        {
            get { return context.ReferenceCountry; }
        }

        public IQueryable<ReferenceCountry> GetReferenceCountry()
        {
            try
            {
                return ReferenceCountry;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetReferenceCountry()"), eventID);
                return null;
            }
        }

        public ReferenceCountry GetReferenceCountry(int id)
        {
            try
            {
                return GetReferenceCountry().Where(c => c.id == id).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetReferenceCountry(id={0})", id), eventID);
                return null;
            };
        }

        public int SaveReferenceCountry(ReferenceCountry ReferenceCountry)
        {
            ReferenceCountry dbEntry;
            try
            {
                if (ReferenceCountry.id == 0)
                {
                    dbEntry = new ReferenceCountry()
                    {
                        id = 0,
                        country_ru = ReferenceCountry.country_ru,
                        country_en = ReferenceCountry.country_en,
                        code = ReferenceCountry.code,
                        ReferenceCars = ReferenceCountry.ReferenceCars,
                        CarsOutDelivery = ReferenceCountry.CarsOutDelivery,
                    };
                    context.ReferenceCountry.Add(dbEntry);
                }
                else
                {
                    dbEntry = context.ReferenceCountry.Find(ReferenceCountry.id);
                    if (dbEntry != null)
                    {
                        dbEntry.country_ru = ReferenceCountry.country_ru;
                        dbEntry.country_en = ReferenceCountry.country_en;
                        dbEntry.code = ReferenceCountry.code;
                        dbEntry.ReferenceCars = ReferenceCountry.ReferenceCars;
                        dbEntry.CarsOutDelivery = ReferenceCountry.CarsOutDelivery;
                    }
                }
                context.SaveChanges();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveReferenceCountry(ReferenceCountry={0})", ReferenceCountry.GetFieldsAndValue()), eventID);
                return -1;
            }
            return dbEntry.id;
        }

        public ReferenceCountry DeleteReferenceCountry(int id)
        {
            ReferenceCountry dbEntry = context.ReferenceCountry.Find(id);
            if (dbEntry != null)
            {
                try
                {
                    context.ReferenceCountry.Remove(dbEntry);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    e.WriteErrorMethod(String.Format("DeleteReferenceCountry(id={0})", id), eventID);
                    return null;
                }
            }
            return dbEntry;
        }
        /// <summary>
        /// Получить запись справочника по коду страны ISO3166
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public ReferenceCountry GetReferenceCountryOfCode(int code)
        {
            try
            {
                return GetReferenceCountry().Where(r => r.code == code).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetReferenceCountryOfCode(code={0})", code), eventID);
                return null;
            }
        }

        #endregion

        #region ReferenceStation

        public IQueryable<ReferenceStation> ReferenceStation
        {
            get { return context.ReferenceStation; }
        }

        public IQueryable<ReferenceStation> GetReferenceStation()
        {
            try
            {
                return ReferenceStation;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetReferenceStation()"), eventID);
                return null;
            }
        }

        public ReferenceStation GetReferenceStation(int id)
        {
            try
            {
                return GetReferenceStation().Where(s => s.id == id).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetReferenceStation(id={0})", id), eventID);
                return null;
            };
        }

        public int SaveReferenceStation(ReferenceStation ReferenceStation)
        {
            ReferenceStation dbEntry;
            try
            {
                if (ReferenceStation.id == 0)
                {
                    dbEntry = new ReferenceStation()
                    {
                        id = 0,
                        name = ReferenceStation.name,
                        station = ReferenceStation.station,
                        internal_railroad = ReferenceStation.internal_railroad,
                        ir_abbr = ReferenceStation.ir_abbr,
                        name_network = ReferenceStation.name_network,
                        nn_abbr = ReferenceStation.nn_abbr,
                        code_cs = ReferenceStation.code_cs,
                        CarsOutDelivery = ReferenceStation.CarsOutDelivery
                    };
                    context.ReferenceStation.Add(dbEntry);
                }
                else
                {
                    dbEntry = context.ReferenceStation.Find(ReferenceStation.id);
                    if (dbEntry != null)
                    {
                        dbEntry.name = ReferenceStation.name;
                        dbEntry.station = ReferenceStation.station;
                        dbEntry.internal_railroad = ReferenceStation.internal_railroad;
                        dbEntry.ir_abbr = ReferenceStation.ir_abbr;
                        dbEntry.name_network = ReferenceStation.name_network;
                        dbEntry.nn_abbr = ReferenceStation.nn_abbr;
                        dbEntry.code_cs = ReferenceStation.code_cs;
                        dbEntry.CarsOutDelivery = ReferenceStation.CarsOutDelivery;
                    }
                }
                context.SaveChanges();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveReferenceStation(ReferenceStation={0})", ReferenceStation.GetFieldsAndValue()), eventID);
                return -1;
            }
            return dbEntry.id;
        }

        public ReferenceStation DeleteReferenceStation(int id)
        {
            ReferenceStation dbEntry = context.ReferenceStation.Find(id);
            if (dbEntry != null)
            {
                try
                {
                    context.ReferenceStation.Remove(dbEntry);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    e.WriteErrorMethod(String.Format("DeleteReferenceStation(id={0})", id), eventID);
                    return null;
                }
            }
            return dbEntry;
        }

        public ReferenceStation GetReferenceStationOfCodecs(int codecs)
        {
            try
            {
                return GetReferenceStation().Where(s => s.code_cs == codecs).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetReferenceStationOfCodecs(codecs={0})", codecs), eventID);
                return null;
            };
        }

        #endregion

        #region OWNERS
        #region ReferenceOwners
        public IQueryable<ReferenceOwners> ReferenceOwners
        {
            get { return context.ReferenceOwners; }
        }

        public IQueryable<ReferenceOwners> GetReferenceOwners()
        {
            try
            {
                return ReferenceOwners;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetReferenceOwners()"), eventID);
                return null;
            }
        }

        public ReferenceOwners GetReferenceOwners(int id)
        {
            try
            {
                return GetReferenceOwners().Where(o => o.id == id).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetReferenceOwners(id={0})", id), eventID);
                return null;
            };
        }

        public int SaveReferenceOwners(ReferenceOwners ReferenceOwners)
        {
            ReferenceOwners dbEntry;
            try
            {
                if (ReferenceOwners.id == 0)
                {
                    dbEntry = new ReferenceOwners()
                    {
                        id = 0,
                        owner_name = ReferenceOwners.owner_name,
                        owner_abr = ReferenceOwners.owner_abr,
                        id_kis = ReferenceOwners.id_kis,
                        ReferenceOwnerCars = ReferenceOwners.ReferenceOwnerCars

                    };
                    context.ReferenceOwners.Add(dbEntry);
                }
                else
                {
                    dbEntry = context.ReferenceOwners.Find(ReferenceOwners.id);
                    if (dbEntry != null)
                    {
                        dbEntry.owner_name = ReferenceOwners.owner_name;
                        dbEntry.owner_abr = ReferenceOwners.owner_abr;
                        dbEntry.id_kis = ReferenceOwners.id_kis;
                        dbEntry.ReferenceOwnerCars = ReferenceOwners.ReferenceOwnerCars;
                    }
                }
                context.SaveChanges();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveReferenceOwners(ReferenceOwners={0})", ReferenceOwners.GetFieldsAndValue()), eventID);
                return -1;
            }
            return dbEntry.id;
        }

        public ReferenceOwners DeleteReferenceOwners(int id)
        {
            ReferenceOwners dbEntry = context.ReferenceOwners.Find(id);
            if (dbEntry != null)
            {
                try
                {
                    context.ReferenceOwners.Remove(dbEntry);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    e.WriteErrorMethod(String.Format("DeleteReferenceOwners(id={0})", id), eventID);
                    return null;
                }
            }
            return dbEntry;
        }

        public ReferenceOwners GetReferenceOwnersOfKIS(int id_kis)
        {
            try
            {
                return GetReferenceOwners().Where(o => o.id_kis == id_kis).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetReferenceOwnersOfKIS(id_kis={0})", id_kis), eventID);
                return null;
            };
        }

        #endregion

        #region ReferenceOwnerCars
        public IQueryable<ReferenceOwnerCars> ReferenceOwnerCars
        {
            get { return context.ReferenceOwnerCars; }
        }

        public IQueryable<ReferenceOwnerCars> GetReferenceOwnerCars()
        {
            try
            {
                return ReferenceOwnerCars;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetReferenceOwnerCars()"), eventID);
                return null;
            }
        }

        public ReferenceOwnerCars GetReferenceOwnerCars(int id)
        {
            try
            {
                return GetReferenceOwnerCars().Where(o => o.id == id).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetReferenceOwnerCars(id={0})", id), eventID);
                return null;
            };
        }

        public int SaveReferenceOwnerCars(ReferenceOwnerCars ReferenceOwnerCars)
        {
            ReferenceOwnerCars dbEntry;
            try
            {
                if (ReferenceOwnerCars.id == 0)
                {
                    dbEntry = new ReferenceOwnerCars()
                    {
                        id = 0,
                        num = ReferenceOwnerCars.num,
                        id_owner = ReferenceOwnerCars.id_owner,
                        start_lease = ReferenceOwnerCars.start_lease,
                        end_lease = ReferenceOwnerCars.end_lease,
                        id_arrival = ReferenceOwnerCars.id_arrival,
                        create_dt = ReferenceOwnerCars.create_dt,
                        create_user = ReferenceOwnerCars.create_user,
                        change_dt = ReferenceOwnerCars.change_dt,
                        change_user = ReferenceOwnerCars.change_user,
                    };
                    context.ReferenceOwnerCars.Add(dbEntry);
                }
                else
                {
                    dbEntry = context.ReferenceOwnerCars.Find(ReferenceOwnerCars.id);
                    if (dbEntry != null)
                    {
                        dbEntry.num = ReferenceOwnerCars.num;
                        dbEntry.id_owner = ReferenceOwnerCars.id_owner;
                        dbEntry.start_lease = ReferenceOwnerCars.start_lease;
                        dbEntry.end_lease = ReferenceOwnerCars.end_lease;
                        dbEntry.id_arrival = ReferenceOwnerCars.id_arrival;
                        dbEntry.create_dt = ReferenceOwnerCars.create_dt;
                        dbEntry.create_user = ReferenceOwnerCars.create_user;
                        dbEntry.change_dt = ReferenceOwnerCars.change_dt;
                        dbEntry.change_user = ReferenceOwnerCars.change_user;
                    }
                }
                context.SaveChanges();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveReferenceOwnerCars(ReferenceOwnerCars={0})", ReferenceOwnerCars.GetFieldsAndValue()), eventID);
                return -1;
            }
            return dbEntry.id;
        }

        public ReferenceOwnerCars DeleteReferenceOwnerCars(int id)
        {
            ReferenceOwnerCars dbEntry = context.ReferenceOwnerCars.Find(id);
            if (dbEntry != null)
            {
                try
                {
                    context.ReferenceOwnerCars.Remove(dbEntry);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    e.WriteErrorMethod(String.Format("DeleteReferenceOwnerCars(id={0})", id), eventID);
                    return null;
                }
            }
            return dbEntry;
        }
        /// <summary>
        /// Удалить все аренды по указаному номеру вагона
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public int DeleteReferenceOwnerCarsOfNum(int num)
        {
            try
            {
                foreach(ReferenceOwnerCars roc in context.ReferenceOwnerCars.Where(o=>o.num == num).ToList()){
                    context.ReferenceOwnerCars.Remove(roc);
                }
                return context.SaveChanges();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("DeleteReferenceOwnerCarsOfNum(num:{0})", num), eventID);
                return -1;
            }
        }
        #endregion
        #endregion

        #region ReferenceConsignee
        public IQueryable<ReferenceConsignee> ReferenceConsignee
        {
            get { return context.ReferenceConsignee; }
        }

        public IQueryable<ReferenceConsignee> GetReferenceConsignee()
        {
            try
            {
                return ReferenceConsignee;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetReferenceConsignee()"), eventID);
                return null;
            }
        }

        public ReferenceConsignee GetReferenceConsignee(int id)
        {
            try
            {
                return GetReferenceConsignee().Where(c => c.id == id).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetReferenceConsignee()"), eventID);
                return null;
            }
        }

        public ReferenceConsignee GetReferenceConsigneeOfKis(int id_kis)
        {
            try
            {
                return GetReferenceConsignee().Where(c => c.id_kis == id_kis).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetReferenceConsignee()"), eventID);
                return null;
            }
        }

        public int SaveReferenceConsignee(ReferenceConsignee ReferenceConsignee)
        {
            ReferenceConsignee dbEntry;
            try
            {
                if (ReferenceConsignee.id == 0)
                {
                    dbEntry = new ReferenceConsignee()
                    {
                        id = 0,
                        name_ru = ReferenceConsignee.name_ru,
                        name_en = ReferenceConsignee.name_en,
                        name_full_ru = ReferenceConsignee.name_full_ru,
                        name_full_en = ReferenceConsignee.name_full_en,
                        name_abr_ru = ReferenceConsignee.name_abr_ru, 
                        name_abr_en = ReferenceConsignee.name_abr_en,
                        id_shop = ReferenceConsignee.id_shop,
                        create_dt = ReferenceConsignee.create_dt != DateTime.Parse("01.01.0001") ? ReferenceConsignee.create_dt : DateTime.Now,
                        create_user = ReferenceConsignee.create_user != null ? ReferenceConsignee.create_user : System.Environment.UserDomainName + @"\" + System.Environment.UserName,
                        change_dt = null,
                        change_user = null,
                        id_kis = ReferenceConsignee.id_kis,
                        CarsInpDelivery = ReferenceConsignee.CarsInpDelivery,
                        Shops = ReferenceConsignee.Shops,
                    };
                    context.ReferenceConsignee.Add(dbEntry);
                }
                else
                {
                    dbEntry = context.ReferenceConsignee.Find(ReferenceConsignee.id);
                    // Сделано для отмены изменения даты создания строки
                    EFDbContext context_real = new EFDbContext();
                    ReferenceConsignee old_ReferenceConsignee = context_real.ReferenceConsignee.Where(c => c.id == ReferenceConsignee.id).FirstOrDefault();
                    if (dbEntry != null)
                    {
                        dbEntry.name_ru = ReferenceConsignee.name_ru;
                        dbEntry.name_en = ReferenceConsignee.name_en;
                        dbEntry.name_full_ru = ReferenceConsignee.name_full_ru;
                        dbEntry.name_full_en = ReferenceConsignee.name_full_en;
                        dbEntry.name_abr_ru = ReferenceConsignee.name_abr_ru; 
                        dbEntry.name_abr_en = ReferenceConsignee.name_abr_en;
                        dbEntry.id_shop = ReferenceConsignee.id_shop;
                        dbEntry.create_dt = old_ReferenceConsignee.create_dt;
                        dbEntry.create_user = old_ReferenceConsignee.create_user;
                        dbEntry.change_dt = ReferenceConsignee.change_dt != null ? ReferenceConsignee.change_dt : DateTime.Now;
                        dbEntry.change_user = ReferenceConsignee.change_user != null ? ReferenceConsignee.change_user : System.Environment.UserDomainName + @"\" + System.Environment.UserName;
                        dbEntry.CarsInpDelivery = ReferenceConsignee.CarsInpDelivery;
                        dbEntry.Shops = ReferenceConsignee.Shops;
                    }
                }
                context.SaveChanges();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveReferenceCargo(ReferenceCargo={0})", ReferenceConsignee.GetFieldsAndValue()), eventID);
                return -1;
            }
            return dbEntry.id;
        }

        public ReferenceConsignee DeleteReferenceConsignee(int id)
        {
            ReferenceConsignee dbEntry = context.ReferenceConsignee.Find(id);
            if (dbEntry != null)
            {
                try
                {
                    context.ReferenceConsignee.Remove(dbEntry);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    e.WriteErrorMethod(String.Format("DeleteReferenceConsignee(id={0})", id), eventID);
                    return null;
                }
            }
            return dbEntry;
        }
        #endregion


    }
}
