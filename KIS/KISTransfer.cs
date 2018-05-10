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
using EFKIS.Helpers;
using RW;
using EFRW.Concrete;
using EFRW.Entities;
using EFKIS.Abstract;


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
        no_del_input = -15,
        old_wagon_is_nathist = -16, // NanHist устарел
        set_table_arrival_sostav = -30,
        set_table_sending_sostav = -31,
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

        
        protected int day_waiting_interval_cars = 3; // интервал ожидания прибытия вагона (суток) на АМКР из станций УЗ Кривого Рога
        public int DayWaitingIntervalCars { get { return this.day_waiting_interval_cars; } set { this.day_waiting_interval_cars = value; } }

        protected int day_range_arrival_kis_copy = 2; // тайм аут (суток) по времени для составов перенесеных из КИС для копирования в систему RailCars
        public int DayRangeArrivalKisCopy { get { return this.day_range_arrival_kis_copy; } set { this.day_range_arrival_kis_copy = value; } }

        protected int day_control_arrival_kis_add_data = 1; // Период(суток) контроля системы КИС вагоны из УЗ КР на предмет вставки новых строк.
        public int DayControlArrivalKisAddData { get { return this.day_control_arrival_kis_add_data; } set { this.day_control_arrival_kis_add_data = value; } }

        protected int day_control_sending_kis_add_data = 1; // Период(суток) контроля системы КИС вагоны из УЗ КР на предмет вставки новых строк.
        public int DayControlSendingKisAddData { get { return this.day_control_sending_kis_add_data; } set { this.day_control_sending_kis_add_data = value; } }

        protected int day_control_input_kis_add_data = 1; // Период(суток) контроля системы КИС вагоны по прибытию на предмет вставки новых строк.
        public int DayControlInputKisAddData { get { return this.day_control_input_kis_add_data; } set { this.day_control_input_kis_add_data = value; } }

        protected int day_control_output_kis_add_data = 1; // Период(суток) контроля системы КИС вагоны по отправке на предмет вставки новых строк.
        public int DayControlOutputKisAddData { get { return this.day_control_output_kis_add_data; } set { this.day_control_output_kis_add_data = value; } }

        protected bool status_control_output_kis = false; // Контроль состояния закрытия строки системы КИС вагоны по отправке.
        public bool StatusControlOutputKis { get { return this.status_control_output_kis; } set { this.status_control_output_kis = value; } }

        protected bool transfer_input_kis = false; // Признак переноса данных из системы КИС в систему RC (1-при совподении правил переносим,0 при совподении правил просто закроем строку).
        public bool TransferInputKis { get { return this.transfer_input_kis; } set { this.transfer_input_kis = value; } }

        protected bool transfer_output_kis = false; // Признак переноса данных из системы КИС в систему RC (1-при совподении правил переносим,0 при совподении правил просто закроем строку).
        public bool TransferOutputKis { get { return this.transfer_output_kis; } set { this.transfer_output_kis = value; } }

        public KISTransfer()
        {

        }

        public KISTransfer(service servece_owner)
        {
            this.servece_owner = servece_owner;
        }

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

        /// <summary>
        /// Получить или обновить общий список вагонов, список не поставленных и не обнавленных вагонов
        /// </summary>
        /// <param name="orc_sostav"></param>
        public int SetListWagon(ref IBufferArrivalSostav orc_sostav, List<PromVagon> list_pv, List<PromNatHist> list_nh)
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
                if (orc_sostav is RCBufferArrivalSostav)
                {
                    return ef_tkis.SaveRCBufferArrivalSostav((RCBufferArrivalSostav)orc_sostav);
                }
                if (orc_sostav is RWBufferArrivalSostav)
                {
                    return ef_tkis.SaveRWBufferArrivalSostav((RWBufferArrivalSostav)orc_sostav);
                }
                return 0;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SetListWagon(orc_sostav={0}, list_pv={1}, list_pv={2})", orc_sostav.GetFieldsAndValue(), list_pv, list_nh), servece_owner, eventID);
                return (int)errorTransfer.global;
            }
        }
        /// <summary>
        /// Получить или обновить общий список вагонов, список не поставленных и не обнавленных вагонов
        /// </summary>
        /// <param name="orc_sostav"></param>
        /// <returns></returns>
        public int SetListWagon(ref IBufferArrivalSostav orc_sostav)
        {
            try
            {
                EFWagons ef_wag = new EFWagons();
                // Формирование общего списка вагонов и постановка их на путь станции прибытия
                List<PromVagon> list_pv = ef_wag.GetVagon(orc_sostav.natur, orc_sostav.id_station_kis, orc_sostav.day, orc_sostav.month, orc_sostav.year, orc_sostav.napr == 2 ? true : false).ToList();
                List<PromNatHist> list_nh = ef_wag.GetNatHist(orc_sostav.natur, orc_sostav.id_station_kis, orc_sostav.day, orc_sostav.month, orc_sostav.year, orc_sostav.napr == 2 ? true : false).ToList();
                return SetListWagon(ref orc_sostav, list_pv, list_nh);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SetListWagon(orc_sostav={0})", orc_sostav.GetFieldsAndValue()), servece_owner, eventID);
                return (int)errorTransfer.global;
            }
        }

   }
}
