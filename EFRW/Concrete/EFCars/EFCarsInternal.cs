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
    /// Класс внутренего перемещения вагонов
    /// </summary>
    public class EFCarsInternal : IRepository<CarsInternal>
    {
        private eventID eventID = eventID.EFRW_EFCarsInternal;

        private EFDbContext db;

        public EFCarsInternal(EFDbContext db)
        {

            this.db = db;
        }

        public EFCarsInternal()
        {

            this.db = new EFDbContext();
        }

        public Database Database
        {
            get { return this.db.Database; }
        }

        public IEnumerable<CarsInternal> Get()
        {
            try
            {
                return db.Select<CarsInternal>();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Get()"), eventID);
                return null;
            }
        }

        public CarsInternal Get(int id)
        {
            try
            {
                return db.Select<CarsInternal>(id);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Get(id={0})", id), eventID);
                return null;
            }
        }

        public void Add(CarsInternal item)
        {
            try
            {
                item.user_create = item.user_create != null ? item.user_create : System.Environment.UserDomainName + @"\" + System.Environment.UserName;
                item.dt_create = item.dt_create != DateTime.Parse("01.01.0001") ? item.dt_create : DateTime.Now;
                db.Insert<CarsInternal>(item);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Add(item={0})", item), eventID);
            }
        }

        public void Update(CarsInternal item)
        {
            try
            {
                db.Update<CarsInternal>(item);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Update(item={0})", item), eventID);
            }
        }

        public void AddOrUpdate(CarsInternal item)
        {
            try
            {
                CarsInternal dbEntry = db.CarsInternal.Find(item.id);
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
                CarsInternal item = db.Delete<CarsInternal>(id);
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

        public CarsInternal Refresh(CarsInternal item)
        {
            try
            {
                db.Entry(item).State = EntityState.Detached;
                return db.Select<CarsInternal>(item.id);
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
