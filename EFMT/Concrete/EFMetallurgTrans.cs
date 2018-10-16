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
using System.Data;

namespace EFMT.Concrete
{
    public enum mtOperation : int { not = 0, coming = 1, tsp = 2 }

    public enum mtConsignee : int { AMKR = 1 }

    //public class HistoryArrivalCars
    //{
    //    public int ID { get; set; }
    //    public int IDSostav { get; set; }
    //    public int Num { get; set; }
    //    public int CountryCode { get; set; }
    //    public float Weight { get; set; }
    //    public int CargoCode { get; set; }
    //    public string Cargo { get; set; }
    //    public int StationCode { get; set; }
    //    public string Station { get; set; }
    //    public int Consignee { get; set; }
    //    public string CompositionIndex { get; set; }
    //    public DateTime DateStart { get; set; }
    //    public DateTime DateStop { get; set; }
    //    public int TrainNumber { get; set; }
    //    public int? NumDocArrival { get; set; }
    //    public DateTime? Arrival { get; set; }
    //    public string UserName { get; set; }
    //    public List<ArrivalCars> list_operation { get; set; }
    //}

    public class HistoryArrivalCars
    {
        public List<ArrivalCars> list_operation { get; set; }
    }

    public class EFMetallurgTrans : IMT
    {
        private eventID eventID = eventID.EFMetallurgTrans;

        protected EFDbContext context = new EFDbContext();

