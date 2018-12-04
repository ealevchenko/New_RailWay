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
    /// Класс внутрение станции АМКР
    /// </summary>
    public class EFDirectoryInternalStations : IRepository<Directory_InternalStations>
    {
        private eventID eventID = eventID.EFRW_EFDirectoryInternalStations;

        private EFDbContext db;

        public EFDirectoryInternalStations(EFDbContext db)
        {

            this.db = db;
        }

        public EFDirectoryInternalStations()
        {

            this.db = new EFDbContext();
        }

        public Database Database
        {
            get { return this.db.Database; }
        }

        public IEnumerable<Directory_InternalStations> Get()
        {
            try
            {
                return db.Select<Directory_InternalStations>();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Get()"), eventID);
                return null;
            }
        }

        public Directory_InternalStations Get(int id)
        {
            try
            {
                return db.Select<Directory_InternalStations>(id);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Get(id={0})", id), eventID);
                return null;
            }
        }

        public void Add(Directory_InternalStations item)
        {
            try
            {
                item.user_create = item.user_create ?? System.Environment.UserDomainName + @"\" + System.Environment.UserName;
                item.dt_create = item.dt_create != DateTime.Parse("01.01.0001") ? item.dt_create : DateTime.Now;
                db.Insert<Directory_InternalStations>(item);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Add(item={0})", item), eventID);
            }
        }

        public void Update(Directory_InternalStations item)
        {
            try
            {
                item.user_edit = item.user_edit ?? System.Environment.UserDomainName + @"\" + System.Environment.UserName;
                item.dt_edit = item.dt_edit != null ? item.dt_edit : DateTime.Now;
                db.Update<Directory_InternalStations>(item);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Update(item={0})", item), eventID);
            }
        }

        public void AddOrUpdate(Directory_InternalStations item)
        {
            try
            {
                Directory_InternalStations dbEntry = db.Directory_InternalStations.Find(item.id);
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
                Directory_InternalStations item = db.Delete<Directory_InternalStations>(id);
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

        public Directory_InternalStations Refresh(Directory_InternalStations item)
        {
            try
            {
                db.Entry(item).State = EntityState.Detached;
                return db.Select<Directory_InternalStations>(item.id);
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
