using EFKIS.Abstract;
using EFKIS.Entities;
using MessageLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFKIS.Concrete
{
    public class rwCar
    {
        public int num { get; set; }
        public string rod { get; set; }
    }

    public class EFWagons:IKIS
    {
        private eventID eventID = eventID.EFWagons;

        protected EFDbContext context = new EFDbContext();
        
        #region KometaVagonSob  
      
        public IQueryable<KometaVagonSob> KometaVagonSob
        {
            get { return context.KometaVagonSob; }
        }
        /// <summary>
        /// Получить все вагоны
        /// </summary>
        /// <returns></returns>
        public IQueryable<KometaVagonSob> GetVagonsSob()
        {
            try
            {
                return KometaVagonSob;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetVagonsSob()"), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить список вагонов по указаному номеру
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public IQueryable<KometaVagonSob> GetVagonsSob(int num)
        {
            try
            {
                return GetVagonsSob().Where(c => c.N_VAGON == num).OrderByDescending(c => c.DATE_AR);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetVagonsSob(etsng={0})", num), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить список вагонов по указаному номеру с незаконченой арендой
        /// </summary>
        /// <param name="num"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public KometaVagonSob GetVagonsSob(int num, DateTime dt)
        {
            try
            {
                return GetVagonsSob(num).Where(v => v.DATE_AR <= dt & v.DATE_END == null).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetVagonsSob(num={0}, dt={1})", num, dt), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить список вагонов по которым поменяли владельца за указаную дату период до
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="day_period"></param>
        /// <returns></returns>
        public IQueryable<KometaVagonSob> GetChangeVagonsSob(DateTime dt, int day_period)
        {
            try
            {
                DateTime start_dt = dt.AddDays(-1 * day_period);
                return GetVagonsSob().Where(c => c.DATE_AR >= start_dt & c.DATE_AR <= dt).OrderBy(c => c.DATE_AR);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetChangeVagonsSob(dt={0}, day_period={1})", dt, day_period), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить список вагонов по которым поменяли владельца за указанный период
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="day_period"></param>
        /// <returns></returns>
        public IQueryable<KometaVagonSob> GetChangeVagonsSob(int day_period)
        {
            try
            {
                return GetChangeVagonsSob(DateTime.Now, day_period);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetChangeVagonsSob(day_period={0})", day_period), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить список всех вагонов
        /// </summary>
        /// <returns></returns>
        public List<rwCar> GetVagons()
        {
            List<rwCar> list = new List<rwCar>();
            try
            {
                List<IGrouping<int, KometaVagonSob>> group_list = new List<IGrouping<int, KometaVagonSob>>();
                group_list = GetVagonsSob().GroupBy(s => s.N_VAGON).ToList();
                foreach (IGrouping<int, KometaVagonSob> group_wag in group_list)
                {
                    KometaVagonSob kv = group_wag.OrderBy(v => v.ROD).FirstOrDefault();
                    if (kv != null)
                    {
                        list.Add(new rwCar() { num = kv.N_VAGON, rod = kv.ROD });
                    }
                }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetVagons()"), eventID);
                return null;
            }
            return list;
            //return rep_vsob.db.SqlQuery<rwCar>("SELECT [N_VAGON], [ROD] FROM KOMETA.VAGON_SOB group by [N_VAGON], [ROD]");
        }
        #endregion


    }
}
