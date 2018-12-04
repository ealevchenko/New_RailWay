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
    /// Класс справочник грузов
    /// </summary>
    public class EFDirectoryCargo : IRepository<Directory_Cargo>
    {
        private eventID eventID = eventID.EFRW_EFDirectoryCargo;

        private EFDbContext db;

        public EFDirectoryCargo(EFDbContext db)
        {

            this.db = db;
        }

        public EFDirectoryCargo()
        {

            this.db = new EFDbContext();
        }

        public Database Database
        {
            get { return this.db.Database; }
        }

        public IEnumerable<Directory_Cargo> Get()
        {
            try
            {
                return db.Select<Directory_Cargo>();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Get()"), eventID);
                return null;
            }
        }

        public Directory_Cargo Get(int id)
        {
            try
            {
                return db.Select<Directory_Cargo>(id);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Get(id={0})", id), eventID);
                return null;
            }
        }

        public void Add(Directory_Cargo item)
        {
            try
            {
                item.user_create = item.user_create ?? System.Environment.UserDomainName + @"\" + System.Environment.UserName;
                item.dt_create = item.dt_create != DateTime.Parse("01.01.0001") ? item.dt_create : DateTime.Now;
                db.Insert<Directory_Cargo>(item);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Add(item={0})", item), eventID);
            }
        }

        public void Update(Directory_Cargo item)
        {
            try
            {
                item.user_edit = item.user_edit ?? System.Environment.UserDomainName + @"\" + System.Environment.UserName;
                item.dt_edit = item.dt_edit != null ? item.dt_edit : DateTime.Now;
                db.Update<Directory_Cargo>(item);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Update(item={0})", item), eventID);
            }
        }

        public void AddOrUpdate(Directory_Cargo item)
        {
            try
            {
                Directory_Cargo dbEntry = db.Directory_Cargo.Find(item.id);
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
                Directory_Cargo item = db.Delete<Directory_Cargo>(id);
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

        public Directory_Cargo Refresh(Directory_Cargo item)
        {
            try
            {
                db.Entry(item).State = EntityState.Detached;
                return db.Select<Directory_Cargo>(item.id);
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
