using EFReference.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web_RailWay.App_Code
{
    public static class ViewReferenceHelpers
    {
        static EFReference.Concrete.EFReference ef_reference = new EFReference.Concrete.EFReference();

        #region html
        /// <summary>
        /// Вернуть название станции по справочнику УЗ
        /// </summary>
        /// <param name="html"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static MvcHtmlString GetReferenceStationsOfCode(this HtmlHelper html, int code)
        {
            Stations station = ef_reference.GetStationsOfCode(code);
            return MvcHtmlString.Create(station != null ? station.station + "(" + station.code_cs.ToString() + ")" : code.ToString());
        }
        public static MvcHtmlString GetReferenceStationsOfCodecs(this HtmlHelper html, int codecs)
        {
            Stations station = ef_reference.GetStationsOfCodeCS(codecs);
            return MvcHtmlString.Create(station != null ? station.station + "(" + station.code_cs.ToString() + ")" : codecs.ToString());
        }

        /// <summary>
        /// Вернуть название страны СНГ (прибытие)
        /// </summary>
        /// <param name="html"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static MvcHtmlString GetReferenceCountryOfCountryCode(this HtmlHelper html, int code)
        {
            if (code > 99)
            {
                code = int.Parse(code.ToString().Substring(0, 2));
            }
            States state = ef_reference.GetStates(code);
            return MvcHtmlString.Create(state != null ? state.state : code.ToString());
        }
        /// <summary>
        /// Показать груз
        /// </summary>
        /// <param name="html"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static MvcHtmlString GetReferenceCargoOfCode(this HtmlHelper html, int code)
        {
            Cargo cargo = ef_reference.GetCargoOfCodeETSNG(code);
            if (cargo == null)
            {
                cargo = ef_reference.GetCorrectCargo(code);
            }
            return MvcHtmlString.Create(cargo != null ? cargo.name_etsng + "(" + code.ToString() + ")" : code.ToString());
        }
        #endregion
        
        public static string GetReferenceStationsOfCodecs(int? code)
        {
            if (code == null) return "-";
            Stations station = ef_reference.GetStationsOfCodeCS((int)code);
            return station != null ? station.station : code.ToString();
        }


    }
}