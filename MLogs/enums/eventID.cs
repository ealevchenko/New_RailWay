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

        #region Вспомогательные общие модуля 1000

        RWSettings_RWSetting = 1100, // Библиотека доступа к настройкам сервисов БД, *.config файлы

        RWWebAPI = 1200,                // Библиотеки получения данных с WebApi RailWay
        RWWebAPI_ClientWebAPI = 1201,   // Библиотека клиент WebApi RailWay
        RWWebAPI_RWReference = 1202,    // Библиотека получения данных справочников ситемы RailWay
        RWWebAPI_Reference = 1203,      // Библиотека получения данных общих справочников железных дорог
        #endregion

        //RWSettings_RWDBSetting = 1110,    
    
        //Sockets_EFSockets = 1200,
        //Sockets_RWSocketBase = 1210,
        //Sockets_RWSocketServer = 1220,
        //Sockets_RWSocketClient = 1230,
        
        //// Новые сервисы работающие как отдельные потоки
        //RWService_MT_Transfer = 2000, // new
        //RWService_KIS_Transfer = 3000, // new
        //RWService_RailWay = 4000, // new

        #region Служба МеталлургТранс 2000
        MTTServices = 2000,                 // Служба
        
        EFMetallurgTrans = 2100,            // Библиотека доступа к данным БД MT

        MetallurgTrans = 2200,              // Библиотека сервисов обработки данных МеталлургТранс
        MetallurgTrans_MTTransfer = 2201,   // Модуль переноса данных МеталлургТранс
        MetallurgTrans_SFTPClient = 2202,   // Модуль доступа к SFTP серверам (данных МеталлургТранс)
        MetallurgTrans_MTThread = 2202,     // Модуль потоков переноса данных
        #endregion

        #region Служба RailCars 3000
        EFRailCars = 3100,                  // Библиотека доступа к данным БД системы RailCars
        RCReference = 3200,                 // Библиотека доступа к справочникам системы RailCars
        EFSAP = 3300,                       // Библиотека доступа к справочникам обновляемых SAP
        EFRCReference = 3400,               // Библиотека доступа к справочникам системы RailCars+
        #endregion

        #region Служба RailWay 4000

        EFRW = 4100,                        // Библиотека доступа к данным БД RailWay
        EFRW_EFReference = 4101,            // Модуль доступа к БД справочников системы RailWay 
        #endregion

        #region Служба общей справочной системы 5000
        EFReference = 5100,                 // Библиотека доступа к данным обшим справочникам ЖД

        #endregion

        #region Служба системы КИС 6000

        EFWagons = 6100,                        // Библиотека доступа к данным БД системы КИС
        EFTKIS = 6200,                          // Библиотека доступа к таблицам переноса данных из системы КИС в систему RailCars, RailWay 
        #endregion

        #region Модуля согласования старой ситемы Railcars c новой Railway 10000
        OLDVersion = 10000,                 // Библиотеки старой системы RailCars
        OLDVersion_TRailCars = 10100,       // Библиотека переноса вагонов в старую систему RailCars
        #endregion


    }
}
