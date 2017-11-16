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

        #region KOMETA

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

        #region KometaSobstvForNakl
        /// <summary>
        /// Получить собственников по накладной
        /// </summary>
        /// <returns></returns>
        public IQueryable<KometaSobstvForNakl> GetSobstvForNakl()
        {
            try
            {
                return context.KometaSobstvForNakl;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetSobstvForNakl()"), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить собственников по накладной по коду
        /// </summary>
        /// <param name="kod_sob"></param>
        /// <returns></returns>
        public KometaSobstvForNakl GetSobstvForNakl(int kod_sob)
        {
            try
            {
                return GetSobstvForNakl().Where(s => s.SOBSTV == kod_sob).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetSobstvForNakl(kod_sob={0})", kod_sob), eventID);
                return null;
            }
        }
        #endregion

        #endregion

        #region PROM (Информация по станции промышленная)

        #region PROM.GRUZ_SP
        /// <summary>
        /// Получить все грузы
        /// </summary>
        /// <returns></returns>
        public IQueryable<PromGruzSP> GetGruzSP()
        {
            try
            {
                return context.PromGruzSP;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetGruzSP()"), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить груз по коду груза
        /// </summary>
        /// <param name="k_gruz"></param>
        /// <returns></returns>
        public PromGruzSP GetGruzSP(int k_gruz)
        {
            try
            {
                return GetGruzSP().Where(g => g.K_GRUZ == k_gruz).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetGruzSP(k_gruz={0})", k_gruz), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить код груза по коду ЕТСНГ (corect - false без коррекции кода ЕТСНГ, corect - true с коррекцией кода ЕТСНГ поиск по диапазону код0 ... код9)
        /// </summary>
        /// <param name="tar_gr"></param>
        /// <param name="corect"></param>
        /// <returns></returns>
        public PromGruzSP GetGruzSPToTarGR(int? tar_gr, bool corect)
        {
            try
            {
                if (!corect)
                {
                    return GetGruzSP().Where(g => g.TAR_GR == tar_gr).FirstOrDefault();
                }
                else
                {
                    return GetGruzSP().Where(g => g.TAR_GR >= tar_gr * 10 & g.TAR_GR <= (tar_gr * 10) + 9).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetGruzSPToTarGR(tar_gr={0}, corect={1})", tar_gr,corect), eventID);
                return null;
            }
        }

        #endregion

        #endregion


        #region NUM_VAG (Информация по вагонам)

        #region NumVagStpr1Gr (Справочник грузов по вагонам)
        /// <summary>
        /// список грузов
        /// </summary>
        /// <returns></returns>
        public IQueryable<NumVagStpr1Gr> GetSTPR1GR()
        {
            try
            {
                return context.NumVagStpr1Gr;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetSTPR1GR()"), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить груз по kod_gr
        /// </summary>
        /// <param name="kod_gr"></param>
        /// <returns></returns>
        public NumVagStpr1Gr GetSTPR1GR(int kod_gr)
        {
            try
            {
                return GetSTPR1GR().Where(g => g.KOD_GR == kod_gr).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetSTPR1GR(kod_gr={0})", kod_gr), eventID);
                return null;
            }
        }
        #endregion

        #endregion

    }
}
