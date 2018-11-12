using EFRW.Abstract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFRW.Concrete
{
    public class Repository
    {
        public static IQueryable<TEntity> Select<TEntity>() where TEntity : class
        {
            EFDbContext context = new EFDbContext();

            // Здесь мы можем указывать различные настройки контекста,
            // например выводить в отладчик сгенерированный SQL-код
            context.Database.Log =
                (s => System.Diagnostics.Debug.WriteLine(s));

            // Загрузка данных с помощью универсального метода Set
            return context.Set<TEntity>();
        }

        //public static IQueryable<TEntity> Select<TEntity>(int id) where TEntity : class
        //{
        //    EFDbContext context = new EFDbContext();

        //    // Здесь мы можем указывать различные настройки контекста,
        //    // например выводить в отладчик сгенерированный SQL-код
        //    context.Database.Log =
        //        (s => System.Diagnostics.Debug.WriteLine(s));

        //    // Загрузка данных с помощью универсального метода Set

        //    //return context.Set<TEntity>().ToList().Find(id);
        //    return context.Set<TEntity>().Find(id).AsQueryable();
        //}

        public static void Insert<TEntity>(TEntity entity) where TEntity : class
        {
            // Настройки контекста
            EFDbContext context = new EFDbContext();
            context.Database.Log = (s => System.Diagnostics.Debug.WriteLine(s));

            context.Entry(entity).State = EntityState.Added;
            context.SaveChanges();
        }
        /// <summary>
        /// Запись нескольких полей в БД
        /// </summary>
        public static void Inserts<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            // Настройки контекста
            EFDbContext context = new EFDbContext();

            // Отключаем отслеживание и проверку изменений для оптимизации вставки множества полей
            context.Configuration.AutoDetectChangesEnabled = false;
            context.Configuration.ValidateOnSaveEnabled = false;

            context.Database.Log = (s => System.Diagnostics.Debug.WriteLine(s));


            foreach (TEntity entity in entities)
                context.Entry(entity).State = EntityState.Added;
            context.SaveChanges();

            context.Configuration.AutoDetectChangesEnabled = true;
            context.Configuration.ValidateOnSaveEnabled = true;
        }

        public static void Update<TEntity>(TEntity entity, EFDbContext context) where TEntity : class
        {
            // Настройки контекста

            context.Database.Log = (s => System.Diagnostics.Debug.WriteLine(s));

            context.Entry<TEntity>(entity).State = EntityState.Modified;
            context.SaveChanges();
        }

        public static void Delete<TEntity>(TEntity entity, EFDbContext context) where TEntity : class
        {
            // Настройки контекста
            //EFDbContext context = new EFDbContext();
            context.Database.Log = (s => System.Diagnostics.Debug.WriteLine(s));

            context.Entry<TEntity>(entity).State = EntityState.Deleted;
            context.SaveChanges();
        }
    }
}
