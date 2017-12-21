using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageLog
{
    public enum service : int
    {
        Null = -1,
        Test = 1,
        // сервисы МеталлургТранса
        ServicesMT = 100,
        TransferApproaches = 111,
        TransferArrival = 112,        
        TransferMT = 121,
        HostMT = 122,
        CloseTransferApproaches = 131,
        CloseTransferArrival = 132,

        // сервисы КИС
        ServicesKIS = 200,
        CopyArrivalSostavKIS = 211,     // поток переноса данных в буфер
        TransferArrivalKIS = 212,       // поток переноса вагонов в прибытие с УЗ по данным КИС
        CloseArrivalSostavKIS = 213,    // поток закрытия данных в буфере

        CopyInputSostavKIS = 221,     // поток переноса данных в буфер прибытие из станций АМКР
        TransferInputKIS = 222,       // поток переноса вагонов в прибытие из станций АМКР по данным КИС
        CloseInputSostavKIS = 223,    // поток закрытия данных в буфере

        CopyOutputSostavKIS = 231,     // поток переноса данных в буфер прибытие из станций АМКР
        TransferOutputKIS = 232,       // поток переноса вагонов в прибытие из станций АМКР по данным КИС
        CloseOutputSostavKIS = 233,    // поток закрытия данных в буфере
    }

}
