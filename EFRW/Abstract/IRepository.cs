using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFRW.Abstract
{
    interface IRepository<T> : IDisposable where T : class
    {
        IEnumerable<T> GetList(); // получение всех объектов
        T Get(int id); // получение одного объекта по id
        void Add(T item); // создание объекта
        void Update(T item); // обновление объекта
        void Delete(int id); // удаление объекта по id
        int Save();  // сохранение изменений
    }

}
