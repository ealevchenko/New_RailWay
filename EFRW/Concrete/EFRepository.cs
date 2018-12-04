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
}
