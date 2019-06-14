
using EFTD.Abstract;
using EFTD.Concrete;
using EFTD.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFOC.Concrete
{
    public class EFMarriageCause : IRepository<MarriageCause>
    {

        private EFDbContext db;

        public EFMarriageCause(EFDbContext db)
        {

            this.db = db;
        }

        public EFMarriageCause()
        {

            this.db = new EFDbContext();
        }

        public Database Database
        {
            get { return this.db.Database; }
        }

        public IEnumerable<MarriageCause> Get()
        {
            try
            {
                return db.Select<MarriageCause>();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public MarriageCause Get(int id)
        {
            try
            {
                return db.Select<MarriageCause>(id);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public void Add(MarriageCause item)
        {
            try
            {
                db.Insert<MarriageCause>(item);
            }
            catch (Exception e)
            {

            }
        }

        public void Update(MarriageCause item)
        {
            try
            {
                db.Update<MarriageCause>(item);
            }
            catch (Exception e)
            {

            }
        }

        public void AddOrUpdate(MarriageCause item)
        {
            try
            {
                MarriageCause dbEntry = db.MarriageCause.Find(item.id);
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

            }

        }

        public void Delete(int id)
        {
            try
            {
                MarriageCause item = db.Delete<MarriageCause>(id);
            }
            catch (Exception e)
            {

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
                return -1;
            }
        }

        public MarriageCause Refresh(MarriageCause item)
        {
            try
            {
                db.Entry(item).State = EntityState.Detached;
                return db.Select<MarriageCause>(item.id);
            }
            catch (Exception e)
            {
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
