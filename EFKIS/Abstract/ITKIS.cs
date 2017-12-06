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
        IQueryable<BufferArrivalSostav> BufferArrivalSostav { get; }
        IQueryable<BufferArrivalSostav> GetBufferArrivalSostav();
        BufferArrivalSostav GetBufferArrivalSostav(int id);
        int SaveArrivalSostav(BufferArrivalSostav BufferArrivalSostav);
        BufferArrivalSostav DeleteBufferArrivalSostav(int id);
        DateTime? GetLastDateTime();
        IQueryable<BufferArrivalSostav> GetBufferArrivalSostav(DateTime start, DateTime stop);
        IQueryable<BufferArrivalSostav> GetBufferArrivalSostavNoClose();
        BufferArrivalSostav GetBufferArrivalSostavOfNatur(int natur);
        #endregion
    }
}
