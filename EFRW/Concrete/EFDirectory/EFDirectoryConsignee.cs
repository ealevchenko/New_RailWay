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
    /// Класс грузополучатели
    /// </summary>
    public class EFDirectoryConsignee : IRepository<Directory_Consignee>
    {
        private eventID eventID = eventID.EFRW_EFDirectoryConsignee;

        private EFDbContext db;

        public EFDirectoryConsignee(EFDbContext db)
        {

            this.db = db;
        }

        public EFDirectoryConsignee()
        {

            this.db = new EFDbContext();
        }

        public Database Database
        {
            get { return this.db.Database; }
        }

        public IEnumerable<Directory_Consignee> Get()
        {
            try
            {
                return db.Select<Directory_Consignee>();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Get()"), eventID);
                return null;
            }
        }

        public Directory_Consignee Get(int id)
        {
            try
            {
                return db.Select<Directory_Consignee>(id);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Get(id={0})", id), eventID);
                return null;
            }
        }

        public void Add(Directory_Consignee item)
        {
            try
            {
                item.user_create = item.user_create ?? System.Environment.UserDomainName + @"\" + System.Environment.UserName;
                item.dt_create = item.dt_create != DateTime.Parse("01.01.0001") ? item.dt_create : DateTime.Now;
                db.Insert<Directory_Consignee>(item);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Add(item={0})", item), eventID);
            }
        }

        public void Update(Directory_Consignee item)
        {
            try
            {
                item.user_edit = item.user_edit ?? System.Environment.UserDomainName + @"\" + System.Environment.UserName;
                item.dt_edit = item.dt_edit != null ? item.dt_edit : DateTime.Now;
                db.Update<Directory_Consignee>(item);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Update(item={0})", item), eventID);
            }
        }

        public void AddOrUpdate(Directory_Consignee item)
        {
            try
            {
                Directory_Consignee dbEntry = db.Directory_Consignee.Find(item.id);
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
                Directory_Consignee item = db.Delete<Directory_Consignee>(id);
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

        public Directory_Consignee Refresh(Directory_Consignee item)
        {
            try
            {
                db.Entry(item).State = EntityState.Detached;
                return db.Select<Directory_Consignee>(item.id);
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
