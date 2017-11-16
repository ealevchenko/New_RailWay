using EFKIS.Concrete;
using EFKIS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFKIS.Abstract
{
    public interface IKIS
    {
        #region KOMETA

        #region KometaVagonSob

        IQueryable<KometaVagonSob> KometaVagonSob { get; }
        IQueryable<KometaVagonSob> GetVagonsSob();
        IQueryable<KometaVagonSob> GetVagonsSob(int num);
        KometaVagonSob GetVagonsSob(int num, DateTime dt);
        IQueryable<KometaVagonSob> GetChangeVagonsSob(DateTime dt, int day_period);
        IQueryable<KometaVagonSob> GetChangeVagonsSob(int day_period);
        List<rwCar> GetVagons();
        #endregion

        #region KometaSobstvForNakl
        IQueryable<KometaSobstvForNakl> GetSobstvForNakl();
        KometaSobstvForNakl GetSobstvForNakl(int kod_sob);
        #endregion

        #endregion

        #region PROM

        #region PROM.GRUZ_SP
        IQueryable<PromGruzSP> GetGruzSP();
        PromGruzSP GetGruzSP(int k_gruz); 
        PromGruzSP GetGruzSPToTarGR(int? tar_gr, bool corect);
        #endregion
        #endregion

        #region NUM_VAG (Информация по вагонам)

        #region NumVagStpr1Gr (Справочник грузов по вагонам)
        IQueryable<NumVagStpr1Gr> GetSTPR1GR();
        NumVagStpr1Gr GetSTPR1GR(int kod_gr);
        #endregion
        #endregion
    }
}
