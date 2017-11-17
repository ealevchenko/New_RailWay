using EFRC.Concrete;
using EFRC.Entities;
using MessageLog;
using RWWebAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCReferences
{
    public class RCReference
    {
        private eventID eventID = eventID.RCReference;
        
        public RCReference() { }

        #region Определение ID по справочникам
        ///// <summary>
        ///// Определить ID станции системы Railcars (если ID нет в системе создать по данным справочника KIS)
        ///// </summary>
        ///// <param name="id_station_kis"></param>
        ///// <param name="num_way"></param>
        ///// <returns>id станции системы Railcars</returns>
        //public int? DefinitionIDStations(int id_station_kis, int? num_way)
        //{
        //    int? stan = rs_stat.GetIDStationsOfKis(id_station_kis);
        //    if (stan == null)
        //    {
        //        if (num_way != null) { num_way = 0; }
        //        int outer_side = (int)num_way % 2; // 0-четн. 1-нечет.
        //        NumVagStan st = vc.GetStations(id_station_kis);
        //        if (st != null)
        //        {
        //            int res = rs_stat.SaveStations(new STATIONS()
        //            {
        //                id_stat = 0,
        //                name = st.NAME,
        //                id_ora = id_station_kis,
        //                outer_side = outer_side,
        //                is_uz = 0
        //            });
        //            if (res > 0) return res;
        //        }
        //    }
        //    return stan;
        //}
        ///// <summary>
        ///// Определить ID станции системы Railcars (если ID нет в системе создать по данным глобальных станций)
        ///// </summary>
        ///// <param name="code_cs"></param>
        ///// <returns></returns>
        //public int DefinitionIDStations(int code_cs)
        //{
        //    return reference.DefinitionIDStation(code_cs);
        //}
        ///// <summary>
        ///// Определить ID пути системы Railcars (если ID нет в системе создать путь)
        ///// </summary>
        ///// <param name="id_station_kis"></param>
        ///// <param name="num_way"></param>
        ///// <returns></returns>
        //public int? DefinitionIDWays(int id_station, int? num_way)
        //{
        //    if (num_way != null)
        //    {
        //        int? way = rs_ways.GetIDWaysToStations(id_station, ((int)num_way).ToString());
        //        if (way == null)
        //        {
        //            int res = rs_ways.SaveWays(new WAYS()
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
        //        WAYS ws = rs_ways.GetWaysOfStations(id_station).OrderBy(w => w.num).FirstOrDefault();
        //        if (ws != null) return ws.id_way;
        //    }
        //    return null;
        //}
        /// <summary>
        /// Получить ID вагона системы Railcars (если id нет создать из данных КИС или создать временную строку)
        /// </summary>
        /// <param name="num_vag"></param>
        /// <param name="dt"></param>
        /// <param name="train_number"></param>
        /// <param name="id_sostav"></param>
        /// <param name="natur"></param>
        /// <param name="transit"></param>
        /// <returns></returns>
        public int DefinitionSetIDVagon(int num_vag, DateTime dt, int train_number, int? id_sostav, int? natur, bool transit)
        {
            try
            {
                EFRailCars efrc = new EFRailCars();
                Wagons kis = new Wagons();
                int? id_vagons = efrc.GetIDVagons(num_vag, dt);
                if (id_vagons == null)
                {
                    id_vagons = efrc.GetIDNewVagons(num_vag, dt);
                    if (id_vagons == null)
                    {
                        KometaVagonSob kvs = kis.GetKometaVagonSob(num_vag, dt);
                        VAGONS wag;
                        if (kvs != null)
                        {
                            int? owner = DefinitionIDOwner(kvs.SOB, null); // Определим id владельца (системы railCars)                        
                            wag = new VAGONS()
                            {
                                id_vag = 0,
                                num = num_vag,
                                id_ora = null,
                                id_owner = owner,
                                id_stat = null,
                                is_locom = train_number,
                                locom_seria = null,
                                rod = kvs.ROD,
                                st_otpr = "-",
                                date_ar = kvs.DATE_AR,
                                date_end = kvs.DATE_END,
                                date_in = dt,
                                IDSostav = id_sostav,
                                Natur = natur,
                                Transit = transit
                            };
                        }
                        else
                        {
                            wag = new VAGONS()
                            {
                                id_vag = 0,
                                num = num_vag,
                                id_ora = null,
                                id_owner = null,
                                id_stat = null,
                                is_locom = train_number,
                                locom_seria = null,
                                rod = null,
                                st_otpr = "-",
                                date_ar = null,
                                date_end = null,
                                date_in = dt,
                                IDSostav = id_sostav,
                                Natur = natur,
                                Transit = transit
                            };
                        }
                        id_vagons = efrc.SaveVAGONS(wag); // Вернуть id или ошибку
                    }
                }
                return (int)id_vagons;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("DefinitionSetIDVagon(num_vag={0}, dt={1}, train_number={2}, id_sostav={3}, natur={4}, transit={5})", num_vag, dt, train_number, id_sostav, natur, transit), eventID);
                return -1;
            }
        }
        /// <summary>
        /// Определить Id владельца (если id нет в системе RailCars создать из данных КИС)
        /// </summary>
        /// <param name="id_sob_kis"></param>
        /// <param name="id_owner_country"></param>
        /// <returns></returns>
        public int? DefinitionIDOwner(int id_sob_kis, int? id_owner_country)
        {
            try
            {
                EFRailCars efrc = new EFRailCars();
                Wagons kis = new Wagons();
                int? id_own = efrc.GetIDOwnersOfKis(id_sob_kis);
                if (id_own == null)
                {

                    KometaSobstvForNakl sfn = kis.GetSobstvForNakl(id_sob_kis);
                    if (sfn != null)
                    {
                        int res = efrc.SaveOWNERS(new OWNERS()
                        {
                            id_owner = 0,
                            name = sfn.NPLAT,
                            abr = sfn.ABR,
                            id_country = id_owner_country,
                            id_ora = id_sob_kis,
                            id_ora_temp = null,
                        });
                        if (res > 0) { id_own = res; }
                    }
                }
                return id_own;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("DefinitionIDOwner(id_sob_kis={0}, id_owner_country={1})", id_sob_kis, id_owner_country), eventID);
                return null;
            }
        }
        /// <summary>
        /// Определить Id груза (если id нет в системе RailCars создать из данных КИС)
        /// </summary>
        /// <param name="id_gruz_prom_kis"></param>
        /// <param name="id_gruz_vag_kis"></param>
        /// <returns></returns>
        public int? DefinitionIDGruzs(int? id_gruz_prom_kis, int? id_gruz_vag_kis)
        {
            try
            {
                EFRailCars efrc = new EFRailCars();
                Wagons kis = new Wagons();
                int? id_gruz = efrc.GetIDGruzs(id_gruz_prom_kis, id_gruz_vag_kis);
                if (id_gruz == null)
                {
                    if (id_gruz_prom_kis != null & id_gruz_vag_kis == null)
                    {
                        PromGruzSP pg = kis.GetGruzSP((int)id_gruz_prom_kis);
                        if (pg != null)
                        {
                            int res = efrc.SaveGRUZS(new GRUZS()
                            {
                                id_gruz = 0,
                                name = pg.ABREV_GR.Trim(),
                                name_full = pg.NAME_GR.Trim(),
                                id_ora = id_gruz_prom_kis,
                                id_ora2 = null,
                                ETSNG = pg.TAR_GR,

                            });
                            if (res > 0) { id_gruz = res; }
                        }
                    }
                    // отправляемые грузы
                    if (id_gruz_vag_kis != null & id_gruz_prom_kis == null)
                    {
                        NumVagStpr1Gr nvgr = kis.GetSTPR1GR((int)id_gruz_vag_kis);
                        if (nvgr != null)
                        {
                            int res = efrc.SaveGRUZS(new GRUZS()
                            {
                                id_gruz = 0,
                                name = nvgr.GR.Trim(),
                                name_full = nvgr.GR.Trim(),
                                id_ora = null,
                                id_ora2 = id_gruz_vag_kis,
                                ETSNG = null
                            });
                            if (res > 0) { id_gruz = res; }
                        }
                    }
                }
                return id_gruz;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("DefinitionIDGruzs(id_gruz_prom_kis={0}, id_gruz_vag_kis={1})", id_gruz_prom_kis, id_gruz_vag_kis), eventID);
                return null;
            }
        }
        /// <summary>
        /// Определить Id груза по коду ЕТСНГ (если id нет в системе RailCars создать из данных КИС или справочника ЕТСНГ)
        /// </summary>
        /// <param name="id_cargo"></param>
        /// <returns></returns>
        public int? DefinitionIDGruzs(int id_cargo)
        {
            try
            {
                EFRailCars efrc = new EFRailCars();
                Wagons kis = new Wagons();
                Reference api_reference = new Reference();
                int? id_gr = efrc.GetIDGruzsToETSNG(id_cargo);
                if (id_gr == null)
                {
                    PromGruzSP pg;
                    pg = kis.GetGruzSPToTarGR(id_cargo, false);
                    if (pg == null)
                    {
                        pg = kis.GetGruzSPToTarGR(id_cargo, true);
                        if (pg == null)
                        {
                            Cargo cargo = api_reference.GetCargoOfCodeETSNG(id_cargo);
                            //Code_Cargo cargo = refRailway.GetCargos_ETSNG(id_cargo);
                            if (cargo != null)
                            {
                                int res = efrc.SaveGRUZS(new GRUZS()
                                {
                                    id_gruz = 0,
                                    name = cargo.name_etsng.Length > 200 ? cargo.name_etsng.Remove(199).Trim() : cargo.name_etsng.Trim(),
                                    name_full = cargo.name_etsng.Length > 500 ? cargo.name_etsng.Remove(499).Trim() : cargo.name_etsng.Trim(),
                                    id_ora = null,
                                    id_ora2 = null,
                                    ETSNG = cargo.code_etsng,

                                });
                                if (res > 0) { id_gr = res; }
                            }
                            else { id_gr = 0; }

                        }
                        else { return DefinitionIDGruzs(pg.K_GRUZ, null); }
                    }
                    else { return DefinitionIDGruzs(pg.K_GRUZ, null); }

                }
                return id_gr;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("DefinitionIDGruzs(id_cargo={0})", id_cargo), eventID);
                return null;
            }
        }
        ///// <summary>
        ///// Определить id цеха (если id нет в системе RailCars создать из данных КИС)
        ///// </summary>
        ///// <param name="id_shop_kis"></param>
        ///// <returns></returns>
        //public int? DefinitionIDShop(int id_shop_kis)
        //{
        //    int? id_shop = rs_shp.GetIDShopsOfKis(id_shop_kis);
        //    if (id_shop == null)
        //    {
        //        PromCex cex = pc.GetCex(id_shop_kis);
        //        if (cex != null)
        //        {
        //            int res = rs_shp.SaveShop(new SHOPS()
        //            {
        //                id_shop = 0,
        //                name = cex.ABREV_P,
        //                name_full = cex.NAME_P,
        //                id_stat = null,
        //                id_ora = id_shop_kis
        //            });
        //            if (res > 0) { id_shop = res; }
        //        }
        //    }
        //    return id_shop;
        //}
        ///// <summary>
        ///// Определить id тупика (если id нет в системе RailCars создать из данных КИС)
        ///// </summary>
        ///// <param name="id_tupik_kis"></param>
        ///// <returns></returns>
        //public int? DefinitionIDTupiki(int? id_tupik_kis)
        //{
        //    if (id_tupik_kis == null) return null;
        //    int? id_tupik = rs_tp.GetIDTupikOfKis((int)id_tupik_kis);
        //    if (id_tupik == null)
        //    {
        //        NumVagStpr1Tupik tupik = vc.GetStpr1Tupik((int)id_tupik_kis);
        //        if (tupik != null)
        //        {
        //            int res = rs_tp.SaveTUPIKI(new TUPIKI()
        //            {
        //                id_tupik = 0,
        //                id_ora = id_tupik_kis,
        //                name = tupik.NAMETUPIK
        //            });
        //            if (res > 0) { id_tupik = res; }
        //        }
        //        else return null;
        //    }
        //    return id_tupik;
        //}
        ///// <summary>
        ///// Получить id справочника из кода страны (европейский стандарт)
        ///// </summary>
        ///// <param name="id_code_europe"></param>
        ///// <returns></returns>
        //public int? DefinitionIDContries(int id_code_europe)
        //{
        //    if (id_code_europe <= 0) return 0;
        //    NumVagStran str = vc.GetStranOfCodeEurope(id_code_europe);
        //    if (str == null) return null; //TODO: Можно доработать сделав подбор кода (европ от isa отличается на 1 цифру больше, убирать последнюю цифру и пробовать как код iso)  
        //    return reference.DefinitionIDCountryCode(str.KOD_STRAN);
        //}

        /// <summary>
        /// Получить id груза по коду ЕТ СНГ 
        /// </summary>
        /// <param name="id_cargo"></param>
        /// <returns></returns>
        public int DefinitionIDCargo(int id_cargo)
        {
            EFRCReference efrc_reference = new EFRCReference();
            Reference api_reference = new Reference();
            EFRC.Entities.ReferenceCargo ref_cargo = efrc_reference.GetReferenceCargoOfCodeETSNG(id_cargo);
            if (ref_cargo == null)
            {
                Cargo cargo = api_reference.GetCargoOfCodeETSNG(id_cargo);
                EFRC.Entities.ReferenceCargo new_cargo = new EFRC.Entities.ReferenceCargo()
                {
                    IDCargo = 0,
                    Name = cargo.name_etsng.Length > 200 ? cargo.name_etsng.Remove(199).Trim() : cargo.name_etsng.Trim(),
                    NameFull = cargo.name_etsng.Length > 500 ? cargo.name_etsng.Remove(499).Trim() : cargo.name_etsng.Trim(),
                    ETSNG = id_cargo,
                    TypeCargo = 0,
                    DateTime = DateTime.Now
                };
                int res = efrc_reference.SaveReferenceCargo(new_cargo);
                if (res > 0) { return res; }
                else return 0;
            }
            return ref_cargo.IDCargo;
        }
        #endregion

        #region Синхронизация справочников
        ///// <summary>
        ///// Синхронизировать справочник вагонов (тип, владелец, аренда, страна владельца)
        ///// </summary>
        ///// <param name="day"></param>
        ///// <returns></returns>
        //public int SynchronizeWagons(int day)
        //{
        //    string mess_synchronize = String.Format("синхронизации справочника системы RailWay:Wagons, за {0} дней", day);
        //    string mess_synchronize_exec = "Выполнение " + mess_synchronize;
        //    string mess_synchronize_error = "выполнения " + mess_synchronize;
        //    string mess_synchronize_error1 = "Ошибка выполнения " + mess_synchronize;
        //    try
        //    {
        //        List<KometaVagonSob> list_shange = kc.GetChangeVagonsSob(day).ToList();
        //        if (list_shange.Count() == 0) return 0;
        //        int updates = 0;
        //        int errors = 0;
        //        int skippeds = 0;
        //        foreach (KometaVagonSob kvs in list_shange)
        //        {
        //            int? owner = DefinitionIDOwner(kvs.SOB, null); // Определим id владельца (системы railCars) 
        //            VAGONS wag_new = null;
        //            VAGONS wag = rs_vag.GetVagons(kvs.N_VAGON, kvs.DATE_AR);
        //            if (wag == null)
        //            {
        //                wag = rs_vag.GetNewVagons(kvs.N_VAGON, kvs.DATE_AR);
        //                if (wag == null)
        //                {
        //                    // создадим новую строку с этой арендой                        
        //                    wag_new = new VAGONS()
        //                    {
        //                        id_vag = 0,
        //                        num = kvs.N_VAGON,
        //                        id_ora = null,
        //                        id_owner = owner,
        //                        id_stat = null,
        //                        is_locom = null,
        //                        locom_seria = null,
        //                        rod = kvs.ROD,
        //                        st_otpr = "-",
        //                        date_ar = kvs.DATE_AR,
        //                        date_end = kvs.DATE_END,
        //                        date_in = null,
        //                        IDSostav = null,
        //                        Natur = null,
        //                        Transit = false,
        //                    };
        //                }
        //                else
        //                {
        //                    // обновим аренду на вновь сосзданом
        //                    wag_new = new VAGONS()
        //                    {
        //                        id_vag = wag.id_vag,
        //                        num = wag.num,
        //                        id_ora = wag.id_ora,
        //                        id_owner = owner,
        //                        id_stat = wag.id_stat,
        //                        is_locom = wag.is_locom,
        //                        locom_seria = wag.locom_seria,
        //                        rod = kvs.ROD,
        //                        st_otpr = wag.st_otpr,
        //                        date_ar = kvs.DATE_AR,
        //                        date_end = kvs.DATE_END,
        //                        date_in = wag.date_in,
        //                        IDSostav = wag.IDSostav,
        //                        Natur = wag.Natur,
        //                        Transit = wag.Transit,
        //                    };
        //                }
        //            }
        //            else
        //            {
        //                if (wag.date_ar < kvs.DATE_AR | wag.id_owner != owner)
        //                {
        //                    // создадим новую строку с этой арендой старую закроем
        //                    wag_new = new VAGONS()
        //                    {
        //                        id_vag = 0,
        //                        num = kvs.N_VAGON,
        //                        id_ora = null,
        //                        id_owner = owner,
        //                        id_stat = null,
        //                        is_locom = null,
        //                        locom_seria = null,
        //                        rod = kvs.ROD,
        //                        st_otpr = "-",
        //                        date_ar = kvs.DATE_AR,
        //                        date_end = kvs.DATE_END,
        //                        date_in = null,
        //                        IDSostav = null,
        //                        Natur = null,
        //                        Transit = false,
        //                    };
        //                    if (wag.date_end == null)
        //                    {
        //                        wag.date_end = kvs.DATE_AR.AddMinutes(-1);
        //                        rs_vag.SaveVAGONS(wag);
        //                    }
        //                }
        //                else skippeds++;
        //            }
        //            if (wag_new != null)
        //            {

        //                int res = rs_vag.SaveVAGONS(wag_new);
        //                if (res > 0) updates++;
        //                if (res < 0) errors++;
        //            }
        //        }
        //        mess_synchronize_exec += String.Format(", определено для синхронизации:{0} строк, синхронизировано: {1}, пропущено: {2}, ошибок синхронизации: {3}.", list_shange.Count(), updates, skippeds, errors);
        //        ServicesEventLog.LogWarning(mess_synchronize_exec, eventID);
        //        if (list_shange.Count() > 0 & list_shange.Count() > skippeds) { mess_synchronize_exec.SaveLogEvents(errors > 0 ? EventStatus.Error : EventStatus.Ok, eventID); }
        //        return updates;
        //    }
        //    catch (Exception e)
        //    {
        //        ServicesEventLog.LogError(e, mess_synchronize_error, eventID);
        //        return -1;
        //    }
        //}
        #endregion
    }
}
