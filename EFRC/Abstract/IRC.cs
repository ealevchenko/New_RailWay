using EFRC.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFRC.Abstract
{
    public interface IRC
    {
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
        #endregion

        #region VAGON_OPERATIONS
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
    }
}
