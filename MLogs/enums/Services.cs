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
        //CopyArrivalSostavMT = 211,  // поток переноса данных
        //TransferArrivalMT = 212,    // поток переноса вагонов в прибытие с УЗ
        CopyArrivalSostavKIS = 211, // поток переноса данных
        TransferArrivalKIS = 212,   // поток переноса вагонов в прибытие с УЗ по данным КИС
    }

}
