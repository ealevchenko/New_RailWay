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

        protected EFTRCdbContext context_rc = new EFTRCdbContext();
        protected EFTRWdbContext context_rw = new EFTRWdbContext();

        #region RC

        #region BufferArrivalSostav Перенос прибывших с УЗ вагонов по данным КИС
        public IQueryable<RCBufferArrivalSostav> RCBufferArrivalSostav
        {
            get { return context_rc.RCBufferArrivalSostav; }
        }

        public IQueryable<RCBufferArrivalSostav> GetRCBufferArrivalSostav()
        {
            try
            {
                return RCBufferArrivalSostav;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetRCBufferArrivalSostav()"), eventID);
                return null;
            }
        }

        public RCBufferArrivalSostav GetRCBufferArrivalSostav(int id)
        {
            try
            {
                return GetRCBufferArrivalSostav().Where(s => s.id == id).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetRCBufferArrivalSostav(id={0})", id), eventID);
                return null;
            }
        }

        public int SaveRCBufferArrivalSostav(RCBufferArrivalSostav RCBufferArrivalSostav)
        {
            RCBufferArrivalSostav dbEntry;
            try
            {
                if (RCBufferArrivalSostav.id == 0)
                {
                    dbEntry = new RCBufferArrivalSostav()
                    {
                        id = RCBufferArrivalSostav.id,
                        datetime = RCBufferArrivalSostav.datetime,
                        day = RCBufferArrivalSostav.day,
                        month = RCBufferArrivalSostav.month,
                        year = RCBufferArrivalSostav.year,
                        hour = RCBufferArrivalSostav.hour,
                        minute = RCBufferArrivalSostav.minute,
                        natur = RCBufferArrivalSostav.natur,
                        id_station_kis = RCBufferArrivalSostav.id_station_kis,
                        way_num = RCBufferArrivalSostav.way_num,
                        napr = RCBufferArrivalSostav.napr,
                        count_wagons = RCBufferArrivalSostav.count_wagons,
                        count_nathist = RCBufferArrivalSostav.count_nathist,
                        count_set_wagons = RCBufferArrivalSostav.count_set_wagons,
                        count_set_nathist = RCBufferArrivalSostav.count_set_nathist,
                        close = RCBufferArrivalSostav.close,
                        close_user = RCBufferArrivalSostav.close_user,
                        status = RCBufferArrivalSostav.status,
                        list_wagons = RCBufferArrivalSostav.list_wagons,
                        list_no_set_wagons = RCBufferArrivalSostav.list_no_set_wagons,
                        list_no_update_wagons = RCBufferArrivalSostav.list_no_update_wagons,
                        message = RCBufferArrivalSostav.message,
                    };
                    context_rc.RCBufferArrivalSostav.Add(dbEntry);
                }
                else
                {
                    dbEntry = context_rc.RCBufferArrivalSostav.Find(RCBufferArrivalSostav.id);
                    if (dbEntry != null)
                    {
                        dbEntry.datetime = RCBufferArrivalSostav.datetime;
                        dbEntry.day = RCBufferArrivalSostav.day;
                        dbEntry.month = RCBufferArrivalSostav.month;
                        dbEntry.year = RCBufferArrivalSostav.year;
                        dbEntry.hour = RCBufferArrivalSostav.hour;
                        dbEntry.minute = RCBufferArrivalSostav.minute;
                        dbEntry.natur = RCBufferArrivalSostav.natur;
                        dbEntry.id_station_kis = RCBufferArrivalSostav.id_station_kis;
                        dbEntry.way_num = RCBufferArrivalSostav.way_num;
                        dbEntry.napr = RCBufferArrivalSostav.napr;
                        dbEntry.count_wagons = RCBufferArrivalSostav.count_wagons;
                        dbEntry.count_nathist = RCBufferArrivalSostav.count_nathist;
                        dbEntry.count_set_wagons = RCBufferArrivalSostav.count_set_wagons;
                        dbEntry.count_set_nathist = RCBufferArrivalSostav.count_set_nathist;
                        dbEntry.close = RCBufferArrivalSostav.close;
                        dbEntry.close_user = RCBufferArrivalSostav.close_user;
                        dbEntry.status = RCBufferArrivalSostav.status;
                        dbEntry.list_wagons = RCBufferArrivalSostav.list_wagons;
                        dbEntry.list_no_set_wagons = RCBufferArrivalSostav.list_no_set_wagons;
                        dbEntry.list_no_update_wagons = RCBufferArrivalSostav.list_no_update_wagons;
                        dbEntry.message = RCBufferArrivalSostav.message;
                    }
                }
                context_rc.SaveChanges();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveRCBufferArrivalSostav(RCBufferArrivalSostav={0})", RCBufferArrivalSostav.GetFieldsAndValue()), eventID);
                return -1;
            }
            return dbEntry.id;
        }

        public RCBufferArrivalSostav DeleteRCBufferArrivalSostav(int id)
        {
            RCBufferArrivalSostav dbEntry = context_rc.RCBufferArrivalSostav.Find(id);
            if (dbEntry != null)
            {
                try
                {
                    context_rc.RCBufferArrivalSostav.Remove(dbEntry);

                    context_rc.SaveChanges();
                }
                catch (Exception e)
                {
                    e.WriteErrorMethod(String.Format("DeleteRCBufferArrivalSostav(id={0})", id), eventID);
                    return null;
                }
            }
            return dbEntry;
        }
        /// <summary>
        /// Вернуть последнее время по которому перенесли состав
        /// </summary>
        /// <returns></returns>
        public DateTime? GetLastDateTimeRCBufferArrivalSostav()
        {
            try
            {
                RCBufferArrivalSostav oas = GetRCBufferArrivalSostav().OrderByDescending(a => a.datetime).FirstOrDefault();
                return oas != null ? (DateTime?)oas.datetime : null;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetLastDateTimeRCBufferArrivalSostav()"), eventID);
                return null;
            }
        }
        /// <summary>
        /// Выбрать перенесеные составы за указанный период
        /// </summary>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        public IQueryable<RCBufferArrivalSostav> GetRCBufferArrivalSostav(DateTime start, DateTime stop)
        {
            try
            {
                return GetRCBufferArrivalSostav().Where(o => o.datetime >= start & o.datetime <= stop);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetRCBufferArrivalSostav()"), eventID);
                return null;
            }
        }
        /// <summary>
        /// Выбрать не закрытые перенесеные составы
        /// </summary>
        /// <returns></returns>
        public IQueryable<RCBufferArrivalSostav> GetRCBufferArrivalSostavNoClose()
        {
            try
            {
                return GetRCBufferArrivalSostav().Where(o => o.close == null).OrderBy(o => o.datetime);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetRCBufferArrivalSostavNoClose()"), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить строку по натурке
        /// </summary>
        /// <param name="natur"></param>
        /// <returns></returns>
        public RCBufferArrivalSostav GetRCBufferArrivalSostavOfNatur(int natur)
        {
            try
            {
                return GetRCBufferArrivalSostav().Where(o => o.natur == natur).OrderByDescending(o => o.datetime).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetRCBufferArrivalSostavOfNatur(natur={0})", natur), eventID);
                return null;
            }
        }
        /// <summary>
        /// Закрыть строку
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int CloseRCBufferArrivalSostav(int id) {
            try
            {
                return CloseRCBufferArrivalSostav(id, System.Environment.UserDomainName + @"\" + System.Environment.UserName);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CloseRCBufferArrivalSostav(id={0})", id), eventID);
                return -1;
            }

        }

        public int CloseRCBufferArrivalSostav(int id, string user)
        {
            try
            {
                RCBufferArrivalSostav bas = GetRCBufferArrivalSostav(id);
                if (bas == null) return 0;
                bas.close = DateTime.Now;
                bas.close_user = user;
                return SaveRCBufferArrivalSostav(bas);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CloseRCBufferArrivalSostav(id={0}, user={1})", id, user), eventID);
                return -1;
            }

        }

        #endregion

        #region BufferInputSostav Перенос прибывающих вагонов на станцию по данным КИС
        public IQueryable<RCBufferInputSostav> RCBufferInputSostav
        {
            get { return context_rc.RCBufferInputSostav; }
        }

        public IQueryable<RCBufferInputSostav> GetRCBufferInputSostav()
        {
            try
            {
                return RCBufferInputSostav;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetRCBufferInputSostav()"), eventID);
                return null;
            }
        }

        public RCBufferInputSostav GetRCBufferInputSostav(int id)
        {
            try
            {
                return GetRCBufferInputSostav().Where(s => s.id == id).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetRCBufferInputSostav(id={0})", id), eventID);
                return null;
            }
        }

        public int SaveRCBufferInputSostav(RCBufferInputSostav RCBufferInputSostav)
        {
            RCBufferInputSostav dbEntry;
            try
            {
                if (RCBufferInputSostav.id == 0)
                {
                    dbEntry = new RCBufferInputSostav()
                    {
                        id = RCBufferInputSostav.id,
                        datetime = RCBufferInputSostav.datetime,
                        doc_num = RCBufferInputSostav.doc_num,
                        id_station_from_kis = RCBufferInputSostav.id_station_from_kis,
                        way_num_kis = RCBufferInputSostav.way_num_kis,
                        napr = RCBufferInputSostav.napr,
                        id_station_on_kis = RCBufferInputSostav.id_station_on_kis,
                        count_wagons = RCBufferInputSostav.count_wagons,
                        count_set_wagons = RCBufferInputSostav.count_set_wagons,
                        natur = RCBufferInputSostav.natur,
                        close = RCBufferInputSostav.close,
                        close_user = RCBufferInputSostav.close_user,
                        status = RCBufferInputSostav.status,
                        message = RCBufferInputSostav.message
                    };
                    context_rc.RCBufferInputSostav.Add(dbEntry);
                }
                else
                {
                    dbEntry = context_rc.RCBufferInputSostav.Find(RCBufferInputSostav.id);
                    if (dbEntry != null)
                    {
                        dbEntry.id = RCBufferInputSostav.id;
                        dbEntry.datetime = RCBufferInputSostav.datetime;
                        dbEntry.doc_num = RCBufferInputSostav.doc_num;
                        dbEntry.id_station_from_kis = RCBufferInputSostav.id_station_from_kis;
                        dbEntry.way_num_kis = RCBufferInputSostav.way_num_kis;
                        dbEntry.napr = RCBufferInputSostav.napr;
                        dbEntry.id_station_on_kis = RCBufferInputSostav.id_station_on_kis;
                        dbEntry.count_wagons = RCBufferInputSostav.count_wagons;
                        dbEntry.count_set_wagons = RCBufferInputSostav.count_set_wagons;
                        dbEntry.natur = RCBufferInputSostav.natur;
                        dbEntry.close = RCBufferInputSostav.close;
                        dbEntry.close_user = RCBufferInputSostav.close_user;
                        dbEntry.status = RCBufferInputSostav.status;
                        dbEntry.message = RCBufferInputSostav.message;
                    }
                }
                context_rc.SaveChanges();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveRCBufferInputSostav(RCBufferInputSostav={0})", RCBufferInputSostav.GetFieldsAndValue()), eventID);
                return -1;
            }
            return dbEntry.id;
        }

        public RCBufferInputSostav DeleteRCBufferInputSostav(int id)
        {
            RCBufferInputSostav dbEntry = context_rc.RCBufferInputSostav.Find(id);
            if (dbEntry != null)
            {
                try
                {
                    context_rc.RCBufferInputSostav.Remove(dbEntry);

                    context_rc.SaveChanges();
                }
                catch (Exception e)
                {
                    e.WriteErrorMethod(String.Format("DeleteRCBufferInputSostav(id={0})", id), eventID);
                    return null;
                }
            }
            return dbEntry;
        }
        /// <summary>
        /// Вернуть последнее время по которому перенесли состав
        /// </summary>
        /// <returns></returns>
        public DateTime? GetLastDateTimeRCBufferInputSostav()
        {
            try
            {
                RCBufferInputSostav bis = GetRCBufferInputSostav().OrderByDescending(a => a.datetime).FirstOrDefault();
                return bis != null ? (DateTime?)bis.datetime : null;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetLastDateTimeRCBufferInputSostav()"), eventID);
                return null;
            }
        }
        /// <summary>
        /// Вернуть за строки за указанный период
        /// </summary>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        public IQueryable<RCBufferInputSostav> GetRCBufferInputSostav(DateTime start, DateTime stop)
        {
            try
            {
                return GetRCBufferInputSostav().Where(o => o.datetime >= start & o.datetime <= stop);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetRCBufferInputSostav(start={0}, stop={1})", start, stop), eventID);
                return null;
            }
        }
        /// <summary>
        /// Вернуть не закрытые составы
        /// </summary>
        /// <returns></returns>
        public IQueryable<RCBufferInputSostav> GetRCBufferInputSostavNoClose()
        {
            try
            {
                return GetRCBufferInputSostav().Where(i => i.close == null).OrderBy(i => i.datetime);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetRCBufferInputSostavNoClose()"), eventID);
                return null;
            }
        }

        #endregion

        #region BufferOutputSostav Перенос отправленных вагонов на станцию по данным КИС
        public IQueryable<RCBufferOutputSostav> RCBufferOutputSostav
        {
            get { return context_rc.RCBufferOutputSostav; }
        }

        public IQueryable<RCBufferOutputSostav> GetRCBufferOutputSostav()
        {
            try
            {
                return RCBufferOutputSostav;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetRCBufferOutputSostav()"), eventID);
                return null;
            }
        }

        public RCBufferOutputSostav GetRCBufferOutputSostav(int id)
        {
            try
            {
                return GetRCBufferOutputSostav().Where(s => s.id == id).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetRCBufferOutputSostav(id={0})", id), eventID);
                return null;
            }
        }

        public int SaveRCBufferOutputSostav(RCBufferOutputSostav RCBufferOutputSostav)
        {
            RCBufferOutputSostav dbEntry;
            try
            {
                if (RCBufferOutputSostav.id == 0)
                {
                    dbEntry = new RCBufferOutputSostav()
                    {
                        id = RCBufferOutputSostav.id,
                        datetime = RCBufferOutputSostav.datetime,
                        doc_num = RCBufferOutputSostav.doc_num,
                        id_station_on_kis = RCBufferOutputSostav.id_station_on_kis,
                        way_num_kis = RCBufferOutputSostav.way_num_kis,
                        napr = RCBufferOutputSostav.napr,
                        id_station_from_kis = RCBufferOutputSostav.id_station_from_kis,
                        count_wagons = RCBufferOutputSostav.count_wagons,
                        count_set_wagons = RCBufferOutputSostav.count_set_wagons,
                        close = RCBufferOutputSostav.close,
                        close_user = RCBufferOutputSostav.close_user,
                        status = RCBufferOutputSostav.status,
                        message = RCBufferOutputSostav.message
                    };
                    context_rc.RCBufferOutputSostav.Add(dbEntry);
                }
                else
                {
                    dbEntry = context_rc.RCBufferOutputSostav.Find(RCBufferOutputSostav.id);
                    if (dbEntry != null)
                    {
                        dbEntry.id = RCBufferOutputSostav.id;
                        dbEntry.datetime = RCBufferOutputSostav.datetime;
                        dbEntry.doc_num = RCBufferOutputSostav.doc_num;
                        dbEntry.id_station_on_kis = RCBufferOutputSostav.id_station_on_kis;
                        dbEntry.way_num_kis = RCBufferOutputSostav.way_num_kis;
                        dbEntry.napr = RCBufferOutputSostav.napr;
                        dbEntry.id_station_from_kis = RCBufferOutputSostav.id_station_from_kis;
                        dbEntry.count_wagons = RCBufferOutputSostav.count_wagons;
                        dbEntry.count_set_wagons = RCBufferOutputSostav.count_set_wagons;
                        dbEntry.close = RCBufferOutputSostav.close;
                        dbEntry.close_user = RCBufferOutputSostav.close_user;
                        dbEntry.status = RCBufferOutputSostav.status;
                        dbEntry.message = RCBufferOutputSostav.message;
                    }
                }
                context_rc.SaveChanges();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveRCBufferOutputSostav(RCBufferOutputSostav={0})", RCBufferOutputSostav.GetFieldsAndValue()), eventID);
                return -1;
            }
            return dbEntry.id;
        }

        public RCBufferOutputSostav DeleteRCBufferOutputSostav(int id)
        {
            RCBufferOutputSostav dbEntry = context_rc.RCBufferOutputSostav.Find(id);
            if (dbEntry != null)
            {
                try
                {
                    context_rc.RCBufferOutputSostav.Remove(dbEntry);

                    context_rc.SaveChanges();
                }
                catch (Exception e)
                {
                    e.WriteErrorMethod(String.Format("DeleteRCBufferOutputSostav(id={0})", id), eventID);
                    return null;
                }
            }
            return dbEntry;
        }

        public DateTime? GetLastDateTimeRCBufferOutputSostav()
        {
            try
            {
                RCBufferOutputSostav bos = GetRCBufferOutputSostav().OrderByDescending(a => a.datetime).FirstOrDefault();
                return bos != null ? (DateTime?)bos.datetime : null;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetLastDateTimeRCBufferOutputSostav()"), eventID);
                return null;
            }
        }

        public IQueryable<RCBufferOutputSostav> GetRCBufferOutputSostav(DateTime start, DateTime stop)
        {
            try
            {
                return GetRCBufferOutputSostav().Where(o => o.datetime >= start & o.datetime <= stop);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetRCBufferOutputSostav(start={0}, stop={1})",start,stop), eventID);
                return null;
            }
        }
        /// <summary>
        /// Вернуть не закрытые составы
        /// </summary>
        /// <returns></returns>
        public IQueryable<RCBufferOutputSostav> GetRCBufferOutputSostavNoClose()
        {
            try
            {
                return GetRCBufferOutputSostav().Where(i => i.close == null).OrderBy(i => i.datetime);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetRCBufferOutputSostavNoClose()"), eventID);
                return null;
            }
        }
        #endregion
        #endregion

        #region RW
        #region BufferArrivalSostav Перенос прибывших с УЗ вагонов по данным КИС
        public IQueryable<RWBufferArrivalSostav> RWBufferArrivalSostav
        {
            get { return context_rw.RWBufferArrivalSostav; }
        }

        public IQueryable<RWBufferArrivalSostav> GetRWBufferArrivalSostav()
        {
            try
            {
                return RWBufferArrivalSostav;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetRWBufferArrivalSostav()"), eventID);
                return null;
            }
        }

        public RWBufferArrivalSostav GetRWBufferArrivalSostav(int id)
        {
            try
            {
                return GetRWBufferArrivalSostav().Where(s => s.id == id).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetRWBufferArrivalSostav(id={0})", id), eventID);
                return null;
            }
        }

        public int SaveRWBufferArrivalSostav(RWBufferArrivalSostav RWBufferArrivalSostav)
        {
            RWBufferArrivalSostav dbEntry;
            try
            {
                if (RWBufferArrivalSostav.id == 0)
                {
                    dbEntry = new RWBufferArrivalSostav()
                    {
                        id = RWBufferArrivalSostav.id,
                        datetime = RWBufferArrivalSostav.datetime,
                        day = RWBufferArrivalSostav.day,
                        month = RWBufferArrivalSostav.month,
                        year = RWBufferArrivalSostav.year,
                        hour = RWBufferArrivalSostav.hour,
                        minute = RWBufferArrivalSostav.minute,
                        natur = RWBufferArrivalSostav.natur,
                        id_station_kis = RWBufferArrivalSostav.id_station_kis,
                        way_num = RWBufferArrivalSostav.way_num,
                        napr = RWBufferArrivalSostav.napr,
                        count_wagons = RWBufferArrivalSostav.count_wagons,
                        count_nathist = RWBufferArrivalSostav.count_nathist,
                        count_set_wagons = RWBufferArrivalSostav.count_set_wagons,
                        count_set_nathist = RWBufferArrivalSostav.count_set_nathist,
                        close = RWBufferArrivalSostav.close,
                        close_user = RWBufferArrivalSostav.close_user,
                        close_comment = RWBufferArrivalSostav.close_comment,
                        status = RWBufferArrivalSostav.status,
                        list_wagons = RWBufferArrivalSostav.list_wagons,
                        list_no_set_wagons = RWBufferArrivalSostav.list_no_set_wagons,
                        list_no_update_wagons = RWBufferArrivalSostav.list_no_update_wagons,
                        message = RWBufferArrivalSostav.message,
                    };
                    context_rw.RWBufferArrivalSostav.Add(dbEntry);
                }
                else
                {
                    dbEntry = context_rw.RWBufferArrivalSostav.Find(RWBufferArrivalSostav.id);
                    if (dbEntry != null)
                    {
                        dbEntry.datetime = RWBufferArrivalSostav.datetime;
                        dbEntry.day = RWBufferArrivalSostav.day;
                        dbEntry.month = RWBufferArrivalSostav.month;
                        dbEntry.year = RWBufferArrivalSostav.year;
                        dbEntry.hour = RWBufferArrivalSostav.hour;
                        dbEntry.minute = RWBufferArrivalSostav.minute;
                        dbEntry.natur = RWBufferArrivalSostav.natur;
                        dbEntry.id_station_kis = RWBufferArrivalSostav.id_station_kis;
                        dbEntry.way_num = RWBufferArrivalSostav.way_num;
                        dbEntry.napr = RWBufferArrivalSostav.napr;
                        dbEntry.count_wagons = RWBufferArrivalSostav.count_wagons;
                        dbEntry.count_nathist = RWBufferArrivalSostav.count_nathist;
                        dbEntry.count_set_wagons = RWBufferArrivalSostav.count_set_wagons;
                        dbEntry.count_set_nathist = RWBufferArrivalSostav.count_set_nathist;
                        dbEntry.close = RWBufferArrivalSostav.close;
                        dbEntry.close_user = RWBufferArrivalSostav.close_user;
                        dbEntry.close_comment = RWBufferArrivalSostav.close_comment;
                        dbEntry.status = RWBufferArrivalSostav.status;
                        dbEntry.list_wagons = RWBufferArrivalSostav.list_wagons;
                        dbEntry.list_no_set_wagons = RWBufferArrivalSostav.list_no_set_wagons;
                        dbEntry.list_no_update_wagons = RWBufferArrivalSostav.list_no_update_wagons;
                        dbEntry.message = RWBufferArrivalSostav.message;
                    }
                }
                context_rw.SaveChanges();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveRWBufferArrivalSostav(RWBufferArrivalSostav={0})", RWBufferArrivalSostav.GetFieldsAndValue()), eventID);
                return -1;
            }
            return dbEntry.id;
        }

        public RWBufferArrivalSostav DeleteRWBufferArrivalSostav(int id)
        {
            RWBufferArrivalSostav dbEntry = context_rw.RWBufferArrivalSostav.Find(id);
            if (dbEntry != null)
            {
                try
                {
                    context_rw.RWBufferArrivalSostav.Remove(dbEntry);
                    context_rw.SaveChanges();
                }
                catch (Exception e)
                {
                    e.WriteErrorMethod(String.Format("DeleteRWBufferArrivalSostav(id={0})", id), eventID);
                    return null;
                }
            }
            return dbEntry;
        }
        /// <summary>
        /// Вернуть последнее время по которому перенесли состав
        /// </summary>
        /// <returns></returns>
        public DateTime? GetLastDateTimeRWBufferArrivalSostav()
        {
            try
            {
                RWBufferArrivalSostav bas = GetRWBufferArrivalSostav().OrderByDescending(a => a.datetime).FirstOrDefault();
                return bas != null ? (DateTime?)bas.datetime : null;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetLastDateTimeRWBufferArrivalSostav()"), eventID);
                return null;
            }
        }
        /// <summary>
        /// Выбрать перенесеные составы за указанный период
        /// </summary>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        public IQueryable<RWBufferArrivalSostav> GetRWBufferArrivalSostav(DateTime start, DateTime stop)
        {
            try
            {
                return GetRWBufferArrivalSostav().Where(o => o.datetime >= start & o.datetime <= stop);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetRWBufferArrivalSostav(start={0}, stop={1})", start, stop), eventID);
                return null;
            }
        }
        /// <summary>
        /// Выбрать не закрытые перенесеные составы
        /// </summary>
        /// <returns></returns>
        public IQueryable<RWBufferArrivalSostav> GetRWBufferArrivalSostavNoClose()
        {
            try
            {
                return GetRWBufferArrivalSostav().Where(o => o.close == null).OrderBy(o => o.datetime);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetRWBufferArrivalSostavNoClose()"), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить строку по натурке
        /// </summary>
        /// <param name="natur"></param>
        /// <returns></returns>
        public RWBufferArrivalSostav GetRWBufferArrivalSostavOfNatur(int natur)
        {
            try
            {
                return GetRWBufferArrivalSostav().Where(o => o.natur == natur).OrderByDescending(o => o.datetime).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetRWBufferArrivalSostavOfNatur(natur={0})", natur), eventID);
                return null;
            }
        }
        /// <summary>
        /// Закрыть строку
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int CloseRWBufferArrivalSostav(int id)
        {
            try
            {
                return CloseRWBufferArrivalSostav(id, System.Environment.UserDomainName + @"\" + System.Environment.UserName);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CloseRWBufferArrivalSostav(id={0})", id), eventID);
                return -1;
            }

        }

        public int CloseRWBufferArrivalSostav(int id, string user)
        {
            try
            {
                RWBufferArrivalSostav bas = GetRWBufferArrivalSostav(id);
                if (bas == null) return 0;
                bas.close = DateTime.Now;
                bas.close_user = user;
                return SaveRWBufferArrivalSostav(bas);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CloseRWBufferArrivalSostav(id={0}, user={1})", id, user), eventID);
                return -1;
            }

        }

        public int CloseRWBufferArrivalSostav(int id, string coment, string user)
        {
            try
            {
                RWBufferArrivalSostav bas = GetRWBufferArrivalSostav(id);
                if (bas == null) return 0;
                bas.close = DateTime.Now;
                bas.close_user = user;
                bas.close_comment = coment;
                return SaveRWBufferArrivalSostav(bas);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CloseRWBufferArrivalSostav(id={0}, user={1})", id, user), eventID);
                return -1;
            }

        }

        #endregion

        #region RWBufferSendingSostav Перенос отправленных на УЗ вагонов по данным КИС
        public IQueryable<RWBufferSendingSostav> RWBufferSendingSostav
        {
            get { return context_rw.RWBufferSendingSostav; }
        }

        public IQueryable<RWBufferSendingSostav> GetRWBufferSendingSostav()
        {
            try
            {
                return RWBufferSendingSostav;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetRWBufferSendingSostav()"), eventID);
                return null;
            }
        }

        public RWBufferSendingSostav GetRWBufferSendingSostav(int id)
        {
            try
            {
                return GetRWBufferSendingSostav().Where(s => s.id == id).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetRWBufferSendingSostav(id={0})", id), eventID);
                return null;
            }
        }

        public int SaveRWBufferSendingSostav(RWBufferSendingSostav RWBufferSendingSostav)
        {
            RWBufferSendingSostav dbEntry;
            try
            {
                if (RWBufferSendingSostav.id == 0)
                {
                    dbEntry = new RWBufferSendingSostav()
                    {
                        id = RWBufferSendingSostav.id,
                        datetime = RWBufferSendingSostav.datetime,
                        day = RWBufferSendingSostav.day,
                        month = RWBufferSendingSostav.month,
                        year = RWBufferSendingSostav.year,
                        hour = RWBufferSendingSostav.hour,
                        minute = RWBufferSendingSostav.minute,
                        natur = RWBufferSendingSostav.natur,
                        id_station_from_kis = RWBufferSendingSostav.id_station_from_kis,
                        id_station_on_kis = RWBufferSendingSostav.id_station_on_kis,
                        count_nathist = RWBufferSendingSostav.count_nathist,
                        count_set_nathist = RWBufferSendingSostav.count_set_nathist,
                        close = RWBufferSendingSostav.close,
                        close_user = RWBufferSendingSostav.close_user,
                        close_comment = RWBufferSendingSostav.close_comment,
                        status = RWBufferSendingSostav.status, 
                        list_wagons = RWBufferSendingSostav.list_wagons,
                        list_no_set_wagons = RWBufferSendingSostav.list_no_set_wagons,
                        message = RWBufferSendingSostav.message,
                    };
                    context_rw.RWBufferSendingSostav.Add(dbEntry);
                }
                else
                {
                    dbEntry = context_rw.RWBufferSendingSostav.Find(RWBufferSendingSostav.id);
                    if (dbEntry != null)
                    {
                        dbEntry.datetime = RWBufferSendingSostav.datetime;
                        dbEntry.day = RWBufferSendingSostav.day;
                        dbEntry.month = RWBufferSendingSostav.month;
                        dbEntry.year = RWBufferSendingSostav.year;
                        dbEntry.hour = RWBufferSendingSostav.hour;
                        dbEntry.minute = RWBufferSendingSostav.minute;
                        dbEntry.natur = RWBufferSendingSostav.natur;
                        dbEntry.id_station_from_kis = RWBufferSendingSostav.id_station_from_kis;
                        dbEntry.id_station_on_kis = RWBufferSendingSostav.id_station_on_kis;
                        dbEntry.count_nathist = RWBufferSendingSostav.count_nathist;
                        dbEntry.count_set_nathist = RWBufferSendingSostav.count_set_nathist;
                        dbEntry.close = RWBufferSendingSostav.close;
                        dbEntry.close_user = RWBufferSendingSostav.close_user;
                        dbEntry.close_comment = RWBufferSendingSostav.close_comment;
                        dbEntry.status = RWBufferSendingSostav.status;
                        dbEntry.list_wagons = RWBufferSendingSostav.list_wagons;
                        dbEntry.list_no_set_wagons = RWBufferSendingSostav.list_no_set_wagons;
                        dbEntry.message = RWBufferSendingSostav.message;
                    }
                }
                context_rw.SaveChanges();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveRWBufferSendingSostav(RWBufferSendingSostav={0})", RWBufferSendingSostav.GetFieldsAndValue()), eventID);
                return -1;
            }
            return dbEntry.id;
        }

        public RWBufferSendingSostav DeleteRWBufferSendingSostav(int id)
        {
            RWBufferSendingSostav dbEntry = context_rw.RWBufferSendingSostav.Find(id);
            if (dbEntry != null)
            {
                try
                {
                    context_rw.RWBufferSendingSostav.Remove(dbEntry);
                    context_rw.SaveChanges();
                }
                catch (Exception e)
                {
                    e.WriteErrorMethod(String.Format("DeleteRWBufferSendingSostav(id={0})", id), eventID);
                    return null;
                }
            }
            return dbEntry;
        }
        /// <summary>
        /// Вернуть последнее время по которому перенесли состав
        /// </summary>
        /// <returns></returns>
        public DateTime? GetLastDateTimeRWBufferSendingSostav()
        {
            try
            {
                RWBufferSendingSostav bas = GetRWBufferSendingSostav().OrderByDescending(a => a.datetime).FirstOrDefault();
                return bas != null ? (DateTime?)bas.datetime : null;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetLastDateTimeRWBufferSendingSostav()"), eventID);
                return null;
            }
        }
        /// <summary>
        /// Выбрать перенесеные составы за указанный период
        /// </summary>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        public IQueryable<RWBufferSendingSostav> GetRWBufferSendingSostav(DateTime start, DateTime stop)
        {
            try
            {
                return GetRWBufferSendingSostav().Where(o => o.datetime >= start & o.datetime <= stop);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetRWBufferSendingSostav(start={0}, stop={1})",start,stop), eventID);
                return null;
            }
        }
        /// <summary>
        /// Выбрать не закрытые перенесеные составы
        /// </summary>
        /// <returns></returns>
        public IQueryable<RWBufferSendingSostav> GetRWBufferSendingSostavNoClose()
        {
            try
            {
                return GetRWBufferSendingSostav().Where(o => o.close == null).OrderBy(o => o.datetime);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetRWBufferSendingSostavNoClose()"), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить строку по натурке
        /// </summary>
        /// <param name="natur"></param>
        /// <returns></returns>
        public RWBufferSendingSostav GetRWBufferSendingSostavOfNatur(int natur)
        {
            try
            {
                return GetRWBufferSendingSostav().Where(o => o.natur == natur).OrderByDescending(o => o.datetime).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetRWBufferSendingSostavOfNatur(natur={0})", natur), eventID);
                return null;
            }
        }
        /// <summary>
        /// Закрыть строку
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int CloseRWBufferSendingSostav(int id)
        {
            try
            {
                return CloseRWBufferSendingSostav(id, System.Environment.UserDomainName + @"\" + System.Environment.UserName);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CloseRWBufferSendingSostav(id={0})", id), eventID);
                return -1;
            }

        }

        public int CloseRWBufferSendingSostav(int id, string user)
        {
            try
            {
                RWBufferSendingSostav bas = GetRWBufferSendingSostav(id);
                if (bas == null) return 0;
                bas.close = DateTime.Now;
                bas.close_user = user;
                return SaveRWBufferSendingSostav(bas);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CloseRWBufferSendingSostav(id={0}, user={1})", id, user), eventID);
                return -1;
            }

        }

        public int CloseRWBufferSendingSostav(int id, string coment, string user)
        {
            try
            {
                RWBufferSendingSostav bas = GetRWBufferSendingSostav(id);
                if (bas == null) return 0;
                bas.close = DateTime.Now;
                bas.close_user = user;
                bas.close_comment = coment;
                return SaveRWBufferSendingSostav(bas);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CloseRWBufferArrivalSostav(id={0}, user={1})", id, user), eventID);
                return -1;
            }

        }

        #endregion

        #region RWBufferInputSostav Перенос вагонов по внутреним станциям по прибытию
        public IQueryable<RWBufferInputSostav> RWBufferInputSostav
        {
            get { return context_rw.RWBufferInputSostav; }
        }

        public IQueryable<RWBufferInputSostav> GetRWBufferInputSostav()
        {
            try
            {
                return RWBufferInputSostav;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetRWBufferInputSostav()"), eventID);
                return null;
            }
        }

        public RWBufferInputSostav GetRWBufferInputSostav(int id)
        {
            try
            {
                return GetRWBufferInputSostav().Where(s => s.id == id).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetRWBufferInputSostav(id={0})", id), eventID);
                return null;
            }
        }

        public int SaveRWBufferInputSostav(RWBufferInputSostav RWBufferInputSostav)
        {
            RWBufferInputSostav dbEntry;
            try
            {
                if (RWBufferInputSostav.id == 0)
                {
                    dbEntry = new RWBufferInputSostav()
                    {
                        id = 0,
                        datetime = RWBufferInputSostav.datetime ,
                        doc_num = RWBufferInputSostav.doc_num ,
                        id_station_from_kis = RWBufferInputSostav.id_station_from_kis ,
                        way_num_kis = RWBufferInputSostav.way_num_kis ,
                        napr = RWBufferInputSostav.napr ,
                        id_station_on_kis = RWBufferInputSostav.id_station_on_kis ,
                        count_wagons = RWBufferInputSostav.count_wagons ,
                        count_set_wagons = RWBufferInputSostav.count_set_wagons ,
                        natur = RWBufferInputSostav.natur ,
                        status = RWBufferInputSostav.status ,
                        list_wagons = RWBufferInputSostav.list_wagons ,
                        list_no_set_wagons = RWBufferInputSostav.list_no_set_wagons ,
                        message = RWBufferInputSostav.message ,
                        close = RWBufferInputSostav.close ,
                        close_user = RWBufferInputSostav.close_user ,
                        close_comment = RWBufferInputSostav.close_comment
                    };
                    context_rw.RWBufferInputSostav.Add(dbEntry);
                }
                else
                {
                    dbEntry = context_rw.RWBufferInputSostav.Find(RWBufferInputSostav.id);
                    if (dbEntry != null)
                    {
                        dbEntry.datetime = RWBufferInputSostav.datetime;
                        dbEntry.doc_num = RWBufferInputSostav.doc_num;
                        dbEntry.id_station_from_kis = RWBufferInputSostav.id_station_from_kis;
                        dbEntry.way_num_kis = RWBufferInputSostav.way_num_kis;
                        dbEntry.napr = RWBufferInputSostav.napr;
                        dbEntry.id_station_on_kis = RWBufferInputSostav.id_station_on_kis;
                        dbEntry.count_wagons = RWBufferInputSostav.count_wagons;
                        dbEntry.count_set_wagons = RWBufferInputSostav.count_set_wagons;
                        dbEntry.natur = RWBufferInputSostav.natur;
                        dbEntry.status = RWBufferInputSostav.status;
                        dbEntry.list_wagons = RWBufferInputSostav.list_wagons;
                        dbEntry.list_no_set_wagons = RWBufferInputSostav.list_no_set_wagons;
                        dbEntry.message = RWBufferInputSostav.message;
                        dbEntry.close = RWBufferInputSostav.close;
                        dbEntry.close_user = RWBufferInputSostav.close_user;
                        dbEntry.close_comment = RWBufferInputSostav.close_comment;
                    }
                }
                context_rw.SaveChanges();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveRWBufferInputSostav(RWBufferInputSostav={0})", RWBufferInputSostav.GetFieldsAndValue()), eventID);
                return -1;
            }
            return dbEntry.id;
        }

        public RWBufferInputSostav DeleteRWBufferInputSostav(int id)
        {
            RWBufferInputSostav dbEntry = context_rw.RWBufferInputSostav.Find(id);
            if (dbEntry != null)
            {
                try
                {
                    context_rw.RWBufferInputSostav.Remove(dbEntry);
                    context_rw.SaveChanges();
                }
                catch (Exception e)
                {
                    e.WriteErrorMethod(String.Format("DeleteRWBufferInputSostav(id={0})", id), eventID);
                    return null;
                }
            }
            return dbEntry;
        }
        /// <summary>
        /// Вернуть последнее время по которому перенесли состав
        /// </summary>
        /// <returns></returns>
        public DateTime? GetLastDateTimeRWBufferInputSostav()
        {
            try
            {
                RWBufferInputSostav bas = GetRWBufferInputSostav().OrderByDescending(a => a.datetime).FirstOrDefault();
                return bas != null ? (DateTime?)bas.datetime : null;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetLastDateTimeRWBufferInputSostav()"), eventID);
                return null;
            }
        }
        /// <summary>
        /// Выбрать перенесеные составы за указанный период
        /// </summary>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        public IQueryable<RWBufferInputSostav> GetRWBufferInputSostav(DateTime start, DateTime stop)
        {
            try
            {
                return GetRWBufferInputSostav().Where(o => o.datetime >= start & o.datetime <= stop);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetRWBufferInputSostav(start={0}, stop={1})",start,stop), eventID);
                return null;
            }
        }
        /// <summary>
        /// Выбрать не закрытые перенесеные составы
        /// </summary>
        /// <returns></returns>
        public IQueryable<RWBufferInputSostav> GetRWBufferInputSostavNoClose()
        {
            try
            {
                return GetRWBufferInputSostav().Where(o => o.close == null).OrderBy(o => o.datetime);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetRWBufferInputSostavNoClose()"), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить строку по натурке
        /// </summary>
        /// <param name="natur"></param>
        /// <returns></returns>
        public RWBufferInputSostav GetRWBufferInputSostavOfNatur(int natur)
        {
            try
            {
                return GetRWBufferInputSostav().Where(o => o.natur == natur).OrderByDescending(o => o.datetime).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetRWBufferInputSostavOfNatur(natur={0})", natur), eventID);
                return null;
            }
        }
        /// <summary>
        /// Закрыть строку  по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int CloseRWBufferInputSostav(int id)
        {
            try
            {
                return CloseRWBufferInputSostav(id, System.Environment.UserDomainName + @"\" + System.Environment.UserName);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CloseRWBufferInputSostav(id={0})", id), eventID);
                return -1;
            }

        }
        /// <summary>
        /// Закрыть строку  по id указав пользователя
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public int CloseRWBufferInputSostav(int id, string user)
        {
            try
            {
                RWBufferInputSostav bis = GetRWBufferInputSostav(id);
                if (bis == null) return 0;
                bis.close = DateTime.Now;
                bis.close_user = user;
                return SaveRWBufferInputSostav(bis);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CloseRWBufferInputSostav(id={0}, user={1})", id, user), eventID);
                return -1;
            }

        }
        /// <summary>
        /// Закрыть строку  по id указав пользователя и коментарий
        /// </summary>
        /// <param name="id"></param>
        /// <param name="coment"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public int CloseRWBufferInputSostav(int id, string coment, string user)
        {
            try
            {
                RWBufferInputSostav bis = GetRWBufferInputSostav(id);
                if (bis == null) return 0;
                bis.close = DateTime.Now;
                bis.close_user = user;
                bis.close_comment = coment;
                return SaveRWBufferInputSostav(bis);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CloseRWBufferInputSostav(id={0}, user={1})", id, user), eventID);
                return -1;
            }

        }

        #endregion

        #region RWBufferOutputSostav Перенос вагонов по внутреним станциям по отправке
        public IQueryable<RWBufferOutputSostav> RWBufferOutputSostav
        {
            get { return context_rw.RWBufferOutputSostav; }
        }

        public IQueryable<RWBufferOutputSostav> GetRWBufferOutputSostav()
        {
            try
            {
                return RWBufferOutputSostav;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetRWBufferOutputSostav()"), eventID);
                return null;
            }
        }

        public RWBufferOutputSostav GetRWBufferOutputSostav(int id)
        {
            try
            {
                return GetRWBufferOutputSostav().Where(s => s.id == id).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetRWBufferOutputSostav(id={0})", id), eventID);
                return null;
            }
        }

        public int SaveRWBufferOutputSostav(RWBufferOutputSostav RWBufferOutputSostav)
        {
            RWBufferOutputSostav dbEntry;
            try
            {
                if (RWBufferOutputSostav.id == 0)
                {
                    dbEntry = new RWBufferOutputSostav()
                    {
                        id = 0,
                        datetime = RWBufferOutputSostav.datetime,
                        doc_num = RWBufferOutputSostav.doc_num,
                        id_station_from_kis = RWBufferOutputSostav.id_station_from_kis,
                        way_num_kis = RWBufferOutputSostav.way_num_kis,
                        napr = RWBufferOutputSostav.napr,
                        id_station_on_kis = RWBufferOutputSostav.id_station_on_kis,
                        count_wagons = RWBufferOutputSostav.count_wagons,
                        count_set_wagons = RWBufferOutputSostav.count_set_wagons,
                        status = RWBufferOutputSostav.status,
                        list_wagons = RWBufferOutputSostav.list_wagons,
                        list_no_set_wagons = RWBufferOutputSostav.list_no_set_wagons,
                        message = RWBufferOutputSostav.message,
                        close = RWBufferOutputSostav.close,
                        close_user = RWBufferOutputSostav.close_user,
                        close_comment = RWBufferOutputSostav.close_comment
                    };
                    context_rw.RWBufferOutputSostav.Add(dbEntry);
                }
                else
                {
                    dbEntry = context_rw.RWBufferOutputSostav.Find(RWBufferOutputSostav.id);
                    if (dbEntry != null)
                    {
                        dbEntry.datetime = RWBufferOutputSostav.datetime;
                        dbEntry.doc_num = RWBufferOutputSostav.doc_num;
                        dbEntry.id_station_from_kis = RWBufferOutputSostav.id_station_from_kis;
                        dbEntry.way_num_kis = RWBufferOutputSostav.way_num_kis;
                        dbEntry.napr = RWBufferOutputSostav.napr;
                        dbEntry.id_station_on_kis = RWBufferOutputSostav.id_station_on_kis;
                        dbEntry.count_wagons = RWBufferOutputSostav.count_wagons;
                        dbEntry.count_set_wagons = RWBufferOutputSostav.count_set_wagons;
                        dbEntry.status = RWBufferOutputSostav.status;
                        dbEntry.list_wagons = RWBufferOutputSostav.list_wagons;
                        dbEntry.list_no_set_wagons = RWBufferOutputSostav.list_no_set_wagons;
                        dbEntry.message = RWBufferOutputSostav.message;
                        dbEntry.close = RWBufferOutputSostav.close;
                        dbEntry.close_user = RWBufferOutputSostav.close_user;
                        dbEntry.close_comment = RWBufferOutputSostav.close_comment;
                    }
                }
                context_rw.SaveChanges();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveRWBufferOutputSostav(RWBufferOutputSostav={0})", RWBufferOutputSostav.GetFieldsAndValue()), eventID);
                return -1;
            }
            return dbEntry.id;
        }

        public RWBufferOutputSostav DeleteRWBufferOutputSostav(int id)
        {
            RWBufferOutputSostav dbEntry = context_rw.RWBufferOutputSostav.Find(id);
            if (dbEntry != null)
            {
                try
                {
                    context_rw.RWBufferOutputSostav.Remove(dbEntry);
                    context_rw.SaveChanges();
                }
                catch (Exception e)
                {
                    e.WriteErrorMethod(String.Format("DeleteRWBufferOutputSostav(id={0})", id), eventID);
                    return null;
                }
            }
            return dbEntry;
        }
        /// <summary>
        /// Вернуть последнее время по которому перенесли состав
        /// </summary>
        /// <returns></returns>
        public DateTime? GetLastDateTimeRWBufferOutputSostav()
        {
            try
            {
                RWBufferOutputSostav bas = GetRWBufferOutputSostav().OrderByDescending(a => a.datetime).FirstOrDefault();
                return bas != null ? (DateTime?)bas.datetime : null;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetLastDateTimeRWBufferOutputSostav()"), eventID);
                return null;
            }
        }
        /// <summary>
        /// Выбрать перенесеные составы за указанный период
        /// </summary>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        public IQueryable<RWBufferOutputSostav> GetRWBufferOutputSostav(DateTime start, DateTime stop)
        {
            try
            {
                return GetRWBufferOutputSostav().Where(o => o.datetime >= start & o.datetime <= stop);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetRWBufferOutputSostav(start={0}, stop={1})", start, stop), eventID);
                return null;
            }
        }
        /// <summary>
        /// Выбрать не закрытые перенесеные составы
        /// </summary>
        /// <returns></returns>
        public IQueryable<RWBufferOutputSostav> GetRWBufferOutputSostavNoClose()
        {
            try
            {
                return GetRWBufferOutputSostav().Where(o => o.close == null).OrderBy(o => o.datetime);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetRWBufferOutputSostavNoClose()"), eventID);
                return null;
            }
        }
        /// <summary>
        /// Закрыть строку  по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int CloseRWBufferOutputSostav(int id)
        {
            try
            {
                return CloseRWBufferOutputSostav(id, System.Environment.UserDomainName + @"\" + System.Environment.UserName);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CloseRWBufferOutputSostav(id={0})", id), eventID);
                return -1;
            }

        }
        /// <summary>
        /// Закрыть строку  по id указав пользователя
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public int CloseRWBufferOutputSostav(int id, string user)
        {
            try
            {
                RWBufferOutputSostav bos = GetRWBufferOutputSostav(id);
                if (bos == null) return 0;
                bos.close = DateTime.Now;
                bos.close_user = user;
                return SaveRWBufferOutputSostav(bos);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CloseRWBufferOutputSostav(id={0}, user={1})", id, user), eventID);
                return -1;
            }

        }
        /// <summary>
        /// Закрыть строку  по id указав пользователя и коментарий
        /// </summary>
        /// <param name="id"></param>
        /// <param name="coment"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public int CloseRWBufferOutputSostav(int id, string coment, string user)
        {
            try
            {
                RWBufferOutputSostav bos = GetRWBufferOutputSostav(id);
                if (bos == null) return 0;
                bos.close = DateTime.Now;
                bos.close_user = user;
                bos.close_comment = coment;
                return SaveRWBufferOutputSostav(bos);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CloseRWBufferOutputSostav(id={0}, user={1})", id, user), eventID);
                return -1;
            }

        }

        #endregion

        #endregion
    }
}
