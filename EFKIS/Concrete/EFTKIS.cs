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

        #region BufferArrivalSostav Перенос прибывших с УЗ вагонов по данным КИС
        public IQueryable<BufferArrivalSostav> BufferArrivalSostav
        {
            get { return context.BufferArrivalSostav; }
        }

        public IQueryable<BufferArrivalSostav> GetBufferArrivalSostav()
        {
            try
            {
                return BufferArrivalSostav;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetBufferArrivalSostav()"), eventID);
                return null;
            }
        }

        public BufferArrivalSostav GetBufferArrivalSostav(int id)
        {
            try
            {
                return GetBufferArrivalSostav().Where(s => s.id == id).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetBufferArrivalSostav(id={0})", id), eventID);
                return null;
            }
        }

        public int SaveArrivalSostav(BufferArrivalSostav ArrivalSostav)
        {
            BufferArrivalSostav dbEntry;
            try
            {
                if (ArrivalSostav.id == 0)
                {
                    dbEntry = new BufferArrivalSostav()
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
                    context.BufferArrivalSostav.Add(dbEntry);
                }
                else
                {
                    dbEntry = context.BufferArrivalSostav.Find(ArrivalSostav.id);
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
                e.WriteErrorMethod(String.Format("SaveBufferArrivalSostav(ArrivalSostav={0})", BufferArrivalSostav.GetFieldsAndValue()), eventID);
                return -1;
            }
            return dbEntry.id;
        }

        public BufferArrivalSostav DeleteBufferArrivalSostav(int id)
        {
            BufferArrivalSostav dbEntry = context.BufferArrivalSostav.Find(id);
            if (dbEntry != null)
            {
                try
                {
                    context.BufferArrivalSostav.Remove(dbEntry);

                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    e.WriteErrorMethod(String.Format("DeleteBufferArrivalSostav(id={0})", id), eventID);
                    return null;
                }
            }
            return dbEntry;
        }
        /// <summary>
        /// Вернуть последнее время по которому перенесли состав
        /// </summary>
        /// <returns></returns>
        public DateTime? GetLastDateTimeBufferArrivalSostav()
        {
            try
            {
                BufferArrivalSostav oas = GetBufferArrivalSostav().OrderByDescending(a => a.datetime).FirstOrDefault();
                return oas != null ? (DateTime?)oas.datetime : null;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetLastDateTimeBufferArrivalSostav()"), eventID);
                return null;
            }
        }
        /// <summary>
        /// Выбрать перенесеные составы за указанный период
        /// </summary>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        public IQueryable<BufferArrivalSostav> GetBufferArrivalSostav(DateTime start, DateTime stop)
        {
            try
            {
                return GetBufferArrivalSostav().Where(o => o.datetime >= start & o.datetime <= stop);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetBufferArrivalSostav()"), eventID);
                return null;
            }
        }
        /// <summary>
        /// Выбрать не закрытые перенесеные составы
        /// </summary>
        /// <returns></returns>
        public IQueryable<BufferArrivalSostav> GetBufferArrivalSostavNoClose()
        {
            try
            {
                return GetBufferArrivalSostav().Where(o => o.close == null).OrderBy(o => o.datetime);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetBufferArrivalSostavNoClose()"), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить строку по натурке
        /// </summary>
        /// <param name="natur"></param>
        /// <returns></returns>
        public BufferArrivalSostav GetBufferArrivalSostavOfNatur(int natur)
        {
            try
            {
                return GetBufferArrivalSostav().Where(o => o.natur == natur).OrderByDescending(o => o.datetime).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetBufferArrivalSostavOfNatur(natur={0})", natur), eventID);
                return null;
            }
        }
        #endregion

        #region BufferInputSostav Перенос прибывающих вагонов на станцию по данным КИС
        public IQueryable<BufferInputSostav> BufferInputSostav
        {
            get { return context.BufferInputSostav; }
        }

        public IQueryable<BufferInputSostav> GetBufferInputSostav()
        {
            try
            {
                return BufferInputSostav;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetBufferInputSostav()"), eventID);
                return null;
            }
        }

        public BufferInputSostav GetBufferInputSostav(int id)
        {
            try
            {
                return GetBufferInputSostav().Where(s => s.id == id).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetBufferInputSostav(id={0})", id), eventID);
                return null;
            }
        }

        public int SaveBufferInputSostav(BufferInputSostav BufferInputSostav)
        {
            BufferInputSostav dbEntry;
            try
            {
                if (BufferInputSostav.id == 0)
                {
                    dbEntry = new BufferInputSostav()
                    {
                        id = BufferInputSostav.id,
                        datetime = BufferInputSostav.datetime,
                        doc_num = BufferInputSostav.doc_num,
                        id_station_from_kis = BufferInputSostav.id_station_from_kis,
                        way_num_kis = BufferInputSostav.way_num_kis,
                        napr = BufferInputSostav.napr,
                        id_station_on_kis = BufferInputSostav.id_station_on_kis,
                        count_wagons = BufferInputSostav.count_wagons,
                        count_set_wagons = BufferInputSostav.count_set_wagons,
                        natur = BufferInputSostav.natur,
                        close = BufferInputSostav.close,
                        close_user = BufferInputSostav.close_user,
                        status = BufferInputSostav.status,
                        message = BufferInputSostav.message
                    };
                    context.BufferInputSostav.Add(dbEntry);
                }
                else
                {
                    dbEntry = context.BufferInputSostav.Find(BufferInputSostav.id);
                    if (dbEntry != null)
                    {
                        dbEntry.id = BufferInputSostav.id;
                        dbEntry.datetime = BufferInputSostav.datetime;
                        dbEntry.doc_num = BufferInputSostav.doc_num;
                        dbEntry.id_station_from_kis = BufferInputSostav.id_station_from_kis;
                        dbEntry.way_num_kis = BufferInputSostav.way_num_kis;
                        dbEntry.napr = BufferInputSostav.napr;
                        dbEntry.id_station_on_kis = BufferInputSostav.id_station_on_kis;
                        dbEntry.count_wagons = BufferInputSostav.count_wagons;
                        dbEntry.count_set_wagons = BufferInputSostav.count_set_wagons;
                        dbEntry.natur = BufferInputSostav.natur;
                        dbEntry.close = BufferInputSostav.close;
                        dbEntry.close_user = BufferInputSostav.close_user;
                        dbEntry.status = BufferInputSostav.status;
                        dbEntry.message = BufferInputSostav.message;
                    }
                }
                context.SaveChanges();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveBufferInputSostav(BufferInputSostav={0})", BufferInputSostav.GetFieldsAndValue()), eventID);
                return -1;
            }
            return dbEntry.id;
        }

        public BufferInputSostav DeleteBufferInputSostav(int id)
        {
            BufferInputSostav dbEntry = context.BufferInputSostav.Find(id);
            if (dbEntry != null)
            {
                try
                {
                    context.BufferInputSostav.Remove(dbEntry);

                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    e.WriteErrorMethod(String.Format("DeleteBufferInputSostav(id={0})", id), eventID);
                    return null;
                }
            }
            return dbEntry;
        }
        /// <summary>
        /// Вернуть последнее время по которому перенесли состав
        /// </summary>
        /// <returns></returns>
        public DateTime? GetLastDateTimeBufferInputSostav()
        {
            try
            {
                BufferInputSostav bis = GetBufferInputSostav().OrderByDescending(a => a.datetime).FirstOrDefault();
                return bis != null ? (DateTime?)bis.datetime : null;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetLastDateTimeBufferInputSostav()"), eventID);
                return null;
            }
        }
        /// <summary>
        /// Вернуть за строки за указанный период
        /// </summary>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        public IQueryable<BufferInputSostav> GetBufferInputSostav(DateTime start, DateTime stop)
        {
            try
            {
                return GetBufferInputSostav().Where(o => o.datetime >= start & o.datetime <= stop);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetBufferInputSostav(start={0}, stop={1})", start, stop), eventID);
                return null;
            }
        }
        #endregion

        #region BufferOutputSostav Перенос отправленных вагонов на станцию по данным КИС
        public IQueryable<BufferOutputSostav> BufferOutputSostav
        {
            get { return context.BufferOutputSostav; }
        }

        public IQueryable<BufferOutputSostav> GetBufferOutputSostav()
        {
            try
            {
                return BufferOutputSostav;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetBufferOutputSostav()"), eventID);
                return null;
            }
        }

        public BufferOutputSostav GetBufferOutputSostav(int id)
        {
            BufferOutputSostav dbEntry = context.BufferOutputSostav.Find(id);
            if (dbEntry != null)
            {
                try
                {
                    context.BufferOutputSostav.Remove(dbEntry);

                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    e.WriteErrorMethod(String.Format("GetBufferOutputSostav(id={0})", id), eventID);
                    return null;
                }
            }
            return dbEntry;
        }

        public int SaveBufferOutputSostav(BufferOutputSostav BufferOutputSostav)
        {
            BufferOutputSostav dbEntry;
            try
            {
                if (BufferOutputSostav.id == 0)
                {
                    dbEntry = new BufferOutputSostav()
                    {
                        id = BufferOutputSostav.id,
                        datetime = BufferOutputSostav.datetime,
                        doc_num = BufferOutputSostav.doc_num,
                        id_station_on_kis = BufferOutputSostav.id_station_on_kis,
                        way_num_kis = BufferOutputSostav.way_num_kis,
                        napr = BufferOutputSostav.napr,
                        id_station_from_kis = BufferOutputSostav.id_station_from_kis,
                        count_wagons = BufferOutputSostav.count_wagons,
                        count_set_wagons = BufferOutputSostav.count_set_wagons,
                        close = BufferOutputSostav.close,
                        close_user = BufferOutputSostav.close_user,
                        status = BufferOutputSostav.status,
                        message = BufferOutputSostav.message
                    };
                    context.BufferOutputSostav.Add(dbEntry);
                }
                else
                {
                    dbEntry = context.BufferOutputSostav.Find(BufferOutputSostav.id);
                    if (dbEntry != null)
                    {
                        dbEntry.id = BufferOutputSostav.id;
                        dbEntry.datetime = BufferOutputSostav.datetime;
                        dbEntry.doc_num = BufferOutputSostav.doc_num;
                        dbEntry.id_station_on_kis = BufferOutputSostav.id_station_on_kis;
                        dbEntry.way_num_kis = BufferOutputSostav.way_num_kis;
                        dbEntry.napr = BufferOutputSostav.napr;
                        dbEntry.id_station_from_kis = BufferOutputSostav.id_station_from_kis;
                        dbEntry.count_wagons = BufferOutputSostav.count_wagons;
                        dbEntry.count_set_wagons = BufferOutputSostav.count_set_wagons;
                        dbEntry.close = BufferOutputSostav.close;
                        dbEntry.close_user = BufferOutputSostav.close_user;
                        dbEntry.status = BufferOutputSostav.status;
                        dbEntry.message = BufferOutputSostav.message;
                    }
                }
                context.SaveChanges();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveBufferOutputSostav(BufferOutputSostav={0})", BufferOutputSostav.GetFieldsAndValue()), eventID);
                return -1;
            }
            return dbEntry.id;
        }

        public BufferOutputSostav DeleteBufferOutputSostav(int id)
        {
            BufferOutputSostav dbEntry = context.BufferOutputSostav.Find(id);
            if (dbEntry != null)
            {
                try
                {
                    context.BufferOutputSostav.Remove(dbEntry);

                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    e.WriteErrorMethod(String.Format("DeleteBufferOutputSostav(id={0})", id), eventID);
                    return null;
                }
            }
            return dbEntry;
        }

        public DateTime? GetLastDateTimeBufferOutputSostav()
        {
            try
            {
                BufferOutputSostav bos = GetBufferOutputSostav().OrderByDescending(a => a.datetime).FirstOrDefault();
                return bos != null ? (DateTime?)bos.datetime : null;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetLastDateTimeBufferOutputSostav()"), eventID);
                return null;
            }
        }

        public IQueryable<BufferOutputSostav> GetBufferOutputSostav(DateTime start, DateTime stop)
        {
            try
            {
                return GetBufferOutputSostav().Where(o => o.datetime >= start & o.datetime <= stop);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetBufferOutputSostav(start={0}, stop={1})",start,stop), eventID);
                return null;
            }
        }
        #endregion
    }
}
