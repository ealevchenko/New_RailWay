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
        // сервисы МеталлургТранса
        ServicesMT = 100,
        TransferApproaches = 111,
        TransferArrival = 112,        
        TransferMT = 121,
        HostMT = 122,
        CloseTransfer = 131,
    }

}
