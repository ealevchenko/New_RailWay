
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
    public class EFMarriageWork : IRepository<MarriageWork>
    {

        private EFDbContext db;

        public EFMarriageWork(EFDbContext db)
        {

            this.db = db;
        }

        public EFMarriageWork()
        {

            this.db = new EFDbContext();
        }

        public Database Database
        {
            get { return this.db.Database; }
        }

        public IEnumerable<MarriageWork> Get()
        {
            try
            {
                return db.Select<MarriageWork>();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public MarriageWork Get(int id)
        {
            try
            {
                return db.Select<MarriageWork>(id);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public void Add(MarriageWork item)
        {
            try
            {
                db.Insert<MarriageWork>(item);
            }
            catch (Exception e)
            {

            }
        }

        public void Update(MarriageWork item)
        {
            try
            {
                db.Update<MarriageWork>(item);
            }
            catch (Exception e)
            {

            }
        }

        public void AddOrUpdate(MarriageWork item)
        {
            try
            {
                MarriageWork dbEntry = db.MarriageWork.Find(item.id);
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
                MarriageWork item = db.Delete<MarriageWork>(id);
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

        public MarriageWork Refresh(MarriageWork item)
        {
            try
            {
                db.Entry(item).State = EntityState.Detached;
                return db.Select<MarriageWork>(item.id);
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
