using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFTD.Abstract
{
    public interface IRepository<T> : IDisposable where T : class
    {
        Database Database { get; }
        IEnumerable<T> Get();       // получение всех объектов
        T Get(int id);              // получение одного объекта по id
        void Add(T item);           // создание объекта
        void Update(T item);        // обновление объекта
        void AddOrUpdate(T item);   // добавить или обновить
        void Delete(int id);        // удаление объекта по id
        int Save();                 // сохранение изменений в базе
        T Refresh(T item);          // Обновить объект с базаой
    }
}
