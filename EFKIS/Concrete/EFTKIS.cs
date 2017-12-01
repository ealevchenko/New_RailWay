using EFKIS.Abstract;
using EFKIS.Entities;
using MessageLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libClass;

namespace EFKIS.Concrete
{
    public class EFTKIS : ITKIS
    {
        private eventID eventID = eventID.EFTKIS;

        protected EFTDbContext context = new EFTDbContext();


        public IQueryable<ArrivalSostav> ArrivalSostav
        {
            get { return context.ArrivalSostav; }
        }

        public IQueryable<ArrivalSostav> GetArrivalSostav()
        {
            try
            {
                return ArrivalSostav;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetArrivalSostav()"), eventID);
                return null;
            }
        }

        public ArrivalSostav GetArrivalSostav(int id)
        {
            try
            {
                return GetArrivalSostav().Where(s => s.id == id).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetArrivalSostav(id={0})", id), eventID);
                return null;
            }
        }

        public int SaveArrivalSostav(ArrivalSostav ArrivalSostav)
        {
            ArrivalSostav dbEntry;
            try
            {
                if (ArrivalSostav.id == 0)
                {
                    dbEntry = new ArrivalSostav()
                    {
                        id = ArrivalSostav.id,
                        datetime = ArrivalSostav.datetime,
                        day = ArrivalSostav.day,
                        month = ArrivalSostav.month,
                        year = ArrivalSostav.year,
                        hour = ArrivalSostav.hour,
                        minute = ArrivalSostav.minute,
                        natur = ArrivalSostav.natur,
                        id_station_kis = ArrivalSostav.id_station_kis,
                        way_num = ArrivalSostav.way_num,
                        napr = ArrivalSostav.napr,
                        count_wagons = ArrivalSostav.count_wagons,
                        count_nathist = ArrivalSostav.count_nathist,
                        count_set_wagons = ArrivalSostav.count_set_wagons,
                        count_set_nathist = ArrivalSostav.count_set_nathist,
                        close = ArrivalSostav.close,
                        close_user = ArrivalSostav.close_user,
                        status = ArrivalSostav.status,
                        list_wagons = ArrivalSostav.list_wagons,
                        list_no_set_wagons = ArrivalSostav.list_no_set_wagons,
                        list_no_update_wagons = ArrivalSostav.list_no_update_wagons,
                        message = ArrivalSostav.message,
                    };
                    context.ArrivalSostav.Add(dbEntry);
                }
                else
                {
                    dbEntry = context.ArrivalSostav.Find(ArrivalSostav.id);
                    if (dbEntry != null)
                    {
                        dbEntry.datetime = ArrivalSostav.datetime;
                        dbEntry.day = ArrivalSostav.day;
                        dbEntry.month = ArrivalSostav.month;
                        dbEntry.year = ArrivalSostav.year;
                        dbEntry.hour = ArrivalSostav.hour;
                        dbEntry.minute = ArrivalSostav.minute;
                        dbEntry.natur = ArrivalSostav.natur;
                        dbEntry.id_station_kis = ArrivalSostav.id_station_kis;
                        dbEntry.way_num = ArrivalSostav.way_num;
                        dbEntry.napr = ArrivalSostav.napr;
                        dbEntry.count_wagons = ArrivalSostav.count_wagons;
                        dbEntry.count_nathist = ArrivalSostav.count_nathist;
                        dbEntry.count_set_wagons = ArrivalSostav.count_set_wagons;
                        dbEntry.count_set_nathist = ArrivalSostav.count_set_nathist;
                        dbEntry.close = ArrivalSostav.close;
                        dbEntry.close_user = ArrivalSostav.close_user;
                        dbEntry.status = ArrivalSostav.status;
                        dbEntry.list_wagons = ArrivalSostav.list_wagons;
                        dbEntry.list_no_set_wagons = ArrivalSostav.list_no_set_wagons;
                        dbEntry.list_no_update_wagons = ArrivalSostav.list_no_update_wagons;
                        dbEntry.message = ArrivalSostav.message;
                    }
                }
                context.SaveChanges();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveArrivalSostav(ArrivalSostav={0})", ArrivalSostav.GetFieldsAndValue()), eventID);
                return -1;
            }
            return dbEntry.id;
        }

        public ArrivalSostav DeleteArrivalSostav(int id)
        {
            ArrivalSostav dbEntry = context.ArrivalSostav.Find(id);
            if (dbEntry != null)
            {
                try
                {
                    context.ArrivalSostav.Remove(dbEntry);

                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    e.WriteErrorMethod(String.Format("DeleteArrivalSostav(id={0})", id), eventID);
                    return null;
                }
            }
            return dbEntry;
        }
        /// <summary>
        /// Вернуть последнее время по которому перенесли состав
        /// </summary>
        /// <returns></returns>
        public DateTime? GetLastDateTime()
        {
            try
            {
                ArrivalSostav oas = GetArrivalSostav().OrderByDescending(a => a.datetime).FirstOrDefault();
                return oas != null ? (DateTime?)oas.datetime : null;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetLastDateTime()"), eventID);
                return null;
            }
        }
        /// <summary>
        /// Выбрать перенесеные составы за указанный период
        /// </summary>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        public IQueryable<ArrivalSostav> GetArrivalSostav(DateTime start, DateTime stop)
        {
            try
            {
                return GetArrivalSostav().Where(o => o.datetime >= start & o.datetime <= stop);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetArrivalSostav()"), eventID);
                return null;
            }
        }
        /// <summary>
        /// Выбрать не закрытые перенесеные составы
        /// </summary>
        /// <returns></returns>
        public IQueryable<ArrivalSostav> GetArrivalSostavNoClose()
        {
            try
            {
                return GetArrivalSostav().Where(o => o.close == null).OrderBy(o => o.datetime);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetArrivalSostavNoClose()"), eventID);
                return null;
            }
        }


    }
}
