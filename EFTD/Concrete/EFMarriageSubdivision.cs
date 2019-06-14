
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
    public class EFMarriageSubdivision : IRepository<MarriageSubdivision>
    {

        private EFDbContext db;

        public EFMarriageSubdivision(EFDbContext db)
        {

            this.db = db;
        }

        public EFMarriageSubdivision()
        {

            this.db = new EFDbContext();
        }

        public Database Database
        {
            get { return this.db.Database; }
        }

        public IEnumerable<MarriageSubdivision> Get()
        {
            try
            {
                return db.Select<MarriageSubdivision>();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public MarriageSubdivision Get(int id)
        {
            try
            {
                return db.Select<MarriageSubdivision>(id);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public void Add(MarriageSubdivision item)
        {
            try
            {
                db.Insert<MarriageSubdivision>(item);
            }
            catch (Exception e)
            {

            }
        }

        public void Update(MarriageSubdivision item)
        {
            try
            {
                db.Update<MarriageSubdivision>(item);
            }
            catch (Exception e)
            {

            }
        }

        public void AddOrUpdate(MarriageSubdivision item)
        {
            try
            {
                MarriageSubdivision dbEntry = db.MarriageSubdivision.Find(item.id);
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
                MarriageSubdivision item = db.Delete<MarriageSubdivision>(id);
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

        public MarriageSubdivision Refresh(MarriageSubdivision item)
        {
            try
            {
                db.Entry(item).State = EntityState.Detached;
                return db.Select<MarriageSubdivision>(item.id);
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
