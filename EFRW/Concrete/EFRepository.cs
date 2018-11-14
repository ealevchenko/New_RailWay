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
    /// <summary>
    /// Класс справочник вагонов
    /// </summary>
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
    /// <summary>
    /// Класс справочник типов вагонов
    /// </summary>
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
    /// <summary>
    /// Класс справочник групп вагонов
    /// </summary>
    public class EFDirectoryGroupCars : IRepository<Directory_GroupCars>
    {
        private eventID eventID = eventID.EFRW_EFDirectoryGroupCars;

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
    /// <summary>
    /// Класс справочник грузов
    /// </summary>
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
    /// <summary>
    /// Класс типов грузов
    /// </summary>
    public class EFDirectoryTypeCargo : IRepository<Directory_TypeCargo>
    {
        private eventID eventID = eventID.EFRW_EFDirectoryTypeCargo;

        private EFDbContext db;

        public EFDirectoryTypeCargo(EFDbContext db)
        {

            this.db = db;
        }

        public EFDirectoryTypeCargo()
        {

            this.db = new EFDbContext();
        }

        public IEnumerable<Directory_TypeCargo> Get()
        {
            try
            {
                return db.Select<Directory_TypeCargo>();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Get()"), eventID);
                return null;
            }
        }

        public Directory_TypeCargo Get(int id)
        {
            try
            {
                return db.Select<Directory_TypeCargo>(id);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Get(id={0})", id), eventID);
                return null;
            }
        }

        public void Add(Directory_TypeCargo item)
        {
            try
            {
                db.Insert<Directory_TypeCargo>(item);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Add(item={0})", item), eventID);
            }
        }

        public void Update(Directory_TypeCargo item)
        {
            try
            {
                db.Update<Directory_TypeCargo>(item);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Update(item={0})", item), eventID);
            }
        }

        public void AddOrUpdate(Directory_TypeCargo item)
        {
            try
            {
                Directory_TypeCargo dbEntry = db.Directory_TypeCargo.Find(item.id);
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
                Directory_TypeCargo item = db.Delete<Directory_TypeCargo>(id);
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

        public Directory_TypeCargo Refresh(Directory_TypeCargo item)
        {
            try
            {
                db.Entry(item).State = EntityState.Detached;
                return db.Select<Directory_TypeCargo>(item.id);
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
    /// <summary>
    /// Класс группы грузов грузов
    /// </summary>
    public class EFDirectoryGroupCargo : IRepository<Directory_GroupCargo>
    {
        private eventID eventID = eventID.EFRW_EFDirectoryGroupCargo;

        private EFDbContext db;

        public EFDirectoryGroupCargo(EFDbContext db)
        {

            this.db = db;
        }

        public EFDirectoryGroupCargo()
        {

            this.db = new EFDbContext();
        }

        public IEnumerable<Directory_GroupCargo> Get()
        {
            try
            {
                return db.Select<Directory_GroupCargo>();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Get()"), eventID);
                return null;
            }
        }

        public Directory_GroupCargo Get(int id)
        {
            try
            {
                return db.Select<Directory_GroupCargo>(id);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Get(id={0})", id), eventID);
                return null;
            }
        }

        public void Add(Directory_GroupCargo item)
        {
            try
            {
                db.Insert<Directory_GroupCargo>(item);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Add(item={0})", item), eventID);
            }
        }

        public void Update(Directory_GroupCargo item)
        {
            try
            {
                db.Update<Directory_GroupCargo>(item);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Update(item={0})", item), eventID);
            }
        }

        public void AddOrUpdate(Directory_GroupCargo item)
        {
            try
            {
                Directory_GroupCargo dbEntry = db.Directory_GroupCargo.Find(item.id);
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
                Directory_GroupCargo item = db.Delete<Directory_GroupCargo>(id);
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

        public Directory_GroupCargo Refresh(Directory_GroupCargo item)
        {
            try
            {
                db.Entry(item).State = EntityState.Detached;
                return db.Select<Directory_GroupCargo>(item.id);
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

    #region ВЛАДЕЛЬЦЫ
    /// <summary>
    /// Класс список владельцев
    /// </summary>
    public class EFDirectoryOwners : IRepository<Directory_Owners>
    {
        private eventID eventID = eventID.EFRW_EFDirectoryOwners;

        private EFDbContext db;

        public EFDirectoryOwners(EFDbContext db)
        {

            this.db = db;
        }

        public EFDirectoryOwners()
        {

            this.db = new EFDbContext();
        }

        public IEnumerable<Directory_Owners> Get()
        {
            try
            {
                return db.Select<Directory_Owners>();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Get()"), eventID);
                return null;
            }
        }

        public Directory_Owners Get(int id)
        {
            try
            {
                return db.Select<Directory_Owners>(id);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Get(id={0})", id), eventID);
                return null;
            }
        }

        public void Add(Directory_Owners item)
        {
            try
            {
                item.user_create = item.user_create ?? System.Environment.UserDomainName + @"\" + System.Environment.UserName;
                item.dt_create = item.dt_create != DateTime.Parse("01.01.0001") ? item.dt_create : DateTime.Now;
                db.Insert<Directory_Owners>(item);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Add(item={0})", item), eventID);
            }
        }

        public void Update(Directory_Owners item)
        {
            try
            {
                item.user_edit = item.user_edit ?? System.Environment.UserDomainName + @"\" + System.Environment.UserName;
                item.dt_edit = item.dt_edit != null ? item.dt_edit : DateTime.Now;
                db.Update<Directory_Owners>(item);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Update(item={0})", item), eventID);
            }
        }

        public void AddOrUpdate(Directory_Owners item)
        {
            try
            {
                Directory_Owners dbEntry = db.Directory_Owners.Find(item.id);
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
                Directory_Owners item = db.Delete<Directory_Owners>(id);
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

        public Directory_Owners Refresh(Directory_Owners item)
        {
            try
            {
                db.Entry(item).State = EntityState.Detached;
                return db.Select<Directory_Owners>(item.id);
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
    /// <summary>
    /// Класс аренды вагонов и владельцев
    /// </summary>
    public class EFDirectoryOwnerCars : IRepository<Directory_OwnerCars>
    {
        private eventID eventID = eventID.EFRW_EFDirectoryOwnerCars;

        private EFDbContext db;

        public EFDirectoryOwnerCars(EFDbContext db)
        {

            this.db = db;
        }

        public EFDirectoryOwnerCars()
        {

            this.db = new EFDbContext();
        }

        public IEnumerable<Directory_OwnerCars> Get()
        {
            try
            {
                return db.Select<Directory_OwnerCars>();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Get()"), eventID);
                return null;
            }
        }

        public Directory_OwnerCars Get(int id)
        {
            try
            {
                return db.Select<Directory_OwnerCars>(id);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Get(id={0})", id), eventID);
                return null;
            }
        }

        public void Add(Directory_OwnerCars item)
        {
            try
            {
                item.user_create = item.user_create ?? System.Environment.UserDomainName + @"\" + System.Environment.UserName;
                item.dt_create = item.dt_create != DateTime.Parse("01.01.0001") ? item.dt_create : DateTime.Now;
                db.Insert<Directory_OwnerCars>(item);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Add(item={0})", item), eventID);
            }
        }

        public void Update(Directory_OwnerCars item)
        {
            try
            {
                item.user_edit = item.user_edit ?? System.Environment.UserDomainName + @"\" + System.Environment.UserName;
                item.dt_edit = item.dt_edit != null ? item.dt_edit : DateTime.Now;
                db.Update<Directory_OwnerCars>(item);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Update(item={0})", item), eventID);
            }
        }

        public void AddOrUpdate(Directory_OwnerCars item)
        {
            try
            {
                Directory_OwnerCars dbEntry = db.Directory_OwnerCars.Find(item.id);
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
                Directory_OwnerCars item = db.Delete<Directory_OwnerCars>(id);
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

        public Directory_OwnerCars Refresh(Directory_OwnerCars item)
        {
            try
            {
                db.Entry(item).State = EntityState.Detached;
                return db.Select<Directory_OwnerCars>(item.id);
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

    #region ВНУТРЕНИЕ СПРАВОЧНИКИ
    /// <summary>
    /// Класс внутрение станции АМКР
    /// </summary>
    public class EFDirectoryInternalStations : IRepository<Directory_InternalStations>
    {
        private eventID eventID = eventID.EFRW_EFDirectoryInternalStations;

        private EFDbContext db;

        public EFDirectoryInternalStations(EFDbContext db)
        {

            this.db = db;
        }

        public EFDirectoryInternalStations()
        {

            this.db = new EFDbContext();
        }

        public IEnumerable<Directory_InternalStations> Get()
        {
            try
            {
                return db.Select<Directory_InternalStations>();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Get()"), eventID);
                return null;
            }
        }

        public Directory_InternalStations Get(int id)
        {
            try
            {
                return db.Select<Directory_InternalStations>(id);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Get(id={0})", id), eventID);
                return null;
            }
        }

        public void Add(Directory_InternalStations item)
        {
            try
            {
                item.user_create = item.user_create ?? System.Environment.UserDomainName + @"\" + System.Environment.UserName;
                item.dt_create = item.dt_create != DateTime.Parse("01.01.0001") ? item.dt_create : DateTime.Now;
                db.Insert<Directory_InternalStations>(item);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Add(item={0})", item), eventID);
            }
        }

        public void Update(Directory_InternalStations item)
        {
            try
            {
                item.user_edit = item.user_edit ?? System.Environment.UserDomainName + @"\" + System.Environment.UserName;
                item.dt_edit = item.dt_edit != null ? item.dt_edit : DateTime.Now;
                db.Update<Directory_InternalStations>(item);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Update(item={0})", item), eventID);
            }
        }

        public void AddOrUpdate(Directory_InternalStations item)
        {
            try
            {
                Directory_InternalStations dbEntry = db.Directory_InternalStations.Find(item.id);
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
                Directory_InternalStations item = db.Delete<Directory_InternalStations>(id);
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

        public Directory_InternalStations Refresh(Directory_InternalStations item)
        {
            try
            {
                db.Entry(item).State = EntityState.Detached;
                return db.Select<Directory_InternalStations>(item.id);
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
    /// <summary>
    /// Класс цеха
    /// </summary>
    public class EFDirectoryShops : IRepository<Directory_Shops>
    {
        private eventID eventID = eventID.EFRW_EFDirectoryShops;

        private EFDbContext db;

        public EFDirectoryShops(EFDbContext db)
        {

            this.db = db;
        }

        public EFDirectoryShops()
        {

            this.db = new EFDbContext();
        }

        public IEnumerable<Directory_Shops> Get()
        {
            try
            {
                return db.Select<Directory_Shops>();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Get()"), eventID);
                return null;
            }
        }

        public Directory_Shops Get(int id)
        {
            try
            {
                return db.Select<Directory_Shops>(id);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Get(id={0})", id), eventID);
                return null;
            }
        }

        public void Add(Directory_Shops item)
        {
            try
            {
                item.user_create = item.user_create ?? System.Environment.UserDomainName + @"\" + System.Environment.UserName;
                item.dt_create = item.dt_create != DateTime.Parse("01.01.0001") ? item.dt_create : DateTime.Now;
                db.Insert<Directory_Shops>(item);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Add(item={0})", item), eventID);
            }
        }

        public void Update(Directory_Shops item)
        {
            try
            {
                item.user_edit = item.user_edit ?? System.Environment.UserDomainName + @"\" + System.Environment.UserName;
                item.dt_edit = item.dt_edit != null ? item.dt_edit : DateTime.Now;
                db.Update<Directory_Shops>(item);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Update(item={0})", item), eventID);
            }
        }

        public void AddOrUpdate(Directory_Shops item)
        {
            try
            {
                Directory_Shops dbEntry = db.Directory_Shops.Find(item.id);
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
                Directory_Shops item = db.Delete<Directory_Shops>(id);
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

        public Directory_Shops Refresh(Directory_Shops item)
        {
            try
            {
                db.Entry(item).State = EntityState.Detached;
                return db.Select<Directory_Shops>(item.id);
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
    /// <summary>
    /// Класс вагонаопрокид
    /// </summary>
    public class EFDirectoryOverturn : IRepository<Directory_Overturn>
    {
        private eventID eventID = eventID.EFRW_EFDirectoryOverturn;

        private EFDbContext db;

        public EFDirectoryOverturn(EFDbContext db)
        {

            this.db = db;
        }

        public EFDirectoryOverturn()
        {

            this.db = new EFDbContext();
        }

        public IEnumerable<Directory_Overturn> Get()
        {
            try
            {
                return db.Select<Directory_Overturn>();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Get()"), eventID);
                return null;
            }
        }

        public Directory_Overturn Get(int id)
        {
            try
            {
                return db.Select<Directory_Overturn>(id);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Get(id={0})", id), eventID);
                return null;
            }
        }

        public void Add(Directory_Overturn item)
        {
            try
            {
                item.user_create = item.user_create ?? System.Environment.UserDomainName + @"\" + System.Environment.UserName;
                item.dt_create = item.dt_create != DateTime.Parse("01.01.0001") ? item.dt_create : DateTime.Now;
                db.Insert<Directory_Overturn>(item);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Add(item={0})", item), eventID);
            }
        }

        public void Update(Directory_Overturn item)
        {
            try
            {
                item.user_edit = item.user_edit ?? System.Environment.UserDomainName + @"\" + System.Environment.UserName;
                item.dt_edit = item.dt_edit != null ? item.dt_edit : DateTime.Now;
                db.Update<Directory_Overturn>(item);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Update(item={0})", item), eventID);
            }
        }

        public void AddOrUpdate(Directory_Overturn item)
        {
            try
            {
                Directory_Overturn dbEntry = db.Directory_Overturn.Find(item.id);
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
                Directory_Overturn item = db.Delete<Directory_Overturn>(id);
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

        public Directory_Overturn Refresh(Directory_Overturn item)
        {
            try
            {
                db.Entry(item).State = EntityState.Detached;
                return db.Select<Directory_Overturn>(item.id);
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
    /// <summary>
    /// Класс пути и перегоны
    /// </summary>
    public class EFDirectoryWays : IRepository<Directory_Ways>
    {
        private eventID eventID = eventID.EFRW_EFDirectoryWays;

        private EFDbContext db;

        public EFDirectoryWays(EFDbContext db)
        {

            this.db = db;
        }

        public EFDirectoryWays()
        {

            this.db = new EFDbContext();
        }

        public IEnumerable<Directory_Ways> Get()
        {
            try
            {
                return db.Select<Directory_Ways>();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Get()"), eventID);
                return null;
            }
        }

        public Directory_Ways Get(int id)
        {
            try
            {
                return db.Select<Directory_Ways>(id);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Get(id={0})", id), eventID);
                return null;
            }
        }

        public void Add(Directory_Ways item)
        {
            try
            {
                item.user_create = item.user_create ?? System.Environment.UserDomainName + @"\" + System.Environment.UserName;
                item.dt_create = item.dt_create != DateTime.Parse("01.01.0001") ? item.dt_create : DateTime.Now;
                db.Insert<Directory_Ways>(item);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Add(item={0})", item), eventID);
            }
        }

        public void Update(Directory_Ways item)
        {
            try
            {
                item.user_edit = item.user_edit ?? System.Environment.UserDomainName + @"\" + System.Environment.UserName;
                item.dt_edit = item.dt_edit != null ? item.dt_edit : DateTime.Now;
                db.Update<Directory_Ways>(item);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Update(item={0})", item), eventID);
            }
        }

        public void AddOrUpdate(Directory_Ways item)
        {
            try
            {
                Directory_Ways dbEntry = db.Directory_Ways.Find(item.id);
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
                Directory_Ways item = db.Delete<Directory_Ways>(id);
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

        public Directory_Ways Refresh(Directory_Ways item)
        {
            try
            {
                db.Entry(item).State = EntityState.Detached;
                return db.Select<Directory_Ways>(item.id);
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
    /// <summary>
    /// Класс типы путей
    /// </summary>
    public class EFDirectoryTypeWays : IRepository<Directory_TypeWays>
    {
        private eventID eventID = eventID.EFRW_EFDirectoryTypeWays;

        private EFDbContext db;

        public EFDirectoryTypeWays(EFDbContext db)
        {

            this.db = db;
        }

        public EFDirectoryTypeWays()
        {

            this.db = new EFDbContext();
        }

        public IEnumerable<Directory_TypeWays> Get()
        {
            try
            {
                return db.Select<Directory_TypeWays>();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Get()"), eventID);
                return null;
            }
        }

        public Directory_TypeWays Get(int id)
        {
            try
            {
                return db.Select<Directory_TypeWays>(id);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Get(id={0})", id), eventID);
                return null;
            }
        }

        public void Add(Directory_TypeWays item)
        {
            try
            {
                db.Insert<Directory_TypeWays>(item);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Add(item={0})", item), eventID);
            }
        }

        public void Update(Directory_TypeWays item)
        {
            try
            {
                db.Update<Directory_TypeWays>(item);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Update(item={0})", item), eventID);
            }
        }

        public void AddOrUpdate(Directory_TypeWays item)
        {
            try
            {
                Directory_TypeWays dbEntry = db.Directory_TypeWays.Find(item.id);
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
                Directory_TypeWays item = db.Delete<Directory_TypeWays>(id);
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

        public Directory_TypeWays Refresh(Directory_TypeWays item)
        {
            try
            {
                db.Entry(item).State = EntityState.Detached;
                return db.Select<Directory_TypeWays>(item.id);
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

    #region ОБЩИЕ СПРАВОЧНИКИ
    /// <summary>
    /// Класс грузополучатели
    /// </summary>
    public class EFDirectoryConsignee : IRepository<Directory_Consignee>
    {
        private eventID eventID = eventID.EFRW_EFDirectoryConsignee;

        private EFDbContext db;

        public EFDirectoryConsignee(EFDbContext db)
        {

            this.db = db;
        }

        public EFDirectoryConsignee()
        {

            this.db = new EFDbContext();
        }

        public IEnumerable<Directory_Consignee> Get()
        {
            try
            {
                return db.Select<Directory_Consignee>();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Get()"), eventID);
                return null;
            }
        }

        public Directory_Consignee Get(int id)
        {
            try
            {
                return db.Select<Directory_Consignee>(id);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Get(id={0})", id), eventID);
                return null;
            }
        }

        public void Add(Directory_Consignee item)
        {
            try
            {
                item.user_create = item.user_create ?? System.Environment.UserDomainName + @"\" + System.Environment.UserName;
                item.dt_create = item.dt_create != DateTime.Parse("01.01.0001") ? item.dt_create : DateTime.Now;
                db.Insert<Directory_Consignee>(item);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Add(item={0})", item), eventID);
            }
        }

        public void Update(Directory_Consignee item)
        {
            try
            {
                item.user_edit = item.user_edit ?? System.Environment.UserDomainName + @"\" + System.Environment.UserName;
                item.dt_edit = item.dt_edit != null ? item.dt_edit : DateTime.Now;
                db.Update<Directory_Consignee>(item);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Update(item={0})", item), eventID);
            }
        }

        public void AddOrUpdate(Directory_Consignee item)
        {
            try
            {
                Directory_Consignee dbEntry = db.Directory_Consignee.Find(item.id);
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
                Directory_Consignee item = db.Delete<Directory_Consignee>(id);
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

        public Directory_Consignee Refresh(Directory_Consignee item)
        {
            try
            {
                db.Entry(item).State = EntityState.Detached;
                return db.Select<Directory_Consignee>(item.id);
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
    /// <summary>
    /// Класс стран
    /// </summary>
    public class EFDirectoryCountry : IRepository<Directory_Country>
    {
        private eventID eventID = eventID.EFRW_EFDirectoryCountry;

        private EFDbContext db;

        public EFDirectoryCountry(EFDbContext db)
        {

            this.db = db;
        }

        public EFDirectoryCountry()
        {

            this.db = new EFDbContext();
        }

        public IEnumerable<Directory_Country> Get()
        {
            try
            {
                return db.Select<Directory_Country>();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Get()"), eventID);
                return null;
            }
        }

        public Directory_Country Get(int id)
        {
            try
            {
                return db.Select<Directory_Country>(id);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Get(id={0})", id), eventID);
                return null;
            }
        }

        public void Add(Directory_Country item)
        {
            try
            {

                db.Insert<Directory_Country>(item);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Add(item={0})", item), eventID);
            }
        }

        public void Update(Directory_Country item)
        {
            try
            {
                db.Update<Directory_Country>(item);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Update(item={0})", item), eventID);
            }
        }

        public void AddOrUpdate(Directory_Country item)
        {
            try
            {
                Directory_Country dbEntry = db.Directory_Country.Find(item.id);
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
                Directory_Country item = db.Delete<Directory_Country>(id);
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

        public Directory_Country Refresh(Directory_Country item)
        {
            try
            {
                db.Entry(item).State = EntityState.Detached;
                return db.Select<Directory_Country>(item.id);
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
    /// <summary>
    /// Класс внешних станций
    /// </summary>
    public class EFDirectoryExternalStations : IRepository<Directory_ExternalStations>
    {
        private eventID eventID = eventID.EFRW_EFDirectoryExternalStations;

        private EFDbContext db;

        public EFDirectoryExternalStations(EFDbContext db)
        {

            this.db = db;
        }

        public EFDirectoryExternalStations()
        {

            this.db = new EFDbContext();
        }

        public IEnumerable<Directory_ExternalStations> Get()
        {
            try
            {
                return db.Select<Directory_ExternalStations>();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Get()"), eventID);
                return null;
            }
        }

        public Directory_ExternalStations Get(int id)
        {
            try
            {
                return db.Select<Directory_ExternalStations>(id);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Get(id={0})", id), eventID);
                return null;
            }
        }

        public void Add(Directory_ExternalStations item)
        {
            try
            {

                db.Insert<Directory_ExternalStations>(item);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Add(item={0})", item), eventID);
            }
        }

        public void Update(Directory_ExternalStations item)
        {
            try
            {
                db.Update<Directory_ExternalStations>(item);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Update(item={0})", item), eventID);
            }
        }

        public void AddOrUpdate(Directory_ExternalStations item)
        {
            try
            {
                Directory_ExternalStations dbEntry = db.Directory_ExternalStations.Find(item.id);
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
                Directory_ExternalStations item = db.Delete<Directory_ExternalStations>(id);
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

        public Directory_ExternalStations Refresh(Directory_ExternalStations item)
        {
            try
            {
                db.Entry(item).State = EntityState.Detached;
                return db.Select<Directory_ExternalStations>(item.id);
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
    /// <summary>
    /// Класс внутренего перемещения вагонов
    /// </summary>
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
    /// <summary>
    /// Класс операций над  вагоном
    /// </summary>
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
    /// <summary>
    /// 
    /// </summary>
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
    /// <summary>
    /// Класс сотояний вагона
    /// </summary>
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
