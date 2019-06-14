
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
    public class EFMarriageClassification : IRepository<MarriageClassification>
    {

        private EFDbContext db;

        public EFMarriageClassification(EFDbContext db)
        {

            this.db = db;
        }

        public EFMarriageClassification()
        {

            this.db = new EFDbContext();
        }

        public Database Database
        {
            get { return this.db.Database; }
        }

        public IEnumerable<MarriageClassification> Get()
        {
            try
            {
                return db.Select<MarriageClassification>();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public MarriageClassification Get(int id)
        {
            try
            {
                return db.Select<MarriageClassification>(id);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public void Add(MarriageClassification item)
        {
            try
            {
                db.Insert<MarriageClassification>(item);
            }
            catch (Exception e)
            {

            }
        }

        public void Update(MarriageClassification item)
        {
            try
            {
                db.Update<MarriageClassification>(item);
            }
            catch (Exception e)
            {

            }
        }

        public void AddOrUpdate(MarriageClassification item)
        {
            try
            {
                MarriageClassification dbEntry = db.MarriageClassification.Find(item.id);
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
                MarriageClassification item = db.Delete<MarriageClassification>(id);
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

        public MarriageClassification Refresh(MarriageClassification item)
        {
            try
            {
                db.Entry(item).State = EntityState.Detached;
                return db.Select<MarriageClassification>(item.id);
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
