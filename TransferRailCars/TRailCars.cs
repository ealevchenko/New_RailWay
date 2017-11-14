using EFMT.Concrete;
using MT.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferRailCars
{
    public class TRailCars
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
            public DateTime DateTime_on { get; set; }
            public DateTime DateTime_from { get; set; }
            public int? ParentID { get; set; }
            public trWagon[] Wagons { get; set; }
        }

        /// <summary>
        /// Получить пакет данных trSostav
        /// </summary>
        /// <param name="id_sostav"></param>
        /// <returns></returns>
        public trSostav GetSostav(int id_sostav)
        {
            //// Определим класс данных состав
            //EFMetallurgTrans efmt = new EFMetallurgTrans();
            //ArrivalSostav sost = efmt.GetArrivalSostav(id_sostav);
            
            ////MTSostav sost = mtc.Get_MTSostav(id_sostav);
            //// Определим код станции по справочникам
            //int? codecs_in = refRW.GetCodeCSStations(int.Parse(sost.CompositionIndex.Substring(9, 4)) * 10);
            //int? codecs_from = refRW.GetCodeCSStations(int.Parse(sost.CompositionIndex.Substring(0, 4)) * 10);
            //// Определим класс данных вагоны
            //List<trWagon
            //    > list_wag = new List<trWagon>();
            //list_wag = GetListWagonInArrival(mtc.Get_MTListToSostav(id_sostav), codecs_in, mtc.GetMTConsignee(tMTConsignee.AMKR));
            //List<MTSostav> list_mt = mtc.GetOperationMTSostavDestinct(sost.ID);
            //trSostav sostav = new trSostav()
            //{
            //    id = sost.ID,
            //    codecs_in_station = codecs_in,
            //    codecs_from_station = codecs_from,
            //    //FileName = sost.FileName,
            //    //CompositionIndex = sost.CompositionIndex,
            //    DateTime_on = list_mt.Count > 0 ? list_mt.Last().DateTime : sost.DateTime,
            //    DateTime_from = sost.DateTime,
            //    //Operation = sost.Operation,
            //    //Create = sost.Create,
            //    //Close = sost.Close,
            //    ParentID = sost.ParentID,
            //    Wagons = list_wag != null ? list_wag.ToArray() : null,
            //};
            //return sostav;
            return null;
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
            //try
            //{
            //    KIS_RC_Transfer rc_transfer = new KIS_RC_Transfer(servece_owner); // Перенос в системе RailCars

            //    trSostav sostav = GetSostav(id_sostav);
            //    // Поставим вагоны в систему RailCars
            //    int res_arc;

            //    try
            //    {
            //        res_arc = rc_transfer.PutInArrival(sostav);
            //        if (res_arc < 0)
            //        {
            //            ServicesEventLog.LogError(String.Format(mess_transfer_error1 + ", код ошибки: {1}.", res_arc), servece_owner, this.eventID);
            //        }
            //    }
            //    catch (Exception e)
            //    {
            //        ServicesEventLog.LogError(e, mess_transfer_error, servece_owner, eventID);
            //        res_arc = -1;
            //    }
            //    // Поставим вагоны в систему RailWay            
            //    // TODO: Выполнить код постановки вагонов в систему RailWay (прибытие из КР)
            //    // ..................


            //    // Создаем или изменяем строки в справочнике САП
            //    int rec_sap;
            //    try
            //    {
            //        rec_sap = sap_transfer.PutInSapIncomingSupply(sostav);
            //    }
            //    catch (Exception e)
            //    {
            //        ServicesEventLog.LogError(e, mess_sap_error, servece_owner, eventID);
            //        rec_sap = -1;
            //    }
            //    result = res_arc; //TODO: переделат возврат после (Выполнить код постановки вагонов в систему RailWay (прибытие из КР))
            //}
            //catch (AggregateException agex)
            //{
            //    agex.Handle(ex =>
            //    {
            //        ServicesEventLog.LogError(ex, mess_transfer_error, servece_owner, eventID);
            //        return true;
            //    });
            //    return -1;
            //}

            return result;
        }
    }
}