        public System.Data.Entity.Database Database
        {
            get { return context.Database; }
        }

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
                e.WriteErrorMethod(String.Format("GetApproachesCars(id={0})", id), eventID);
                return null;
            }
        }

        public int SaveApproachesCars(ApproachesCars ApproachesCars)
        {
            //EFDbContext context1 = new EFDbContext();
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
                        ApproachesSostav = ApproachesCars.ApproachesSostav,
                        UserName = ApproachesCars.UserName
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
                        dbEntry.Arrival = ApproachesCars.Arrival;
                        dbEntry.ParentID = ApproachesCars.ParentID;
                        //dbEntry.ApproachesSostav = ApproachesCars.ApproachesSostav;
                        dbEntry.UserName = ApproachesCars.UserName;
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
                SqlParameter user_name = new SqlParameter("@UserName", System.Environment.UserDomainName + @"\" + System.Environment.UserName);

                return context.Database.ExecuteSqlCommand("UPDATE [MT].[ApproachesCars] SET NumDocArrival = @doc, Arrival = @dt, UserName = @UserName" +
                                " where NumDocArrival is null and Arrival is null and Num=@num and DateOperation<=@dt and [Weight]=@weight", idoc, dt_amkr, inum, iweight, user_name);
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
                SqlParameter user_name = new SqlParameter("@UserName", System.Environment.UserDomainName + @"\" + System.Environment.UserName);

                return context.Database.ExecuteSqlCommand("UPDATE [MT].[ApproachesCars] SET NumDocArrival = @doc, Arrival = @dt_stop , UserName = @UserName" +
                                " where NumDocArrival is null and Arrival is null and Num=@num and DateOperation>=@dt_start and DateOperation<=@dt_stop", idoc, dt_start, dt_stop, inum, user_name);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CloseArrivalCarsOfDoc(doc={0}, num={1}, dt={2}, day={3})", doc, num, dt, day), eventID);
                return -1;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<DateTime> GroupDateApproachesCars()
        {
            try
            {
                return context.Database.SqlQuery<DateTime>("SELECT Distinct(Convert(date,DateOperation,120)) FROM MT.ApproachesCars where(Arrival IS NULL) ORDER BY Convert(date, DateOperation, 120) DESC").ToList();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GroupDateApproachesCars()"), eventID);
                return null;
            }
        }
        /// <summary>
        /// Показать не закрытые вагоны
        /// </summary>
        /// <returns></returns>
        public IQueryable<ApproachesCars> GetNoCloseApproachesCars()
        {
            try
            {
                return GetApproachesCars().Where(c => c.Arrival == null);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetNotCloseApproachesCars()"), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить следущую запис по указанному вагону выше указанной даты операции
        /// </summary>
        /// <param name="num_car"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        public ApproachesCars GetApproachesCarsOfNextCar(int num_car, DateTime operation)
        {
            try
            {
                return GetApproachesCars().Where(c => c.Num == num_car & c.DateOperation > operation).OrderBy(c => c.DateOperation).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetApproachesCarsOfNextCar(num_car={0},operation={1})", num_car, operation), eventID);
                return null;
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
        /// <summary>
        /// Получить следующую строку
        /// </summary>
        /// <param name="parent_id"></param>
        /// <returns></returns>
        public ApproachesSostav GetApproachesSostavOfParentID(int parent_id)
        {
            try
            {
                return GetApproachesSostav().Where(s => s.ParentID == parent_id).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetApproachesSostavOfParentID(parent_id={0})", parent_id), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить список операций над составом
        /// </summary>
        /// <param name="id_sostav"></param>
        /// <param name="destinct"></param>
        /// <returns></returns>
        public List<ApproachesSostav> GetApproachesSostavLocation(int id_sostav, bool destinct)
        {
            try
            {
                List<ApproachesSostav> list = new List<ApproachesSostav>();
                ApproachesSostav sostav = GetApproachesSostav(id_sostav);
                if (sostav != null)
                {
                    list.Add(sostav);
                    if (destinct)
                    {
                        GetApproachesSostavLocationDestinct(ref list, sostav);
                    }
                    else { GetApproachesSostavLocation(ref list, sostav); }
                }
                return list;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetApproachesSostavLocation(id_sostav={0}, destinct={1})", id_sostav, destinct), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить список операций для состава от последней к первой
        /// </summary>
        /// <param name="list"></param>
        /// <param name="id_sostav"></param>
        protected void GetApproachesSostavLocationDestinct(ref List<ApproachesSostav> list, ApproachesSostav sostav)
        {
            try
            {
                if (sostav == null || sostav.ParentID == null) return;
                ApproachesSostav csostav = GetApproachesSostav((int)sostav.ParentID);
                if (csostav != null)
                {
                    list.Add(csostav);
                    GetApproachesSostavLocationDestinct(ref list, csostav);
                }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetApproachesSostavLocationDestinct(list={0}, sostav={1})", list, sostav), eventID);
                return;
            }
        }
        /// <summary>
        /// Получить список операций для состава от первой к последней
        /// </summary>
        /// <param name="list"></param>
        /// <param name="sostav"></param>
        protected void GetApproachesSostavLocation(ref List<ApproachesSostav> list, ApproachesSostav sostav)
        {
            try
            {
                if (sostav == null) return;
                ApproachesSostav csostav = GetApproachesSostavOfParentID((int)sostav.ID);
                if (csostav != null)
                {
                    list.Add(csostav);
                    GetApproachesSostavLocation(ref list, csostav);
                }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetApproachesSostavLocation(list={0}, sostav={1})", list, sostav), eventID);
                return;
            }
        }
        /// <summary>
        /// Получить список количества натурок txt- файлы
        /// </summary>
        /// <returns></returns>
        public List<CountNaturList> GetCountApproachesNaturList()
        {
            try
            {
                int type = 1;
                SqlParameter i_type = new SqlParameter("@type", type);
                List<CountNaturList> list = this.Database.SqlQuery<CountNaturList>("EXEC [MT].[GetCountNaturList] @type", i_type).ToList();
                return list;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetCountApproachesNaturList()"), eventID);
                return null;
            }
        }

        #endregion

        #endregion

        #region Составы на станциях УЗ КР

        #region ArrivalCars

        public string GetListArrivalCarsToString(List<ArrivalCars> list)
        {
            if (list == null) return null;
            string res = null;
            foreach (ArrivalCars car in list)
            {
                res += car.Num.ToString() + ";";
            }
            return res;
        }

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
                        ArrivalSostav = ArrivalCars.ArrivalSostav,
                        UserName = ArrivalCars.UserName
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
                        dbEntry.UserName = ArrivalCars.UserName;
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
        /// Получить вагон по id состава и номеру вагона
        /// </summary>
        /// <param name="id_sostav"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public ArrivalCars GetArrivalCarsOfSostavNum(int id_sostav, int num)
        {
            try
            {
                return GetArrivalCars().Where(c => c.IDSostav == id_sostav & c.Num == num).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetArrivalCarsOfSostavNum(id_sostav={0}, num={0})", id_sostav, num), eventID);
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
                SqlParameter iweight = new SqlParameter("@weight", Math.Round((weight != null ? (decimal)weight : 0), 0));
                SqlParameter user_name = new SqlParameter("@UserName", System.Environment.UserDomainName + @"\" + System.Environment.UserName);

                return context.Database.ExecuteSqlCommand("UPDATE [MT].[ArrivalCars] SET NumDocArrival = @doc, Arrival = @dt , UserName = @UserName" +
                                " where NumDocArrival is null and Arrival is null and Num=@num and DateOperation<=@dt and [Weight]=@weight", idoc, dt_amkr, inum, iweight, user_name);
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
                SqlParameter dt_start = new SqlParameter("@dt_start", dt.AddDays(day * -1));
                SqlParameter dt_stop = new SqlParameter("@dt_stop", dt);
                SqlParameter user_name = new SqlParameter("@UserName", System.Environment.UserDomainName + @"\" + System.Environment.UserName);
                return context.Database.ExecuteSqlCommand("UPDATE [MT].[ArrivalCars] SET NumDocArrival = @doc, Arrival = @dt_stop , UserName = @UserName" +
                                " where NumDocArrival is null and Arrival is null and Num=@num and DateOperation>=@dt_start and DateOperation<=@dt_stop", idoc, dt_start, dt_stop, inum, user_name);
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
                string sql = "SELECT * FROM MT.ArrivalCars where IDSostav = " + id_sostav.ToString() + " and [Consignee] in(" + (Consignees_s != null ? Consignees_s : "0") + ")";
                return context.Database.SqlQuery<ArrivalCars>(sql).ToList();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetArrivalCarsOfConsignees(id_sostav={0}, Consignees={1})", id_sostav, Consignees.IntsToString(';')), eventID);
                return null;
            }
        }

        public List<ArrivalCars> GetArrivalCarsOfConsignees(int[] Consignees)
        {
            try
            {
                string Consignees_s = Consignees.IntsToString(',');
                string sql = "SELECT * FROM MT.ArrivalCars where [Consignee] in(" + (Consignees_s != null ? Consignees_s : "0") + ")";
                return context.Database.SqlQuery<ArrivalCars>(sql).ToList();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetArrivalCarsOfConsignees(Consignees={0})", Consignees.IntsToString(';')), eventID);
                return null;
            }
        }
        /// <summary>
        /// Найти все записи по указаному вагону ниже указанной даты
        /// </summary>
        /// <param name="num"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public IQueryable<ArrivalCars> GetArrivalCars(int num, DateTime dt)
        {
            try
            {
                return GetArrivalCars().Where(c => c.Num == num & c.DateOperation <= dt);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetArrivalCars(num={0}, dt={1})", num, dt), eventID);
                return null;
            }
        }
        /// <summary>
        /// Показать следующую запись
        /// </summary>
        /// <param name="num"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public ArrivalCars GetArrivalCarsOfNextCar(int num, DateTime dt)
        {
            try
            {
                return GetArrivalCars().Where(c => c.Num == num & c.DateOperation > dt).OrderBy(c => c.DateOperation).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetArrivalCars(num={0}, dt={1})", num, dt), eventID);
                return null;
            }
        }
        /// <summary>
        /// Закрыть не закрытую последнюю запись по указаному вагону ниже указанной даты 
        /// </summary>
        /// <param name="num"></param>
        /// <param name="dt"></param>
        /// <param name="code_close"></param>
        /// <returns></returns>
        public int CloseArrivalCars(int num, DateTime dt, int code_close)
        {
            try
            {
                ArrivalCars car = GetArrivalCars(num, dt).Where(c => c.NumDocArrival == null).OrderByDescending(c => c.DateOperation).FirstOrDefault();
                if (car != null)
                {
                    car.NumDocArrival = code_close;
                    car.Arrival = dt;
                    car.UserName = System.Environment.UserDomainName + @"\" + System.Environment.UserName;
                    return SaveArrivalCars(car);
                }
                return 0;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CloseArrivalCars(num={0}, dt={1}, code_close={2})", num, dt, code_close), eventID);
                return -1;
            }
        }
        /// <summary>
        /// Закрыть прибытие вагона из станции УЗ КР
        /// </summary>
        /// <param name="id_sostav"></param>
        /// <param name="num"></param>
        /// <param name="doc"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public int CloseArrivalCars(int id_sostav, int num, int doc, DateTime dt)
        {
            try
            {
                ArrivalCars arr_car = GetArrivalCarsOfSostavNum(id_sostav, num);
                if (arr_car != null)
                {
                    arr_car.Arrival = dt;
                    arr_car.NumDocArrival = doc;
                    return SaveArrivalCars(arr_car);
                }
                return 0;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CloseArrivalCars(id_sostav={0}, num={1}, doc={2}, dt={3})", id_sostav, num, doc, dt), eventID);
                return -1;
            }
        }
        /// <summary>
        /// Найти следующий вагон
        /// </summary>
        /// <param name="parent_id"></param>
        /// <returns></returns>
        public ArrivalCars GetArrivalCarsOfParentID(int parent_id)
        {
            try
            {
                return GetArrivalCars().Where(c => c.ParentID == parent_id).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetArrivalCarsOfParentID(id={0})", parent_id), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить первые прибытия вагонов
        /// </summary>
        /// <param name="num_car"></param>
        /// <returns></returns>
        public IQueryable<ArrivalCars> GetFirstArrivalCarsOfNum(int num_car)
        {
            try
            {
                return GetArrivalCars().Where(c => c.Num == num_car & c.ParentID == null).OrderBy(c => c.ID);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetFirstArrivalCarsOfNum(num_car={0})", num_car), eventID);
                return null;
            }
        }
        /// <summary>
        /// Сформировать историю прибытия вагонов на УЗ Кривого Рога
        /// </summary>
        /// <param name="car"></param>
        /// <param name="history_car"></param>
        protected void GetHistoryArrivalCarsOfNum(ArrivalCars car, ref List<ArrivalCars> history_car)
        {
            history_car.Add(car);
            ArrivalCars next = GetArrivalCarsOfParentID(car.ID);
            if (next != null)
            {
                GetHistoryArrivalCarsOfNum(next, ref history_car);
            }
        }
        /// <summary>
        /// Получить историю прибытия вагонов на УЗ Кривого Рога
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public List<HistoryArrivalCars> GetHistoryArrivalCarsOfNum(int num)
        {
            try
            {
                List<ArrivalCars> first_list = GetFirstArrivalCarsOfNum(num).ToList();
                List<HistoryArrivalCars> history_list = new List<HistoryArrivalCars>();
                foreach (ArrivalCars car in first_list)
                {
                    List<ArrivalCars> history_car = new List<ArrivalCars>();
                    GetHistoryArrivalCarsOfNum(car, ref history_car);
                    history_list.Add(new HistoryArrivalCars() { list_operation = history_car });

                }
                return history_list;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetHistoryArrivalCarsOfNum(num={0})", num), eventID);
                return null;
            }
        }
        /// <summary>
        /// Удалить из списка ArrivalCars, объект с указаным номером вагона
        /// </summary>
        /// <param name="list"></param>
        /// <param name="num_car"></param>
        /// <returns></returns>
        public bool DeleteExistArrivalCars(ref List<ArrivalCars> list, int num_car)
        {
            bool Result = false;
            try
            {
                if (list == null) return false;

                int index = list.Count() - 1;
                while (index >= 0)
                {
                    if (list[index].Num == num_car)
                    {
                        list.RemoveAt(index);
                        Result = true;
                    }
                    index--;
                }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("DeleteExistArrivalCars(list={0}, num_car={1})", list, num_car), eventID);
            }
            return Result;
        }
        /// <summary>
        /// Удалить совпадения двух списков ArrivalCars (проверка по номеру вагона)
        /// </summary>
        /// <param name="list_new"></param>
        /// <param name="list_old"></param>
        public void RemoveMatchingArrivalCars(ref List<ArrivalCars> list_new, ref List<ArrivalCars> list_old)
        {
            try
            {
                if (list_new == null & list_old == null) return;
                int index = list_new.Count() - 1;
                while (index >= 0)
                {
                    if (DeleteExistArrivalCars(ref list_old, list_new[index].Num))
                    {
                        list_new.RemoveAt(index);
                    }
                    index--;
                }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("RemoveMatchingArrivalCars(list_new={0}, list_old={1})", list_new, list_old), eventID);
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
                        //dbEntry.ArrivalCars = ArrivalSostav.ArrivalCars;
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
        /// <summary>
        /// Показать не закрытые составы по индексу и дате
        /// </summary>
        /// <param name="index"></param>
        /// <param name="date"></param>
        /// <returns></returns>
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
        /// Показать не закрытые составы по индексу и дате и рериоду отбора суток
        /// </summary>
        /// <param name="index"></param>
        /// <param name="date"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public ArrivalSostav GetNoCloseArrivalSostav(string index, DateTime date, int period)
        {
            try
            {
                DateTime data_start = date.AddDays(-1 * period);
                return GetArrivalSostav().Where(s => s.CompositionIndex == index & s.Close == null & s.Arrival == null & s.DateTime >= data_start & s.DateTime <= date).OrderByDescending(s => s.DateTime).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetNoCloseArrivalSostav(index={0}, date={1},  period={2})", index, date, period), eventID);
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
        /// <summary>
        /// Получить список составов по указаному коду прибытия
        /// </summary>
        /// <param name="id_arrival"></param>
        /// <returns></returns>
        public IQueryable<ArrivalSostav> GetArrivalSostavOfIDArrival(int id_arrival)
        {
            try
            {
                return GetArrivalSostav().Where(c => c.IDArrival == id_arrival).OrderBy(c => c.ID);

            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetArrivalSostavOfIDArrival(id_arrival={0})", id_arrival), eventID);
                return null;
            }
        }
        /// <summary>
        /// Вернуть список отсутсвующих номеров вагонов в новом составе
        /// </summary>
        /// <param name="id_sostav"></param>
        /// <returns></returns>
        public List<int> GetNotCarsOfOldArrivalSostav(int id_sostav)
        {
            try
            {
                return GetNotCarsOfOldArrivalSostav(GetArrivalSostav(id_sostav));
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetNotCarsOfOldArrivalSostav(id_sostav={0})", id_sostav), eventID);
                return null;
            }
        }
        /// <summary>
        /// Вернуть список отсутсвующих номеров вагонов в новом составе 
        /// </summary>
        /// <param name="sostav"></param>
        /// <returns></returns>
        public List<int> GetNotCarsOfOldArrivalSostav(ArrivalSostav sostav)
        {
            try
            {
                List<int> list_num_car = new List<int>();
                if (sostav.ParentID == null) return list_num_car;
                ArrivalSostav sostav_old = GetArrivalSostav((int)sostav.ParentID);
                foreach (ArrivalCars car in sostav_old.ArrivalCars.ToList())
                {
                    if (sostav.ArrivalCars.ToList().Find(c => c.Num == car.Num) == null)
                    {
                        list_num_car.Add(car.Num);
                    }
                }
                return list_num_car;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetNotCarsOfOldArrivalSostav(sostav={0})", sostav.GetFieldsAndValue()), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить список количества натурок xml- файлы
        /// </summary>
        /// <returns></returns>
        public List<CountNaturList> GetCountArrivalNaturList()
        {
            try
            {
                int type = 0;
                SqlParameter i_type = new SqlParameter("@type", type);
                List<CountNaturList> list = this.Database.SqlQuery<CountNaturList>("EXEC [MT].[GetCountNaturList] @type", i_type).ToList();
                return list;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetCountArrivalNaturList()"), eventID);
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
                return GetConsignee().Where(c => c.send == send & c.consignee1 == (int)Consignee);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetConsignee(send={0}, Consignee={1})", send, Consignee), eventID);
                return null;
            }
        }

        public bool IsConsigneeSend(bool send, int code, mtConsignee Consignee)
        {
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



        #region CountCarsOfSostav
        /// <summary>
        /// Вернуть список составов по которым есть не закрытые вагоны
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public List<CountCarsOfSostav> GetNoCloseArrivalCarsOfStationUZ(int code)
        {
            try
            {
                return GetArrivalCarsOfStationUZ(code, true);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetNoCloseArrivalCarsOfStationUZ(code={0})", code), eventID);
                return null;
            }
        }
        /// <summary>
        /// Вернуть список составов по станциям УЗ КР с выбором показать все или только не закрытые
        /// </summary>
        /// <param name="code"></param>
        /// <param name="close"></param>
        /// <returns></returns>
        public List<CountCarsOfSostav> GetArrivalCarsOfStationUZ(int code, bool close)
        {
            try
            {
                SqlParameter icode = new SqlParameter("@code", code);
                SqlParameter bclose = new SqlParameter("@close", close);
                return context.Database.SqlQuery<CountCarsOfSostav>("EXEC [MT].[GetArrivalCarsOfStationUZ] @code, @close", icode, bclose).ToList();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetArrivalCarsOfStationUZ(code={0}, close={0})", code, close), eventID);
                return null;
            }
        }
        #endregion

        #region WT Контроль собственного парка
        #region WagonsTracking

        public IQueryable<WagonsTracking> WagonsTracking
        {
            get { return context.WagonsTracking; }
        }

        public IQueryable<WagonsTracking> GetWagonsTracking()
        {
            try
            {
                return WagonsTracking;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetWagonsTracking()"), eventID);
                return null;
            }
        }

        public WagonsTracking GetWagonsTracking(int id)
        {
            try
            {
                return GetWagonsTracking().Where(t => t.id == id).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetWagonsTracking(id={0})", id), eventID);
                return null;
            }
        }

        public int SaveWagonsTracking(WagonsTracking WagonsTracking)
        {
            //EFDbContext context1 = new EFDbContext();
            WagonsTracking dbEntry;
            try
            {
                if (WagonsTracking.id == 0)
                {
                    dbEntry = new WagonsTracking()
                    {
                        id = WagonsTracking.id,
                        nvagon = WagonsTracking.nvagon,
                        st_disl = WagonsTracking.st_disl,
                        nst_disl = WagonsTracking.nst_disl,
                        kodop = WagonsTracking.kodop,
                        nameop = WagonsTracking.nameop,
                        full_nameop = WagonsTracking.full_nameop,
                        dt = WagonsTracking.dt,
                        st_form = WagonsTracking.st_form,
                        nst_form = WagonsTracking.nst_form,
                        idsost = WagonsTracking.idsost,
                        nsost = WagonsTracking.nsost,
                        st_nazn = WagonsTracking.st_nazn,
                        nst_nazn = WagonsTracking.nst_nazn,
                        ntrain = WagonsTracking.ntrain,
                        st_end = WagonsTracking.st_end,
                        nst_end = WagonsTracking.nst_end,
                        kgr = WagonsTracking.kgr,
                        nkgr = WagonsTracking.nkgr,
                        id_cargo = WagonsTracking.id_cargo,
                        kgrp = WagonsTracking.kgrp,
                        ves = WagonsTracking.ves,
                        updated = WagonsTracking.updated,
                        kgro = WagonsTracking.kgro,
                        km = WagonsTracking.km,
                    };
                    context.WagonsTracking.Add(dbEntry);
                }
                else
                {
                    dbEntry = context.WagonsTracking.Find(WagonsTracking.id);
                    if (dbEntry != null)
                    {
                        dbEntry.nvagon = WagonsTracking.nvagon;
                        dbEntry.st_disl = WagonsTracking.st_disl;
                        dbEntry.nst_disl = WagonsTracking.nst_disl;
                        dbEntry.kodop = WagonsTracking.kodop;
                        dbEntry.nameop = WagonsTracking.nameop;
                        dbEntry.full_nameop = WagonsTracking.full_nameop;
                        dbEntry.dt = WagonsTracking.dt;
                        dbEntry.st_form = WagonsTracking.st_form;
                        dbEntry.nst_form = WagonsTracking.nst_form;
                        dbEntry.idsost = WagonsTracking.idsost;
                        dbEntry.nsost = WagonsTracking.nsost;
                        dbEntry.st_nazn = WagonsTracking.st_nazn;
                        dbEntry.nst_nazn = WagonsTracking.nst_nazn;
                        dbEntry.ntrain = WagonsTracking.ntrain;
                        dbEntry.st_end = WagonsTracking.st_end;
                        dbEntry.nst_end = WagonsTracking.nst_end;
                        dbEntry.kgr = WagonsTracking.kgr;
                        dbEntry.nkgr = WagonsTracking.nkgr;
                        dbEntry.id_cargo = WagonsTracking.id_cargo;
                        dbEntry.kgrp = WagonsTracking.kgrp;
                        dbEntry.ves = WagonsTracking.ves;
                        dbEntry.updated = WagonsTracking.updated;
                        dbEntry.kgro = WagonsTracking.kgro;
                        dbEntry.km = WagonsTracking.km;
                    }
                }
                context.SaveChanges();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveWagonsTracking(WagonsTracking={0})", WagonsTracking.GetFieldsAndValue()), eventID);
                return -1;
            }
            return dbEntry.id;
        }

        public WagonsTracking DeleteWagonsTracking(int id)
        {
            WagonsTracking dbEntry = context.WagonsTracking.Find(id);
            if (dbEntry != null)
            {
                try
                {
                    context.WagonsTracking.Remove(dbEntry);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    e.WriteErrorMethod(String.Format("DeleteWagonsTracking(id={0})", id), eventID);
                    return null;
                }
            }
            return dbEntry;
        }
        /// <summary>
        /// Вернуть список операций по номеру вагона
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public IQueryable<WagonsTracking> GetWagonsTrackingOfNumCars(int num)
        {
            try
            {
                return GetWagonsTracking().Where(t => t.nvagon == num);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetWagonsTrackingOfNumCars(num={0})", num), eventID);
                return null;
            }
        }
        #endregion

        #region ListWagonsTracking
        public IQueryable<ListWagonsTracking> ListWagonsTracking
        {
            get { return context.ListWagonsTracking; }
        }

        public IQueryable<ListWagonsTracking> GetListWagonsTracking()
        {
            try
            {
                return ListWagonsTracking;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetListWagonsTracking()"), eventID);
                return null;
            }
        }

        public ListWagonsTracking GetListWagonsTracking(int nvagon)
        {
            try
            {
                return GetListWagonsTracking().Where(t => t.nvagon == nvagon).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetListWagonsTracking(nvagon={0})", nvagon), eventID);
                return null;
            }
        }

        public int SaveListWagonsTracking(ListWagonsTracking ListWagonsTracking)
        {
            ListWagonsTracking dbEntry;
            try
            {
                dbEntry = context.ListWagonsTracking.Find(ListWagonsTracking.nvagon);
                if (dbEntry == null)
                {
                    dbEntry = new ListWagonsTracking()
                    {
                        nvagon = ListWagonsTracking.nvagon,
                        SAP = ListWagonsTracking.SAP,
                        Description = ListWagonsTracking.Description,
                        //report1 = ListWagonsTracking.report1,
                        WTCarsReports = ListWagonsTracking.WTCarsReports,
                    };
                    context.ListWagonsTracking.Add(dbEntry);
                }
                else
                {
                    dbEntry.nvagon = ListWagonsTracking.nvagon;
                    dbEntry.SAP = ListWagonsTracking.SAP;
                    dbEntry.Description = ListWagonsTracking.Description;
                    //dbEntry.report1 = ListWagonsTracking.report1;
                    dbEntry.WTCarsReports = ListWagonsTracking.WTCarsReports;
                }
                context.SaveChanges();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveListWagonsTracking(ListWagonsTracking={0})", ListWagonsTracking.GetFieldsAndValue()), eventID);
                return -1;
            }
            return dbEntry.nvagon;
        }

        public ListWagonsTracking DeleteListWagonsTracking(int nvagon)
        {
            ListWagonsTracking dbEntry = context.ListWagonsTracking.Find(nvagon);
            if (dbEntry != null)
            {
                try
                {
                    context.ListWagonsTracking.Remove(dbEntry);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    e.WriteErrorMethod(String.Format("DeleteListWagonsTracking(nvagon={0})", nvagon), eventID);
                    return null;
                }
            }
            return dbEntry;
        }
        #endregion

        #region WTReports
        public IQueryable<WTReports> WTReports
        {
            get { return context.WTReports; }
        }

        public IQueryable<WTReports> GetWTReports()
        {
            try
            {
                return WTReports;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetWTReports()"), eventID);
                return null;
            }
        }

        public WTReports GetWTReports(int id)
        {
            try
            {
                return GetWTReports().Where(t => t.id == id).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetWTReports(id={0})", id), eventID);
                return null;
            }
        }

        public int SaveWTReports(WTReports WTReports)
        {
            WTReports dbEntry;
            try
            {
                if (WTReports.id == 0)
                {
                    dbEntry = new WTReports()
                    {
                        id = WTReports.id,
                        Report = WTReports.Report,
                        Description = WTReports.Description,
                        WTCarsReports = WTReports.WTCarsReports,
                    };
                    context.WTReports.Add(dbEntry);
                }
                else
                {
                    dbEntry = context.WTReports.Find(WTReports.id);
                    if (dbEntry != null)
                    {
                        dbEntry.Report = WTReports.Report;
                        dbEntry.Description = WTReports.Description;
                        dbEntry.WTCarsReports = WTReports.WTCarsReports;
                    }
                }
                context.SaveChanges();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveWTReports(WTReports={0})", WTReports.GetFieldsAndValue()), eventID);
                return -1;
            }
            return dbEntry.id;
        }

        public WTReports DeleteWTReports(int id)
        {
            WTReports dbEntry = context.WTReports.Find(id);
            if (dbEntry != null)
            {
                try
                {
                    context.WTReports.Remove(dbEntry);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    e.WriteErrorMethod(String.Format("DeleteWTReports(id={0})", id), eventID);
                    return null;
                }
            }
            return dbEntry;
        }
        #endregion

        #region WTCarsReports
        public IQueryable<WTCarsReports> WTCarsReports
        {
            get { return context.WTCarsReports; }
        }

        public IQueryable<WTCarsReports> GetWTCarsReports()
        {
            try
            {
                return WTCarsReports;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetWTCarsReports()"), eventID);
                return null;
            }
        }

        public WTCarsReports GetWTCarsReports(int id)
        {
            try
            {
                return GetWTCarsReports().Where(t => t.id == id).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetWTCarsReports(id={0})", id), eventID);
                return null;
            }
        }

        public int SaveWTCarsReports(WTCarsReports WTCarsReports)
        {
            WTCarsReports dbEntry;
            try
            {
                if (WTCarsReports.id == 0)
                {
                    dbEntry = new WTCarsReports()
                    {
                        id = WTCarsReports.id,
                        id_wtreport = WTCarsReports.id_wtreport,
                        nvagon = WTCarsReports.nvagon,
                        ListWagonsTracking = WTCarsReports.ListWagonsTracking,
                        WTReports = WTCarsReports.WTReports,
                    };
                    context.WTCarsReports.Add(dbEntry);
                }
                else
                {
                    dbEntry = context.WTCarsReports.Find(WTCarsReports.id);
                    if (dbEntry != null)
                    {
                        dbEntry.id_wtreport = WTCarsReports.id_wtreport;
                        dbEntry.nvagon = WTCarsReports.nvagon;
                        dbEntry.ListWagonsTracking = WTCarsReports.ListWagonsTracking;
                        dbEntry.WTReports = WTCarsReports.WTReports;
                    }
                }
                context.SaveChanges();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveWTCarsReports(WTCarsReports={0})", WTCarsReports.GetFieldsAndValue()), eventID);
                return -1;
            }
            return dbEntry.id;
        }

        public WTCarsReports DeleteWTCarsReports(int id)
        {
            WTCarsReports dbEntry = context.WTCarsReports.Find(id);
            if (dbEntry != null)
            {
                try
                {
                    context.WTCarsReports.Remove(dbEntry);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    e.WriteErrorMethod(String.Format("DeleteWTCarsReports(id={0})", id), eventID);
                    return null;
                }
            }
            return dbEntry;
        }
        #endregion

        #region WTCycle
        public IQueryable<WTCycle> WTCycle
        {
            get { return context.WTCycle; }
        }

        public IQueryable<WTCycle> GetWTCycle()
        {
            try
            {
                return WTCycle;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetWTCycle()"), eventID);
                return null;
            }
        }

        public WTCycle GetWTCycle(int id)
        {
            try
            {
                return GetWTCycle().Where(t => t.id == id).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetWTCycle(id={0})", id), eventID);
                return null;
            }
        }

        public IQueryable<WTCycle> GetWTCycleOfNumCar(int num)
        {
            try
            {
                return GetWTCycle().Where(t => t.WagonsTracking.nvagon == num);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetWTCycleOfNumCar(num={0})", num), eventID);
                return null;
            }
        }

        public int SaveWTCycle(WTCycle WTCycle)
        {
            WTCycle dbEntry;
            try
            {
                if (WTCycle.id == 0)
                {
                    dbEntry = new WTCycle()
                    {
                        id = WTCycle.id,
                        id_wt = WTCycle.id_wt,
                        cycle = WTCycle.cycle,
                        route = WTCycle.route,
                        station_from = WTCycle.station_from,
                        station_end = WTCycle.station_end,
                        WagonsTracking = WTCycle.WagonsTracking
                    };
                    context.WTCycle.Add(dbEntry);
                }
                else
                {
                    dbEntry = context.WTCycle.Find(WTCycle.id);
                    if (dbEntry != null)
                    {
                        dbEntry.id_wt = WTCycle.id_wt;
                        dbEntry.cycle = WTCycle.cycle;
                        dbEntry.route = WTCycle.route;
                        dbEntry.station_from = WTCycle.station_from;
                        dbEntry.station_end = WTCycle.station_end;
                        dbEntry.WagonsTracking = WTCycle.WagonsTracking;
                    }
                }
                context.SaveChanges();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveWTCycle(WTCycle={0})", WTCycle.GetFieldsAndValue()), eventID);
                return -1;
            }
            return dbEntry.id;
        }

        public WTCycle DeleteWTCycle(int id)
        {
            WTCycle dbEntry = context.WTCycle.Find(id);
            if (dbEntry != null)
            {
                try
                {
                    context.WTCycle.Remove(dbEntry);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    e.WriteErrorMethod(String.Format("DeleteWTCycle(id={0})", id), eventID);
                    return null;
                }
            }
            return dbEntry;
        }
        #endregion

        //#region CycleWagonsTracking
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="id_report"></param>
        ///// <param name="start"></param>
        ///// <param name="stop"></param>
        ///// <returns></returns>
        //public List<CycleWagonsTracking> GetCycleWagonsTrackingOfReports(int id_report, DateTime start, DateTime stop)
        //{
        //    try
        //    {
        //        SqlParameter i_id_report = new SqlParameter("@idreport", id_report);
        //        SqlParameter dt_start = new SqlParameter("@start", start);
        //        SqlParameter dt_stop = new SqlParameter("@stop", stop);

        //        List<CycleWagonsTracking> list = this.Database.SqlQuery<CycleWagonsTracking>("EXEC [MT].[GetCycleDBWagonsTrackingOfCarsReports] @idreport, @start, @stop", i_id_report, dt_start, dt_stop).ToList();
        //        return list;

        //    }
        //    catch (Exception e)
        //    {
        //        e.WriteErrorMethod(String.Format("GetCycleWagonsTrackingOfReports(id_report={0},start ={1},stop ={2})", id_report,start,stop), eventID);
        //        return null;
        //    }
        //}
        //#endregion

        #region RouteWagonTracking
        /// <summary>
        /// Вернуть все маршруты по выбранным вагонам за указаный период
        /// </summary>
        /// <param name="id_report"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        public List<RouteWagonTracking> GetRouteWagonTrackingOfReports(int id_report, DateTime start, DateTime stop)
        {
            try
            {
                SqlParameter i_id_report = new SqlParameter("@idreport", id_report);
                SqlParameter dt_start = new SqlParameter("@start", start);
                SqlParameter dt_stop = new SqlParameter("@stop", stop);

                List<RouteWagonTracking> list = this.Database.SqlQuery<RouteWagonTracking>("EXEC [MT].[GetRouteWagonsTrackingOfCarsReports] @idreport, @start, @stop", i_id_report, dt_start, dt_stop).ToList();
                return list;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetRouteWagonTrackingOfReports(id_report={0},start ={1},stop ={2})", id_report, start, stop), eventID);
                return null;
            }
        }
        /// <summary>
        /// Вернуть последний маршрут по выбранным вагонам за указаный период
        /// </summary>
        /// <param name="id_report"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        public List<RouteWagonTracking> GetLastRouteWagonTrackingOfReports(int id_report, DateTime start, DateTime stop)
        {
            try
            {
                SqlParameter i_id_report = new SqlParameter("@idreport", id_report);
                SqlParameter dt_start = new SqlParameter("@start", start);
                SqlParameter dt_stop = new SqlParameter("@stop", stop);

                List<RouteWagonTracking> list = this.Database.SqlQuery<RouteWagonTracking>("EXEC [MT].[GetLastRouteWagonsTrackingOfCarsReports] @idreport, @start, @stop", i_id_report, dt_start, dt_stop).ToList();
                return list;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetLastRouteWagonTrackingOfReports(id_report={0},start ={1},stop ={2})", id_report, start, stop), eventID);
                return null;
            }
        }

        public List<CurentWagonTracking> GetLastWagonTrackingOfReports(int id_report, DateTime start, DateTime stop)
        {
            try
            {
                List<CurentWagonTracking> list_result = new List<CurentWagonTracking>();
                List<RouteWagonTracking> list = GetLastRouteWagonTrackingOfReports(id_report, start, stop);
                List<IGrouping<string, RouteWagonTracking>> gr_station = list.GroupBy(g => g.name_station_group).ToList();
                foreach (IGrouping<string, RouteWagonTracking> gr in gr_station)
                {

                    int? count_norma_1 = gr.Where(r => r.route == 1 & r.time_left >= 0).Count();
                    int? count_norma_2 = gr.Where(r => r.route == 2 & r.time_left >= 0).Count();
                    int? count_norma_3 = gr.Where(r => r.route == 3 & r.time_left >= 0).Count();
                    int? count_norma_4 = gr.Where(r => r.route == 4 & r.time_left >= 0).Count();
                    int? count_surplus_1 = gr.Where(r => r.route == 1 & r.time_left < 0).Count();
                    int? count_surplus_2 = gr.Where(r => r.route == 2 & r.time_left < 0).Count();
                    int? count_surplus_3 = gr.Where(r => r.route == 3 & r.time_left < 0).Count();
                    int? count_surplus_4 = gr.Where(r => r.route == 4 & r.time_left < 0).Count();
                    int? count_1 = gr.Where(r => r.route == 1).Count();
                    int? count_2 = gr.Where(r => r.route == 2).Count();
                    int? count_3 = gr.Where(r => r.route == 3).Count();
                    int? count_4 = gr.Where(r => r.route == 4).Count();
                    RouteWagonTracking rwt_limit_1 = gr.Where(r => r.route == 1).FirstOrDefault();
                    RouteWagonTracking rwt_limit_2 = gr.Where(r => r.route == 2).FirstOrDefault();
                    RouteWagonTracking rwt_limit_3 = gr.Where(r => r.route == 3).FirstOrDefault();
                    RouteWagonTracking rwt_limit_4 = gr.Where(r => r.route == 4).FirstOrDefault();

                    decimal? time_limit_1 = rwt_limit_1 != null ? rwt_limit_1.time_limit : null;
                    decimal? time_limit_2 = rwt_limit_2 != null ? rwt_limit_2.time_limit : null;
                    decimal? time_limit_3 = rwt_limit_3 != null ? rwt_limit_3.time_limit : null;
                    decimal? time_limit_4 = rwt_limit_4 != null ? rwt_limit_4.time_limit : null;

                    List<RouteWagonTracking> list_rwt_1 = gr.Where(r => r.route == 1).ToList();
                    List<RouteWagonTracking> list_rwt_2 = gr.Where(r => r.route == 2).ToList();
                    List<RouteWagonTracking> list_rwt_3 = gr.Where(r => r.route == 3).ToList();
                    List<RouteWagonTracking> list_rwt_4 = gr.Where(r => r.route == 4).ToList();


                    decimal? time_avg_1 = list_rwt_1 != null && list_rwt_1.Count() > 0 ? (decimal?)list_rwt_1.Average(r => r.dt_difference) : null;
                    decimal? time_avg_2 = list_rwt_2 != null && list_rwt_2.Count() > 0 ? (decimal?)list_rwt_2.Average(r => r.dt_difference) : null;
                    decimal? time_avg_3 = list_rwt_3 != null && list_rwt_3.Count() > 0 ? (decimal?)list_rwt_3.Average(r => r.dt_difference) : null;
                    decimal? time_avg_4 = list_rwt_4 != null && list_rwt_4.Count() > 0 ? (decimal?)list_rwt_4.Average(r => r.dt_difference) : null;

                    CurentWagonTracking new_current = new CurentWagonTracking()
                    {
                        name_station = gr.Key,
                        count_1 = count_1 != 0 ? count_1 : null,
                        count_2 = count_2 != 0 ? count_2 : null,
                        count_3 = count_3 != 0 ? count_3 : null,
                        count_4 = count_4 != 0 ? count_4 : null,
                        count_norma_1 = count_norma_1 != 0 ? count_norma_1 : null,
                        count_norma_2 = count_norma_2 != 0 ? count_norma_2 : null,
                        count_norma_3 = count_norma_3 != 0 ? count_norma_3 : null,
                        count_norma_4 = count_norma_4 != 0 ? count_norma_4 : null,
                        count_surplus_1 = count_surplus_1 != 0 ? count_surplus_1 : null,
                        count_surplus_2 = count_surplus_2 != 0 ? count_surplus_2 : null,
                        count_surplus_3 = count_surplus_3 != 0 ? count_surplus_3 : null,
                        count_surplus_4 = count_surplus_4 != 0 ? count_surplus_4 : null,
                        time_limit_1 = time_limit_1 != 0 ? time_limit_1 : null,
                        time_limit_2 = time_limit_2 != 0 ? time_limit_2 : null,
                        time_limit_3 = time_limit_3 != 0 ? time_limit_3 : null,
                        time_limit_4 = time_limit_4 != 0 ? time_limit_4 : null,
                        time_avg_1 = time_avg_1 != null ? (decimal?)Math.Round((decimal)time_avg_1, 2) : null,
                        time_avg_2 = time_avg_2 != null ? (decimal?)Math.Round((decimal)time_avg_2, 2) : null,
                        time_avg_3 = time_avg_3 != null ? (decimal?)Math.Round((decimal)time_avg_3, 2) : null,
                        time_avg_4 = time_avg_4 != null ? (decimal?)Math.Round((decimal)time_avg_4, 2) : null
                    };
                    list_result.Add(new_current);
                }

                return list_result;

            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetLastWagonTrackingOfReports(id_report={0},start ={1},stop ={2})", id_report, start, stop), eventID);
                return null;
            }
        }

        #endregion

        #region OperationWagonsTracking
        /// <summary>
        /// Вернуть список операций по указаному вагону в интервале указанных id
        /// </summary>
        /// <param name="nvagon"></param>
        /// <param name="id_start"></param>
        /// <param name="id_stop"></param>
        /// <returns></returns>
        public List<OperationWagonsTracking> GetOperationWagonsTrackingOfNumCar(int nvagon, int? id_start, int? id_stop)
        {
            try
            {
                SqlParameter i_nvagon = new SqlParameter("@nvagon", nvagon);
                SqlParameter i_id_start = new SqlParameter();
                i_id_start.ParameterName = "@id_start";
                i_id_start.SqlDbType = SqlDbType.Int;
                //i_id_start.SqlValue = id_start;
                if (id_start != null)
                {
                    i_id_start.SqlValue = id_start;
                }
                else
                {
                    i_id_start.SqlValue = DBNull.Value;
                }

                SqlParameter i_id_stop = new SqlParameter();
                i_id_stop.ParameterName = "@id_stop";
                i_id_stop.SqlDbType = SqlDbType.Int;
                //i_id_stop.SqlValue = id_stop;
                if (i_id_stop != null)
                {
                    i_id_stop.SqlValue = id_stop;
                }
                else
                {
                    i_id_stop.SqlValue = DBNull.Value;
                }
                List<OperationWagonsTracking> list = this.Database.SqlQuery<OperationWagonsTracking>("[MT].[GetOperationWagonsTrackingOfNumCarAndID] @nvagon, @id_start, @id_stop", i_nvagon, i_id_start, i_id_stop).ToList();
                return list;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetOperationWagonsTrackingOfNumCar(nvagon={0},id_start ={1},id_stop ={2})", nvagon, id_start, id_stop), eventID);
                return null;
            }

        }
        #endregion

        //#region RouteDBWagonsTracking
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="id_report"></param>
        ///// <param name="start"></param>
        ///// <param name="stop"></param>
        ///// <returns></returns>
        //public List<RouteDBWagonsTracking> GetRouteWagonsTrackingOfReports(int id_report, DateTime start, DateTime stop)
        //{
        //    try
        //    {
        //        SqlParameter i_id_report = new SqlParameter("@idreport", id_report);
        //        SqlParameter dt_start = new SqlParameter("@start", start);
        //        SqlParameter dt_stop = new SqlParameter("@stop", stop);

        //        List<RouteDBWagonsTracking> list = this.Database.SqlQuery<RouteDBWagonsTracking>("EXEC [MT].[GetRouteDBWagonsTrackingOfCarsReports] @idreport, @start, @stop", i_id_report, dt_start, dt_stop).ToList();
        //        return list;

        //    }
        //    catch (Exception e)
        //    {
        //        e.WriteErrorMethod(String.Format("GetRouteWagonsTrackingOfReports(id_report={0})", id_report), eventID);
        //        return null;
        //    }
        //}

        //public List<RouteDBWagonsTracking> GetLastRouteWagonsTrackingOfReports(int id_report, DateTime start, DateTime stop)
        //{
        //    try
        //    {

        //        List<RouteDBWagonsTracking> list_last = new List<RouteDBWagonsTracking>();
        //        List<IGrouping<int, RouteDBWagonsTracking>> gr = GetRouteWagonsTrackingOfReports(id_report, start, stop).GroupBy(r=>r.nvagon).ToList();
        //        foreach (IGrouping<int, RouteDBWagonsTracking> rw in gr) { 
        //            RouteDBWagonsTracking last = rw.OrderByDescending(r=>r.cycle).FirstOrDefault();
        //            if (last != null)
        //            {
        //                list_last.Add(last);
        //            }
        //        }
        //        //List<IGrouping<int?, RouteDBWagonsTracking>> gr4 = list_last.Where(r => r.id_4 != null).GroupBy(r => r.station_from_4).ToList();
        //        //List<IGrouping<int?, RouteDBWagonsTracking>> gr3 = list_last.Where(r => r.id_3 != null & r.id_4 == null).GroupBy(r => r.station_disl_3).ToList();
        //        //List<IGrouping<int?, RouteDBWagonsTracking>> gr2 = list_last.Where(r => r.id_2 != null & r.id_3 == null & r.id_4 == null).GroupBy(r => r.station_end_2).ToList();
        //        //List<IGrouping<int?, RouteDBWagonsTracking>> gr1 = list_last.Where(r => r.id_1 != null & r.id_2 == null & r.id_3 == null & r.id_4 == null).GroupBy(r => r.station_disl_1).ToList();

        //        return list_last;
        //    }
        //    catch (Exception e)
        //    {
        //        e.WriteErrorMethod(String.Format("GetLastRouteWagonsTrackingOfReports(id_report={0})", id_report), eventID);
        //        return null;
        //    }
        //}
        //#endregion
        #endregion

        #region BufferArrivalSostav
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
                return GetBufferArrivalSostav().Where(c => c.id == id).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetBufferArrivalSostav(id={0})", id), eventID);
                return null;
            }
        }

        public int SaveBufferArrivalSostav(BufferArrivalSostav BufferArrivalSostav)
        {
            BufferArrivalSostav dbEntry;
            try
            {
                if (BufferArrivalSostav.id == 0)
                {
                    dbEntry = new BufferArrivalSostav()
                    {
                        id = 0,
                        datetime = BufferArrivalSostav.datetime,
                        id_sostav = BufferArrivalSostav.id_sostav,
                        id_arrival = BufferArrivalSostav.id_arrival,
                        count_wagons = BufferArrivalSostav.count_wagons,
                        count_set_wagons = BufferArrivalSostav.count_set_wagons,
                        close = BufferArrivalSostav.close,
                        close_user = BufferArrivalSostav.close_user,
                        close_comment = BufferArrivalSostav.close_comment,
                        list_wagons = BufferArrivalSostav.list_wagons,
                        list_no_set_wagons = BufferArrivalSostav.list_no_set_wagons,
                        message = BufferArrivalSostav.message,
                    };
                    context.BufferArrivalSostav.Add(dbEntry);
                }
                else
                {
                    dbEntry = context.BufferArrivalSostav.Find(BufferArrivalSostav.id);
                    if (dbEntry != null)
                    {
                        dbEntry.datetime = BufferArrivalSostav.datetime;
                        dbEntry.id_sostav = BufferArrivalSostav.id_sostav;
                        dbEntry.id_arrival = BufferArrivalSostav.id_arrival;
                        dbEntry.count_wagons = BufferArrivalSostav.count_wagons;
                        dbEntry.count_set_wagons = BufferArrivalSostav.count_set_wagons;
                        dbEntry.close = BufferArrivalSostav.close;
                        dbEntry.close_user = BufferArrivalSostav.close_user;
                        dbEntry.close_comment = BufferArrivalSostav.close_comment;
                        dbEntry.list_wagons = BufferArrivalSostav.list_wagons;
                        dbEntry.list_no_set_wagons = BufferArrivalSostav.list_no_set_wagons;
                        dbEntry.message = BufferArrivalSostav.message;
                    }
                }
                context.SaveChanges();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveBufferArrivalSostav(BufferArrivalSostav={0})", BufferArrivalSostav.GetFieldsAndValue()), eventID);
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
        /// Показать все строки по одному коду прибытия
        /// </summary>
        /// <param name="id_arrival"></param>
        /// <returns></returns>
        public IQueryable<BufferArrivalSostav> GetBufferArrivalSostavOfIDArrival(int id_arrival)
        {
            try
            {
                return GetBufferArrivalSostav().Where(c => c.id_arrival == id_arrival);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetBufferArrivalSostavOfIDArrival(id_arrival={0})", id_arrival), eventID);
                return null;
            }
        }
        /// <summary>
        /// Показать не закрытые составы
        /// </summary>
        /// <returns></returns>
        public IQueryable<BufferArrivalSostav> GetBufferArrivalSostavOfNotClose()
        {
            try
            {
                return GetBufferArrivalSostav().Where(c => c.close == null);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetBufferArrivalSostavOfNotClose()"), eventID);
                return null;
            }
        }
        #endregion

    }
}
