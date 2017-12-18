using EFMT.Concrete;
using EFMT.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMT.Abstract
{
    public interface IMT
    {
        #region На подходах
        IQueryable<ApproachesCars> ApproachesCars { get; }
        IQueryable<ApproachesCars> GetApproachesCars();
        ApproachesCars GetApproachesCars(int id);
        int SaveApproachesCars(ApproachesCars ApproachesCars);
        ApproachesCars DeleteApproachesCars(int id);
        int DeleteApproachesCarsOfSostav(int id_sostav);

        IQueryable<ApproachesCars> GetApproachesCarsOfSostav(int idsostav);
        IQueryable<ApproachesCars> GetApproachesCarsOfNumCar(int num_car, bool order);
        //bool IsApproachesCarsOfSostav(int idsostav);
        //int SetArrivalApproachesCars(int num, int idsostav, DateTime dt_operation);
        //int CloseApproachesSostav();
        int CloseApproachesCarsOfDocWeight(int doc, int num, DateTime dt, decimal? weight);
        int CloseApproachesCarsOfDocDay(int doc, int num, DateTime dt, int day);
        List<DateTime> GroupDateApproachesCars();

        IQueryable<ApproachesSostav> ApproachesSostav { get; }
        IQueryable<ApproachesSostav> GetApproachesSostav();
        ApproachesSostav GetApproachesSostav(int id);
        int SaveApproachesSostav(ApproachesSostav ApproachesSostav);
        ApproachesSostav DeleteApproachesSostav(int id);

        //ApproachesSostav GetApproachesSostavOfParentID(int parent_id);
        //IQueryable<ApproachesSostav> GetApproachesSostavOfIndex(string index, bool start);
        ApproachesSostav GetApproachesSostavOfFile(string file);
        ApproachesSostav GetNoCloseApproachesSostav(string index, DateTime date);
        ApproachesSostav GetApproachesSostavOfParentID(int parent_id);
        List<ApproachesSostav> GetApproachesSostavLocation(int id_sostav, bool destinct);
        //IQueryable<ApproachesSostav> GetNoCloseApproachesSostav();
        //List<ApproachesSostav> GetApproachesSostavLocation(int id_sostav, bool destinct);
        ////ApproachesSostavLocation GetApproachesSostavLocation(int id);
        //int SetApproachesCars(int doc, int num, DateTime dt, int day);



        //List<GroupTypeCargoApproaches> GetCargoApproachesOfGroup(int? group);
        //List<GroupTypeStationCurrentCargoApproaches> GetCargoApproachesOfType(int? type);
        //List<GroupTypeOwnerCargoApproaches> GetCargoApproachesOfTypeStationCurrent(int? type, int stationcurrent);
        #endregion

        #region На станциях УЗ КР
        IQueryable<ArrivalCars> ArrivalCars { get; }
        IQueryable<ArrivalCars> GetArrivalCars();
        ArrivalCars GetArrivalCars(int id);
        int SaveArrivalCars(ArrivalCars ArrivalCars);
        ArrivalCars DeleteArrivalCars(int id);
        int DeleteArrivalCarsOfSostav(int id_sostav);

        IQueryable<ArrivalCars> GetArrivalCarsOfSostav(int id_sostav);
        IQueryable<ArrivalCars> GetArrivalCarsOfNumCar(int num_car, bool order);
        //bool IsArrivalCarsOfSostav(int idsostav);
        List<ArrivalCars> GetArrivalCarsOfConsignees(int id_sostav, int[] Consignees);
        List<ArrivalCars> GetArrivalCarsOfConsignees(int[] Consignees);
        ArrivalCars GetArrivalCarsToNatur(int natur, int num_wag, DateTime dt, int day);
        int CloseArrivalCarsOfDocWeight(int doc, int num, DateTime dt, decimal? weight);
        int CloseArrivalCarsOfDocDay(int doc, int num, DateTime dt, int day);
        IQueryable<ArrivalCars> GetArrivalCars(int num, DateTime dt);
        int CloseArrivalCars(int num, DateTime dt, int code_close);


        IQueryable<ArrivalSostav> ArrivalSostav { get; }
        IQueryable<ArrivalSostav> GetArrivalSostav();
        ArrivalSostav GetArrivalSostav(int id);
        int SaveArrivalSostav(ArrivalSostav ArrivalSostav);
        ArrivalSostav DeleteArrivalSostav(int id);

        //ArrivalSostav GetArrivalSostavOfParentID(int parent_id);
        //IQueryable<ArrivalSostav> GetArrivalSostavOfIndex(string index, bool start);
        ArrivalSostav GetArrivalSostavOfFile(string file);
        ArrivalSostav GetNoCloseArrivalSostav(string index, DateTime date);
        IQueryable<ArrivalSostav> GetArrivalSostavOfIDArrival(int id_arrival, bool order);
        ArrivalSostav GetFirstArrivalSostavOfIDArrival(int id_arrival);
        //int SetArrivalCars(int doc, int num, DateTime dt, int day);
        //IQueryable<ArrivalSostav> GetStartArrivalSostav();
        //IQueryable<ArrivalSostav> GetStartArrivalSostav(DateTime start, DateTime stop);
        int GetNextIDArrival();
        //int GetIDArrival(int id);
        //int CloseArrivalSostav();
        //List<ArrivalSostav> GetOperationArrivalSostav(int id_sostav, bool destinct);

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
    }
}
