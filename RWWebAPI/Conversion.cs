using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RWWebAPI
{
    public static class Conversion
    {
        public static bool In<T>(this T source, params T[] list)
        {
            return list.Contains(source);
        }

        public static DateTime? DateConversion(this string date)
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
    }
}
