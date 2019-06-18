
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
    public class EFMarriageDistrictObject : IRepository<MarriageDistrictObject>
    {

        private EFDbContext db;

        public EFMarriageDistrictObject(EFDbContext db)
        {

            this.db = db;
        }

        public EFMarriageDistrictObject()
        {

            this.db = new EFDbContext();
        }

        public Database Database
        {
            get { return this.db.Database; }
        }

        public IEnumerable<MarriageDistrictObject> Get()
        {
            try
            {
                return db.Select<MarriageDistrictObject>();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public MarriageDistrictObject Get(int id)
        {
            try
            {
                return db.Select<MarriageDistrictObject>(id);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public void Add(MarriageDistrictObject item)
        {
            try
            {
                db.Insert<MarriageDistrictObject>(item);
            }
            catch (Exception e)
            {

            }
        }

        public void Update(MarriageDistrictObject item)
        {
            try
            {
                db.Update<MarriageDistrictObject>(item);
            }
            catch (Exception e)
            {

            }
        }

        public void AddOrUpdate(MarriageDistrictObject item)
        {
            try
            {
                MarriageDistrictObject dbEntry = db.MarriageDistrictObject.Find(item.id);
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
                MarriageDistrictObject item = db.Delete<MarriageDistrictObject>(id);
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

        public MarriageDistrictObject Refresh(MarriageDistrictObject item)
        {
            try
            {
                db.Entry(item).State = EntityState.Detached;
                return db.Select<MarriageDistrictObject>(item.id);
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
