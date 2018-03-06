using EFRW.Concrete;
using EFRW.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW
{
    public static class Filters
    {
        public delegate bool flOperation<T>(T ps, int id_station);

        public static T[] FilterOperation<T>(this IEnumerable<T> source, flOperation<T> filter, int id)
        {
            ArrayList aList = new ArrayList();
            foreach (T s in source)
            {
                if (filter(s, id))
                {
                    aList.Add(s);
                }
            }
            return ((T[])aList.ToArray(typeof(T)));
        }

        public static bool IsSendingStation(Cars car, int id)
        {
            EFRailWay ef_rw = new EFRailWay();
            CarOperations operation = ef_rw.GetCurrentCarOperationsOfCar(car.id);
            return operation != null && operation.send_id_station == id ? true:false;
        }
    }
}
