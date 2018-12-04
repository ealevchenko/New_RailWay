using EFRW.Abstract;
using EFRW.Entities;
using MessageLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFRW.Concrete.EFCars
{
    /// <summary>
    /// Класс операций над  вагоном
    /// </summary>
    public class EFCarOperations : IRepository<CarOperations>
    {
        private eventID eventID = eventID.EFRW_EFCarOperations;

        private EFDbContext db;

        public EFCarOperations(EFDbContext db)
        {

            this.db = db;
        }

        public EFCarOperations()
        {

            this.db = new EFDbContext();
        }

        public Database Database
        {
            get { return this.db.Database; }
        }

        public IEnumerable<CarOperations> Get()
        {
            try
            {
                return db.Select<CarOperations>();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Get()"), eventID);
                return null;
            }
        }

        public CarOperations Get(int id)
        {
            try
            {
                return db.Select<CarOperations>(id);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Get(id={0})", id), eventID);
                return null;
            }
        }

        public void Add(CarOperations item)
        {
            try
            {
                item.user_create = item.user_create != null ? item.user_create : System.Environment.UserDomainName + @"\" + System.Environment.UserName;
                item.dt_create = item.dt_create != DateTime.Parse("01.01.0001") ? item.dt_create : DateTime.Now;
                db.Insert<CarOperations>(item);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Add(item={0})", item), eventID);
            }
        }

        public void Update(CarOperations item)
        {
            try
            {
                item.user_edit = item.user_edit != null ? item.user_edit : System.Environment.UserDomainName + @"\" + System.Environment.UserName;
                item.dt_edit = item.dt_edit != null ? item.dt_edit : DateTime.Now;
                db.Update<CarOperations>(item);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Update(item={0})", item), eventID);
            }
        }

        public void AddOrUpdate(CarOperations item)
        {
            try
            {
                CarOperations dbEntry = db.CarOperations.Find(item.id);
                if (dbEntry == null)
                {
                    Add(item);
                }
                else
                {
                    Update(item);
                }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("AddOrUpdate(item={0})", item), eventID);
            }

        }

        public void Delete(int id)
        {
            try
            {
                CarOperations item = db.Delete<CarOperations>(id);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Delete(id={0})", id), eventID);
            }
        }

        public int Save()
        {
            try
            {
                return db.SaveChanges();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Save()"), eventID);
                return -1;
            }
        }

        public CarOperations Refresh(CarOperations item)
        {
            try
            {
                db.Entry(item).State = EntityState.Detached;
                return db.Select<CarOperations>(item.id);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Refresh(item={0})", item), eventID);
                return null;
            }
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
