using MessageLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libClass;
using EFReference.Abstract;
using EFReference.Entities;

namespace EFReference.Concrete
{
    public class EFReference : IReference
    {
        private eventID eventID = eventID.EFReference;

        protected EFDbContext context = new EFDbContext();
        
        #region Cargo
        public IQueryable<Cargo> Cargo
        {
            get { return context.Cargo; }
        }
        /// <summary>
        /// Получить все строки справочника грузов
        /// </summary>
        /// <returns></returns>
        public IQueryable<Cargo> GetCargo()
        {
            try
            {
                return Cargo;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetCargo()"), eventID);
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_cargo"></param>
        /// <returns></returns>
        public Cargo GetCargo(int id)
        {
            try
            {
                return GetCargo().Where(c => c.id == id).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetCargo(id={0})", id), eventID);
                return null;
            }
        }

        public int SaveCargo(Cargo Cargo)
        {
            Cargo dbEntry;
            try
            {
                if (Cargo.id == 0)
                {
                    dbEntry = new Cargo()
                    {
                        id = 0,
                        code_etsng = Cargo.code_etsng,
                        name_etsng = Cargo.name_etsng,
                        code_gng = Cargo.code_gng,
                        name_gng = Cargo.name_gng,
                        id_sap = Cargo.id_sap
                    };
                    context.Cargo.Add(dbEntry);
                }
                else
                {
                    dbEntry = context.Cargo.Find(Cargo.id);
                    if (dbEntry != null)
                    {
                        dbEntry.code_etsng = Cargo.code_etsng;
                        dbEntry.name_etsng = Cargo.name_etsng;
                        dbEntry.code_gng = Cargo.code_gng;
                        dbEntry.name_gng = Cargo.name_gng;
                        dbEntry.id_sap = Cargo.id_sap;
                    }
                }
                context.SaveChanges();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveCargo(Cargo={0})", Cargo.GetFieldsAndValue()), eventID);
                return -1;
            }
            return dbEntry.id;
        }

        public Cargo DeleteCargo(int id)
        {
            Cargo dbEntry = context.Cargo.Find(id);
            if (dbEntry != null)
            {
                try
                {
                    context.Cargo.Remove(dbEntry);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    e.WriteErrorMethod(String.Format("DeleteCargo(id={0})", id), eventID);
                    return null;
                }
            }
            return dbEntry;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code_etsng"></param>
        /// <returns></returns>
        public Cargo GetCargoOfCodeETSNG(int code_etsng)
        {
            return GetCargo().Where(c => c.code_etsng == code_etsng).FirstOrDefault();
        }
        /// <summary>
        /// Вернуть список грузов по указаному деапазону кодов ETSNG
        /// </summary>
        /// <param name="code_start"></param>
        /// <param name="code_stop"></param>
        /// <returns></returns>
        public IQueryable<Cargo> GetCargoOfCodeETSNG(int code_etsng_start, int code_etsng_stop)
        {
            return GetCargo().Where(c => c.code_etsng >= code_etsng_start & c.code_etsng <= code_etsng_stop);
        }
        /// <summary>
        /// Вернуть уточненую строку ReferenceCargo
        /// </summary>
        /// <param name="code_etsng"></param>
        /// <returns></returns>
        public Cargo GetCorrectCargo(int code_etsng)
        {
            Cargo ref_cargo = GetCargoOfCodeETSNG(code_etsng);
            if (ref_cargo == null)
            {
                ref_cargo = GetCargoOfCodeETSNG(code_etsng * 10, (code_etsng * 10) + 9).FirstOrDefault();
            }
            return ref_cargo;
        }
        /// <summary>
        /// Вернуть откорректированный код ETSNG
        /// </summary>
        /// <param name="code_etsng"></param>
        /// <returns></returns>
        public int GetCodeCorrectCargo(int code_etsng)
        {
            Cargo ref_cargo = GetCorrectCargo(code_etsng);
            return ref_cargo != null ? ref_cargo.code_etsng : code_etsng;
        }
        #endregion

        #region Stations

        public IQueryable<Stations> Stations
        {
            get { return context.Stations; }
        }

        public IQueryable<Stations> GetStations()
        {
            try
            {
                return Stations;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GeStations()"), eventID);
                return null;
            }
        }

        public Stations GetStations(int id)
        {
            try
            {
                return GetStations().Where(c => c.id == id).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GeStations(id={0})",id), eventID);
                return null;
            }
        }

        public int SaveStations(Stations Stations)
        {
            Stations dbEntry;
            try
            {
                if (Stations.id == 0)
                {
                    dbEntry = new Stations()
                    {
                        id = 0,
                        code = Stations.code,
                        code_cs = Stations.code_cs,
                        station = Stations.station,
                        id_ir = Stations.id_ir,
                        InternalRailroad = Stations.InternalRailroad
                    };
                    context.Stations.Add(dbEntry);
                }
                else
                {
                    dbEntry = context.Stations.Find(Stations.id);
                    if (dbEntry != null)
                    {
                        dbEntry.code = Stations.code;
                        dbEntry.code_cs = Stations.code_cs;
                        dbEntry.station = Stations.station;
                        dbEntry.id_ir = Stations.id_ir;
                        dbEntry.InternalRailroad = Stations.InternalRailroad;
                    }
                }
                context.SaveChanges();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveStations(Stations={0})", Stations.GetFieldsAndValue()), eventID);
                return -1;
            }
            return dbEntry.id;
        }

        public Stations DeleteStations(int id)
        {
            Stations dbEntry = context.Stations.Find(id);
            if (dbEntry != null)
            {
                try
                {
                    context.Stations.Remove(dbEntry);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    e.WriteErrorMethod(String.Format("DeleteStations(id={0})", id), eventID);
                    return null;
                }
            }
            return dbEntry;
        }

        public Stations GetStationsOfCode(int code) {
            try
            {
                return GetStations().Where(c => c.code == code).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetStationsOfCode(code={0})", code), eventID);
                return null;
            }
        }

        public IQueryable<Stations>  GetStationsOfCode(int code_start, int code_stop) {
            try
            {
                return GetStations().Where(c => c.code >= code_start & c.code<=code_stop);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetStationsOfCode(code_start={0}, code_stop={1})", code_start, code_stop), eventID);
                return null;
            }
        }
        /// <summary>
        /// Вернуть строку станции по скорректированному коду c проверкой текущего кода или нет (добавляем в конец вариант 0..9)
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Stations GetCorrectStationsOfCode(int code, bool check)
        {
            try
            {
                Stations ref_station = null;
                if (check) { ref_station = GetStationsOfCode(code); }
                if (ref_station == null)
                {
                    ref_station = GetStationsOfCode(code * 10, (code * 10) + 9).FirstOrDefault();
                }
                return ref_station;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetCorrectStationsOfCode(code={0})", code), eventID);
                return null;
            }
        }

        public Stations GetStationsOfCodeCS(int code_cs) {
            try
            {
                return GetStations().Where(c => c.code_cs == code_cs).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetStationsOfCode(code_cs={0})", code_cs), eventID);
                return null;
            }
        }

        #endregion

        #region Countrys
        public IQueryable<Countrys> Countrys
        {
            get { return context.Countrys; }
        }

        public IQueryable<Countrys> GetCountrys()
        {
            try
            {
                return Countrys;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetCountrys()"), eventID);
                return null;
            }
        }

        public Countrys GetCountrys(int id)
        {
            try
            {
                return GetCountrys().Where(c => c.id == id).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetCountrys(id={0})", id), eventID);
                return null;
            }
        }

        public int SaveCountrys(Countrys Countrys)
        {
            Countrys dbEntry;
            try
            {
                if (Countrys.id == 0)
                {
                    dbEntry = new Countrys()
                    {
                        id = 0,
                        country = Countrys.country, 
                        alpha_2 = Countrys.alpha_2,
                        alpha_3 = Countrys.alpha_3, 
                        code = Countrys.code,
                        iso3166_2 = Countrys.iso3166_2,
                        id_state = Countrys.id_state,
                        code_europe = Countrys.code_europe,
                        States = Countrys.States
                    };
                    context.Countrys.Add(dbEntry);
                }
                else
                {
                    dbEntry = context.Countrys.Find(Countrys.id);
                    if (dbEntry != null)
                    {
                        dbEntry.country = Countrys.country;
                        dbEntry.alpha_2 = Countrys.alpha_2;
                        dbEntry.alpha_3 = Countrys.alpha_3;
                        dbEntry.code = Countrys.code;
                        dbEntry.iso3166_2 = Countrys.iso3166_2;
                        dbEntry.id_state = Countrys.id_state;
                        dbEntry.code_europe = Countrys.code_europe;
                        dbEntry.States = Countrys.States;
                    }
                }
                context.SaveChanges();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveCountrys(Countrys={0})", Countrys.GetFieldsAndValue()), eventID);
                return -1;
            }
            return dbEntry.id;
        }

        public Countrys DeleteCountrys(int id)
        {
            Countrys dbEntry = context.Countrys.Find(id);
            if (dbEntry != null)
            {
                try
                {
                    context.Countrys.Remove(dbEntry);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    e.WriteErrorMethod(String.Format("DeleteCountrys(id={0})", id), eventID);
                    return null;
                }
            }
            return dbEntry;
        }
        /// <summary>
        /// Вернуть строку справочника по коду СНГ и стран балтии
        /// </summary>
        /// <param name="code_sng"></param>
        /// <returns></returns>
        public Countrys GetCountryOfCodeSNG(int code_sng)
        {
            return GetCountrys().Where(c => c.id_state == code_sng).FirstOrDefault();
        }
        /// <summary>
        /// Вернуть строку кодов страны по коду iso
        /// </summary>
        /// <param name="code_iso"></param>
        /// <returns></returns>
        public Countrys GetCountryOfCode(int code_iso)
        {
            return GetCountrys().Where(c => c.code == code_iso).FirstOrDefault();
        }
        #endregion
    }
}
