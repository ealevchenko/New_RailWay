using EFRW.Concrete;
using EFRW.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFRW.Abstract
{
    public interface IRailWay
    {
        Database Database { get; }

        void RefreshAll();
        
        List<Option> GetTypeSendTransfer();
        string GetTypeSendTransfer(int type);
        List<Option> GetSide();
        string GetSide(int side);

        #region Stations
        IQueryable<Stations> Stations { get; }
        IQueryable<Stations> GetStations();
        IQueryable<Stations> GetStations(bool link);
        Stations GetStations(int id);
        Stations GetStations(int id, bool link);
        int SaveStations(Stations Stations);
        Stations DeleteStations(int id);

        IQueryable<Stations> GetStationsOfSelect(bool view, bool uz);
        IQueryable<Stations> GetStationsOfViewAMKR();

        Stations GetStationsOfKis(int id_kis);
        Stations GetStationsOfCodeUZ(int code_uz);

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

        #region Ways
        IQueryable<Ways> Ways { get; }
        IQueryable<Ways> GetWays();
        Ways GetWays(int id);
        int SaveWays(Ways Ways);
        Ways DeleteWays(int id);
        IQueryable<Ways> GetWaysOfStation(int id_station);
        Ways GetWaysOfStation(int id_station, string num);
        Ways GetWaysOfArrivalUZ(int id_station);
        Ways GetWaysOfSendingUZ(int id_station);
        IQueryable<Ways> GetWaysOfStationAMKR();
        IQueryable<Ways> GetWaysOfStationUZ();
        #endregion

        #region Shops
        IQueryable<Shops> Shops { get; }
        IQueryable<Shops> GetShops();
        Shops GetShops(int id);
        Shops GetShopsOfKis(int id_kis);
        int SaveShops(Shops Shops);
        Shops DeleteShops(int id);
        #endregion

        #region Deadlock
        IQueryable<Deadlock> Deadlock { get; }
        IQueryable<Deadlock> GetDeadlock();
        Deadlock GetDeadlock(int id);
        int SaveDeadlock(Deadlock Deadlock);
        Deadlock DeleteDeadlock(int id);
        #endregion


        #region Cars
        IQueryable<Cars> Cars { get; }
        IQueryable<Cars> GetCars();
        Cars GetCars(int id);
        int SaveCars(Cars Cars);
        //int SaveCars(List<Cars> Cars);
        Cars DeleteCars(int id);
        IQueryable<Cars> GetCarsOfSostav(int id_sostav);
        IQueryable<Cars> GetCarsOfArrival(int id_arrival);
        IQueryable<Cars> GetCarsOfNum(int num);
        Cars GetLastCarsOfNum(int num);
        Cars GetCarsOfSostavNum(int id_sostav, int num);
        Cars GetCarsOfArrivalNum(int id_arrival, int num);
        List<Cars> GetCarsOfArrivalNum(int id_arrival, int[] nums);
        Cars GetCarsOfSetKIS(int num, DateTime dt_amkr, int natur);
        Cars GetCarsOfSetKIS(int num, DateTime dt_amkr, int natur, int hour_interval);
        Cars GetCarsOfSetKIS(int num, int natur);
        Cars GetCarsOfNumDT(int num, DateTime dt_amkr, int hour_interval);
        Cars GetCarsOfParentID(int parent_id);
        IQueryable<Cars> GetFirstCarsOfNum(int num);
        List<CarsHistory> GetHistoryCarsOfNum(int num);
        CarsHistory CreateCarsHistory(Cars Cars, int position);
        #endregion

        #region CarOperations
        IQueryable<CarOperations> CarOperations { get; }
        IQueryable<CarOperations> GetCarOperations();
        CarOperations GetCarOperations(int id);
        int SaveCarOperations(CarOperations CarOperations);
        CarOperations DeleteCarOperations(int id);
        int DeleteCarOperationsOfCars(int id_car);
        IQueryable<CarOperations> GetCarOperationsOfNumCar(int num);
        CarOperations GetLastCarOperationsOfNumCar(int num);
        IQueryable<CarOperations> GetOpenCarOperationsOfWay(int id_way);
        int GetLastPositionOpenCarOperationsOfWay(int id_way);

        #endregion

        #region CarConditions
        IQueryable<CarConditions> CarConditions { get; }
        IQueryable<CarConditions> GetCarConditions();
        CarConditions GetCarConditions(int id);
        int SaveCarConditions(CarConditions CarConditions);
        CarConditions DeleteCarConditions(int id);
        #endregion

        #region CarStatus
        IQueryable<CarStatus> CarStatus { get; }
        IQueryable<CarStatus> GetCarStatus();
        CarStatus GetCarStatus(int id);
        int SaveCarStatus(CarStatus CarStatus);
        CarStatus DeleteCarStatus(int id);
        #endregion

        #region CarsInpDelivery
        IQueryable<CarsInpDelivery> CarsInpDelivery { get; }
        IQueryable<CarsInpDelivery> GetCarsInpDelivery();
        CarsInpDelivery GetCarsInpDelivery(int id);
        int SaveCarsInpDelivery(CarsInpDelivery CarsInpDelivery);
        CarsInpDelivery DeleteCarsInpDelivery(int id);
        int DeleteCarsInpDeliveryOfCars(int id_car);
        CarsInpDelivery GetCarsInpDeliveryOfCar(int id_car);
        CarsInpDelivery GetCarsInpDeliveryOfNumArrival(int num, int id_arrival);
        int GetDefaultIDArrival();
        #endregion

        #region CarsOutDelivery
        IQueryable<CarsOutDelivery> CarsOutDelivery { get; }
        IQueryable<CarsOutDelivery> GetCarsOutDelivery();
        CarsOutDelivery GetCarsOutDelivery(int id);
        int SaveCarsOutDelivery(CarsOutDelivery CarsOutDelivery);
        CarsOutDelivery DeleteCarsOutDelivery(int id);
        int DeleteCarsOutDeliveryOfCars(int id_car);
        #endregion
    }
}
