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
        RWWebAPI = 1200,
        RWWebAPI_ClientWebAPI = 1210,
        RWWebAPI_RWReference = 1211,
        //RWSettings_RWDBSetting = 1110,    
    
        //Sockets_EFSockets = 1200,
        //Sockets_RWSocketBase = 1210,
        //Sockets_RWSocketServer = 1220,
        //Sockets_RWSocketClient = 1230,
        
        //// Новые сервисы работающие как отдельные потоки
        //RWService_MT_Transfer = 2000, // new
        //RWService_KIS_Transfer = 3000, // new
        //RWService_RailWay = 4000, // new

        EFMetallurgTrans = 2100, // Библиотека доступа к данным БД MT
        //EFMetallurgTrans_EFMTApproaches = 2110,
        //EFMetallurgTrans_EFMTArrival = 2120,

        MetallurgTrans = 3000, // Библиотека сервисов обработки данных МеталлургТранс
        MetallurgTrans_MTTransfer = 3100, // Модуль переноса данных МеталлургТранс

        EFRW = 4000, // Библиотека доступа к данным БД RailWay
        EFRW_EFReference = 4100, // Модуль доступа к БД справочников системы RailWay 
    }
}
