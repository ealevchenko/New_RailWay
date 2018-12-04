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
    /// Класс внешних станций
    /// </summary>
    public class EFDirectoryExternalStations : IRepository<Directory_ExternalStations>
    {
        private eventID eventID = eventID.EFRW_EFDirectoryExternalStations;

        private EFDbContext db;

        public EFDirectoryExternalStations(EFDbContext db)
        {

            this.db = db;
        }

        public EFDirectoryExternalStations()
        {

            this.db = new EFDbContext();
        }

        public Database Database
        {
            get { return this.db.Database; }
        }

        public IEnumerable<Directory_ExternalStations> Get()
        {
            try
            {
                return db.Select<Directory_ExternalStations>();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Get()"), eventID);
                return null;
            }
        }

        public Directory_ExternalStations Get(int id)
        {
            try
            {
                return db.Select<Directory_ExternalStations>(id);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Get(id={0})", id), eventID);
                return null;
            }
        }

        public void Add(Directory_ExternalStations item)
        {
            try
            {

                db.Insert<Directory_ExternalStations>(item);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Add(item={0})", item), eventID);
            }
        }

        public void Update(Directory_ExternalStations item)
        {
            try
            {
                db.Update<Directory_ExternalStations>(item);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Update(item={0})", item), eventID);
            }
        }

        public void AddOrUpdate(Directory_ExternalStations item)
        {
            try
            {
                Directory_ExternalStations dbEntry = db.Directory_ExternalStations.Find(item.id);
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
                Directory_ExternalStations item = db.Delete<Directory_ExternalStations>(id);
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

        public Directory_ExternalStations Refresh(Directory_ExternalStations item)
        {
            try
            {
                db.Entry(item).State = EntityState.Detached;
                return db.Select<Directory_ExternalStations>(item.id);
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
