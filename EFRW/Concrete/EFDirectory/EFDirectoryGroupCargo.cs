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
    /// Класс группы грузов грузов
    /// </summary>
    public class EFDirectoryGroupCargo : IRepository<Directory_GroupCargo>
    {
        private eventID eventID = eventID.EFRW_EFDirectoryGroupCargo;

        private EFDbContext db;

        public EFDirectoryGroupCargo(EFDbContext db)
        {

            this.db = db;
        }

        public EFDirectoryGroupCargo()
        {

            this.db = new EFDbContext();
        }

        public Database Database
        {
            get { return this.db.Database; }
        }

        public IEnumerable<Directory_GroupCargo> Get()
        {
            try
            {
                return db.Select<Directory_GroupCargo>();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Get()"), eventID);
                return null;
            }
        }

        public Directory_GroupCargo Get(int id)
        {
            try
            {
                return db.Select<Directory_GroupCargo>(id);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Get(id={0})", id), eventID);
                return null;
            }
        }

        public void Add(Directory_GroupCargo item)
        {
            try
            {
                db.Insert<Directory_GroupCargo>(item);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Add(item={0})", item), eventID);
            }
        }

        public void Update(Directory_GroupCargo item)
        {
            try
            {
                db.Update<Directory_GroupCargo>(item);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Update(item={0})", item), eventID);
            }
        }

        public void AddOrUpdate(Directory_GroupCargo item)
        {
            try
            {
                Directory_GroupCargo dbEntry = db.Directory_GroupCargo.Find(item.id);
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
                Directory_GroupCargo item = db.Delete<Directory_GroupCargo>(id);
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

        public Directory_GroupCargo Refresh(Directory_GroupCargo item)
        {
            try
            {
                db.Entry(item).State = EntityState.Detached;
                return db.Select<Directory_GroupCargo>(item.id);
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
