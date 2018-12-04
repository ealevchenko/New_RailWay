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
    /// Класс стран
    /// </summary>
    public class EFDirectoryCountry : IRepository<Directory_Country>
    {
        private eventID eventID = eventID.EFRW_EFDirectoryCountry;

        private EFDbContext db;

        public EFDirectoryCountry(EFDbContext db)
        {

            this.db = db;
        }

        public EFDirectoryCountry()
        {

            this.db = new EFDbContext();
        }

        public Database Database
        {
            get { return this.db.Database; }
        }

        public IEnumerable<Directory_Country> Get()
        {
            try
            {
                return db.Select<Directory_Country>();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Get()"), eventID);
                return null;
            }
        }

        public Directory_Country Get(int id)
        {
            try
            {
                return db.Select<Directory_Country>(id);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Get(id={0})", id), eventID);
                return null;
            }
        }

        public void Add(Directory_Country item)
        {
            try
            {

                db.Insert<Directory_Country>(item);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Add(item={0})", item), eventID);
            }
        }

        public void Update(Directory_Country item)
        {
            try
            {
                db.Update<Directory_Country>(item);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Update(item={0})", item), eventID);
            }
        }

        public void AddOrUpdate(Directory_Country item)
        {
            try
            {
                Directory_Country dbEntry = db.Directory_Country.Find(item.id);
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
                Directory_Country item = db.Delete<Directory_Country>(id);
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

        public Directory_Country Refresh(Directory_Country item)
        {
            try
            {
                db.Entry(item).State = EntityState.Detached;
                return db.Select<Directory_Country>(item.id);
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
