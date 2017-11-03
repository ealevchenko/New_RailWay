using EFRW.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFRW.Abstract
{
    public interface IReference
    {
        #region ReferenceCargo
        IQueryable<ReferenceCargo> ReferenceCargo { get; }
        int SaveReferenceCargo(ReferenceCargo ReferenceCargo);
        ReferenceCargo DeleteReferenceCargo(int id);

        IQueryable<ReferenceCargo> GetReferenceCargo();
        ReferenceCargo GetReferenceCargo(int id);
        ReferenceCargo GetReferenceCargoOfCodeETSNG(int code_etsng);
        IQueryable<ReferenceCargo> GetReferenceCargoOfCodeETSNG(int code_etsng_start, int code_etsng_stop);
        int GetCorectReferenceCargo(int code_etsng);

        #endregion

        #region ReferenceTypeCargo
        IQueryable<ReferenceTypeCargo> ReferenceTypeCargo { get; }
        int SaveReferenceTypeCargo(ReferenceTypeCargo ReferenceTypeCargo);
        ReferenceTypeCargo DeleteReferenceTypeCargo(int id);
        #endregion
    }
}
