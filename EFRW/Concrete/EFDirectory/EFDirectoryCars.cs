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
    /// Класс справочник вагонов
    /// </summary>
    public class EFDirectoryCars : IRepository<Directory_Cars>
    {
        private eventID eventID = eventID.EFRW_EFDirectoryCars;

        private EFDbContext db;

        public EFDirectoryCars(EFDbContext db)
        {

            this.db = db;
        }

        public EFDirectoryCars()
        {

            this.db = new EFDbContext();
        }

        public Database Database
        {
            get { return this.db.Database; }
        }

        public IEnumerable<Directory_Cars> Get()
        {
            try
            {
                return db.Select<Directory_Cars>();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Get()"), eventID);
                return null;
            }
        }

        public Directory_Cars Get(int id)
        {
            try
            {
                return db.Select<Directory_Cars>(id);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Get(id={0})", id), eventID);
                return null;
            }
        }

        public void Add(Directory_Cars item)
        {
            try
            {
                item.user_create = item.user_create ?? System.Environment.UserDomainName + @"\" + System.Environment.UserName;
                item.dt_create = item.dt_create != DateTime.Parse("01.01.0001") ? item.dt_create : DateTime.Now;
                db.Insert<Directory_Cars>(item);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Add(item={0})", item), eventID);
            }
        }

        public void Update(Directory_Cars item)
        {
            try
            {
                item.user_edit = item.user_edit ?? System.Environment.UserDomainName + @"\" + System.Environment.UserName;
                item.dt_edit = item.dt_edit != null ? item.dt_edit : DateTime.Now;
                db.Update<Directory_Cars>(item);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Update(item={0})", item), eventID);
            }
        }

        public void AddOrUpdate(Directory_Cars item)
        {
            try
            {
                Directory_Cars dbEntry = db.Directory_Cars.Find(item.num);
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
                Directory_Cars item = db.Delete<Directory_Cars>(id);
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

        public Directory_Cars Refresh(Directory_Cars item)
        {
            try
            {
                db.Entry(item).State = EntityState.Detached;
                return db.Select<Directory_Cars>(item.num);
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
