
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
    public class EFMarriageDistrict : IRepository<MarriageDistrict>
    {

        private EFDbContext db;

        public EFMarriageDistrict(EFDbContext db)
        {

            this.db = db;
        }

        public EFMarriageDistrict()
        {

            this.db = new EFDbContext();
        }

        public Database Database
        {
            get { return this.db.Database; }
        }

        public IEnumerable<MarriageDistrict> Get()
        {
            try
            {
                return db.Select<MarriageDistrict>();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public MarriageDistrict Get(int id)
        {
            try
            {
                return db.Select<MarriageDistrict>(id);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public void Add(MarriageDistrict item)
        {
            try
            {
                db.Insert<MarriageDistrict>(item);
            }
            catch (Exception e)
            {

            }
        }

        public void Update(MarriageDistrict item)
        {
            try
            {
                db.Update<MarriageDistrict>(item);
            }
            catch (Exception e)
            {

            }
        }

        public void AddOrUpdate(MarriageDistrict item)
        {
            try
            {
                MarriageDistrict dbEntry = db.MarriageDistrict.Find(item.id);
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
                MarriageDistrict item = db.Delete<MarriageDistrict>(id);
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

        public MarriageDistrict Refresh(MarriageDistrict item)
        {
            try
            {
                db.Entry(item).State = EntityState.Detached;
                return db.Select<MarriageDistrict>(item.id);
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
