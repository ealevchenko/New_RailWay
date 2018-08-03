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
        IQueryable<RCBufferArrivalSostav> RCBufferArrivalSostav { get; }
        IQueryable<RCBufferArrivalSostav> GetRCBufferArrivalSostav();
        RCBufferArrivalSostav GetRCBufferArrivalSostav(int id);
        int SaveRCBufferArrivalSostav(RCBufferArrivalSostav BufferArrivalSostav);
        RCBufferArrivalSostav DeleteRCBufferArrivalSostav(int id);
        DateTime? GetLastDateTimeRCBufferArrivalSostav();
        IQueryable<RCBufferArrivalSostav> GetRCBufferArrivalSostav(DateTime start, DateTime stop);
        IQueryable<RCBufferArrivalSostav> GetRCBufferArrivalSostavNoClose();
        RCBufferArrivalSostav GetRCBufferArrivalSostavOfNatur(int natur);
        int CloseRCBufferArrivalSostav(int id);
        int CloseRCBufferArrivalSostav(int id, string user);
        #endregion

        #region BufferInputSostav Перенос прибывающих вагонов на станцию по данным КИС
        IQueryable<RCBufferInputSostav> RCBufferInputSostav { get; }
        IQueryable<RCBufferInputSostav> GetRCBufferInputSostav();
        RCBufferInputSostav GetRCBufferInputSostav(int id);
        int SaveRCBufferInputSostav(RCBufferInputSostav BufferInputSostav);
        RCBufferInputSostav DeleteRCBufferInputSostav(int id);

        DateTime? GetLastDateTimeRCBufferInputSostav();
        IQueryable<RCBufferInputSostav> GetRCBufferInputSostav(DateTime start, DateTime stop);
        IQueryable<RCBufferInputSostav> GetRCBufferInputSostavNoClose();
        #endregion

        #region BufferOutputSostav Перенос отправленных вагонов на станцию по данным КИС
        IQueryable<RCBufferOutputSostav> RCBufferOutputSostav { get; }
        IQueryable<RCBufferOutputSostav> GetRCBufferOutputSostav();
        RCBufferOutputSostav GetRCBufferOutputSostav(int id);
        int SaveRCBufferOutputSostav(RCBufferOutputSostav BufferOutputSostav);
        RCBufferOutputSostav DeleteRCBufferOutputSostav(int id);

        DateTime? GetLastDateTimeRCBufferOutputSostav();
        IQueryable<RCBufferOutputSostav> GetRCBufferOutputSostav(DateTime start, DateTime stop);
        IQueryable<RCBufferOutputSostav> GetRCBufferOutputSostavNoClose();
        #endregion

        #region Перенос прибывших с УЗ вагонов по данным КИС
        IQueryable<RWBufferArrivalSostav> RWBufferArrivalSostav { get; }
        IQueryable<RWBufferArrivalSostav> GetRWBufferArrivalSostav();
        RWBufferArrivalSostav GetRWBufferArrivalSostav(int id);
        int SaveRWBufferArrivalSostav(RWBufferArrivalSostav RWBufferArrivalSostav);
        RWBufferArrivalSostav DeleteRWBufferArrivalSostav(int id);
        DateTime? GetLastDateTimeRWBufferArrivalSostav();
        IQueryable<RWBufferArrivalSostav> GetRWBufferArrivalSostav(DateTime start, DateTime stop);
        IQueryable<RWBufferArrivalSostav> GetRWBufferArrivalSostavNoClose();
        RWBufferArrivalSostav GetRWBufferArrivalSostavOfNatur(int natur);
        int CloseRWBufferArrivalSostav(int id);
        int CloseRWBufferArrivalSostav(int id, string user);
        int CloseRWBufferArrivalSostav(int id, string coment, string user);
        #endregion

        #region RWBufferSendingSostav Перенос отправленных на УЗ вагонов по данным КИС
        IQueryable<RWBufferSendingSostav> RWBufferSendingSostav  { get; }
        IQueryable<RWBufferSendingSostav> GetRWBufferSendingSostav();
        RWBufferSendingSostav GetRWBufferSendingSostav(int id);
        int SaveRWBufferSendingSostav(RWBufferSendingSostav RWBufferSendingSostav);
        RWBufferSendingSostav DeleteRWBufferSendingSostav(int id);
        DateTime? GetLastDateTimeRWBufferSendingSostav();
        IQueryable<RWBufferSendingSostav> GetRWBufferSendingSostav(DateTime start, DateTime stop);
        IQueryable<RWBufferSendingSostav> GetRWBufferSendingSostavNoClose();
        RWBufferSendingSostav GetRWBufferSendingSostavOfNatur(int natur);
        int CloseRWBufferSendingSostav(int id);
        int CloseRWBufferSendingSostav(int id, string user);
        int CloseRWBufferSendingSostav(int id, string coment,  string user);
        #endregion

        #region RWBufferInputSostav Перенос вагонов по внутреним станциям по прибытию
        IQueryable<RWBufferInputSostav> RWBufferInputSostav { get; }
        IQueryable<RWBufferInputSostav> GetRWBufferInputSostav();
        RWBufferInputSostav GetRWBufferInputSostav(int id);
        int SaveRWBufferInputSostav(RWBufferInputSostav RWBufferInputSostav);
        RWBufferInputSostav DeleteRWBufferInputSostav(int id);
        DateTime? GetLastDateTimeRWBufferInputSostav();
        IQueryable<RWBufferInputSostav> GetRWBufferInputSostav(DateTime start, DateTime stop);
        IQueryable<RWBufferInputSostav> GetRWBufferInputSostavNoClose();
        RWBufferInputSostav GetRWBufferInputSostavOfNatur(int natur);
        int CloseRWBufferInputSostav(int id);
        int CloseRWBufferInputSostav(int id, string user);
        int CloseRWBufferInputSostav(int id, string coment, string user);
        #endregion

        #region RWBufferOutputSostav Перенос вагонов по внутреним станциям по отправке
        IQueryable<RWBufferOutputSostav> RWBufferOutputSostav { get; }
        IQueryable<RWBufferOutputSostav> GetRWBufferOutputSostav();
        RWBufferOutputSostav GetRWBufferOutputSostav(int id);
        int SaveRWBufferOutputSostav(RWBufferOutputSostav RWBufferOutputSostav);
        RWBufferOutputSostav DeleteRWBufferOutputSostav(int id);
        DateTime? GetLastDateTimeRWBufferOutputSostav();
        IQueryable<RWBufferOutputSostav> GetRWBufferOutputSostav(DateTime start, DateTime stop);
        IQueryable<RWBufferOutputSostav> GetRWBufferOutputSostavNoClose();
        int CloseRWBufferOutputSostav(int id);
        int CloseRWBufferOutputSostav(int id, string user);
        int CloseRWBufferOutputSostav(int id, string coment, string user);
        #endregion

    }
}
