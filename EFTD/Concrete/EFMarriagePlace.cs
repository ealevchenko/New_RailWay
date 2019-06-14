
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
    public class EFMarriagePlace : IRepository<MarriagePlace>
    {

        private EFDbContext db;

        public EFMarriagePlace(EFDbContext db)
        {

            this.db = db;
        }

        public EFMarriagePlace()
        {

            this.db = new EFDbContext();
        }

        public Database Database
        {
            get { return this.db.Database; }
        }

        public IEnumerable<MarriagePlace> Get()
        {
            try
            {
                return db.Select<MarriagePlace>();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public MarriagePlace Get(int id)
        {
            try
            {
                return db.Select<MarriagePlace>(id);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public void Add(MarriagePlace item)
        {
            try
            {
                db.Insert<MarriagePlace>(item);
            }
            catch (Exception e)
            {

            }
        }

        public void Update(MarriagePlace item)
        {
            try
            {
                db.Update<MarriagePlace>(item);
            }
            catch (Exception e)
            {

            }
        }

        public void AddOrUpdate(MarriagePlace item)
        {
            try
            {
                MarriagePlace dbEntry = db.MarriagePlace.Find(item.id);
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
                MarriagePlace item = db.Delete<MarriagePlace>(id);
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

        public MarriagePlace Refresh(MarriagePlace item)
        {
            try
            {
                db.Entry(item).State = EntityState.Detached;
                return db.Select<MarriagePlace>(item.id);
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
