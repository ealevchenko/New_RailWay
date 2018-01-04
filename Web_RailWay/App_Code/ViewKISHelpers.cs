using EFKIS.Concrete;
using EFKIS.Entities;
using KIS;
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
    public static class ViewKISHelpers
    {
        #region Преобразование
        /// <summary>
        /// Вернуть текстовое сообщение из ресурса
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string GetResource(this string key)
        {
            ResourceManager rmMT = new ResourceManager(typeof(KISResource));
            return rmMT.GetString(key, CultureInfo.CurrentCulture);
        }
        #endregion

        #region html

        #region KIST
        public static MvcHtmlString GetKISStatus(this HtmlHelper html, int code)
        {
            return MvcHtmlString.Create(((statusSting)code).ToString());
        }
        /// <summary>
        /// Вернуть текстовое название операции
        /// </summary>
        /// <param name="html"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static MvcHtmlString GetKISStatusString(this HtmlHelper html, int code)
        {
            return MvcHtmlString.Create(html.GetKISStatus(code).ToString().GetResource());            
            //return MvcHtmlString.Create(((statusSting)code).ToString().GetResource());
        }
        /// <summary>
        /// Вернуть статус постановки вагона
        /// </summary>
        /// <param name="html"></param>
        /// <param name="cars"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static MvcHtmlString GetStatusSetCars(this HtmlHelper html, string car, string list)
        {
            if (String.IsNullOrWhiteSpace(list)) return MvcHtmlString.Create("Ok");
            int res = list.IndexOf(car);
            return MvcHtmlString.Create(res >= 0 ? "Error" : "Ok");
        }

        public static MvcHtmlString GetStatusUpdateCars(this HtmlHelper html, string car, string list)
        {
            if (String.IsNullOrWhiteSpace(list)) return MvcHtmlString.Create("Ok");
            string[] list_cars = list.Split(';');
            foreach (string car_upd in list_cars) {
                if (!String.IsNullOrWhiteSpace(car_upd))
                {
                    string[] car_status = car_upd.Split(':');
                    if (car_status != null && car_status.Count() == 2)
                    {
                        if (car_status[0] == car)
                        {
                            return MvcHtmlString.Create(((errorTransfer)int.Parse(car_status[1].ToString().Trim())).ToString().GetResource());
                        }
                    }
                }
            }
            return MvcHtmlString.Create("Ok");
        }
        
        #region Wagons
        /// <summary>
        /// Название станции
        /// </summary>
        /// <param name="html"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static MvcHtmlString GetKometaStation(this HtmlHelper html, int id)
        {
            EFWagons ef_wag = new EFWagons();
            KometaStan station = ef_wag.GetKometaStan(id);
            return MvcHtmlString.Create(station != null ? station.NAME : id.ToString());
        }

        #endregion



        #endregion

        #endregion
    }
}