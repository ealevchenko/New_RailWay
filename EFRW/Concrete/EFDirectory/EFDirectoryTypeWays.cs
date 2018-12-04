using EFRW.Abstract;
using EFRW.Entities;
using MessageLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFRW.Concrete.EFDirectory
{
    /// <summary>
    /// Класс типы путей
    /// </summary>
    public class EFDirectoryTypeWays : IRepository<Directory_TypeWays>
    {
        private eventID eventID = eventID.EFRW_EFDirectoryTypeWays;

        private EFDbContext db;

        public EFDirectoryTypeWays(EFDbContext db)
        {

            this.db = db;
        }

        public EFDirectoryTypeWays()
        {

            this.db = new EFDbContext();
        }

        public Database Database
        {
            get { return this.db.Database; }
        }

        public IEnumerable<Directory_TypeWays> Get()
        {
            try
            {
                return db.Select<Directory_TypeWays>();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Get()"), eventID);
                return null;
            }
        }

        public Directory_TypeWays Get(int id)
        {
            try
            {
                return db.Select<Directory_TypeWays>(id);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Get(id={0})", id), eventID);
                return null;
            }
        }

        public void Add(Directory_TypeWays item)
        {
            try
            {
                db.Insert<Directory_TypeWays>(item);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Add(item={0})", item), eventID);
            }
        }

        public void Update(Directory_TypeWays item)
        {
            try
            {
                db.Update<Directory_TypeWays>(item);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Update(item={0})", item), eventID);
            }
        }

        public void AddOrUpdate(Directory_TypeWays item)
        {
            try
            {
                Directory_TypeWays dbEntry = db.Directory_TypeWays.Find(item.id);
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
                Directory_TypeWays item = db.Delete<Directory_TypeWays>(id);
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

        public Directory_TypeWays Refresh(Directory_TypeWays item)
        {
            try
            {
                db.Entry(item).State = EntityState.Detached;
                return db.Select<Directory_TypeWays>(item.id);
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
