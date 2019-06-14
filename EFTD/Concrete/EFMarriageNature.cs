
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
    public class EFMarriageNature : IRepository<MarriageNature>
    {

        private EFDbContext db;

        public EFMarriageNature(EFDbContext db)
        {

            this.db = db;
        }

        public EFMarriageNature()
        {

            this.db = new EFDbContext();
        }

        public Database Database
        {
            get { return this.db.Database; }
        }

        public IEnumerable<MarriageNature> Get()
        {
            try
            {
                return db.Select<MarriageNature>();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public MarriageNature Get(int id)
        {
            try
            {
                return db.Select<MarriageNature>(id);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public void Add(MarriageNature item)
        {
            try
            {
                db.Insert<MarriageNature>(item);
            }
            catch (Exception e)
            {

            }
        }

        public void Update(MarriageNature item)
        {
            try
            {
                db.Update<MarriageNature>(item);
            }
            catch (Exception e)
            {

            }
        }

        public void AddOrUpdate(MarriageNature item)
        {
            try
            {
                MarriageNature dbEntry = db.MarriageNature.Find(item.id);
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
                MarriageNature item = db.Delete<MarriageNature>(id);
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

        public MarriageNature Refresh(MarriageNature item)
        {
            try
            {
                db.Entry(item).State = EntityState.Detached;
                return db.Select<MarriageNature>(item.id);
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
