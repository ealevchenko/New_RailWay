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
    /// Класс исходящих поставок
    /// </summary>
    public class EFCarOutboundDelivery : IRepository<CarOutboundDelivery>
    {
        private eventID eventID = eventID.EFRW_EFCarOutboundDelivery;

        private EFDbContext db;

        public EFCarOutboundDelivery(EFDbContext db)
        {

            this.db = db;
        }

        public EFCarOutboundDelivery()
        {

            this.db = new EFDbContext();
        }

        public Database Database
        {
            get { return this.db.Database; }
        }

        public IEnumerable<CarOutboundDelivery> Get()
        {
            try
            {
                return db.Select<CarOutboundDelivery>();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Get()"), eventID);
                return null;
            }
        }

        public CarOutboundDelivery Get(int id)
        {
            try
            {
                return db.Select<CarOutboundDelivery>(id);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Get(id={0})", id), eventID);
                return null;
            }
        }

        public void Add(CarOutboundDelivery item)
        {
            try
            {
                db.Insert<CarOutboundDelivery>(item);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Add(item={0})", item), eventID);
            }
        }

        public void Update(CarOutboundDelivery item)
        {
            try
            {
                db.Update<CarOutboundDelivery>(item);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Update(item={0})", item), eventID);
            }
        }

        public void AddOrUpdate(CarOutboundDelivery item)
        {
            try
            {
                CarOutboundDelivery dbEntry = db.CarOutboundDelivery.Find(item.id);
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
                CarOutboundDelivery item = db.Delete<CarOutboundDelivery>(id);
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

        public CarOutboundDelivery Refresh(CarOutboundDelivery item)
        {
            try
            {
                db.Entry(item).State = EntityState.Detached;
                return db.Select<CarOutboundDelivery>(item.id);
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
