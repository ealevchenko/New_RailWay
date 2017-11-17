using EFRC.Abstract;
using EFRC.Entities;
using MessageLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libClass;

namespace EFRC.Concrete
{
    public class EFRCReference :IRCReference
    {
        private eventID eventID = eventID.EFRCReference;

        protected EFDbContext context = new EFDbContext();

        public EFRCReference() { }

        #region ReferenceCargo (Справочник грузов)
        /// <summary>
        /// 
        /// </summary>
        public IQueryable<ReferenceCargo> ReferenceCargo
        {
            get { return context.ReferenceCargo; }
        }
        /// <summary>
        /// Добавить или править
        /// </summary>
        /// <param name="ReferenceCargo"></param>
        /// <returns></returns>
        public int SaveReferenceCargo(ReferenceCargo ReferenceCargo)
        {
            ReferenceCargo dbEntry;
            try
            {

                if (ReferenceCargo.IDCargo == 0)
                {
                    dbEntry = new ReferenceCargo()
                    {
                        IDCargo = 0,
                        Name = ReferenceCargo.Name,
                        NameFull = ReferenceCargo.NameFull,
                        ETSNG = ReferenceCargo.ETSNG,
                        TypeCargo = ReferenceCargo.TypeCargo,
                        DateTime = ReferenceCargo.DateTime
                    };
                    context.ReferenceCargo.Add(dbEntry);
                }
                else
                {
                    dbEntry = context.ReferenceCargo.Find(ReferenceCargo.IDCargo);
                    if (dbEntry != null)
                    {
                        dbEntry.Name = ReferenceCargo.Name;
                        dbEntry.NameFull = ReferenceCargo.NameFull;
                        dbEntry.ETSNG = ReferenceCargo.ETSNG;
                        dbEntry.TypeCargo = ReferenceCargo.TypeCargo;
                        dbEntry.DateTime = ReferenceCargo.DateTime;
                    }
                }

                context.SaveChanges();
            }
            catch (Exception e)
            {

                e.WriteErrorMethod(String.Format("SaveReferenceCargo(ReferenceCargo={0})", ReferenceCargo.GetFieldsAndValue()), eventID);
                return -1;
            }
            return dbEntry.IDCargo;

        }
        /// <summary>
        /// Удалить
        /// </summary>
        /// <param name="IDCargo"></param>
        /// <returns></returns>
        public ReferenceCargo DeleteReferenceCargo(int IDCargo)
        {
            ReferenceCargo dbEntry = context.ReferenceCargo.Find(IDCargo);
            if (dbEntry != null)
            {
                try
                {
                    context.ReferenceCargo.Remove(dbEntry);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    e.WriteErrorMethod(String.Format("DeleteReferenceCargo(IDCargo={0})", IDCargo), eventID);
                    return null;
                }
            }
            return dbEntry;
        }
        /// <summary>
        /// 
        /// </summary>
        public IQueryable<TypeCargo> TypeCargo
        {
            get { return context.TypeCargo; }
        }
        /// <summary>
        /// Добавить править
        /// </summary>
        /// <param name="TypeCargo"></param>
        /// <returns></returns>
        public int SaveTypeCargo(TypeCargo TypeCargo)
        {
            TypeCargo dbEntry;
            try
            {
            if (TypeCargo.ID == 0)
            {
                dbEntry = new TypeCargo()
                {
                    ID = 0,
                    TypeCargoRU = TypeCargo.TypeCargoRU,
                    TypeCargoEN = TypeCargo.TypeCargoEN
                };
                context.TypeCargo.Add(dbEntry);
            }
            else
            {
                dbEntry = context.TypeCargo.Find(TypeCargo.ID);
                if (dbEntry != null)
                {
                    dbEntry.TypeCargoRU = TypeCargo.TypeCargoRU;
                    dbEntry.TypeCargoEN = TypeCargo.TypeCargoEN;
                }
            }

                context.SaveChanges();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveTypeCargo(TypeCargo={0})", TypeCargo.GetFieldsAndValue()), eventID);
                return -1;
            }
            return dbEntry.ID;
        }
        /// <summary>
        /// Удалить
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public TypeCargo DeleteTypeCargo(int ID)
        {
            TypeCargo dbEntry = context.TypeCargo.Find(ID);
            if (dbEntry != null)
            {
                try
                {
                    context.TypeCargo.Remove(dbEntry);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    e.WriteErrorMethod(String.Format("DeleteTypeCargo(ID={0})", ID), eventID);
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
        public ReferenceCargo GetReferenceCargo(int id_cargo)
        {
            return GetReferenceCargo().Where(r => r.IDCargo == id_cargo).FirstOrDefault();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code_etsng"></param>
        /// <returns></returns>
        public ReferenceCargo GetReferenceCargoOfCodeETSNG(int code_etsng)
        {
            return GetReferenceCargo().Where(r => r.ETSNG == code_etsng).FirstOrDefault();
        }
        /// <summary>
        /// Вернуть список грузов по указаному деапазону кодов ETSNG
        /// </summary>
        /// <param name="code_start"></param>
        /// <param name="code_stop"></param>
        /// <returns></returns>
        public IQueryable<ReferenceCargo> GetReferenceCargoOfCodeETSNG(int code_start, int code_stop)
        {
            return GetReferenceCargo().Where(r => r.ETSNG >= code_start & r.ETSNG <= code_stop);
        }
        /// <summary>
        /// Вернуть откорректированный код ETSNG
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public int GetCorectReferenceCargo(int code)
        {
            ReferenceCargo ref_cargo = GetReferenceCargoOfCodeETSNG(code);
            if (ref_cargo == null)
            {
                ref_cargo = GetReferenceCargoOfCodeETSNG(code * 10, (code * 10) + 9).FirstOrDefault();
            }
            return ref_cargo != null ? ref_cargo.ETSNG : code;
        }
        /// <summary>
        /// Получить все строки рода груза
        /// </summary>
        /// <returns></returns>
        public IQueryable<TypeCargo> GetTypeCargo()
        {
            try
            {
                return TypeCargo;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetTypeCargo()"), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить указаный род груза
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TypeCargo GetTypeCargo(int id)
        {
            return GetTypeCargo().Where(t => t.ID == id).FirstOrDefault();
        }
        #endregion

        #region ReferenceCountry (Справочник стран)
        public IQueryable<ReferenceCountry> ReferenceCountry
        {
            get { return context.ReferenceCountry; }
        }
        /// <summary>
        /// Добавить или править
        /// </summary>
        /// <param name="ReferenceCountry"></param>
        /// <returns></returns>
        public int SaveReferenceCountry(ReferenceCountry ReferenceCountry)
        {
            ReferenceCountry dbEntry;
            try
            {
                if (ReferenceCountry.IDCountry == 0)
                {
                    dbEntry = new ReferenceCountry()
                    {
                        IDCountry = 0,
                        Country = ReferenceCountry.Country,
                        Code = ReferenceCountry.Code
                    };
                    context.ReferenceCountry.Add(dbEntry);
                }
                else
                {
                    dbEntry = context.ReferenceCountry.Find(ReferenceCountry.IDCountry);
                    if (dbEntry != null)
                    {
                        dbEntry.Country = ReferenceCountry.Country;
                        dbEntry.Code = ReferenceCountry.Code;
                    }
                }

                context.SaveChanges();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveReferenceCountry(ReferenceCountry={0})", ReferenceCountry.GetFieldsAndValue()), eventID);
                return -1;
            }
            return dbEntry.IDCountry;

        }
        /// <summary>
        /// Удалить
        /// </summary>
        /// <param name="IDCountry"></param>
        /// <returns></returns>
        public ReferenceCountry DeleteReferenceCountry(int IDCountry)
        {
            ReferenceCountry dbEntry = context.ReferenceCountry.Find(IDCountry);
            if (dbEntry != null)
            {
                context.ReferenceCountry.Remove(dbEntry);
                try
                {
                    context.ReferenceCountry.Remove(dbEntry);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    e.WriteErrorMethod(String.Format("DeleteReferenceCountry(IDCountry={0})", IDCountry), eventID);
                    return null;
                }
            }
            return dbEntry;
        }
        /// <summary>
        /// Получить перечень всех записей справочника
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// Получить запись справочника по id
        /// </summary>
        /// <param name="id_country"></param>
        /// <returns></returns>
        public ReferenceCountry GetReferenceCountry(int id_country)
        {
            return GetReferenceCountry().Where(r => r.IDCountry == id_country).FirstOrDefault();
        }
        /// <summary>
        /// Получить запись справочника по коду страны ISO3166
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public ReferenceCountry GetReferenceCountryOfCode(int code)
        {
            return GetReferenceCountry().Where(r => r.Code == code).FirstOrDefault();
        }

        #endregion

        #region ReferenceStation (Справочник станций)
        public IQueryable<ReferenceStation> ReferenceStation
        {
            get { return context.ReferenceStation; }
        }
        /// <summary>
        /// Добавить или править
        /// </summary>
        /// <param name="ReferenceCountry"></param>
        /// <returns></returns>
        public int SaveReferenceStation(ReferenceStation ReferenceStation)
        {
            ReferenceStation dbEntry;
            try
            {
                if (ReferenceStation.IDStation == 0)
                {
                    dbEntry = new ReferenceStation()
                    {
                        IDStation = 0,
                        Name = ReferenceStation.Name,
                        Station = ReferenceStation.Station,
                        InternalRailroad = ReferenceStation.InternalRailroad,
                        IR_Abbr = ReferenceStation.IR_Abbr,
                        NameNetwork = ReferenceStation.NameNetwork,
                        NN_Abbr = ReferenceStation.NN_Abbr,
                        CodeCS = ReferenceStation.CodeCS
                    };
                    context.ReferenceStation.Add(dbEntry);
                }
                else
                {
                    dbEntry = context.ReferenceStation.Find(ReferenceStation.IDStation);
                    if (dbEntry != null)
                    {
                        dbEntry.Name = ReferenceStation.Name;
                        dbEntry.Station = ReferenceStation.Station;
                        dbEntry.InternalRailroad = ReferenceStation.InternalRailroad;
                        dbEntry.IR_Abbr = ReferenceStation.IR_Abbr;
                        dbEntry.NameNetwork = ReferenceStation.NameNetwork;
                        dbEntry.NN_Abbr = ReferenceStation.NN_Abbr;
                        dbEntry.CodeCS = ReferenceStation.CodeCS;
                    }
                }

                context.SaveChanges();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveReferenceStation(ReferenceStation={0})", ReferenceStation.GetFieldsAndValue()), eventID);
                return -1;
            }
            return dbEntry.IDStation;
        }
        /// <summary>
        /// Удалить
        /// </summary>
        /// <param name="IDCountry"></param>
        /// <returns></returns>
        public ReferenceStation DeleteReferenceStation(int IDStation)
        {
            ReferenceStation dbEntry = context.ReferenceStation.Find(IDStation);
            if (dbEntry != null)
            {
                context.ReferenceStation.Remove(dbEntry);
                try
                {
                    context.ReferenceStation.Remove(dbEntry);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    e.WriteErrorMethod(String.Format("DeleteReferenceStation(IDStation={0})", IDStation), eventID);
                    return null;
                }
            }
            return dbEntry;
        }
        /// <summary>
        /// Показать все станции
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// Показать станцию по указаному id
        /// </summary>
        /// <param name="IDStation"></param>
        /// <returns></returns>
        public ReferenceStation GetReferenceStation(int IDStation)
        {
            return GetReferenceStation().Where(s => s.IDStation == IDStation).FirstOrDefault();
        }
        /// <summary>
        /// Показать станцию по указаному коду
        /// </summary>
        /// <param name="codecs"></param>
        /// <returns></returns>
        public ReferenceStation GetReferenceStationOfCode(int codecs)
        {
            return GetReferenceStation().Where(s => s.CodeCS == codecs).FirstOrDefault();
        }
        #endregion


    }
}
