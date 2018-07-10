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

        #region KometaStrana
        IQueryable<KometaStrana> KometaStrana { get; }
        IQueryable<KometaStrana> GetKometaStrana();
        KometaStrana GetKometaStrana(int KOD_STRAN);
        #endregion

        #endregion

        #region PROM

        #region PROM.GRUZ_SP
        IQueryable<PromGruzSP> GetGruzSP();
        PromGruzSP GetGruzSP(int k_gruz); 
        PromGruzSP GetGruzSPToTarGR(int? tar_gr, bool corect);
        #endregion

        #region PROM.SOSTAV
        //IQueryable<PromSostav> PromSostav { get; }
        //IQueryable<PromSostav> GetPromSostav();
        //IQueryable<PromSostav> GetPromSostav(DateTime start, DateTime stop);
        //IQueryable<PromSostav> GetInputPromSostav();
        //IQueryable<PromSostav> GetInputPromSostav(DateTime start, DateTime stop);
        //IQueryable<PromSostav> GetInputPromSostav(DateTime start, DateTime stop, bool sort);
        //IQueryable<PromSostav> GetOutputPromSostav();
        //IQueryable<PromSostav> GetOutputPromSostav(DateTime start, DateTime stop);
        //IQueryable<PromSostav> GetOutputPromSostav(DateTime start, DateTime stop, bool sort);
        #endregion

        #endregion Prom_Sostav

        IQueryable<Prom_Sostav> GetProm_Sostav();
        IQueryable<Prom_Sostav> GetProm_Sostav(DateTime start, DateTime stop);
        IQueryable<Prom_Sostav> GetInputProm_Sostav();
        IQueryable<Prom_Sostav> GetInputProm_Sostav(DateTime start, DateTime stop);
        IQueryable<Prom_Sostav> GetInputProm_Sostav(DateTime start, DateTime stop, bool sort);
        IQueryable<Prom_Sostav> GetOutputProm_Sostav();
        IQueryable<Prom_Sostav> GetOutputProm_Sostav(DateTime start, DateTime stop);
        IQueryable<Prom_Sostav> GetOutputProm_Sostav(DateTime start, DateTime stop, bool sort);

        IQueryable<Prom_SostavAndCount> GetProm_SostavAndCount();
        IQueryable<Prom_SostavAndCount> GetProm_SostavAndCount(DateTime start, DateTime stop);
        IQueryable<Prom_SostavAndCount> GetProm_SostavAndCount(int? natur, int? day, int? month, int? year, int? hour, int? minute);

        #region

        #region PROM.Nat_Hist
        IQueryable<PromNatHist> PromNatHist { get; }
        IQueryable<PromNatHist> GetPromNatHist();

        IQueryable<PromNatHist> GetNatHist(int natur, int station, int day, int month, int year, bool? sort);
        IQueryable<PromNatHist> GetNatHist(int natur, int station, int day, int month, int year);
        IQueryable<PromNatHist> GetNatHistSendingOfNaturAndDate(int natur, int day, int month, int year, bool? sort);
        IQueryable<PromNatHist> GetNatHistSendingOfNaturAndDate(int natur, int day, int month, int year);

        IQueryable<PromNatHist> GetNatHistSendingOfNaturAndDT(int natur, int day, int month, int year, int hour, int minute, bool? sort);
        PromNatHist GetNatHistSendingOfNaturNumDT(int natur, int num, int day, int month, int year, int hour, int minute);
        PromNatHist GetNatHistSendingOfNumDT(int num, int day, int month, int year, int hour, int minute);
        PromNatHist GetNatHist(int natur, int station, int day, int month, int year, int wag);

        IQueryable<PromNatHist> GetNatHistOfVagon(int num_vag);

        IQueryable<PromNatHist> GetNatHistOfVagonMoreSD(int num_vag, DateTime start);
        IQueryable<PromNatHist> GetNatHistOfVagonMoreSD(int num_vag, DateTime start, bool sort);

        IQueryable<PromNatHist> GetNatHistOfVagonLessPR(int num_vag, DateTime start);
        IQueryable<PromNatHist> GetNatHistOfVagonLessPR(int num_vag, DateTime start, bool sort);
        IQueryable<PromNatHist> GetNatHistOfVagonLessEqualPR(int num_vag, DateTime start);
        IQueryable<PromNatHist> GetNatHistOfVagonLessEqualPR(int num_vag, DateTime start, bool sort);
        IQueryable<PromNatHist> GetNatHistOfVagonGreaterEqualPR(int num_vag, DateTime start);
        IQueryable<PromNatHist> GetNatHistOfVagonGreaterEqualPR(int num_vag, DateTime start, bool sort);

        #endregion

        #region Prom_NatHist
        IQueryable<Prom_NatHist> GetProm_NatHist();
        IQueryable<Prom_NatHist> GetPRProm_NatHist(int natur, int day, int month, int year, int hour, int minute);
        IQueryable<Prom_NatHist> GetSDProm_NatHist(int natur, int day, int month, int year, int hour, int minute);
        #endregion

        #region Prom_NatHistAndSostav
        IQueryable<Prom_NatHistAndSostav> GetProm_NatHistAndSostav();
        IQueryable<Prom_NatHistAndSostav> GetProm_NatHistAndSostav(int num);
        #endregion

        #region PROM.Vagon
        IQueryable<PromVagon> PromVagon {get;}
        IQueryable<PromVagon> GetVagon();
        IQueryable<PromVagon> GetVagon(int natur, int station, int day, int month, int year, bool? sort);
        IQueryable<PromVagon> GetVagon(int natur, int station, int day, int month, int year);
        int? CountWagonsVagon(int natur, int station, int day, int month, int year);
        PromVagon GetVagon(int natur, int station, int day, int month, int year, int num);
        #endregion

        #region Prom_Vagon

        IQueryable<Prom_Vagon> GetProm_Vagon();
        IQueryable<Prom_Vagon> GetPRProm_Vagon(int natur, int day, int month, int year, int hour, int minute);
        IQueryable<Prom_Vagon> GetSDProm_Vagon(int natur, int day, int month, int year, int hour, int minute);

        #endregion

        #region Prom_VagonAndSostav

        IQueryable<Prom_VagonAndSostav> GetProm_VagonAndSostav();
        IQueryable<Prom_VagonAndSostav> GetProm_VagonAndSostav(int num);
        #endregion

        #region PROM.CEX
        IQueryable<PromCex> PromCex {get;}
        IQueryable<PromCex> GetCex();
        PromCex GetCex(int k_podr);
        #endregion

        #endregion

        #region NUM_VAG (Информация по вагонам)

        #region NumVagStpr1Gr (Справочник грузов по вагонам)
        IQueryable<NumVagStpr1Gr> GetSTPR1GR();
        NumVagStpr1Gr GetSTPR1GR(int kod_gr);
        #endregion

        #region NumVagStpr1InStDoc (Составы по прибытию)

        IQueryable<NumVagStpr1InStDoc> NumVagStpr1InStDoc { get; }
        IQueryable<NumVagStpr1InStDoc> GetSTPR1InStDoc();
        NumVagStpr1InStDoc GetSTPR1InStDoc(int doc);
        IQueryable<NumVagStpr1InStDoc> GetSTPR1InStDoc(DateTime start, DateTime stop);
        IQueryable<NumVagStpr1InStDoc> GetSTPR1InStDoc(DateTime start, DateTime stop, bool order);
        IQueryable<NumVagStpr1InStDoc> GetSTPR1InStDocOfAmkr(string where);
        IQueryable<NumVagStpr1InStDoc> GetSTPR1InStDocOfAmkr();
        #endregion

        #region NumVagStpr1InStVag (вагоны по прибытию)
        IQueryable<NumVagStpr1InStVag> NumVagStpr1InStVag { get; }
        IQueryable<NumVagStpr1InStVag> GetSTPR1InStVag();
        IQueryable<NumVagStpr1InStVag> GetSTPR1InStVag(int doc);
        IQueryable<NumVagStpr1InStVag> GetSTPR1InStVag(int doc, bool sort);
        int GetCountSTPR1InStVag(int doc);
        #endregion

        #region NumVagStpr1OutStDoc (Составы по отправке)
        IQueryable<NumVagStpr1OutStDoc> NumVagStpr1OutStDoc { get; }
        IQueryable<NumVagStpr1OutStDoc> GetSTPR1OutStDoc();
        NumVagStpr1OutStDoc GetSTPR1OutStDoc(int doc);
        IQueryable<NumVagStpr1OutStDoc> GetSTPR1OutStDoc(DateTime start, DateTime stop);
        IQueryable<NumVagStpr1OutStDoc> GetSTPR1OutStDoc(DateTime start, DateTime stop, bool order);
        IQueryable<NumVagStpr1OutStDoc> GetSTPR1OutStDocOfAmkr(string where);
        IQueryable<NumVagStpr1OutStDoc> GetSTPR1OutStDocOfAmkr();
        #endregion

        #region NumVagStpr1OutStVag (вагоны по отправке)
        IQueryable<NumVagStpr1OutStVag> NumVagStpr1OutStVag { get; }
        IQueryable<NumVagStpr1OutStVag> GetSTPR1OutStVag();
        IQueryable<NumVagStpr1OutStVag> GetSTPR1OutStVag(int doc);
        IQueryable<NumVagStpr1OutStVag> GetSTPR1OutStVag(int doc, bool sort);
        int GetCountSTPR1OutStVag(int doc);
        IQueryable<NumVagStpr1OutStVag> GetSTPR1OutStDocOfCarAndDoc(int[] nums_car, int[] nums_doc);
        #endregion

        #region NumVagStan (справочник станций)

        IQueryable<NumVagStan> NumVagStan { get; }
        IQueryable<NumVagStan> GetNumVagStations();
        NumVagStan GetNumVagStations(int id_stan);

        #endregion

        #region NumVagStpr1Tupik (справочник тупиков)
        IQueryable<NumVagStpr1Tupik> NumVagStpr1Tupik { get; }
        IQueryable<NumVagStpr1Tupik> GetNumVagStpr1Tupik();
        NumVagStpr1Tupik GetNumVagStpr1Tupik(int id_tupik);
        #endregion

        #region NumVagStran (справочник стран)
        IQueryable<NumVagStran> NumVagStran { get; }
        IQueryable<NumVagStran> GetNumVagStran();
        NumVagStran GetNumVagStran(int npp);
        NumVagStran GetNumVagStranOfCodeISO(int code);
        NumVagStran GetNumVagStranOfCodeEurope(int code);
        NumVagStran GetNumVagStranOfCodeSNG(int? code);
        #endregion

        #endregion
    }
}
