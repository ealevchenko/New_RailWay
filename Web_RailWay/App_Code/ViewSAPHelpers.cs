using EFRC.Concrete;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Web;
using System.Web.Mvc;
using Web_RailWay.App_LocalResources;

namespace Web_RailWay.App_Code
{
    public static class ViewSAPHelpers
    {
        static EFSAP ef_sap = new EFSAP();


        #region Преобразование
        /// <summary>
        /// Вернуть текстовое сообщение из ресурса
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string GetResource(this string key)
        {
            ResourceManager rmsap = new ResourceManager(typeof(SAPResource));
            return rmsap.GetString(key, CultureInfo.CurrentCulture);
        }
        /// <summary>
        /// Выделить из строки Index номер документа
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static int GetNumDocOfIndex(this string index)
        {
            if (String.IsNullOrWhiteSpace(index)) return 0;
            int doc = index.IndexOf("N:");
            int dt = index.IndexOf(" D:");
            if (doc >= 0 & dt >= 0) {
                return int.Parse(index.Substring(doc + 2, dt - 1));
            }
            return 0;
        }
        /// <summary>
        /// Выделить из строки Index дату документа
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static DateTime? GetDateTimeOfIndex(this string index)
        {
            if (String.IsNullOrWhiteSpace(index)) return null;
            try
            {
                int dt = index.IndexOf("D:");
                if (dt >= 0)
                {
                    int day = int.Parse(index.Substring(dt + 2, 2));
                    int mont = int.Parse(index.Substring(dt + 5, 2));
                    int year = int.Parse(index.Substring(dt + 8, 4));
                    int tire = index.IndexOf("-");
                    int hour = int.Parse(index.Substring(dt + 13, tire - (dt + 13)));
                    int min = int.Parse(index.Substring(tire + 1, index.Length - (tire + 1)));
                    return new DateTime(year, mont, day, hour, min, 0);
                }
            }
            catch (Exception e) {
                return null;
            }
            return null;
        }
        #endregion

        #region html

        #region Общие
        /// <summary>
        /// Получить номер документа по данным SAPIncSupply
        /// </summary>
        /// <param name="html"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static MvcHtmlString GetNumDocOfSAPISIndex(this HtmlHelper html, string index)
        {
            if (String.IsNullOrWhiteSpace(index)) return MvcHtmlString.Create("-");
            int doc = index.GetNumDocOfIndex();
            return MvcHtmlString.Create(doc>0  ? doc.ToString(): index);
        }
        /// <summary>
        /// Получить дату документа по данным SAPIncSupply
        /// </summary>
        /// <param name="html"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static MvcHtmlString GetDateTimeDocOfSAPISIndex(this HtmlHelper html, string index)
        {
            if (String.IsNullOrWhiteSpace(index)) return MvcHtmlString.Create("-");
            DateTime? dt = index.GetDateTimeOfIndex();
            return MvcHtmlString.Create(dt != null ? dt.ToString() : index);
        }
        #endregion

        #endregion
    }
}