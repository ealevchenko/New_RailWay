using EFMT.Abstract;
using MessageLog;
using EFMT.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libClass;
using System.Data.SqlClient;
using RWConversionFunctions;

namespace EFMT.Concrete
{
    public enum mtOperation : int { not = 0, coming = 1, tsp = 2 }

    public enum mtConsignee : int { AMKR = 1 }
    
    public class EFMetallurgTrans : IMT
    {
        private eventID eventID = eventID.EFMetallurgTrans;

        protected EFDbContext context = new EFDbContext();

        #region Составы на подходах

        #region ApproachesCars
        public IQueryable<ApproachesCars> ApproachesCars
        {
            get { return context.ApproachesCars; }
        }

        public IQueryable<ApproachesCars> GetApproachesCars()
        {
            try
            {
                return ApproachesCars;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetApproachesCars()"), eventID);
                return null;
            }
        }

        public ApproachesCars GetApproachesCars(int id)
        {
            try
            {
                return GetApproachesCars().Where(c => c.ID == id).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetApproachesCars(id={0})",id), eventID);
                return null;
            }
        }

        public int SaveApproachesCars(ApproachesCars ApproachesCars)
        {
            ApproachesCars dbEntry;
            try
            {
                if (ApproachesCars.ID == 0)
                {
                    dbEntry = new ApproachesCars()
                    {
                        ID = 0,
                        IDSostav = ApproachesCars.IDSostav,
                        CompositionIndex = ApproachesCars.CompositionIndex,
                        Num = ApproachesCars.Num,
                        CountryCode = ApproachesCars.CountryCode,
                        Weight = ApproachesCars.Weight,
                        CargoCode = ApproachesCars.CargoCode,
                        TrainNumber = ApproachesCars.TrainNumber,
                        Operation = ApproachesCars.Operation,
                        DateOperation = ApproachesCars.DateOperation,
                        CodeStationFrom = ApproachesCars.CodeStationFrom,
                        CodeStationOn = ApproachesCars.CodeStationOn,
                        CodeStationCurrent = ApproachesCars.CodeStationCurrent,
                        CountWagons = ApproachesCars.CountWagons,
                        SumWeight = ApproachesCars.SumWeight,
                        FlagCargo = ApproachesCars.FlagCargo,
                        Route = ApproachesCars.Route,
                        Owner = ApproachesCars.Owner,
                        NumDocArrival = ApproachesCars.NumDocArrival,
                        Arrival = ApproachesCars.Arrival,
                        ParentID = ApproachesCars.ParentID,
                        ApproachesSostav = ApproachesCars.ApproachesSostav
                    };
                    context.ApproachesCars.Add(dbEntry);
                }
                else
                {
                    dbEntry = context.ApproachesCars.Find(ApproachesCars.ID);
                    if (dbEntry != null)
                    {
                        dbEntry.IDSostav = ApproachesCars.IDSostav;
                        dbEntry.CompositionIndex = ApproachesCars.CompositionIndex;
                        dbEntry.Num = ApproachesCars.Num;
                        dbEntry.CountryCode = ApproachesCars.CountryCode;
                        dbEntry.Weight = ApproachesCars.Weight;
                        dbEntry.CargoCode = ApproachesCars.CargoCode;
                        dbEntry.TrainNumber = ApproachesCars.TrainNumber;
                        dbEntry.Operation = ApproachesCars.Operation;
                        dbEntry.DateOperation = ApproachesCars.DateOperation;
                        dbEntry.CodeStationFrom = ApproachesCars.CodeStationFrom;
                        dbEntry.CodeStationOn = ApproachesCars.CodeStationOn;
                        dbEntry.CodeStationCurrent = ApproachesCars.CodeStationCurrent;
                        dbEntry.CountWagons = ApproachesCars.CountWagons;
                        dbEntry.SumWeight = ApproachesCars.SumWeight;
                        dbEntry.FlagCargo = ApproachesCars.FlagCargo;
                        dbEntry.Route = ApproachesCars.Route;
                        dbEntry.Owner = ApproachesCars.Owner;
                        dbEntry.NumDocArrival = ApproachesCars.NumDocArrival;
                        dbEntry.ParentID = ApproachesCars.ParentID;
                        dbEntry.ApproachesSostav = ApproachesCars.ApproachesSostav;
                    }
                }
                context.SaveChanges();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveApproachesCars(ApproachesCars={0})", ApproachesCars.GetFieldsAndValue()), eventID);
                return -1;
            }
            return dbEntry.ID;
        }

