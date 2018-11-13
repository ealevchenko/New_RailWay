using EFRW.Abstract;
using EFRW.Entities;
using MessageLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFRW.Concrete
{
    public static class EFRepository
    {
        public static IQueryable<TEntity> Select<TEntity>(this EFDbContext context) where TEntity : class
        {

            // Здесь мы можем указывать различные настройки контекста,
            // например выводить в отладчик сгенерированный SQL-код
            context.Database.Log =
                (s => System.Diagnostics.Debug.WriteLine(s));

            // Загрузка данных с помощью универсального метода Set
            return context.Set<TEntity>();
        }

        public static TEntity Select<TEntity>(this EFDbContext context, int id) where TEntity : class
        {

            // Здесь мы можем указывать различные настройки контекста,
            // например выводить в отладчик сгенерированный SQL-код
            context.Database.Log =
                (s => System.Diagnostics.Debug.WriteLine(s));

            // Загрузка данных с помощью универсального метода Set
            return context.Set<TEntity>().Find(id);
        }        

        public static void Insert<TEntity>(this EFDbContext context, TEntity entity) where TEntity : class
        {
            // Здесь мы можем указывать различные настройки контекста,
            // например выводить в отладчик сгенерированный SQL-код
            context.Database.Log =
                (s => System.Diagnostics.Debug.WriteLine(s));

            context.Entry(entity).State = EntityState.Added;
        }  
        /// <summary>
        /// Запись нескольких полей в БД
        /// </summary>
        public static void Inserts<TEntity>(this EFDbContext context, IEnumerable<TEntity> entities) where TEntity : class
        {

            //// Отключаем отслеживание и проверку изменений для оптимизации вставки множества полей
            //context.Configuration.AutoDetectChangesEnabled = false;
            //context.Configuration.ValidateOnSaveEnabled = false;

            context.Database.Log = (s => System.Diagnostics.Debug.WriteLine(s));

            foreach (TEntity entity in entities)
                context.Entry(entity).State = EntityState.Added;


            //context.Configuration.AutoDetectChangesEnabled = true;
            //context.Configuration.ValidateOnSaveEnabled = true;
        }

        public static void Update<TEntity>(this EFDbContext context, TEntity entity) where TEntity : class
        {
            // Здесь мы можем указывать различные настройки контекста,
            // например выводить в отладчик сгенерированный SQL-код
            context.Database.Log =
                (s => System.Diagnostics.Debug.WriteLine(s));

            context.Entry(entity).State = EntityState.Modified;
        }
        
        public static TEntity Delete<TEntity>(this EFDbContext context, int id) where TEntity : class
        {
            // Здесь мы можем указывать различные настройки контекста,
            // например выводить в отладчик сгенерированный SQL-код
            context.Database.Log =
                (s => System.Diagnostics.Debug.WriteLine(s));
            
            TEntity item = context.Set<TEntity>().Find(id);
            if (item != null)
                context.Entry<TEntity>(item).State = EntityState.Deleted;
            return item;
        }

    }

    #region СПРАВОЧНИКИ

    #region ВАГОНЫ
    // Класс справочник вагонов
    public class EFDirectoryCars : IRepository<Directory_Cars>
    {
        private eventID eventID = eventID.EFRW_EFDirectoryCars;

        private EFDbContext db;

        public EFDirectoryCars(EFDbContext db)
        {

            this.db = db;
        }

        public EFDirectoryCars()
        {

            this.db = new EFDbContext();
        }

        public IEnumerable<Directory_Cars> Get()
        {
            try
            {
                return db.Select<Directory_Cars>();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Get()"), eventID);
                return null;
            }
        }

        public Directory_Cars Get(int id)
        {
            try
            {
                return db.Select<Directory_Cars>(id);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Get(id={0})", id), eventID);
                return null;
            }
        }

        public void Add(Directory_Cars item)
        {
            try
            {
                item.user_create = item.user_create ?? System.Environment.UserDomainName + @"\" + System.Environment.UserName;
                item.dt_create = item.dt_create != DateTime.Parse("01.01.0001") ? item.dt_create : DateTime.Now;
                db.Insert<Directory_Cars>(item);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Add(item={0})", item), eventID);
            }
        }

        public void Update(Directory_Cars item)
        {
            try
            {
                item.user_edit = item.user_edit ?? System.Environment.UserDomainName + @"\" + System.Environment.UserName;
                item.dt_edit = item.dt_edit != null ? item.dt_edit : DateTime.Now;
                db.Update<Directory_Cars>(item);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Update(item={0})", item), eventID);
            }
        }

        public void AddOrUpdate(Directory_Cars item)
        {
            try
            {
                Directory_Cars dbEntry = db.Directory_Cars.Find(item.num);
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
                Directory_Cars item = db.Delete<Directory_Cars>(id);
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

        public Directory_Cars Refresh(Directory_Cars item)
        {
            try
            {
                db.Entry(item).State = EntityState.Detached;
                return db.Select<Directory_Cars>(item.num);
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
    // Класс справочник типов вагонов
    public class EFDirectoryTypeCars : IRepository<Directory_TypeCars>
    {
        private eventID eventID = eventID.EFRW_EFDirectoryTypeCars;

        private EFDbContext db;

        public EFDirectoryTypeCars(EFDbContext db)
        {

            this.db = db;
        }

        public EFDirectoryTypeCars()
        {

            this.db = new EFDbContext();
        }

        public IEnumerable<Directory_TypeCars> Get()
        {
            try
            {
                return db.Select<Directory_TypeCars>();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Get()"), eventID);
                return null;
            }
        }

        public Directory_TypeCars Get(int id)
        {
            try
            {
                return db.Select<Directory_TypeCars>(id);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Get(id={0})", id), eventID);
                return null;
            }
        }

        public void Add(Directory_TypeCars item)
        {
            try
            {
                db.Insert<Directory_TypeCars>(item);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Add(item={0})", item), eventID);
            }
        }

        public void Update(Directory_TypeCars item)
        {
            try
            {
                db.Update<Directory_TypeCars>(item);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Update(item={0})", item), eventID);
            }
        }

        public void AddOrUpdate(Directory_TypeCars item)
        {
            try
            {
                Directory_TypeCars dbEntry = db.Directory_TypeCars.Find(item.id);
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
                Directory_TypeCars item = db.Delete<Directory_TypeCars>(id);
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

        public Directory_TypeCars Refresh(Directory_TypeCars item)
        {
            try
            {
                db.Entry(item).State = EntityState.Detached;
                return db.Select<Directory_TypeCars>(item.id);
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
    // Класс справочник групп вагонов
    public class EFDirectoryGroupCars : IRepository<Directory_GroupCars>
    {
        private eventID eventID = eventID.EFRW_DirectoryGroupCars;

        private EFDbContext db;

        public EFDirectoryGroupCars(EFDbContext db)
        {

            this.db = db;
        }

        public EFDirectoryGroupCars()
        {

            this.db = new EFDbContext();
        }

        public IEnumerable<Directory_GroupCars> Get()
        {
            try
            {
                return db.Select<Directory_GroupCars>();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Get()"), eventID);
                return null;
            }
        }

        public Directory_GroupCars Get(int id)
        {
            try
            {
                return db.Select<Directory_GroupCars>(id);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Get(id={0})", id), eventID);
                return null;
            }
        }

        public void Add(Directory_GroupCars item)
        {
            try
            {
                db.Insert<Directory_GroupCars>(item);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Add(item={0})", item), eventID);
            }
        }

        public void Update(Directory_GroupCars item)
        {
            try
            {
                db.Update<Directory_GroupCars>(item);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Update(item={0})", item), eventID);
            }
        }

        public void AddOrUpdate(Directory_GroupCars item)
        {
            try
            {
                Directory_GroupCars dbEntry = db.Directory_GroupCars.Find(item.id);
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
                Directory_GroupCars item = db.Delete<Directory_GroupCars>(id);
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

        public Directory_GroupCars Refresh(Directory_GroupCars item)
        {
            try
            {
                db.Entry(item).State = EntityState.Detached;
                return db.Select<Directory_GroupCars>(item.id);
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
    #endregion

    #region ГРУЗЫ
    public class EFDirectoryCargo : IRepository<Directory_Cargo>
    {
        private eventID eventID = eventID.EFRW_EFDirectoryCargo;

        private EFDbContext db;

        public EFDirectoryCargo(EFDbContext db)
        {

            this.db = db;
        }

        public EFDirectoryCargo()
        {

            this.db = new EFDbContext();
        }

        public IEnumerable<Directory_Cargo> Get()
        {
            try
            {
                return db.Select<Directory_Cargo>();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Get()"), eventID);
                return null;
            }
        }

        public Directory_Cargo Get(int id)
        {
            try
            {
                return db.Select<Directory_Cargo>(id);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Get(id={0})", id), eventID);
                return null;
            }
        }

        public void Add(Directory_Cargo item)
        {
            try
            {
                item.user_create = item.user_create ?? System.Environment.UserDomainName + @"\" + System.Environment.UserName;
                item.dt_create = item.dt_create != DateTime.Parse("01.01.0001") ? item.dt_create : DateTime.Now;
                db.Insert<Directory_Cargo>(item);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Add(item={0})", item), eventID);
            }
        }

        public void Update(Directory_Cargo item)
        {
            try
            {
                item.user_edit = item.user_edit ?? System.Environment.UserDomainName + @"\" + System.Environment.UserName;
                item.dt_edit = item.dt_edit != null ? item.dt_edit : DateTime.Now;
                db.Update<Directory_Cargo>(item);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Update(item={0})", item), eventID);
            }
        }

        public void AddOrUpdate(Directory_Cargo item)
        {
            try
            {
                Directory_Cargo dbEntry = db.Directory_Cargo.Find(item.id);
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
                Directory_Cargo item = db.Delete<Directory_Cargo>(id);
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

        public Directory_Cargo Refresh(Directory_Cargo item)
        {
            try
            {
                db.Entry(item).State = EntityState.Detached;
                return db.Select<Directory_Cargo>(item.id);
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
    #endregion

    #endregion

    #region ВАГОНЫ
    // Класс внутренего перемещения вагонов
    public class EFCarsInternal : IRepository<CarsInternal>
    {
        private eventID eventID = eventID.EFRW_EFCarsInternal;

        private EFDbContext db;

        public EFCarsInternal(EFDbContext db)
        {

            this.db = db;
        }

        public EFCarsInternal()
        {

            this.db = new EFDbContext();
        }

        public IEnumerable<CarsInternal> Get()
        {
            try
            {
                return db.Select<CarsInternal>();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Get()"), eventID);
                return null;
            }
        }

        public CarsInternal Get(int id)
        {
            try
            {
                return db.Select<CarsInternal>(id);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Get(id={0})", id), eventID);
                return null;
            }
        }

        public void Add(CarsInternal item)
        {
            try
            {
                item.user_create = item.user_create != null ? item.user_create : System.Environment.UserDomainName + @"\" + System.Environment.UserName;
                item.dt_create = item.dt_create != DateTime.Parse("01.01.0001") ? item.dt_create : DateTime.Now;
                db.Insert<CarsInternal>(item);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Add(item={0})", item), eventID);
            }
        }

        public void Update(CarsInternal item)
        {
            try
            {
                db.Update<CarsInternal>(item);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Update(item={0})", item), eventID);
            }
        }

        public void AddOrUpdate(CarsInternal item)
        {
            try
            {
                CarsInternal dbEntry = db.CarsInternal.Find(item.id);
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
                CarsInternal item = db.Delete<CarsInternal>(id);
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

        public CarsInternal Refresh(CarsInternal item)
        {
            try
            {
                db.Entry(item).State = EntityState.Detached;
                return db.Select<CarsInternal>(item.id);
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
    // Класс операций над  вагоном
    public class EFCarOperations : IRepository<CarOperations>
    {
        private eventID eventID = eventID.EFRW_EFCarOperations;

        private EFDbContext db;

        public EFCarOperations(EFDbContext db)
        {

            this.db = db;
        }

        public EFCarOperations()
        {

            this.db = new EFDbContext();
        }

        public IEnumerable<CarOperations> Get()
        {
            try
            {
                return db.Select<CarOperations>();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Get()"), eventID);
                return null;
            }
        }

        public CarOperations Get(int id)
        {
            try
            {
                return db.Select<CarOperations>(id);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Get(id={0})", id), eventID);
                return null;
            }
        }

        public void Add(CarOperations item)
        {
            try
            {
                item.user_create = item.user_create != null ? item.user_create : System.Environment.UserDomainName + @"\" + System.Environment.UserName;
                item.dt_create = item.dt_create != DateTime.Parse("01.01.0001") ? item.dt_create : DateTime.Now;
                db.Insert<CarOperations>(item);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Add(item={0})", item), eventID);
            }
        }

        public void Update(CarOperations item)
        {
            try
            {
                item.user_edit = item.user_edit != null ? item.user_edit : System.Environment.UserDomainName + @"\" + System.Environment.UserName;
                item.dt_edit = item.dt_edit != null ? item.dt_edit : DateTime.Now;
                db.Update<CarOperations>(item);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Update(item={0})", item), eventID);
            }
        }

        public void AddOrUpdate(CarOperations item)
        {
            try
            {
                CarOperations dbEntry = db.CarOperations.Find(item.id);
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
                CarOperations item = db.Delete<CarOperations>(id);
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

        public CarOperations Refresh(CarOperations item)
        {
            try
            {
                db.Entry(item).State = EntityState.Detached;
                return db.Select<CarOperations>(item.id);
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
    // Класс входящих поставок
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
    // Класс исходящих поставок
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

    public class EFCarConditions : IRepository<CarConditions>
    {
        private eventID eventID = eventID.EFRW_EFCarConditions;

        private EFDbContext db;

        public EFCarConditions(EFDbContext db)
        {

            this.db = db;
        }

        public EFCarConditions()
        {

            this.db = new EFDbContext();
        }

        public IEnumerable<CarConditions> Get()
        {
            try
            {
                return db.Select<CarConditions>();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Get()"), eventID);
                return null;
            }
        }

        public CarConditions Get(int id)
        {
            try
            {
                return db.Select<CarConditions>(id);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Get(id={0})", id), eventID);
                return null;
            }
        }

        public void Add(CarConditions item)
        {
            try
            {
                db.Insert<CarConditions>(item);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Add(item={0})", item), eventID);
            }
        }

        public void Update(CarConditions item)
        {
            try
            {
                db.Update<CarConditions>(item);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Update(item={0})", item), eventID);
            }
        }

        public void AddOrUpdate(CarConditions item)
        {
            try
            {
                CarConditions dbEntry = db.CarConditions.Find(item.id);
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
                CarConditions item = db.Delete<CarConditions>(id);
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

        public CarConditions Refresh(CarConditions item)
        {
            try
            {
                db.Entry(item).State = EntityState.Detached;
                return db.Select<CarConditions>(item.id);
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

    public class EFCarStatus : IRepository<CarStatus>
    {
        private eventID eventID = eventID.EFRW_EFCarStatus;

        private EFDbContext db;

        public EFCarStatus(EFDbContext db)
        {

            this.db = db;
        }

        public EFCarStatus()
        {

            this.db = new EFDbContext();
        }

        public IEnumerable<CarStatus> Get()
        {
            try
            {
                return db.Select<CarStatus>();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Get()"), eventID);
                return null;
            }
        }

        public CarStatus Get(int id)
        {
            try
            {
                return db.Select<CarStatus>(id);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Get(id={0})", id), eventID);
                return null;
            }
        }

        public void Add(CarStatus item)
        {
            try
            {
                db.Insert<CarStatus>(item);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Add(item={0})", item), eventID);
            }
        }

        public void Update(CarStatus item)
        {
            try
            {
                db.Update<CarStatus>(item);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Update(item={0})", item), eventID);
            }
        }

        public void AddOrUpdate(CarStatus item)
        {
            try
            {
                CarStatus dbEntry = db.CarStatus.Find(item.id);
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
                CarStatus item = db.Delete<CarStatus>(id);
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

        public CarStatus Refresh(CarStatus item)
        {
            try
            {
                db.Entry(item).State = EntityState.Detached;
                return db.Select<CarStatus>(item.id);
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
    #endregion

}
