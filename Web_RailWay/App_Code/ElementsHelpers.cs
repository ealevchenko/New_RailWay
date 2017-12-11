using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web_RailWay.App_Code
{
    public static class ElementsHelpers
    {
        #region Списки для компанентов ...List
        public delegate string FilterGetRailWayObject<T>(T id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="filter"></param>
        /// <param name="selected"></param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> GetSelectListItem(this List<int> source, FilterGetRailWayObject<int?> filter, int? selected)
        {
            List<SelectListItem> sli = new List<SelectListItem>();
            foreach (int i in source)
            {
                if (i == selected)
                {
                    sli.Add(new SelectListItem() { Text = filter(i), Value = i.ToString(), Selected = true });
                }
                else
                {
                    sli.Add(new SelectListItem() { Text = filter(i), Value = i.ToString() });
                }
            }
            return sli;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="filter"></param>
        /// <param name="first"></param>
        /// <param name="selected"></param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> GetSelectListItem(this List<int> source, FilterGetRailWayObject<int?> filter, string first, int? selected)
        {
            List<SelectListItem> sli = new List<SelectListItem>();
            sli.Add(new SelectListItem() { Text = first, Value = "0" });
            foreach (int i in source)
            {
                if (i == selected)
                {
                    sli.Add(new SelectListItem()
                    {
                        Text = filter(i),
                        Value = i.ToString(),
                        Selected = true
                    });
                }
                else
                {
                    sli.Add(new SelectListItem() { Text = filter(i), Value = i.ToString() });
                }
            }
            return sli;

        }
        #endregion
    }
}