using EFRW.Entities1;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFRW.Abstract
{
    public interface IReference
    {
        Database Database { get; }

        #region ReferenceCargo
        IQueryable<ReferenceCargo> ReferenceCargo { get; }
        int SaveReferenceCargo(ReferenceCargo ReferenceCargo);
        ReferenceCargo DeleteReferenceCargo(int id);

        IQueryable<ReferenceCargo> GetReferenceCargo();
        ReferenceCargo GetReferenceCargo(int id);
        ReferenceCargo GetReferenceCargoOfCodeETSNG(int code_etsng);
        IQueryable<ReferenceCargo> GetReferenceCargoOfCodeETSNG(int code_etsng_start, int code_etsng_stop);
        ReferenceCargo GetCorectReferenceCargo(int code_etsng);
        int GetCodeCorectReferenceCargo(int code_etsng);

        #endregion

        #region ReferenceTypeCargo
        IQueryable<ReferenceTypeCargo> ReferenceTypeCargo { get; }
        int SaveReferenceTypeCargo(ReferenceTypeCargo ReferenceTypeCargo);
        ReferenceTypeCargo DeleteReferenceTypeCargo(int id);
        #endregion

        #region ReferenceGroupCargo
        IQueryable<ReferenceGroupCargo> ReferenceGroupCargo { get; }
        int SaveReferenceGroupCargo(ReferenceGroupCargo ReferenceGroupCargo);
        ReferenceGroupCargo DeleteReferenceGroupCargo(int id);
        #endregion

        #region ReferenceCars
        IQueryable<ReferenceCars> ReferenceCars { get; }
        IQueryable<ReferenceCars> GetReferenceCars();
        ReferenceCars GetReferenceCars(int num);
        int SaveReferenceCars(ReferenceCars ReferenceCars);
        ReferenceCars DeleteReferenceCars(int num);
        #endregion

        #region ReferenceTypeCars
        IQueryable<ReferenceTypeCars> ReferenceTypeCars { get; }
        IQueryable<ReferenceTypeCars> GetReferenceTypeCars();
        ReferenceTypeCars GetReferenceTypeCars(int id);
        int SaveReferenceTypeCars(ReferenceTypeCars ReferenceTypeCars);
        ReferenceTypeCars DeleteReferenceTypeCars(int id);
        ReferenceTypeCars GetReferenceTypeCarsOfAbr(string abr);
        #endregion

        #region ReferenceGroupCars
        IQueryable<ReferenceGroupCars> ReferenceGroupCars { get; }
        IQueryable<ReferenceGroupCars> GetReferenceGroupCars();
        ReferenceGroupCars GetReferenceGroupCars(int id);
        int SaveReferenceGroupCars(ReferenceGroupCars ReferenceGroupCars);
        ReferenceGroupCars DeleteReferenceGroupCars(int id);
        #endregion

        #region ReferenceCountry
        IQueryable<ReferenceCountry> ReferenceCountry { get; }
        IQueryable<ReferenceCountry> GetReferenceCountry();
        ReferenceCountry GetReferenceCountry(int id);
        int SaveReferenceCountry(ReferenceCountry ReferenceCountry);
        ReferenceCountry DeleteReferenceCountry(int id);
        ReferenceCountry GetReferenceCountryOfCode(int code);
        #endregion

        #region ReferenceStation
        IQueryable<ReferenceStation> ReferenceStation { get; }
        IQueryable<ReferenceStation> GetReferenceStation();
        ReferenceStation GetReferenceStation(int id);
        int SaveReferenceStation(ReferenceStation ReferenceStation);
        ReferenceStation DeleteReferenceStation(int id);
        ReferenceStation GetReferenceStationOfCodecs(int codecs);
        #endregion

        #region ReferenceOwners
        IQueryable<ReferenceOwners> ReferenceOwners { get; }
        IQueryable<ReferenceOwners> GetReferenceOwners();
        ReferenceOwners GetReferenceOwners(int id);
        int SaveReferenceOwners(ReferenceOwners ReferenceOwners);
        ReferenceOwners DeleteReferenceOwners(int id);
        ReferenceOwners GetReferenceOwnersOfKIS(int id_kis);
        #endregion

        #region ReferenceOwnerCars
        IQueryable<ReferenceOwnerCars> ReferenceOwnerCars { get; }
        IQueryable<ReferenceOwnerCars> GetReferenceOwnerCars();
        ReferenceOwnerCars GetReferenceOwnerCars(int id);
        int SaveReferenceOwnerCars(ReferenceOwnerCars ReferenceOwnerCars);
        ReferenceOwnerCars DeleteReferenceOwnerCars(int id);
        int DeleteReferenceOwnerCarsOfNum(int num);
        #endregion

        #region ReferenceConsignee
        IQueryable<ReferenceConsignee> ReferenceConsignee { get; }
        IQueryable<ReferenceConsignee> GetReferenceConsignee();
        ReferenceConsignee GetReferenceConsignee(int id);
        ReferenceConsignee GetReferenceConsigneeOfKis(int id_kis);
        int SaveReferenceConsignee(ReferenceConsignee ReferenceConsignee);
        ReferenceConsignee DeleteReferenceConsignee(int id);
        #endregion
    }
}
