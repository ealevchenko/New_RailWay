using MessageLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libClass;
using EFKIS.Entities;
using EFKIS.Concrete;
using System.Globalization;
using EFRW.Entities;
using RW;
using EFRW.Concrete;
using EFMT.Concrete;
using EFKIS.Abstract;

namespace KIS
{
    public class KIS_RW_Transfer : KISTransfer
    {
        private eventID eventID = eventID.KIS_RWTransfer;
        //protected service servece_owner = service.Null;
        bool log_detali = false;
        
        public KIS_RW_Transfer()
            : base()
        {

        }

        public KIS_RW_Transfer(service servece_owner)
            : base(servece_owner)
        {
            this.servece_owner = servece_owner;
        }

        #region Таблица переноса составов из КИС [RWBufferArrivalSostav]
        /// <summary>
        /// Сохранить состав из КИС
        /// </summary>
        /// <param name="ps"></param>
        /// <returns></returns>
        protected int SaveRWBufferArrivalSostav(PromSostav ps, statusSting status)
        {
            EFTKIS ef_tkis = new EFTKIS();
            try
            {
                DateTime DT = DateTime.Parse(ps.D_DD.ToString() + "-" + ps.D_MM.ToString() + "-" + ps.D_YY.ToString() + " " + ps.T_HH.ToString() + ":" + ps.T_MI.ToString() + ":00", CultureInfo.CreateSpecificCulture("ru-RU"));
                return ef_tkis.SaveRWBufferArrivalSostav(new RWBufferArrivalSostav()
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
                e.WriteErrorMethod(String.Format("SaveRWBufferArrivalSostav(ps={0}, status={1})", ps.GetFieldsAndValue(), status), servece_owner, eventID);
                return -1;
            }
        }
        /// <summary>
        /// Найти и удалить из списка ArrivalSostav елемент natur
        /// </summary>
        /// <param name="list"></param>
        /// <param name="natur"></param>
        /// <returns></returns>
        protected bool DelExistRWBufferArrivalSostav(ref List<RWBufferArrivalSostav> list, int natur)
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
                e.WriteErrorMethod(String.Format("DelExistRWBufferArrivalSostav(list={0}, natur={1})", list, natur), servece_owner, eventID);
            }
            return Result;
        }
        /// <summary>
        /// Проверяет списки PromSostav и ArrivalSostav на повторяющие натурные листы, оставляет в списке PromSostav - добавленные составы, ArrivalSostav - удаленные из КИС составы
        /// </summary>
        /// <param name="list_ps"></param>
        /// <param name="list_as"></param>
        protected void DelExistRWBufferArrivalSostav(ref List<PromSostav> list_ps, ref List<RWBufferArrivalSostav> list_as)
        {
            try
            {
                int index = list_ps.Count() - 1;
                while (index >= 0)
                {
                    if (DelExistRWBufferArrivalSostav(ref list_as, list_ps[index].N_NATUR))
                    {
                        list_ps.RemoveAt(index);
                    }
                    index--;
                }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("DelExistRWBufferArrivalSostav(list_ps={0}, list_as={1})", list_ps, list_as), servece_owner, eventID);
            }
        }
        /// <summary>
        /// Удалить ранее перенесеные составы
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        protected int DeleteRWBufferArrivalSostav(List<RWBufferArrivalSostav> list)
        {
            EFTKIS ef_tkis = new EFTKIS();
            try
            {
                if (list == null | list.Count == 0) return 0;
                int delete = 0;
                int errors = 0;
                foreach (RWBufferArrivalSostav or_as in list)
                {
                    // Удалим вагоны из системы RailCars
                    // TODO: Сделать код удаления вагонов из RailWay
                    //transfer_rc.DeleteVagonsToNaturList(or_as.NaturNum, or_as.DateTime);
                    or_as.close = DateTime.Now;
                    or_as.close_user = System.Environment.UserDomainName + @"\" + System.Environment.UserName;
                    or_as.status = (int)statusSting.Delete;
                    int res = ef_tkis.SaveRWBufferArrivalSostav(or_as);
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
                e.WriteErrorMethod(String.Format("DeleteRWBufferArrivalSostav(list={0})", list), servece_owner, eventID);
                return -1;
            }
        }
        /// <summary>
        /// Добавить новые составы появившиеся после переноса
        /// </summary>
        /// <param name="list"></param>
        protected int InsertRWBufferArrivalSostav(List<PromSostav> list)
        {
            try
            {
                if (list == null | list.Count == 0) return 0;
                int insers = 0;
                int errors = 0;
                foreach (PromSostav ps in list)
                {
                    int res = SaveRWBufferArrivalSostav(ps, statusSting.Insert);
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
                e.WriteErrorMethod(String.Format("InsertRWBufferArrivalSostav(list={0})", list), servece_owner, eventID);
                return -1;
            }
        }
        #endregion

        #region Таблица переноса составов из КИС [RWBufferSendingSostav]
        /// <summary>
        /// Сохранить состав из КИС
        /// </summary>
        /// <param name="ps"></param>
        /// <returns></returns>
        protected int SaveRWBufferSendingSostav(PromSostav ps, statusSting status)
        {
            EFTKIS ef_tkis = new EFTKIS();
            try
            {
                DateTime DT = DateTime.Parse(ps.D_DD.ToString() + "-" + ps.D_MM.ToString() + "-" + ps.D_YY.ToString() + " " + ps.T_HH.ToString() + ":" + ps.T_MI.ToString() + ":00", CultureInfo.CreateSpecificCulture("ru-RU"));
                return ef_tkis.SaveRWBufferSendingSostav(new RWBufferSendingSostav()
                {
                    id = 0,
                    datetime = DT,
                    day = (int)ps.D_DD,
                    month = (int)ps.D_MM,
                    year = (int)ps.D_YY,
                    hour = (int)ps.T_HH,
                    minute = (int)ps.T_MI,
                    natur = ps.N_NATUR, 
                    id_station_from_kis = (int)ps.K_ST, 
                    id_station_on_kis = (int)ps.K_ST_PR,
                    count_nathist = null,
                    count_set_nathist = null,
                    close = null,
                    close_user = null,
                    status = (int)status,
                    list_no_set_wagons = null,
                    message = null,
                });
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveRWBufferSendingSostav(ps={0}, status={1})", ps.GetFieldsAndValue(), status), servece_owner, eventID);
                return -1;
            }
        }
        /// <summary>
        /// Найти и удалить из списка ArrivalSostav елемент natur
        /// </summary>
        /// <param name="list"></param>
        /// <param name="natur"></param>
        /// <returns></returns>
        protected bool DelExistRWBufferSendingSostav(ref List<RWBufferSendingSostav> list, int natur)
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
                e.WriteErrorMethod(String.Format("DelExistRWBufferSendingSostav(list={0}, natur={1})", list, natur), servece_owner, eventID);
            }
            return Result;
        }
        /// <summary>
        /// Проверяет списки PromSostav и ArrivalSostav на повторяющие натурные листы, оставляет в списке PromSostav - добавленные составы, ArrivalSostav - удаленные из КИС составы
        /// </summary>
        /// <param name="list_ps"></param>
        /// <param name="list_as"></param>
        protected void DelExistRWBufferSendingSostav(ref List<PromSostav> list_ps, ref List<RWBufferSendingSostav> list_as)
        {
            try
            {
                int index = list_ps.Count() - 1;
                while (index >= 0)
                {
                    if (DelExistRWBufferSendingSostav(ref list_as, list_ps[index].N_NATUR))
                    {
                        list_ps.RemoveAt(index);
                    }
                    index--;
                }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("DelExistRWBufferSendingSostav(list_ps={0}, list_as={1})", list_ps, list_as), servece_owner, eventID);
            }
        }
        /// <summary>
        /// Удалить ранее перенесеные составы
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        protected int DeleteRWBufferSendingSostav(List<RWBufferSendingSostav> list)
        {
            EFTKIS ef_tkis = new EFTKIS();
            try
            {
                if (list == null | list.Count == 0) return 0;
                int delete = 0;
                int errors = 0;
                foreach (RWBufferSendingSostav bss in list)
                {
                    // TODO: Сделать код удаления вагонов из RailWay
                    //transfer_rc.DeleteVagonsToNaturList(or_as.NaturNum, or_as.DateTime);
                    bss.close = DateTime.Now;
                    bss.close_user = System.Environment.UserDomainName + @"\" + System.Environment.UserName;
                    bss.status = (int)statusSting.Delete;
                    int res = ef_tkis.SaveRWBufferSendingSostav(bss);
                    if (res > 0) delete++;
                    if (res < 0)
                    {
                        String.Format("Ошибка выполнения метода DeleteRWBufferSendingSostav, удаление строки:{0} из таблицы состояния переноса составов сданных на УЗ по данным системы КИС", bss.id).WriteError(servece_owner, this.eventID);
                        errors++;
                    }
                }
                String.Format("Таблица состояния переноса составов зданных на УЗ по данным системы КИС, определенно удаленных в системе КИС {0} составов, удалено из таблицы {1}, ошибок удаления {2}.", list.Count(), delete, errors).WriteWarning(servece_owner, this.eventID);
                return delete;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("DeleteRWBufferSendingSostav(list={0})", list), servece_owner, eventID);
                return -1;
            }
        }
        /// <summary>
        /// Добавить новые составы появившиеся после переноса
        /// </summary>
        /// <param name="list"></param>
        protected int InsertRWBufferSendingSostav(List<PromSostav> list)
        {
            try
            {
                if (list == null | list.Count == 0) return 0;
                int insers = 0;
                int errors = 0;
                foreach (PromSostav ps in list)
                {
                    int res = SaveRWBufferSendingSostav(ps, statusSting.Insert);
                    if (res > 0) insers++;
                    if (res < 0)
                    {
                        String.Format("Ошибка выполнения метода InsertRWBufferSendingSostav, добавления строки состава по данным системы КИС(натурный лист:{0}, дата:{1}-{2}-{3} {4}:{5}) в таблицу состояния переноса составов зашедших на АМКР по данным системы КИС", ps.N_NATUR, ps.D_DD, ps.D_MM, ps.D_YY, ps.T_HH, ps.T_MI).WriteError(servece_owner, this.eventID);
                        errors++;
                    }
                }
                String.Format("Таблица состояния переноса составов сданных на УЗ по данным системы КИС, определенно добавленных в системе КИС {0} составов, добавлено в таблицу {1}, ошибок добавления {2}.", list.Count(), insers, errors).WriteWarning(servece_owner, this.eventID);
                return insers;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("InsertRWBufferSendingSostav(list={0})", list), servece_owner, eventID);
                return -1;
            }
        }
        #endregion

        #region дополнительные методы к RWOperation
        /// <summary>
        /// Поставить вагон в сисему RailWay по данным PromVagon
        /// </summary>
        /// <param name="prom_vagon"></param>
        /// <param name="set_operation"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        public Cars SetCarsToRailWay(PromVagon prom_vagon, RW.RWOperation.SetCarOperation set_operation, IOperation operation)
        {
            try
            {
                RWOperation rw_oper = new RWOperation(this.servece_owner);
                CarsInpDelivery delivery = CreateCarsInpDelivery(prom_vagon);
                return rw_oper.SetCarsToRailWay(prom_vagon.N_VAG, prom_vagon.N_NATUR * -1, delivery, set_operation, operation);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SetCarsToRailWay(prom_vagon={0}, set_operation={1}, operation={2})", prom_vagon.GetFieldsAndValue(), set_operation, operation), servece_owner, eventID);
                return null;
            }
        }
        /// <summary>
        /// Создать справочник SAP Входящие поставки по данным PromVagon
        /// </summary>
        /// <param name="prom_vagon"></param>
        /// <returns></returns>
        public CarsInpDelivery CreateCarsInpDelivery(PromVagon prom_vagon)
        {
            try
            {
                RWReference rw_ref = new RWReference(true);
                RWOperation rw_oper = new RWOperation(this.servece_owner);
                DateTime dt_oper = DateTime.Parse(prom_vagon.D_PR_DD.ToString() + "-" + prom_vagon.D_PR_MM.ToString() + "-" + prom_vagon.D_PR_YY.ToString() + " " + prom_vagon.T_PR_HH.ToString() + ":" + prom_vagon.T_PR_MI.ToString() + ":00", CultureInfo.CreateSpecificCulture("ru-RU"));
                // Определим код груза
                return rw_oper.CreateCarsInpDelivery(prom_vagon.N_VAG,
                    (prom_vagon.N_NATUR * -1),
                    dt_oper,
                    "-",
                    prom_vagon.NPP,
                    prom_vagon != null && prom_vagon.KOD_STRAN != null ? (int)prom_vagon.KOD_STRAN : 0,
                    rw_ref.GetCorrectCodeETSNGOfKis(prom_vagon.K_GR),
                    prom_vagon != null && prom_vagon.WES_GR != null ? (float)prom_vagon.WES_GR : 0,
                    7932);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CreateCarsInpDelivery(prom_vagon={0})", prom_vagon.GetFieldsAndValue()), servece_owner, eventID);
                return null;
            }
        }
        #endregion

        #region дополнительные методы к EFWagons
        /// <summary>
        /// 
        /// </summary>
        /// <param name="natur"></param>
        /// <param name="num_vag"></param>
        /// <param name="dt_amkr"></param>
        /// <param name="id_station_kis"></param>
        /// <returns></returns>
        public PromNatHist GetCorrectNatHist(int natur, int num_vag, DateTime dt_amkr, int id_station_kis) {
            EFWagons ef_wag = new EFWagons();
            PromNatHist pnh = ef_wag.GetNatHist(natur, id_station_kis, dt_amkr.Day, dt_amkr.Month, dt_amkr.Year, num_vag);
                if (pnh == null & id_station_kis == 18)
                {
                    // Если промышленная, попробовать Промышленная-керамет
                    pnh = ef_wag.GetNatHist(natur, 81, dt_amkr.Day, dt_amkr.Month, dt_amkr.Year, num_vag);
                }
                return pnh;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="natur"></param>
        /// <param name="num_vag"></param>
        /// <param name="dt_amkr"></param>
        /// <param name="id_station_kis"></param>
        /// <returns></returns>
        public PromVagon GetCorrectVagon(int natur, int num_vag, DateTime dt_amkr, int id_station_kis)
        {
            EFWagons ef_wag = new EFWagons();
            PromVagon pv = ef_wag.GetVagon(natur, id_station_kis, dt_amkr.Day, dt_amkr.Month, dt_amkr.Year, num_vag);
            if (pv == null & id_station_kis == 18)
            {
                // Если промышленная, попробовать Промышленная-керамет
                pv = ef_wag.GetVagon(natur, 81, dt_amkr.Day, dt_amkr.Month, dt_amkr.Year, num_vag);
            }
                return pv;
        }
        #endregion

        #region ПЕРЕНОС И ОБНАВЛЕНИЕ ВАГОНОВ ИЗ СИСТЕМЫ КИС в СИСТЕМУ RailWAY
        /// <summary>
        /// Обновить таблицу буфер прибывающих составов из станций УЗ по системы КИС
        /// </summary>
        /// <returns></returns>
        public int CopyBufferArrivalSostavOfKIS()
        {
            int res_rw = CopyBufferArrivalSostavOfKIS(this.day_control_arrival_kis_add_data);
            return res_rw;
        }
        /// <summary>
        /// Обновить таблицу буфер отправленных составов на станции УЗ по системы КИС
        /// </summary>
        /// <returns></returns>
        public int CopyBufferSendingSostavOfKIS()
        {
            int res_rw = CopyBufferSendingSostavOfKIS(this.day_control_sending_kis_add_data);
            return res_rw;
        }
        /// <summary>
        /// Обновить таблицу буфер прибывающих составов из станций УЗ по системы КИС
        /// </summary>
        /// <param name="day_control_add_natur"></param>
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
            List<RWBufferArrivalSostav> list_arrivalsostav = new List<RWBufferArrivalSostav>();
            try
            {
                // Считаем дату последненго состава
                DateTime? lastDT = ef_tkis.GetLastDateTimeRWBufferArrivalSostav();
                if (lastDT != null)
                {
                    // Данные есть получим новые
                    list_newsostav = ef_wag.GetInputPromSostav(((DateTime)lastDT).AddSeconds(1), DateTime.Now, false).ToList();
                    list_oldsostav = ef_wag.GetInputPromSostav(((DateTime)lastDT).AddDays(day_control_add_natur * -1), ((DateTime)lastDT).AddSeconds(1), false).ToList();
                    list_arrivalsostav = ef_tkis.GetRWBufferArrivalSostav(((DateTime)lastDT).AddDays(day_control_add_natur * -1), ((DateTime)lastDT).AddSeconds(1)).ToList();
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

                        int res = SaveRWBufferArrivalSostav(ps, statusSting.Normal);
                        if (res > 0) normals++;
                        if (res < 0) { errors++; }
                    }
                    string mess_new = String.Format("[RailWay] Таблица состояния переноса составов зашедших на АМКР по данным системы КИС (определено новых составов:{0}, перенесено:{1}, ошибок переноса:{2}).", list_newsostav.Count(), normals, errors);
                    mess_new.WriteInformation(servece_owner, this.eventID);
                    if (list_newsostav.Count() > 0) mess_new.WriteEvents(errors > 0 ? EventStatus.Error : EventStatus.Ok, servece_owner, eventID);
                }
                // Обновим информацию по составам которые были перенесены
                if (list_oldsostav.Count() > 0 & list_arrivalsostav.Count() > 0)
                {
                    List<PromSostav> list_ps = new List<PromSostav>();
                    list_ps = list_oldsostav;
                    List<RWBufferArrivalSostav> list_as = new List<RWBufferArrivalSostav>();
                    list_as = list_arrivalsostav.Where(a => a.status != (int)statusSting.Delete).ToList();
                    DelExistRWBufferArrivalSostav(ref list_ps, ref list_as);
                    int ins = InsertRWBufferArrivalSostav(list_ps);
                    int del = DeleteRWBufferArrivalSostav(list_as);
                    string mess_upd = String.Format("[RailWay] Таблица состояния переноса составов зашедших на АМКР по данным системы КИС (определено добавленных составов:{0}, перенесено:{1}, определено удаленных составов:{2}, удалено:{3}).",
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
        /// Обновить таблицу буфер отправленных составов на УЗ по системы КИС
        /// </summary>
        /// <param name="day_control_add_natur"></param>
        /// <returns></returns>
        public int CopyBufferSendingSostavOfKIS(int day_control_add_natur)
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
            List<RWBufferSendingSostav> list_sendingsostav = new List<RWBufferSendingSostav>();
            try
            {
                // Считаем дату последненго состава
                DateTime? lastDT = ef_tkis.GetLastDateTimeRWBufferSendingSostav();
                if (lastDT != null)
                {
                    // Данные есть получим новые
                    list_newsostav = ef_wag.GetOutputPromSostav(((DateTime)lastDT).AddSeconds(1), DateTime.Now, false).ToList();
                    list_oldsostav = ef_wag.GetOutputPromSostav(((DateTime)lastDT).AddDays(day_control_add_natur * -1), ((DateTime)lastDT).AddSeconds(1), false).ToList();
                    list_sendingsostav = ef_tkis.GetRWBufferSendingSostav(((DateTime)lastDT).AddDays(day_control_add_natur * -1), ((DateTime)lastDT).AddSeconds(1)).ToList();
                }
                else
                {
                    // Таблица пуста получим первый раз
                    list_newsostav = ef_wag.GetOutputPromSostav(DateTime.Now.AddDays(day_control_add_natur * -1), DateTime.Now, false).ToList();
                }
                // Переносим информацию по новым составам
                if (list_newsostav.Count() > 0)
                {
                    foreach (PromSostav ps in list_newsostav)
                    {

                        int res = SaveRWBufferSendingSostav(ps, statusSting.Normal);
                        if (res > 0) normals++;
                        if (res < 0) { errors++; }
                    }
                    string mess_new = String.Format("[RailWay] Таблица состояния переноса составов сданных на УЗ по данным системы КИС (определено новых составов:{0}, перенесено:{1}, ошибок переноса:{2}).", list_newsostav.Count(), normals, errors);
                    mess_new.WriteInformation(servece_owner, this.eventID);
                    if (list_newsostav.Count() > 0) mess_new.WriteEvents(errors > 0 ? EventStatus.Error : EventStatus.Ok, servece_owner, eventID);
                }
                // Обновим информацию по составам которые были перенесены
                if (list_oldsostav.Count() > 0 & list_sendingsostav.Count() > 0)
                {
                    List<PromSostav> list_ps = new List<PromSostav>();
                    list_ps = list_oldsostav;
                    List<RWBufferSendingSostav> list_ss = new List<RWBufferSendingSostav>();
                    list_ss = list_sendingsostav.Where(a => a.status != (int)statusSting.Delete).ToList();
                    DelExistRWBufferSendingSostav(ref list_ps, ref list_ss);
                    int ins = InsertRWBufferSendingSostav(list_ps);
                    int del = DeleteRWBufferSendingSostav(list_ss);
                    string mess_upd = String.Format("[RailWay] Таблица состояния переноса составов сданных на УЗ по данным системы КИС (определено добавленных составов:{0}, перенесено:{1}, определено удаленных составов:{2}, удалено:{3}).",
                    list_ps.Count(), ins, list_ss.Count(), del);
                    mess_upd.WriteInformation(servece_owner, this.eventID);
                    if (list_ps.Count() > 0 | list_ss.Count() > 0) mess_upd.WriteEvents(EventStatus.Ok, servece_owner, eventID);
                    normals += ins;
                }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CopyBufferSendingSostavOfKIS(day_control_add_natur={0})", day_control_add_natur), servece_owner, eventID);
                return -1;
            }
            return normals;
        }

        public int TransferArrivalKISToRailWay()
        {
            try
            {
                EFTKIS ef_tkis = new EFTKIS();
                int close = 0;
                IQueryable<RWBufferArrivalSostav> list_noClose = ef_tkis.GetRWBufferArrivalSostavNoClose();
                if (list_noClose == null || list_noClose.Count() == 0) return 0;
                foreach (RWBufferArrivalSostav bas in list_noClose.ToList())
                {
                    try
                    {
                        string mess_put = String.Format("Состав (натурный лист: {0}, дата: {1}, ID строки буфера переноса: {2}), прибывающий с УЗ на станцию АМКР (id_kis:{3}) по данным системы КИС", bas.natur, bas.datetime, bas.id, bas.id_station_kis);
                        RWBufferArrivalSostav bas_result = new RWBufferArrivalSostav();
                        bas_result = bas;
                        // 1. Поставим состав на путь станции АМКР системы RailWay
                        int res_put = SetWayRailWayOfKIS(ref bas_result);
                        // 2. Обновление составов на пути станции АМКР системы RailWay
                        int res_upd = UpdWayRailWayOfKIS(ref bas_result);

                        //Закрыть состав
                        if (bas_result.count_wagons != null & bas_result.count_nathist != null & bas_result.count_set_wagons != null & bas_result.count_set_nathist != null
                            & bas_result.count_wagons == bas_result.count_nathist & bas_result.count_wagons == bas_result.count_set_wagons & bas_result.count_wagons == bas_result.count_set_nathist)
                        {
                            bas_result.close = DateTime.Now;
                            bas_result.close_user = System.Environment.UserDomainName + @"\" + System.Environment.UserName;
                            int res_close = ef_tkis.SaveRWBufferArrivalSostav(bas_result);
                            mess_put += " - перенесен и закрыт";
                            mess_put.WriteEvents(res_close > 0 ? EventStatus.Ok : EventStatus.Error, servece_owner, eventID);
                            close++;
                        }
                    }
                    catch (Exception e)
                    {
                        e.WriteError(String.Format("Ошибка обработки строки буфера переноса состава (натурный лист: {0}, дата: {1}, ID строки буфера переноса: {2})",bas.natur, bas.datetime, bas.id), servece_owner, eventID);
                    }
                }
                return close;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("TransferArrivalOfKIS()"), servece_owner, eventID);
                return -1;
            }
        }

        #region ПОСТАВИМ НА ПУТЬ СИСТЕМЫ RAILWAY ПО ДАННЫМ PROM_VAG
        /// <summary>
        /// Принять вагоны состава на путь станции по данным КИС 
        /// </summary>
        /// <param name="bas_sostav"></param>
        /// <param name="first"></param>
        /// <param name="secondary"></param>
        /// <returns></returns>
        public int SetWayRailWayOfKIS(ref RWBufferArrivalSostav bas_sostav)
        {
            try
            {
                string mess_transf = String.Format("cостава (натурный лист: {0}, дата: {1}, ID строки буфера переноса: {2}), прибывающий с УЗ на станцию АМКР", 
                    bas_sostav.natur, bas_sostav.datetime, bas_sostav.id);
                string mess_transf1 = " по данным системы КИС.";
                string mess_arr_sostav = "Перенос " + mess_transf;
                string mess_error_arr_sostav = "Ошибка переноса " + mess_transf;

                RWReference rw_ref = new RWReference(base.servece_owner, true); // создавать содержимое справочника из данных КИС
                EFWagons ef_wag = new EFWagons();
                EFTKIS ef_tkis = new EFTKIS();

                int id_stations_rw = 0;
                int id_ways_rw = 0;

                id_stations_rw = rw_ref.GetIDStationsOfKIS(bas_sostav.id_station_kis);
                if (id_stations_rw <= 0)
                {
                    String.Format(mess_error_arr_sostav + mess_transf1 + " - ID станции: {0} не определён в справочнике системы RailWay", bas_sostav.id_station_kis).WriteError(servece_owner, eventID);
                    return (int)errorTransfer.no_stations;
                }
                if (id_stations_rw == 26) id_stations_rw = 27; // Коррекция Промышленная Керамет -> 'это промышленная
                // Определим путь на станции система RailCars
                id_ways_rw = rw_ref.GetIDDefaultWayOfStation(id_stations_rw, bas_sostav.way_num.ToString());
                if (id_ways_rw <= 0)
                {
                    String.Format(mess_error_arr_sostav + mess_transf1 + " - ID пути: {0} станции: {1} не определён в справочнике системы RailWay", bas_sostav.way_num, bas_sostav.id_station_kis).WriteError(servece_owner, eventID);
                    return (int)errorTransfer.no_ways;
                }

                // Формирование общего списка вагонов и постановка их на путь станции прибытия (строку корректируе основной)
                IBufferArrivalSostav ibas = bas_sostav;
                int res_set_list = SetListWagon(ref ibas);
                bas_sostav = (RWBufferArrivalSostav)ibas;
                if (bas_sostav.list_no_set_wagons == null & bas_sostav.list_wagons == null) return 0;

                if (res_set_list >= 0)
                {
                    List<int> set_wagons = new List<int>();
                    // Обнавляем вагоны
                    if (bas_sostav.count_set_wagons != null & bas_sostav.list_no_set_wagons != null)
                    {
                        set_wagons = GetWagonsToListInt(bas_sostav.list_no_set_wagons); // доствавим вагоны
                    }
                    // Ставим вагоны в первый раз
                    if (bas_sostav.count_set_wagons == null & bas_sostav.list_no_set_wagons == null & bas_sostav.list_wagons != null)
                    {
                        set_wagons = GetWagonsToListInt(bas_sostav.list_wagons); // поставим занаво
                    }
                    if (set_wagons.Count() == 0) return 0;
                    ResultTransfers result_first = new ResultTransfers(set_wagons.Count(), 0, null, null, 0, 0);
                    // Ставим вагоны на путь станции
                    bas_sostav.list_no_set_wagons = null;
                    foreach (int wag in set_wagons)
                    {
                        if (result_first.SetResultInsert(SetCarToWayRailWay(bas_sostav.natur, wag, bas_sostav.datetime, id_ways_rw)))
                        {
                            // Ошибка
                            bas_sostav.list_no_set_wagons += wag.ToString() + ";";
                        }
                    }
                    bas_sostav.count_set_wagons = bas_sostav.count_set_wagons == null ? result_first.ResultInsert : (int)bas_sostav.count_set_wagons + result_first.ResultInsert;
                    mess_arr_sostav += mess_transf1 + String.Format("По данным системы КИС, определено для переноса: {0} вагонов, перенесено: {1} вагонов, ранее перенесено: {2} вагонов, ошибок переноса {3}.",
                        set_wagons.Count(), result_first.inserts, result_first.skippeds, result_first.errors);
                    mess_arr_sostav.WriteInformation(servece_owner, eventID);
                    if (set_wagons.Count() > 0) { mess_arr_sostav.WriteEvents(result_first.errors > 0 ? EventStatus.Error : EventStatus.Ok, servece_owner, eventID); }
                    // Сохранить результат и вернуть код
                    if (ef_tkis.SaveRWBufferArrivalSostav(bas_sostav) < 0)
                    { return (int)errorTransfer.set_table_arrival_sostav; }
                    else { return result_first.ResultInsert; }
                }
                else
                {
                    return res_set_list; // вернуло ошибку
                }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SetWayRailWayOfKIS(orc_sostav={0})", bas_sostav.GetFieldsAndValue()), servece_owner, eventID);
                return (int)errorTransfer.global;
            }
        }
        /// <summary>
        /// Создать и поставить на путь входящий вагон по данным КИС таблица PromVagon
        /// </summary>
        /// <param name="natur"></param>
        /// <param name="num_vag"></param>
        /// <param name="dt_amkr"></param>
        /// <param name="id_way"></param>
        /// <param name="id_station_kis"></param>
        /// <returns></returns>
        public int SetCarKISToWayRailWay(int natur, int num_vag, DateTime dt_amkr, int id_way, int id_station_kis)
        {
            try
            {
                //EFWagons ef_wag = new EFWagons();
                RWOperation rw_oper = new RWOperation(this.servece_owner);

                // Получим информацию для заполнения вагона с учетом отсутствия данных в PromVagon
                //PromVagon pv = ef_wag.GetVagon(natur, id_station_kis, dt_amkr.Day, dt_amkr.Month, dt_amkr.Year, num_vag);
                //if (pv == null & id_station_kis == 18)
                //{
                //    // Если промышленная, попробовать Промышленная-керамет
                //    pv = ef_wag.GetVagon(natur, 81, dt_amkr.Day, dt_amkr.Month, dt_amkr.Year, num_vag);
                //}
                PromVagon pv = GetCorrectVagon(natur, num_vag, dt_amkr, id_station_kis);
                //PromNatHist pnh = ef_wag.GetNatHist(natur, id_station_kis, dt_amkr.Day, dt_amkr.Month, dt_amkr.Year, num_vag);
                //if (pnh == null & id_station_kis == 18)
                //{
                //    // Если промышленная, попробовать Промышленная-керамет
                //    pnh = ef_wag.GetNatHist(natur, 81, dt_amkr.Day, dt_amkr.Month, dt_amkr.Year, num_vag);
                //}
                PromNatHist pnh = GetCorrectNatHist(natur, num_vag, dt_amkr, id_station_kis);
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
                // Создать новый вагон по данным КИС 
                int res_car = 0;
                Cars new_car = SetCarsToRailWay(pv, rw_oper.OperationArrivalUZWay, new OperationArrivalUZWay(id_way, dt_amkr, dt_amkr, natur, null, pv.GODN));
                if (new_car != null)
                {
                    res_car = rw_oper.SaveChanges(new_car);
                }
                return res_car;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SetCarKISToWayRailWay()"), servece_owner, eventID);
                return (int)errorTransfer.global;
            }
        }
        /// <summary>
        /// Поставить вагон на путь станции системы RailWay
        /// </summary>
        /// <param name="natur"></param>
        /// <param name="num_vag"></param>
        /// <param name="dt_amkr"></param>
        /// <param name="id_way"></param>
        /// <returns></returns>
        public int SetCarToWayRailWay(int natur, int num_vag, DateTime dt_amkr, int id_way)
        {
            try
            {
                RWOperation rw_oper = new RWOperation(this.servece_owner);
                EFRailWay ef_rw = new EFRailWay();
                EFMetallurgTrans ef_mt = new EFMetallurgTrans();
                //EFWagons ef_wag = new EFWagons();

                Ways way = ef_rw.GetWays(id_way);
                Stations station = way.Stations;

                // Получим станции УЗ по которым можно получать вагоны на указаную станцию
                List<Stations> list_station_arrival_uz_to_station = ef_rw.GetArrivalStationsNodes(station.id).Where(s => s.Stations.station_uz == true).ToList().Select(s => s.Stations).ToList();
                // Получить список путей станций УЗ по которым можно получать вагоны на указаную станцию
                List<Ways> list_ways_arrival_uz_to_station = new List<Ways>();
                list_station_arrival_uz_to_station.ForEach(s => list_ways_arrival_uz_to_station.Add(ef_rw.GetWaysOfArrivalUZ(s.id)));
                // Получить список путей отправки и приема
                List<Ways> list_arrival_ways_uz = ef_rw.GetWays().Where(w => w.Stations.station_uz == true & w.num == "1").ToList();
                List<Ways> list_sending_ways_uz = ef_rw.GetWays().Where(w => w.Stations.station_uz == true & w.num == "2").ToList();
                // Получим последнюю открытую операцию по указанному вагону
                CarOperations last_operation = rw_oper.GetLastOpenOperation(rw_oper.IsOpenAllOperation(num_vag), true); // проверить вагон в системе 
                if (last_operation != null)
                {   // Вагон есть в системе 
                    Cars car = last_operation.Cars; // Определим вагон
                    // Вагон статит на пути станции УЗ (по которой возможен проход вагона на станцию АМКР) для отправки на станцию АМКР?
                    int id_way_car = last_operation.IsSetWay(list_ways_arrival_uz_to_station.Select(w => w.id).ToArray(), null);
                    if (id_way_car > 0)
                    { // Стоит в прибытии станции УЗ по которой можно получить вагон на станцию АМКР
                        //TODO: Дополнительно можно сделать проверку на интервал времени нахождения в прибытии
                        //if (car.dt_uz >= dt_amkr.AddDays(this.day_waiting_interval_cars * -1))

                        //car.dt_inp_amkr = dt_amkr;
                        //car.natur_kis = natur;
                        // Выполним операцию
                        //int res_car = rw_oper.ExecSaveOperation(car, rw_oper.OperationArrivalUZWay, new OperationArrivalUZWay(id_way, dt_amkr, dt_amkr, natur, null, pv.GODN));
                        int res_car = rw_oper.ExecSaveOperation(car, rw_oper.OperationArrivalUZWay, new OperationArrivalUZWay(id_way, dt_amkr, dt_amkr, natur, null, null));
                        if (res_car > 0)
                        {
                            // Закрываем прибытие
                            int res_close_mt = ef_mt.CloseArrivalCars(car.id_sostav, car.num, natur, dt_amkr);
                        }
                        Console.WriteLine("Вагон {0} - cтоит в прибытии станции УЗ по которой можно получить вагон на станцию АМКР - результат переноса {1}", num_vag, res_car);
                        return res_car;

                    }
                    else
                    { // Не стоит в прибытии станции УЗ по которой можно получить вагон на станцию АМКР
                        // Вагон статит на пути любой станции УЗ для отправки на станцию АМКР?
                        id_way_car = last_operation.IsSetWay(list_arrival_ways_uz.Select(w => w.id).ToArray(), null);
                        if (id_way_car > 0)
                        { // Стоит в прибытии с любой станции УЗ 

                            //car.dt_inp_amkr = dt_amkr;
                            //car.natur_kis = natur;
                            // Выполним операцию
                            //int res_car = rw_oper.ExecSaveOperation(car, rw_oper.OperationArrivalUZWay, new OperationArrivalUZWay(id_way, dt_amkr, dt_amkr, natur, null, pv.GODN));
                            int res_car = rw_oper.ExecSaveOperation(car, rw_oper.OperationArrivalUZWay, new OperationArrivalUZWay(id_way, dt_amkr, dt_amkr, natur, null, null));
                            if (res_car > 0)
                            {
                                // Закрываем прибытие
                                int res_close_mt = ef_mt.CloseArrivalCars(car.id_sostav, car.num, natur, dt_amkr);
                            }
                            Console.WriteLine("Вагон {0} - cтоит в прибытии станции УЗ - результат переноса {1}", num_vag, res_car);
                            return res_car;

                        }
                        else
                        { // Не стоит в прибытии со всех станций УЗ
                            // Вагон статит на пути принятия с АМКР, любой станции УЗ?
                            id_way_car = last_operation.IsSetWay(list_sending_ways_uz.Select(w => w.id).ToArray(), null);
                            if (id_way_car > 0)
                            { // Вагон отправлен на УЗ назад
                                if ((car.dt_uz >= dt_amkr.AddDays(this.day_waiting_interval_cars * -1) & (car.natur_kis == null)) | (car.natur_kis != null && car.natur_kis == natur))
                                { // Вагон во временом деапазоне и натурка KIS неопределена

                                    //car.dt_inp_amkr = dt_amkr;
                                    //car.natur_kis = natur;
                                    // Выполним операцию
                                    //int res_car = rw_oper.ExecSaveOperation(car, rw_oper.OperationArrivalUZWay, new OperationArrivalUZWay(id_way, dt_amkr, dt_amkr, natur, null, pv.GODN));
                                    int res_car = rw_oper.ExecSaveOperation(car, rw_oper.OperationArrivalUZWay, new OperationArrivalUZWay(id_way, dt_amkr, dt_amkr, natur, null, null));
                                    if (res_car > 0)
                                    {
                                        // Закрываем прибытие
                                        int res_close_mt = ef_mt.CloseArrivalCars(car.id_sostav, car.num, natur, dt_amkr);
                                    }
                                    Console.WriteLine("Вагон {0} - cтоит на станции УЗ принятый с АМКР - результат переноса {1}", num_vag, res_car);
                                    return res_car;

                                }
                                else
                                {
                                    // Закрыть старый
                                    int res_car_close = rw_oper.CloseSaveCar(car, dt_amkr);
                                    // Создать новый вагон по данным КИС 
                                    int res_car = 0;
                                    if (res_car_close > 0)
                                    {
                                        res_car = SetCarKISToWayRailWay(natur, num_vag, dt_amkr, id_way, (int)station.id_kis);
                                    }
                                    Console.WriteLine("Вагон {0} - cтоит на станции УЗ принятый с АМКР уже давно - результат закрытия старого {1}. Создать новый вагон по данным КИС  - результат создания {2}", num_vag, res_car_close, res_car);
                                    return res_car;
                                }

                            }
                            else
                            { // Вагон находится в системе RAILWAY

                                //Console.WriteLine("Вагон {0} - cтоит в сисстеме Railway", car.num);
                                if (car.dt_uz >= dt_amkr.AddDays(this.day_waiting_interval_cars * -1) & (car.natur_kis == null || (car.natur_kis != null & car.natur_kis == natur)))
                                {
                                    // Вагон находится во временном диапазоне и натурка не определена или равна
                                    // Обновим
                                    int res_car = rw_oper.SetSaveCar(car, natur, null, dt_amkr);
                                    if (res_car > 0)
                                    {
                                        // Закрываем прибытие
                                        int res_close_mt = ef_mt.CloseArrivalCars(car.id_sostav, car.num, natur, dt_amkr);
                                    }
                                    Console.WriteLine("Вагон {0} - cтоит в сисстеме Railway, находится во временном диапазоне и натурка не определена или равна - результат обновления {1}", num_vag, res_car);
                                    return res_car;

                                }
                                else
                                {
                                    // Вагон не находится во временном диапазоне или натурка определена и неравна текущей
                                    // Закрыть старый
                                    int res_car_close = rw_oper.CloseSaveCar(car, dt_amkr);
                                    int res_car = 0;
                                    if (res_car_close > 0)
                                    {
                                        res_car = SetCarKISToWayRailWay(natur, num_vag, dt_amkr, id_way, (int)station.id_kis);
                                    }
                                    // Создать вагон по данным КИС
                                    Console.WriteLine("Вагон {0} - cтоит в сисстеме Railway, НЕ находится во временном диапазоне или натурка пределена и НЕ равна - результат закрытия старого {1}. Создать новый вагон по данным КИС - результат создания {2}", num_vag, res_car_close, res_car);
                                    return res_car;
                                }

                            }
                        }
                    }
                }
                else
                {
                    // Вагона небыло в системе 
                    // Создать вагон по данным КИС
                    int res_car = 0;
                    res_car = SetCarKISToWayRailWay(natur, num_vag, dt_amkr, id_way, (int)station.id_kis);
                    Console.WriteLine("Вагон {0} - НЕ было в сисстеме RAILWAY. Создать новый вагон по данным КИС - результат создания {1}", num_vag, res_car);
                    return res_car;
                }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SetCarToWayRailWay()"), servece_owner, eventID);
                return (int)errorTransfer.global;
            }
        }
        #endregion

        #region ОБНОВИТЬ ВАГОН НА ПУТИ СИСТЕМЫ RAILWAY ПО ДАННЫМ PROM_VAG
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bas_sostav"></param>
        /// <returns></returns>
        public int UpdWayRailWayOfKIS(ref RWBufferArrivalSostav bas_sostav)
        {
            try
            {
                string mess_upd = String.Format("состава (натурный лист: {0}, дата: {1}, ID строки буфера переноса: {2}), прибывшего на АМКР",bas_sostav.natur, bas_sostav.datetime, bas_sostav.id);
                string mess_upd_sostav = "Обновление " + mess_upd;
                string mess_error_upd_sostav = "Ошибка обновления";

                RWReference rw_ref = new RWReference(base.servece_owner, true); // создавать содержимое справочника из данных КИС
                EFWagons ef_wag = new EFWagons();
                EFTKIS ef_tkis = new EFTKIS();

                if (bas_sostav == null) return 0;
                if (bas_sostav.count_set_wagons == null) return 0; // вагоны не поставлены на путь станции

                int id_stations_rw = 0;
                int id_ways_rw = 0;

                id_stations_rw = rw_ref.GetIDStationsOfKIS(bas_sostav.id_station_kis);
                if (id_stations_rw <= 0)
                {
                    String.Format(mess_error_upd_sostav + mess_upd + " - ID станции: {0} не определён в справочнике системы RailWay", bas_sostav.id_station_kis).WriteError(servece_owner, eventID);
                    return (int)errorTransfer.no_stations;
                }
                if (id_stations_rw == 26) id_stations_rw = 27; // Коррекция Промышленная Керамет -> 'это промышленная
                // Определим путь на станции система RailCars
                id_ways_rw = rw_ref.GetIDDefaultWayOfStation(id_stations_rw, bas_sostav.way_num.ToString());
                if (id_ways_rw <= 0)
                {
                    String.Format(mess_error_upd_sostav + mess_upd + " - ID пути: {0} станции: {1} не определён в справочнике системы RailWay", bas_sostav.way_num, bas_sostav.id_station_kis).WriteError(servece_owner, eventID);
                    return (int)errorTransfer.no_ways;
                }

                // Обновим информацию по количеству вагонов в таблице NatHist
                List<PromNatHist> list_nh = ef_wag.GetNatHist(bas_sostav.natur, bas_sostav.id_station_kis, bas_sostav.day, bas_sostav.month, bas_sostav.year, bas_sostav.napr == 2 ? true : false).ToList();
                bas_sostav.count_nathist = list_nh.Count() > 0 ? list_nh.Count() as int? : null;

                List<int> set_wagons = new List<int>();
                // Обнавляем вагоны
                if (bas_sostav.count_set_nathist != null & bas_sostav.list_no_update_wagons != null & bas_sostav.count_set_wagons != null)
                {
                    set_wagons = GetWagonsToListInt(bas_sostav.list_no_update_wagons); // до обновим вагоны
                }
                if (bas_sostav.count_set_nathist != null & bas_sostav.list_no_update_wagons == null & bas_sostav.count_set_wagons != null & bas_sostav.count_set_wagons != bas_sostav.count_set_nathist)
                {
                    set_wagons = GetWagonsToListInt(bas_sostav.list_wagons); // поставим занаво
                }
                // Обновляем вагоны в первый раз
                if ((bas_sostav.count_set_nathist == null | bas_sostav.count_set_nathist == 0) & bas_sostav.list_no_update_wagons == null & bas_sostav.count_set_wagons != null)
                {
                    set_wagons = GetWagonsToListInt(bas_sostav.list_wagons); // поставим занаво
                }
                if (set_wagons.Count() == 0) return 0;
                ResultTransfers result = new ResultTransfers(set_wagons.Count(), null, 0, null, 0, 0);
                bas_sostav.list_no_update_wagons = null;
                bas_sostav.message = null;
                // Ставим вагоны на путь станции
                foreach (int wag in set_wagons)
                {
                    if (result.SetResultUpdate(UpdCarToWayRailWay(bas_sostav.natur, wag, bas_sostav.datetime, id_ways_rw)))
                    {
                        // Ошибка
                        bas_sostav.list_no_update_wagons += wag.ToString() + ";";
                        bas_sostav.message += wag.ToString() + ":" + result.result.ToString() + ";";
                    }
                }
                bas_sostav.count_set_nathist = (bas_sostav.count_set_nathist != null ? bas_sostav.count_set_nathist : 0) + result.ResultUpdate;
                mess_upd_sostav += String.Format(" (id_rw_station : {0}, id_rw_way : {1}) по данным системы КИС. Определено для обновления: {2} вагонов, обновлено: {3} вагонов, ранее обновлено: {4} вагонов, ошибок обновления {5}.",id_stations_rw, id_ways_rw, set_wagons.Count(), result.updates, result.skippeds, result.errors);
                mess_upd_sostav.WriteInformation(servece_owner, eventID);
                //if (set_wagons.Count() > 0) { mess_update_sostav.SaveLogEvents(result.errors > 0 ? EventStatus.Error : EventStatus.Ok, servece_owner, eventID); }
                // Сохранить результат и вернуть код
                if (ef_tkis.SaveRWBufferArrivalSostav(bas_sostav) < 0)
                { return (int)errorTransfer.set_table_arrival_sostav; }
                else { return result.ResultUpdate; }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("UpdWayRailWayOfKIS(bas_sostav={0})", bas_sostav.GetFieldsAndValue()), servece_owner, eventID);
                return (int)errorTransfer.global;
            }
        }

        public int UpdCarToWayRailWay(int natur, int num_vag, DateTime dt_amkr, int id_way)
        {
            try
            {
                //RWOperation rw_oper = new RWOperation(this.servece_owner);
                EFRailWay ef_rw = new EFRailWay();
                RWReference rw_ref = new RWReference(base.servece_owner, true);
                //EFMetallurgTrans ef_mt = new EFMetallurgTrans();
                //EFWagons ef_wag = new EFWagons();

                int result = 0;

                Ways way = ef_rw.GetWays(id_way);
                Stations station = way.Stations;

                string mess = String.Format("грузополучателя и годности вагона №:{0}, принадлежащего составу (натурный лист: {1}, дата: {2}) стоящего на пути станции (станция АМКР: {3}, путь: {4})", num_vag, natur, dt_amkr, station.name_ru, way.num + "- " + way.name_ru);
                string mess_update_vag = "Обновление " + mess;
                string mess_update_vag_err = "Ошибка обновления " + mess;
         
                PromNatHist pnh = GetCorrectNatHist(natur, num_vag, dt_amkr, (int)station.id_kis);
                if (pnh == null)
                {
                    String.Format(mess_update_vag_err + ", код ошибки:{0}", errorTransfer.no_wagon_is_nathist.ToString()).WriteError(servece_owner, eventID);
                    return (int)errorTransfer.no_wagon_is_nathist;
                }
                // Определим грузополучателя
                int? id_consignee = null;
                if (pnh.K_POL_GR != null)
                {
                    id_consignee = rw_ref.GetIDReferenceConsigneeOfKis((int)pnh.K_POL_GR, true);
                }

                // Обновим данные
                if (id_consignee!= null | pnh.GODN!=null | !String.IsNullOrWhiteSpace(pnh.ST_OTPR) ){
                    Cars car = ef_rw.GetCarsOfSetKIS(num_vag, dt_amkr, natur);
                    if (car != null) {
                        if (id_consignee!= null) car.CarsInpDelivery.ToList()[0].id_consignee = id_consignee;
                        if (!String.IsNullOrWhiteSpace(pnh.ST_OTPR)) car.CarsInpDelivery.ToList()[0].station_shipment = pnh.ST_OTPR;
                        if (pnh.GODN != null)
                        {
                            foreach (CarOperations operations in car.CarOperations)
                            {
                                if (operations.id_car_conditions == null) operations.id_car_conditions = pnh.GODN;
                            }
                        }
                        result = ef_rw.SaveCars(car);
                        if (result < 0)
                        {
                            String.Format(mess_update_vag_err + ", код ошибки:{0}", result).WriteError(servece_owner, eventID);
                            return result;
                        }
                    }
                }
                if (id_consignee == null)
                {
                    String.Format(mess_update_vag_err + ", код ошибки:{0}, PromNatHist.K_POL_GR:{1}.", errorTransfer.no_shop.ToString(), pnh.K_POL_GR).WriteWarning(servece_owner, eventID);
                    return (int)errorTransfer.no_shop;
                }
                //Обновлять пока не появится годность
                if (pnh.GODN == null)
                {
                    String.Format(mess_update_vag_err + ", код ошибки:{0}", errorTransfer.no_godn.ToString()).WriteWarning(servece_owner, eventID);
                    return (int)errorTransfer.no_godn;
                }
                return result;

            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SetCarToWayRailWay()"), servece_owner, eventID);
                return (int)errorTransfer.global;
            }
        }
        #endregion

        #endregion

        #region Закрыть перенос составов
        public int CloseBufferArrivalSostav()
        {
            int res_rw = CloseRWBufferArrivalSostav();
            return res_rw;
        }

        public int CloseRWBufferArrivalSostav()
        {

            EFTKIS ef_tkis = new EFTKIS();
            int close = 0;
            int skip = 0;
            int error = 0;

            List<RWBufferArrivalSostav> list = new List<RWBufferArrivalSostav>();
            list = ef_tkis.GetRWBufferArrivalSostavNoClose().OrderBy(c => c.datetime).ToList();
            foreach (RWBufferArrivalSostav bas in list.ToList())
            {
                int res = CloseRWBufferArrivalSostav(bas);
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

        public int CloseRWBufferArrivalSostav(RWBufferArrivalSostav bas)
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
                return DeleteSostavRWBufferArrivalSostav(bas.id);
                //}
            }
            // Ситуация-2.  Проверим наличие вагонов в системе КИС (Могли отменить натурку убрать данные из таблиц NanHist)
            if ((list_pv != null && list_pv.Count() > 0) & (list_pnh == null || list_pnh.Count() == 0))
            {
                return DeleteSostavRWBufferArrivalSostav(bas.id);
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
                        return ef_tkis.SaveRWBufferArrivalSostav(bas);
                    }
                }
            }
            // пропускаем
            return 0;
        }
        #endregion

        #region Коррекция системы переноса
        public int DeleteSostavRWBufferArrivalSostav(int id)
        {
            //EFTKIS ef_tkis = new EFTKIS();
            //EFRailCars ef_rc = new EFRailCars();
            //EFMetallurgTrans ef_mt = new EFMetallurgTrans();

            //TODO: Коррекция системы переноса - реализовать удаление из RAILWAY
            int del_rc = 0;
            //int err_del_rc = 0;
            //int del_sap = 0;
            //int upd_mt = 0;
            //try
            //{
            //    RWBufferArrivalSostav del_bas = ef_tkis.GetRWBufferArrivalSostav(id);
            //    RWBufferArrivalSostav new_bas = null;
            //    List<RWBufferArrivalSostav> not_close_list = ef_tkis.GetRWBufferArrivalSostav().Where(b => b.datetime >= del_bas.datetime & b.id != del_bas.id).ToList();
            //    foreach (RWBufferArrivalSostav bas in not_close_list)
            //    {
            //        if (bas.list_wagons != null && del_bas.list_wagons != null && bas.list_wagons.Trim() == del_bas.list_wagons.Trim())
            //        {
            //            new_bas = bas;
            //            break;
            //        }
            //    }
            //    string mess = String.Format("Коррекция данных (Удалена натурка {0} от {1}, создана новая {2} от {3}). ",
            //        del_bas.natur, del_bas.datetime, (new_bas != null ? (int?)new_bas.natur : null), (new_bas != null ? (DateTime?)new_bas.datetime : null));

            //    List<VAGON_OPERATIONS> list_del_wagons = ef_rc.GetVagonsOperationsToNatur(del_bas.natur, del_bas.datetime).ToList();
            //    foreach (VAGON_OPERATIONS wag in list_del_wagons)
            //    {
            //        int? way = wag.id_way;
            //        int idsostav = (int)wag.IDSostav;
            //        VAGON_OPERATIONS res_del = ef_rc.DeleteVAGON_OPERATIONS(wag.id_oper);
            //        if (res_del != null)
            //        {
            //            del_rc++;
            //            //String.Format(mess + "Из системы RailCars - удален вагон {0}", wag.num_vagon).WriteEvents(servece_owner, eventID);
            //            if (way != null)
            //            {
            //                ef_rc.OffSetCars((int)way, 1);
            //            }
            //            if (wag.IDSostav < 0)
            //            {
            //                // idsostav отрицательный
            //                EFSAP ef_sap = new EFSAP();
            //                ef_sap.DeleteSAPIncSupplySostav(idsostav);
            //                del_sap++;
            //            }
            //            if (wag.IDSostav > 0)
            //            {
            //                // idsostav положительный
            //                if (new_bas != null)
            //                {
            //                    int new_natur = new_bas.natur;
            //                    DateTime date_amkr = new_bas.datetime;
            //                    ArrivalCars arr_car = ef_mt.GetArrivalCars((int)wag.IDSostav);
            //                    if (arr_car != null)
            //                    {
            //                        arr_car.NumDocArrival = new_natur;
            //                        arr_car.Arrival = date_amkr;
            //                        arr_car.UserName = System.Environment.UserDomainName + @"\" + System.Environment.UserName;
            //                        ef_mt.SaveArrivalCars(arr_car);
            //                        upd_mt++;
            //                    }
            //                }
            //            }
            //        }
            //        else
            //        {
            //            err_del_rc++;
            //        }
            //    }
            //    if (err_del_rc == 0)
            //    {
            //        del_bas.close = DateTime.Now;
            //        del_bas.close_user = System.Environment.UserDomainName + @"\" + System.Environment.UserName;
            //        del_bas.status = (int)statusSting.Delete;
            //        ef_tkis.SaveRWBufferArrivalSostav(del_bas);
            //    }
            //    String.Format(mess + "Из системы RailCars - удалено {0} вагонов, ошибок удаления {1}, из справочника САП вхю пост. удалено {2} строк, в прибытии МТ Скорректировано {3} строки."
            //        , del_rc, err_del_rc, del_sap, upd_mt).WriteEvents(err_del_rc > 0 ? EventStatus.Error : EventStatus.Ok, servece_owner, eventID);
            //}
            //catch (Exception e)
            //{
            //    e.WriteErrorMethod(String.Format("DeleteSostavBufferArrivalSostav(id={0})", id), servece_owner, eventID);
            //    return (int)errorTransfer.global;
            //}
            return del_rc;
        }
        #endregion


    }
}
