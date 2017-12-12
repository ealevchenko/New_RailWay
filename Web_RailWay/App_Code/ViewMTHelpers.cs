using EFMT.Abstract;
using EFMT.Concrete;
using EFMT.Entities;
using MetallurgTrans;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Web_RailWay.App_LocalResources;

namespace Web_RailWay.App_Code
{
    
    
    public static class ViewMTHelpers
    {
        static EFMetallurgTrans ef_mt = new EFMetallurgTrans();

        #region Преобразование
        /// <summary>
        /// Вернуть текстовое сообщение из ресурса
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetResource(this string key)
        {
            ResourceManager rmMT = new ResourceManager(typeof(MTResource));
            return rmMT.GetString(key, CultureInfo.CurrentCulture);
        }
        /// <summary>
        /// Выделить из строки Index код станции отправки
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static int GetCodeFromOfIndex(this string index)
        {
            if (String.IsNullOrWhiteSpace(index) & index.Length == 13 ) return 0;
            return int.Parse(index.Substring(0, 4)) * 10;
        }
        /// <summary>
        /// Выделить из строки Index код станции прибытия
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static int GetCodeOnOfIndex(this string index)
        {
            if (String.IsNullOrWhiteSpace(index) & index.Length == 13 ) return 0;
            return int.Parse(index.Substring(9, 4)) * 10;
        }
        /// <summary>
        /// Выделение из строки CountryCode (xxx) кода страны СНГ
        /// </summary>
        /// <param name="Country"></param>
        /// <returns></returns>
        public static int GetCodeOfCountry(this string Country)
        {
            if (String.IsNullOrWhiteSpace(Country) || Country.Length < 3) return 0;
            return int.Parse(Country.Substring(0, 2));
        }

        #endregion

        public static bool IsConsigneeAMKR(this int consignee)
        {
            return ef_mt.IsConsignee(consignee, mtConsignee.AMKR);
        }

        public static IEnumerable<SelectListItem> GetViewCars(int? selected)
        {
            List<SelectListItem> sli = new List<SelectListItem>();
            sli.Add(new SelectListItem() { Text = "all_cars".GetResource(), Value = "0" , Selected = selected==0 ? true: false });
            sli.Add(new SelectListItem() { Text = "amkr_cars".GetResource(), Value = "1" , Selected = selected==1 ? true: false});
            sli.Add(new SelectListItem() { Text = "amkr_cars_arival".GetResource(), Value = "2" , Selected = selected==2 ? true: false});
            sli.Add(new SelectListItem() { Text = "amkr_cars_no_arival".GetResource(), Value = "3" , Selected = selected==3 ? true: false});
            return sli;
        }

        #region html

        #region Общие
        /// <summary>
        /// Вернуть текстовое название операции
        /// </summary>
        /// <param name="html"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static MvcHtmlString GetMTOperations(this HtmlHelper html, int id)
        {
            return MvcHtmlString.Create(((mtOperation)id).ToString().GetResource());
        }

        public static MvcHtmlString GetMTNumDocArrival(this HtmlHelper html, int doc)
        {
            return MvcHtmlString.Create(doc<=0  ?  ((mtt_err_arrival)doc).ToString().GetResource() : doc.ToString());
        }

        public static MvcHtmlString GetMTStationFromOfIndex(this HtmlHelper html, string index)
        {
            if (String.IsNullOrWhiteSpace(index)) return MvcHtmlString.Create("-");
            return html.GetReferenceStationsOfCode(index.GetCodeFromOfIndex());
        }

        public static MvcHtmlString GetMTStationOnOfIndex(this HtmlHelper html, string index)
        {
            if (String.IsNullOrWhiteSpace(index)) return MvcHtmlString.Create("-");
            return html.GetReferenceStationsOfCode(index.GetCodeOnOfIndex());
        }

        public static MvcHtmlString GetMTConsignee(this HtmlHelper html, int id)
        {
            Consignee consignee = ef_mt.GetConsignee(id);
            return MvcHtmlString.Create(consignee != null ? ((mtConsignee)consignee.consignee1).ToString() : id.ToString());
        }
        /// <summary>
        /// Вернуть статус строки прибывшего вагона
        /// </summary>
        /// <param name="html"></param>
        /// <param name="consignee"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static MvcHtmlString GetStatusMTArrivalCars(this HtmlHelper html, int consignee, int? doc)
        {
            if (ef_mt.IsConsignee(consignee, mtConsignee.AMKR))
            {
                return MvcHtmlString.Create(doc != null ? "amkr-taken" : "amkr");
            } return MvcHtmlString.Create("normal");
        }
        #endregion

        #region Arrival
        /// <summary>
        /// Вернуть индекс состава
        /// </summary>
        /// <param name="html"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static MvcHtmlString GetMTArrivalIndex(this HtmlHelper html, int id)
        {
            ArrivalSostav arr_s = ef_mt.GetArrivalSostav(id);
            return MvcHtmlString.Create(arr_s != null ? arr_s.CompositionIndex : " - ");
        }
        /// <summary>
        /// Вернуть дату операции
        /// </summary>
        /// <param name="html"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static MvcHtmlString GetMTArrivalDT(this HtmlHelper html, int id)
        {
            ArrivalSostav arr_s = ef_mt.GetArrivalSostav(id);
            return MvcHtmlString.Create(arr_s != null ? Thread.CurrentThread.CurrentCulture.Name == "en-US" ? arr_s.DateTime.ToString("MM/dd/yyyy hh:mm") : arr_s.DateTime.ToString("dd.MM.yyyy hh:mm") : " - ");
        }
        /// <summary>
        /// Вернуть операцию МТ над вагоном
        /// </summary>
        /// <param name="html"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static MvcHtmlString GetMTArrivalOperations(this HtmlHelper html, int id)
        {
            ArrivalSostav arr_s = ef_mt.GetArrivalSostav(id);
            return arr_s != null ? GetMTOperations(html, arr_s.Operation) : MvcHtmlString.Create("-");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="html"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static MvcHtmlString GetMTArrivalTrain(this HtmlHelper html, int id)
        {
            ArrivalSostav arr_s = ef_mt.GetArrivalSostav(id);
            return MvcHtmlString.Create( arr_s != null ?  arr_s.ArrivalCars != null && arr_s.ArrivalCars.Count()>0 ? arr_s.ArrivalCars.ToList()[0].TrainNumber.ToString() :"-"  : (string)"-" );
        }
        /// <summary>
        /// Получить количесво вагонов указаного грузополучателя по указаному составу
        /// </summary>
        /// <param name="html"></param>
        /// <param name="id"></param>
        /// <param name="consignee"></param>
        /// <returns></returns>
        public static MvcHtmlString GetCountMTArrivalCarsAMKR(this HtmlHelper html, int id, mtConsignee consignee)
        {
            return MvcHtmlString.Create(ef_mt.GetArrivalCarsOfConsignees(id, ef_mt.GetConsigneeToCodes(consignee)).Count().ToString());
        }

        #endregion

        #endregion
    }
}