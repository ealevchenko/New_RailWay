using EFKIS.Concrete;
using EFKIS.Entities;
using EFRW.Concrete;
using EFRW.Entities;
using MessageLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW
{
    public class RWReference
    {
        private eventID eventID = eventID.RW_RWReference;
        protected service servece_owner = service.Null;
        private bool reference_kis = true; // Использовать справочники КИС

        public RWReference(bool reference_kis)
        {
            this.reference_kis = reference_kis;
        }

        public RWReference(service servece_owner, bool reference_kis)
        {
            this.servece_owner = servece_owner;
        }

        #region Справочник вагонов
        /// <summary>
        /// Создать строку справочника "Вагоны"
        /// </summary>
        /// <param name="num"></param>
        /// <param name="id_type"></param>
        /// <param name="id_country"></param>
        /// <param name="output_uz"></param>
        /// <returns></returns>
        public int CreateReferenceCars(int num, int id_arrival, DateTime dt, int id_type, int id_country, bool? output_uz, bool create_owner_cars)
        {
            try
            {
                EFRW.Concrete.EFReference ef_ref = new EFRW.Concrete.EFReference();
                ReferenceCars ref_car = new ReferenceCars()
                {
                    num = num,
                    id_type = id_type,
                    lifting_capacity = 0,
                    tare = 0,
                    id_country = id_country,
                    count_axles = null,
                    is_output_uz = output_uz,
                    create_dt = DateTime.Now,
                    create_user = System.Environment.UserDomainName + @"\" + System.Environment.UserName,
                    change_dt = null,
                    change_user = null,
                    //Cars = null,
                    //ReferenceCountry = null,
                    //ReferenceOwnerCars = null,
                    //ReferenceTypeCars = null
                };
                int res = ef_ref.SaveReferenceCars(ref_car);
                // Создаем список аренд и собственника вагона
                if (res > 0 & create_owner_cars)
                {
                    int res_oc = AddOwnerCars(num, id_arrival, dt);
                }
                return res;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CreateReferenceCars(num={0}, id_arrival={1}, dt={2}, id_type={3}, id_country={4}, output_uz={5}, create_owner_cars={6})", num, id_arrival, dt, id_type, id_country, output_uz, create_owner_cars), servece_owner, eventID);
                return -1;
            }
        }
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
        public ReferenceCars GetReferenceCarsOfNum(int num, int id_arrival, DateTime dt, int id_country, bool create_car, bool create_owner)
        {
            try
            {
                EFRW.Concrete.EFReference ef_ref = new EFRW.Concrete.EFReference();
                EFWagons ef_wag = new EFWagons();
                //EFRailWay ef_rw = new EFRailWay();
                ReferenceCars ref_car = ef_ref.GetReferenceCars(num);
                // Создаем строку справочника вагона и строки аренды и владельца
                if (ref_car == null & create_car)
                {
                    KometaVagonSob car_kis = null;
                    ReferenceTypeCars type_car_kis = null;
                    List<KometaVagonSob> list_owner = null;
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
                            type_car_kis = GetReferenceTypeCarsOfRod(car_kis != null ? car_kis.ROD : null);
                        }
                    }
                    // Создать строку справочника "Вагоны"
                    int res = CreateReferenceCars(num, id_arrival, dt, (type_car_kis != null ? type_car_kis.id : 0), id_country, (car_kis != null ? true : false), create_owner);
                    if (res > 0)
                    {
                        ref_car = ef_ref.GetReferenceCars(res);
                    }
                }
                return ref_car;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetReferenceCarsOfNum(num={0}, id_arrival={1}, dt={2}, id_country={3}, create_car={4}, create_owner={5})", num, id_arrival, dt, id_country, create_car, create_owner), servece_owner, eventID);
                return null;
            }
        }
        #endregion

        #region Справочник типов вагонов
        /// <summary>
        /// Вернуть строку справочника тип вагона по роду вагона, если род вагона пуст вернет тип "Не определен"
        /// </summary>
        /// <param name="rod"></param>
        /// <returns></returns>
        public ReferenceTypeCars GetReferenceTypeCarsOfRod(string rod)
        {
            try
            {
                EFRW.Concrete.EFReference ef_ref = new EFRW.Concrete.EFReference();
                if (String.IsNullOrWhiteSpace(rod)) return ef_ref.GetReferenceTypeCars(0);
                return ef_ref.GetReferenceTypeCarsOfAbr(rod);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetReferenceTypeCarsOfRod(rod={0})", rod), servece_owner, eventID);
                return null;
            }
        }
        #endregion

        #region Справочник "Владельцы вагонов"
        /// <summary>
        /// Создать строку справочника "Владелец вагона"
        /// </summary>
        /// <param name="owner_name"></param>
        /// <param name="owner_abr"></param>
        /// <param name="id_kis"></param>
        /// <returns></returns>
        public int CreateOwner(string owner_name, string owner_abr, int? id_kis)
        {
            try
            {
                EFRW.Concrete.EFReference ef_ref = new EFRW.Concrete.EFReference();
                ReferenceOwners owner_rw_new = new ReferenceOwners()
                {
                    id = 0,
                    owner_name = owner_name,
                    owner_abr = owner_abr,
                    id_kis = id_kis
                };
                return ef_ref.SaveReferenceOwners(owner_rw_new);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CreateOwner(owner_name={0}, owner_abr={1}, id_kis={2})", owner_name, owner_abr, id_kis), servece_owner, eventID);
                return -1;
            }
        }
        /// <summary>
        /// Получить владельца по id_kis, если нет создать по данным КИС
        /// </summary>
        /// <param name="id_kis"></param>
        /// <param name="create"></param>
        /// <returns></returns>
        public ReferenceOwners GetReferenceOwnersOfKIS(int id_kis, bool create)
        {
            try
            {
                EFWagons ef_wag = new EFWagons();
                EFRW.Concrete.EFReference ef_ref = new EFRW.Concrete.EFReference();
                ReferenceOwners owner_rw = ef_ref.GetReferenceOwnersOfKIS(id_kis);
                if (owner_rw == null & create)
                {
                    KometaSobstvForNakl owner_kis = ef_wag.GetSobstvForNakl(id_kis);
                    if (owner_kis != null)
                    {
                        int res = CreateOwner(owner_kis.NPLAT, owner_kis.ABR, owner_kis.SOBSTV);
                        if (res > 0)
                        {
                            owner_rw = ef_ref.GetReferenceOwners(res);
                        }
                    }
                }
                return owner_rw;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetReferenceOwnersOfKIS(id_kis={0}, create={1})", servece_owner, id_kis, create), servece_owner, eventID);
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
                EFRW.Concrete.EFReference ef_ref = new EFRW.Concrete.EFReference();
                ReferenceOwnerCars owner_car = new ReferenceOwnerCars()
                {
                    id = 0,
                    num = num,
                    id_owner = id_owner,
                    start_lease = start_lease,
                    end_lease = end_lease,
                    id_arrival = id_arrival,
                    create_dt = DateTime.Now,
                    create_user = System.Environment.UserDomainName + @"\" + System.Environment.UserName,
                    change_dt = null,
                    change_user = null,
                };
                return ef_ref.SaveReferenceOwnerCars(owner_car);
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
                EFWagons ef_wag = new EFWagons();
                EFRW.Concrete.EFReference ef_ref = new EFRW.Concrete.EFReference();
                int result = 0;
                List<KometaVagonSob> list_owner = ef_wag.GetVagonsSob(num).ToList();
                // Проверим есть данные и признак использовать данные КИС - переносим данные в сисему RW
                if (list_owner != null && list_owner.Count() > 0 & this.reference_kis)
                {
                    foreach (KometaVagonSob kvs in list_owner)
                    {
                        // Определим владельца если нет создадим
                        ReferenceOwners owner_rw = GetReferenceOwnersOfKIS(kvs.SOB, true);
                        // Создать по данным кис
                        int res_roc = CreateOwnerCars(num, (owner_rw != null ? owner_rw.id : 0), kvs.DATE_AR, kvs.DATE_END, id_arrival);
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

        #region Справочник "Стран"
        /// <summary>
        /// Получить строку справочника "Стран" по коду, если нет создвать новую
        /// </summary>
        /// <param name="code"></param>
        /// <param name="create"></param>
        /// <returns></returns>
        protected ReferenceCountry GetReferenceCountry(EFReference.Entities.Countrys code, bool create)
        {
            try
            {
                if (code == null) return null;
                if (code.code <= 0) return null;
                EFRW.Concrete.EFReference ef_ref = new EFRW.Concrete.EFReference();
                ReferenceCountry ref_country = ef_ref.GetReferenceCountryOfCode(code.code);
                if (ref_country == null & create)
                {
                    ReferenceCountry new_rc = new ReferenceCountry()
                    {
                        id = 0,
                        country_ru = code.country,
                        country_en = code.country,
                        code = code.code,
                    };
                    int res = ef_ref.SaveReferenceCountry(new_rc);
                    if (res > 0)
                    {
                        ref_country = ef_ref.GetReferenceCountry(res);
                    }
                }
                return ref_country;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetReferenceCountry(code={0}, create={1})", code, create), servece_owner, eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить id страны из справочника "Стран" по коду, если нет создвать новую
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        protected int GetIDReferenceCountry(EFReference.Entities.Countrys code)
        {
            try
            {
                ReferenceCountry ref_country = GetReferenceCountry(code, true);
                return ref_country != null ? ref_country.id : 0;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetIDReferenceCountry(code={0})", code), servece_owner, eventID);
                return 0;
            }
        }
        /// <summary>
        /// Получить id строки справочника "Стран" системы RailWay по коду СНГ и стран балтии.
        /// </summary>
        /// <param name="code_country_sng"></param>
        /// <returns></returns>
        public int GetIDReferenceCountryOfCodeSNG(int code_country_sng)
        {
            try
            {
                EFReference.Concrete.EFReference ef_reference = new EFReference.Concrete.EFReference();
                EFReference.Entities.Countrys code = ef_reference.GetCountryOfCodeSNG(code_country_sng);
                return GetIDReferenceCountry(code);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetIDReferenceCountryOfCodeSNG(code_country_sng={0})", code_country_sng), servece_owner, eventID);
                return 0;
            }
        }
        /// <summary>
        /// Получить id строки справочника "Стран" системы RailWay по коду МеталлургТранса
        /// </summary>
        /// <param name="code_mt"></param>
        /// <returns></returns>
        public int GetIDReferenceCountryOfCodeMT(string code_mt)
        {
            try
            {
                int country = int.Parse(code_mt.Substring(0, 2));
                return GetIDReferenceCountryOfCodeSNG(country);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetIDReferenceCountryOfCodeMT(code_mt={0})", code_mt), servece_owner, eventID);
                return 0;
            }
        }

        public int GetIDReferenceCountryOfCodeISO(int code_country_iso)
        {
            try
            {
                EFReference.Concrete.EFReference ef_reference = new EFReference.Concrete.EFReference();
                EFReference.Entities.Countrys code = ef_reference.GetCountryOfCode(code_country_iso);
                return GetIDReferenceCountry(code);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetIDReferenceCountryOfCodeSNG(code_country_sng={0})", code_country_iso), servece_owner, eventID);
                return 0;
            }
        }

        #endregion

        #region Справочник "Грузов"
        /// <summary>
        /// Вернуть строку справочника "Грузов" по коду ЕТСНГ, если нет создать
        /// </summary>
        /// <param name="code_etsng"></param>
        /// <param name="create"></param>
        /// <returns></returns>
        public ReferenceCargo GetReferenceCargo(int code_etsng, bool create)
        {
            try
            {
                EFRW.Concrete.EFReference ef_ref = new EFRW.Concrete.EFReference();
                EFReference.Concrete.EFReference reference = new EFReference.Concrete.EFReference();
                ReferenceCargo ref_cargo = ef_ref.GetReferenceCargoOfCodeETSNG(code_etsng);
                if (ref_cargo == null)
                {
                    EFReference.Entities.Cargo cargo = reference.GetCargoOfCodeETSNG(code_etsng);
                    //EFReference.Entities.Cargo cargo = reference.GetCorrectCargo(code_etsng);
                    ReferenceCargo new_cargo;
                    if (cargo != null & create)
                    {
                        new_cargo = new ReferenceCargo()
                        {
                            id = 0,
                            name_ru = cargo.name_etsng.Length > 200 ? cargo.name_etsng.Remove(199).Trim() : cargo.name_etsng.Trim(),
                            name_en = cargo.name_etsng.Length > 200 ? cargo.name_etsng.Remove(199).Trim() : cargo.name_etsng.Trim(),
                            name_full_ru = cargo.name_etsng.Length > 500 ? cargo.name_etsng.Remove(499).Trim() : cargo.name_etsng.Trim(),
                            name_full_en = cargo.name_etsng.Length > 500 ? cargo.name_etsng.Remove(499).Trim() : cargo.name_etsng.Trim(),
                            etsng = code_etsng,
                            id_type = 0
                        };
                    }
                    else
                    {
                        new_cargo = new ReferenceCargo()
                        {
                            id = 0,
                            name_ru = "?",
                            name_en = "?",
                            name_full_ru = "?",
                            name_full_en = "?",
                            etsng = code_etsng,
                            id_type = 0
                        };
                    }
                    int res_cargo = ef_ref.SaveReferenceCargo(new_cargo);
                    if (res_cargo > 0)
                    {
                        ref_cargo = ef_ref.GetReferenceCargo(res_cargo);
                    }

                }
                return ref_cargo;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetReferenceCargo(code_etsng={0}, create={1})", code_etsng, create), servece_owner, eventID);
                return null;
            }
        }
        /// <summary>
        /// Вернуть id строки справочника "Грузов" по коду ЕТСНГ, если нет создать
        /// </summary>
        /// <param name="code_etsng"></param>
        /// <returns></returns>
        public int GetIDReferenceCargoOfCodeETSNG(int code_etsng)

        {
            try
            {
                ReferenceCargo ref_cargo = GetReferenceCargo(code_etsng, true);
                return ref_cargo != null ? ref_cargo.id : 0;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetIDReferenceCargoOfCodeETSNG(code_etsng={0})", code_etsng), servece_owner, eventID);
                return 0;
            }
        }
        public int GetIDReferenceCargoOfCorrectCodeETSNG(int code_etsng)
        {
            EFReference.Concrete.EFReference reference = new EFReference.Concrete.EFReference();
            return GetIDReferenceCargoOfCodeETSNG(reference.GetCodeCorrectCargo(code_etsng));
        }

        public int GetCorrectCodeETSNGOfKis(int? code_kis)
        {
            if (code_kis != null)
            {
                EFWagons kis = new EFWagons();
                PromGruzSP pg = kis.GetGruzSP((int)code_kis);
                if (pg != null && pg.TAR_GR != null)
                {
                    EFReference.Concrete.EFReference reference = new EFReference.Concrete.EFReference();
                    return reference.GetCodeCorrectCargo((int)pg.TAR_GR);
                }
            }
            return 0;
        }

        #endregion

        #region Справочник "Станций railway"
        /// <summary>
        /// Вернуть строку станции УЗ справочника "Станций railway" по индексу поезда МТ
        /// </summary>
        /// <param name="index_mt"></param>
        /// <param name="create"></param>
        /// <returns></returns>
        public Stations GetStationsUZ(string index_mt, bool create)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(index_mt)) return null;
                EFReference.Concrete.EFReference reference = new EFReference.Concrete.EFReference();
                int code = int.Parse(index_mt.Substring(9, 4));
                EFReference.Entities.Stations station_in = reference.GetStationsOfCode(code * 10);
                int codecs = station_in != null ? (int)station_in.code_cs : code * 10;
                return GetStationsUZ(codecs, true);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetStationsUZ(index_mt={0}, create={1})", index_mt, create), servece_owner, eventID);
                return null;
            }
        }
        /// <summary>
        /// Вернуть строку станции УЗ справочника "Станций railway" по коду уз
        /// </summary>
        /// <param name="code_uz"></param>
        /// <param name="create"></param>
        /// <returns></returns>
        public Stations GetStationsUZ(int code_uz, bool create)
        {
            try
            {
                EFRailWay ef_rw = new EFRailWay();
                EFReference.Concrete.EFReference reference = new EFReference.Concrete.EFReference();
                Stations station = ef_rw.GetStationsOfCodeUZ(code_uz);
                // Если нет станции создадим
                if (station == null & create)
                {
                    EFReference.Entities.Stations station_uz = reference.GetStationsOfCodeCS(code_uz);
                    station = new Stations()
                    {
                        id = 0,
                        name_ru = station_uz.station,
                        name_en = station_uz.station,
                        view = false,
                        exit_uz = false,
                        station_uz = true,
                        id_rs = null,
                        id_kis = null,
                        default_side = null,
                        code_uz = code_uz,
                    };
                    int res_st = ef_rw.SaveStations(station);
                    if (res_st > 0)
                    {
                        // Создадим путь для отправки на АМКР
                        Ways way_inp_amkr = new Ways()
                        {
                            id = 0,
                            id_station = res_st,
                            num = "1",
                            name_ru = "Путь для отправки на АМКР",
                            name_en = "The way to send to AMKR",
                            position = 1,
                            capacity = null,
                            id_car_status = 15,
                            tupik = null,
                            dissolution = null,
                            defrosting = null,
                            overturning = null,
                            pto = null,
                            cleaning = null,
                            rest = null,
                            id_rc = null,
                        };
                        ef_rw.SaveWays(way_inp_amkr);
                        // Создадим путь для принятия из АМКР
                        Ways way_out_amkr = new Ways()
                        {
                            id = 0,
                            id_station = res_st,
                            num = "2",
                            name_ru = "Путь для приема с АМКР",
                            name_en = "Path for reception with AMKR",
                            position = 1,
                            capacity = null,
                            id_car_status = null,
                            tupik = null,
                            dissolution = null,
                            defrosting = null,
                            overturning = null,
                            pto = null,
                            cleaning = null,
                            rest = null,
                            id_rc = null,
                        };
                        ef_rw.SaveWays(way_out_amkr);
                        station = ef_rw.GetStations(res_st);
                    }
                }
                return station;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetStationsUZ(code_uz={0}, create={1})", code_uz, create), servece_owner, eventID);
                return null;
            }
        }
        /// <summary>
        /// Вернуть строку станции справочника "Станций railway" по id kis, если нет создать по данным КИС
        /// </summary>
        /// <param name="id_kis"></param>
        /// <param name="create"></param>
        /// <returns></returns>
        public Stations GetStations(int id_kis, bool create)
        {
            try
            {
                EFRailWay ef_rw = new EFRailWay();
                EFWagons kis = new EFWagons();
                Stations station = ef_rw.GetStationsOfKis(id_kis);
                // Если нет станции создадим
                if (station == null & create)
                {
                    KometaStan st = kis.GetKometaStan(id_kis);
                    station = new Stations()
                    {
                        id = 0,
                        name_ru = st!=null ? st.NAME : "?",
                        name_en = st != null ? st.NAME : "?",
                        view = false,
                        exit_uz = false,
                        station_uz = st != null && st.MPS != null ? (bool)st.MPS : false,
                        id_rs = null,
                        id_kis = id_kis,
                        default_side = null,
                        code_uz = null,
                    };
                    int res_st = ef_rw.SaveStations(station);
                    station = ef_rw.GetStationsOfKis(res_st);
                }
                return station;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetStations(id_kis={0}, create={1})", id_kis, create), servece_owner, eventID);
                return null;
            }
        }
        /// <summary>
        /// Вернуть id станции справочника "Станций railway" по id kis, если нет создать по данным КИС 
        /// </summary>
        /// <param name="id_kis"></param>
        /// <returns></returns>
        public int GetIDStationsOfKIS(int? id_kis)
        {
            try
            {
                if (id_kis == null) return 0;
                Stations station = GetStations((int)id_kis, this.reference_kis == true ? true: false);
                return station != null ? station.id : 0;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetIDStationsOfKIS(id_kis={0})", id_kis), servece_owner, eventID);
                return 0;
            }
        }


        #endregion

        #region Справочник "Станций УЗ для отправки"
        /// <summary>
        /// Вернуть строку справочника "Станций УЗ для отправки" по коду (с контрольной суммой) станции, если нет создать
        /// </summary>
        /// <param name="codecs"></param>
        /// <param name="create"></param>
        /// <returns></returns>
        public ReferenceStation GetReferenceStation(int codecs, bool create)
        {
            try
            {
                EFRW.Concrete.EFReference ef_ref = new EFRW.Concrete.EFReference();
                EFReference.Concrete.EFReference reference = new EFReference.Concrete.EFReference();
                ReferenceStation ref_station = ef_ref.GetReferenceStationOfCodecs(codecs);
                if (ref_station == null)
                {
                    EFReference.Entities.Stations station = reference.GetStationsOfCodeCS(codecs);
                    ReferenceStation new_station;
                    if (station != null & create)
                    {
                        EFReference.Entities.InternalRailroad ir = reference.GetInternalRailroad(station.id_ir!= null ? (int)station.id_ir:0);
                        EFReference.Entities.States state = reference.GetStates(ir != null ? ir.id_state : 0);  
                        string internal_railroad = ir!=null ? ir.internal_railroad : "?";
                        string ir_abbr = ir != null ? ir.abbr : "?";
                        string name_network = state != null ? state.name_network : "?";
                        string nn_abbr = state != null ? state.abb_ru : "?";
                        string name = station.station + ", " + ir_abbr + ". ж/д. "+ nn_abbr;

                        new_station = new ReferenceStation()
                        {
                            id = 0,
                            name = name,
                            station = station.station,
                            internal_railroad = internal_railroad,
                            ir_abbr = ir_abbr,
                            name_network = name_network,
                            nn_abbr = nn_abbr,
                            code_cs = codecs,
                        };
                    }
                    else
                    {
                        new_station = new ReferenceStation()
                        {
                            id = 0,
                            name = "?",
                            station = "?",
                            internal_railroad = "?",
                            ir_abbr = "?",
                            name_network = "?",
                            nn_abbr = "?",
                            code_cs = codecs,
                        };
                    }
                    int res_station = ef_ref.SaveReferenceStation(new_station);
                    if (res_station > 0)
                    {
                        ref_station = ef_ref.GetReferenceStation(res_station);
                    }

                }
                return ref_station;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetReferenceStation(codecs={0}, create={1})", codecs, create), servece_owner, eventID);
                return null;
            }
        }
        /// <summary>
        /// Вернуть id строкb справочника "Станций УЗ для отправки" по коду (с контрольной суммой) станции, если нет создать
        /// </summary>
        /// <param name="codecs"></param>
        /// <returns></returns>
        public int GetIDGetReferenceStationOfCodecs(int codecs)
        {
            try
            {
                ReferenceStation ref_station = GetReferenceStation(codecs, true);
                return ref_station != null ? ref_station.id : 0;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetIDGetReferenceStationOfCodecs(codecs={0})", codecs), servece_owner, eventID);
                return 0;
            }
        }
        #endregion

        #region Справочник "Путей railway"
        /// <summary>
        /// Вернуть строку пути справочника "Пути railway" по id станции и номеру пути, если нет создать по данным КИС
        /// </summary>
        /// <param name="id_station"></param>
        /// <param name="num"></param>
        /// <param name="create"></param>
        /// <returns></returns>
        public Ways GetWay(int id_station, string num, bool create)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(num)) return null;
                EFRailWay ef_rw = new EFRailWay();
                EFWagons kis = new EFWagons();
                Ways way = ef_rw.GetWaysOfStation(id_station, num);
                // Если нет станции создадим
                if (way == null & create)
                {
                    // Создадим путь для отправки на АМКР
                    way = new Ways()
                    {
                        id = 0,
                        id_station = id_station,
                        num = num,
                        name_ru = "Путь создан по данным КИС",
                        name_en = "The path is based on the KIS data",
                        position = 0,
                        capacity = null,
                        id_car_status = null,
                        tupik = null,
                        dissolution = null,
                        defrosting = null,
                        overturning = null,
                        pto = null,
                        cleaning = null,
                        rest = null,
                        id_rc = null,
                    };
                    int res_w = ef_rw.SaveWays(way);
                    way = ef_rw.GetWays(res_w);
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
        public int GetIDWayOfStation(int id_station, string num) {
            try
            {
                Ways way = GetWay(id_station, num, this.reference_kis == true ? true : false);
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
        public int GetIDDefaultWayOfStation(int id_station, string num) {
            try
            {
                EFRailWay ef_rw = new EFRailWay();
                int id = GetIDWayOfStation(id_station, num);
                if (id == 0)
                {
                    Ways Way = ef_rw.GetWaysOfStation(id_station).OrderBy(w => w.num).FirstOrDefault();
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

        //public int? DefinitionIDWays(int id_station, int? num_way)
        //{
        //    EFRailCars efrc = new EFRailCars();
        //    if (num_way != null)
        //    {
        //        int? way = efrc.GetIDWaysToStations(id_station, ((int)num_way).ToString());
        //        if (way == null)
        //        {
        //            int res = efrc.SaveWAYS(new WAYS()
        //            {
        //                id_way = 0,
        //                id_stat = id_station,
        //                id_park = null,
        //                num = ((int)num_way).ToString(),
        //                name = "?",
        //                vag_capacity = null,
        //                order = null,
        //                bind_id_cond = null,
        //                for_rospusk = null,
        //            });
        //            if (res > 0) return res;
        //        }
        //        return way;
        //    }
        //    else
        //    {
        //        WAYS ws = efrc.GetWaysOfStations(id_station).OrderBy(w => w.num).FirstOrDefault();
        //        if (ws != null) return ws.id_way;
        //    }
        //    return null;
        //}
        #endregion

        #region Справочник "Цехов"
        /// <summary>
        /// Получить id цеха по id цеха системы кис, если нет создать по данным КИС
        /// </summary>
        /// <param name="id_kis"></param>
        /// <param name="create"></param>
        /// <returns></returns>
        public int? GetIDShopOfKis(int id_kis, bool create)
        {
            try
            {
                EFRailWay ef_rw = new EFRailWay();
                Shops shop = ef_rw.GetShopsOfKis(id_kis);
                if (shop == null & create & this.reference_kis)
                {
                    EFWagons kis = new EFWagons();
                    PromCex cex = kis.GetCex(id_kis);
                    if (cex != null)
                    {
                        Shops new_shop = new Shops()
                        {
                            id = 0,
                            id_station = null,
                            name_ru = cex.ABREV_P,
                            name_en = cex.ABREV_P,
                            name_full_ru = cex.NAME_P,
                            name_full_en = cex.NAME_P,
                            code_amkr = null,
                            id_kis = id_kis,
                            parent_id = null,
                        };
                        int res = ef_rw.SaveShops(new_shop);
                        if (res > 0)
                        {
                            shop = ef_rw.GetShops(res);
                        }
                    }
                }
                return shop != null ? (int?)shop.id : null;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetIDShopOfKis(id_kis={0}, create={1})", id_kis, create), servece_owner, eventID);
                return 0;
            }
        }
        #endregion        

        #region Справочник "Тупиков"
        /// <summary>
        /// Получить id тупика по id тупика системы кис, если нет создать по данным КИС
        /// </summary>
        /// <param name="id_kis"></param>
        /// <param name="create"></param>
        /// <returns></returns>
        public int? GetIDDeadlockOfKis(int id_kis, bool create)
        {
            try
            {
                EFRailWay ef_rw = new EFRailWay();
                Deadlock deadlock = ef_rw.GetDeadlockOfKis(id_kis);
                if (deadlock == null & create & this.reference_kis)
                {
                    EFWagons kis = new EFWagons();
                    NumVagStpr1Tupik typik = kis.GetNumVagStpr1Tupik(id_kis);
                    if (typik != null)
                    {
                        Deadlock new_deadlock = new Deadlock()
                        {
                            id = 0, 
                            name = typik.NAMETUPIK, 
                            description = typik.NAMETUPIK,
                            id_kis = typik.ID_CEX_TUPIK
 
                        };
                        int res = ef_rw.SaveDeadlock(new_deadlock);
                        if (res > 0)
                        {
                            deadlock = ef_rw.GetDeadlock(res);
                        }
                    }
                }
                return deadlock != null ? (int?)deadlock.id : null;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetIDDeadlockOfKis(id_kis={0}, create={1})", id_kis, create), servece_owner, eventID);
                return 0;
            }
        }
        #endregion  

        #region Справочник "Грузополучателей"

        public int GetIDReferenceConsigneeOfKis(int id_kis, bool create)
        {
            try
            {
                EFRW.Concrete.EFReference ef_ref = new EFRW.Concrete.EFReference();
                ReferenceConsignee consignee = ef_ref.GetReferenceConsigneeOfKis(id_kis);
                if (consignee == null & create & this.reference_kis)
                {
                    EFWagons kis = new EFWagons();
                    PromCex cex = kis.GetCex(id_kis);
                    if (cex != null)
                    {


                        ReferenceConsignee new_consignee = new ReferenceConsignee()
                        {
                            id = 0,
                            name_ru = cex.ABREV_P,
                            name_en = cex.ABREV_P,
                            name_full_ru = cex.NAME_P,
                            name_full_en = cex.NAME_P,
                            name_abr_ru = cex.ABREV_P,
                            name_abr_en = cex.ABREV_P,
                            id_shop = id_kis>=10 & id_kis<800 ? GetIDShopOfKis(id_kis, true) : null,
                            create_dt = DateTime.Now,
                            create_user = System.Environment.UserDomainName + @"\" + System.Environment.UserName,
                            change_dt = null,
                            change_user = null,
                            id_kis = id_kis,
                        };
                        int res = ef_ref.SaveReferenceConsignee(new_consignee);
                        if (res > 0)
                        {
                            consignee = ef_ref.GetReferenceConsignee(res);
                        }

                    }
                }
                return consignee != null ? consignee.id : 0;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetIDReferenceConsigneeOfKis(id_kis={0}, create={1})", id_kis, create), servece_owner, eventID);
                return 0;
            }
        }
        #endregion


    }
}
