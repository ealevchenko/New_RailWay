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
        old_wagon_is_nathist = -16, // NanHist устарел
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

        private int day_range_arrival_kis_copy = 2; // тайм аут (суток) по времени для составов перенесеных из КИС для копирования в систему RailCars
        public int DayRangeArrivalKisCopy { get { return this.day_range_arrival_kis_copy; } set { this.day_range_arrival_kis_copy = value; } }

        private int day_control_arrival_kis_add_data = 1; // Период(суток) контроля системы КИС вагоны из УЗ КР на предмет вставки новых строк.
        public int DayControlArrivalKisAddData { get { return this.day_control_arrival_kis_add_data; } set { this.day_control_arrival_kis_add_data = value; } }

        private int day_control_input_kis_add_data = 1; // Период(суток) контроля системы КИС вагоны по прибытию на предмет вставки новых строк.
        public int DayControlInputKisAddData { get { return this.day_control_input_kis_add_data; } set { this.day_control_input_kis_add_data = value; } }

        private int day_control_output_kis_add_data = 1; // Период(суток) контроля системы КИС вагоны по отправке на предмет вставки новых строк.
        public int DayControlOutputKisAddData { get { return this.day_control_output_kis_add_data; } set { this.day_control_output_kis_add_data = value; } }

        private bool status_control_output_kis = false; // Контроль состояния закрытия строки системы КИС вагоны по отправке.
        public bool StatusControlOutputKis { get { return this.status_control_output_kis; } set { this.status_control_output_kis = value; } }

        private bool transfer_input_kis = false; // Признак переноса данных из системы КИС в систему RC (1-при совподении правил переносим,0 при совподении правил просто закроем строку).
        public bool TransferInputKis { get { return this.transfer_input_kis; } set { this.transfer_input_kis = value; } }

        private bool transfer_output_kis = false; // Признак переноса данных из системы КИС в систему RC (1-при совподении правил переносим,0 при совподении правил просто закроем строку).
        public bool TransferOutputKis { get { return this.transfer_output_kis; } set { this.transfer_output_kis = value; } }

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

        #region Таблица переноса составов из КИС [BufferInputSostav]
        /// <summary>
        /// Сохранить состав из КИС
        /// </summary>
        /// <param name="inp_sostav"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        protected int SaveBufferInputSostav(NumVagStpr1InStDoc inp_sostav, statusSting status)
        {
            EFTKIS ef_tkis = new EFTKIS();
            try
            {
                return ef_tkis.SaveBufferInputSostav(new BufferInputSostav()
                {
                    id = 0,
                    datetime = inp_sostav.DATE_IN_ST,
                    doc_num = inp_sostav.ID_DOC,
                    id_station_from_kis = inp_sostav.ST_IN_ST != null ? (int)inp_sostav.ST_IN_ST : 0,
                    way_num_kis = inp_sostav.N_PUT_IN_ST != null ? (int)inp_sostav.N_PUT_IN_ST : 0,
                    napr = inp_sostav.NAPR_IN_ST != null ? (int)inp_sostav.NAPR_IN_ST : 0,
                    id_station_on_kis = inp_sostav.K_STAN != null ? (int)inp_sostav.K_STAN : 0,
                    natur = inp_sostav.OLD_N_NATUR,
                    count_wagons = null,
                    count_set_wagons = null,
                    close = null,
                    close_user = null,
                    status = (int)status,
                    message = null,
                });
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveBufferInputSostav(inp_sostav={0}, status={1})", inp_sostav.GetFieldsAndValue(), status), servece_owner, eventID);
                return -1;
            }
        }
        /// <summary>
        /// проверить изменения в количестве вагонов в составе
        /// </summary>
        /// <param name="bis"></param>
        /// <param name="doc"></param>
        protected void CheckChangeExistBufferInputSostav(BufferInputSostav bis, int doc)
        {
            EFTKIS ef_tkis = new EFTKIS();
            EFWagons ef_wag = new EFWagons();
            int count_vag = ef_wag.GetCountSTPR1InStVag(doc);
            // Количество вагонов изменено
            if (bis.count_wagons > 0 & count_vag > 0 & bis.count_wagons != count_vag)
            {
                // Изменим количество вагонов и отправим на переустановку вагонов
                bis.count_wagons = count_vag;
                bis.status = (int)statusSting.Update;
                bis.close = null;
                ef_tkis.SaveBufferInputSostav(bis);
            }
        }
        /// <summary>
        /// Найти и удалить из списка Oracle_InputSostav елемент doc
        /// </summary>
        /// <param name="list"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        protected bool DelExistBufferInputSostav(ref List<BufferInputSostav> list, int doc)
        {
            bool Result = false;
            int index = list.Count() - 1;
            while (index >= 0)
            {
                if (list[index].doc_num == doc)
                {
                    CheckChangeExistBufferInputSostav(list[index], doc); // количество вагонов
                    list.RemoveAt(index);
                    Result = true;
                }
                index--;
            }
            return Result;
        }
        /// <summary>
        /// Проверяет списки NumVagStpr1InStDoc и BufferInputSostav на повторяющие документы, оставляет в списке NumVagStpr1InStDoc - добавленные составы, BufferInputSostav - удаленные из КИС составы
        /// </summary>
        /// <param name="list_is"></param>
        /// <param name="list_ois"></param>
        protected void DelExistBufferInputSostav(ref List<NumVagStpr1InStDoc> list_is, ref List<BufferInputSostav> list_ois)
        {
            int index = list_is.Count() - 1;
            while (index >= 0)
            {
                if (DelExistBufferInputSostav(ref list_ois, list_is[index].ID_DOC))
                {
                    list_is.RemoveAt(index);
                }
                index--;
            }
        }
        /// <summary>
        /// удалить строку состава отсутсвующего после переноса
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        protected int DeleteBufferInputSostav(List<BufferInputSostav> list)
        {
            EFRailCars ef_rc = new EFRailCars();
            EFTKIS ef_tkis = new EFTKIS();
            if (list == null || list.Count == 0) return 0;
            int delete = 0;
            int errors = 0;
            foreach (BufferInputSostav bis in list)
            {
                // Удалим вагоны из системы RailCars
                // TODO: Сделать код удаления вагонов из RailWay
                // Удалим вагоны из системы RailCars
                //transfer_rc.DeleteVagonsToDocInput(or_is.DocNum);
                bis.close = DateTime.Now;
                bis.close_user = System.Environment.UserDomainName + @"\" + System.Environment.UserName;
                bis.status = (int)statusSting.Delete;
                int res = ef_tkis.SaveBufferInputSostav(bis);
                if (res > 0) delete++;
                if (res < 1)
                {
                    String.Format("Ошибка выполнения метода DeleteBufferInputSostav, удаление строки:{0} из таблицы состояния переноса составов (на подходах) по данным системы КИС", bis.id).WriteError(servece_owner, this.eventID);
                    errors++;
                }
            }
            String.Format("Таблица состояния переноса составов (на подходах) по данным системы КИС, определенно удаленных в системе КИС {0} составов, удалено из таблицы {1}, ошибок удаления {2}.", list.Count(), delete, errors).WriteWarning(servece_owner, this.eventID);
            return delete;
        }
        /// <summary>
        /// Добавить новые составы появившиеся после переноса
        /// </summary>
        /// <param name="list"></param>
        protected int InsertBufferInputSostav(List<NumVagStpr1InStDoc> list)
        {
            if (list == null | list.Count == 0) return 0;
            int insers = 0;
            int errors = 0;
            foreach (NumVagStpr1InStDoc inp_s in list)
            {
                int res = SaveBufferInputSostav(inp_s, statusSting.Insert);
                if (res > 0) insers++;
                if (res < 1)
                {
                    String.Format("Ошибка выполнения метода InsertBufferInputSostav, добавления строки состава по данным системы КИС(№ документа:{0}, дата:{1}) в таблицу состояния переноса составов по прибытию BufferInputSostav", inp_s.ID_DOC, inp_s.DATE_IN_ST).WriteError(servece_owner, this.eventID);
                    errors++;
                }
            }
            String.Format("Таблица состояния переноса составов (по прибытию) по данным системы КИС, определенно добавленных в системе КИС {0} составов, добавлено в таблицу {1}, ошибок добавления {2}.", list.Count(), insers, errors).WriteWarning(servece_owner, this.eventID);
            return insers;
        }
        #endregion

        #region Таблица переноса составов из КИС [BufferOutputSostav]
        /// <summary>
        /// Создать и сохранить строку BufferOutputSostav
        /// </summary>
        /// <param name="out_sostav"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        protected int SaveBufferOutputSostav(NumVagStpr1OutStDoc out_sostav, statusSting status)
        {
            EFTKIS ef_tkis = new EFTKIS();
            try
            {
                return ef_tkis.SaveBufferOutputSostav(new BufferOutputSostav()
                {
                    id = 0,
                    datetime = out_sostav.DATE_OUT_ST,
                    doc_num = out_sostav.ID_DOC,
                    id_station_on_kis = out_sostav.ST_OUT_ST != null ? (int)out_sostav.ST_OUT_ST : 0,
                    way_num_kis = out_sostav.N_PUT_OUT_ST != null ? (int)out_sostav.N_PUT_OUT_ST : 0,
                    napr = out_sostav.NAPR_OUT_ST != null ? (int)out_sostav.NAPR_OUT_ST : 0,
                    id_station_from_kis = out_sostav.K_STAN != null ? (int)out_sostav.K_STAN : 0,
                    count_wagons = null,
                    count_set_wagons = null,
                    close = null,
                    close_user = null,
                    status = (int)status,
                    message = null,
                });
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveBufferOutputSostav(out_sostav={0}, status={1})", out_sostav.GetFieldsAndValue(), status), servece_owner, eventID);
                return -1;
            }
        }
        /// <summary>
        /// Проверить изменения в количестве вагонов в составе
        /// </summary>
        /// <param name="bos"></param>
        /// <param name="doc"></param>
        protected void CheckChangeExistBufferOutputSostav(BufferOutputSostav bos, int doc)
        {
            EFTKIS ef_tkis = new EFTKIS();
            EFWagons ef_wag = new EFWagons();
            int count_vag = ef_wag.GetCountSTPR1OutStVag(doc);
            // Количество вагонов изменено
            if (bos.count_wagons > 0 & count_vag > 0 & bos.count_wagons != count_vag)
            {
                // Изменим количество вагонов и отправим на переустановку вагонов
                bos.count_wagons = count_vag;
                bos.status = (int)statusSting.Update;
                bos.close = null;
                ef_tkis.SaveBufferOutputSostav(bos);
            }
        }
        /// <summary>
        /// Найти и удалить из списка Oracle_OutputSostav елемент doc
        /// </summary>
        /// <param name="list"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        protected bool DelExistBufferOutputSostav(ref List<BufferOutputSostav> list, int doc)
        {
            bool Result = false;
            int index = list.Count() - 1;
            while (index >= 0)
            {
                if (list[index].doc_num == doc)
                {
                    CheckChangeExistBufferOutputSostav(list[index], doc); // количество вагонов
                    list.RemoveAt(index);
                    Result = true;
                }
                index--;
            }
            return Result;
        }
        /// <summary>
        /// Проверяет списки NumVagStpr1OutStDoc и Oracle_OutputSostav на повторяющие документы, оставляет в списке NumVagStpr1OutStDoc - добавленные составы, Oracle_OutputSostav - удаленные из КИС составы
        /// </summary>
        /// <param name="list_is"></param>
        /// <param name="list_oos"></param>
        protected void DelExistBufferOutputSostav(ref List<NumVagStpr1OutStDoc> list_is, ref List<BufferOutputSostav> list_oos)
        {
            int index = list_is.Count() - 1;
            while (index >= 0)
            {
                if (DelExistBufferOutputSostav(ref list_oos, list_is[index].ID_DOC))
                {
                    list_is.RemoveAt(index);
                }
                index--;
            }
        }
        /// <summary>
        /// удалить строку состава отсутсвующего после переноса
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        protected int DeleteBufferOutputSostav(List<BufferOutputSostav> list)
        {
            EFRailCars ef_rc = new EFRailCars();
            EFTKIS ef_tkis = new EFTKIS();
            
            if (list == null || list.Count == 0) return 0;
            int delete = 0;
            int errors = 0;
            foreach (BufferOutputSostav bos in list)
            {
                // TODO: Сделать код удаления вагонов из RailWay
                // Удалим вагоны из системы RailCars
                //transfer_rc.DeleteVagonsToDocOutput(or_os.DocNum);
                // TODO: Сделать код удаления вагонов из RailWay

                bos.close = DateTime.Now;
                bos.close_user = System.Environment.UserDomainName + @"\" + System.Environment.UserName;
                bos.status = (int)statusSting.Delete;
                int res = ef_tkis.SaveBufferOutputSostav(bos);
                if (res > 0) delete++;
                if (res < 1)
                {
                    String.Format("Ошибка выполнения метода DeleteBufferOutputSostav, удаление строки:{0} из таблицы состояния переноса составов (по прибытию) по данным системы КИС", bos.id).WriteError(servece_owner, this.eventID);
                    errors++;
                }
            }
            String.Format("Таблица состояния переноса составов (по прибытию) по данным системы КИС, определенно удаленных в системе КИС {0} составов, удалено из таблицы {1}, ошибок удаления {2}.", list.Count(), delete, errors).WriteWarning(servece_owner, this.eventID);
            return delete;
        }
        /// <summary>
        /// Добавить новые составы появившиеся после переноса
        /// </summary>
        /// <param name="list"></param>
        protected int InsertBufferOutputSostav(List<NumVagStpr1OutStDoc> list)
        {
            if (list == null | list.Count == 0) return 0;
            int insers = 0;
            int errors = 0;
            foreach (NumVagStpr1OutStDoc out_s in list)
            {
                int res = SaveBufferOutputSostav(out_s, statusSting.Insert);
                if (res > 0) insers++;
                if (res < 1)
                {
                    String.Format("Ошибка выполнения метода InsertBufferOutputSostav, добавления строки состава по данным системы КИС(№ документа:{0}, дата:{1}) в таблицу состояния переноса составов по прибытию BufferOutputSostav", out_s.ID_DOC, out_s.DATE_OUT_ST).WriteError(servece_owner, this.eventID);
                    errors++;
                }
            }
            String.Format("Таблица состояния переноса составов (по отправке) по данным системы КИС, определенно добавленных в системе КИС {0} составов, добавлено в таблицу {1}, ошибок добавления {2}.", list.Count(), insers, errors).WriteWarning(servece_owner, this.eventID);
            return insers;
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

        #region Операции с таблицей переноса составов из КИС [BufferArrivalSostav]
        public int CopyBufferArrivalSostavOfKIS() {
            return CopyBufferArrivalSostavOfKIS(this.day_control_arrival_kis_add_data);
        }
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
                DateTime? lastDT = ef_tkis.GetLastDateTimeBufferArrivalSostav();
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
            if (list_noClose == null || list_noClose.Count() == 0) return 0;
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
                ArrivalCars mt_list = GetIDSostavCloseMTCar(natur, num_vag, dt_amkr, pv.WES_GR);
                if (mt_list != null)
                {
                    idsostav = mt_list.IDSostav;
                }
                // Проверим есть строка в справочнеке САП поставки
                sap_trans.CheckingWagonToSAPSupply(idsostav, pv, mt_list);

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
        /// <summary>
        /// Вернуть МТ строку с вагоном закрытую по натурке
        /// </summary>
        /// <param name="natur"></param>
        /// <param name="num_vag"></param>
        /// <param name="dt_amkr"></param>
        /// <param name="wes_gr"></param>
        /// <returns></returns>
        protected ArrivalCars GetIDSostavCloseMTCar(int natur, int num_vag, DateTime dt_amkr, decimal? wes_gr)
        {
            EFMetallurgTrans ef_mt = new EFMetallurgTrans();
            // Сделать отметку на МТ о принятии вагона
            int res_close_arrival = ef_mt.CloseArrivalCarsOfDocWeight(natur, num_vag, dt_amkr, wes_gr);
            if (res_close_arrival <= 0)
            {
                res_close_arrival = ef_mt.CloseArrivalCarsOfDocDay(natur, num_vag, dt_amkr, 1);
                // если нет строки закрыть последннюю строку с этим вагоном код закрытия системой (-1)
                if (res_close_arrival <= 0)
                {
                    ef_mt.CloseArrivalCars(num_vag, dt_amkr, -1);
                }
            }
            int res_close_approaches = ef_mt.CloseApproachesCarsOfDocWeight(natur, num_vag, dt_amkr, wes_gr);
            if (res_close_approaches <= 0)
            {
                res_close_approaches = ef_mt.CloseApproachesCarsOfDocDay(natur, num_vag, dt_amkr, 1);
            }
            ArrivalCars mt_list = ef_mt.GetArrivalCarsToNatur(natur, num_vag, dt_amkr, 15);
            return mt_list;
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
                ResultTransfers result = new ResultTransfers(set_wagons.Count(), null, 0, null, 0, 0);
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


        #region ПЕРЕНОС ПРИБЫВШИХ СОСТАВОВ НА СТАНЦИЮ С ВАГОНАМИ ИЗ СИСТЕМЫ КИС в RailWay (перенос по прибытию)

        #region Операции с таблицей переноса составов из КИС [BufferInputSostav]
        public int CopyBufferInputSostavOfKIS() {
            return CopyBufferInputSostavOfKIS(this.day_control_input_kis_add_data);
        }
        /// <summary>
        /// Перенос информации о составах по внутреним станциям по прибытию
        /// </summary>
        /// <returns></returns>
        public int CopyBufferInputSostavOfKIS(int day_control_ins)
        {
            EFTKIS ef_tkis = new EFTKIS();
            EFWagons ef_wag = new EFWagons();
            int errors = 0;
            int normals = 0;
            // список новых составов в системе КИС
            List<NumVagStpr1InStDoc> list_newsostav = new List<NumVagStpr1InStDoc>();
            // список уже перенесенных в RailWay составов в системе КИС (с учетом периода контроля dayControllingAddNatur)
            List<NumVagStpr1InStDoc> list_oldsostav = new List<NumVagStpr1InStDoc>();
            // список уже перенесенных в RailWay составов (с учетом периода контроля dayControllingAddNatur)
            List<BufferInputSostav> list_inputsostav = new List<BufferInputSostav>();
            try
            {
                // Считаем дату последненго состава
                DateTime? lastDT = ef_tkis.GetLastDateTimeBufferInputSostav();
                if (lastDT != null)
                {
                    // Данные есть получим новые
                    list_newsostav = ef_wag.GetSTPR1InStDoc(((DateTime)lastDT).AddSeconds(1), DateTime.Now, false).ToList();
                    list_oldsostav = ef_wag.GetSTPR1InStDoc(((DateTime)lastDT).AddDays(day_control_ins * -1), ((DateTime)lastDT).AddSeconds(1), false).ToList();
                    list_inputsostav = ef_tkis.GetBufferInputSostav(((DateTime)lastDT).AddDays(day_control_ins * -1), ((DateTime)lastDT).AddSeconds(1)).ToList();
                }
                else
                {
                    // Таблица пуста получим первый раз
                    list_newsostav = ef_wag.GetSTPR1InStDoc(DateTime.Now.AddDays(day_control_ins * -1), DateTime.Now, false).ToList();
                }
                // Переносим информацию по новым составам
                if (list_newsostav.Count() > 0)
                {
                    foreach (NumVagStpr1InStDoc inps in list_newsostav)
                    {

                        int res = SaveBufferInputSostav(inps, statusSting.Normal);
                        if (res > 0) normals++;
                        if (res < 1) { errors++; }
                    }
                    string mess_new = String.Format("Таблица состояния переноса составов (перенос по прибытию), по данным системы КИС (определено новых составов:{0}, перенесено:{1}, ошибок переноса:{2}).", list_newsostav.Count(), normals, errors);
                    mess_new.WriteInformation(servece_owner, this.eventID);
                    if (list_newsostav.Count() > 0) mess_new.WriteEvents(errors > 0 ? EventStatus.Error : EventStatus.Ok, servece_owner, eventID);
                }
                // Обновим информацию по составам которые были перенесены
                if (list_oldsostav.Count() > 0 & list_inputsostav.Count() > 0)
                {
                    List<NumVagStpr1InStDoc> list_is = new List<NumVagStpr1InStDoc>();
                    list_is = list_oldsostav;
                    List<BufferInputSostav> list_ois = new List<BufferInputSostav>();
                    list_ois = list_inputsostav.Where(a => a.status != (int)statusSting.Delete).ToList();
                    DelExistBufferInputSostav(ref list_is, ref list_ois);
                    int ins = InsertBufferInputSostav(list_is);
                    int del = DeleteBufferInputSostav(list_ois);

                    string mess_upd = String.Format("Таблица состояния переноса сосставов (по прибытию) по данным системы КИС (определено добавленных составов:{0}, перенесено:{1}, определено удаленных составов:{2}, удалено:{3}).",
                    list_is.Count(), ins, list_ois.Count(), del);
                    mess_upd.WriteInformation(servece_owner, this.eventID);
                    if (list_is.Count() > 0 | list_is.Count() > 0) mess_upd.WriteEvents(EventStatus.Ok, servece_owner, eventID);
                    normals += ins;
                }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CopyBufferInputSostavOfKIS(day_control_ins={0})", day_control_ins), servece_owner, eventID);
                return -1;
            }
            return normals;
        }
        #endregion
        /// <summary>
        /// Перенести в прибытие снаций (системы RailWay) все составы по прибытию из системы КИС 
        /// </summary>
        /// <returns></returns>
        public int TransferArrivalOfKISInput()
        {
            EFTKIS ef_tkis = new EFTKIS();
            EFRW.Concrete.EFRailWay ef_rw = new EFRW.Concrete.EFRailWay();
            int close = 0;
            IQueryable<BufferInputSostav> list_noClose = ef_tkis.GetBufferInputSostavNoClose();
            if (list_noClose == null || list_noClose.Count() == 0) return 0;
            foreach (BufferInputSostav bis in list_noClose.ToList())
            {
                try
                {
                    string mess_put = String.Format("Состав (№ документа: {0}, дата: {1}, ID строки: {2}) по прибытию из внутренних станций по данным системы КИС", bis.doc_num, bis.datetime, bis.id);
                    BufferInputSostav kis_inp_sostav = new BufferInputSostav();
                    kis_inp_sostav = bis;
                    //Закрыть состав
                    if (kis_inp_sostav.count_wagons != null & kis_inp_sostav.count_set_wagons != null & kis_inp_sostav.count_wagons == kis_inp_sostav.count_set_wagons)
                    {
                        kis_inp_sostav.close = DateTime.Now;
                        kis_inp_sostav.close_user = System.Environment.UserDomainName + @"\" + System.Environment.UserName;
                        int res_close = ef_tkis.SaveBufferInputSostav(kis_inp_sostav);
                        mess_put += " - перенесен и закрыт";
                        mess_put.WriteEvents(res_close > 0 ? EventStatus.Ok : EventStatus.Error, servece_owner, eventID);
                        close++;
                    }
                    if (ef_rw.IsRulesTransferOfKis(bis.id_station_from_kis, bis.id_station_on_kis, EFRW.Concrete.EFRailWay.typeSendTransfer.kis_input))
                    {
                        if (this.transfer_input_kis)
                        {
                            int res_put = TransferArrivalToStation(ref kis_inp_sostav);
                        }
                        else
                        {
                            kis_inp_sostav.close = DateTime.Now;
                            kis_inp_sostav.close_user = System.Environment.UserDomainName + @"\" + System.Environment.UserName;
                            int res_close = ef_tkis.SaveBufferInputSostav(kis_inp_sostav);
                            mess_put += " - пропущен, процесс переноса отключен";
                            mess_put.WriteEvents(res_close > 0 ? EventStatus.Ok : EventStatus.Error, servece_owner, eventID);
                            close++;
                        }
                    }
                    else { 
                        kis_inp_sostav.close = DateTime.Now;
                        kis_inp_sostav.close_user = System.Environment.UserDomainName + @"\" + System.Environment.UserName;
                        int res_close = ef_tkis.SaveBufferInputSostav(kis_inp_sostav);
                        mess_put += " - пропущен по несоответствию правил и закрыт.";
                        mess_put.WriteEvents(res_close > 0 ? EventStatus.Ok : EventStatus.Error, servece_owner, eventID);
                        close++;                    
                    }
                    // Поставим состав на станции АМКР системы RailCars
                    

                }
                catch (Exception e)
                {
                    e.WriteErrorMethod(String.Format("TransferArrivalOfKISInput()"), servece_owner, eventID);
                    return -1;
                }
            }

            return close;
        }

        public int TransferArrivalToStation(ref BufferInputSostav bis)
        {
            RCReference rc_ref = new RCReference();
            EFWagons ef_wag = new EFWagons();

            string mess_transf = String.Format("состава (№ документа: {0}, дата: {1}, ID строки: {2}) по прибытию из внутренних станций по данным системы КИС", bis.doc_num, bis.datetime, bis.id);
            string mess_sostav_err = "Ошибка переноса " + mess_transf;
            // Определим станцию отправитель
            int? id_stations_from = rc_ref.DefinitionIDStations(bis.id_station_from_kis, bis.way_num_kis);
            if (id_stations_from == null)
            {
                String.Format(mess_sostav_err + " - ID станции отправки: {0} не определён в справочнике системы RailWay", bis.id_station_from_kis).WriteError(servece_owner, eventID);
                return (int)errorTransfer.no_stations;
            }
            if (id_stations_from == 42) id_stations_from = 20; // Коррекция Промышленная Керамет -> 'это промышленная
            // Определим станцию получатель
            int? id_stations_on = rc_ref.DefinitionIDStations(bis.id_station_on_kis, null);
            if (id_stations_on == null)
            {
                String.Format(mess_sostav_err + " - ID станции прибытия: {0} не определён в справочнике системы RailWay", bis.id_station_on_kis).WriteError(servece_owner, eventID);
                return (int)errorTransfer.no_stations;
            }
            if (id_stations_on == 42) id_stations_on = 20; // Коррекция Промышленная Керамет -> 'это промышленная
            // Определим путь на станции
            int? id_ways = rc_ref.DefinitionIDWays((int)id_stations_from);
            if (id_ways == null)
            {
                String.Format(mess_sostav_err + " - ID пути: {0} станции: {1} не определён в справочнике системы RailWay", bis.way_num_kis, id_stations_from).WriteError(servece_owner, eventID);
                return (int)errorTransfer.no_ways;
            }
            // Формирование общего списка вагонов и постановка их на путь станции прибытия
            List<NumVagStpr1InStVag> list_pv = ef_wag.GetSTPR1InStVag(bis.doc_num, bis.napr == 2 ? true : false).ToList();
            bis.count_wagons = list_pv.Count(); // Определим количество вагонов
            return TransferArrivalSostavToStation(ref bis, (int)id_stations_from, (int)id_stations_on, (int)id_ways);

        }

        public int TransferArrivalSostavToStation(ref BufferInputSostav bis, int id_stations_from, int id_stations_on, int id_ways)
        {
            EFWagons ef_wag = new EFWagons();
            EFTKIS ef_tkis = new EFTKIS();
            string mess_transf = String.Format("состава (№ документа: {0}, дата: {1}, ID строки: {2}) по прибытию из внутренних станций (со станции: {3}, с пути : {4}, на станцию:{5}) по данным системы КИС.", bis.doc_num, bis.datetime, bis.id, id_stations_from, id_ways, id_stations_on);
            string mess_sostav = "Перенос " + mess_transf;
            string mess_sostav_err = "переноса " + mess_transf;
            if (bis == null) return 0;
            try
            {
                if (bis.count_wagons == null) return 0; // нет вагонов для копирования
                // Обнавляем вагоны
                if (bis.count_wagons != null & bis.count_set_wagons != null & bis.count_wagons != bis.count_set_wagons)
                {
                    // Удалим вагоны из системы RailCars
                    //DeleteVagonsToDocInput(bis.doc_num);
                    bis.count_set_wagons = null;
                }
                if (bis.count_wagons == 0) return 0; // нет вагонов для копирования
                if (bis.count_wagons != null & bis.count_set_wagons != null & bis.count_wagons == bis.count_set_wagons) return 0; // нет вагонов для копирования
                // Ставим вагоны
                ResultTransfers result = new ResultTransfers((int)bis.count_wagons, 0, null, null, 0, 0);
                // Ставим вагоны на путь станции
                IQueryable<NumVagStpr1InStVag> list = ef_wag.GetSTPR1InStVag(bis.doc_num, bis.napr == 2 ? true : false);
                bis.message = null;
                string mesage_error = null;
                foreach (NumVagStpr1InStVag vag_is in list)
                {
                    mesage_error += vag_is.N_VAG.ToString() + ":";
                    if (result.SetResultInsert(TransferArrivalCarsToStation(bis.doc_num, bis.datetime, vag_is, id_stations_from, id_stations_on, id_ways, ref mesage_error)))
                    {

                    }
                    mesage_error += result.result.ToString() + ";";
                }
                bis.message = mesage_error;
                bis.count_set_wagons = result.ResultInsert;

                mess_sostav += String.Format(" Определено для переноса: {0} вагонов, перенесено: {1} вагонов, ранее перенесено: {2} вагонов, ошибок переноса {3}.", bis.count_wagons, result.inserts, result.skippeds, result.errors);

                mess_sostav.WriteInformation(servece_owner, eventID);
                if (bis.count_wagons > 0) { mess_sostav.WriteEvents(result.errors > 0 ? EventStatus.Error : EventStatus.Ok, servece_owner, eventID); }
                // Сохранить результат и вернуть код
                if (ef_tkis.SaveBufferInputSostav(bis) < 0) return (int)errorTransfer.global; else return result.ResultInsert;

            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("TransferArrivalSostavToStation(bis={0}, id_stations_from={1}, id_stations_on={2}, id_ways={3})", bis.GetFieldsAndValue(), id_stations_from, id_stations_on, id_ways), servece_owner, eventID);
                return (int)errorTransfer.global;
            }
        }

        protected int TransferArrivalCarsToStation(int num_doc, DateTime dt_input, NumVagStpr1InStVag vag_is, int id_stations_from, int id_stations_on, int? id_ways, ref string mesage_error)
        {
            EFWagons ef_wag = new EFWagons();
            EFRailCars ef_rc = new EFRailCars();
            RCReference rc_ref = new RCReference();
            EFSAP ef_sap = new EFSAP();
            EFMetallurgTrans ef_mt = new EFMetallurgTrans();
            SAPTransfer sap_trans = new SAPTransfer();

            string mess = String.Format("вагона №:{0}, принадлежащего составу (№ док: {1}, дата: {2}) со станции (код станции:{3}, код пути:{4}) в прибытие на станцию (код станции:{5})",
                vag_is.N_VAG, num_doc, dt_input, id_stations_from, id_ways, id_stations_on);
            string mess_vag = "Перенос " + mess;
            string mess_vag_err = "переноса " + mess;
            string mess_vag_err1 = "Ошибка переноса " + mess;
            string mess_vag_err2 = "Ошибка удаления цепочки " + mess;

            try
            {
                // Найдем вагон в натурном листе PromNatHist
                PromNatHist pnh = ef_wag.GetNatHistOfVagonLess(vag_is.N_VAG, dt_input, true).FirstOrDefault();
                if (pnh == null)
                {
                    String.Format(mess_vag_err1 + ", код ошибки:{0}", errorTransfer.no_wagon_is_nathist.ToString()).WriteError(servece_owner, eventID);
                    return (int)errorTransfer.no_wagon_is_nathist;
                }
                DateTime dt_amkr = (DateTime)Filters.GetDateTime(pnh);
                int id_sostav = ef_sap.GetDefaultIDSAPIncSupply();
                int? id_operarion = null;
                DateTime? data_uz = null;
                List<VAGON_OPERATIONS> list_wag = ef_rc.GetVagonsOperationsOfPresentWay(pnh.N_VAG).ToList();
                if (list_wag != null && list_wag.Count() > 0)
                {
                    VAGON_OPERATIONS wag = list_wag[0]; // берем последнюю не закрытую запись
                    id_operarion = wag.id_oper;
                    if (wag.n_natur != pnh.N_NATUR)
                    {
                        // Проверка данные по вагону устарели 
                        if (wag.dt_amkr!=null && wag.dt_amkr > dt_amkr) {
                            return (int)errorTransfer.old_wagon_is_nathist;
                        }
                        ArrivalCars mt_list = GetIDSostavCloseMTCar(pnh.N_NATUR, vag_is.N_VAG, dt_amkr, pnh.WES_GR);
                        if (mt_list != null)
                        {
                            id_sostav = mt_list.IDSostav;
                            data_uz = mt_list.DateOperation;
                        }
                        // Проверим есть строка в справочнеке САП поставки
                        sap_trans.CheckingWagonToSAPSupply(id_sostav, pnh, mt_list);
                    }
                    else
                    {
                        // это нужный вагон
                        id_sostav = (int)wag.IDSostav;
                        data_uz = wag.dt_uz;
                    }
                }
                else
                {
                    List<VAGON_OPERATIONS> list_wag_arr = ef_rc.GetVagonsOperationsOfPresentArrival(pnh.N_VAG).ToList();
                    if (list_wag_arr != null && list_wag_arr.Count() > 0)
                    {
                        VAGON_OPERATIONS wag = list_wag_arr[0]; // берем последнюю не закрытую запись
                        id_operarion = wag.id_oper;
                        if (wag.n_natur == pnh.N_NATUR & wag.st_lock_id_stat == id_stations_on)
                        {
                            return 0;
                        }
                        // Закрыть все прибытия вагонов на путях
                        if (list_wag_arr != null && list_wag_arr.Count() > 0)
                        {
                            // Уберем из прибытия вагоны
                            foreach (VAGON_OPERATIONS wag_close in list_wag_arr)
                            {
                                wag_close.is_present = 0;
                                wag_close.is_hist = 1;
                                wag_close.st_lock_id_stat = null;
                                wag_close.st_lock_order = null;
                                wag_close.st_lock_side = null;
                                wag_close.st_lock_train = null;
                                wag_close.st_shop = null;
                                wag_close.st_gruz_front = null;
                                wag_close.st_lock_locom1 = null;
                                wag_close.st_lock_locom2 = null;
                                ef_rc.SaveVAGON_OPERATIONS(wag_close);
                            }
                        }
                    }
                    // Открытой операции по вагонам нет, ставим по данным КИС в прибытие станции
                    ArrivalCars mt_list = GetIDSostavCloseMTCar(pnh.N_NATUR, vag_is.N_VAG, dt_amkr, pnh.WES_GR);
                    if (mt_list != null)
                    {
                        id_sostav = mt_list.IDSostav;
                        data_uz = mt_list.DateOperation;
                    }
                    // Проверим есть строка в справочнеке САП поставки
                    sap_trans.CheckingWagonToSAPSupply(id_sostav, pnh, mt_list);
                }

                int id_wagon = rc_ref.DefinitionSetIDVagon(vag_is.N_VAG, dt_amkr, -1, null, pnh.N_NATUR, false); // определить id вагона (если нет создать новый id? локоматив -1)
                //
                int? id_gruz = rc_ref.DefinitionIDGruzs(null, vag_is.GR_IN_ST);
                if (id_gruz == null)
                {
                    String.Format(mess_vag_err1 + ", код ошибки:{0}, STPR1_IN_ST_VAG.GR_IN_ST:{1}.", errorTransfer.no_gruz.ToString(), vag_is.GR_IN_ST).WriteError(servece_owner, eventID);
                    mesage_error += ((int)errorTransfer.no_gruz).ToString() + ",";
                }
                //
                int res = ef_rc.InsertInputVagon(id_sostav, num_doc, pnh.N_NATUR, id_wagon, vag_is.N_VAG, data_uz != null ? (DateTime)data_uz : dt_amkr, dt_amkr, dt_input, id_stations_from, vag_is.N_IN_ST, id_gruz,
                        vag_is.REM_IN_ST, id_stations_on, -1, vag_is.GODN_IN_ST, id_operarion, id_ways);
                // Если вагон поставился в прибытие, закроем предыдущую запись
                if (res >= 0)
                {
                    // Закрыть все присутсвия вагонов на путях
                    if (list_wag != null && list_wag.Count() > 0)
                    {
                        foreach (VAGON_OPERATIONS wag_close in list_wag)
                        {
                            wag_close.is_present = 0;
                            ef_rc.SaveVAGON_OPERATIONS(wag_close);
                        }
                    }
                }
                if (res < 0)
                {
                    String.Format(mess_vag_err1 + ", код ошибки:{0}.", res).WriteError(servece_owner, eventID);
                }
                return res;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("TransferArrivalCarsToStation(doc={0}, dt_input={1}, vag_is={2}, id_stations_from={3}, id_stations_on={4}, id_ways={5}, mesage_error={6})",
                    num_doc, dt_input, vag_is.GetFieldsAndValue(), id_stations_from, id_stations_on, id_ways, mesage_error), servece_owner, eventID);
                return (int)errorTransfer.global;
            }
        }
        #endregion

        #region ПЕРЕНОС ОТПРАВЛЕНЫХ СОСТАВОВ НА СТАНЦИЮ С ВАГОНАМИ ИЗ СИСТЕМЫ КИС в RailWay (перенос по отправке)

        #region Операции с таблицей переноса составов из КИС [BufferOutputSostav]
        public int CopyBufferOutputSostavOfKIS() {
            return CopyBufferOutputSostavOfKIS(this.day_control_output_kis_add_data, this.status_control_output_kis);
        }
        /// <summary>
        /// Перенос информации о составах по внутреним станциям по отправке
        /// </summary>
        /// <param name="day_control_ins"></param>
        /// <returns></returns>
        public int CopyBufferOutputSostavOfKIS(int day_control_ins, bool is_status)
        {
            EFTKIS ef_tkis = new EFTKIS();
            EFWagons ef_wag = new EFWagons();
            int errors = 0;
            int normals = 0;
            // список новых составов в системе КИС
            List<NumVagStpr1OutStDoc> list_newsostav = new List<NumVagStpr1OutStDoc>();
            // список уже перенесенных в RailWay составов в системе КИС (с учетом периода контроля dayControllingAddNatur)
            List<NumVagStpr1OutStDoc> list_oldsostav = new List<NumVagStpr1OutStDoc>();
            // список уже перенесенных в RailWay составов (с учетом периода контроля dayControllingAddNatur)
            List<BufferOutputSostav> list_outputsostav = new List<BufferOutputSostav>();
            try
            {
                // Считаем дату последненго состава
                DateTime? lastDT = ef_tkis.GetLastDateTimeBufferOutputSostav();
                if (lastDT != null)
                {
                    // Данные есть получим новые
                    list_newsostav = ef_wag.GetSTPR1OutStDoc(((DateTime)lastDT).AddSeconds(1), DateTime.Now, false).ToList();
                    list_oldsostav = ef_wag.GetSTPR1OutStDoc(((DateTime)lastDT).AddDays(day_control_ins * -1), ((DateTime)lastDT).AddSeconds(1), false).ToList();
                    list_outputsostav = ef_tkis.GetBufferOutputSostav(((DateTime)lastDT).AddDays(day_control_ins * -1), ((DateTime)lastDT).AddSeconds(1)).ToList();
                }
                else
                {
                    // Таблица пуста получим первый раз
                    list_newsostav = ef_wag.GetSTPR1OutStDoc(DateTime.Now.AddDays(day_control_ins * -1), DateTime.Now, false).ToList();
                }
                // Переносим информацию по новым составам
                if (list_newsostav.Count() > 0)
                {
                    foreach (NumVagStpr1OutStDoc inps in list_newsostav)
                    {
                        if (is_status && inps.STATUS != 1) break;
                        int res = SaveBufferOutputSostav(inps, statusSting.Normal);
                        if (res > 0) normals++;
                        if (res < 1) { errors++; }
                        
                    }
                    string mess_new = String.Format("Таблица состояния переноса составов (перенос по отправке), по данным системы КИС (определено новых составов:{0}, перенесено:{1}, ошибок переноса:{2}).", list_newsostav.Count(), normals, errors);
                    mess_new.WriteInformation(servece_owner, this.eventID);
                    if (list_newsostav.Count() > 0) mess_new.WriteEvents(errors > 0 ? EventStatus.Error : EventStatus.Ok, servece_owner, eventID);
                }
                // Обновим информацию по составам которые были перенесены
                if (list_oldsostav.Count() > 0 & list_outputsostav.Count() > 0)
                {
                    List<NumVagStpr1OutStDoc> list_os = new List<NumVagStpr1OutStDoc>();
                    list_os = list_oldsostav;
                    List<BufferOutputSostav> list_oos = new List<BufferOutputSostav>();
                    list_oos = list_outputsostav.Where(a => a.status != (int)statusSting.Delete).ToList();
                    DelExistBufferOutputSostav(ref list_os, ref list_oos);
                    int ins = InsertBufferOutputSostav(list_os);
                    int del = DeleteBufferOutputSostav(list_oos);
                    string mess_upd = String.Format("Таблица состояния переноса сосставов (по прибытию) по данным системы КИС (определено добавленных составов:{0}, перенесено:{1}, определено удаленных составов:{2}, удалено:{3}).",
                        list_os.Count(), ins, list_oos.Count(), del);
                    mess_upd.WriteInformation(servece_owner, this.eventID);
                    if (list_os.Count() > 0 | list_os.Count() > 0) mess_upd.WriteEvents(EventStatus.Ok, servece_owner, eventID);
                    normals += ins;
                }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CopyBufferOutputSostavOfKIS(day_control_ins={0})", day_control_ins), servece_owner, eventID);
                return -1;
            }

            return normals;
        }
        #endregion

        public int TransferArrivalOfKISOutput()
        {
            EFTKIS ef_tkis = new EFTKIS();
            EFRW.Concrete.EFRailWay ef_rw = new EFRW.Concrete.EFRailWay();
            int close = 0;
            IQueryable<BufferOutputSostav> list_noClose = ef_tkis.GetBufferOutputSostavNoClose();
            if (list_noClose == null || list_noClose.Count() == 0) return 0;
            foreach (BufferOutputSostav bos in list_noClose.ToList())
            {
                try
                {
                    string mess_put = String.Format("Состав (№ документа: {0}, дата: {1}, ID строки: {2}) по отправке из внутренних станций по данным системы КИС", bos.doc_num, bos.datetime, bos.id);
                    BufferOutputSostav kis_out_sostav = new BufferOutputSostav();
                    kis_out_sostav = bos;
                    //Закрыть состав
                    if (kis_out_sostav.count_wagons != null & kis_out_sostav.count_set_wagons != null & kis_out_sostav.count_wagons == kis_out_sostav.count_set_wagons)
                    {
                        kis_out_sostav.close = DateTime.Now;
                        kis_out_sostav.close_user = System.Environment.UserDomainName + @"\" + System.Environment.UserName;
                        int res_close = ef_tkis.SaveBufferOutputSostav(kis_out_sostav);
                        mess_put += " - перенесен и закрыт";
                        mess_put.WriteEvents(res_close > 0 ? EventStatus.Ok : EventStatus.Error, servece_owner, eventID);
                        close++;
                    }
                    if (ef_rw.IsRulesTransferOfKis(bos.id_station_from_kis, bos.id_station_on_kis, EFRW.Concrete.EFRailWay.typeSendTransfer.kis_output))
                    {
                        if (this.transfer_input_kis)
                        {
                            int res_put = TransferArrivalToStation(ref kis_out_sostav);
                        }
                        else
                        {
                            kis_out_sostav.close = DateTime.Now;
                            kis_out_sostav.close_user = System.Environment.UserDomainName + @"\" + System.Environment.UserName;
                            int res_close = ef_tkis.SaveBufferOutputSostav(kis_out_sostav);
                            mess_put += " - пропущен, процесс переноса отключен";
                            mess_put.WriteEvents(res_close > 0 ? EventStatus.Ok : EventStatus.Error, servece_owner, eventID);
                            close++;
                        }
                    }
                    else { 
                        kis_out_sostav.close = DateTime.Now;
                        kis_out_sostav.close_user = System.Environment.UserDomainName + @"\" + System.Environment.UserName;
                        int res_close = ef_tkis.SaveBufferOutputSostav(kis_out_sostav);
                        mess_put += " - пропущен по несоответствию правил и закрыт.";
                        mess_put.WriteEvents(res_close > 0 ? EventStatus.Ok : EventStatus.Error, servece_owner, eventID);
                        close++;                    
                    }
                    // Поставим состав на станции АМКР системы RailCars
                    

                }
                catch (Exception e)
                {
                    e.WriteErrorMethod(String.Format("TransferArrivalOfKISOutput()"), servece_owner, eventID);
                    return -1;
                }
            }

            return close;
        }

        public int TransferArrivalToStation(ref BufferOutputSostav bos)
        {
            RCReference rc_ref = new RCReference();
            EFWagons ef_wag = new EFWagons();

            string mess_transf = String.Format("состава (№ документа: {0}, дата: {1}, ID строки: {2}) по отправке из внутренних станций по данным системы КИС", bos.doc_num, bos.datetime, bos.id);
            string mess_sostav_err = "Ошибка переноса " + mess_transf;
            // Определим станцию отправитель
            int? id_stations_from = rc_ref.DefinitionIDStations(bos.id_station_from_kis, bos.way_num_kis);
            if (id_stations_from == null)
            {
                String.Format(mess_sostav_err + " - ID станции отправки: {0} не определён в справочнике системы RailWay", bos.id_station_from_kis).WriteError(servece_owner, eventID);
                return (int)errorTransfer.no_stations;
            }
            if (id_stations_from == 42) id_stations_from = 20; // Коррекция Промышленная Керамет -> 'это промышленная
            // Определим станцию получатель
            int? id_stations_on = rc_ref.DefinitionIDStations(bos.id_station_on_kis, null);
            if (id_stations_on == null)
            {
                String.Format(mess_sostav_err + " - ID станции прибытия: {0} не определён в справочнике системы RailWay", bos.id_station_on_kis).WriteError(servece_owner, eventID);
                return (int)errorTransfer.no_stations;
            }
            if (id_stations_on == 42) id_stations_on = 20; // Коррекция Промышленная Керамет -> 'это промышленная
            // Определим путь на станции
            int? id_ways = rc_ref.DefinitionIDWays((int)id_stations_from);
            if (id_ways == null)
            {
                String.Format(mess_sostav_err + " - ID пути: {0} станции: {1} не определён в справочнике системы RailWay", bos.way_num_kis, id_stations_from).WriteError(servece_owner, eventID);
                return (int)errorTransfer.no_ways;
            }
            // Формирование общего списка вагонов и постановка их на путь станции прибытия
            List<NumVagStpr1OutStVag> list_pv = ef_wag.GetSTPR1OutStVag(bos.doc_num, bos.napr == 2 ? true : false).ToList();
            bos.count_wagons = list_pv.Count(); // Определим количество вагонов
            return TransferArrivalSostavToStation(ref bos, (int)id_stations_from, (int)id_stations_on, (int)id_ways);

        }

        public int TransferArrivalSostavToStation(ref BufferOutputSostav bos, int id_stations_from, int id_stations_on, int id_ways)
        {
            EFWagons ef_wag = new EFWagons();
            EFTKIS ef_tkis = new EFTKIS();
            string mess_transf = String.Format("состава (№ документа: {0}, дата: {1}, ID строки: {2}) по прибытию из внутренних станций (со станции: {3}, с пути : {4}, на станцию:{5}) по данным системы КИС.", bos.doc_num, bos.datetime, bos.id, id_stations_from, id_ways, id_stations_on);
            string mess_sostav = "Перенос " + mess_transf;
            string mess_sostav_err = "переноса " + mess_transf;
            if (bos == null) return 0;
            try
            {
                if (bos.count_wagons == null) return 0; // нет вагонов для копирования
                // Обнавляем вагоны
                if (bos.count_wagons != null & bos.count_set_wagons != null & bos.count_wagons != bos.count_set_wagons)
                {
                    // Удалим вагоны из системы RailCars
                    //DeleteVagonsToDocInput(bis.doc_num);
                    bos.count_set_wagons = null;
                }
                if (bos.count_wagons == 0) return 0; // нет вагонов для копирования
                if (bos.count_wagons != null & bos.count_set_wagons != null & bos.count_wagons == bos.count_set_wagons) return 0; // нет вагонов для копирования
                // Ставим вагоны
                ResultTransfers result = new ResultTransfers((int)bos.count_wagons, 0, null, null, 0, 0);
                // Ставим вагоны на путь станции
                IQueryable<NumVagStpr1OutStVag> list = ef_wag.GetSTPR1OutStVag(bos.doc_num, bos.napr == 2 ? true : false);
                bos.message = null;
                string mesage_error = null;
                foreach (NumVagStpr1OutStVag vag_is in list)
                {
                    mesage_error += vag_is.N_VAG.ToString() + ":";
                    if (result.SetResultInsert(TransferArrivalCarsToStation(bos.doc_num, bos.datetime, vag_is, id_stations_from, id_stations_on, id_ways, ref mesage_error)))
                    {

                    }
                    mesage_error += result.result.ToString() + ";";
                }
                bos.message = mesage_error;
                bos.count_set_wagons = result.ResultInsert;

                mess_sostav += String.Format(" Определено для переноса: {0} вагонов, перенесено: {1} вагонов, ранее перенесено: {2} вагонов, ошибок переноса {3}.", bos.count_wagons, result.inserts, result.skippeds, result.errors);

                mess_sostav.WriteInformation(servece_owner, eventID);
                if (bos.count_wagons > 0) { mess_sostav.WriteEvents(result.errors > 0 ? EventStatus.Error : EventStatus.Ok, servece_owner, eventID); }
                // Сохранить результат и вернуть код
                if (ef_tkis.SaveBufferOutputSostav(bos) < 0) return (int)errorTransfer.global; else return result.ResultInsert;

            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("TransferArrivalSostavToStation(bis={0}, id_stations_from={1}, id_stations_on={2}, id_ways={3})", bos.GetFieldsAndValue(), id_stations_from, id_stations_on, id_ways), servece_owner, eventID);
                return (int)errorTransfer.global;
            }
        }

        protected int TransferArrivalCarsToStation(int num_doc, DateTime dt_input, NumVagStpr1OutStVag vag_os, int id_stations_from, int id_stations_on, int? id_ways, ref string mesage_error)
        {
            EFWagons ef_wag = new EFWagons();
            EFRailCars ef_rc = new EFRailCars();
            RCReference rc_ref = new RCReference();
            EFSAP ef_sap = new EFSAP();
            EFMetallurgTrans ef_mt = new EFMetallurgTrans();
            SAPTransfer sap_trans = new SAPTransfer();

            string mess = String.Format("вагона №:{0}, принадлежащего составу (№ док: {1}, дата: {2}) со станции (код станции:{3}, код пути:{4}) в прибытие на станцию (код станции:{5})",
                vag_os.N_VAG, num_doc, dt_input, id_stations_from, id_ways, id_stations_on);
            string mess_vag = "Перенос " + mess;
            string mess_vag_err = "переноса " + mess;
            string mess_vag_err1 = "Ошибка переноса " + mess;
            string mess_vag_err2 = "Ошибка удаления цепочки " + mess;

            try
            {
                // Найдем вагон в натурном листе PromNatHist
                PromNatHist pnh = ef_wag.GetNatHistOfVagonLess(vag_os.N_VAG, dt_input, true).FirstOrDefault();
                if (pnh == null)
                {
                    String.Format(mess_vag_err1 + ", код ошибки:{0}", errorTransfer.no_wagon_is_nathist.ToString()).WriteError(servece_owner, eventID);
                    return (int)errorTransfer.no_wagon_is_nathist;
                }
                DateTime dt_amkr = (DateTime)Filters.GetDateTime(pnh);
                int id_sostav = ef_sap.GetDefaultIDSAPIncSupply();
                int? id_operarion = null;
                DateTime? data_uz = null;
                List<VAGON_OPERATIONS> list_wag = ef_rc.GetVagonsOperationsOfPresentWay(pnh.N_VAG).ToList();
                if (list_wag != null && list_wag.Count() > 0)
                {
                    VAGON_OPERATIONS wag = list_wag[0]; // берем последнюю не закрытую запись
                    id_operarion = wag.id_oper;
                    if (wag.n_natur != pnh.N_NATUR)
                    {
                        // Проверка данные по вагону устарели 
                        if (wag.dt_amkr != null && wag.dt_amkr > dt_amkr)
                        {
                            return (int)errorTransfer.old_wagon_is_nathist;
                        }
                        ArrivalCars mt_list = GetIDSostavCloseMTCar(pnh.N_NATUR, vag_os.N_VAG, dt_amkr, pnh.WES_GR);
                        if (mt_list != null)
                        {
                            id_sostav = mt_list.IDSostav;
                            data_uz = mt_list.DateOperation;
                        }
                        // Проверим есть строка в справочнеке САП поставки
                        sap_trans.CheckingWagonToSAPSupply(id_sostav, pnh, mt_list);
                    }
                    else
                    {
                        // это нужный вагон
                        id_sostav = (int)wag.IDSostav;
                        data_uz = wag.dt_uz;
                    }
                }
                else
                {
                    List<VAGON_OPERATIONS> list_wag_arr = ef_rc.GetVagonsOperationsOfPresentArrival(pnh.N_VAG).ToList();
                    if (list_wag_arr != null && list_wag_arr.Count() > 0)
                    {
                        VAGON_OPERATIONS wag = list_wag_arr[0]; // берем последнюю не закрытую запись
                        id_operarion = wag.id_oper;
                        if (wag.n_natur == pnh.N_NATUR & wag.st_lock_id_stat == id_stations_on)
                        {
                            return 0;
                        }
                        // Закрыть все прибытия вагонов на путях
                        if (list_wag_arr != null && list_wag_arr.Count() > 0)
                        {
                            // Уберем из прибытия вагоны
                            foreach (VAGON_OPERATIONS wag_close in list_wag_arr)
                            {
                                wag_close.is_present = 0;
                                wag_close.is_hist = 1;
                                wag_close.st_lock_id_stat = null;
                                wag_close.st_lock_order = null;
                                wag_close.st_lock_side = null;
                                wag_close.st_lock_train = null;
                                wag_close.st_shop = null;
                                wag_close.st_gruz_front = null;
                                wag_close.st_lock_locom1 = null;
                                wag_close.st_lock_locom2 = null;
                                ef_rc.SaveVAGON_OPERATIONS(wag_close);
                            }
                        }
                    }
                    // Открытой опреации по вагонам нет, ставим по данным КИС в прибытие станции
                    ArrivalCars mt_list = GetIDSostavCloseMTCar(pnh.N_NATUR, vag_os.N_VAG, dt_amkr, pnh.WES_GR);
                    if (mt_list != null)
                    {
                        id_sostav = mt_list.IDSostav;
                        data_uz = mt_list.DateOperation;
                    }
                    // Проверим есть строка в справочнеке САП поставки
                    sap_trans.CheckingWagonToSAPSupply(id_sostav, pnh, mt_list);
                }

                int id_wagon = rc_ref.DefinitionSetIDVagon(vag_os.N_VAG, dt_amkr, -1, null, pnh.N_NATUR, false); // определить id вагона (если нет создать новый id? локоматив -1)
                //
                int? id_gruz = rc_ref.DefinitionIDGruzs(null, vag_os.GR_OUT_ST);
                if (id_gruz == null)
                {
                    String.Format(mess_vag_err1 + ", код ошибки:{0}, STPR1_IN_ST_VAG.GR_IN_ST:{1}.", errorTransfer.no_gruz.ToString(), vag_os.GR_OUT_ST).WriteError(servece_owner, eventID);
                    mesage_error += ((int)errorTransfer.no_gruz).ToString() + ",";
                }
                //
                int res = ef_rc.InsertInputVagon(id_sostav, num_doc, pnh.N_NATUR, id_wagon, vag_os.N_VAG, data_uz != null ? (DateTime)data_uz : dt_amkr, dt_amkr, dt_input, id_stations_from, vag_os.N_OUT_ST, id_gruz,
                        vag_os.REM_IN_ST, id_stations_on, -1, vag_os.GODN_OUT_ST, id_operarion, id_ways);
                // Если вагон поставился в прибытие, закроем предыдущую запись
                if (res >= 0)
                {
                    // Закрыть все присутсвия вагонов на путях
                    if (list_wag != null && list_wag.Count() > 0)
                    {
                        foreach (VAGON_OPERATIONS wag_close in list_wag)
                        {
                            wag_close.is_present = 0;
                            ef_rc.SaveVAGON_OPERATIONS(wag_close);
                        }
                    }
                }
                if (res < 0)
                {
                    String.Format(mess_vag_err1 + ", код ошибки:{0}.", res).WriteError(servece_owner, eventID);
                }
                return res;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("TransferArrivalCarsToStation(doc={0}, dt_input={1}, vag_is={2}, id_stations_from={3}, id_stations_on={4}, id_ways={5}, mesage_error={6})",
                    num_doc, dt_input, vag_os.GetFieldsAndValue(), id_stations_from, id_stations_on, id_ways, mesage_error), servece_owner, eventID);
                return (int)errorTransfer.global;
            }
        }
        #endregion


        #region Закрыть перенос составов
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int CloseBufferArrivalSostav()
        {

            EFTKIS ef_tkis = new EFTKIS();
            int close = 0;
            int skip = 0;
            int error = 0;

            List<BufferArrivalSostav> list = new List<BufferArrivalSostav>();
            list = ef_tkis.GetBufferArrivalSostavNoClose().OrderBy(c => c.datetime).ToList();
            foreach (BufferArrivalSostav bas in list.ToList())
            {
                int res = CloseBufferArrivalSostav(bas);
                if (res > 0) { close++; }
                if (res == 0) { skip++; }
                if (res < 0) { error++; }
            }
            string mess = String.Format("Проверка буфера переноса вагонов из системы КИС в систему RailCars - выполнена, определено {0} не перенесенных состава, закрыто автоматически {1}, пропущено {2}, ошибки закрытия {3}.",
                list != null ? list.Count() : 0, close, skip, error);
            mess.WriteInformation(servece_owner, this.eventID);
            if (list != null && list.Count() > 0) { mess.WriteEvents(error > 0 ? EventStatus.Error : EventStatus.Ok, servece_owner, eventID); }
            return close;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bas"></param>
        /// <returns></returns>
        public int CloseBufferArrivalSostav(BufferArrivalSostav bas)
        {

            EFWagons ef_wag = new EFWagons();
            EFTKIS ef_tkis = new EFTKIS();
            List<PromVagon> list_pv = ef_wag.GetVagon(bas.natur, bas.id_station_kis, bas.day, bas.month, bas.year).ToList();
            List<PromNatHist> list_pnh = ef_wag.GetNatHist(bas.natur, bas.id_station_kis, bas.day, bas.month, bas.year).ToList();
            // Ситуация-1. Проверим наличие вагонов в системе КИС (Могли отменить натурку данных нет в таблицах PromVagons, NanHist)
            if ((list_pv == null || list_pv.Count() == 0) & (list_pnh == null || list_pnh.Count() == 0))
            {
                // данных нет в двух таблицах
                //if (bas.list_wagons != null) { 
                // вагоны были выставленны
                // удалим вагоны по этому составу, но проверим если была сосздана новая натурка с этими вагонми тогда сделаем коррекцию вагонов по прибытию
                return DeleteSostavBufferArrivalSostav(bas.id);
                //}
            }
            // Ситуация-2.  Проверим наличие вагонов в системе КИС (Могли отменить натурку убрать данные из таблиц NanHist)
            if ((list_pv != null && list_pv.Count() > 0) & (list_pnh == null || list_pnh.Count() == 0))
            {
                return DeleteSostavBufferArrivalSostav(bas.id);
            }
            // Ситуация-3.  Не все вагоны обновились (нет годности и цеха))
            if ((list_pv != null && list_pv.Count() > 0) & (list_pnh != null && list_pnh.Count() > 0))
            {
                //return DeleteSostavBufferArrivalSostav(bas.id);
                if (bas.message != null)
                {
                    if (bas.message.Contains(((int)errorTransfer.no_stations).ToString())) return 0;
                    if (bas.message.Contains(((int)errorTransfer.no_ways).ToString())) return 0;
                    if (bas.message.Contains(((int)errorTransfer.no_wagons).ToString())) return 0;
                    if (bas.message.Contains(((int)errorTransfer.no_wagon_is_list).ToString())) return 0;
                    if (bas.message.Contains(((int)errorTransfer.no_wagon_is_nathist).ToString())) return 0;
                    // Даем срок закрыть данные
                    if (bas.datetime < DateTime.Now.AddDays(-1 * this.day_range_arrival_kis_copy))
                    {
                        bas.close = DateTime.Now;
                        bas.close_user = System.Environment.UserDomainName + @"\" + System.Environment.UserName;
                        return ef_tkis.SaveArrivalSostav(bas);
                    }
                }
            }
            // пропускаем
            return 0;
        }
        #endregion

        #region Коррекция системы переноса
        /// <summary>
        /// Удалить составы по ранее ошибочно созданной натурке (натурку создали затем убрали и эти вагоны занеслись по новой натурке)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteSostavBufferArrivalSostav(int id)
        {
            EFTKIS ef_tkis = new EFTKIS();
            EFRailCars ef_rc = new EFRailCars();
            EFMetallurgTrans ef_mt = new EFMetallurgTrans();

            int del_rc = 0;
            int err_del_rc = 0;
            int del_sap = 0;
            int upd_mt = 0;
            try
            {
                BufferArrivalSostav del_bas = ef_tkis.GetBufferArrivalSostav(id);
                BufferArrivalSostav new_bas = null;
                List<BufferArrivalSostav> not_close_list = ef_tkis.GetBufferArrivalSostav().Where(b => b.datetime >= del_bas.datetime & b.id != del_bas.id).ToList();
                foreach (BufferArrivalSostav bas in not_close_list)
                {
                    if (bas.list_wagons != null && del_bas.list_wagons != null && bas.list_wagons.Trim() == del_bas.list_wagons.Trim())
                    {
                        new_bas = bas;
                        break;
                    }
                }
                string mess = String.Format("Коррекция данных (Удалена натурка {0} от {1}, создана новая {2} от {3}). ",
                    del_bas.natur, del_bas.datetime, (new_bas != null ? (int?)new_bas.natur : null), (new_bas != null ? (DateTime?)new_bas.datetime : null));

                List<VAGON_OPERATIONS> list_del_wagons = ef_rc.GetVagonsOperationsToNatur(del_bas.natur, del_bas.datetime).ToList();
                foreach (VAGON_OPERATIONS wag in list_del_wagons)
                {
                    int? way = wag.id_way;
                    int idsostav = (int)wag.IDSostav;
                    VAGON_OPERATIONS res_del = ef_rc.DeleteVAGON_OPERATIONS(wag.id_oper);
                    if (res_del != null)
                    {
                        del_rc++;
                        //String.Format(mess + "Из системы RailCars - удален вагон {0}", wag.num_vagon).WriteEvents(servece_owner, eventID);
                        if (way != null)
                        {
                            ef_rc.OffSetCars((int)way, 1);
                        }
                        if (wag.IDSostav < 0)
                        {
                            // idsostav отрицательный
                            EFSAP ef_sap = new EFSAP();
                            ef_sap.DeleteSAPIncSupplySostav(idsostav);
                            del_sap++;
                        }
                        if (wag.IDSostav > 0)
                        {
                            // idsostav положительный
                            if (new_bas != null)
                            {
                                int new_natur = new_bas.natur;
                                DateTime date_amkr = new_bas.datetime;
                                ArrivalCars arr_car = ef_mt.GetArrivalCars((int)wag.IDSostav);
                                if (arr_car != null)
                                {
                                    arr_car.NumDocArrival = new_natur;
                                    arr_car.Arrival = date_amkr;
                                    arr_car.UserName = System.Environment.UserDomainName + @"\" + System.Environment.UserName;
                                    ef_mt.SaveArrivalCars(arr_car);
                                    upd_mt++;
                                }
                            }
                        }
                    }
                    else
                    {
                        err_del_rc++;
                    }
                }
                if (err_del_rc == 0)
                {
                    del_bas.close = DateTime.Now;
                    del_bas.close_user = System.Environment.UserDomainName + @"\" + System.Environment.UserName;
                    del_bas.status = (int)statusSting.Delete;
                    ef_tkis.SaveArrivalSostav(del_bas);
                }
                String.Format(mess + "Из системы RailCars - удалено {0} вагонов, ошибок удаления {1}, из справочника САП вхю пост. удалено {2} строк, в прибытии МТ Скорректировано {3} строки."
                    , del_rc, err_del_rc, del_sap, upd_mt).WriteEvents(err_del_rc > 0 ? EventStatus.Error : EventStatus.Ok, servece_owner, eventID);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("DeleteSostavBufferArrivalSostav(id={0})", id), servece_owner, eventID);
                return (int)errorTransfer.global;
            }
            return del_rc;
        }
        #endregion
    }
}
