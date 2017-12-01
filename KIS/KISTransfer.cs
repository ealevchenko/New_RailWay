using EFKIS.Concrete;
using EFKIS.Entities;
using MessageLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libClass;
using System.Globalization;
using RCReferences;

namespace KIS
{
    public enum errorTransfer : int
    {
        global = -1,
        no_stations = -2,
        no_ways = -3,
        no_wagons = -4,
        no_owner_country = -5,
        no_owner = -6,
        no_shop = -7,
        no_gruz = -8,
        no_wagon_is_list = -9,
        no_wagon_is_nathist = -10,
        no_godn = -11,
        no_del_output = -12,
        no_tupik = -13,
        no_station_nazn = -14,
        no_del_input = -15
    }

    public enum statusSting : int { Normal = 0, Delete = 1, Insert = 2, Update = 3 }

    public class KISTransfer
    {
        private eventID eventID = eventID.KIS_KISTransfer;
        protected service servece_owner = service.Null;

        public KISTransfer()
        {

        }

        public KISTransfer(service servece_owner)
        {
            this.servece_owner = servece_owner;
        }

        #region Таблица переноса составов из КИС [ArrivalSostav]
        /// <summary>
        /// Сохранить состав из КИС
        /// </summary>
        /// <param name="ps"></param>
        /// <returns></returns>
        protected int SaveArrivalSostav(PromSostav ps, statusSting status)
        {
            EFTKIS ef_tkis = new EFTKIS();
            try
            {
                DateTime DT = DateTime.Parse(ps.D_DD.ToString() + "-" + ps.D_MM.ToString() + "-" + ps.D_YY.ToString() + " " + ps.T_HH.ToString() + ":" + ps.T_MI.ToString() + ":00", CultureInfo.CreateSpecificCulture("ru-RU"));
                return ef_tkis.SaveArrivalSostav(new ArrivalSostav()
                {
                    id = 0,
                    datetime = DT,
                    day = (int)ps.D_DD,
                    month = (int)ps.D_MM,
                    year = (int)ps.D_YY,
                    hour = (int)ps.T_HH,
                    minute = (int)ps.T_MI,
                    natur = ps.N_NATUR,
                    id_station_kis = (int)ps.K_ST,
                    way_num = ps.N_PUT,
                    napr = ps.NAPR,
                    count_wagons = null,
                    count_nathist = null,
                    count_set_wagons = null,
                    count_set_nathist = null,
                    close = null,
                    close_user = null,
                    status = (int)status,
                    list_wagons = null,
                    list_no_set_wagons = null,
                    list_no_update_wagons = null,
                    message = null,
                });
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveArrivalSostav(ps={0}, status={1})", ps.GetFieldsAndValue(), status), servece_owner, eventID);
                return -1;
            }
        }
        /// <summary>
        /// Найти и удалить из списка ArrivalSostav елемент natur
        /// </summary>
        /// <param name="list"></param>
        /// <param name="natur"></param>
        /// <returns></returns>
        protected bool DelExistArrivalSostav(ref List<ArrivalSostav> list, int natur)
        {
            bool Result = false;
            try
            {
                int index = list.Count() - 1;
                while (index >= 0)
                {
                    if (list[index].natur == natur)
                    {
                        list.RemoveAt(index);
                        Result = true;
                    }
                    index--;
                }
                return Result;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("DelExistArrivalSostav(list={0}, natur={1})", list, natur), servece_owner, eventID);
            }
            return Result;
        }
        /// <summary>
        /// Проверяет списки PromSostav и ArrivalSostav на повторяющие натурные листы, оставляет в списке PromSostav - добавленные составы, ArrivalSostav - удаленные из КИС составы
        /// </summary>
        /// <param name="list_ps"></param>
        /// <param name="list_as"></param>
        protected void DelExistArrivalSostav(ref List<PromSostav> list_ps, ref List<ArrivalSostav> list_as)
        {
            try
            {
                int index = list_ps.Count() - 1;
                while (index >= 0)
                {
                    if (DelExistArrivalSostav(ref list_as, list_ps[index].N_NATUR))
                    {
                        list_ps.RemoveAt(index);
                    }
                    index--;
                }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("DelExistArrivalSostav(list_ps={0}, list_as={1})", list_ps, list_as), servece_owner, eventID);
            }
        }
        /// <summary>
        /// Удалить ранее перенесеные составы
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        protected int DeleteArrivalSostav(List<ArrivalSostav> list)
        {
            EFTKIS ef_tkis = new EFTKIS();
            try
            {
                if (list == null | list.Count == 0) return 0;
                int delete = 0;
                int errors = 0;
                foreach (ArrivalSostav or_as in list)
                {
                    // Удалим вагоны из системы RailCars
                    // TODO: Сделать код удаления вагонов из RailWay
                    //transfer_rc.DeleteVagonsToNaturList(or_as.NaturNum, or_as.DateTime);
                    or_as.close = DateTime.Now;
                    or_as.close_user = System.Environment.UserDomainName + @"\" + System.Environment.UserName;
                    or_as.status = (int)statusSting.Delete;
                    int res = ef_tkis.SaveArrivalSostav(or_as);
                    if (res > 0) delete++;
                    if (res < 0)
                    {
                        String.Format("Ошибка выполнения метода DeleteArrivalSostav, удаление строки:{0} из таблицы состояния переноса составов зашедших на АМКР по данным системы КИС", or_as.id).WriteError(servece_owner, this.eventID);
                        errors++;
                    }
                }
                String.Format("Таблица состояния переноса составов зашедших на АМКР по данным системы КИС, определенно удаленных в системе КИС {0} составов, удалено из таблицы {1}, ошибок удаления {2}.", list.Count(), delete, errors).WriteWarning(servece_owner, this.eventID);
                return delete;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("DeleteArrivalSostav(list={0})", list), servece_owner, eventID);
                return -1;
            }
        }
        /// <summary>
        /// Добавить новые составы появившиеся после переноса
        /// </summary>
        /// <param name="list"></param>
        protected int InsertArrivalSostav(List<PromSostav> list)
        {
            try
            {
                if (list == null | list.Count == 0) return 0;
                int insers = 0;
                int errors = 0;
                foreach (PromSostav ps in list)
                {
                    int res = SaveArrivalSostav(ps, statusSting.Insert);
                    if (res > 0) insers++;
                    if (res < 0)
                    {
                        String.Format("Ошибка выполнения метода InsertArrivalSostav, добавления строки состава по данным системы КИС(натурный лист:{0}, дата:{1}-{2}-{3} {4}:{5}) в таблицу состояния переноса составов зашедших на АМКР по данным системы КИС", ps.N_NATUR, ps.D_DD, ps.D_MM, ps.D_YY, ps.T_HH, ps.T_MI).WriteError(servece_owner, this.eventID);
                        errors++;
                    }
                }
                String.Format("Таблица состояния переноса составов зашедших на АМКР по данным системы КИС, определенно добавленных в системе КИС {0} составов, добавлено в таблицу {1}, ошибок добавления {2}.", list.Count(), insers, errors).WriteWarning(servece_owner, this.eventID);
                return insers;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("InsertArrivalSostav(list={0})", list), servece_owner, eventID);
                return -1;
            }
        }
        #endregion

        #region Операции с списками номеров вагонов
        /// <summary>
        /// Пренадлежит указаный вагон списку вагонов
        /// </summary>
        /// <param name="num"></param>
        /// <param name="wagons"></param>
        /// <returns></returns>
        protected bool IsWagonToList(int num, int[] wagons)
        {
            if (wagons == null) return false;
            foreach (int wnum in wagons)
            {
                if (num == wnum) return true;
            }
            return false;
        }
        /// <summary>
        /// Найти и удалить номер из списка
        /// </summary>
        /// <param name="num"></param>
        /// <param name="wagons"></param>
        /// <returns></returns>
        protected bool DelWagonToList(int num, ref List<int> wagons)
        {
            int index = wagons.Count() - 1;
            while (index >= 0)
            {
                if (wagons[index] == num)
                {
                    wagons.RemoveAt(index);
                    return true;
                }
                index--;
            }
            return false;
        }

        /// <summary>
        /// Преобразовать строку список вагонов в масив номеров вагонов типа int
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        protected int[] GetWagonsToInt(string list)
        {
            if (list == null) return null;
            string[] wagons = !String.IsNullOrWhiteSpace(list) ? list.Split(';') : null;
            List<int> ints = new List<int>();
            foreach (string st in wagons)
            {
                if (!String.IsNullOrWhiteSpace(st))
                {
                    ints.Add(int.Parse(st));
                }
            }
            return ints.ToArray();
        }

        protected List<int> GetWagonsToListInt(string list)
        {
            int[] ints = GetWagonsToInt(list);
            if (ints != null) return ints.ToList();
            return null;
        }
        /// <summary>
        /// Преобразовать List<PromVagon> в список номеров вагонов типа string
        /// </summary>
        /// <param name="list_pv"></param>
        /// <returns></returns>
        protected string GetWagonsToString(List<PromVagon> list_pv)
        {
            if (list_pv == null | list_pv.Count() == 0) return null;
            string res = null;
            foreach (PromVagon pv in list_pv)
            {
                res += pv.N_VAG.ToString() + ";";
            }
            return res;
        }
        /// <summary>
        /// Преобразовать List<PromNatHist> в список номеров вагонов типа string
        /// </summary>
        /// <param name="list_nh"></param>
        /// <returns></returns>
        protected string GetWagonsToString(List<PromNatHist> list_nh)
        {
            if (list_nh == null | list_nh.Count() == 0) return null;
            string res = null;
            foreach (PromNatHist pv in list_nh)
            {
                res += pv.N_VAG.ToString() + ";";
            }
            return res;
        }
        /// <summary>
        /// Преобразовать List<int> в список номеров вагонов типа string
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        protected string GetWagonsToString(List<int> list)
        {
            if (list == null) return null;
            string res = null;
            foreach (int pv in list)
            {
                res += pv.ToString() + ";";
            }
            return res;
        }
        /// <summary>
        /// Преобразовать List<PromVagon> в масив номеров вагонов типа int
        /// </summary>
        /// <param name="list_pv"></param>
        /// <returns></returns>
        protected int[] GetWagonsToInt(List<PromVagon> list_pv)
        {
            if (list_pv == null | list_pv.Count() == 0) return null;
            return GetWagonsToInt(GetWagonsToString(list_pv));
        }

        protected bool DeleteExistWagon(ref List<int> list, int wag)
        {
            if (list == null) return false;
            bool Result = false;
            int index = list.Count() - 1;
            while (index >= 0)
            {
                if (list[index] == wag)
                {
                    list.RemoveAt(index);
                    Result = true;
                }
                index--;
            }
            return Result;
        }

        protected void DeleteExistWagon(ref List<int> list_new, ref List<int> list_old)
        {
            if (list_new == null & list_old == null) return;
            int index = list_new.Count() - 1;
            while (index >= 0)
            {
                if (DeleteExistWagon(ref list_old, list_new[index]))
                {
                    list_new.RemoveAt(index);
                }
                index--;
            }
        }

        #endregion

        #region ПЕРЕНОС И ОБНАВЛЕНИЕ ВАГОНОВ ИЗ СИСТЕМЫ КИС в RailWay

        #region Операции с таблицей переноса составов из КИС [ArrivalSostav]
        /// <summary>
        /// Перенос информации о составах защедших на АМКР по системе КИС (с проверкой на изменение натуральных листов)
        /// </summary>
        /// <returns></returns>
        public int CopyArrivalKISToRailWay(int day_control_add_natur)
        {
            EFTKIS ef_tkis = new EFTKIS();
            EFWagons ef_wag = new EFWagons();

            int errors = 0;
            int normals = 0;
            // список новых составов в системе КИС
            List<PromSostav> list_newsostav = new List<PromSostav>();
            // список уже перенесенных в RailWay составов в системе КИС (с учетом периода контроля dayControllingAddNatur)
            List<PromSostav> list_oldsostav = new List<PromSostav>();
            // список уже перенесенных в RailWay составов (с учетом периода контроля dayControllingAddNatur)
            List<ArrivalSostav> list_arrivalsostav = new List<ArrivalSostav>();
            try
            {
                // Считаем дату последненго состава
                DateTime? lastDT = ef_tkis.GetLastDateTime();
                if (lastDT != null)
                {
                    // Данные есть получим новые
                    list_newsostav = ef_wag.GetInputPromSostav(((DateTime)lastDT).AddSeconds(1), DateTime.Now, false).ToList();
                    list_oldsostav = ef_wag.GetInputPromSostav(((DateTime)lastDT).AddDays(day_control_add_natur * -1), ((DateTime)lastDT).AddSeconds(1), false).ToList();
                    list_arrivalsostav = ef_tkis.GetArrivalSostav(((DateTime)lastDT).AddDays(day_control_add_natur * -1), ((DateTime)lastDT).AddSeconds(1)).ToList();
                }
                else
                {
                    // Таблица пуста получим первый раз
                    list_newsostav = ef_wag.GetInputPromSostav(DateTime.Now.AddDays(day_control_add_natur * -1), DateTime.Now, false).ToList();
                }
                // Переносим информацию по новым составам
                if (list_newsostav.Count() > 0)
                {
                    foreach (PromSostav ps in list_newsostav)
                    {

                        int res = SaveArrivalSostav(ps, statusSting.Normal);
                        if (res > 0) normals++;
                        if (res < 0) { errors++; }
                    }
                    string mess_new = String.Format("Таблица состояния переноса составов зашедших на АМКР по данным системы КИС (определено новых составов:{0}, перенесено:{1}, ошибок переноса:{2}).", list_newsostav.Count(), normals, errors);
                    mess_new.WriteInformation(servece_owner, this.eventID);
                    if (list_newsostav.Count() > 0) mess_new.WriteEvents(errors > 0 ? EventStatus.Error : EventStatus.Ok, servece_owner, eventID);
                }
                // Обновим информацию по составам которые были перенесены
                if (list_oldsostav.Count() > 0 & list_arrivalsostav.Count() > 0)
                {
                    List<PromSostav> list_ps = new List<PromSostav>();
                    list_ps = list_oldsostav;
                    List<ArrivalSostav> list_as = new List<ArrivalSostav>();
                    list_as = list_arrivalsostav.Where(a => a.status != (int)statusSting.Delete).ToList();
                    DelExistArrivalSostav(ref list_ps, ref list_as);
                    int ins = InsertArrivalSostav(list_ps);
                    int del = DeleteArrivalSostav(list_as);
                    string mess_upd = String.Format("Таблица состояния переноса составов зашедших на АМКР по данным системы КИС (определено добавленных составов:{0}, перенесено:{1}, определено удаленных составов:{2}, удалено:{3}).",
                    list_ps.Count(), ins, list_as.Count(), del);
                    mess_upd.WriteInformation(servece_owner, this.eventID);
                    if (list_ps.Count() > 0 | list_as.Count() > 0) mess_upd.WriteEvents(EventStatus.Ok, servece_owner, eventID);
                    normals += ins;
                }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CopyArrivalKISToRailWay(day_control_add_natur={0})", day_control_add_natur), servece_owner, eventID);
                return -1;
            }
            return normals;
        }
        /// <summary>
        /// Получить или обновить общий список вагонов, список не поставленных и не обнавленных вагонов
        /// </summary>
        /// <param name="orc_sostav"></param>
        public int SetListWagon(ref ArrivalSostav orc_sostav, List<PromVagon> list_pv, List<PromNatHist> list_nh)
        {
            EFTKIS ef_tkis = new EFTKIS();
            if (list_pv.Count() == 0 & list_nh.Count() == 0) return 0; // Списков вагонов нет
            try
            {

                //Создать и список вагонов заново и поставить их на путь ( or_as.CountWagons, or_as.ListWagons )
                List<int> old_wagons = GetWagonsToListInt(orc_sostav.list_wagons);
                if (list_pv.Count() > 0)
                { orc_sostav.list_wagons = GetWagonsToString(list_pv); }
                else { orc_sostav.list_wagons = GetWagonsToString(list_nh); }
                List<int> new_wagons = GetWagonsToListInt(orc_sostav.list_wagons);

                List<int> wagons_no_set = GetWagonsToListInt(orc_sostav.list_no_set_wagons);
                List<int> wagons_no_upd = GetWagonsToListInt(orc_sostav.list_no_update_wagons);
                List<int> wagons_buf = new List<int>();
                List<int> wagons_no_set_buf = new List<int>();
                List<int> wagons_no_upd_buf = new List<int>();
                // Удалить вагоны не найденные в новом списке из списка непоставленных на станцию вагонов 
                if (wagons_no_set != null)
                {
                    wagons_buf = GetWagonsToListInt(orc_sostav.list_wagons);
                    wagons_no_set_buf = GetWagonsToListInt(orc_sostav.list_no_set_wagons);
                    DeleteExistWagon(ref wagons_buf, ref wagons_no_set_buf);
                    foreach (int wag in wagons_no_set_buf)
                    {
                        DeleteExistWagon(ref wagons_no_set, wag);
                    }
                }
                // Удалить вагоны не найденные в новом списке из списка необновленных вагонов 
                if (wagons_no_upd != null)
                {
                    wagons_buf = GetWagonsToListInt(orc_sostav.list_wagons);
                    wagons_no_upd_buf = GetWagonsToListInt(orc_sostav.list_no_update_wagons);
                    DeleteExistWagon(ref wagons_buf, ref wagons_no_upd_buf);
                    foreach (int wag in wagons_no_upd_buf)
                    {
                        DeleteExistWagon(ref wagons_no_upd, wag);
                    }
                }
                // сформировать строчные списки не поставленных и не обнавленных вагонов
                orc_sostav.list_no_set_wagons = GetWagonsToString(wagons_no_set);
                orc_sostav.list_no_update_wagons = GetWagonsToString(wagons_no_upd);
                // Добавить в списки не поставленных и не обнавленных вагонов новые вагоны из нового списка
                DeleteExistWagon(ref new_wagons, ref old_wagons);
                foreach (int wag in new_wagons)
                {
                    if (wagons_no_set != null)
                    { orc_sostav.list_no_set_wagons += wag.ToString() + ";"; }
                    if (wagons_no_upd != null)
                    { orc_sostav.list_no_update_wagons += wag.ToString() + ";"; }
                }

                if (list_pv.Count() > 0)
                { orc_sostav.count_wagons = list_pv.Count(); }
                else { orc_sostav.count_wagons = list_nh.Count(); }
                // Сохранить и вернуть результат
                return ef_tkis.SaveArrivalSostav(orc_sostav);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SetListWagon(orc_sostav={0}, list_pv={1}, list_pv={2})", orc_sostav.GetFieldsAndValue(), list_pv, list_nh), servece_owner, eventID);
                return (int)errorTransfer.global;
            }
        }
        #endregion

        /// <summary>
        /// Поставить все составы прибывшие на АМКР по системе КИС (перечень составов берется из таблицы учета прибытия составов на АМКР системы RailWay)
        /// </summary>
        /// <returns></returns>
        public int TransferSostavKisTorailWay()
        {
            EFTKIS ef_tkis = new EFTKIS();
            int close = 0;
            IQueryable<ArrivalSostav> list_noClose = ef_tkis.GetArrivalSostavNoClose();
            if (list_noClose == null | list_noClose.Count() == 0) return 0;
            foreach (ArrivalSostav or_as in list_noClose.ToList())
            {
                try
                {
                    string mess_put = String.Format("Состав (натурный лист: {0}, дата: {1}, ID строки: {2}) прибывший на АМКР по данным системы КИС", or_as.natur, or_as.datetime, or_as.id_station_kis);
                    ArrivalSostav kis_sostav = new ArrivalSostav();
                    kis_sostav = or_as;
                    // Поставим состав на станции АМКР системы RailCars


                    int res_put = PutCarsToStation(ref kis_sostav);
                    // Обновление составов на станции АМКР системы RailCars
                    //int res_upd = rc_trans.UpdateCarsToStation(ref kis_sostav);
                    //TODO: ВЫПОЛНИТЬ КОД: Поставим состав на станции АМКР системы RailWay         
                    //.............................

                    //Закрыть состав
                    if (kis_sostav.count_wagons != null & kis_sostav.count_nathist != null & kis_sostav.count_set_wagons != null & kis_sostav.count_set_nathist != null
                        & kis_sostav.count_wagons == kis_sostav.count_nathist & kis_sostav.count_wagons == kis_sostav.count_set_wagons & kis_sostav.count_wagons == kis_sostav.count_set_nathist)
                    {
                        kis_sostav.close = DateTime.Now;
                        kis_sostav.close_user = System.Environment.UserDomainName + @"\" + System.Environment.UserName;
                        int res_close = ef_tkis.SaveArrivalSostav(kis_sostav);
                        mess_put += " - перенесен и закрыт";
                        mess_put.WriteEvents(res_close > 0 ? EventStatus.Ok : EventStatus.Error, servece_owner, eventID);
                        close++;
                    }
                }
                catch (Exception e)
                {
                    e.WriteErrorMethod(String.Format("TransferSostavKisTorailWay()"), servece_owner, eventID);
                    return -1;
                }
            }
            return close;
        }
        /// <summary>
        /// Поставить вагоны состава на путь станции 
        /// </summary>
        /// <param name="orc_sostav"></param>
        /// <returns></returns>
        public int PutCarsToStation(ref ArrivalSostav orc_sostav)
        {
            RCReference rc_ref = new RCReference();
            EFWagons ef_wag = new EFWagons();
            string mess_transf = String.Format("состава (натурный лист: {0}, дата: {1}, ID строки: {2}) прибывшего на АМКР по данным системы КИС", orc_sostav.natur, orc_sostav.datetime, orc_sostav.id);
            string mess_arr_sostav_err = "Ошибка переноса " + mess_transf;
            // Определим станцию назначения
            int? id_stations = rc_ref.DefinitionIDStations(orc_sostav.id, orc_sostav.way_num);
            if (id_stations == null)
            {
                String.Format(mess_arr_sostav_err + " - ID станции: {0} не определён в справочнике системы RailWay", orc_sostav.id_station_kis).WriteError(servece_owner, eventID);
                return (int)errorTransfer.no_stations;
            }
            // Определим путь на станции
            int? id_ways = rc_ref.DefinitionIDWays((int)id_stations, orc_sostav.way_num);
            if (id_ways == null)
            {
                String.Format(mess_arr_sostav_err + " - ID пути: {0} станции: {1} не определён в справочнике системы RailWay", orc_sostav.way_num, orc_sostav.id_station_kis).WriteError(servece_owner, eventID);
                return (int)errorTransfer.no_ways;
            }

            // Формирование общего списка вагонов и постановка их на путь станции прибытия
            List<PromVagon> list_pv = ef_wag.GetVagon(orc_sostav.natur, orc_sostav.id_station_kis, orc_sostav.day, orc_sostav.month, orc_sostav.year, orc_sostav.napr == 2 ? true : false).ToList();
            List<PromNatHist> list_nh = ef_wag.GetNatHist(orc_sostav.natur, orc_sostav.id_station_kis, orc_sostav.day, orc_sostav.month, orc_sostav.year, orc_sostav.napr == 2 ? true : false).ToList();
            int res_set_list = SetListWagon(ref orc_sostav, list_pv, list_nh);
            if (res_set_list >= 0)
            {
                //return SetCarsToStation(ref orc_sostav, (int)id_stations, (int)id_ways);
            }
            return res_set_list;
        }

        ///// <summary>
        ///// Поставить вагоны на путь станции АМКР
        ///// </summary>
        ///// <param name="orc_sostav"></param>
        ///// <param name="id_stations"></param>
        ///// <param name="id_ways"></param>
        ///// <returns></returns>
        //public int SetCarsToStation(ref ArrivalSostav orc_sostav, int id_stations, int id_ways)
        //{
        //    string mess_transf = String.Format("состава (натурный лист: {0}, дата: {1}, ID строки: {2}) прибывшего на АМКР (станция: {3}, путь: {4}) по данным системы КИС.", orc_sostav.NaturNum, orc_sostav.DateTime, orc_sostav.IDOrcSostav, id_stations, id_ways);
        //    string mess_arr_sostav = "Перенос " + mess_transf;
        //    string mess_arr_sostav_err = "переноса " + mess_transf;
        //    if (orc_sostav == null) return 0;
        //    if (orc_sostav.ListNoSetWagons == null & orc_sostav.ListWagons == null) return 0;
        //    try
        //    {
        //        List<int> set_wagons = new List<int>();
        //        // Обнавляем вагоны
        //        if (orc_sostav.CountSetWagons != null & orc_sostav.ListNoSetWagons != null)
        //        {
        //            set_wagons = GetWagonsToListInt(orc_sostav.ListNoSetWagons); // доствавим вагоны
        //        }
        //        // Ставим вагоны в первый раз
        //        if (orc_sostav.CountSetWagons == null & orc_sostav.ListNoSetWagons == null & orc_sostav.ListWagons != null)
        //        {
        //            set_wagons = GetWagonsToListInt(orc_sostav.ListWagons); // поставим занаво
        //        }
        //        if (set_wagons.Count() == 0) return 0;
        //        ResultTransfers result = new ResultTransfers(set_wagons.Count(), 0, null, null, 0, 0);
        //        // Ставим вагоны на путь станции
        //        orc_sostav.ListNoSetWagons = null;
        //        foreach (int wag in set_wagons)
        //        {
        //            mtcont.SetNaturToMTList(orc_sostav.NaturNum, wag, orc_sostav.DateTime, 15); // Поставим натурку на прибывший вагон по МТ
        //            new_arrival.SetArrivalCars(orc_sostav.NaturNum, wag, orc_sostav.DateTime, 15); // Поставим натурку на прибывший вагон по МТ новый сбор данных
        //            new_approaches.SetApproachesCars(orc_sostav.NaturNum, wag, orc_sostav.DateTime, 15); // Поставим натурку на прибывший вагон по МТ новый сбор данных
        //            if (result.SetResultInsert(SetCarMTToStation(orc_sostav.NaturNum, wag, orc_sostav.DateTime, id_stations, id_ways, orc_sostav.IDOrcStation)))
        //            {
        //                // Ошибка
        //                orc_sostav.ListNoSetWagons += wag.ToString() + ";";
        //            }
        //        }
        //        orc_sostav.CountSetWagons = orc_sostav.CountSetWagons == null ? result.ResultInsert : (int)orc_sostav.CountSetWagons + result.ResultInsert;
        //        mess_arr_sostav += String.Format(" Определено для переноса: {0} вагонов, перенесено: {1} вагонов, ранее перенесено: {2} вагонов, ошибок переноса {3}.", set_wagons.Count(), result.inserts, result.skippeds, result.errors);
        //        ServicesEventLog.LogWarning(mess_arr_sostav, servece_owner, eventID);
        //        if (set_wagons.Count() > 0) { mess_arr_sostav.SaveLogEvents(result.errors > 0 ? EventStatus.Error : EventStatus.Ok, servece_owner, eventID); }
        //        // Сохранить результат и вернуть код
        //        if (oas.SaveOracle_ArrivalSostav(orc_sostav) < 0) return (int)errorTransfer.global; else return result.ResultInsert;
        //    }
        //    catch (Exception e)
        //    {
        //        ServicesEventLog.LogError(e, mess_arr_sostav_err, servece_owner, eventID);
        //        return (int)errorTransfer.global;
        //    }

        //}
        #endregion

    }
}
