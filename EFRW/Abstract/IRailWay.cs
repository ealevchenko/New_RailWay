using EFRW.Concrete;
using EFRW.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFRW.Abstract
{
    public interface IRailWay
    {
        List<Option> GetTypeSendTransfer();
        string GetTypeSendTransfer(int type);
        List<Option> GetSide();
        string GetSide(int side);

        #region Stations
        IQueryable<Stations> Stations { get; }
        IQueryable<Stations> GetStations();
        Stations GetStations(int id);
        int SaveStations(Stations Stations);
        Stations DeleteStations(int id);

        IQueryable<Stations> GetStationsOfSelect(bool view, bool uz);
        IQueryable<Stations> GetStationsOfViewAMKR();

        Stations GetStationsOfKis(int id_kis);

        #endregion

        #region StationsNodes
        IQueryable<StationsNodes> StationsNodes { get; }
        IQueryable<StationsNodes> GetStationsNodes();
        StationsNodes GetStationsNodes(int id);
        int SaveStationsNodes(StationsNodes StationsNodes);
        StationsNodes DeleteStationsNodes(int id);

        IQueryable<StationsNodes> GetSendStationsNodes(int id_station);
        IQueryable<StationsNodes> GetArrivalStationsNodes(int id_station);

        IQueryable<StationsNodes> GetStationsNodes(int id_station_from, int id_station_on, EFRW.Concrete.EFRailWay.typeSendTransfer st);
        bool IsRulesTransfer(int id_station_from, int id_station_on, EFRW.Concrete.EFRailWay.typeSendTransfer st);
        bool IsRulesTransferOfKis(int id_station_from_kis, int id_station_on_kis, EFRW.Concrete.EFRailWay.typeSendTransfer st);

        #endregion
    }
}
