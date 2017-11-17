using EFMT.Concrete;
using EFRC.Concrete;
using EFRC.Entities;
using MessageLog;
using MT.Entities;
using RCReferences;
using RWWebAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferRailCars
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
        public DateTime DateTime_on { get; set; }
        public DateTime DateTime_from { get; set; }
        public int? ParentID { get; set; }
        public trWagon[] Wagons { get; set; }
    }

    public class TRailCars
    {

        private eventID eventID = eventID.OLDVersion_TRailCars;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="codes_consignee"></param>
        /// <returns></returns>
        private bool IsConsignee(int code, int[] codes_consignee)
        {
            foreach (int c in codes_consignee)
            {
                if (c == code) return true;
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <param name="id_stat_receiving"></param>
        /// <param name="code_consignee"></param>
        /// <returns></returns>
        private List<trWagon> GetListWagonInArrival(IQueryable<ArrivalCars> list, int? id_stat_receiving, int[] code_consignee)
        {
            //bool bOk = false;
            if (list == null | id_stat_receiving == null | code_consignee == null) return null;
            List<trWagon> list_wag = new List<trWagon>();
            try
            {
                int position = 1;
                foreach (ArrivalCars wag in list)
                {
                    // состояние вагонов
                    int id_conditions = 17; // ожидает прибытие с УЗ                        
                    // червоная
                    if (id_stat_receiving == 467201)
                    {
                        if (wag.StationCode == id_stat_receiving & IsConsignee(wag.Consignee, code_consignee))
                        {
                            //bOk = true; 
                            list_wag.Add(new trWagon()
                            {
                                Position = position++,
                                CarriageNumber = wag.Num,
                                CountryCode = wag.CountryCode,
                                Weight = wag.Weight,
                                IDCargo = wag.CargoCode,
                                Cargo = wag.Cargo,
                                IDStation = wag.StationCode,
                                Station = wag.Station,
                                Consignee = wag.Consignee,
                                Operation = wag.Operation,
                                CompositionIndex = wag.CompositionIndex,
                                DateOperation = wag.DateOperation,
                                TrainNumber = wag.TrainNumber,
                                Conditions = id_conditions,
                            });
                        }
                        //else { id_conditions = 18; } // маневры на УЗ
                        // если есть хоть один вагон АМКР и конечная станция червонная
                    }
                    // главн
                    if (id_stat_receiving == 467004)
                    {
                        //bOk = true;
                        if (wag.StationCode != id_stat_receiving | !IsConsignee(wag.Consignee, code_consignee))
                            return null; // есть вагон недошедший до станции назанчения или с кодом грузополучателя не АМКР 
                        list_wag.Add(new trWagon()
                        {
                            Position = wag.Position,
                            CarriageNumber = wag.Num,
                            CountryCode = wag.CountryCode,
                            Weight = wag.Weight,
                            IDCargo = wag.CargoCode,
                            Cargo = wag.Cargo,
                            IDStation = wag.StationCode,
                            Station = wag.Station,
                            Consignee = wag.Consignee,
                            Operation = wag.Operation,
                            CompositionIndex = wag.CompositionIndex,
                            DateOperation = wag.DateOperation,
                            TrainNumber = wag.TrainNumber,
                            Conditions = id_conditions,
                        });
                    }

                }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetListWagonInArrival(list={0}, id_stat_receiving={1}, code_consignee = {2})", list, id_stat_receiving, code_consignee), eventID);
                return null;
            }
            return list_wag;
        }
        /// <summary>
        /// Получить пакет данных trSostav
        /// </summary>
        /// <param name="id_sostav"></param>
        /// <returns></returns>
        public trSostav GetSostav(int id_sostav)
        {
            try
            {
                // Определим класс данных состав
                EFMetallurgTrans efmt = new EFMetallurgTrans();
                ArrivalSostav sost = efmt.GetArrivalSostav(id_sostav);

                //MTSostav sost = mtc.Get_MTSostav(id_sostav);
                // Определим код станции по справочникам
                Reference api_reference = new Reference();
                Stations station_in = api_reference.GetStationsOfCode(int.Parse(sost.CompositionIndex.Substring(9, 4)) * 10);
                Stations station_from = api_reference.GetStationsOfCode(int.Parse(sost.CompositionIndex.Substring(0, 4)) * 10);
                int? codecs_in = station_in != null ? station_in.code_cs : int.Parse(sost.CompositionIndex.Substring(9, 4)) * 10;
                int? codecs_from = station_from != null ? station_from.code_cs : int.Parse(sost.CompositionIndex.Substring(0, 4)) * 10;
                //int? codecs_in = api_reference..GetCodeCSStations(int.Parse(sost.CompositionIndex.Substring(9, 4)) * 10);
                //int? codecs_from = refRW.GetCodeCSStations(int.Parse(sost.CompositionIndex.Substring(0, 4)) * 10);

                // Определим класс данных вагоны
                List<trWagon> list_wag = new List<trWagon>();
                list_wag = GetListWagonInArrival(efmt.GetArrivalCarsOfSostav(id_sostav), codecs_in, efmt.GetListCodeConsigneeOfConsignee(mtConsignee.AMKR));
                ArrivalSostav first_sostav = efmt.GetFirstArrivalSostavOfIDArrival(sost.IDArrival);
                //List<ArrivalCars> list_mt = mtc.GetOperationMTSostavDestinct(sost.ID);
                trSostav sostav = new trSostav()
                {
                    id = sost.ID,
                    codecs_in_station = codecs_in,
                    codecs_from_station = codecs_from,
                    //FileName = sost.FileName,
                    //CompositionIndex = sost.CompositionIndex,
                    DateTime_on = first_sostav != null ? first_sostav.DateTime : sost.DateTime,
                    DateTime_from = sost.DateTime,
                    //Operation = sost.Operation,
                    //Create = sost.Create,
                    //Close = sost.Close,
                    ParentID = sost.ParentID,
                    Wagons = list_wag != null ? list_wag.ToArray() : null,
                };
                return sostav;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetSostav(id_sostav={0})", id_sostav), eventID);
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_sostav"></param>
        /// <returns></returns>
        public int ArrivalToRailCars(int id_sostav)
        {
            string mess_transfer = String.Format("переноса состава:{0} в прибытие системы Railway", id_sostav);
            string mess_sap = String.Format("формирования строк справочника SAP входящие поставки для состава: {0}", id_sostav);
            string mess_transfer_error = mess_transfer;
            string mess_transfer_error1 = String.Format("Ошибка " + mess_transfer);
            string mess_sap_error = mess_sap;

            int result = 0;
            try
            {
                //KIS_RC_Transfer rc_transfer = new KIS_RC_Transfer(servece_owner); // Перенос в системе RailCars

                trSostav sostav = GetSostav(id_sostav);
                // Поставим вагоны в систему RailCars
                int res_arc;

                try
                {
                    //res_arc = rc_transfer.PutInArrival(sostav);
                    res_arc = PutInArrival(sostav);
                    if (res_arc < 0)
                    {
                        String.Format("Ошибка постановки состава №{0} в прибытие АМКР, код ошибки {1}", id_sostav, res_arc).WriteError(this.eventID);
                    }
                }
                catch (Exception e)
                {
                    e.WriteError(String.Format("Ошибка постановки состава №{0} в прибытие АМКР", id_sostav), eventID);
                    res_arc = -1;
                }
                // Поставим вагоны в систему RailWay            
                // TODO: Выполнить код постановки вагонов в систему RailWay (прибытие из КР)
                // ..................

                // Создаем или изменяем строки в справочнике САП
                //int rec_sap;
                //try
                //{
                //    rec_sap = sap_transfer.PutInSapIncomingSupply(sostav);
                //}
                //catch (Exception e)
                //{
                //    ServicesEventLog.LogError(e, mess_sap_error, servece_owner, eventID);
                //    rec_sap = -1;
                //}
                result = res_arc; //TODO: переделат возврат после (Выполнить код постановки вагонов в систему RailWay (прибытие из КР))
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("ArrivalToRailCars(id_sostav={0})", id_sostav), eventID);

            }
            return result;
        }

        #region Поставить вагоны в прибытие из станций Кривого Рога
        /// <summary>
        /// Поставить вагоны в прибытие из станций УЗ
        /// </summary>
        /// <param name="sostav"></param>
        /// <returns></returns>
        public int PutInArrival(trSostav sostav)
        {
            EFRailCars efrc = new EFRailCars();
            string mess_transfer = String.Format("состава:{0} в прибытие со станции УЗ:{1} системы Railway", sostav.id, sostav.codecs_in_station);
            string mess_transfer_sostav = "Перенос " + mess_transfer;
            string mess_transfer_error = "переноса " + mess_transfer;
            string mess_transfer_error1 = "Ошибка переноса " + mess_transfer;
            try
            {

                if (sostav == null) return 0;
                ResultTransfers result = new ResultTransfers(sostav.Wagons != null ? sostav.Wagons.Count() : 0, 0, null, null, 0, 0);
                //if (sostav.ParentID != null)
                //{
                //    string mess_del = String.Format("удаления состава:{0} из прибытия системы Railway, по которому получено новое ТСП (новый состав:{1})", sostav.ParentID, sostav.id);
                //    string mess_del_error = mess_transfer;
                //    string mess_del_error1 = String.Format("Ошибка " + mess_transfer);
                //    try
                //    {
                //        //TODO: !! ВОЗМОЖНО (ПРИБЫТИЕ С УЗ) - доработать удаление составов по которым происходит тсп (удалять только те вагоны которых нет в новом тсп и добавлять новые - вдруг поезд приняли уже на станцию)
                //        EFRailCars efrc = new EFRailCars();
                //        int res = efrc.DeleteVagonsToInsertMT((int)sostav.ParentID); // Удалить предыдущий состав (По этому составу было новое ТСП) 
                //        if (res < 0)
                //        {
                //            mess_del_error1.WriteError(eventID);
                //            return (int)errorTransfer.global;
                //        }
                //    }
                //    catch (Exception e)
                //    {
                //        e.WriteError(mess_del_error, eventID);
                //        return (int)errorTransfer.global;
                //    }
                //}
                if (sostav.Wagons == null) return 0;
                List<trWagon> list_new_vag = sostav.Wagons.ToList();
                // проверим старый состав с этим вагоном
                if (sostav.ParentID != null)
                {
                    List<IGrouping<int?, VAGON_OPERATIONS>> group_list_old_vag = efrc.GetVagonsOperationsGroupingVagon((int)sostav.ParentID).ToList();
                    foreach (IGrouping<int?, VAGON_OPERATIONS> group_wag in group_list_old_vag)
                    {
                        // старый вавгон есть в новом списке
                        trWagon tw = list_new_vag.Find(w => w.CarriageNumber == group_wag.Key);
                        if (tw != null)
                        {
                            // есть, проверка стоит в прибытии или приняли в систему
                            // получить список по убыванию, последняя операция вверху
                            List<VAGON_OPERATIONS> list_old_wag = group_wag.OrderByDescending(o => o.id_oper).ToList();
                            if (list_old_wag != null && list_old_wag.Count() > 0)
                            {
                                if ((list_old_wag[0].id_stat == 33 & list_old_wag[0].st_lock_id_stat == 13) |
                                    (list_old_wag[0].id_stat == 33 & list_old_wag[0].st_lock_id_stat == 4) |
                                    (list_old_wag[0].id_stat == 35 & list_old_wag[0].st_lock_id_stat == 20))
                                {
                                    // стоит в прибытии, удалим
                                    efrc.DeleteVagonsOperations((int)sostav.ParentID, group_wag.Key != null ? (int)group_wag.Key : 0);
                                }
                                else { 
                                    // уже в работе, заменим номер состава, уберем из нового списка вагон
                                    efrc.UpdateIDSostavVagonsOperations((int)sostav.ParentID, group_wag.Key != null ? (int)group_wag.Key : 0, (int)sostav.id);
                                    result.IncSkipped();
                                    list_new_vag.Remove(tw);
                                }
                            }
                        }
                        else { 
                            // нет, удалить из системы все записи по старому вагону
                            efrc.DeleteVagonsOperations((int)sostav.ParentID, group_wag.Key != null ? (int)group_wag.Key : 0);
                        }
                    }
                }
                // ставим вагоны в прибытие
                foreach (trWagon wag in list_new_vag)
                {
                    
                    string mess_transfer_vag = String.Format("вагона:{0}, состава:{1} в прибытие со станции УЗ:{2} системы Railway", wag.CarriageNumber, sostav.id, sostav.codecs_in_station);
                    string mess_transfer_vag_error = "переноса " + mess_transfer_vag;
                    string mess_transfer_vag_error1 = "Ошибка переноса " + mess_transfer_vag;
                    try
                    {
                        // есть, проверка стоит в прибытии или приняли в систему
                        // получить список по убыванию, последняя операция вверху
                        List<VAGON_OPERATIONS> list_old_wag = efrc.GetVagonsOperations(sostav.id, wag.CarriageNumber).ToList();
                        if (list_old_wag != null && list_old_wag.Count() > 0)
                        {
                            if ((list_old_wag[0].id_stat == 33 & list_old_wag[0].st_lock_id_stat == 13) |
                                (list_old_wag[0].id_stat == 33 & list_old_wag[0].st_lock_id_stat == 4) |
                                (list_old_wag[0].id_stat == 35 & list_old_wag[0].st_lock_id_stat == 20))
                            {
                                // стоит в прибытии, удалим
                                efrc.DeleteVagonsOperations(sostav.id, wag.CarriageNumber);
                            }
                            else
                            {
                                // уже в работе
                                result.IncSkipped();
                                continue;

                            }
                        }
                        // ставим
                        if (result.SetResultInsert(ArrivalWagon(wag, sostav.id, sostav.DateTime_on, sostav.DateTime_from, sostav.codecs_in_station)))
                        {
                            String.Format(mess_transfer_vag_error1 + ", код ошибки: {0}", ((errorTransfer)result.result).ToString()).WriteError(eventID);
                        }
                    }
                    catch (Exception e)
                    {
                        e.WriteError(mess_transfer_vag_error, eventID);
                        result.IncError();
                    }
                }
                mess_transfer_sostav += String.Format(", определено для переноса:{0} вагонов, перенесено: {1}, перенесено ранее: {2}, ошибок переноса: {3}.", result.counts, result.inserts, result.skippeds, result.errors);
                mess_transfer_sostav.WriteWarning(eventID);
                //TODO: Добавить формирование события
                //if (result.counts > 0) { mess_transfer_sostav.SaveLogEvents(result.errors > 0 ? EventStatus.Error : EventStatus.Ok, servece_owner, eventID); }
                return result.ResultInsert;
            }
            catch (Exception e)
            {
                e.WriteError(mess_transfer_error, eventID);
                return (int)errorTransfer.global;
            }
        }
        /// <summary>
        /// Поставить вагон в прибытие из станций УЗ
        /// </summary>
        /// <param name="wag"></param>
        /// <param name="id_sostav"></param>
        /// <param name="dt"></param>
        /// <param name="codecs_station"></param>
        /// <returns></returns>
        public int ArrivalWagon(trWagon wag, int id_sostav, DateTime dt_on, DateTime dt_from, int? codecs_station)
        {
            string mess_transfer_vag = String.Format("вагона:{0}, состава:{1} в прибытие со станции УЗ:{2} системы Railway", wag.CarriageNumber, id_sostav, codecs_station);
            string mess_transfer_vag_error = "переноса " + mess_transfer_vag;
            string mess_transfer_vag_error1 = "Ошибка переноса " + mess_transfer_vag;
            RCReference rc_ref = new RCReference();
            try
            {
                if (codecs_station == null)
                {
                    return (int)errorTransfer.no_stations;
                }
                // Определим id вагона и собственника вагона
                int id_vagon = rc_ref.DefinitionSetIDVagon(wag.CarriageNumber, dt_from, wag.TrainNumber, id_sostav, null, wag.Conditions == 17 ? false : true);// если вагон имеет состояние ожидает прибытие c УЗ
                if (id_vagon < 0)
                {
                    return (int)errorTransfer.no_owner_country;
                }
                //TODO: !! УБРАТЬ (ПОСТАНОВКА ВАГОНА В ПРИБЫТИЕ С УЗ) - убрать определение груза он будет братся из справочника САП вход поставки
                int? id_gruz = rc_ref.DefinitionIDGruzs(wag.IDCargo);
                int id_way_33 = 998; // Путь УЗ с которого прийдет состав
                int id_way_35 = 1002;// Путь УЗ с которого прийдет состав
                EFRailCars efrc = new EFRailCars();
                //if (!efrc.IsVagonOperationMT(id_sostav, dt_from, id_vagon)) // вагон не стоит
                //{
                    // Поставим вагон

                    int res1 = 0;
                    int res2 = 0;
                    if (codecs_station == 467004) // Кривой Рог Гл.
                    {
                        res1 = efrc.InsertVagon(id_sostav, id_vagon, wag.CarriageNumber, dt_on, dt_from, 33, wag.Position, id_gruz, (decimal)wag.Weight, 13, wag.TrainNumber, wag.Conditions, id_way_33);
                        res2 = efrc.InsertVagon(id_sostav, id_vagon, wag.CarriageNumber, dt_on, dt_from, 33, wag.Position, id_gruz, (decimal)wag.Weight, 4, wag.TrainNumber, wag.Conditions, id_way_33);
                        if (res1 < 0) return res1;
                        if (res2 < 0) return res2;
                        return res1;
                    }
                    if (codecs_station == 467201) // Кривой Рог черв.
                    {

                        res1 = efrc.InsertVagon(id_sostav, id_vagon, wag.CarriageNumber, dt_on, dt_from, 35, wag.Position, id_gruz, (decimal)wag.Weight, 20, wag.TrainNumber, wag.Conditions, id_way_35);
                        return res1;
                    }
                    return (int)errorTransfer.no_stations; ;
                //}
                //return 0;
            }
            catch (Exception e)
            {
                e.WriteError(mess_transfer_vag_error, eventID);
                return (int)errorTransfer.global;
            }
        }
        #endregion

    }
}
