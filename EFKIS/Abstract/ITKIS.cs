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
        #region Перенос прибывших с УЗ вагонов по данным КИС
        IQueryable<BufferArrivalSostav> BufferArrivalSostav { get; }
        IQueryable<BufferArrivalSostav> GetBufferArrivalSostav();
        BufferArrivalSostav GetBufferArrivalSostav(int id);
        int SaveArrivalSostav(BufferArrivalSostav BufferArrivalSostav);
        BufferArrivalSostav DeleteBufferArrivalSostav(int id);
        DateTime? GetLastDateTimeBufferArrivalSostav();
        IQueryable<BufferArrivalSostav> GetBufferArrivalSostav(DateTime start, DateTime stop);
        IQueryable<BufferArrivalSostav> GetBufferArrivalSostavNoClose();
        BufferArrivalSostav GetBufferArrivalSostavOfNatur(int natur);
        int CloseBufferArrivalSostav(int id);
        #endregion

        #region BufferInputSostav Перенос прибывающих вагонов на станцию по данным КИС
        IQueryable<BufferInputSostav> BufferInputSostav { get; }
        IQueryable<BufferInputSostav> GetBufferInputSostav();
        BufferInputSostav GetBufferInputSostav(int id);
        int SaveBufferInputSostav(BufferInputSostav BufferInputSostav);
        BufferInputSostav DeleteBufferInputSostav(int id);

        DateTime? GetLastDateTimeBufferInputSostav();
        IQueryable<BufferInputSostav> GetBufferInputSostav(DateTime start, DateTime stop);
        #endregion

        #region BufferOutputSostav Перенос отправленных вагонов на станцию по данным КИС
        IQueryable<BufferOutputSostav> BufferOutputSostav { get; }
        IQueryable<BufferOutputSostav> GetBufferOutputSostav();
        BufferOutputSostav GetBufferOutputSostav(int id);
        int SaveBufferOutputSostav(BufferOutputSostav BufferOutputSostav);
        BufferOutputSostav DeleteBufferOutputSostav(int id);

        DateTime? GetLastDateTimeBufferOutputSostav();
        IQueryable<BufferOutputSostav> GetBufferOutputSostav(DateTime start, DateTime stop);
        #endregion

    }
}
