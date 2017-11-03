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
        IQueryable<ApproachesCars> ApproachesCars { get; }
        IQueryable<ApproachesCars> GetApproachesCars();
        ApproachesCars GetApproachesCars(int id);
        int SaveApproachesCars(ApproachesCars ApproachesCars);
        ApproachesCars DeleteApproachesCars(int id);
        int DeleteApproachesCarsOfSostav(int id_sostav);

        IQueryable<ApproachesCars> GetApproachesCarsOfSostav(int idsostav);
        //IQueryable<ApproachesCars> GetApproachesCarsOfNumCar(int num);
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



    }
}