        public ApproachesCars DeleteApproachesCars(int id)
        {
            ApproachesCars dbEntry = context.ApproachesCars.Find(id);
            if (dbEntry != null)
            {
                try
                {                
                context.ApproachesCars.Remove(dbEntry);

                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    e.WriteErrorMethod(String.Format("DeleteApproachesCars(id={0})", id), eventID);
                    return null;
                }
            }
            return dbEntry;
        }

        public int DeleteApproachesCarsOfSostav(int id_sostav)
        {
            try
            {
                SqlParameter IDSostav = new SqlParameter("@IDSostav", id_sostav);
                return context.Database.ExecuteSqlCommand("DELETE FROM MT.ApproachesCars WHERE IDSostav = @IDSostav", IDSostav);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("DeleteApproachesCarsOfSostav(id_sostav={0})", id_sostav), eventID);
                return -1;
            }
        }
        /// <summary>
        /// Вернуть вагоны по указанному id_sostav
        /// </summary>
        /// <param name="id_sostav"></param>
        /// <returns></returns>
        public IQueryable<ApproachesCars> GetApproachesCarsOfSostav(int id_sostav)
        {
            try
            {
                return GetApproachesCars().Where(c => c.IDSostav == id_sostav);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetApproachesCarsOfSostav(id_sostav={0})", id_sostav), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить список вагонов по номеру с сортировкой по поступлению
        /// </summary>
        /// <param name="num_car"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public IQueryable<ApproachesCars> GetApproachesCarsOfNumCar(int num_car, bool order)
        {
            try
            {
                if (!order)
                {
                    return GetApproachesCars().Where(c => c.Num == num_car).OrderBy(c => c.ID);
                }
                else
                {
                    return GetApproachesCars().Where(c => c.Num == num_car).OrderByDescending(c => c.ID);
                }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetApproachesCarsOfNumCar(num_car={0},order={1})", num_car, order), eventID);
                return null;
            }
        }

        /// <summary>
        /// Закрыть вагон на подходах установив номер документа и дату захода (поиск по номеру вагона и весу)
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="num"></param>
        /// <param name="dt"></param>
        /// <param name="weight"></param>
        /// <returns></returns>
        public int CloseApproachesCarsOfDocWeight(int doc, int num, DateTime dt, decimal? weight)
        {
            try
            {
                SqlParameter idoc = new SqlParameter("@doc", doc);
                SqlParameter inum = new SqlParameter("@num", num);
                SqlParameter dt_amkr = new SqlParameter("@dt", dt);
                SqlParameter iweight = new SqlParameter("@weight", Math.Round((weight != null ? (decimal)weight : 0), 0));

                return context.Database.ExecuteSqlCommand("UPDATE [MT].[ApproachesCars] SET NumDocArrival = @doc, Arrival = @dt" +
                                " where NumDocArrival is null and Arrival is null and Num=@num and DateOperation<=@dt and [Weight]=@weight", idoc, dt_amkr, inum, iweight);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CloseApproachesCarsOfDoc(doc={0}, num={1}, dt={2}, weight={3})", doc, num, dt, weight), eventID);
                return -1;
            }
        }
        /// <summary>
        /// Закрыть вагон на подходах установив номер документа и дату захода (поиск по номеру вагона и интервалу времени)
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="num"></param>
        /// <param name="dt"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public int CloseApproachesCarsOfDocDay(int doc, int num, DateTime dt, int day)
        {
            try
            {
                SqlParameter idoc = new SqlParameter("@doc", doc);
                SqlParameter inum = new SqlParameter("@num", num);
                SqlParameter dt_start = new SqlParameter("@dt_start", dt.AddDays(day * -1));
                SqlParameter dt_stop = new SqlParameter("@dt_stop", dt);

                return context.Database.ExecuteSqlCommand("UPDATE [MT].[ApproachesCars] SET NumDocArrival = @doc, Arrival = @dt_stop" +
                                " where NumDocArrival is null and Arrival is null and Num=@num and DateOperation>=@dt_start and DateOperation<=@dt_stop", idoc, dt_start, dt_stop, inum);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CloseArrivalCarsOfDoc(doc={0}, num={1}, dt={2}, day={3})", doc, num, dt, day), eventID);
                return -1;
            }
        }

        #endregion

        #region ApproachesSostav
        public IQueryable<ApproachesSostav> ApproachesSostav
        {
            get { return context.ApproachesSostav; }
        }

        public IQueryable<ApproachesSostav> GetApproachesSostav()
        {
            try
            {
                return ApproachesSostav;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetApproachesSostav()"), eventID);
                return null;
            }
        }

        public ApproachesSostav GetApproachesSostav(int id)
        {
            try
            {
                return GetApproachesSostav().Where(s => s.ID == id).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetApproachesSostav(id={0})", id), eventID);
                return null;
            }
        }

        public int SaveApproachesSostav(ApproachesSostav ApproachesSostav)
        {
            ApproachesSostav dbEntry;
            try
            {
                if (ApproachesSostav.ID == 0)
                {
                    dbEntry = new ApproachesSostav()
                    {
                        ID = 0,
                        FileName = ApproachesSostav.FileName,
                        CompositionIndex = ApproachesSostav.CompositionIndex,
                        DateTime = ApproachesSostav.DateTime,
                        Create = ApproachesSostav.Create,
                        Close = ApproachesSostav.Close,
                        Approaches = ApproachesSostav.Approaches,
                        ParentID = ApproachesSostav.ParentID,
                        ApproachesCars = ApproachesSostav.ApproachesCars,
                    };
                    context.ApproachesSostav.Add(dbEntry);
                }
                else
                {
                    dbEntry = context.ApproachesSostav.Find(ApproachesSostav.ID);
                    if (dbEntry != null)
                    {
                        dbEntry.FileName = ApproachesSostav.FileName;
                        dbEntry.CompositionIndex = ApproachesSostav.CompositionIndex;
                        dbEntry.DateTime = ApproachesSostav.DateTime;
                        dbEntry.Create = ApproachesSostav.Create;
                        dbEntry.Close = ApproachesSostav.Close;
                        dbEntry.Approaches = ApproachesSostav.Approaches;
                        dbEntry.ParentID = ApproachesSostav.ParentID;
                    }
                }
                context.SaveChanges();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveApproachesSostav(ApproachesSostav={0})", ApproachesSostav.GetFieldsAndValue()), eventID);
                return -1;
            }
            return dbEntry.ID;
        }

        public ApproachesSostav DeleteApproachesSostav(int id)
        {
            ApproachesSostav dbEntry = context.ApproachesSostav.Find(id);
            if (dbEntry != null)
            {
                try
                {
                    if (DeleteApproachesCarsOfSostav(dbEntry.ID) < 0) return null; // Удалить вагоны состава  
                    context.ApproachesSostav.Remove(dbEntry);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    e.WriteErrorMethod(String.Format("DeleteApproachesSostav(id={0})", id), eventID);
                    return null;
                }
            }
            return dbEntry;
        }
        /// <summary>
        /// Получить состав по имени файла
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public ApproachesSostav GetApproachesSostavOfFile(string file)
        {
            try
            {
                return GetApproachesSostav().Where(s => s.FileName == file).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetApproachesSostavOfFile(file={0})", file), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить незакрытый состав по индексу и дате
        /// </summary>
        /// <param name="index"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public ApproachesSostav GetNoCloseApproachesSostav(string index, DateTime date)
        {
            try
            {
                return GetApproachesSostav().Where(s => s.CompositionIndex == index & s.Close == null & s.Approaches == null & s.DateTime <= date).OrderByDescending(s => s.DateTime).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetNoCloseApproachesSostav(index={0}, date={1})", index, date), eventID); 
                return null;
            }
        }

        #endregion

        #endregion

        #region Составы на станциях УЗ КР

        #region ArrivalCars

        public IQueryable<ArrivalCars> ArrivalCars
        {
            get { return context.ArrivalCars; }
        }

        public IQueryable<ArrivalCars> GetArrivalCars()
        {
            try
            {
                return ArrivalCars;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetArrivalCars()"), eventID);
                return null;
            }
        }

        public ArrivalCars GetArrivalCars(int id)
        {
            try
            {
                return GetArrivalCars().Where(c => c.ID == id).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetArrivalCars(id={0})", id), eventID);
                return null;
            }
        }

        public int SaveArrivalCars(ArrivalCars ArrivalCars)
        {
            ArrivalCars dbEntry;
            try
            {
                if (ArrivalCars.ID == 0)
                {
                    dbEntry = new ArrivalCars()
                    {
                        ID = 0,
                        IDSostav = ArrivalCars.IDSostav,
                        Position = ArrivalCars.Position,
                        Num = ArrivalCars.Num,
                        CountryCode = ArrivalCars.CountryCode,
                        Weight = ArrivalCars.Weight,
                        CargoCode = ArrivalCars.CargoCode,
                        Cargo = ArrivalCars.Cargo,
                        StationCode = ArrivalCars.StationCode,
                        Station = ArrivalCars.Station,
                        Consignee = ArrivalCars.Consignee,
                        Operation = ArrivalCars.Operation,
                        CompositionIndex = ArrivalCars.CompositionIndex,
                        DateOperation = ArrivalCars.DateOperation,
                        TrainNumber = ArrivalCars.TrainNumber,
                        NumDocArrival = ArrivalCars.NumDocArrival,
                        Arrival = ArrivalCars.Arrival, 
                        ParentID = ArrivalCars.ParentID, 
                        ArrivalSostav = ArrivalCars.ArrivalSostav
                    };
                    context.ArrivalCars.Add(dbEntry);
                }
                else
                {
                    dbEntry = context.ArrivalCars.Find(ArrivalCars.ID);
                    if (dbEntry != null)
                    {
                        dbEntry.IDSostav = ArrivalCars.IDSostav;
                        dbEntry.Position = ArrivalCars.Position;
                        dbEntry.Num = ArrivalCars.Num;
                        dbEntry.CountryCode = ArrivalCars.CountryCode;
                        dbEntry.Weight = ArrivalCars.Weight;
                        dbEntry.CargoCode = ArrivalCars.CargoCode;
                        dbEntry.Cargo = ArrivalCars.Cargo;
                        dbEntry.StationCode = ArrivalCars.StationCode;
                        dbEntry.Station = ArrivalCars.Station;
                        dbEntry.Consignee = ArrivalCars.Consignee;
                        dbEntry.Operation = ArrivalCars.Operation;
                        dbEntry.CompositionIndex = ArrivalCars.CompositionIndex;
                        dbEntry.DateOperation = ArrivalCars.DateOperation;
                        dbEntry.TrainNumber = ArrivalCars.TrainNumber;
                        dbEntry.NumDocArrival = ArrivalCars.NumDocArrival;
                        dbEntry.Arrival = ArrivalCars.Arrival;
                        dbEntry.ParentID = ArrivalCars.ParentID;
                        dbEntry.ArrivalSostav = ArrivalCars.ArrivalSostav;
                    }
                }
                context.SaveChanges();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveArrivalCars(ArrivalCars={0})", ArrivalCars.GetFieldsAndValue()), eventID);
                return -1;
            }
            return dbEntry.ID;
        }

        public ArrivalCars DeleteArrivalCars(int id)
        {
            ArrivalCars dbEntry = context.ArrivalCars.Find(id);
            if (dbEntry != null)
            {
                try
                {
                    context.ArrivalCars.Remove(dbEntry);

                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    e.WriteErrorMethod(String.Format("DeleteArrivalCars(id={0})", id), eventID);
                    return null;
                }
            }
            return dbEntry;
        }

        public int DeleteArrivalCarsOfSostav(int id_sostav)
        {
            try
            {
                SqlParameter IDSostav = new SqlParameter("@IDSostav", id_sostav);
                return context.Database.ExecuteSqlCommand("DELETE FROM MT.ArrivalCars WHERE IDSostav = @IDSostav", IDSostav);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("DeleteArrivalCarsOfSostav(id_sostav={0})", id_sostav), eventID);
                return -1;
            }
        }

        public IQueryable<ArrivalCars> GetArrivalCarsOfSostav(int id_sostav)
        {
            try
            {
                return GetArrivalCars().Where(c => c.IDSostav == id_sostav);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetArrivalCarsOfSostav(id_sostav={0})", id_sostav), eventID);
                return null;
            }
        }

        /// <summary>
        /// Получить список вагонов по номеру с сортировкой по поступлению
        /// </summary>
        /// <param name="num_car"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public IQueryable<ArrivalCars> GetArrivalCarsOfNumCar(int num_car, bool order)
        {
            try
            {
                if (!order)
                {
                    return GetArrivalCars().Where(c => c.Num == num_car).OrderBy(c => c.ID);
                }
                else
                {
                    return GetArrivalCars().Where(c => c.Num == num_car).OrderByDescending(c => c.ID);
                }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetArrivalCarsOfNumCar(num_car={0},order={1})", num_car, order), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить строку вагона из состава по натурному листу, номеру вагона, дате захода на АМКР
        /// </summary>
        /// <param name="natur"></param>
        /// <param name="num_wag"></param>
        /// <param name="dt"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public ArrivalCars GetArrivalCarsToNatur(int natur, int num_wag, DateTime dt, int day)
        {
            DateTime dt_st = dt.AddDays(-1 * day);
            try
            {
                return GetArrivalCars().Where(l => l.NumDocArrival == natur & l.Num == num_wag & l.DateOperation >= dt_st & l.DateOperation <= dt).OrderByDescending(l => l.ID).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetArrivalCarsToNatur(natur={0}, num_wag={1}, dt={2}, day={3})", natur, num_wag, dt, day), eventID);
                return null;
            }
        }
        /// <summary>
        /// Закрыть вагон в прибытии установив номер документа и дату захода (поиск по номеру вагона и весу)
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="num"></param>
        /// <param name="dt"></param>
        /// <param name="weight"></param>
        /// <returns></returns>
        public int CloseArrivalCarsOfDocWeight(int doc, int num, DateTime dt, decimal? weight)
        {
            try
            {
                SqlParameter idoc = new SqlParameter("@doc", doc);
                SqlParameter inum = new SqlParameter("@num", num);
                SqlParameter dt_amkr = new SqlParameter("@dt", dt);
                SqlParameter iweight = new SqlParameter("@weight", Math.Round((weight!=null ? (decimal) weight: 0),0));

                return context.Database.ExecuteSqlCommand("UPDATE [MT].[ArrivalCars] SET NumDocArrival = @doc, Arrival = @dt" +
                                " where NumDocArrival is null and Arrival is null and Num=@num and DateOperation<=@dt and [Weight]=@weight", idoc, dt_amkr, inum, iweight);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CloseArrivalCarsOfDoc(doc={0}, num={1}, dt={2}, weight={3})", doc, num, dt, weight), eventID);
                return -1;
            }
        }
        /// <summary>
        /// Закрыть вагон в прибытии установив номер документа и дату захода (поиск по номеру вагона и интервалу времени)
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="num"></param>
        /// <param name="dt"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public int CloseArrivalCarsOfDocDay(int doc, int num, DateTime dt, int day)
        {
            try
            {
                SqlParameter idoc = new SqlParameter("@doc", doc);
                SqlParameter inum = new SqlParameter("@num", num);
                SqlParameter dt_start = new SqlParameter("@dt_start", dt.AddDays(day*-1));
                SqlParameter dt_stop = new SqlParameter("@dt_stop", dt);

                return context.Database.ExecuteSqlCommand("UPDATE [MT].[ArrivalCars] SET NumDocArrival = @doc, Arrival = @dt_stop" +
                                " where NumDocArrival is null and Arrival is null and Num=@num and DateOperation>=@dt_start and DateOperation<=@dt_stop", idoc, dt_start, dt_stop, inum );
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CloseArrivalCarsOfDoc(doc={0}, num={1}, dt={2}, day={3})", doc, num, dt, day), eventID);
                return -1;
            }
        }
        /// <summary>
        /// Вернуть список вагонов по укзанному составу с указаным кодом грузополучателя
        /// </summary>
        /// <param name="id_sostav"></param>
        /// <param name="Consignees"></param>
        /// <returns></returns>
        public List<ArrivalCars> GetArrivalCarsOfConsignees(int id_sostav, int[] Consignees)
        {
            try
            {
                string Consignees_s = Consignees.IntsToString(',');
                string sql = "SELECT * FROM MT.ArrivalCars where IDSostav = " + id_sostav.ToString() + " and [Consignee] in(" + Consignees_s + ")";
                return context.Database.SqlQuery<ArrivalCars>(sql).ToList();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetArrivalCarsOfConsignees(id_sostav={0}, Consignees={1})", id_sostav, Consignees.IntsToString(';')), eventID);                
                return null;
            }
        }

        #endregion

        #region ArrivalSostav

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
                return GetArrivalSostav().Where(c => c.ID == id).FirstOrDefault();
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
                if (ArrivalSostav.ID == 0)
                {
                    dbEntry = new ArrivalSostav()
                    {
                        ID = 0,
                        IDArrival = ArrivalSostav.IDArrival,
                        FileName = ArrivalSostav.FileName,
                        CompositionIndex = ArrivalSostav.CompositionIndex,
                        DateTime = ArrivalSostav.DateTime,
                        Operation = ArrivalSostav.Operation,
                        Create = ArrivalSostav.Create,
                        Close = ArrivalSostav.Close,
                        Arrival = ArrivalSostav.Arrival,
                        ParentID = ArrivalSostav.ParentID,
                        ArrivalCars = ArrivalSostav.ArrivalCars
                    };
                    context.ArrivalSostav.Add(dbEntry);
                }
                else
                {
                    dbEntry = context.ArrivalSostav.Find(ArrivalSostav.ID);
                    if (dbEntry != null)
                    {
                        dbEntry.IDArrival = ArrivalSostav.IDArrival;
                        dbEntry.FileName = ArrivalSostav.FileName;
                        dbEntry.CompositionIndex = ArrivalSostav.CompositionIndex;
                        dbEntry.DateTime = ArrivalSostav.DateTime;
                        dbEntry.Operation = ArrivalSostav.Operation;
                        dbEntry.Create = ArrivalSostav.Create;
                        dbEntry.Close = ArrivalSostav.Close;
                        dbEntry.Arrival = ArrivalSostav.Arrival;
                        dbEntry.ParentID = ArrivalSostav.ParentID;
                        dbEntry.ArrivalCars = ArrivalSostav.ArrivalCars;
                    }
                }
                context.SaveChanges();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveArrivalSostav(ArrivalSostav={0})", ArrivalSostav.GetFieldsAndValue()), eventID);
                return -1;
            }
            return dbEntry.ID;
        }

        public ArrivalSostav DeleteArrivalSostav(int id)
        {
            ArrivalSostav dbEntry = context.ArrivalSostav.Find(id);
            if (dbEntry != null)
            {
                try
                {
                    if (DeleteArrivalCarsOfSostav(dbEntry.ID) < 0) return null; // Удалить вагоны состава
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

        public ArrivalSostav GetArrivalSostavOfFile(string file)
        {
            try
            {
                return GetArrivalSostav().Where(s => s.FileName == file).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetArrivalSostavOfFile(file={0})", file), eventID);
                return null;
            }
        }

        public ArrivalSostav GetNoCloseArrivalSostav(string index, DateTime date)
        {
            try
            {
                return GetArrivalSostav().Where(s => s.CompositionIndex == index & s.Close == null & s.Arrival == null & s.DateTime <= date).OrderByDescending(s => s.DateTime).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetNoCloseArrivalSostav(index={0}, date={1})", index, date), eventID); 
                return null;
            }
        }
        /// <summary>
        /// Получить список составов пренадлежащих указанному коду прибытия с сортировкой
        /// </summary>
        /// <param name="id_arrival"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public IQueryable<ArrivalSostav> GetArrivalSostavOfIDArrival(int id_arrival, bool order)
        {
            try
            {
                if (!order)
                {
                    return GetArrivalSostav().Where(c => c.IDArrival == id_arrival).OrderBy(c => c.ID);
                }
                else
                {
                    return GetArrivalSostav().Where(c => c.IDArrival == id_arrival).OrderByDescending(c => c.ID);
                }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetArrivalSostavOfIDArrival(id_arrival={0}, order={1})", id_arrival, order), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить первую строку состава пренадлежащему указанному коду прибытия.
        /// </summary>
        /// <param name="id_arrival"></param>
        /// <returns></returns>
        public ArrivalSostav GetFirstArrivalSostavOfIDArrival(int id_arrival)
        {
            try
            {
                return GetArrivalSostav().Where(c => c.IDArrival == id_arrival & c.ParentID == null).FirstOrDefault();

            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetFirstArrivalSostavOfIDArrival(id_arrival={0})", id_arrival), eventID);
                return null;
            }
        }

        public int GetNextIDArrival()
        {
            int? id = context.Database.SqlQuery<int?>("SELECT max([IDArrival]) FROM [MT].[ArrivalSostav]").FirstOrDefault();
            return id != null ? (int)++id : 0;
        }

        #endregion

        #endregion

        #region Грузополучатели

        public IQueryable<Consignee> Consignee
        {
            get { return context.Consignee; }
        }

        public IQueryable<Consignee> GetConsignee()
        {
            try
            {
                return Consignee;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetConsignee()"), eventID);
                return null;
            }
        }

        public Consignee GetConsignee(int code)
        {
            try
            {
                return GetConsignee().Where(c => c.code == code).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetConsignee(code={0})", code), eventID);
                return null;
            }
        }

        public int SaveConsignee(Consignee Consignee)
        {
            Consignee dbEntry;
            try
            {
                dbEntry = context.Consignee.Find(Consignee.code);

                if (dbEntry == null)
                {
                    dbEntry = new Consignee()
                    {
                        code = Consignee.code,
                        description = Consignee.description,
                        consignee1 = Consignee.consignee1,
                        send = Consignee.send
                    };
                    context.Consignee.Add(dbEntry);
                }
                else
                {
                        dbEntry.description = Consignee.description;
                        dbEntry.consignee1 = Consignee.consignee1;
                        dbEntry.send = Consignee.send;
                }
                context.SaveChanges();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveConsignee(Consignee={0})", Consignee.GetFieldsAndValue()), eventID);
                return -1;
            }
            return dbEntry.code;
        }

        public Consignee DeleteConsignee(int code)
        {
            Consignee dbEntry = context.Consignee.Find(code);
            if (dbEntry != null)
            {
                try
                {
                    context.Consignee.Remove(dbEntry);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    e.WriteErrorMethod(String.Format("DeleteConsignee(code={0})", code), eventID);
                    return null;
                }
            }
            return dbEntry;
        }
        /// <summary>
        /// Получить список кодов грузополучателей основных или досылочных
        /// </summary>
        /// <param name="send"></param>
        /// <returns></returns>
        public IQueryable<Consignee> GetConsignee(bool send, mtConsignee Consignee)
        {
            try
            {
                return GetConsignee().Where(c=>c.send == send & c.consignee1 == (int)Consignee);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetConsignee(send={0}, Consignee={1})", send, Consignee), eventID);
                return null;
            }
        }

        public bool IsConsigneeSend(bool send, int code, mtConsignee Consignee) {
            Consignee consignee = GetConsignee().Where(c => c.send == send & c.code == code & c.consignee1 == (int)Consignee).FirstOrDefault();
            return consignee != null ? true : false;
        }
        /// <summary>
        /// Код пренадлежит грузополучателю 
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool IsConsignee(int Code, mtConsignee Consignee)
        {
            Consignee consignee = GetConsignee().Where(c => c.code == Code & c.consignee1 == (int)Consignee).FirstOrDefault();
            return consignee != null ? true : false;
        }
        /// <summary>
        /// Получить список кодов по указанному грузополучателю
        /// </summary>
        /// <param name="Consignee"></param>
        /// <returns></returns>
        public int[] GetListCodeConsigneeOfConsignee(mtConsignee Consignee)
        {
            try
            {
                List<int> codes = new List<int>();
                GetConsignee().Where(c => c.consignee1 == (int)Consignee).ToList().ForEach(c => codes.Add(c.code));
                return codes.ToArray();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetListCodeConsigneeOfConsignee(Consignee={0})", Consignee), eventID);
                return null;
            }

        }
        /// <summary>
        /// Получить список строк кодов указанного грузополучателя
        /// </summary>
        /// <param name="tmtc"></param>
        /// <returns></returns>
        public IQueryable<Consignee> GetConsignee(mtConsignee tmtc)
        {
            return GetConsignee().Where(c => c.consignee1 == (int)tmtc);
        }
        /// <summary>
        /// Получить список кодов указанного грузополучателя
        /// </summary>
        /// <param name="tmtc"></param>
        /// <returns></returns>
        public int[] GetConsigneeToCodes(mtConsignee tmtc)
        {
            return GetConsignee(tmtc).Select(c => c.code).ToArray();
        }

        #endregion







    }
}
