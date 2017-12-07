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
using EFRC.Concrete;
using EFMT.Entities;
using EFMT.Concrete;
using EFRC.Entities;


namespace KIS
{
    public class trWagon
    {
        public int Position { get; set; }
        public int CarriageNumber { get; set; }
        public int CountryCode { get; set; }
        public float Weight { get; set; }
        public int IDCargo { get; set; }
        public string Cargo { get; set; }
        public int IDStation { get; set; }
        public string Station { get; set; }
        public int Consignee { get; set; }
        public string Operation { get; set; }
        public string CompositionIndex { get; set; }
        public DateTime DateOperation { get; set; }
        public int TrainNumber { get; set; }
        public int Conditions { get; set; }
    }

    public class trSostav
    {
        public int id { get; set; }
        public int? codecs_in_station { get; set; } // Станция получатель состава
        public int? codecs_from_station { get; set; } // Станция отравитель состава
        //public string FileName { get; set; }
        //public string CompositionIndex { get; set; }
        public DateTime DateTime_on { get; set; }
        public DateTime DateTime_from { get; set; }
        //public int Operation { get; set; }
        //public DateTime Create { get; set; }
        //public DateTime? Close { get; set; }
        public int? ParentID { get; set; }
        public trWagon[] Wagons { get; set; }
    }

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
        no_del_input = -15,
        set_table_arrival_sostav = -30,
    }

    public enum statusSting : int { Normal = 0, Delete = 1, Insert = 2, Update = 3 }

    /// <summary>
    /// Класс данных результат переноса массва данных
    /// </summary>
    public class ResultTransfers
    {
        public int counts { get; set; }
        public int result { get; set; }
        public int? inserts { get; set; }
        public int? updates { get; set; }
        public int? deletes { get; set; }
        public int? skippeds { get; set; }
        public int? errors { get; set; }
        public ResultTransfers(int count, int? inserts, int? updates, int? deletes, int? skippeds, int? errors)
        {
            this.counts = count;
            this.inserts = inserts;
            this.updates = updates;
            this.deletes = deletes;
            this.skippeds = skippeds;
            this.errors = errors;
        }
        public void IncInsert() { if (inserts != null) inserts++; }
        public void IncUpdate() { if (updates != null) updates++; }
        public void IncDelete() { if (deletes != null) deletes++; }
        public void IncSkipped() { if (skippeds != null) skippeds++; }
        public void IncError() { if (errors != null) errors++; }
        public int ResultInsert { get { if (this.inserts != null & this.skippeds != null) { return (int)this.inserts + (int)this.skippeds; } else return 0; } }
        public int ResultDelete { get { if (this.deletes != null & this.skippeds != null) { return (int)this.deletes + (int)this.skippeds; } else return 0; } }
        public int ResultUpdate { get { if (this.updates != null & this.skippeds != null) { return (int)this.updates + (int)this.skippeds; } else return 0; } }
        /// <summary>
        /// Обработать резултат (возращает true если была ошибка)
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool SetResultInsert(int result)
        {
            this.result = result;
            if (result < 0) { IncError(); return true; }
            if (result == 0) { IncSkipped(); }
            if (result > 0) { IncInsert(); }
            return false;
        }
        public bool SetResultDelete(int result)
        {
            this.result = result;
            if (result < 0) { IncError(); return true; }
            if (result == 0) { IncSkipped(); }
            if (result > 0) { IncDelete(); }
            return false;
        }
        public bool SetResultUpdate(int result)
        {
            this.result = result;
            if (result < 0) { IncError(); return true; }
            if (result == 0) { IncSkipped(); }
            if (result > 0) { IncUpdate(); }
            return false;
        }
    }

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

        #region Таблица переноса составов из КИС [BufferArrivalSostav]
        /// <summary>
        /// Сохранить состав из КИС
        /// </summary>
        /// <param name="ps"></param>
        /// <returns></returns>
        protected int SaveBufferArrivalSostav(PromSostav ps, statusSting status)
        {
            EFTKIS ef_tkis = new EFTKIS();
            try
            {
                DateTime DT = DateTime.Parse(ps.D_DD.ToString() + "-" + ps.D_MM.ToString() + "-" + ps.D_YY.ToString() + " " + ps.T_HH.ToString() + ":" + ps.T_MI.ToString() + ":00", CultureInfo.CreateSpecificCulture("ru-RU"));
                return ef_tkis.SaveArrivalSostav(new BufferArrivalSostav()
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
                e.WriteErrorMethod(String.Format("SaveBufferArrivalSostav(ps={0}, status={1})", ps.GetFieldsAndValue(), status), servece_owner, eventID);
                return -1;
            }
        }
        /// <summary>
        /// Найти и удалить из списка ArrivalSostav елемент natur
        /// </summary>
        /// <param name="list"></param>
        /// <param name="natur"></param>
        /// <returns></returns>
        protected bool DelExistBufferArrivalSostav(ref List<BufferArrivalSostav> list, int natur)
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
                e.WriteErrorMethod(String.Format("DelExistBufferArrivalSostav(list={0}, natur={1})", list, natur), servece_owner, eventID);
            }
            return Result;
        }
        /// <summary>
        /// Проверяет списки PromSostav и ArrivalSostav на повторяющие натурные листы, оставляет в списке PromSostav - добавленные составы, ArrivalSostav - удаленные из КИС составы
        /// </summary>
        /// <param name="list_ps"></param>
        /// <param name="list_as"></param>
        protected void DelExistBufferArrivalSostav(ref List<PromSostav> list_ps, ref List<BufferArrivalSostav> list_as)
        {
            try
            {
                int index = list_ps.Count() - 1;
                while (index >= 0)
                {
                    if (DelExistBufferArrivalSostav(ref list_as, list_ps[index].N_NATUR))
                    {
                        list_ps.RemoveAt(index);
                    }
                    index--;
                }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("DelExistBufferArrivalSostav(list_ps={0}, list_as={1})", list_ps, list_as), servece_owner, eventID);
            }
        }
        /// <summary>
        /// Удалить ранее перенесеные составы
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        protected int DeleteBufferArrivalSostav(List<BufferArrivalSostav> list)
        {
            EFTKIS ef_tkis = new EFTKIS();
            try
            {
                if (list == null | list.Count == 0) return 0;
                int delete = 0;
                int errors = 0;
                foreach (BufferArrivalSostav or_as in list)
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
                e.WriteErrorMethod(String.Format("DeleteBufferArrivalSostav(list={0})", list), servece_owner, eventID);
                return -1;
            }
        }
        /// <summary>
        /// Добавить новые составы появившиеся после переноса
        /// </summary>
        /// <param name="list"></param>
        protected int InsertBufferArrivalSostav(List<PromSostav> list)
        {
            try
            {
                if (list == null | list.Count == 0) return 0;
                int insers = 0;
                int errors = 0;
                foreach (PromSostav ps in list)
                {
                    int res = SaveBufferArrivalSostav(ps, statusSting.Insert);
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
                e.WriteErrorMethod(String.Format("InsertBufferArrivalSostav(list={0})", list), servece_owner, eventID);
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
        public int CopyBufferArrivalSostavOfKIS(int day_control_add_natur)
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
            List<BufferArrivalSostav> list_arrivalsostav = new List<BufferArrivalSostav>();
            try
            {
                // Считаем дату последненго состава
                DateTime? lastDT = ef_tkis.GetLastDateTime();
                if (lastDT != null)
                {
                    // Данные есть получим новые
                    list_newsostav = ef_wag.GetInputPromSostav(((DateTime)lastDT).AddSeconds(1), DateTime.Now, false).ToList();
                    list_oldsostav = ef_wag.GetInputPromSostav(((DateTime)lastDT).AddDays(day_control_add_natur * -1), ((DateTime)lastDT).AddSeconds(1), false).ToList();
                    list_arrivalsostav = ef_tkis.GetBufferArrivalSostav(((DateTime)lastDT).AddDays(day_control_add_natur * -1), ((DateTime)lastDT).AddSeconds(1)).ToList();
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

                        int res = SaveBufferArrivalSostav(ps, statusSting.Normal);
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
                    List<BufferArrivalSostav> list_as = new List<BufferArrivalSostav>();
                    list_as = list_arrivalsostav.Where(a => a.status != (int)statusSting.Delete).ToList();
                    DelExistBufferArrivalSostav(ref list_ps, ref list_as);
                    int ins = InsertBufferArrivalSostav(list_ps);
                    int del = DeleteBufferArrivalSostav(list_as);
                    string mess_upd = String.Format("Таблица состояния переноса составов зашедших на АМКР по данным системы КИС (определено добавленных составов:{0}, перенесено:{1}, определено удаленных составов:{2}, удалено:{3}).",
                    list_ps.Count(), ins, list_as.Count(), del);
                    mess_upd.WriteInformation(servece_owner, this.eventID);
                    if (list_ps.Count() > 0 | list_as.Count() > 0) mess_upd.WriteEvents(EventStatus.Ok, servece_owner, eventID);
                    normals += ins;
                }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CopyBufferArrivalSostavOfKIS(day_control_add_natur={0})", day_control_add_natur), servece_owner, eventID);
                return -1;
            }
            return normals;
        }
        /// <summary>
        /// Получить или обновить общий список вагонов, список не поставленных и не обнавленных вагонов
        /// </summary>
        /// <param name="orc_sostav"></param>
        public int SetListWagon(ref BufferArrivalSostav orc_sostav, List<PromVagon> list_pv, List<PromNatHist> list_nh)
        {
            EFTKIS ef_tkis = new EFTKIS();
            if (list_pv.Count() == 0 & list_nh.Count() == 0) return 0; // Списков вагонов нет
            try
            {

                //Создать и список вагонов заново и поставить их на путь ( or_as.CountWagons, or_as.list_wagons )
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
        public int TransferArrivalOfKIS()
        {
            EFTKIS ef_tkis = new EFTKIS();
            int close = 0;
            IQueryable<BufferArrivalSostav> list_noClose = ef_tkis.GetBufferArrivalSostavNoClose();
            if (list_noClose == null | list_noClose.Count() == 0) return 0;
            foreach (BufferArrivalSostav or_as in list_noClose.ToList())
            {
                try
                {
                    string mess_put = String.Format("Состав (натурный лист: {0}, дата: {1}, ID строки: {2}) прибывший на АМКР по данным системы КИС", or_as.natur, or_as.datetime, or_as.id_station_kis);
                    BufferArrivalSostav kis_sostav = new BufferArrivalSostav();
                    kis_sostav = or_as;
                    // Поставим состав на станции АМКР системы RailCars
                    int res_put = PutCarsToStation(ref kis_sostav);
                    // Обновление составов на станции АМКР системы RailCars
                    int res_upd = UpdateCarsToStation(ref kis_sostav);
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
                    e.WriteErrorMethod(String.Format("TransferArrivalOfKIS()"), servece_owner, eventID);
                    return -1;
                }
            }
            return close;
        }

        #region ПОСТАВИТЬ НА ПУТЬ PROM_VAG
        /// <summary>
        /// Поставить вагоны состава на путь станции 
        /// </summary>
        /// <param name="orc_sostav"></param>
        /// <returns></returns>
        public int PutCarsToStation(ref BufferArrivalSostav orc_sostav)
        {
            RCReference rc_ref = new RCReference();
            EFWagons ef_wag = new EFWagons();
            string mess_transf = String.Format("состава (натурный лист: {0}, дата: {1}, ID строки: {2}) прибывшего на АМКР по данным системы КИС", orc_sostav.natur, orc_sostav.datetime, orc_sostav.id);
            string mess_arr_sostav_err = "Ошибка переноса " + mess_transf;
            // Определим станцию назначения
            int? id_stations = rc_ref.DefinitionIDStations(orc_sostav.id_station_kis, orc_sostav.way_num);
            if (id_stations == null)
            {
                String.Format(mess_arr_sostav_err + " - ID станции: {0} не определён в справочнике системы RailWay", orc_sostav.id_station_kis).WriteError(servece_owner, eventID);
                return (int)errorTransfer.no_stations;
            }
            if (id_stations == 42) id_stations = 20; // Коррекция Промышленная Керамет -> 'это промышленная
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
                return SetCarsToStation(ref orc_sostav, (int)id_stations, (int)id_ways);
            }
            return res_set_list;
        }
        /// <summary>
        /// Поставить вагоны на путь станции АМКР
        /// </summary>
        /// <param name="orc_sostav"></param>
        /// <param name="id_stations"></param>
        /// <param name="id_ways"></param>
        /// <returns></returns>
        public int SetCarsToStation(ref BufferArrivalSostav orc_sostav, int id_stations, int id_ways)
        {
            EFTKIS ef_tkis = new EFTKIS();
            string mess_transf = String.Format("состава (натурный лист: {0}, дата: {1}, ID строки: {2}) прибывшего на АМКР (станция: {3}, путь: {4}) по данным системы КИС.", orc_sostav.natur, orc_sostav.datetime, orc_sostav.id_station_kis, id_stations, id_ways);
            string mess_arr_sostav = "Перенос " + mess_transf;
            string mess_arr_sostav_err = "переноса " + mess_transf;
            if (orc_sostav == null) return 0;
            if (orc_sostav.list_no_set_wagons == null & orc_sostav.list_wagons == null) return 0;
            try
            {
                List<int> set_wagons = new List<int>();
                // Обнавляем вагоны
                if (orc_sostav.count_set_wagons != null & orc_sostav.list_no_set_wagons != null)
                {
                    set_wagons = GetWagonsToListInt(orc_sostav.list_no_set_wagons); // доствавим вагоны
                }
                // Ставим вагоны в первый раз
                if (orc_sostav.count_set_wagons == null & orc_sostav.list_no_set_wagons == null & orc_sostav.list_wagons != null)
                {
                    set_wagons = GetWagonsToListInt(orc_sostav.list_wagons); // поставим занаво
                }
                if (set_wagons.Count() == 0) return 0;
                ResultTransfers result = new ResultTransfers(set_wagons.Count(), 0, null, null, 0, 0);
                // Ставим вагоны на путь станции
                orc_sostav.list_no_set_wagons = null;
                foreach (int wag in set_wagons)
                {
                    //mtcont.SetNaturToMTList(orc_sostav.natur, wag, orc_sostav.datetime, 15); // Поставим натурку на прибывший вагон по МТ
                    //new_arrival.SetArrivalCars(orc_sostav.NaturNum, wag, orc_sostav.DateTime, 15); // Поставим натурку на прибывший вагон по МТ новый сбор данных
                    //new_approaches.SetApproachesCars(orc_sostav.NaturNum, wag, orc_sostav.DateTime, 15); // Поставим натурку на прибывший вагон по МТ новый сбор данных
                    if (result.SetResultInsert(SetCarMTToStation(orc_sostav.natur, wag, orc_sostav.datetime, id_stations, id_ways, orc_sostav.id_station_kis)))
                    {
                        // Ошибка
                        orc_sostav.list_no_set_wagons += wag.ToString() + ";";
                    }
                }
                orc_sostav.count_set_wagons = orc_sostav.count_set_wagons == null ? result.ResultInsert : (int)orc_sostav.count_set_wagons + result.ResultInsert;
                mess_arr_sostav += String.Format(" Определено для переноса: {0} вагонов, перенесено: {1} вагонов, ранее перенесено: {2} вагонов, ошибок переноса {3}.", set_wagons.Count(), result.inserts, result.skippeds, result.errors);
                mess_arr_sostav.WriteInformation(servece_owner, eventID);
                if (set_wagons.Count() > 0) { mess_arr_sostav.WriteEvents(result.errors > 0 ? EventStatus.Error : EventStatus.Ok, servece_owner, eventID); }
                // Сохранить результат и вернуть код
                if (ef_tkis.SaveArrivalSostav(orc_sostav) < 0)
                { return (int)errorTransfer.set_table_arrival_sostav; }
                else { return result.ResultInsert; }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SetCarsToStation(orc_sostav={0}, id_stations={1}, id_stations={2})", orc_sostav, id_stations, id_ways), servece_owner, eventID);
                return (int)errorTransfer.global;
            }

        }
        /// <summary>
        /// Поставить вагон МТ на станцию АМКР
        /// </summary>
        /// <param name="natur"></param>
        /// <param name="num_vag"></param>
        /// <param name="dt_amkr"></param>
        /// <param name="id_stations"></param>
        /// <param name="id_ways"></param>
        /// <param name="id_stat_kis"></param>
        /// <returns></returns>
        protected int SetCarMTToStation(int natur, int num_vag, DateTime dt_amkr, int id_stations, int id_ways, int id_stat_kis)
        {
            EFSAP ef_sap = new EFSAP();
            EFWagons ef_wag = new EFWagons();
            EFMetallurgTrans ef_mt = new EFMetallurgTrans();
            SAPTransfer sap_trans = new SAPTransfer();
            EFRailCars ef_rc = new EFRailCars();
            RCReference rc_ref = new RCReference();
            string mess = String.Format("вагона №:{0}, принадлежащего составу (натурный лист: {1}, дата: {2}) на путь станции (код станции:{3}, код пути:{4})", num_vag, natur, dt_amkr, id_stations, id_ways);
            string mess_arr_vag = "Перенос " + mess;
            string mess_arr_vag_err = "переноса " + mess;
            string mess_arr_vag_err1 = "Ошибка переноса " + mess;
            try
            {
                
                int idsostav = ef_sap.GetDefaultIDSAPIncSupply();
                // Получим информацию для заполнения вагона с учетом отсутствия данных в PromVagon
                PromVagon pv = ef_wag.GetVagon(natur, id_stat_kis, dt_amkr.Day, dt_amkr.Month, dt_amkr.Year, num_vag);
                PromNatHist pnh = ef_wag.GetNatHist(natur, id_stat_kis, dt_amkr.Day, dt_amkr.Month, dt_amkr.Year, num_vag);
                if (pv == null & pnh == null) return (int)errorTransfer.no_wagon_is_list;   // Ошибка нет вагонов в списке
                if (pv == null)
                {
                    pv = new PromVagon()
                    {
                        N_VAG = pnh.N_VAG,
                        NPP = pnh != null ? (int)pnh.NPP : 0,
                        GODN = pnh.GODN,
                        K_ST = pnh.K_ST,
                        N_NATUR = pnh.N_NATUR,
                        D_PR_DD = pnh.D_PR_DD,
                        D_PR_MM = pnh.D_PR_MM,
                        D_PR_YY = pnh.D_PR_YY,
                        T_PR_HH = pnh.T_PR_HH,
                        T_PR_MI = pnh.T_PR_MI,
                        KOD_STRAN = pnh.KOD_STRAN,
                        WES_GR = pnh.WES_GR,
                        K_GR = pnh.K_GR
                    };
                }
                // Сделать отметку на МТ о принятии вагона
                int res_close_arrival = ef_mt.CloseArrivalCarsOfDocWeight(natur, num_vag, dt_amkr, pv.WES_GR);
                if (res_close_arrival <= 0)
                { 
                    res_close_arrival = ef_mt.CloseArrivalCarsOfDocDay(natur, num_vag, dt_amkr, 1);
                }
                int res_close_approaches = ef_mt.CloseApproachesCarsOfDocWeight(natur, num_vag, dt_amkr, pv.WES_GR);
                if (res_close_approaches <= 0)
                {
                    res_close_approaches = ef_mt.CloseApproachesCarsOfDocDay(natur, num_vag, dt_amkr, 1);
                }
                ArrivalCars mt_list = ef_mt.GetArrivalCarsToNatur(natur, num_vag, dt_amkr, 15);
                if (mt_list != null)
                {
                    idsostav = mt_list.IDSostav;
                }
                // Проверим есть строка в справочнеке САП поставки
                sap_trans.CheckingWagonToSAPSupply(idsostav, pv);

                                                                 //if (idsostav > 0)
                                                                 //{
                                                                 //    int res_appr = approaches.CloseApproachesSostav(idsostav);
                                                                 //    string mes_appr = String.Format("Состав {0}, принятый на АМКР закрыт в таблице составов  на подходе id={1}", idsostav, res_appr);
                                                                 //    string mes_appr_err = String.Format("Ошибка закрытия состава {0} в таблице составов  на подходе, код ошибки ={1}", idsostav, res_appr);
                                                                 //    if (res_appr > 0)
                                                                 //    {
                                                                 //        mes_appr.WriteInformation(servece_owner, eventID);
                                                                 //    }
                                                                 //    if (res_appr < 0)
                                                                 //    {
                                                                 //        mes_appr_err.WriteError(servece_owner, eventID);
                                                                 //    }
                                                                 //    mes_appr.WriteEvents(res_appr < 0 ? EventStatus.Error : EventStatus.Ok, servece_owner, eventID);
                                                                 //} // Закроем в базе подходов принятый состав
                                                                 // Проверим в наличии в прибытия из любой станции УЗ на станцию id_stations
                VAGON_OPERATIONS vagon = ef_rc.GetVagonsOfArrivalUZ(idsostav, num_vag, ef_rc.GetUZStationsToID().ToArray(), id_stations);
                if (vagon != null)
                {
                    // Поставить на путь станции
                    // Обновим из КИС необходимости
                    // Удалим остальные
                    int res = ef_rc.TakeVagonOfUZ(idsostav, num_vag, ef_rc.GetUZStationsToID().ToArray(), natur, dt_amkr, id_stations, id_ways, pv.GODN != null ? (int)pv.GODN : -1);
                    String.Format(mess_arr_vag + ", выполнен из прибытия на указанную станцию, ID операции {0}", res).WriteInformation(servece_owner, eventID);
                }
                else
                {
                    // Проверим в наличии в прибытия из любой станции УЗ на любую станцию АМКР
                    IQueryable<VAGON_OPERATIONS> vagons_uz = ef_rc.GetVagonsOfArrival(idsostav, num_vag, ef_rc.GetUZStationsToID().ToArray());
                    if (vagons_uz.Count() > 0)
                    {
                        //возмем первый 
                        // Поставить на путь станции
                        // Обновим из КИС необходимости
                        // Удалим остальные
                        int res = ef_rc.TakeVagonOfAllUZ(idsostav, num_vag, ef_rc.GetUZStationsToID().ToArray(), natur, dt_amkr, id_stations, id_ways, pv.GODN != null ? (int)pv.GODN : -1);
                        String.Format(mess_arr_vag + ", выполнен из прибытия, ID операции {0}", res).WriteInformation(servece_owner, eventID);
                    }
                    else
                    {
                        // Проверим в наличии записи по данному составу и вагону на станциях АМКР
                        IQueryable<VAGON_OPERATIONS> vagons_amkr = ef_rc.GetVagonsOfStationAMKR(idsostav, num_vag, ef_rc.GetAMKRStationsToID().ToArray());
                        if (vagons_amkr.Count() > 0)
                        {
                            //List<VAGON_OPERATIONS> test = vagons_amkr.ToList(); // тест
                            //TODO: если он отправлен на станции УЗ (закрыть прибытие уз и сделать строку в операциях)
                            // Обновим из КИС необходимости
                            int res = ef_rc.UpdateVagon(idsostav, num_vag, ef_rc.GetAMKRStationsToID().ToArray(), dt_amkr, pv.GODN != null ? (int)pv.GODN : -1, natur);
                            String.Format(mess_arr_vag + ", выполнено обновление, вручную принятого вагона, обновлено:{0} строк", res).WriteInformation(servece_owner, eventID);
                        }
                        else
                        {
                            //создадим по данным КИС
                            int id_wagon = rc_ref.DefinitionSetIDVagon(num_vag, dt_amkr, -1, null, natur, false); // определить id вагона (если нет создать новый id? локоматив -1)
                            if (!ef_rc.IsVagonOperationKIS(natur, dt_amkr, (int)num_vag))
                            {

                                int res = ef_rc.InsertVagon(natur, dt_amkr, id_wagon, num_vag, idsostav, (mt_list != null ? mt_list.DateOperation as DateTime? : null), id_stations, id_ways, id_stat_kis, pv.GODN, 15);
                                String.Format(mess_arr_vag + ", выполнен по данным КИС (idsostav:{0} - не найден), ID операции {1}", idsostav, res).WriteInformation(servece_owner, eventID);
                                if (res < 0)
                                {
                                    String.Format(mess_arr_vag_err1 + ", код ошибки:{0}", res).WriteError(servece_owner, eventID);
                                }
                                return res;
                            }
                            else return 0; // Вагон уже стоит
                        }
                    }
                }
                return 0;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SetCarMTToStation(natur={0}, num_vag={1}, dt_amkr={2}, id_stations={3}, id_ways={4}, id_stat_kis={5})", natur, num_vag, dt_amkr, id_stations, id_ways, id_stat_kis), servece_owner, eventID);
                return (int)errorTransfer.global;
            }
        }
        #endregion

        #region ОБНОВИТЬ НА ПУТИ NAN_HIST
        /// <summary>
        /// Обновить информацию о вагонах состава
        /// </summary>
        /// <param name="orc_sostav"></param>
        /// <returns></returns>
        public int UpdateCarsToStation(ref BufferArrivalSostav orc_sostav)
        {
            RCReference rc_ref = new RCReference();
            EFWagons ef_wag = new EFWagons();
            string mess_update = String.Format("состава (натурный лист: {0}, дата: {1}, ID строки: {2}) прибывшего на АМКР по данным системы КИС", orc_sostav.natur, orc_sostav.datetime, orc_sostav.id_station_kis);
            string mess_update_err = "Ошибка обновления " + mess_update;
            // Определим станцию назначения
            int? id_stations = rc_ref.DefinitionIDStations(orc_sostav.id_station_kis, orc_sostav.way_num);
            if (id_stations == null)
            {
                String.Format(mess_update_err + " - ID станции: {0} не определён в справочнике системы RailWay", orc_sostav.id_station_kis).WriteError(servece_owner, eventID);
                return (int)errorTransfer.no_stations;
            }
            if (id_stations == 42) id_stations = 20; // Коррекция Промышленная Керамет -> 'это промышленная
            // Определим путь на станции
            int? id_ways = rc_ref.DefinitionIDWays((int)id_stations, orc_sostav.way_num);
            if (id_ways == null)
            {
                String.Format(mess_update_err + " - ID пути: {0} станции: {1} не определён в справочнике системы RailWay", orc_sostav.way_num, orc_sostav.id_station_kis).WriteError(servece_owner, eventID);
                return (int)errorTransfer.no_ways;
            }
            // Обновим информацию по количеству вагонов в таблице NatHist
            List<PromNatHist> list_nh = ef_wag.GetNatHist(orc_sostav.natur, orc_sostav.id_station_kis, orc_sostav.day, orc_sostav.month, orc_sostav.year, orc_sostav.napr == 2 ? true : false).ToList();
            orc_sostav.count_nathist = list_nh.Count() > 0 ? list_nh.Count() as int? : null;
            int res_upd = UpdCarsToStation(ref orc_sostav, (int)id_stations, (int)id_ways);
            return res_upd;
        }
        /// <summary>
        /// Обновить информацию по вагонам перенесеным по данным КИС
        /// </summary>
        /// <param name="orc_sostav"></param>
        /// <param name="id_stations"></param>
        /// <param name="id_ways"></param>
        /// <returns></returns>
        public int UpdCarsToStation(ref BufferArrivalSostav orc_sostav, int id_stations, int id_ways)
        {
            EFTKIS ef_tkis = new EFTKIS();
            string mess_update = String.Format("состава (натурный лист: {0}, дата: {1}, ID строки: {2}) прибывшего на АМКР (станция: {3}, путь: {4}) по данным системы КИС.", orc_sostav.natur, orc_sostav.datetime, orc_sostav.id_station_kis, id_stations, id_ways);
            string mess_update_sostav = "Обновление " + mess_update;
            string mess_update_sostav_err = "обновления " + mess_update;
            if (orc_sostav == null) return 0;
            if (orc_sostav.count_set_wagons == null) return 0; // вагоны не поставлены на путь станции

            try
            {
                List<int> set_wagons = new List<int>();
                // Обнавляем вагоны
                if (orc_sostav.count_set_nathist != null & orc_sostav.list_no_update_wagons != null & orc_sostav.count_set_wagons != null)
                {
                    set_wagons = GetWagonsToListInt(orc_sostav.list_no_update_wagons); // до обновим вагоны
                }
                if (orc_sostav.count_set_nathist != null & orc_sostav.list_no_update_wagons == null & orc_sostav.count_set_wagons != null & orc_sostav.count_set_wagons != orc_sostav.count_set_nathist)
                {
                    set_wagons = GetWagonsToListInt(orc_sostav.list_wagons); // поставим занаво
                }
                // Обновляем вагоны в первый раз
                if ((orc_sostav.count_set_nathist == null | orc_sostav.count_set_nathist == 0) & orc_sostav.list_no_update_wagons == null & orc_sostav.count_set_wagons != null)
                {
                    set_wagons = GetWagonsToListInt(orc_sostav.list_wagons); // поставим занаво
                }
                if (set_wagons.Count() == 0) return 0;
                ResultTransfers result = new ResultTransfers(set_wagons.Count(), null,0 , null,0,0);
                orc_sostav.list_no_update_wagons = null;
                orc_sostav.message = null;
                // Ставим вагоны на путь станции
                foreach (int wag in set_wagons)
                {
                    if (result.SetResultUpdate(UpdCarToStation(orc_sostav.natur, wag, orc_sostav.datetime, id_stations, id_ways, orc_sostav.id_station_kis)))
                    {
                        // Ошибка
                        orc_sostav.list_no_update_wagons += wag.ToString() + ";";
                        orc_sostav.message += wag.ToString() + ":" + result.result.ToString() + ";";
                    }
                }
                orc_sostav.count_set_nathist = (orc_sostav.count_set_nathist != null ? orc_sostav.count_set_nathist : 0) + result.ResultUpdate;
                mess_update_sostav += String.Format(" Определено для обновления: {0} вагонов, обновлено: {1} вагонов, ранее обновлено: {2} вагонов, ошибок обновления {3}.", set_wagons.Count(), result.inserts, result.skippeds, result.errors);
                mess_update_sostav.WriteInformation(servece_owner, eventID);
                //if (set_wagons.Count() > 0) { mess_update_sostav.SaveLogEvents(result.errors > 0 ? EventStatus.Error : EventStatus.Ok, servece_owner, eventID); }
                // Сохранить результат и вернуть код
                if (ef_tkis.SaveArrivalSostav(orc_sostav) < 0)
                { return (int)errorTransfer.set_table_arrival_sostav; }
                else { return result.ResultUpdate; }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("UpdCarsToStation(orc_sostav={0}, id_stations={1}, id_stations={2})", orc_sostav, id_stations, id_ways), servece_owner, eventID);
                return (int)errorTransfer.global;
            }
        }
        /// <summary>
        /// Обновить информацию по вагону перенесеному по данным КИС
        /// </summary>
        /// <param name="natur"></param>
        /// <param name="num_vag"></param>
        /// <param name="dt_amkr"></param>
        /// <param name="id_stations"></param>
        /// <param name="id_ways"></param>
        /// <param name="id_stat_kis"></param>
        /// <returns></returns>
        protected int UpdCarToStation(int natur, int num_vag, DateTime dt_amkr, int id_stations, int id_ways, int id_stat_kis)
        {
            EFWagons ef_wag = new EFWagons();
            RCReference rc_ref = new RCReference();
            EFRailCars ef_rc = new EFRailCars();
            string mess = String.Format("вагона №:{0}, принадлежащего составу (натурный лист: {1}, дата: {2}) стоящего на пути станции (код станции:{3}, код пути:{4})", num_vag, natur, dt_amkr, id_stations, id_ways);
            string mess_update_vag = "Обновление данных " + mess;
            string mess_update_vag_err = "обновления данных " + mess;
            string mess_update_vag_err1 = "Ошибка " + mess_update_vag_err + mess;
            try
            {
                PromNatHist pnh = ef_wag.GetNatHist(natur, id_stat_kis, dt_amkr.Day, dt_amkr.Month, dt_amkr.Year, num_vag);
                if (pnh == null)
                {
                    String.Format(mess_update_vag_err1 + ", код ошибки:{0}", errorTransfer.no_wagon_is_nathist.ToString()).WriteError(servece_owner, eventID);
                    return (int)errorTransfer.no_wagon_is_nathist;
                }
                //TODO: !! ОТКЛЮЧИТЬ (ОБНОВЛЕНИЕ ВАГОНОВ ПО КИСУ) цех получатель и груз, данные будут братся из справочника вх. поставок САП по id sostav и номкру вагона
                int? id_shop = null;
                if (pnh.K_POL_GR != null)
                {
                    id_shop = rc_ref.DefinitionIDShop((int)pnh.K_POL_GR);
                }
                if (id_shop == null)
                {
                    String.Format(mess_update_vag_err1 + ", код ошибки:{0}, PromNatHist.K_POL_GR:{1}.", errorTransfer.no_shop.ToString(), pnh.K_POL_GR).WriteWarning(servece_owner, eventID);
                    return (int)errorTransfer.no_shop;
                }
                // определяем название груза
                int? id_gruz = null;
                if (pnh.K_GR != null)
                {
                    id_gruz = rc_ref.DefinitionIDGruzs((int)pnh.K_GR, null);
                }
                if (id_gruz == null)
                {
                    String.Format(mess_update_vag_err1 + ", код ошибки:{0}, PromNatHist.K_GR:{1}.", errorTransfer.no_gruz.ToString(), pnh.K_GR).WriteError(servece_owner, eventID);
                    return (int)errorTransfer.no_gruz;
                }
                //Обновим все строки операций по вагону за указанную дату захода и номер натурки
                int res = ef_rc.UpdateVagon(dt_amkr, num_vag, natur, ef_rc.GetAMKRStationsToID().ToArray(), (int)id_gruz, (int)id_shop, pnh.GODN);
                if (res < 0)
                {
                    String.Format(mess_update_vag_err1 + ", код ошибки:{0}", res).WriteError(servece_owner, eventID);
                    return res;
                }
                //Обновлять пока не появится годность
                if (pnh.GODN == null)
                {
                    String.Format(mess_update_vag_err1 + ", код ошибки:{0}", errorTransfer.no_godn.ToString()).WriteWarning(servece_owner, eventID);
                    return (int)errorTransfer.no_godn;
                }
                return res;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("UpdCarToStation(natur={0}, num_vag={1}, dt_amkr={2}, id_stations={3}, id_ways={4}, id_stat_kis={5})", natur, num_vag, dt_amkr, id_stations, id_ways, id_stat_kis), servece_owner, eventID);
                return (int)errorTransfer.global;
            }
        }
        #endregion

        #endregion
    }
}
