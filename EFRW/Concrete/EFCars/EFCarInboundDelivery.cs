using EFRW.Abstract;
using EFRW.Entities;
using MessageLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFRW.Concrete.EFCars
{
    /// <summary>
    /// Класс входящих поставок
    /// </summary>
    public class EFCarInboundDelivery : IRepository<CarInboundDelivery>
    {
        private eventID eventID = eventID.EFRW_EFCarInboundDelivery;

        private EFDbContext db;

        public EFCarInboundDelivery(EFDbContext db)
        {

            this.db = db;
        }

        public EFCarInboundDelivery()
        {

            this.db = new EFDbContext();
        }

        public Database Database
        {
            get { return this.db.Database; }
        }

        public IEnumerable<CarInboundDelivery> Get()
        {
            try
            {
                return db.Select<CarInboundDelivery>();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Get()"), eventID);
                return null;
            }
        }

        public CarInboundDelivery Get(int id)
        {
            try
            {
                return db.Select<CarInboundDelivery>(id);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Get(id={0})", id), eventID);
                return null;
            }
        }

        public void Add(CarInboundDelivery item)
        {
            try
            {
                db.Insert<CarInboundDelivery>(item);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Add(item={0})", item), eventID);
            }
        }

        public void Update(CarInboundDelivery item)
        {
            try
            {
                db.Update<CarInboundDelivery>(item);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Update(item={0})", item), eventID);
            }
        }

        public void AddOrUpdate(CarInboundDelivery item)
        {
            try
            {
                CarInboundDelivery dbEntry = db.CarInboundDelivery.Find(item.id);
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
                CarInboundDelivery item = db.Delete<CarInboundDelivery>(id);
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

        public CarInboundDelivery Refresh(CarInboundDelivery item)
        {
            try
            {
                db.Entry(item).State = EntityState.Detached;
                return db.Select<CarInboundDelivery>(item.id);
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
