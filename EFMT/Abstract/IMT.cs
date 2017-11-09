using MT.Entities;
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

        IQueryable<ApproachesSostav> ApproachesSostav { get; }
        IQueryable<ApproachesSostav> GetApproachesSostav();
        ApproachesSostav GetApproachesSostav(int id);
        int SaveApproachesSostav(ApproachesSostav ApproachesSostav);
        ApproachesSostav DeleteApproachesSostav(int id);

        //ApproachesSostav GetApproachesSostavOfParentID(int parent_id);
        //IQueryable<ApproachesSostav> GetApproachesSostavOfIndex(string index, bool start);
        ApproachesSostav GetApproachesSostavOfFile(string file);
        ApproachesSostav GetNoCloseApproachesSostav(string index, DateTime date);
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
        //List<ArrivalCars> GetArrivalCarsOfConsignees(int id_sostav, int[] Consignees);



        IQueryable<ArrivalSostav> ArrivalSostav { get; }
        IQueryable<ArrivalSostav> GetArrivalSostav();
        ArrivalSostav GetArrivalSostav(int id);
        int SaveArrivalSostav(ArrivalSostav ArrivalSostav);
        ArrivalSostav DeleteArrivalSostav(int id);

        //ArrivalSostav GetArrivalSostavOfParentID(int parent_id);
        //IQueryable<ArrivalSostav> GetArrivalSostavOfIndex(string index, bool start);
        ArrivalSostav GetArrivalSostavOfFile(string file);
        ArrivalSostav GetNoCloseArrivalSostav(string index, DateTime date);
        //int SetArrivalCars(int doc, int num, DateTime dt, int day);
        //IQueryable<ArrivalSostav> GetStartArrivalSostav();
        //IQueryable<ArrivalSostav> GetStartArrivalSostav(DateTime start, DateTime stop);
        int GetNextIDArrival();
        //int GetIDArrival(int id);
        //int CloseArrivalSostav();
        //List<ArrivalSostav> GetOperationArrivalSostav(int id_sostav, bool destinct);

        //IQueryable<Consignee> Consignee { get; }
        //int SaveConsignee(Consignee Consignee);
        //Consignee DeleteConsignee(int Code);
        //IQueryable<Consignee> GetConsignee();
        //Consignee GetConsignee(int Code);
        //IQueryable<Consignee> GetConsignee(mtConsignee tmtc);
        //bool IsConsignee(int Code, mtConsignee type);
        //int[] GetConsigneeToCodes(mtConsignee tmtc);
        #endregion
    }
}
