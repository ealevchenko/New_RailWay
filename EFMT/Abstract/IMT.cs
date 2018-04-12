using EFMT.Concrete;
using EFMT.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMT.Abstract
{
    public interface IMT
    {
        Database Database { get; }
        
        #region На подходах
        IQueryable<ApproachesCars> ApproachesCars { get; }
        IQueryable<ApproachesCars> GetApproachesCars();
        ApproachesCars GetApproachesCars(int id);
        int SaveApproachesCars(ApproachesCars ApproachesCars);
        ApproachesCars DeleteApproachesCars(int id);
        int DeleteApproachesCarsOfSostav(int id_sostav);

        IQueryable<ApproachesCars> GetApproachesCarsOfSostav(int idsostav);
        IQueryable<ApproachesCars> GetApproachesCarsOfNumCar(int num_car, bool order);
        int CloseApproachesCarsOfDocWeight(int doc, int num, DateTime dt, decimal? weight);
        int CloseApproachesCarsOfDocDay(int doc, int num, DateTime dt, int day);
        List<DateTime> GroupDateApproachesCars();
        IQueryable<ApproachesCars> GetNoCloseApproachesCars();
        ApproachesCars GetApproachesCarsOfNextCar(int num_car, DateTime operation);

        IQueryable<ApproachesSostav> ApproachesSostav { get; }
        IQueryable<ApproachesSostav> GetApproachesSostav();
        ApproachesSostav GetApproachesSostav(int id);
        int SaveApproachesSostav(ApproachesSostav ApproachesSostav);
        ApproachesSostav DeleteApproachesSostav(int id);


        ApproachesSostav GetApproachesSostavOfFile(string file);
        ApproachesSostav GetNoCloseApproachesSostav(string index, DateTime date);
        ApproachesSostav GetApproachesSostavOfParentID(int parent_id);
        List<ApproachesSostav> GetApproachesSostavLocation(int id_sostav, bool destinct);
        #endregion

        #region На станциях УЗ КР
        IQueryable<ArrivalCars> ArrivalCars { get; }
        IQueryable<ArrivalCars> GetArrivalCars();
        ArrivalCars GetArrivalCars(int id);
        int SaveArrivalCars(ArrivalCars ArrivalCars);
        ArrivalCars DeleteArrivalCars(int id);
        int DeleteArrivalCarsOfSostav(int id_sostav);

        IQueryable<ArrivalCars> GetArrivalCarsOfSostav(int id_sostav);
        ArrivalCars GetArrivalCarsOfSostavNum(int id_sostav, int num);
        IQueryable<ArrivalCars> GetArrivalCarsOfNumCar(int num_car, bool order);
        List<ArrivalCars> GetArrivalCarsOfConsignees(int id_sostav, int[] Consignees);
        List<ArrivalCars> GetArrivalCarsOfConsignees(int[] Consignees);
        ArrivalCars GetArrivalCarsToNatur(int natur, int num_wag, DateTime dt, int day);
        int CloseArrivalCarsOfDocWeight(int doc, int num, DateTime dt, decimal? weight);
        int CloseArrivalCarsOfDocDay(int doc, int num, DateTime dt, int day);
        IQueryable<ArrivalCars> GetArrivalCars(int num, DateTime dt);
        ArrivalCars GetArrivalCarsOfNextCar(int num, DateTime dt);
        int CloseArrivalCars(int num, DateTime dt, int code_close);
        int CloseArrivalCars(int id_sostav, int num, int doc, DateTime dt);
        IQueryable<ArrivalSostav> ArrivalSostav { get; }
        IQueryable<ArrivalSostav> GetArrivalSostav();
        ArrivalSostav GetArrivalSostav(int id);
        int SaveArrivalSostav(ArrivalSostav ArrivalSostav);
        ArrivalSostav DeleteArrivalSostav(int id);


        ArrivalSostav GetArrivalSostavOfFile(string file);
        ArrivalSostav GetNoCloseArrivalSostav(string index, DateTime date);
        ArrivalSostav GetNoCloseArrivalSostav(string index, DateTime date, int period);
        IQueryable<ArrivalSostav> GetArrivalSostavOfIDArrival(int id_arrival, bool order);
        ArrivalSostav GetFirstArrivalSostavOfIDArrival(int id_arrival);
        IQueryable<ArrivalSostav> GetArrivalSostavOfIDArrival(int id_arrival);
        List<int> GetNotCarsOfOldArrivalSostav(int id_sostav);
        List<int> GetNotCarsOfOldArrivalSostav(ArrivalSostav sostav);
        int GetNextIDArrival();


        #endregion

        #region Грузополучатели
        IQueryable<Consignee> Consignee { get; }
        IQueryable<Consignee> GetConsignee();
        Consignee GetConsignee(int code);
        int SaveConsignee(Consignee Consignee);
        Consignee DeleteConsignee(int code);

        IQueryable<Consignee> GetConsignee(bool send, mtConsignee Consignee);
        bool IsConsigneeSend(bool send, int code, mtConsignee Consignee);
        bool IsConsignee(int Code, mtConsignee Consignee);
        int[] GetListCodeConsigneeOfConsignee(mtConsignee Consignee);
        IQueryable<Consignee> GetConsignee(mtConsignee tmtc);
        int[] GetConsigneeToCodes(mtConsignee tmtc);

        //IQueryable<Consignee> Consignee { get; }
        //int SaveConsignee(Consignee Consignee);
        //Consignee DeleteConsignee(int Code);
        //IQueryable<Consignee> GetConsignee();
        //Consignee GetConsignee(int Code);
        //bool IsConsignee(int Code, mtConsignee type);
        #endregion

        #region WagonsTracking
        IQueryable<WagonsTracking> WagonsTracking { get; }
        IQueryable<WagonsTracking> GetWagonsTracking();
        WagonsTracking GetWagonsTracking(int id);
        int SaveWagonsTracking(WagonsTracking WagonsTracking);
        WagonsTracking DeleteWagonsTracking(int id);

        IQueryable<WagonsTracking> GetWagonsTrackingOfNumCars(int num);
        #endregion

        #region ListWagonsTracking
        IQueryable<ListWagonsTracking> ListWagonsTracking { get; }
        IQueryable<ListWagonsTracking> GetListWagonsTracking();
        ListWagonsTracking GetListWagonsTracking(int nvagon);
        int SaveListWagonsTracking(ListWagonsTracking ListWagonsTracking);
        ListWagonsTracking DeleteListWagonsTracking(int nvagon);
        #endregion

        #region WTReports
        IQueryable<WTReports> WTReports { get; }
        IQueryable<WTReports> GetWTReports();
        WTReports GetWTReports(int id);
        int SaveWTReports(WTReports WTReports);
        WTReports DeleteWTReports(int id);
        #endregion

        #region WTCarsReports
        IQueryable<WTCarsReports> WTCarsReports { get; }
        IQueryable<WTCarsReports> GetWTCarsReports();
        WTCarsReports GetWTCarsReports(int id);
        int SaveWTCarsReports(WTCarsReports WTCarsReports);
        WTCarsReports DeleteWTCarsReports(int id);
        #endregion

        #region WTCycle
        IQueryable<WTCycle> WTCycle { get; }
        IQueryable<WTCycle> GetWTCycle();
        WTCycle GetWTCycle(int id);
        IQueryable<WTCycle> GetWTCycleOfNumCar(int num);
        int SaveWTCycle(WTCycle WTCycle);
        WTCycle DeleteWTCycle(int id);
        #endregion

        #region CycleWagonsTracking
            List<CycleWagonsTracking> GetCycleWagonsTrackingOfReports(int id_report, DateTime start, DateTime stop);
        #endregion

        #region RouteWagonTracking
            List<RouteWagonTracking> GetRouteWagonTrackingOfReports(int id_report, DateTime start, DateTime stop);
            List<RouteWagonTracking> GetLastRouteWagonTrackingOfReports(int id_report, DateTime start, DateTime stop);
            List<CurentWagonTracking> GetLastWagonTrackingOfReports(int id_report, DateTime start, DateTime stop);
        #endregion

        #region CountCarsOfSostav
        List<CountCarsOfSostav> GetNoCloseArrivalCarsOfStationUZ(int code);
        List<CountCarsOfSostav> GetArrivalCarsOfStationUZ(int code, bool close);

        #endregion
    }
}
