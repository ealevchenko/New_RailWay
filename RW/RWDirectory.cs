using EFKIS.Entities;
using EFReference.Entities;
using EFRW.Concrete;
using EFRW.Concrete.EFDirectory;
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
    /// Класс справочной систмы RailWay
    /// </summary>
    public class RWDirectory
    {
        private eventID eventID = eventID.RW_RWDirectory;
        private service servece_owner;
        private EFDbContext db;
        private bool log_detali = false;
        private bool reference_kis = true; // Использовать справочники КИС


        public RWDirectory()
        {
            this.db = new EFDbContext();
            this.servece_owner = service.Null;
        }

        public RWDirectory(EFDbContext db)
        {
            this.db = db;
            this.servece_owner = service.Null;
        }

        public RWDirectory(service servece_owner)
        {
            this.servece_owner = servece_owner;
            this.db = new EFDbContext();
        }

        public RWDirectory(EFDbContext db, service servece_owner)
        {
            this.servece_owner = servece_owner;
            this.db = db;
        }

        #region Справочник "Стран"
        /// <summary>
        /// Вернуть страну по коду 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Directory_Country GetCountryOfCode(int code) {
            try
            {
                EFDirectoryCountry ef_country = new EFDirectoryCountry(this.db);
                return ef_country.Get().Where(c => c.code == code).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetCountryOfCode(code={0})", code), servece_owner, eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить строку справочника "Стран" по коду, если нет создвать новую
        /// </summary>
        /// <param name="code"></param>
        /// <param name="create"></param>
        /// <returns></returns>
        protected Directory_Country GetCountry(Countrys code, bool create)
        {
            try
            {
                if (code == null) return null;
                if (code.code <= 0) return null;
                EFDirectoryCountry ef_country = new EFDirectoryCountry(this.db);

                Directory_Country country = GetCountryOfCode(code.code);
                if (country == null & create)
                {
                    country = new Directory_Country()
                    {
                        id = 0,
                        country_ru = code.country,
                        country_en = code.country,
                        code = code.code,
                    };
                    ef_country.Add(country);
                    ef_country.Save();
                }
                return country;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetCountry(code={0}, create={1})", code, create), servece_owner, eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить id страны из справочника "Стран" по коду, если нет создвать новую
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        protected int GetIDCountry(Countrys code)
        {
            try
            {
                Directory_Country country = GetCountry(code, true);
                return country != null ? country.id : 0;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetIDCountry(code={0})", code), servece_owner, eventID);
                return 0;
            }
        }
        /// <summary>
        /// Получить id строки справочника "Стран" системы RailWay по коду СНГ и стран балтии.
        /// </summary>
        /// <param name="code_country_sng"></param>
        /// <returns></returns>
        public int GetIDCountryOfCodeSNG(int code_country_sng)
        {
            try
            {
                EFReference.Concrete.EFReference ef_reference = new EFReference.Concrete.EFReference();
                Countrys code = ef_reference.GetCountryOfCodeSNG(code_country_sng);
                return GetIDCountry(code);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetIDCountryOfCodeSNG(code_country_sng={0})", code_country_sng), servece_owner, eventID);
                return 0;
            }
        }
        /// <summary>
        /// Получить id строки справочника "Стран" системы RailWay по коду МеталлургТранса
        /// </summary>
        /// <param name="code_mt"></param>
        /// <returns></returns>
        public int GetIDCountryOfCodeMT(string code_mt)
        {
            try
            {
                int country = int.Parse(code_mt.Substring(0, 2));
                return GetIDCountryOfCodeSNG(country);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetIDCountryOfCodeMT(code_mt={0})", code_mt), servece_owner, eventID);
                return 0;
            }
        }
        /// <summary>
        /// Получить id строки справочника "Стран" системы RailWay по коду iso
        /// </summary>
        /// <param name="code_country_iso"></param>
        /// <returns></returns>
        public int GetIDCountryOfCodeISO(int code_country_iso)
        {
            try
            {
                EFReference.Concrete.EFReference ef_reference = new EFReference.Concrete.EFReference();
                Countrys code = ef_reference.GetCountryOfCode(code_country_iso);
                return GetIDCountry(code);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetIDCountryOfCodeISO(code_country_sng={0})", code_country_iso), servece_owner, eventID);
                return 0;
            }
        }

        #endregion

        #region Справочник "Грузов"
        /// <summary>
        /// Получить строку справочника "Грузов" по коду ЕТСНГ,
        /// </summary>
        /// <param name="code_etsng"></param>
        /// <returns></returns>
        public Directory_Cargo GetCargoOfCodeETSNG(int code_etsng)
        {
            try
            {
                EFDirectoryCargo ef_cargo = new EFDirectoryCargo(this.db);
                return ef_cargo.Get().Where(c => c.etsng == code_etsng).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetCargoOfCodeETSNG(code_etsng={0})", code_etsng), servece_owner, eventID);
                return null;
            }
        }
        /// <summary>
        /// Вернуть строку справочника "Грузов" по коду ЕТСНГ, если нет создать
        /// </summary>
        /// <param name="code_etsng"></param>
        /// <param name="create"></param>
        /// <returns></returns>
        public Directory_Cargo GetCargoOfCodeETSNG(int code_etsng, bool create)
        {
            try
            {
                EFDirectoryCargo ef_cargo = new EFDirectoryCargo(this.db);
                EFReference.Concrete.EFReference reference = new EFReference.Concrete.EFReference();
                Directory_Cargo cargo = GetCargoOfCodeETSNG(code_etsng);
                if (cargo == null)
                {
                    EFReference.Entities.Cargo ref_cargo = reference.GetCargoOfCodeETSNG(code_etsng);
                    if (ref_cargo != null & create)
                    {
                        cargo = new Directory_Cargo()
                        {
                            name_ru = ref_cargo.name_etsng.Length > 200 ? ref_cargo.name_etsng.Remove(199).Trim() : ref_cargo.name_etsng.Trim(),
                            name_en = ref_cargo.name_etsng.Length > 200 ? ref_cargo.name_etsng.Remove(199).Trim() : ref_cargo.name_etsng.Trim(),
                            name_full_ru = ref_cargo.name_etsng.Length > 500 ? ref_cargo.name_etsng.Remove(499).Trim() : ref_cargo.name_etsng.Trim(),
                            name_full_en = ref_cargo.name_etsng.Length > 500 ? ref_cargo.name_etsng.Remove(499).Trim() : ref_cargo.name_etsng.Trim(),
                            etsng = code_etsng,
                            id_type = 0
                        };
                    }
                    else
                    {
                        cargo = new Directory_Cargo()
                        {
                            name_ru = "?",
                            name_en = "?",
                            name_full_ru = "?",
                            name_full_en = "?",
                            etsng = code_etsng,
                            id_type = 0
                        };
                    }
                    ef_cargo.Add(cargo);
                    ef_cargo.Save();
                }
                return cargo;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetCargoOfCodeETSNG(code_etsng={0}, create={1})", code_etsng, create), servece_owner, eventID);
                return null;
            }
        }
        /// <summary>
        /// Вернуть id строки справочника "Грузов" по коду ЕТСНГ, если нет создать
        /// </summary>
        /// <param name="code_etsng"></param>
        /// <returns></returns>
        public int GetIDCargoOfCodeETSNG(int code_etsng)
        {
            try
            {
                Directory_Cargo cargo = GetCargoOfCodeETSNG(code_etsng, true);
                return cargo != null ? cargo.id : 0;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetIDCargoOfCodeETSNG(code_etsng={0})", code_etsng), servece_owner, eventID);
                return 0;
            }
        }
        /// <summary>
        /// Вернуть id строки справочника "Грузов" по коду ЕТСНГ (с предварительной коррекцией), если нет создать
        /// </summary>
        /// <param name="code_etsng"></param>
        /// <returns></returns>
        public int GetIDCargoOfCorrectCodeETSNG(int code_etsng)
        {
            EFReference.Concrete.EFReference reference = new EFReference.Concrete.EFReference();
            return GetIDCargoOfCodeETSNG(reference.GetCodeCorrectCargo(code_etsng));
        }

        //public int GetIDCorrectCodeETSNGOfKis(int? code_kis)
        //{
        //    if (code_kis != null)
        //    {
        //        EFKIS.Concrete.EFWagons kis = new EFKIS.Concrete.EFWagons();
        //        EFKIS.Entities.PromGruzSP pg = kis.GetGruzSP((int)code_kis);
        //        if (pg != null && pg.TAR_GR != null)
        //        {
        //            EFReference.Concrete.EFReference reference = new EFReference.Concrete.EFReference();
        //            return reference.GetCodeCorrectCargo((int)pg.TAR_GR);
        //        }
        //    }
        //    return 0;
        //}

        #endregion

        #region Справочник "Станций railway"
        /// <summary>
        /// Получить строку справочника "Станций railway" по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Directory_InternalStations GetInternalStations(int id)
        {
            try
            {
                EFDirectoryInternalStations ef_station = new EFDirectoryInternalStations(this.db);
                return ef_station.Get(id);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetWays(id={0})", id), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить строку справочника "Станций railway" по коду станции УЗ
        /// </summary>
        /// <param name="code_uz"></param>
        /// <returns></returns>
        public Directory_InternalStations GetInternalStationsOfCodeUZ(int code_uz)
        {
            try
            {
                EFDirectoryInternalStations ef_station = new EFDirectoryInternalStations(this.db);
                return ef_station.Get().Where(c => c.code_uz == code_uz).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetInternalStationsOfCodeUZ(code_uz={0})", code_uz), servece_owner, eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить строку справочника "Станций railway" по коду станции из системы КИС
        /// </summary>
        /// <param name="id_kis"></param>
        /// <returns></returns>
        public Directory_InternalStations GetInternalStationsOfKIS(int id_kis)
        {
            try
            {
                EFDirectoryInternalStations ef_station = new EFDirectoryInternalStations(this.db);
                return ef_station.Get().Where(c => c.id_kis == id_kis).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetInternalStationsOfKIS(id_kis={0})", id_kis), servece_owner, eventID);
                return null;
            }
        }
        /// <summary>
        /// Вернуть строку станции УЗ справочника "Станций railway" по индексу поезда МТ
        /// </summary>
        /// <param name="index_mt"></param>
        /// <param name="create"></param>
        /// <returns></returns>
        public Directory_InternalStations GetInternalStationsUZ(string index_mt, bool create)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(index_mt)) return null;
                EFReference.Concrete.EFReference reference = new EFReference.Concrete.EFReference();
                int code = int.Parse(index_mt.Substring(9, 4));
                EFReference.Entities.Stations station_in = reference.GetStationsOfCode(code * 10);
                int codecs = station_in != null ? (int)station_in.code_cs : code * 10;
                return GetInternalStationsUZ(codecs, true);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetInternalStationsUZ(index_mt={0}, create={1})", index_mt, create), servece_owner, eventID);
                return null;
            }
        }
        /// <summary>
        /// Вернуть строку станции УЗ справочника "Станций railway" по коду уз
        /// </summary>
        /// <param name="code_uz"></param>
        /// <param name="create"></param>
        /// <returns></returns>
        public Directory_InternalStations GetInternalStationsUZ(int code_uz, bool create)
        {
            try
            {
                EFDirectoryInternalStations ef_station = new EFDirectoryInternalStations(this.db);
                
                EFReference.Concrete.EFReference reference = new EFReference.Concrete.EFReference();
                Directory_InternalStations station = GetInternalStationsOfCodeUZ(code_uz);
                // Если нет станции создадим
                if (station == null & create)
                {
                    Stations station_uz = reference.GetStationsOfCodeCS(code_uz);
                    station = new Directory_InternalStations()
                    {
                        id = 0,
                        name_ru = station_uz.station,
                        name_en = station_uz.station,
                        view = false,
                        exit_uz = false,
                        station_uz = true,
                        id_kis = null,
                        default_side = null,
                        code_uz = code_uz, 
                    };
                    ef_station.Add(station);
                    ef_station.Save();
                    // Если станция создались новая, добавим к ней пути 
                    if (station.id > 0) {
                        CreateDefaultWayStationUZ(station.id);
                    }
                }
                return station;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetInternalStationsUZ(code_uz={0}, create={1})", code_uz, create), servece_owner, eventID);
                return null;
            }
        }
        /// <summary>
        /// Вернуть строку станции справочника "Станций railway" по id kis, если нет создать по данным КИС
        /// </summary>
        /// <param name="id_kis"></param>
        /// <param name="create"></param>
        /// <returns></returns>
        public Directory_InternalStations GetInternalStationsOfKis(int id_kis, bool create)
        {
            try
            {
                EFDirectoryInternalStations ef_station = new EFDirectoryInternalStations(this.db);
                EFKIS.Concrete.EFWagons kis = new EFKIS.Concrete.EFWagons();
                Directory_InternalStations station = GetInternalStationsOfKIS(id_kis);
                // Если нет станции создадим
                if (station == null & create)
                {
                    EFKIS.Entities.KometaStan st = kis.GetKometaStan(id_kis);
                    station = new Directory_InternalStations()
                    {
                        id = 0,
                        name_ru = st != null ? st.NAME : "?",
                        name_en = st != null ? st.NAME : "?",
                        view = false,
                        exit_uz = false,
                        station_uz = st != null && st.MPS != null ? (bool)st.MPS : false,
                        id_kis = null,
                        default_side = null,
                        code_uz = st != null && st.MPS != null ? (int?)0 : null,
                    };
                    ef_station.Add(station);
                    ef_station.Save();

                    // Если станция создались станция УЗ, добавим к ней пути 
                    if (station.id > 0 && station.station_uz)
                    {
                        CreateDefaultWayStationUZ(station.id);
                    }
                }
                return station;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetInternalStationsOfKis(id_kis={0}, create={1})", id_kis, create), servece_owner, eventID);
                return null;
            }
        }
        /// <summary>
        /// Вернуть id станции справочника "Станций railway" по id kis, если нет создать по данным КИС 
        /// </summary>
        /// <param name="id_kis"></param>
        /// <returns></returns>
        public int GetIDInternalStationsOfKIS(int? id_kis)
        {
            try
            {
                if (id_kis == null) return 0;
                Directory_InternalStations station = GetInternalStationsOfKis((int)id_kis, this.reference_kis == true ? true : false);
                return station != null ? station.id : 0;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetInternalStationsOfKis(id_kis={0})", id_kis), servece_owner, eventID);
                return 0;
            }
        }

        #endregion

        #region Справочник "Путей railway"
        /// <summary>
        /// Вернуть путь по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Directory_Ways GetWay(int id)
        {
            try
            {
                EFDirectoryWays ef_way = new EFDirectoryWays(this.db);
                return ef_way.Get(id);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetWays(id={0})", id), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить строку справочника "Путей railway" по коду станции и номеру пити
        /// </summary>
        /// <param name="id_station"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public Directory_Ways GetWaysOfStation(int id_station, string num)
        {
            try
            {
                EFDirectoryWays ef_way = new EFDirectoryWays(this.db);
                return ef_way.Get().Where(w => w.id_station == id_station & w.num == num).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetWaysOfStation(id_station={0}, num={1})", id_station, num), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить список строк справочника "Путей railway" по коду станции
        /// </summary>
        /// <param name="id_station"></param>
        /// <returns></returns>
        public IEnumerable<Directory_Ways> GetWaysOfStation(int id_station)
        {
            try
            {
                EFDirectoryWays ef_way = new EFDirectoryWays(this.db);
                return ef_way.Get().Where(w => w.id_station == id_station);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetWaysOfStation(id_station={0})", id_station), eventID);
                return null;
            }
        }
        /// <summary>
        /// Создать на станции УЗ пути по умолчанию
        /// </summary>
        /// <param name="id_station"></param>
        /// <returns></returns>
        public bool CreateDefaultWayStationUZ(int id_station)
        {
            try
            {
                EFDirectoryWays ef_way = new EFDirectoryWays(this.db);
                Directory_Ways way_send_amkr = new Directory_Ways()
                {
                    id_station = id_station,
                    num = "1",
                    name_ru = "Путь для отправки на АМКР",
                    name_en = "The way to send to AMKR",
                    position = 1,
                    capacity = null,
                    id_type_way = 1,
                    id_car_status = 15,   // ожидаем прибытия с УЗ
                    id_station_end = null,
                    id_shop_end = null,
                    id_overturn_end = null,
                };
                ef_way.Add(way_send_amkr);
                Directory_Ways way_maneuver = new Directory_Ways()
                {
                    id_station = id_station,
                    num = "1",
                    name_ru = "Путь для маневра",
                    name_en = "The way to maneuver",
                    position = 2,
                    capacity = null,
                    id_type_way = 1,
                    id_car_status = 19, // ТСП по УЗ
                    id_station_end = null,
                    id_shop_end = null,
                    id_overturn_end = null,
                };
                ef_way.Add(way_maneuver);
                ef_way.Save();
                return way_send_amkr.id > 0 && way_maneuver.id > 0 ? true : false;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CreateDefaultWayStationUZ(id_station={0})", id_station), servece_owner, eventID);
                return false;
            }
        }
        /// <summary>
        /// Вернуть строку пути справочника "Пути railway" по id станции и номеру пути, если нет создать по данным КИС
        /// </summary>
        /// <param name="id_station"></param>
        /// <param name="num"></param>
        /// <param name="create"></param>
        /// <returns></returns>
        public Directory_Ways GetWay(int id_station, string num, bool create)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(num)) return null;
                EFDirectoryWays ef_way = new EFDirectoryWays(this.db);
                Directory_Ways way = GetWaysOfStation(id_station, num);
                // Если нет станции создадим
                if (way == null & create)
                {
                    // Создадим путь для отправки на АМКР
                    way = new Directory_Ways()
                    {
                        id_station = id_station,
                        num = num,
                        name_ru = "Путь создан по данным КИС",
                        name_en = "The path is based on the KIS data",
                        position = 0,
                        capacity = null,
                        id_type_way = 0,
                        id_car_status = null, 
                        id_station_end = null,
                        id_shop_end = null,
                        id_overturn_end = null,
                    };
                    ef_way.Add(way);
                    ef_way.Save();
                }
                return way;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetWay(id_station={0}, num={1}, create={2})", id_station, num, create), servece_owner, eventID);
                return null;
            }
        }
        /// <summary>
        /// Вернуть id пути справочника "Пути railway" по id станции и номеру пути, если нет создать по данным КИС
        /// </summary>
        /// <param name="id_station"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public int GetIDWayOfStation(int id_station, string num)
        {
            try
            {
                Directory_Ways way = GetWay(id_station, num, this.reference_kis == true ? true : false);
                return way != null ? way.id : 0;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetIDWayOfStation(id_station={0}, num={1})", id_station, num), servece_owner, eventID);
                return 0;
            }
        }
        /// <summary>
        /// Вернуть id пути справочника "Пути railway" по id станции и номеру пути, если нет создать по данным КИС, если не создан вернуть первый путь по умолчанию
        /// </summary>
        /// <param name="id_station"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public int GetIDDefaultWayOfStation(int id_station, string num)
        {
            try
            {

                int id = GetIDWayOfStation(id_station, num);
                if (id == 0)
                {
                    Directory_Ways Way = GetWaysOfStation(id_station).OrderBy(w => w.num).FirstOrDefault();
                    if (Way != null) { id = Way.id; }
                }
                return id;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetIDDefaultWayOfStation(id_station={0}, num={1})", id_station, num), servece_owner, eventID);
                return 0;
            }
        }
        /// <summary>
        /// Получить строки всех перегонов между станциями справочника "Путей railway"
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Directory_Ways> GetWaysExternalStation()
        {
            try
            {
                EFDirectoryWays ef_way = new EFDirectoryWays(this.db);
                return ef_way.Get().Where(w => w.id_station_end != null);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetWaysExternalStation()"), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить строки перегонов для отправки на другие станции справочника "Путей railway" по коду станции
        /// </summary>
        /// <param name="id_station"></param>
        /// <returns></returns>
        public IEnumerable<Directory_Ways> GetFromWaysExternalStation(int id_station)
        {
            try
            {
                EFDirectoryWays ef_way = new EFDirectoryWays(this.db);
                return ef_way.Get().Where(w => w.id_station == id_station);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetFromWaysExternalStation(id_station={0})",id_station), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить строки перегонов для приема из других станций справочника "Путей railway" по коду станции
        /// </summary>
        /// <param name="id_station"></param>
        /// <returns></returns>
        public IEnumerable<Directory_Ways> GetOnWaysExternalStation(int id_station)
        {
            try
            {
                EFDirectoryWays ef_way = new EFDirectoryWays(this.db);
                return ef_way.Get().Where(w => w.id_station_end == id_station);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetOnWaysExternalStation(id_station={0})",id_station), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить строки перегонов для отправки из станции УЗ на другие станции, справочник "Путей railway"
        /// </summary>
        /// <param name="id_station"></param>
        /// <returns></returns>
        public IEnumerable<Directory_Ways> GetFromWaysExternalStationUZ()
        {
            try
            {
                EFDirectoryWays ef_way = new EFDirectoryWays(this.db);
                return ef_way.Get().Where(w => w.Directory_InternalStations.station_uz);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetFromWaysExternalStationUZ()"), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить строки перегонов для приема на станци УЗ из других станций, справочник "Путей railway"
        /// </summary>
        /// <param name="id_station"></param>
        /// <returns></returns>
        public IEnumerable<Directory_Ways> GetOnWaysExternalStationUZ()
        {
            try
            {
                EFDirectoryWays ef_way = new EFDirectoryWays(this.db);
                //List<Directory_Ways> list = ef_way.Get().Where(w => w.id_type_way==2).ToList();
                //List<Directory_Ways> list = ef_way.Get().Where(w => w.Directory_InternalStations1 != null && w.Directory_InternalStations1.station_uz).ToList();
                return ef_way.Get().Where(w => w.Directory_InternalStations1 != null && w.Directory_InternalStations1.station_uz);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetOnWaysExternalStationUZ()"), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить строки путей станций УЗ для отправки на АМКР, справочник "Путей railway"
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Directory_Ways> GetSendingWaysStationUZ()
        {
            try
            {
                EFDirectoryWays ef_way = new EFDirectoryWays(this.db);
                //List<Directory_Ways> list = ef_way.Get().Where(w => w.Directory_InternalStations.station_uz && w.id_station_end == null && w.id_shop_end == null && w.id_overturn_end == null && w.num == "1" ).ToList();
                return ef_way.Get().Where(w => w.Directory_InternalStations.station_uz && w.id_station_end == null && w.id_shop_end == null && w.id_overturn_end == null && w.num == "1");
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetSendingWaysStationUZ()"), eventID);
                return null;
            }
        }

        public IEnumerable<Directory_Ways> GetManeuversWaysStationUZ()
        {
            try
            {
                EFDirectoryWays ef_way = new EFDirectoryWays(this.db);
                //List<Directory_Ways> list = ef_way.Get().Where(w => w.Directory_InternalStations.station_uz && w.id_station_end == null && w.id_shop_end == null && w.id_overturn_end == null && w.num == "2" ).ToList();
                return ef_way.Get().Where(w => w.Directory_InternalStations.station_uz && w.id_station_end == null && w.id_shop_end == null && w.id_overturn_end == null && w.num == "2");
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetManeuversWaysStationUZ()"), eventID);
                return null;
            }
        }
        #endregion

        #region Справочник типов вагонов
        /// <summary>
        /// Вернуть тип вагона по абривиатуре
        /// </summary>
        /// <param name="abr"></param>
        /// <returns></returns>
        public Directory_TypeCars GetTypeCarsOfAbr(string abr)
        {
            try
            {
                EFDirectoryTypeCars ef_type = new EFDirectoryTypeCars(this.db);
                return ef_type.Get().Where(t => t.type_cars_abr_ru.Trim().ToLower() == abr.Trim().ToLower()).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetTypeCarsOfAbr(abr={0})", abr), eventID);
                return null;
            }
        }
        /// <summary>
        /// Вернуть строку справочника тип вагона по роду вагона, если род вагона пуст вернет тип "Не определен"
        /// </summary>
        /// <param name="rod"></param>
        /// <returns></returns>
        public Directory_TypeCars GetTypeCarsOfRod(string rod)
        {
            try
            {
                EFDirectoryTypeCars ef_type = new EFDirectoryTypeCars(this.db);
                if (String.IsNullOrWhiteSpace(rod)) return ef_type.Get(0);
                return GetTypeCarsOfAbr(rod);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetTypeCarsOfRod(rod={0})", rod), servece_owner, eventID);
                return null;
            }
        }
        #endregion

        #region Справочник вагонов
        /// <summary>
        /// Получить строку справочника "Вагон" по номеру, если нет создать новую строку справочника "Вагон", "Аренда Вагона"
        /// </summary>
        /// <param name="num"></param>
        /// <param name="id_arrival"></param>
        /// <param name="dt"></param>
        /// <param name="id_country"></param>
        /// <param name="create_car"></param>
        /// <param name="create_owner"></param>
        /// <returns></returns>
        public Directory_Cars GetCarsOfNum(int num, int id_arrival, DateTime dt, int id_country, string note, string sap, bool create_car, bool create_owner)
        {
            try
            {

                EFDirectoryCars ef_car = new EFDirectoryCars(this.db);
                EFKIS.Concrete.EFWagons ef_wag = new EFKIS.Concrete.EFWagons();
                Directory_Cars car = ef_car.Get(num); ;
                // Создаем строку справочника вагона и строки аренды и владельца
                if (car == null & create_car)
                {
                    EFKIS.Entities.KometaVagonSob car_kis = null;
                    Directory_TypeCars type_car_kis = null;
                    List<EFKIS.Entities.KometaVagonSob> list_owner = null;
                    if (this.reference_kis)
                    {
                        list_owner = ef_wag.GetVagonsSob(num).ToList();
                        if (list_owner != null && list_owner.Count() > 0)
                        {
                            car_kis = ef_wag.GetVagonsSob(num, dt); // Получим текущий вагон из КИС
                            if (car_kis == null)
                            { // Текущего нет возьмем последнюю запись
                                car_kis = list_owner.FirstOrDefault();
                            }
                            type_car_kis = GetTypeCarsOfRod(car_kis != null ? car_kis.ROD : null);
                        }
                    }
                    // Создать строку справочника "Вагоны"
                    car = new Directory_Cars()
                    {
                        num = num,
                        id_type = (type_car_kis != null ? type_car_kis.id : 0),
                        note = note,
                        sap = sap,
                        lifting_capacity = 0,
                        tare = 0,
                        id_country = id_country,
                        count_axles = null,
                        is_output_uz = (car_kis != null ? true : false), // Выход на дорогу

                    };
                    ef_car.Add(car);
                    int res = ef_car.Save();
                    // Создаем список аренд и собственника вагона
                    if (res > 0 && create_owner)
                    {
                        int res_oc = AddOwnerCars(num, id_arrival, dt);
                    }
                }
                return car;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetReferenceCarsOfNum(num={0}, id_arrival={1}, dt={2}, id_country={3}, note={4}, sap={5}, create_car={6}, create_owner={7})",
                    num, id_arrival, dt, id_country, note, sap, create_car, create_owner), servece_owner, eventID);
                return null;
            }
        }
        #endregion

        #region Справочник "Владельцы вагонов"
        /// <summary>
        /// Вернуть строку справочника "Владельцы вагонов" по коду системы KIS
        /// </summary>
        /// <param name="id_kis"></param>
        /// <returns></returns>
        public Directory_Owners GetOwnersOfKIS(int id_kis)
        {
            try
            {
                EFDirectoryOwners ef_owner = new EFDirectoryOwners(this.db);
                return ef_owner.Get().Where(o => o.id_kis == id_kis).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetOwnersOfKIS(id_kis={0})", id_kis), servece_owner, eventID);
                return null;
            };
        }
        /// <summary>
        /// Получить владельца по id_kis, если нет создать по данным КИС
        /// </summary>
        /// <param name="id_kis"></param>
        /// <param name="create"></param>
        /// <returns></returns>
        public Directory_Owners GetOwnersOfKIS(int id_kis, bool create)
        {
            try
            {
                EFKIS.Concrete.EFWagons ef_wag = new EFKIS.Concrete.EFWagons();
                //EFRW.Concrete.EFReference ef_ref = new EFRW.Concrete.EFReference();
                EFDirectoryOwners ef_owner = new EFDirectoryOwners(this.db);
                Directory_Owners owner= GetOwnersOfKIS(id_kis);
                if (owner == null & create)
                {
                    KometaSobstvForNakl owner_kis = ef_wag.GetSobstvForNakl(id_kis);
                    if (owner_kis != null)
                    {
                        owner = new Directory_Owners()
                        {
                            id = 0,
                            owner_name = owner_kis.NPLAT,
                            owner_abr = owner_kis.ABR,
                            id_kis = owner_kis.SOBSTV
                        };
                        ef_owner.Add(owner);
                        ef_owner.Save();
                    }
                }
                return owner;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetOwnersOfKIS(id_kis={0}, create={1})", servece_owner, id_kis, create), servece_owner, eventID);
                return null;
            }
        }
        #endregion

        #region Справочник "Аренда вагонов"
        /// <summary>
        /// Создать строку справочника "Аренда вагона"
        /// </summary>
        /// <param name="num"></param>
        /// <param name="id_owner"></param>
        /// <param name="start_lease"></param>
        /// <param name="end_lease"></param>
        /// <param name="id_arrival"></param>
        /// <returns></returns>
        public int CreateOwnerCars(int num, int id_owner, DateTime? start_lease, DateTime? end_lease, int? id_arrival)
        {
            try
            {
                EFDirectoryOwnerCars ef_oc = new EFDirectoryOwnerCars(this.db);
                Directory_OwnerCars owner_car = new Directory_OwnerCars()
                {
                    num = num,
                    id_owner = id_owner,
                    start_lease = start_lease,
                    end_lease = end_lease,
                    id_arrival = id_arrival,
                };
                ef_oc.Add(owner_car);
                ef_oc.Save();
                return owner_car.id;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CreateOwnerCars(num={0}, id_owner={1}, start_lease={2}, end_lease={3}, id_arrival={4})",
                    num, id_owner, start_lease, end_lease, id_arrival), servece_owner, eventID);
                return -1;
            }
        }
        /// <summary>
        /// Добавить в систему RailWay строки справочника "Аренда вагона" на новый вагон
        /// </summary>
        /// <param name="num"></param>
        /// <param name="id_arrival"></param>
        /// <param name="dt"></param>
        public int AddOwnerCars(int num, int id_arrival, DateTime dt)
        {
            try
            {
                EFKIS.Concrete.EFWagons ef_wag = new EFKIS.Concrete.EFWagons();

                int result = 0;
                List<KometaVagonSob> list_owner = ef_wag.GetVagonsSob(num).ToList();
                // Проверим есть данные и признак использовать данные КИС - переносим данные в сисему RW
                if (list_owner != null && list_owner.Count() > 0 & this.reference_kis)
                {
                    foreach (KometaVagonSob kvs in list_owner)
                    {
                        // Определим владельца если нет создадим
                        Directory_Owners owner = GetOwnersOfKIS(kvs.SOB, true);
                        // Создать по данным кис
                        int res_roc = CreateOwnerCars(num, (owner != null ? owner.id : 0), kvs.DATE_AR, kvs.DATE_END, id_arrival);
                        if (res_roc > 0) { result++; }
                    }
                }
                else
                {
                    //Создать без данных кис
                    int res_roc = CreateOwnerCars(num, 0, dt, null, id_arrival);
                    if (res_roc > 0) { result++; }
                }
                return result;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("AddOwnerCars(num={0}, id_arrival={1}, dt={2})", num, id_arrival, dt), servece_owner, eventID);
                return -1;
            }
        }
        #endregion

    }
}
