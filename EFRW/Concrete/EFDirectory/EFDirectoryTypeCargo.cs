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
    /// Класс типов грузов
    /// </summary>
    public class EFDirectoryTypeCargo : IRepository<Directory_TypeCargo>
    {
        private eventID eventID = eventID.EFRW_EFDirectoryTypeCargo;

        private EFDbContext db;

        public EFDirectoryTypeCargo(EFDbContext db)
        {

            this.db = db;
        }

        public EFDirectoryTypeCargo()
        {

            this.db = new EFDbContext();
        }

        public Database Database
        {
            get { return this.db.Database; }
        }

        public IEnumerable<Directory_TypeCargo> Get()
        {
            try
            {
                return db.Select<Directory_TypeCargo>();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Get()"), eventID);
                return null;
            }
        }

        public Directory_TypeCargo Get(int id)
        {
            try
            {
                return db.Select<Directory_TypeCargo>(id);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Get(id={0})", id), eventID);
                return null;
            }
        }

        public void Add(Directory_TypeCargo item)
        {
            try
            {
                db.Insert<Directory_TypeCargo>(item);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Add(item={0})", item), eventID);
            }
        }

        public void Update(Directory_TypeCargo item)
        {
            try
            {
                db.Update<Directory_TypeCargo>(item);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Update(item={0})", item), eventID);
            }
        }

        public void AddOrUpdate(Directory_TypeCargo item)
        {
            try
            {
                Directory_TypeCargo dbEntry = db.Directory_TypeCargo.Find(item.id);
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
                Directory_TypeCargo item = db.Delete<Directory_TypeCargo>(id);
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

        public Directory_TypeCargo Refresh(Directory_TypeCargo item)
        {
            try
            {
                db.Entry(item).State = EntityState.Detached;
                return db.Select<Directory_TypeCargo>(item.id);
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
