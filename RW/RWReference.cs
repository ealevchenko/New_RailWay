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
        #endregion

        #region Справочник "Станций railway"
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
        #endregion

        #region Справочник "Путей railway"

        #endregion
    }
}
