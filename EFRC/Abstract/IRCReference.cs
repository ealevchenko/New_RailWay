using EFRC.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFRC.Abstract
{
    public interface IRCReference
    {
        #region ReferenceCargo
        IQueryable<ReferenceCargo> ReferenceCargo { get; }
        int SaveReferenceCargo(ReferenceCargo ReferenceCargo);
        ReferenceCargo DeleteReferenceCargo(int IDCargo);

        IQueryable<TypeCargo> TypeCargo { get; }
        int SaveTypeCargo(TypeCargo TypeCargo);
        TypeCargo DeleteTypeCargo(int ID);
        #endregion

        #region ReferenceCountry
        IQueryable<ReferenceCountry> ReferenceCountry { get; }
        int SaveReferenceCountry(ReferenceCountry ReferenceCountry);
        ReferenceCountry DeleteReferenceCountry(int IDCountry);
        #endregion

        #region ReferenceStation
        IQueryable<ReferenceStation> ReferenceStation { get; }
        int SaveReferenceStation(ReferenceStation ReferenceStation);
        ReferenceStation DeleteReferenceStation(int IDStation);
        #endregion
    }
}
