using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RWConversionFunctions
{
    public static class Conversion
    {
        public static bool In<T>(this T source, params T[] list)
        {
            return list.Contains(source);
        }

        public static string IntsToString(this IEnumerable<int> source, char sep)
        {
            if (source == null) return null;
            string list = "";
            foreach (int i in source)
            {
                list += i.ToString() + sep;
            }
            if (!String.IsNullOrWhiteSpace(list)) { return list.Remove(list.Length - 1); } else { return null; }
        }

        public static DateTime? DateNullConversion(this string date)
        {
            if (String.IsNullOrWhiteSpace(date)) return null;
            try
            {
                return (DateTime?)XmlConvert.ToDateTime(date);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static DateTime DateConversion(this string date)
        {
            DateTime? new_date = date.DateNullConversion();
            return new_date != null ? (DateTime)new_date : new DateTime();
        }

    }
}
