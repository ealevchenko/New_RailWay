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

        #region KometaParkState
        IQueryable<KometaParkState> KometaParkState { get; }
        IQueryable<KometaParkState> GetKometaParkState();
        IQueryable<KometaParkState> GetKometaParkState(DateTime Date);
        List<IGrouping<string, KometaParkState>> GetKometaParkStateToStation(DateTime Date);
        IQueryable<KometaParkState> GetKometaParkState(DateTime Date, int id_station);
        List<IGrouping<string, KometaParkState>> GetKometaParkStateToWay(DateTime Date, int id_station);

        #endregion

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

        #region KometaStan

        IQueryable<KometaStan> KometaStan { get; }
        IQueryable<KometaStan> GetKometaStan();
        KometaStan GetKometaStan(int k_stan);
        #endregion

        #endregion

        #region PROM

        #region PROM.GRUZ_SP
        IQueryable<PromGruzSP> GetGruzSP();
        PromGruzSP GetGruzSP(int k_gruz); 
        PromGruzSP GetGruzSPToTarGR(int? tar_gr, bool corect);
        #endregion

        #region PROM.SOSTAV
        IQueryable<PromSostav> PromSostav { get; }
        IQueryable<PromSostav> GetPromSostav();
        IQueryable<PromSostav> GetInputPromSostav();
        IQueryable<PromSostav> GetInputPromSostav(DateTime start, DateTime stop);
        IQueryable<PromSostav> GetInputPromSostav(DateTime start, DateTime stop, bool sort);
        #endregion

        #region PROM.Nat_Hist
        IQueryable<PromNatHist> PromNatHist { get; }
        IQueryable<PromNatHist> GetPromNatHist();
        #endregion

        #region PROM.Vagon
        IQueryable<PromVagon> PromVagon {get;}
        IQueryable<PromVagon> GetVagon();
        IQueryable<PromVagon> GetVagon(int natur, int station, int day, int month, int year, bool? sort);
        IQueryable<PromVagon> GetVagon(int natur, int station, int day, int month, int year);
        int? CountWagonsVagon(int natur, int station, int day, int month, int year);
        PromVagon GetVagon(int natur, int station, int day, int month, int year, int num);
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
