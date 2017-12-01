using EFKIS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFKIS.Abstract
{
    public interface ITKIS
    {
        #region Перенос вагонов по данным КИС
        IQueryable<ArrivalSostav> ArrivalSostav { get; }
        IQueryable<ArrivalSostav> GetArrivalSostav();
        ArrivalSostav GetArrivalSostav(int id);
        int SaveArrivalSostav(ArrivalSostav ArrivalSostav);
        ArrivalSostav DeleteArrivalSostav(int id);
        DateTime? GetLastDateTime();
        IQueryable<ArrivalSostav> GetArrivalSostav(DateTime start, DateTime stop);
        #endregion
    }
}
