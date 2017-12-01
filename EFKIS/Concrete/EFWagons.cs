using EFKIS.Abstract;
using EFKIS.Entities;
using EFKIS.Helpers;
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

        #region KometaParkState

        public IQueryable<KometaParkState> KometaParkState
        {
            get { return context.KometaParkState; }
        }

        public IQueryable<KometaParkState> GetKometaParkState()
        {
            try
            {
                return KometaParkState;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetKometaParkState()"), eventID);
                return null;
            }
        }

        public IQueryable<KometaParkState> GetKometaParkState(DateTime Date)
        {
            DateTime date_start = Date.Date;
            DateTime date_stop = date_start.AddDays(1).AddSeconds(-1);
            try
            {
                return GetKometaParkState().Where(p => p.DATE_DOC >= date_start & p.DATE_DOC <= date_stop).OrderByDescending(p => p.DATE_DOC).OrderBy(p => p.K_STAN).OrderBy(p => p.RAIL).OrderBy(p => p.NN);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetKometaParkState(Date={0})", Date), eventID);
                return null;
            }
        }

        public List<IGrouping<string, KometaParkState>> GetKometaParkStateToStation(DateTime Date)
        {
            //List<IGrouping<int, KometaVagonSob>> group_list = new List<IGrouping<int, KometaVagonSob>>();
            //group_list = GetVagonsSob().GroupBy(s => s.N_VAGON).ToList();
            
            try
            {
                List<KometaParkState> list = GetKometaParkState(Date).ToList();
                return list.GroupBy(s => s.K_STAN).ToList();  
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetKometaParkStateToStation(Date={0})", Date), eventID);
                return null;
            }
        }

        public IQueryable<KometaParkState> GetKometaParkState(DateTime Date, int id_station)
        {
            try
            {
                List<KometaParkState> list = GetKometaParkState(Date).ToList();
                return list.Where(p => int.Parse(p.K_STAN) == id_station).AsQueryable();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetKometaParkState(Date={0}, id_station={1})",Date,id_station), eventID);
                return null;
            }
        }

        public List<IGrouping<string, KometaParkState>> GetKometaParkStateToWay(DateTime Date, int id_station)
        {
            try
            {
                List<KometaParkState> list = GetKometaParkState(Date, id_station).ToList();
                return list.GroupBy(s => s.RAIL).ToList();  
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetKometaParkState(Date={0}, id_station={1})",Date,id_station), eventID);
                return null;
            }
        }

        #endregion

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

        #region KometaStan

        public IQueryable<KometaStan> KometaStan
        {
            get { return context.KometaStan; }
        }
        /// <summary>
        /// Вернуть список станций
        /// </summary>
        /// <returns></returns>
        public IQueryable<KometaStan> GetKometaStan()
        {
            try
            {
                return KometaStan;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetKometaStan()"), eventID);
                return null;
            }
        }
        /// <summary>
        /// Вернуть станцию по коду
        /// </summary>
        /// <param name="k_stan"></param>
        /// <returns></returns>
        public KometaStan GetKometaStan(int k_stan)
        {
            try
            {
                return GetKometaStan().Where(s => s.K_STAN == k_stan).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetKometaStan(k_stan={0})",k_stan), eventID);
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

        #region PROM.SOSTAV

        public IQueryable<PromSostav> PromSostav
        {
            get { return context.PromSostav; }
        }

        public IQueryable<PromSostav> GetPromSostav()
        {
            try
            {
                return PromSostav;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetPromSostav()"), eventID);
                return null;
            }
        }
        /// <summary>
        /// Вернуть составы прибывшие на станцию промышленую
        /// </summary>
        /// <returns></returns>
        public IQueryable<PromSostav> GetInputPromSostav()
        {
            try
            {
                return GetPromSostav().Where(p => p.P_OT == 0 & p.V_P == 1 & p.K_ST != null);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetInputPromSostav()"), eventID);
                return null;
            }
        }
        /// <summary>
        /// Вернуть составы прибывшие на станцию промышленую за указанный период
        /// </summary>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        public IQueryable<PromSostav> GetInputPromSostav(DateTime start, DateTime stop)
        {
            try
            {
                return GetInputPromSostav().ToArray().FilterArrayOfFilterFromTo(Filters.IsGreaterOrequalToLessOrEqual, start, stop).AsQueryable();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetInputPromSostav(start={0}, stop={1})", start, stop), eventID);
                return null;
            }
        }
        /// <summary>
        /// Вернуть составы прибывшие на станцию промышленую за указанный период с сортировкой true - по убывания false - по возростанию
        /// </summary>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public IQueryable<PromSostav> GetInputPromSostav(DateTime start, DateTime stop, bool sort)
        {
            try
            {
                if (sort)
                {
                    return GetInputPromSostav(start, stop)
                        .OrderByDescending(p => p.D_YY)
                        .ThenByDescending(p => p.D_MM)
                        .ThenByDescending(p => p.D_DD)
                        .ThenByDescending(p => p.T_HH)
                        .ThenByDescending(p => p.T_MI);
                }
                else
                {
                    return GetInputPromSostav(start, stop)
                        .OrderBy(p => p.D_YY)
                        .ThenBy(p => p.D_MM)
                        .ThenBy(p => p.D_DD)
                        .ThenBy(p => p.T_HH)
                        .ThenBy(p => p.T_MI);
                }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetInputPromSostav(start={0}, stop={1}, sort={2})", start, stop, sort), eventID);
                return null;
            }

        }


        ///// <summary>
        ///// Выбрать строки с указанием направления
        ///// </summary>
        ///// <returns></returns>
        //public IQueryable<PromSostav> GetPromSostav(bool direction)
        //{
        //    try
        //    {
        //        string sql = "SELECT N_NATUR,D_DD,D_MM,D_YY,T_HH,T_MI,K_ST,N_PUT,NAPR,P_OT,V_P,K_ST_OTPR,K_ST_PR,N_VED_PR,N_SOST_OT,N_SOST_PR,DAT_VVOD FROM PROM.SOSTAV ";
        //        if (direction)
        //        {
        //            sql += "WHERE (P_OT = 1 and K_ST_PR is not null)";
        //        }
        //        else
        //        {

        //            sql += "WHERE ( P_OT = 0 and K_ST_OTPR is not null)";
        //        }

        //        return rep_ps.db.SqlQuery<PromSostav>(sql).AsQueryable();
        //    }
        //    catch (Exception e)
        //    {
        //        ServicesEventLog.LogError(e, "GetPromSostav(1)", eventID);
        //        return null;
        //    }
        //}
        ///// <summary>
        ///// Выбрать строки с указанием направления и временного диапазона
        ///// </summary>
        ///// <param name="start"></param>
        ///// <param name="stop"></param>
        ///// <param name="direction"></param>
        ///// <returns></returns>
        //public IQueryable<PromSostav> GetPromSostav(DateTime start, DateTime stop, bool direction)
        //{
        //    return GetPromSostav(direction).ToArray().FilterArrayOfFilterFromTo(Filters.IsGreaterOrequalToLessOrEqual, start, stop).AsQueryable();
        //}

        ///// <summary>
        ///// Вернуть все прибывшие составы
        ///// </summary>
        ///// <returns></returns>
        //public IQueryable<PromSostav> GetArrivalPromSostav()
        //{
        //    return rep_ps.PromSostav.Where(p => p.P_OT == 0 & p.K_ST_OTPR != null);
        //}
        ///// <summary>
        ///// Вернуть все отправленные составы
        ///// </summary>
        ///// <returns></returns>
        //public IQueryable<PromSostav> GetDeparturePromSostav()
        //{
        //    return rep_ps.PromSostav.Where(p => p.P_OT == 1 & p.K_ST_PR != null);
        //}

        ///// <summary>
        ///// Вернуть состав прибывший на станцию промышленую по натурке
        ///// </summary>
        ///// <param name="natur"></param>
        ///// <returns></returns>
        //public PromSostav GetInputPromSostavToNatur(int natur)
        //{
        //    return GetInputPromSostav().Where(p => p.N_NATUR == natur).FirstOrDefault();
        //}
        ///// <summary>
        ///// Вернуть состав прибывший на станцию промышленую по натурке и дате
        ///// </summary>
        ///// <param name="natur"></param>
        ///// <param name="station"></param>
        ///// <param name="day"></param>
        ///// <param name="month"></param>
        ///// <param name="year"></param>
        ///// <returns></returns>
        //public PromSostav GetArrivalPromSostavToNatur(int natur, int station, int day, int month, int year)
        //{
        //    return GetInputPromSostav().Where(p => p.N_NATUR == natur & p.K_ST == station & p.D_DD == day & p.D_MM == month & p.D_YY == year).FirstOrDefault();
        //}

        //public PromSostav GetInputPromSostavToNatur(int natur, int station, int day, int month, int year)
        //{
        //    return rep_ps.PromSostav.Where(p => p.N_NATUR == natur & p.K_ST == station & p.D_DD == day & p.D_MM == month & p.D_YY == year).FirstOrDefault();
        //}
        ///// <summary>
        ///// Вернуть все составы на станции промышленая за указанный период
        ///// </summary>
        ///// <param name="start"></param>
        ///// <param name="stop"></param>
        ///// <returns></returns>
        //public IQueryable<PromSostav> GetArrivalPromSostav(DateTime start, DateTime stop)
        //{
        //    return GetArrivalPromSostav().ToArray().FilterArrayOfFilterFromTo(Filters.IsGreaterOrequalToLessOrEqual, start, stop).AsQueryable();
        //}
        ///// <summary>
        ///// Вернуть все составы на станции промышленая за указанный период
        ///// </summary>
        ///// <param name="start"></param>
        ///// <param name="stop"></param>
        ///// <returns></returns>
        //public IQueryable<PromSostav> GetDeparturePromSostav(DateTime start, DateTime stop)
        //{
        //    return GetDeparturePromSostav().ToArray().FilterArrayOfFilterFromTo(Filters.IsGreaterOrequalToLessOrEqual, start, stop).AsQueryable();
        //}


        #endregion

        #region PROM.Nat_Hist

        public IQueryable<PromNatHist> PromNatHist
        {
            get { return context.PromNatHist; }
        }

        public IQueryable<PromNatHist> GetPromNatHist()
        {
            try
            {
                return PromNatHist;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetPromNatHist()"), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить список вагонов по натурному листу станции и дате поступления c сортировкой true- npp по убыванию false- npp по возрастанию
        /// </summary>
        /// <param name="natur"></param>
        /// <param name="station"></param>
        /// <param name="day"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public IQueryable<PromNatHist> GetNatHist(int natur, int station, int day, int month, int year, bool? sort)
        {
            try
            {
                if (sort == null)
                {
                    return GetPromNatHist().Where(n => n.N_NATUR == natur & n.K_ST == station & n.D_PR_DD == day & n.D_PR_MM == month & n.D_PR_YY == year);
                }
                if ((bool)sort)
                {
                    return GetPromNatHist().Where(n => n.N_NATUR == natur & n.K_ST == station & n.D_PR_DD == day & n.D_PR_MM == month & n.D_PR_YY == year).OrderByDescending(n => n.NPP);
                }
                else
                {
                    return GetPromNatHist().Where(n => n.N_NATUR == natur & n.K_ST == station & n.D_PR_DD == day & n.D_PR_MM == month & n.D_PR_YY == year).OrderBy(n => n.NPP);
                }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetVagon(natur={0}, station={1}, day={2}, month={3}, year={4}, sort={5})", natur, station, day, month, year, sort), eventID);
                return null;
            }

        }
        ///// <summary>
        ///// Получить список вагонов по натурному листу станции и дате поступления
        ///// </summary>
        ///// <param name="natur"></param>
        ///// <param name="station"></param>
        ///// <param name="day"></param>
        ///// <param name="month"></param>
        ///// <param name="year"></param>
        ///// <returns></returns>
        //public IQueryable<PromNatHist> GetNatHist(int natur, int station, int day, int month, int year)
        //{
        //    return GetNatHist(natur, station, day, month, year, null);
        //}
        ///// <summary>
        ///// Получить вагон по натурному листу станции и дате поступления
        ///// </summary>
        ///// <param name="natur"></param>
        ///// <param name="station"></param>
        ///// <param name="day"></param>
        ///// <param name="month"></param>
        ///// <param name="year"></param>
        ///// <param name="wag"></param>
        ///// <returns></returns>
        //public PromNatHist GetNatHist(int natur, int station, int day, int month, int year, int wag)
        //{
        //    return GetNatHist(natur, station, day, month, year, null).Where(h => h.N_VAG == wag).FirstOrDefault();
        //}
        ///// <summary>
        ///// Получить количество вагонов по натурному листу станции и дате поступления
        ///// </summary>
        ///// <param name="natur"></param>
        ///// <param name="station"></param>
        ///// <param name="day"></param>
        ///// <param name="month"></param>
        ///// <param name="year"></param>
        ///// <returns></returns>
        //public int? CountWagonsNatHist(int natur, int station, int day, int month, int year)
        //{
        //    IQueryable<PromNatHist> pnh = GetNatHist(natur, station, day, month, year);
        //    if (pnh == null) return null;
        //    return pnh.Count();
        //}
        ///// <summary>
        ///// Получить список вагонов по номеру вагона
        ///// </summary>
        ///// <param name="num_vag"></param>
        ///// <returns></returns>
        //public IQueryable<PromNatHist> GetNatHistOfVagon(int num_vag)
        //{
        //    return GetNatHist().Where(n => n.N_VAG == num_vag);
        //}
        ///// <summary>
        ///// Получить список вагонов по номеру вагона и датой прибытия меньше указаной даты
        ///// </summary>
        ///// <param name="num_vag"></param>
        ///// <param name="start"></param>
        ///// <returns></returns>
        //public IQueryable<PromNatHist> GetNatHistOfVagonLess(int num_vag, DateTime start)
        //{
        //    return GetNatHistOfVagon(num_vag).ToArray().FilterArrayOfFilterFrom(Filters.IsLessOrEqual, start).AsQueryable();
        //}
        ///// <summary>
        ///// Получить список вагонов по номеру вагона и датой прибытия меньше указаной даты с сортировкой
        ///// </summary>
        ///// <param name="num_vag"></param>
        ///// <param name="start"></param>
        ///// <param name="sort"></param>
        ///// <returns></returns>
        //public IQueryable<PromNatHist> GetNatHistOfVagonLess(int num_vag, DateTime start, bool sort)
        //{
        //    if (sort)
        //    {
        //        return GetNatHistOfVagonLess(num_vag, start)
        //            .OrderByDescending(p => p.D_PR_YY)
        //            .ThenByDescending(p => p.D_PR_MM)
        //            .ThenByDescending(p => p.D_PR_DD)
        //            .ThenByDescending(p => p.T_PR_HH)
        //            .ThenByDescending(p => p.T_PR_MI);
        //    }
        //    else
        //    {
        //        return GetNatHistOfVagonLess(num_vag, start)
        //            .OrderBy(p => p.D_PR_YY)
        //            .ThenBy(p => p.D_PR_MM)
        //            .ThenBy(p => p.D_PR_DD)
        //            .ThenBy(p => p.T_PR_HH)
        //            .ThenBy(p => p.T_PR_MI);
        //    }

        //}

        #endregion

        #region PROM.Vagon

        public IQueryable<PromVagon> PromVagon
        {
            get { return context.PromVagon; }
        }
        /// <summary>
        /// Получить список всех вагонов станции Промышленная
        /// </summary>
        /// <returns></returns>
        public IQueryable<PromVagon> GetVagon()
        {
            try
            {
                return PromVagon;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetVagon()"), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить список вагонов по натурному листу станции и дате поступления c сортировкой true- npp по убыванию false- npp по возрастанию
        /// </summary>
        /// <param name="natur"></param>
        /// <param name="station"></param>
        /// <param name="day"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public IQueryable<PromVagon> GetVagon(int natur, int station, int day, int month, int year, bool? sort)
        {
            try
            {
                if (sort == null)
                {
                    return GetVagon().Where(n => n.N_NATUR == natur & n.K_ST == station & n.D_PR_DD == day & n.D_PR_MM == month & n.D_PR_YY == year);
                }
                if ((bool)sort)
                {
                    return GetVagon().Where(n => n.N_NATUR == natur & n.K_ST == station & n.D_PR_DD == day & n.D_PR_MM == month & n.D_PR_YY == year).OrderByDescending(n => n.NPP);
                }
                else
                {
                    return GetVagon().Where(n => n.N_NATUR == natur & n.K_ST == station & n.D_PR_DD == day & n.D_PR_MM == month & n.D_PR_YY == year).OrderBy(n => n.NPP);
                }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetVagon(natur={0}, station={1}, day={2}, month={3}, year={4}, sort={5})", natur, station, day, month, year, sort), eventID);
                return null;
            }

        }
        /// <summary>
        /// Получить список вагонов по натурному листу станции и дате поступления
        /// </summary>
        /// <param name="natur"></param>
        /// <param name="station"></param>
        /// <param name="day"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public IQueryable<PromVagon> GetVagon(int natur, int station, int day, int month, int year)
        {
            try
            {
                return GetVagon(natur, station, day, month, year, null);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetVagon(natur={0}, station={1}, day={2}, month={3}, year={4})", natur, station, day, month, year), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить количество вагонов по натурному листу станции и дате поступления
        /// </summary>
        /// <param name="natur"></param>
        /// <param name="station"></param>
        /// <param name="day"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public int? CountWagonsVagon(int natur, int station, int day, int month, int year)
        {

            try
            {
                IQueryable<PromVagon> pv = GetVagon(natur, station, day, month, year);
                return pv == null ? (int? )pv.Count(): null;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CountWagonsVagon(natur={0}, station={1}, day={2}, month={3}, year={4})", natur, station, day, month, year), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить информацию по вагону
        /// </summary>
        /// <param name="natur"></param>
        /// <param name="station"></param>
        /// <param name="day"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public PromVagon GetVagon(int natur, int station, int day, int month, int year, int num)
        {
            try
            {
                return GetVagon(natur, station, day, month, year).Where(p => p.N_VAG == num).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetVagon(natur={0}, station={1}, day={2}, month={3}, year={4}, num={5})", natur, station, day, month, year, num), eventID);
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
