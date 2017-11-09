using EFReference.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFReference.Abstract
{
    public interface IReference
    {
        #region Cargo
        IQueryable<Cargo> Cargo { get; }
        IQueryable<Cargo> GetCargo();
        Cargo GetCargo(int id);
        Cargo DeleteCargo(int id);
        Cargo GetCargoOfCodeETSNG(int code_etsng);
        IQueryable<Cargo> GetCargoOfCodeETSNG(int code_etsng_start, int code_etsng_stop);Cargo GetCorrectCargo(int code_etsng);
        int GetCodeCorrectCargo(int code_etsng);
        #endregion

    }
}
