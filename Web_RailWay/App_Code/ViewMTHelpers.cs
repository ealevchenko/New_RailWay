using EFMT.Abstract;
using EFMT.Concrete;
using MT.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Web_RailWay.App_Code
{
    public static class ViewMTHelpers
    {
        static EFMetallurgTrans ef_mt = new EFMetallurgTrans();

        #region html
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
        #endregion
    }
}