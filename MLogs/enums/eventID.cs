using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageLog
{
    public enum eventID : int
    {
        Null = -1,

        RailWay = 0,
        // Общие модули 1000
        RWSettings_RWSetting = 1100,
        RWSettings_RWDBSetting = 1110,    
    
        Sockets_EFSockets = 1200,
        Sockets_RWSocketBase = 1210,
        Sockets_RWSocketServer = 1220,
        Sockets_RWSocketClient = 1230,
        
        // Новые сервисы работающие как отдельные потоки
        RWService_MT_Transfer = 2000, // new
        RWService_KIS_Transfer = 3000, // new
        RWService_RailWay = 4000, // new

        EFMetallurgTrans = 2100, // new
        EFMetallurgTrans_EFMTApproaches = 2110,
        EFMetallurgTrans_EFMTArrival = 2120,

    }
}
