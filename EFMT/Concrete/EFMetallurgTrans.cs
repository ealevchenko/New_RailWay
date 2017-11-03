using EFMT.Abstract;
using MessageLog;
using MT.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libClass;
using System.Data.SqlClient;

namespace EFMT.Concrete
{
    public class EFMetallurgTrans : IMT
    {
        private eventID eventID = eventID.EFMetallurgTrans;

        protected EFDbContext context = new EFDbContext();

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
                    context.ApproachesSostav.Remove(dbEntry);
                    if (DeleteApproachesCarsOfSostav(dbEntry.ID) < 0) return null; // Удалить вагоны состава                    
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
    }
}
