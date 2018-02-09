using EFRC.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFRC.Abstract
{
    public interface IRC
    {
        
        Database Database { get; }

        #region VAGON_OPERATIONS
        IQueryable<VAGON_OPERATIONS> VAGON_OPERATIONS { get; }
        IQueryable<VAGON_OPERATIONS> GetVAGON_OPERATIONS();
        VAGON_OPERATIONS GetVAGON_OPERATIONS(int id_oper);
        int SaveVAGON_OPERATIONS(VAGON_OPERATIONS VAGON_OPERATIONS);
        VAGON_OPERATIONS DeleteVAGON_OPERATIONS(int id_oper);

        int DeleteVagonsToInsertMT(int id_sostav);
        IQueryable<VAGON_OPERATIONS> GetVagonsOperationsToMTSostav(int id_sostav, DateTime dt_amkr);
        VAGON_OPERATIONS GetVagonsOperationsToMTSostav(int id_sostav, DateTime dt_amkr, int id_vagon);
        IQueryable<VAGON_OPERATIONS> GetVagonsOperations(int id_sostav); 
        IQueryable<IGrouping<int?, VAGON_OPERATIONS>> GetVagonsOperationsGroupingVagon(int id_sostav);
        IQueryable<VAGON_OPERATIONS> GetVagonsOperations(int id_sostav, int num_vag);
        int DeleteVagonsOperations(int id_sostav, int num_vag);
        int UpdateIDSostavVagonsOperations(int id_sostav, int num_vag, int new_id_sostav);
        bool IsVagonOperationMT(int id_sostav, DateTime dt_amkr, int id_vagon);
        int? MaxPositionWay(int id_way);
        int InsertVagon(int natur, DateTime dt_amkr, int id_vagon, int num_vagon, int? id_sostav, DateTime? dt_uz, int id_station, int id_way, int id_stat_kis, int? id_cond, int? id_cond2);
        int InsertVagon(int IDSostav, int id_vagon, int num_vagon, DateTime dt_uz_on, DateTime dt_uz_from, int id_station_from, int position, int? id_gruz, decimal weight_gruz, int id_station_in, int num_train, int id_cond2, int way_from);
        int UpdateVagon(DateTime dt_amkr, int num_vagon, int natur, int[] idstation_amkr, int id_gruz, int id_shop, int? id_cond);
        int UpdateVagon(int id_sostav, int num_vagon, int[] idstation_amkr, DateTime dt_amkr, int? id_cond, int natur);
        VAGON_OPERATIONS GetVagonsOfArrivalUZ(int id_mtsostav, int num, int[] idstation_uz, int idstation);
        IQueryable<VAGON_OPERATIONS> GetVagonsOfArrival(int id_mtsostav, int num, int[] idstation_uz);
        IQueryable<VAGON_OPERATIONS> GetVagonsOfStationAMKR(int id_mtsostav, int num, int[] idstation_amkr);
        IQueryable<VAGON_OPERATIONS> GetVagonsOfArrival(int[] idstation_uz);
        int DeleteVagonsOfArrival(int id_mtsostav, int num, int[] idstation_uz);
        int TakeVagonOfUZ(VAGON_OPERATIONS vagon, int natur, DateTime dt_amkr, int id_stations, int id_ways, int id_cond);
        int TakeVagonOfUZ(int id_mtsostav, int num, int[] idstation_uz, int natur, DateTime dt_amkr, int id_stations, int id_ways, int id_cond);
        int TakeVagonOfAllUZ(int id_mtsostav, int num, int[] idstation_uz, int natur, DateTime dt_amkr, int id_stations, int id_ways, int id_cond);
        IQueryable<VAGON_OPERATIONS> GetWagonsOfWay(int way);
        IQueryable<VAGON_OPERATIONS> GetWagonsOfStation(int id_stat);
        int OffSetCars(int way, int start_num);
        IQueryable<VAGON_OPERATIONS> GetVagonsAMKRToUZ(int[] idstation_uz);
        IQueryable<VAGON_OPERATIONS> GetVagonsOperationsOfPresentWay(int num_vag);
        IQueryable<VAGON_OPERATIONS> GetVagonsOperationsOfPresentArrival(int num_vag);
        #endregion

        #region VAGONS
        IQueryable<VAGONS> VAGONS { get; }
        IQueryable<VAGONS> GetVAGONS();
        VAGONS GetVAGONS(int id_vag);
        int SaveVAGONS(VAGONS VAGONS);
        VAGONS DeleteVAGONS(int id_vag);

        IQueryable<VAGONS> GetVagons(int num_vag);
        VAGONS GetVagons(int num_vag, DateTime dt);
        int? GetIDVagons(int num_vag, DateTime dt);
        VAGONS GetNewVagons(int num_vag, DateTime dt);
        int? GetIDNewVagons(int num_vag, DateTime dt);
        #endregion

        #region OWNERS
        IQueryable<OWNERS> OWNERS { get; }
        IQueryable<OWNERS> GetOWNERS();
        OWNERS GetOWNERS(int id_owner);
        int SaveOWNERS(OWNERS OWNERS);
        OWNERS DeleteOWNERS(int id_owner);

        OWNERS GetOwnersOfKis(int id_sob_kis);
        int? GetIDOwnersOfKis(int id_sob_kis);
        #endregion

        #region GRUZS
        IQueryable<GRUZS> GRUZS { get; }
        IQueryable<GRUZS> GetGRUZS();
        GRUZS GetGRUZS(int id_gruz);
        int SaveGRUZS(GRUZS GRUZS);
        GRUZS DeleteGRUZS(int id_gruz);

        GRUZS GetGruzs(int? id_gruz_prom_kis, int? id_gruz_vag_kis);
        int? GetIDGruzs(int? id_gruz_prom_kis, int? id_gruz_vag_kis);
        GRUZS GetGruzsToETSNG(int? etsng);
        int? GetIDGruzsToETSNG(int? etsng);
        #endregion

        #region STATIONS
        IQueryable<STATIONS> STATIONS { get; }
        IQueryable<STATIONS> GetSTATIONS();
        STATIONS GetSTATIONS(int id_stat);
        int SaveSTATIONS(STATIONS STATIONS);
        STATIONS DeleteSTATIONS(int id_stat);

        STATIONS GetStationsOfKis(int id_station_kis);
        int? GetIDStationsOfKis(int id_station_kis);
        IQueryable<STATIONS> GetUZStations();
        IQueryable<STATIONS> GetAMKRStations();
        List<int> GetUZStationsToID();
        List<int> GetAMKRStationsToID();
        List<int> GetListStations(IQueryable<STATIONS> stations);
        bool IsUZ(int id_stat);
        bool IsAMKR(int id_stat);
        IQueryable<STATIONS> GetStationOfListID(int[] id_statuins);
        IQueryable<STATIONS> GetStationOfNotListID(int[] id_statuins);
        #endregion

        #region WAYS

        IQueryable<WAYS> WAYS {get;}
        IQueryable<WAYS> GetWAYS();
        WAYS GetWAYS(int id_way);
        int SaveWAYS(WAYS WAYS);
        WAYS DeleteWAYS(int id_way);

        IQueryable<WAYS> GetWaysOfStations(int id_station);
        WAYS GetWaysOfStations(int id_station, string num);
        int? GetIDWaysToStations(int id_station, string num);
        int? GetIDStationOfWay(int id_way);
        #endregion

        #region SHOPS
        IQueryable<SHOPS> SHOPS {get;}
        IQueryable<SHOPS> GetSHOPS();
        SHOPS GetSHOPS(int id_shop);
        int SaveSHOPS(SHOPS SHOPS);
        SHOPS DeleteSHOPS(int id_shop);
        SHOPS GetShopsOfKis(int id_shop_kis);
        int? GetIDShopsOfKis(int id_shop_kis);
        IQueryable<SHOPS> GetShopsOfStation(int id_station);
        #endregion

        #region GRUZ_FRONTS
        IQueryable<GRUZ_FRONTS> GRUZ_FRONTS { get; }
        IQueryable<GRUZ_FRONTS> GetGRUZ_FRONTS();
        GRUZ_FRONTS GetGRUZ_FRONTS(int id_gruz_front);
        int SaveGRUZ_FRONTS(GRUZ_FRONTS GRUZ_FRONTS);
        GRUZ_FRONTS DeleteGRUZ_FRONTS(int id_gruz_front);

        IQueryable<GRUZ_FRONTS> GetGRUZ_FRONTSOfStation(int id_station);
        #endregion

    }
}
