using EFKIS.Abstract;
using EFKIS.Entities;
//using EFKIS.Helpers;
using MessageLog;
using RWConversionFunctions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFKIS.Concrete
{
    public class rwCar
    {
        public int num { get; set; }
        public string rod { get; set; }
    }
    /// <summary>
    /// Класс данных 
    /// </summary>
    public class Prom_Sostav
    {
        public Int64 ID { get; set; }
        public DateTime DT_PR { get; set; }
        public DateTime DT { get; set; }
        public int N_NATUR { get; set; }
        public int? D_PR_DD { get; set; }
        public int? D_DD { get; set; }
        public int? D_PR_MM { get; set; }
        public int? D_MM { get; set; }
        public int? D_PR_YY { get; set; }
        public int? D_YY { get; set; }
        public int? T_PR_HH { get; set; }
        public int? T_HH { get; set; }
        public int? T_PR_MI { get; set; }
        public int? T_MI { get; set; }
        public int? K_ST { get; set; }
        public int? N_PUT { get; set; }
        public int? NAPR { get; set; }
        public int? P_OT { get; set; }
        public int? V_P { get; set; }
        public int? K_ST_OTPR { get; set; }
        public int? K_ST_PR { get; set; }
        public int? N_VED_PR { get; set; }
        public int? N_SOST_OT { get; set; }
        public int? N_SOST_PR { get; set; }
        public DateTime? DAT_VVOD { get; set; }
    }

    public class Prom_SostavAndCount : Prom_Sostav
    {

        public int countVagon { get; set; }
        public int? maxVagon { get; set; }
        public int countNatHist { get; set; }
        public int? maxNatHist { get; set; }
        public Prom_SostavAndCount()
            : base()
        {

        }
    }

    public class Prom_Vagon
    {
        public Int64 ID { get; set; }
        public DateTime? DT_PR { get; set; }
        public DateTime? DT_SD { get; set; }
        //public DateTime? DT_PR
        //{
        //    get
        //    {
        //        try
        //        {
        //            return new DateTime((int)D_PR_YY, (int)D_PR_MM, (int)D_PR_DD, (int)T_PR_HH, (int)T_PR_MI, 00);
        //        }
        //        catch (Exception e)
        //        {
        //            return null;
        //        }
        //    }
        //    private set { }
        //}
        //public DateTime? DT_SD
        //{
        //    get
        //    {
        //        try
        //        {
        //            return new DateTime((int)D_SD_YY, (int)D_SD_MM, (int)D_SD_DD, (int)T_SD_HH, (int)T_SD_MI, 00);
        //        }
        //        catch (Exception e)
        //        {
        //            return null;
        //        }
        //    }
        //    private set { }
        //}
        //дата
        public int? D_PR_DD { get; set; }
        public int? D_PR_MM { get; set; }
        public int? D_PR_YY { get; set; }
        public int? T_PR_HH { get; set; }
        public int? T_PR_MI { get; set; }
        //
        public int? D_SD_DD { get; set; }
        public int? D_SD_MM { get; set; }
        public int? D_SD_YY { get; set; }
        public int? T_SD_HH { get; set; }
        public int? T_SD_MI { get; set; }
        //
        public int N_VAG { get; set; }
        public int? NPP { get; set; }
        public int? GODN { get; set; }
        public int? K_ST { get; set; }
        public int? K_ST_KMK { get; set; }
        public int? K_POL_GR { get; set; }
        public int? N_VED_PR { get; set; }

        public int? N_NAK_MPS { get; set; }
        public string OTPRAV { get; set; }
        public string PRIM_GR { get; set; }
        public int? K_GR { get; set; }
        public decimal? WES_GR { get; set; }
        public int N_NATUR { get; set; }
        public int? N_PUT { get; set; }
        public int? K_OP { get; set; }
        public int? K_FRONT { get; set; }
        public int? N_NATUR_T { get; set; }
        public int? GODN_T { get; set; }
        public int? K_GR_T { get; set; }
        public decimal? WES_GR_T { get; set; }
        public int? K_OTPR_GR { get; set; }
        public int? K_ST_NAZN { get; set; }
        public int? K_ST_OTPR { get; set; }
        public string ST_OTPR { get; set; }
        public string ZADER { get; set; }
        public string NEPR { get; set; }
        public string UDOST { get; set; }
        public string SERTIF { get; set; }
        public int? KOD_STRAN { get; set; }
        public int? KOD_SD { get; set; }
        public decimal? NETO { get; set; }
        public decimal? BRUTO { get; set; }
        public decimal? TARA { get; set; }
        public DateTime? DAT_VVOD { get; set; }

        //,POD_L
        //,POD_N
        //,N_NAK_KMK
        //,NAME_PUT
        //,VLAD
        //,STAT1
        //,STAT2
        //,P_GRAZ
        //,P_DVER
        //,P_REM
        //,WES_PER
        //,D_NO_DD
        //,D_NO_MM
        //,D_OO_DD
        //,D_OO_MM
        //,T_NO_HH
        //,T_NO_MI
        //,T_OO_HH
        //,T_OO_MI
        //,POLUCH
        //,N_MPS
        //,N_KMK
        //,NAPR_SD
        //,STAT_SD1
        //,STAT_SD2
        //,P_GRAZ1
        //,P_DVER1
        //,P_REM1
        //,KOL_REM
        //,D_OP_DD
        //,D_OP_MM
        //,D_OP_HH
        //,T_NEP_HH
        //,T_NEP_MI
        //,KOL_OP
        //,K_RREM
        //,P_POVR
        //,D_OT_DD
        //,D_OT_MM
        //,T_OT_HH
        //,T_OT_MI
        //,NORMA
        //,COM
        //,DOGOVOR
        //,GODNOST
        //,GODNOST_SD
        //,DAT_TAMOZ
        //,DAT_DOKUM
        //,DAT_VVOD_DOKUM
        //,DAT_RAZBR
    }

    public class Prom_VagonAndSostav : Prom_Vagon
    {
        // дата
        public int? D_DD { get; set; }
        public int? D_MM { get; set; }
        public int? D_YY { get; set; }
        public int? T_HH { get; set; }
        public int? T_MI { get; set; }
        public DateTime? DT { get; set; }
        public int? P_OT { get; set; }
        public Prom_VagonAndSostav() : base() { }
    }

    public class Prom_NatHist
    {
        public Int64 ID { get; set; }
        public DateTime? DT_PR { get; set; }
        public DateTime? DT_SD { get; set; }
        public int? D_PR_DD { get; set; }
        public int? D_PR_MM { get; set; }
        public int? D_PR_YY { get; set; }
        public int? T_PR_HH { get; set; }
        public int? T_PR_MI { get; set; }
        //
        public int? D_SD_DD { get; set; }
        public int? D_SD_MM { get; set; }
        public int? D_SD_YY { get; set; }
        public int? T_SD_HH { get; set; }
        public int? T_SD_MI { get; set; }
        //
        public int N_VAG { get; set; }
        public int? NPP { get; set; }
        public int? GODN { get; set; }
        public int? K_ST { get; set; }
        public int? K_ST_KMK { get; set; }
        public int? K_POL_GR { get; set; }
        public int? N_VED_PR { get; set; }

        public int? N_NAK_MPS { get; set; }
        public string OTPRAV { get; set; }
        public string PRIM_GR { get; set; }
        public int? K_GR { get; set; }
        public decimal? WES_GR { get; set; }
        public int N_NATUR { get; set; }
        public int? N_PUT { get; set; }
        public int? K_OP { get; set; }
        public int? K_FRONT { get; set; }
        public int? N_NATUR_T { get; set; }
        public int? GODN_T { get; set; }
        public int? K_GR_T { get; set; }
        public decimal? WES_GR_T { get; set; }
        public int? K_OTPR_GR { get; set; }
        public int? K_ST_NAZN { get; set; }
        public int? K_ST_OTPR { get; set; }
        public string ST_OTPR { get; set; }
        public string ZADER { get; set; }
        public string NEPR { get; set; }
        public string UDOST { get; set; }
        public string SERTIF { get; set; }
        public int? KOD_STRAN { get; set; }
        public int? KOD_SD { get; set; }
        public decimal? NETO { get; set; }
        public decimal? BRUTO { get; set; }
        public decimal? TARA { get; set; }
        public DateTime? DAT_VVOD { get; set; }

        //,POD_L
        //,POD_N
        //,N_NAK_KMK
        //,NAME_PUT
        //,VLAD
        //,STAT1
        //,STAT2
        //,P_GRAZ
        //,P_DVER
        //,P_REM
        //,WES_PER
        //,D_NO_DD
        //,D_NO_MM
        //,D_OO_DD
        //,D_OO_MM
        //,T_NO_HH
        //,T_NO_MI
        //,T_OO_HH
        //,T_OO_MI
        //,POLUCH
        //,N_MPS
        //,N_KMK
        //,NAPR_SD
        //,STAT_SD1
        //,STAT_SD2
        //,P_GRAZ1
        //,P_DVER1
        //,P_REM1
        //,KOL_REM
        //,D_OP_DD
        //,D_OP_MM
        //,D_OP_HH
        //,T_NEP_HH
        //,T_NEP_MI
        //,KOL_OP
        //,K_RREM
        //,P_POVR
        //,D_OT_DD
        //,D_OT_MM
        //,T_OT_HH
        //,T_OT_MI
        //,NORMA
        //,COM
        //,DOGOVOR
        //,GODNOST
        //,GODNOST_SD
        //,DAT_TAMOZ
        //,DAT_DOKUM
        //,DAT_VVOD_DOKUM
        //,DAT_RAZBR
    }

    public class Prom_NatHistAndSostav : Prom_NatHist
    {
        // дата
        public int? D_DD { get; set; }
        public int? D_MM { get; set; }
        public int? D_YY { get; set; }
        public int? T_HH { get; set; }
        public int? T_MI { get; set; }
        public DateTime? DT { get; set; }
        public int? P_OT { get; set; }
        public Prom_NatHistAndSostav() : base() { }
    }

    public class NumVag_Stpr1OutStVag
    {
        public Int64 ID { get; set; }
        public int ID_DOC { get; set; }
        public int N_OUT_ST { get; set; }
        public int N_VAG { get; set; }
        public int? GODN_OUT_ST { get; set; }
        public int GR_OUT_ST { get; set; }
        public int SOBSTV { get; set; }
        public string REM_IN_ST { get; set; }
        public int ID_VAG { get; set; }
        public int? ST_NAZN_OUT_ST { get; set; }
        public int? STRAN_OUT_ST { get; set; }
        public int? SOBSTV_OLD { get; set; }
        public int? N_TUP_OUT_ST { get; set; }

    }

    public class EFWagons : IKIS
    {
        private eventID eventID = eventID.EFWagons;

        protected EFDbContext context = new EFDbContext();

        protected string sql_field_dt_pr = "to_date((to_char((CASE WHEN (s.D_PR_DD>=1 and s.D_PR_DD<=TO_CHAR(LAST_DAY(to_date((to_char(1,'09')||'.'||to_char(nvl((CASE WHEN (s.D_PR_MM>=1 and s.D_PR_MM<=12) THEN s.D_PR_MM ELSE 1 END),1),'09')||'.'||to_char(nvl((CASE WHEN (s.D_PR_YY>0) THEN s.D_PR_YY ELSE 1 END),1),'0009')),'dd.mm.yyyy')), 'DD')) THEN s.D_PR_DD ELSE 1 END),'09')||'.'||to_char(nvl((CASE WHEN (s.D_PR_MM>=1 and s.D_PR_MM<=12) THEN s.D_PR_MM ELSE 1 END),1),'09')||'.'||to_char(nvl((CASE WHEN (s.D_PR_YY>0) THEN s.D_PR_YY ELSE 1 END),1),'0009')||' '||to_char(nvl((CASE WHEN (s.T_PR_HH>=0 and s.T_PR_HH<=23) THEN s.T_PR_HH ELSE 0 END),1),'09')||':'||to_char(nvl((CASE WHEN (s.T_PR_MI>=0 and s.T_PR_MI<=59) THEN s.T_PR_MI ELSE 0 END),1),'09')),'dd.mm.yyyy hh24:mi')";
        protected string sql_field_dt = "to_date((to_char((CASE WHEN (s.D_DD>=1 and s.D_DD<=TO_CHAR(LAST_DAY(to_date((to_char(1,'09')||'.'||to_char(nvl((CASE WHEN (s.D_MM>=1 and s.D_MM<=12) THEN s.D_MM ELSE 1 END),1),'09')||'.'||to_char(nvl((CASE WHEN (s.D_YY>0) THEN s.D_YY ELSE 1 END),1),'0009')),'dd.mm.yyyy')), 'DD')) THEN s.D_DD ELSE 1 END),'09')||'.'||to_char(nvl((CASE WHEN (s.D_MM>=1 and s.D_MM<=12) THEN s.D_MM ELSE 1 END),1),'09')||'.'||to_char(nvl((CASE WHEN (s.D_YY>0) THEN s.D_YY ELSE 1 END),1),'0009')||' '||to_char(nvl((CASE WHEN (s.T_HH>=0 and s.T_HH<=23) THEN s.T_HH ELSE 0 END),1),'09')||':'||to_char(nvl((CASE WHEN (s.T_MI>=0 and s.T_MI<=59) THEN s.T_MI ELSE 0 END),1),'09')),'dd.mm.yyyy hh24:mi')";
        protected string sql_field_sostav = "ROWNUM as ID, s.N_NATUR ,s.N_VED_PR ,s.N_SOST_OT, s.N_SOST_PR, s.K_ST, s.K_ST_OTPR ,s.K_ST_PR ,s.N_PUT ,s.NAPR ,s.D_PR_DD ,s.D_PR_MM ,s.D_DD ,s.D_MM ,s.T_PR_HH ,s.T_PR_MI ,s.T_HH ,s.T_MI ,s.P_OT ,s.V_P ,s.ST_OTPR ,s.D_PR_YY ,s.D_YY ,s.DAT_VVOD";
        protected string sql_table_sostav = "FROM PROM.SOSTAV s";

        //protected string sql_NatHist = "SELECT ROWNUM as ID,N_VAG,NPP,D_PR_DD,D_PR_MM,D_PR_YY,T_PR_HH,T_PR_MI,D_SD_DD,D_SD_MM,D_SD_YY,T_SD_HH,T_SD_MI,GODN,K_ST_KMK,K_POL_GR,K_GR,N_VED_PR,N_NAK_MPS,OTPRAV,PRIM_GR,WES_GR,N_NATUR,N_PUT,K_ST,K_OP,K_FRONT,N_NATUR_T,GODN_T,K_GR_T,WES_GR_T,K_OTPR_GR,K_ST_OTPR,K_ST_NAZN,ST_OTPR,ZADER,NEPR,UDOST,SERTIF,KOD_STRAN,KOD_SD,NETO,BRUTO,TARA,DAT_VVOD FROM PROM.NAT_HIST";
        //protected string sql_Vagon = "SELECT ROWNUM as ID,N_VAG,NPP,D_PR_DD,D_PR_MM,D_PR_YY,T_PR_HH,T_PR_MI,D_SD_DD,D_SD_MM,D_SD_YY,T_SD_HH,T_SD_MI,GODN,K_ST_KMK,K_POL_GR,K_GR,N_VED_PR,N_NAK_MPS,OTPRAV,PRIM_GR,WES_GR,N_NATUR,N_PUT,K_ST,K_OP,K_FRONT,N_NATUR_T,GODN_T,K_GR_T,WES_GR_T,K_OTPR_GR,K_ST_OTPR,K_ST_NAZN,ST_OTPR,ZADER,NEPR,UDOST,SERTIF,KOD_STRAN,KOD_SD,NETO,BRUTO,TARA,DAT_VVOD FROM PROM.VAGON";

        //protected string sql_vagon_sostav = "SELECT ROWNUM as ID, " +
        //            "(CASE WHEN (v.D_PR_DD is null) THEN v.D_SD_DD ELSE v.D_PR_DD END) as D_DD, " +
        //            "(CASE WHEN (v.D_PR_MM is null) THEN v.D_SD_MM ELSE v.D_PR_MM END) as D_MM, " +
        //            "(CASE WHEN (v.D_PR_YY is null) THEN v.D_SD_YY ELSE v.D_PR_YY END) as D_YY, " +
        //            "(CASE WHEN (v.T_PR_HH is null) THEN v.T_SD_HH ELSE v.T_PR_HH END) as T_HH, " +
        //            "(CASE WHEN (v.T_PR_MI is null) THEN v.T_SD_MI ELSE v.T_PR_MI END) as T_MI, " +
        //            "(select max(s.P_OT) from PROM.SOSTAV s where (v.N_NATUR=s.N_NATUR and v.D_PR_YY=s.D_YY and v.D_PR_MM=s.D_MM and v.D_PR_DD=s.D_DD) or (v.N_NATUR=s.N_NATUR and v.D_SD_YY=s.D_YY and v.D_SD_MM=s.D_MM and v.D_SD_DD=s.D_DD) ) as P_OT, " +
        //            "to_date((to_char((CASE WHEN ((CASE WHEN (v.D_PR_DD is null) THEN v.D_SD_DD ELSE v.D_PR_DD END)>=1 and (CASE WHEN (v.D_PR_DD is null) THEN v.D_SD_DD ELSE v.D_PR_DD END)<=TO_CHAR(LAST_DAY(to_date((to_char(1,'09')||'.'||to_char(nvl((CASE WHEN ((CASE WHEN (v.D_PR_MM is null) THEN v.D_SD_MM ELSE v.D_PR_MM END)>=1 and (CASE WHEN (v.D_PR_MM is null) THEN v.D_SD_MM ELSE v.D_PR_MM END)<=12) THEN (CASE WHEN (v.D_PR_MM is null) THEN v.D_SD_MM ELSE v.D_PR_MM END) ELSE 1 END),1),'09')||'.'||to_char(nvl((CASE WHEN ((CASE WHEN (v.D_PR_YY is null) THEN v.D_SD_YY ELSE v.D_PR_YY END)>0) THEN (CASE WHEN (v.D_PR_YY is null) THEN v.D_SD_YY ELSE v.D_PR_YY END) ELSE 1 END),1),'0009')),'dd.mm.yyyy')), 'DD')) THEN (CASE WHEN (v.D_PR_DD is null) THEN v.D_SD_DD ELSE v.D_PR_DD END) ELSE 1 END),'09')||'.'||to_char(nvl((CASE WHEN ((CASE WHEN (v.D_PR_MM is null) THEN v.D_SD_MM ELSE v.D_PR_MM END)>=1 and (CASE WHEN (v.D_PR_MM is null) THEN v.D_SD_MM ELSE v.D_PR_MM END)<=12) THEN (CASE WHEN (v.D_PR_MM is null) THEN v.D_SD_MM ELSE v.D_PR_MM END) ELSE 1 END),1),'09')||'.'||to_char(nvl((CASE WHEN ((CASE WHEN (v.D_PR_YY is null) THEN v.D_SD_YY ELSE v.D_PR_YY END)>0) THEN (CASE WHEN (v.D_PR_YY is null) THEN v.D_SD_YY ELSE v.D_PR_YY END) ELSE 1 END),1),'0009')||' '||to_char(nvl((CASE WHEN ((CASE WHEN (v.T_PR_HH is null) THEN v.T_SD_HH ELSE v.T_PR_HH END)>=0 and (CASE WHEN (v.T_PR_HH is null) THEN v.T_SD_HH ELSE v.T_PR_HH END)<=23) THEN (CASE WHEN (v.T_PR_HH is null) THEN v.T_SD_HH ELSE v.T_PR_HH END) ELSE 0 END),1),'09')||':'||to_char(nvl((CASE WHEN ((CASE WHEN (v.T_PR_MI is null) THEN v.T_SD_MI ELSE v.T_PR_MI END)>=0 and (CASE WHEN (v.T_PR_MI is null) THEN v.T_SD_MI ELSE v.T_PR_MI END)<=59) THEN (CASE WHEN (v.T_PR_MI is null) THEN v.T_SD_MI ELSE v.T_PR_MI END) ELSE 0 END),1),'09')),'dd.mm.yyyy hh24:mi') as DT, " +
        //            "v.N_VAG, v.NPP, v.D_PR_DD, v.D_PR_MM, v.D_PR_YY, v.T_PR_HH, v.T_PR_MI, v.D_SD_DD, v.D_SD_MM, v.D_SD_YY, v.T_SD_HH, v.T_SD_MI, v.GODN, v.K_ST_KMK, v.K_POL_GR, v.K_GR, v.N_VED_PR, v.N_NAK_MPS, v.OTPRAV, v.PRIM_GR, v.WES_GR, v.N_NATUR, v.N_PUT, v.K_ST, v.K_OP, v.K_FRONT, v.N_NATUR_T, v.GODN_T, v.K_GR_T, v.WES_GR_T, v.K_OTPR_GR, v.K_ST_OTPR, v.K_ST_NAZN, v.ST_OTPR, v.ZADER, v.NEPR, v.UDOST, v.SERTIF, v.KOD_STRAN, v.KOD_SD, v.NETO, v.BRUTO, v.TARA, v.DAT_VVOD " +
        //            "FROM PROM.VAGON v";
        
        // общие строки формирования запроса
        protected static string field_key = "ROWNUM as ID, ";        
        // строки для формирования запроса к VAGON
        protected static string field_vagon = " N_VAG, v.NPP, v.D_PR_DD, v.D_PR_MM, v.D_PR_YY, v.T_PR_HH, v.T_PR_MI, v.D_SD_DD, v.D_SD_MM, v.D_SD_YY, v.T_SD_HH, v.T_SD_MI, v.GODN, v.K_ST_KMK, v.K_POL_GR, v.K_GR, v.N_VED_PR, v.N_NAK_MPS, v.OTPRAV, v.PRIM_GR, v.WES_GR, v.N_NATUR, v.N_PUT, v.K_ST, v.K_OP, v.K_FRONT, v.N_NATUR_T, v.GODN_T, v.K_GR_T, v.WES_GR_T, v.K_OTPR_GR, v.K_ST_OTPR, v.K_ST_NAZN, v.ST_OTPR, v.ZADER, v.NEPR, v.UDOST, v.SERTIF, v.KOD_STRAN, v.KOD_SD, v.NETO, v.BRUTO, v.TARA, v.DAT_VVOD ";
        protected static string field_vagon_dt_pr = " to_date((to_char((CASE WHEN (v.D_PR_DD>=1 and v.D_PR_DD<=TO_CHAR(LAST_DAY(to_date((to_char(1,'09')||'.'||to_char(nvl((CASE WHEN (v.D_PR_MM>=1 and v.D_PR_MM<=12) THEN v.D_PR_MM ELSE 1 END),1),'09')||'.'||to_char(nvl((CASE WHEN (v.D_PR_YY>0) THEN v.D_PR_YY ELSE 1 END),1),'0009')),'dd.mm.yyyy')), 'DD')) THEN v.D_PR_DD ELSE 1 END),'09')||'.'||to_char(nvl((CASE WHEN (v.D_PR_MM>=1 and v.D_PR_MM<=12) THEN v.D_PR_MM ELSE 1 END),1),'09')||'.'||to_char(nvl((CASE WHEN (v.D_PR_YY>0) THEN v.D_PR_YY ELSE 1 END),1),'0009')||' '||to_char(nvl((CASE WHEN (v.T_PR_HH>=0 and v.T_PR_HH<=23) THEN v.T_PR_HH ELSE 0 END),1),'09')||':'||to_char(nvl((CASE WHEN (v.T_PR_MI>=0 and v.T_PR_MI<=59) THEN v.T_PR_MI ELSE 0 END),1),'09')),'dd.mm.yyyy hh24:mi') ";
        protected static string field_vagon_dt_sd = " to_date((to_char((CASE WHEN (v.D_SD_DD>=1 and v.D_SD_DD<=TO_CHAR(LAST_DAY(to_date((to_char(1,'09')||'.'||to_char(nvl((CASE WHEN (v.D_SD_MM>=1 and v.D_SD_MM<=12) THEN v.D_SD_MM ELSE 1 END),1),'09')||'.'||to_char(nvl((CASE WHEN (v.D_SD_YY>0) THEN v.D_SD_YY ELSE 1 END),1),'0009')),'dd.mm.yyyy')), 'DD')) THEN v.D_SD_DD ELSE 1 END),'09')||'.'||to_char(nvl((CASE WHEN (v.D_SD_MM>=1 and v.D_SD_MM<=12) THEN v.D_SD_MM ELSE 1 END),1),'09')||'.'||to_char(nvl((CASE WHEN (v.D_SD_YY>0) THEN v.D_SD_YY ELSE 1 END),1),'0009')||' '||to_char(nvl((CASE WHEN (v.T_SD_HH>=0 and v.T_SD_HH<=23) THEN v.T_SD_HH ELSE 0 END),1),'09')||':'||to_char(nvl((CASE WHEN (v.T_SD_MI>=0 and v.T_SD_MI<=59) THEN v.T_SD_MI ELSE 0 END),1),'09')),'dd.mm.yyyy hh24:mi') ";
        protected static string field_vagon_dt_pr_null = " (CASE WHEN (v.D_PR_DD is null and v.D_PR_MM is null and v.D_PR_YY is null and v.T_PR_HH is null and v.T_PR_MI is null) THEN null ELSE " + field_vagon_dt_pr + " END ) ";
        protected static string field_vagon_dt_sd_null = " (CASE WHEN (v.D_SD_DD is null and v.D_SD_MM is null and v.D_SD_YY is null and v.T_SD_HH is null and v.T_SD_MI is null) THEN null ELSE " + field_vagon_dt_sd + " END ) ";
        protected static string vagon_table = "  FROM PROM.VAGON v ";
        protected static string order_vagon_field_dt_pr = " ORDER BY v.D_PR_YY, v.D_PR_MM, v.D_PR_DD, v.T_PR_HH, v.T_PR_MI ";
        protected static string order_vagon_field_dt_pr_desc = " ORDER BY v.D_PR_YY DESC, v.D_PR_MM DESC, v.D_PR_DD DESC, v.T_PR_HH DESC, v.T_PR_MI DESC ";
        protected static string order_vagon_field_dt_sd = " ORDER BY v.D_SD_YY, v.D_SD_MM, v.D_SD_DD, v.T_SD_HH, v.T_SD_MI ";
        protected static string order_vagon_field_dt_sd_desc = " ORDER BY v.D_SD_YY DESC, v.D_SD_MM DESC, v.D_SD_DD DESC, v.T_SD_HH DESC, v.T_SD_MI DESC ";
        //select
        protected string sql_vagon_select = "SELECT " + field_key + field_vagon + "," + field_vagon_dt_pr_null + "AS DT_PR, " + field_vagon_dt_sd_null + "AS DT_SD " + vagon_table;
        protected string sql_vagon_sostav_select = "SELECT " + field_key +
            "(CASE WHEN (v.D_PR_DD is null) THEN v.D_SD_DD ELSE v.D_PR_DD END) as D_DD, " +
            "(CASE WHEN (v.D_PR_MM is null) THEN v.D_SD_MM ELSE v.D_PR_MM END) as D_MM, " +
            "(CASE WHEN (v.D_PR_YY is null) THEN v.D_SD_YY ELSE v.D_PR_YY END) as D_YY, " +
            "(CASE WHEN (v.T_PR_HH is null) THEN v.T_SD_HH ELSE v.T_PR_HH END) as T_HH, " +
            "(CASE WHEN (v.T_PR_MI is null) THEN v.T_SD_MI ELSE v.T_PR_MI END) as T_MI, " +
            "(select max(s.P_OT) from PROM.SOSTAV s where (v.N_NATUR=s.N_NATUR and v.D_PR_YY=s.D_YY and v.D_PR_MM=s.D_MM and v.D_PR_DD=s.D_DD) or (v.N_NATUR=s.N_NATUR and v.D_SD_YY=s.D_YY and v.D_SD_MM=s.D_MM and v.D_SD_DD=s.D_DD) ) as P_OT, " +
            "to_date((to_char((CASE WHEN ((CASE WHEN (v.D_PR_DD is null) THEN v.D_SD_DD ELSE v.D_PR_DD END)>=1 and (CASE WHEN (v.D_PR_DD is null) THEN v.D_SD_DD ELSE v.D_PR_DD END)<=TO_CHAR(LAST_DAY(to_date((to_char(1,'09')||'.'||to_char(nvl((CASE WHEN ((CASE WHEN (v.D_PR_MM is null) THEN v.D_SD_MM ELSE v.D_PR_MM END)>=1 and (CASE WHEN (v.D_PR_MM is null) THEN v.D_SD_MM ELSE v.D_PR_MM END)<=12) THEN (CASE WHEN (v.D_PR_MM is null) THEN v.D_SD_MM ELSE v.D_PR_MM END) ELSE 1 END),1),'09')||'.'||to_char(nvl((CASE WHEN ((CASE WHEN (v.D_PR_YY is null) THEN v.D_SD_YY ELSE v.D_PR_YY END)>0) THEN (CASE WHEN (v.D_PR_YY is null) THEN v.D_SD_YY ELSE v.D_PR_YY END) ELSE 1 END),1),'0009')),'dd.mm.yyyy')), 'DD')) THEN (CASE WHEN (v.D_PR_DD is null) THEN v.D_SD_DD ELSE v.D_PR_DD END) ELSE 1 END),'09')||'.'||to_char(nvl((CASE WHEN ((CASE WHEN (v.D_PR_MM is null) THEN v.D_SD_MM ELSE v.D_PR_MM END)>=1 and (CASE WHEN (v.D_PR_MM is null) THEN v.D_SD_MM ELSE v.D_PR_MM END)<=12) THEN (CASE WHEN (v.D_PR_MM is null) THEN v.D_SD_MM ELSE v.D_PR_MM END) ELSE 1 END),1),'09')||'.'||to_char(nvl((CASE WHEN ((CASE WHEN (v.D_PR_YY is null) THEN v.D_SD_YY ELSE v.D_PR_YY END)>0) THEN (CASE WHEN (v.D_PR_YY is null) THEN v.D_SD_YY ELSE v.D_PR_YY END) ELSE 1 END),1),'0009')||' '||to_char(nvl((CASE WHEN ((CASE WHEN (v.T_PR_HH is null) THEN v.T_SD_HH ELSE v.T_PR_HH END)>=0 and (CASE WHEN (v.T_PR_HH is null) THEN v.T_SD_HH ELSE v.T_PR_HH END)<=23) THEN (CASE WHEN (v.T_PR_HH is null) THEN v.T_SD_HH ELSE v.T_PR_HH END) ELSE 0 END),1),'09')||':'||to_char(nvl((CASE WHEN ((CASE WHEN (v.T_PR_MI is null) THEN v.T_SD_MI ELSE v.T_PR_MI END)>=0 and (CASE WHEN (v.T_PR_MI is null) THEN v.T_SD_MI ELSE v.T_PR_MI END)<=59) THEN (CASE WHEN (v.T_PR_MI is null) THEN v.T_SD_MI ELSE v.T_PR_MI END) ELSE 0 END),1),'09')),'dd.mm.yyyy hh24:mi') as DT, " +
            field_vagon_dt_pr_null + "AS DT_PR, " + field_vagon_dt_sd_null + "AS DT_SD, " + field_vagon + vagon_table;


        // строки для формирования запроса к NAT_HIST

        protected static string field_nathist = " h.N_VAG, h.NPP, h.D_PR_DD, h.D_PR_MM, h.D_PR_YY, h.T_PR_HH, h.T_PR_MI, h.D_SD_DD, h.D_SD_MM, h.D_SD_YY, h.T_SD_HH, h.T_SD_MI, h.GODN, h.K_ST_KMK, h.K_POL_GR, h.K_GR, h.N_VED_PR, h.N_NAK_MPS, h.OTPRAV, h.PRIM_GR, h.WES_GR, h.N_NATUR, h.N_PUT, h.K_ST, h.K_OP, h.K_FRONT, h.N_NATUR_T, h.GODN_T, h.K_GR_T, h.WES_GR_T, h.K_OTPR_GR, h.K_ST_OTPR, h.K_ST_NAZN, h.ST_OTPR, h.ZADER, h.NEPR, h.UDOST, h.SERTIF, h.KOD_STRAN, h.KOD_SD, h.NETO, h.BRUTO, h.TARA, h.DAT_VVOD ";
        protected static string field_nathist_dt_pr = " to_date((to_char((CASE WHEN (h.D_PR_DD>=1 and h.D_PR_DD<=TO_CHAR(LAST_DAY(to_date((to_char(1,'09')||'.'||to_char(nvl((CASE WHEN (h.D_PR_MM>=1 and h.D_PR_MM<=12) THEN h.D_PR_MM ELSE 1 END),1),'09')||'.'||to_char(nvl((CASE WHEN (h.D_PR_YY>0) THEN h.D_PR_YY ELSE 1 END),1),'0009')),'dd.mm.yyyy')), 'DD')) THEN h.D_PR_DD ELSE 1 END),'09')||'.'||to_char(nvl((CASE WHEN (h.D_PR_MM>=1 and h.D_PR_MM<=12) THEN h.D_PR_MM ELSE 1 END),1),'09')||'.'||to_char(nvl((CASE WHEN (h.D_PR_YY>0) THEN h.D_PR_YY ELSE 1 END),1),'0009')||' '||to_char(nvl((CASE WHEN (h.T_PR_HH>=0 and h.T_PR_HH<=23) THEN h.T_PR_HH ELSE 0 END),1),'09')||':'||to_char(nvl((CASE WHEN (h.T_PR_MI>=0 and h.T_PR_MI<=59) THEN h.T_PR_MI ELSE 0 END),1),'09')),'dd.mm.yyyy hh24:mi') ";
        protected static string field_nathist_dt_sd = " to_date((to_char((CASE WHEN (h.D_SD_DD>=1 and h.D_SD_DD<=TO_CHAR(LAST_DAY(to_date((to_char(1,'09')||'.'||to_char(nvl((CASE WHEN (h.D_SD_MM>=1 and h.D_SD_MM<=12) THEN h.D_SD_MM ELSE 1 END),1),'09')||'.'||to_char(nvl((CASE WHEN (h.D_SD_YY>0) THEN h.D_SD_YY ELSE 1 END),1),'0009')),'dd.mm.yyyy')), 'DD')) THEN h.D_SD_DD ELSE 1 END),'09')||'.'||to_char(nvl((CASE WHEN (h.D_SD_MM>=1 and h.D_SD_MM<=12) THEN h.D_SD_MM ELSE 1 END),1),'09')||'.'||to_char(nvl((CASE WHEN (h.D_SD_YY>0) THEN h.D_SD_YY ELSE 1 END),1),'0009')||' '||to_char(nvl((CASE WHEN (h.T_SD_HH>=0 and h.T_SD_HH<=23) THEN h.T_SD_HH ELSE 0 END),1),'09')||':'||to_char(nvl((CASE WHEN (h.T_SD_MI>=0 and h.T_SD_MI<=59) THEN h.T_SD_MI ELSE 0 END),1),'09')),'dd.mm.yyyy hh24:mi') ";
        protected static string field_nathist_dt_pr_null = " (CASE WHEN (h.D_PR_DD is null and h.D_PR_MM is null and h.D_PR_YY is null and h.T_PR_HH is null and h.T_PR_MI is null) THEN null ELSE " + field_nathist_dt_pr + " END ) ";
        protected static string field_nathist_dt_sd_null = " (CASE WHEN (h.D_SD_DD is null and h.D_SD_MM is null and h.D_SD_YY is null and h.T_SD_HH is null and h.T_SD_MI is null) THEN null ELSE " + field_nathist_dt_sd + " END ) ";
        protected static string nathist_table = " FROM PROM.NAT_HIST h ";
        protected static string order_nathist_field_dt_pr = " ORDER BY h.D_PR_YY, h.D_PR_MM, h.D_PR_DD, h.T_PR_HH, h.T_PR_MI ";
        protected static string order_nathist_field_dt_pr_desc = " ORDER BY h.D_PR_YY DESC, h.D_PR_MM DESC, h.D_PR_DD DESC, h.T_PR_HH DESC, h.T_PR_MI DESC ";
        protected static string order_nathist_field_dt_sd = " ORDER BY h.D_SD_YY, h.D_SD_MM, h.D_SD_DD, h.T_SD_HH, h.T_SD_MI ";
        protected static string order_nathist_field_dt_sd_desc = " ORDER BY h.D_SD_YY DESC, h.D_SD_MM DESC, h.D_SD_DD DESC, h.T_SD_HH DESC, h.T_SD_MI DESC ";
        //select
        protected string sql_nathist_select = "SELECT " + field_key + field_nathist + "," + field_nathist_dt_pr_null + "AS DT_PR, " + field_nathist_dt_sd_null + "AS DT_SD " + nathist_table;
        protected string sql_nathist_sostav_select = "SELECT " + field_key +
                    "(CASE WHEN (h.D_PR_DD is null) THEN h.D_SD_DD ELSE h.D_PR_DD END) as D_DD, " +
                    "(CASE WHEN (h.D_PR_MM is null) THEN h.D_SD_MM ELSE h.D_PR_MM END) as D_MM, " +
                    "(CASE WHEN (h.D_PR_YY is null) THEN h.D_SD_YY ELSE h.D_PR_YY END) as D_YY, " +
                    "(CASE WHEN (h.T_PR_HH is null) THEN h.T_SD_HH ELSE h.T_PR_HH END) as T_HH, " +
                    "(CASE WHEN (h.T_PR_MI is null) THEN h.T_SD_MI ELSE h.T_PR_MI END) as T_MI, " +
                    "(select max(s.P_OT) from PROM.SOSTAV s where (h.N_NATUR=s.N_NATUR and h.D_PR_YY=s.D_YY and h.D_PR_MM=s.D_MM and h.D_PR_DD=s.D_DD) or (h.N_NATUR=s.N_NATUR and h.D_SD_YY=s.D_YY and h.D_SD_MM=s.D_MM and h.D_SD_DD=s.D_DD) ) as P_OT, " +
                    "to_date((to_char((CASE WHEN ((CASE WHEN (h.D_PR_DD is null) THEN h.D_SD_DD ELSE h.D_PR_DD END)>=1 and (CASE WHEN (h.D_PR_DD is null) THEN h.D_SD_DD ELSE h.D_PR_DD END)<=TO_CHAR(LAST_DAY(to_date((to_char(1,'09')||'.'||to_char(nvl((CASE WHEN ((CASE WHEN (h.D_PR_MM is null) THEN h.D_SD_MM ELSE h.D_PR_MM END)>=1 and (CASE WHEN (h.D_PR_MM is null) THEN h.D_SD_MM ELSE h.D_PR_MM END)<=12) THEN (CASE WHEN (h.D_PR_MM is null) THEN h.D_SD_MM ELSE h.D_PR_MM END) ELSE 1 END),1),'09')||'.'||to_char(nvl((CASE WHEN ((CASE WHEN (h.D_PR_YY is null) THEN h.D_SD_YY ELSE h.D_PR_YY END)>0) THEN (CASE WHEN (h.D_PR_YY is null) THEN h.D_SD_YY ELSE h.D_PR_YY END) ELSE 1 END),1),'0009')),'dd.mm.yyyy')), 'DD')) THEN (CASE WHEN (h.D_PR_DD is null) THEN h.D_SD_DD ELSE h.D_PR_DD END) ELSE 1 END),'09')||'.'||to_char(nvl((CASE WHEN ((CASE WHEN (h.D_PR_MM is null) THEN h.D_SD_MM ELSE h.D_PR_MM END)>=1 and (CASE WHEN (h.D_PR_MM is null) THEN h.D_SD_MM ELSE h.D_PR_MM END)<=12) THEN (CASE WHEN (h.D_PR_MM is null) THEN h.D_SD_MM ELSE h.D_PR_MM END) ELSE 1 END),1),'09')||'.'||to_char(nvl((CASE WHEN ((CASE WHEN (h.D_PR_YY is null) THEN h.D_SD_YY ELSE h.D_PR_YY END)>0) THEN (CASE WHEN (h.D_PR_YY is null) THEN h.D_SD_YY ELSE h.D_PR_YY END) ELSE 1 END),1),'0009')||' '||to_char(nvl((CASE WHEN ((CASE WHEN (h.T_PR_HH is null) THEN h.T_SD_HH ELSE h.T_PR_HH END)>=0 and (CASE WHEN (h.T_PR_HH is null) THEN h.T_SD_HH ELSE h.T_PR_HH END)<=23) THEN (CASE WHEN (h.T_PR_HH is null) THEN h.T_SD_HH ELSE h.T_PR_HH END) ELSE 0 END),1),'09')||':'||to_char(nvl((CASE WHEN ((CASE WHEN (h.T_PR_MI is null) THEN h.T_SD_MI ELSE h.T_PR_MI END)>=0 and (CASE WHEN (h.T_PR_MI is null) THEN h.T_SD_MI ELSE h.T_PR_MI END)<=59) THEN (CASE WHEN (h.T_PR_MI is null) THEN h.T_SD_MI ELSE h.T_PR_MI END) ELSE 0 END),1),'09')),'dd.mm.yyyy hh24:mi') as DT, " +
                    field_nathist_dt_pr_null + "AS DT_PR, " + field_nathist_dt_sd_null + "AS DT_SD, " + field_nathist + nathist_table;

        // строки для формирования запроса к NumVagStpr1OutStVag
        protected static string field_NumVagStpr1OutStVag = " v.ID_DOC, v.N_OUT_ST, v.GODN_OUT_ST, v.GR_OUT_ST, v.REM_IN_ST, v.N_TUP_OUT_ST, v.ST_NAZN_OUT_ST, v.STRAN_OUT_ST, v.N_VAG, v.SOBSTV, v.ID_VAG, v.SOBSTV_OLD ";
        protected static string NumVagStpr1OutStVag_table = " FROM NUM_VAG.STPR1_OUT_ST_VAG v ";
        // select
        protected string sql_NumVagStpr1OutStVag_select = "SELECT " + field_key + field_NumVagStpr1OutStVag + NumVagStpr1OutStVag_table;


        #region KOMETA

        #region KometaParkState

        public IQueryable<KometaParkState> KometaParkState
        {
            get { return context.KometaParkState; }
        }

        public IQueryable<KometaParkState> GetKometaParkState()
        {
            try
            {
                return KometaParkState;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetKometaParkState()"), eventID);
                return null;
            }
        }

        public IQueryable<KometaParkState> GetKometaParkState(DateTime Date)
        {
            DateTime date_start = Date.Date;
            DateTime date_stop = date_start.AddDays(1).AddSeconds(-1);
            try
            {
                return GetKometaParkState().Where(p => p.DATE_DOC >= date_start & p.DATE_DOC <= date_stop).OrderByDescending(p => p.DATE_DOC).OrderBy(p => p.K_STAN).OrderBy(p => p.RAIL).OrderBy(p => p.NN);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetKometaParkState(Date={0})", Date), eventID);
                return null;
            }
        }

        public List<IGrouping<string, KometaParkState>> GetKometaParkStateToStation(DateTime Date)
        {
            //List<IGrouping<int, KometaVagonSob>> group_list = new List<IGrouping<int, KometaVagonSob>>();
            //group_list = GetVagonsSob().GroupBy(s => s.N_VAGON).ToList();

            try
            {
                List<KometaParkState> list = GetKometaParkState(Date).ToList();
                return list.GroupBy(s => s.K_STAN).ToList();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetKometaParkStateToStation(Date={0})", Date), eventID);
                return null;
            }
        }

        public IQueryable<KometaParkState> GetKometaParkState(DateTime Date, int id_station)
        {
            try
            {
                List<KometaParkState> list = GetKometaParkState(Date).ToList();
                return list.Where(p => int.Parse(p.K_STAN) == id_station).AsQueryable();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetKometaParkState(Date={0}, id_station={1})", Date, id_station), eventID);
                return null;
            }
        }

        public List<IGrouping<string, KometaParkState>> GetKometaParkStateToWay(DateTime Date, int id_station)
        {
            try
            {
                List<KometaParkState> list = GetKometaParkState(Date, id_station).ToList();
                return list.GroupBy(s => s.RAIL).ToList();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetKometaParkState(Date={0}, id_station={1})", Date, id_station), eventID);
                return null;
            }
        }

        #endregion

        #region KometaVagonSob

        public IQueryable<KometaVagonSob> KometaVagonSob
        {
            get { return context.KometaVagonSob; }
        }
        /// <summary>
        /// Получить все вагоны
        /// </summary>
        /// <returns></returns>
        public IQueryable<KometaVagonSob> GetVagonsSob()
        {
            try
            {
                return KometaVagonSob;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetVagonsSob()"), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить список вагонов по указаному номеру
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public IQueryable<KometaVagonSob> GetVagonsSob(int num)
        {
            try
            {
                return GetVagonsSob().Where(c => c.N_VAGON == num).OrderByDescending(c => c.DATE_AR);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetVagonsSob(etsng={0})", num), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить список вагонов по указаному номеру с незаконченой арендой
        /// </summary>
        /// <param name="num"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public KometaVagonSob GetVagonsSob(int num, DateTime dt)
        {
            try
            {
                return GetVagonsSob(num).Where(v => v.DATE_AR <= dt & v.DATE_END == null).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetVagonsSob(num={0}, dt={1})", num, dt), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить список вагонов по которым поменяли владельца за указаную дату период до
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="day_period"></param>
        /// <returns></returns>
        public IQueryable<KometaVagonSob> GetChangeVagonsSob(DateTime dt, int day_period)
        {
            try
            {
                DateTime start_dt = dt.AddDays(-1 * day_period);
                return GetVagonsSob().Where(c => c.DATE_AR >= start_dt & c.DATE_AR <= dt).OrderBy(c => c.DATE_AR);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetChangeVagonsSob(dt={0}, day_period={1})", dt, day_period), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить список вагонов по которым поменяли владельца за указанный период
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="day_period"></param>
        /// <returns></returns>
        public IQueryable<KometaVagonSob> GetChangeVagonsSob(int day_period)
        {
            try
            {
                return GetChangeVagonsSob(DateTime.Now, day_period);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetChangeVagonsSob(day_period={0})", day_period), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить список всех вагонов
        /// </summary>
        /// <returns></returns>
        public List<rwCar> GetVagons()
        {
            List<rwCar> list = new List<rwCar>();
            try
            {
                List<IGrouping<int, KometaVagonSob>> group_list = new List<IGrouping<int, KometaVagonSob>>();
                group_list = GetVagonsSob().GroupBy(s => s.N_VAGON).ToList();
                foreach (IGrouping<int, KometaVagonSob> group_wag in group_list)
                {
                    KometaVagonSob kv = group_wag.OrderBy(v => v.ROD).FirstOrDefault();
                    if (kv != null)
                    {
                        list.Add(new rwCar() { num = kv.N_VAGON, rod = kv.ROD });
                    }
                }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetVagons()"), eventID);
                return null;
            }
            return list;
            //return rep_vsob.db.SqlQuery<rwCar>("SELECT [N_VAGON], [ROD] FROM KOMETA.VAGON_SOB group by [N_VAGON], [ROD]");
        }
        #endregion

        #region KometaSobstvForNakl
        /// <summary>
        /// Получить собственников по накладной
        /// </summary>
        /// <returns></returns>
        public IQueryable<KometaSobstvForNakl> GetSobstvForNakl()
        {
            try
            {
                return context.KometaSobstvForNakl;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetSobstvForNakl()"), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить собственников по накладной по коду
        /// </summary>
        /// <param name="kod_sob"></param>
        /// <returns></returns>
        public KometaSobstvForNakl GetSobstvForNakl(int kod_sob)
        {
            try
            {
                return GetSobstvForNakl().Where(s => s.SOBSTV == kod_sob).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetSobstvForNakl(kod_sob={0})", kod_sob), eventID);
                return null;
            }
        }
        #endregion

        #region KometaStan

        public IQueryable<KometaStan> KometaStan
        {
            get { return context.KometaStan; }
        }
        /// <summary>
        /// Вернуть список станций
        /// </summary>
        /// <returns></returns>
        public IQueryable<KometaStan> GetKometaStan()
        {
            try
            {
                return KometaStan;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetKometaStan()"), eventID);
                return null;
            }
        }
        /// <summary>
        /// Вернуть станцию по коду
        /// </summary>
        /// <param name="k_stan"></param>
        /// <returns></returns>
        public KometaStan GetKometaStan(int k_stan)
        {
            try
            {
                return GetKometaStan().Where(s => s.K_STAN == k_stan).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetKometaStan(k_stan={0})", k_stan), eventID);
                return null;
            }
        }
        #endregion

        #region KometaStrana

        public IQueryable<KometaStrana> KometaStrana
        {
            get { return context.KometaStrana; }
        }
        /// <summary>
        /// Вернуть список стран
        /// </summary>
        /// <returns></returns>
        public IQueryable<KometaStrana> GetKometaStrana()
        {
            try
            {
                return KometaStrana;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetKometaStrana()"), eventID);
                return null;
            }
        }
        /// <summary>
        /// Вернуть страну по коду
        /// </summary>
        /// <param name="k_stan"></param>
        /// <returns></returns>
        public KometaStrana GetKometaStrana(int KOD_STRAN)
        {
            try
            {
                return GetKometaStrana().Where(s => s.KOD_STRAN == KOD_STRAN).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetKometaStrana(KOD_STRAN={0})", KOD_STRAN), eventID);
                return null;
            }
        }
        #endregion

        #endregion

        #region PROM (Информация по станции промышленная)

        #region PROM.GRUZ_SP
        /// <summary>
        /// Получить все грузы
        /// </summary>
        /// <returns></returns>
        public IQueryable<PromGruzSP> GetGruzSP()
        {
            try
            {
                return context.PromGruzSP;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetGruzSP()"), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить груз по коду груза
        /// </summary>
        /// <param name="k_gruz"></param>
        /// <returns></returns>
        public PromGruzSP GetGruzSP(int k_gruz)
        {
            try
            {
                return GetGruzSP().Where(g => g.K_GRUZ == k_gruz).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetGruzSP(k_gruz={0})", k_gruz), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить код груза по коду ЕТСНГ (corect - false без коррекции кода ЕТСНГ, corect - true с коррекцией кода ЕТСНГ поиск по диапазону код0 ... код9)
        /// </summary>
        /// <param name="tar_gr"></param>
        /// <param name="corect"></param>
        /// <returns></returns>
        public PromGruzSP GetGruzSPToTarGR(int? tar_gr, bool corect)
        {
            try
            {
                if (!corect)
                {
                    return GetGruzSP().Where(g => g.TAR_GR == tar_gr).FirstOrDefault();
                }
                else
                {
                    return GetGruzSP().Where(g => g.TAR_GR >= tar_gr * 10 & g.TAR_GR <= (tar_gr * 10) + 9).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetGruzSPToTarGR(tar_gr={0}, corect={1})", tar_gr, corect), eventID);
                return null;
            }
        }

        #endregion

        #region Prom_Sostav
        public IQueryable<Prom_Sostav> GetProm_Sostav()
        {
            try
            {
                return context.Database.SqlQuery<Prom_Sostav>("SELECT " + sql_field_sostav + "," + sql_field_dt_pr + " as DT_PR" + "," + sql_field_dt + " as DT " + sql_table_sostav).AsQueryable();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetProm_Sostav()"), eventID);
                return null;
            }
        }

        public IQueryable<Prom_Sostav> GetProm_Sostav(DateTime start, DateTime stop)
        {
            try
            {
                return GetProm_Sostav().Where(p => p.DT >= start & p.DT <= stop);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetProm_Sostav(start={0}, stop={1})", start, stop), eventID);
                return null;
            }
        }

        public IQueryable<Prom_Sostav> GetInputProm_Sostav()
        {
            try
            {
                return GetProm_Sostav().Where(p => p.P_OT == 0 & p.V_P == 1 & p.K_ST != null);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetInputProm_Sostav()"), eventID);
                return null;
            }
        }

        public IQueryable<Prom_Sostav> GetInputProm_Sostav(DateTime start, DateTime stop)
        {
            try
            {
                return GetInputProm_Sostav().Where(p => p.DT_PR >= start & p.DT_PR <= stop);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetInputProm_Sostav(start={0}, stop={1})", start, stop), eventID);
                return null;
            }
        }

        public IQueryable<Prom_Sostav> GetInputProm_Sostav(DateTime start, DateTime stop, bool sort)
        {
            try
            {
                if (sort)
                {
                    return GetInputProm_Sostav(start, stop).OrderByDescending(p => p.DT);
                }
                else
                {
                    return GetInputProm_Sostav(start, stop).OrderBy(p => p.DT);
                }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetInputProm_Sostav(start={0}, stop={1}, sort={2})", start, stop, sort), eventID);
                return null;
            }

        }

        public IQueryable<Prom_Sostav> GetOutputProm_Sostav()
        {
            try
            {
                return GetProm_Sostav().Where(p => p.P_OT == 1);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetOutputProm_Sostav()"), eventID);
                return null;
            }
        }

        public IQueryable<Prom_Sostav> GetOutputProm_Sostav(DateTime start, DateTime stop)
        {
            try
            {
                return GetOutputProm_Sostav().Where(p => p.DT >= start & p.DT <= stop);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetOutputProm_Sostav(start={0}, stop={1})", start, stop), eventID);
                return null;
            }
        }

        public IQueryable<Prom_Sostav> GetOutputProm_Sostav(DateTime start, DateTime stop, bool sort)
        {
            try
            {
                if (sort)
                {
                    return GetOutputProm_Sostav(start, stop).OrderByDescending(p => p.DT);
                }
                else
                {
                    return GetOutputProm_Sostav(start, stop).OrderBy(p => p.DT);
                }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetOutputProm_Sostav(start={0}, stop={1}, sort={2})", start, stop, sort), eventID);
                return null;
            }

        }
        /// <summary>
        /// Показать все составы и количество вагонов
        /// </summary>
        /// <returns></returns>
        public IQueryable<Prom_SostavAndCount> GetProm_SostavAndCount()
        {
            try
            {
                return context.Database.SqlQuery<Prom_SostavAndCount>("SELECT " + sql_field_sostav + "," + sql_field_dt_pr + " as DT_PR" + "," + sql_field_dt + " as DT " +
                     ",(select count(v.N_VAG) from PROM.Vagon v where (v.N_NATUR=s.N_NATUR and v.D_PR_YY=s.D_YY and v.D_PR_MM=s.D_MM and v.D_PR_DD=s.D_DD and s.P_OT=0) or (v.N_NATUR=s.N_NATUR and v.D_SD_YY=s.D_YY and v.D_SD_MM=s.D_MM and v.D_SD_DD=s.D_DD and s.P_OT=1)) as countVagon " +
                     ",(select max(v.NPP) from PROM.Vagon v where (v.N_NATUR=s.N_NATUR and v.D_PR_YY=s.D_YY and v.D_PR_MM=s.D_MM and v.D_PR_DD=s.D_DD and s.P_OT=0) or (v.N_NATUR=s.N_NATUR and v.D_SD_YY=s.D_YY and v.D_SD_MM=s.D_MM and v.D_SD_DD=s.D_DD and s.P_OT=1)) as maxVagon " +
                     ",(select count(h.N_VAG) from PROM.Nat_Hist h where (h.N_NATUR=s.N_NATUR and h.D_PR_YY=s.D_YY and h.D_PR_MM=s.D_MM and h.D_PR_DD=s.D_DD and s.P_OT=0) or (h.N_NATUR=s.N_NATUR and h.D_SD_YY=s.D_YY and h.D_SD_MM=s.D_MM and h.D_SD_DD=s.D_DD and s.P_OT=1)) as countNatHist " +
                     ",(select max(h.NPP) from PROM.Nat_Hist h where (h.N_NATUR=s.N_NATUR and h.D_PR_YY=s.D_YY and h.D_PR_MM=s.D_MM and h.D_PR_DD=s.D_DD and s.P_OT=0) or (h.N_NATUR=s.N_NATUR and h.D_SD_YY=s.D_YY and h.D_SD_MM=s.D_MM and h.D_SD_DD=s.D_DD and s.P_OT=1)) as maxNatHist " +
                    sql_table_sostav).AsQueryable();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetProm_SostavAndCount()"), eventID);
                return null;
            }
        }
        /// <summary>
        /// Показать все составы и количество вагонов за указаный период времени для отчета "Оборот АМКР-УЗ"
        /// </summary>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        public IQueryable<Prom_SostavAndCount> GetProm_SostavAndCount(DateTime start, DateTime stop)
        {
            try
            {

                return context.Database.SqlQuery<Prom_SostavAndCount>("SELECT " + sql_field_sostav + "," + sql_field_dt_pr + " as DT_PR" + "," + sql_field_dt + " as DT " +
                     ",(select count(v.N_VAG) from PROM.Vagon v where (v.N_NATUR=s.N_NATUR and v.D_PR_YY=s.D_YY and v.D_PR_MM=s.D_MM and v.D_PR_DD=s.D_DD and s.P_OT=0) or (v.N_NATUR=s.N_NATUR and v.D_SD_YY=s.D_YY and v.D_SD_MM=s.D_MM and v.D_SD_DD=s.D_DD and s.P_OT=1)) as countVagon " +
                     ",(select max(v.NPP) from PROM.Vagon v where (v.N_NATUR=s.N_NATUR and v.D_PR_YY=s.D_YY and v.D_PR_MM=s.D_MM and v.D_PR_DD=s.D_DD and s.P_OT=0) or (v.N_NATUR=s.N_NATUR and v.D_SD_YY=s.D_YY and v.D_SD_MM=s.D_MM and v.D_SD_DD=s.D_DD and s.P_OT=1)) as maxVagon " +
                     ",(select count(h.N_VAG) from PROM.Nat_Hist h where (h.N_NATUR=s.N_NATUR and h.D_PR_YY=s.D_YY and h.D_PR_MM=s.D_MM and h.D_PR_DD=s.D_DD and s.P_OT=0) or (h.N_NATUR=s.N_NATUR and h.D_SD_YY=s.D_YY and h.D_SD_MM=s.D_MM and h.D_SD_DD=s.D_DD and s.P_OT=1)) as countNatHist " +
                     ",(select max(h.NPP) from PROM.Nat_Hist h where (h.N_NATUR=s.N_NATUR and h.D_PR_YY=s.D_YY and h.D_PR_MM=s.D_MM and h.D_PR_DD=s.D_DD and s.P_OT=0) or (h.N_NATUR=s.N_NATUR and h.D_SD_YY=s.D_YY and h.D_SD_MM=s.D_MM and h.D_SD_DD=s.D_DD and s.P_OT=1)) as maxNatHist " +
                     sql_table_sostav + " WHERE " + sql_field_dt + " >= TO_DATE('" + start.ToString("dd.MM.yyyy HH:mm") + "', 'dd.mm.yyyy hh24:mi') and " + sql_field_dt + " <= TO_DATE('" + stop.ToString("dd.MM.yyyy HH:mm") + "', 'dd.mm.yyyy hh24:mi')"
                     ).AsQueryable();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetProm_SostavAndCount(start={0}, stop={1})", start, stop), eventID);
                return null;
            }
        }
        /// <summary>
        /// Показать состав и количество вагонов для отчета поиск по натурному листу
        /// </summary>
        /// <param name="natur"></param>
        /// <param name="day"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <returns></returns>
        public IQueryable<Prom_SostavAndCount> GetProm_SostavAndCount(int? natur, int? day, int? month, int? year, int? hour, int? minute)
        {
            try
            {
                if (natur != null | day != null | month != null | year != null | hour != null | minute != null)
                {
                    string sql_where = " WHERE s.N_NATUR is not null";
                    if (natur != null)
                    {
                        sql_where += " and s.N_NATUR = " + natur.ToString();
                    };
                    if (day != null)
                    {
                        sql_where += " and s.D_DD = " + day.ToString();
                    };
                    if (month != null)
                    {
                        sql_where += " and s.D_MM = " + month.ToString();
                    };
                    if (year != null)
                    {
                        sql_where += " and s.D_YY = " + year.ToString();
                    };
                    if (hour != null)
                    {
                        sql_where += " and s.T_HH = " + hour.ToString();
                    };
                    if (minute != null)
                    {
                        sql_where += " and s.T_MI = " + minute.ToString();
                    };
                    return context.Database.SqlQuery<Prom_SostavAndCount>("SELECT " + sql_field_sostav + "," + sql_field_dt_pr + " as DT_PR" + "," + sql_field_dt + " as DT " +
                         ",(select count(v.N_VAG) from PROM.Vagon v where (v.N_NATUR=s.N_NATUR and v.D_PR_YY=s.D_YY and v.D_PR_MM=s.D_MM and v.D_PR_DD=s.D_DD and s.P_OT=0) or (v.N_NATUR=s.N_NATUR and v.D_SD_YY=s.D_YY and v.D_SD_MM=s.D_MM and v.D_SD_DD=s.D_DD and s.P_OT=1)) as countVagon " +
                         ",(select max(v.NPP) from PROM.Vagon v where (v.N_NATUR=s.N_NATUR and v.D_PR_YY=s.D_YY and v.D_PR_MM=s.D_MM and v.D_PR_DD=s.D_DD and s.P_OT=0) or (v.N_NATUR=s.N_NATUR and v.D_SD_YY=s.D_YY and v.D_SD_MM=s.D_MM and v.D_SD_DD=s.D_DD and s.P_OT=1)) as maxVagon " +
                         ",(select count(h.N_VAG) from PROM.Nat_Hist h where (h.N_NATUR=s.N_NATUR and h.D_PR_YY=s.D_YY and h.D_PR_MM=s.D_MM and h.D_PR_DD=s.D_DD and s.P_OT=0) or (h.N_NATUR=s.N_NATUR and h.D_SD_YY=s.D_YY and h.D_SD_MM=s.D_MM and h.D_SD_DD=s.D_DD and s.P_OT=1)) as countNatHist " +
                         ",(select max(h.NPP) from PROM.Nat_Hist h where (h.N_NATUR=s.N_NATUR and h.D_PR_YY=s.D_YY and h.D_PR_MM=s.D_MM and h.D_PR_DD=s.D_DD and s.P_OT=0) or (h.N_NATUR=s.N_NATUR and h.D_SD_YY=s.D_YY and h.D_SD_MM=s.D_MM and h.D_SD_DD=s.D_DD and s.P_OT=1)) as maxNatHist " +
                         sql_table_sostav + sql_where).AsQueryable();
                }
                else
                    return new List<Prom_SostavAndCount>().AsQueryable();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetProm_SostavAndCount(natur={0}, day={1}, month={2}, year={3}, hour={4}, minute={5})", natur, day, month, year, hour, minute), eventID);
                return null;
            }
        }

        #endregion

        #region Prom_NatHist
        //public IQueryable<Prom_NatHist> GetProm_NatHist()
        //{
        //    try
        //    {
        //        //return context.Database.SqlQuery<Prom_NatHist>("SELECT ROWNUM as ID,N_VAG,NPP,D_PR_DD,D_PR_MM,D_PR_YY,T_PR_HH,T_PR_MI,D_SD_DD,D_SD_MM,D_SD_YY,T_SD_HH,T_SD_MI,GODN,K_ST_KMK,K_POL_GR,K_GR,N_VED_PR,N_NAK_MPS,OTPRAV,PRIM_GR,WES_GR,N_NATUR,N_PUT,K_ST,K_OP,K_FRONT,N_NATUR_T,GODN_T,K_GR_T,WES_GR_T,K_OTPR_GR,K_ST_OTPR,K_ST_NAZN,ST_OTPR,ZADER,NEPR,UDOST,SERTIF,KOD_STRAN,KOD_SD,NETO,BRUTO,TARA,DAT_VVOD FROM PROM.NAT_HIST").AsQueryable();
        //        return context.Database.SqlQuery<Prom_NatHist>(sql_NatHist).AsQueryable();
        //    }
        //    catch (Exception e)
        //    {
        //        e.WriteErrorMethod(String.Format("GetProm_NatHist()"), eventID);
        //        return null;
        //    }
        //}
        /// <summary>
        /// Показать вагоны по прибытию по указаной натурке, дате и времени
        /// </summary>
        /// <param name="natur"></param>
        /// <param name="day"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <returns></returns>
        public IQueryable<Prom_NatHist> GetArrivalProm_NatHistOfNaturDateTime(int natur, int day, int month, int year, int hour, int minute)
        {
            try
            {
                return GetArrivalProm_NatHistOfNaturDateTime(natur, day, month, year, hour, minute, null);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetArrivalProm_NatHist(natur={0}, day={1}, month={2}, year={3}, hour={4}, minute={5})", natur, day, month, year, hour, minute), eventID);
                return null;
            }
        }
        /// <summary>
        /// Показать вагоны по прибытию по указаной натурке, дате и времени с сортировкой
        /// </summary>
        /// <param name="natur"></param>
        /// <param name="day"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public IQueryable<Prom_NatHist> GetArrivalProm_NatHistOfNaturDateTime(int natur, int day, int month, int year, int hour, int minute, bool? sort)
        {
            try
            {
                string sql = sql_nathist_select +
                    " WHERE h.N_NATUR = " + natur.ToString() +
                    " AND h.D_PR_DD = " + day.ToString() +
                    " AND h.D_PR_MM = " + month.ToString() +
                    " AND h.D_PR_YY = " + year.ToString() +
                    " AND h.T_PR_HH = " + hour.ToString() +
                    " AND h.T_PR_MI = " + minute.ToString();
                if (sort != null)
                {
                    sql += ((bool)sort ? " ORDER BY h.NPP DESC " : " ORDER BY h.NPP ");
                }
                return context.Database.SqlQuery<Prom_NatHist>(sql).AsQueryable();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetArrivalProm_NatHistOfNaturDateTime(natur={0}, day={1}, month={2}, year={3}, hour={4}, minute={5})", natur, day, month, year, hour, minute), eventID);
                return null;
            }
        }
        /// <summary>
        /// Показать вагоны по прибытию по указаной натурке, станции, дате с сортировкой
        /// </summary>
        /// <param name="natur"></param>
        /// <param name="station"></param>
        /// <param name="day"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public IQueryable<Prom_NatHist> GetArrivalProm_NatHistOfNaturStationDate(int natur, int station, int day, int month, int year, bool? sort)
        {
            try
            {
                string sql = sql_nathist_select +
                        " WHERE h.N_NATUR = " + natur.ToString() +
                        " AND h.K_ST = " + station.ToString() +
                        " AND h.D_PR_DD = " + day.ToString() +
                        " AND h.D_PR_MM = " + month.ToString() +
                        " AND h.D_PR_YY = " + year.ToString();
                if (sort != null)
                {
                    sql += ((bool)sort ? " ORDER BY h.NPP DESC " : " ORDER BY h.NPP ");
                }
                return context.Database.SqlQuery<Prom_NatHist>(sql).AsQueryable();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetArrivalProm_NatHistOfNaturStationDate(natur={0}, station={1}, day={2}, month={3}, year={4}, sort={5})", natur, station, day, month, year, sort), eventID);
                return null;
            }

        }
        /// <summary>
        /// Показать вагоны по прибытию по указаной натурке, станции, дате, номеру вагона
        /// </summary>
        /// <param name="natur"></param>
        /// <param name="station"></param>
        /// <param name="day"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public Prom_NatHist GetArrivalProm_NatHistOfNaturNumStationDate(int natur, int num_vag, int station, int day, int month, int year)
        {
            try
            {
                string sql = sql_nathist_select +
                        " WHERE h.N_NATUR = " + natur.ToString() +
                        " AND h.N_VAG = " + num_vag.ToString() +
                        " AND h.K_ST = " + station.ToString() +
                        " AND h.D_PR_DD = " + day.ToString() +
                        " AND h.D_PR_MM = " + month.ToString() +
                        " AND h.D_PR_YY = " + year.ToString();
                return context.Database.SqlQuery<Prom_NatHist>(sql).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetArrivalProm_NatHistOfNaturStationDateNum(natur={0}, num_vag={1}, station={2}, day={3}, month={4}, year={5})", natur, num_vag, station, day, month, year), eventID);
                return null;
            }

        }
        /// <summary>
        /// Показать список NatHist по указаному вагону меньше указанного времени с сортировкой
        /// </summary>
        /// <param name="num_vag"></param>
        /// <param name="start"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public IQueryable<Prom_NatHist> GetArrivalProm_NatHistOfVagonLess(int num_vag, DateTime start, bool sort)
        {
            try
            {
                int day = start.Day;
                int month = start.Month;
                int year = start.Year;
                int hour = start.Hour;
                int minute = start.Minute;
                string sql = sql_nathist_select +
                    "WHERE h.N_VAG = " + num_vag.ToString() + " AND (h.D_PR_YY < " + year.ToString() + " OR (h.D_PR_YY=" + year.ToString() + " AND h.D_PR_MM<" + month.ToString() + ") OR (h.D_PR_YY=" + year.ToString() + " AND h.D_PR_MM=" + month.ToString() + " AND h.D_PR_DD < " + day.ToString() + ") OR (h.D_PR_YY=" + year.ToString() + " AND h.D_PR_MM=" + month.ToString() + " AND h.D_PR_DD = " + day.ToString() + " AND h.T_PR_HH<" + hour.ToString() + ") OR (h.D_PR_YY=" + year.ToString() + " AND h.D_PR_MM=" + month.ToString() + " AND h.D_PR_DD = " + day.ToString() + " AND h.T_PR_HH=" + hour.ToString() + " AND h.T_PR_MI<" + minute.ToString() + ")) " +
                    (sort ? "order by h.D_PR_YY desc, h.D_PR_MM desc, h.D_PR_DD desc, h.T_PR_HH desc, h.T_PR_MI desc" : "order by h.D_PR_YY, h.D_PR_MM, h.D_PR_DD, h.T_PR_HH, h.T_PR_MI");

                return context.Database.SqlQuery<Prom_NatHist>(sql).AsQueryable();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetProm_NatHistOfVagonLessPR(num_vag={0}, start={1}, sort={2})", num_vag, start, sort), eventID);
                return null;
            }
        }
        /// <summary>
        /// Показать список NatHist по указаному вагону меньше или равно указанного времени с сортировкой
        /// </summary>
        /// <param name="num_vag"></param>
        /// <param name="start"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public IQueryable<Prom_NatHist> GetArrivalProm_NatHistOfVagonLessEqual(int num_vag, DateTime start, bool sort)
        {
            try
            {
                int day = start.Day;
                int month = start.Month;
                int year = start.Year;
                int hour = start.Hour;
                int minute = start.Minute;
                string sql = sql_nathist_select +
                    "WHERE h.N_VAG = " + num_vag.ToString() + " AND (h.D_PR_YY < " + year.ToString() + " OR (h.D_PR_YY=" + year.ToString() + " AND h.D_PR_MM<=" + month.ToString() + ") OR (h.D_PR_YY=" + year.ToString() + " AND h.D_PR_MM=" + month.ToString() + " AND h.D_PR_DD <= " + day.ToString() + ") OR (h.D_PR_YY=" + year.ToString() + " AND h.D_PR_MM=" + month.ToString() + " AND h.D_PR_DD = " + day.ToString() + " AND h.T_PR_HH<=" + hour.ToString() + ") OR (h.D_PR_YY=" + year.ToString() + " AND h.D_PR_MM=" + month.ToString() + " AND h.D_PR_DD = " + day.ToString() + " AND h.T_PR_HH=" + hour.ToString() + " AND h.T_PR_MI<=" + minute.ToString() + ")) " +
                    (sort ? "order by h.D_PR_YY desc, h.D_PR_MM desc, h.D_PR_DD desc, h.T_PR_HH desc, h.T_PR_MI desc" : "order by h.D_PR_YY, h.D_PR_MM, h.D_PR_DD, h.T_PR_HH, h.T_PR_MI");

                return context.Database.SqlQuery<Prom_NatHist>(sql).AsQueryable();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetProm_NatHistOfVagonLessEqualPR(num_vag={0}, start={1}, sort={2})", num_vag, start, sort), eventID);
                return null;
            }
        }

        /// <summary>
        /// Показать вагоны по отправке по указанной натурке, за указаное время
        /// </summary>
        /// <param name="natur"></param>
        /// <param name="day"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <returns></returns>
        public IQueryable<Prom_NatHist> GetSendingProm_NatHistOfNaturDateTime(int natur, int day, int month, int year, int hour, int minute)
        {
            try
            {
                return GetSendingProm_NatHistOfNaturDateTime(natur, day, month, year, hour, minute, null);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetSendingProm_NatHist(natur={0}, day={1}, month={2}, year={3}, hour={4}, minute={5})", natur, day, month, year, hour, minute), eventID);
                return null;
            }
        }
        /// <summary>
        /// Показать вагоны по отправке по указанной натурке, за указаное время с сортировкой
        /// </summary>
        /// <param name="natur"></param>
        /// <param name="day"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public IQueryable<Prom_NatHist> GetSendingProm_NatHistOfNaturDateTime(int natur, int day, int month, int year, int hour, int minute, bool? sort)
        {
            try
            {
                string sql = sql_nathist_select +
                    " WHERE h.N_NATUR = " + natur.ToString() +
                    " AND h.D_SD_DD = " + day.ToString() +
                    " AND h.D_SD_MM = " + month.ToString() +
                    " AND h.D_SD_YY = " + year.ToString() +
                    " AND h.T_SD_HH = " + hour.ToString() +
                    " AND h.T_SD_MI = " + minute.ToString();
                if (sort != null)
                {
                    sql += ((bool)sort ? " ORDER BY h.NPP DESC " : " ORDER BY h.NPP ");
                }
                return context.Database.SqlQuery<Prom_NatHist>(sql).AsQueryable();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetSendingProm_NatHist(natur={0}, day={1}, month={2}, year={3}, hour={4}, minute={5})", natur, day, month, year, hour, minute), eventID);
                return null;
            }
        }
        /// <summary>
        /// Показать отправленные вагоны по указаной натурке, дате с сортировкой
        /// </summary>
        /// <param name="natur"></param>
        /// <param name="day"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public IQueryable<Prom_NatHist> GetSendingProm_NatHistOfNaturDate(int natur, int day, int month, int year, bool? sort)
        {
            try
            {
                string sql = sql_nathist_select +
                        " WHERE h.N_NATUR = " + natur.ToString() +
                        " AND h.D_SD_DD = " + day.ToString() +
                        " AND h.D_SD_MM = " + month.ToString() +
                        " AND h.D_SD_YY = " + year.ToString();
                if (sort != null)
                {
                    sql += ((bool)sort ? " ORDER BY h.NPP DESC " : " ORDER BY h.NPP ");
                }
                return context.Database.SqlQuery<Prom_NatHist>(sql).AsQueryable();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetSendingProm_NatHistOfNaturStationDate(natur={0}, day={1}, month={2}, year={3}, sort={4})", natur, day, month, year, sort), eventID);
                return null;
            }

        }
        /// <summary>
        /// Найти вагон по натурке, номеру, дате и времени
        /// </summary>
        /// <param name="natur"></param>
        /// <param name="num"></param>
        /// <param name="day"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <returns></returns>
        public Prom_NatHist GetSendingProm_NatHistOfNaturNumDateTime(int natur, int num, int day, int month, int year, int hour, int minute)
        {
            try
            {
                string sql = sql_nathist_select +
                        " WHERE h.N_NATUR = " + natur.ToString() +
                        " AND h.N_VAG = " + num.ToString() +
                        " AND h.D_SD_DD = " + day.ToString() +
                        " AND h.D_SD_MM = " + month.ToString() +
                        " AND h.D_SD_YY = " + year.ToString() +
                        " AND h.T_SD_HH = " + hour.ToString() +
                        " AND h.T_SD_MI = " + minute.ToString();
                return context.Database.SqlQuery<Prom_NatHist>(sql).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetSendingProm_NatHistOfNaturNumDateTime(natur={0}, num={1}, day={2}, month={3}, year={4}, hour={5}, minute={6})", 
                    natur, num, day, month, year, year, minute), eventID);
                return null;
            }

        }
        /// <summary>
        /// Найти вагон по номеру, дате и времени
        /// </summary>
        /// <param name="num"></param>
        /// <param name="day"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <returns></returns>
        public Prom_NatHist GetSendingProm_NatHistOfNumDateTime(int num, int day, int month, int year, int hour, int minute)
        {
            try
            {
                string sql = sql_nathist_select +
                        " WHERE h.N_VAG = " + num.ToString() +
                        " AND h.D_SD_DD = " + day.ToString() +
                        " AND h.D_SD_MM = " + month.ToString() +
                        " AND h.D_SD_YY = " + year.ToString() +
                        " AND h.T_SD_HH = " + hour.ToString() +
                        " AND h.T_SD_MI = " + minute.ToString();
                return context.Database.SqlQuery<Prom_NatHist>(sql).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetSendingProm_NatHistOfNaturNumDateTime(num={0}, day={1}, month={2}, year={3}, hour={4}, minute={5})", 
                    num, day, month, year, year, minute), eventID);
                return null;
            }

        }
        /// <summary>
        /// Показать список NatHist по указаному вагону больше указанного времени с сортировкой
        /// </summary>
        /// <param name="num_vag"></param>
        /// <param name="start"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public IQueryable<Prom_NatHist> GetSendingProm_NatHistOfVagonMore(int num_vag, DateTime start, bool sort)
        {
            try
            {
                int day = start.Day;
                int month = start.Month;
                int year = start.Year;
                int hour = start.Hour;
                int minute = start.Minute;
                string sql = sql_nathist_select +
                    "WHERE h.N_VAG = " + num_vag.ToString() + " AND (h.D_SD_YY > " + year.ToString() + " OR (h.D_SD_YY=" + year.ToString() + " AND h.D_SD_MM>" + month.ToString() + ") OR (h.D_SD_YY=" + year.ToString() + " AND h.D_SD_MM=" + month.ToString() + " AND h.D_SD_DD > " + day.ToString() + ") OR (h.D_SD_YY=" + year.ToString() + " AND h.D_SD_MM=" + month.ToString() + " AND h.D_SD_DD = " + day.ToString() + " AND h.T_SD_HH>" + hour.ToString() + ") OR (h.D_SD_YY=" + year.ToString() + " AND h.D_SD_MM=" + month.ToString() + " AND h.D_SD_DD = " + day.ToString() + " AND h.T_SD_HH=" + hour.ToString() + " AND h.T_SD_MI>" + minute.ToString() + ")) " +
                    (sort ? "order by h.D_SD_YY desc, h.D_SD_MM desc, h.D_SD_DD desc, h.T_SD_HH desc, h.T_SD_MI desc" : "order by h.D_SD_YY, h.D_SD_MM, h.D_SD_DD, h.T_SD_HH, h.T_SD_MI");

                return context.Database.SqlQuery<Prom_NatHist>(sql).AsQueryable();

            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetProm_NatHistOfVagonMoreSD(num_vag={0}, start={1}, sort={2})", num_vag, start, sort), eventID);
                return null;
            }
        }
        /// <summary>
        /// Показать список NatHist по указаному вагону больше или равно указанного времени с сортировкой
        /// </summary>
        /// <param name="num_vag"></param>
        /// <param name="start"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public IQueryable<Prom_NatHist> GetSendingProm_NatHistOfVagonMoreEqual(int num_vag, DateTime start, bool sort)
        {
            try
            {
                int day = start.Day;
                int month = start.Month;
                int year = start.Year;
                int hour = start.Hour;
                int minute = start.Minute;
                string sql = sql_nathist_select +
                    "WHERE h.N_VAG = " + num_vag.ToString() + " AND (h.D_SD_YY > " + year.ToString() + " OR (h.D_SD_YY=" + year.ToString() + " AND h.D_SD_MM>=" + month.ToString() + ") OR (h.D_SD_YY=" + year.ToString() + " AND h.D_SD_MM=" + month.ToString() + " AND h.D_SD_DD >= " + day.ToString() + ") OR (h.D_SD_YY=" + year.ToString() + " AND h.D_SD_MM=" + month.ToString() + " AND h.D_SD_DD = " + day.ToString() + " AND h.T_SD_HH>=" + hour.ToString() + ") OR (h.D_SD_YY=" + year.ToString() + " AND h.D_SD_MM=" + month.ToString() + " AND h.D_SD_DD = " + day.ToString() + " AND h.T_SD_HH=" + hour.ToString() + " AND h.T_SD_MI>=" + minute.ToString() + ")) " +
                    (sort ? "order by h.D_SD_YY desc, h.D_SD_MM desc, h.D_SD_DD desc, h.T_SD_HH desc, h.T_SD_MI desc" : "order by h.D_SD_YY, h.D_SD_MM, h.D_SD_DD, h.T_SD_HH, h.T_SD_MI");

                return context.Database.SqlQuery<Prom_NatHist>(sql).AsQueryable();

            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetProm_NatHistOfVagonMoreEqualSD(num_vag={0}, start={1}, sort={2})", num_vag, start, sort), eventID);
                return null;
            }
        }


        #endregion

        #region Prom_NatHistAndSostav

        public IQueryable<Prom_NatHistAndSostav> GetProm_NatHistAndSostav()
        {
            try
            {
                return context.Database.SqlQuery<Prom_NatHistAndSostav>(sql_nathist_sostav_select).AsQueryable();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetProm_NatHistAndSostav()"), eventID);
                return null;
            }
        }

        public IQueryable<Prom_NatHistAndSostav> GetProm_NatHistAndSostav(int num)
        {
            try
            {
                //string sql = sql_nathist_sostav_select + " where h.N_VAG = " + num.ToString() + " order by D_YY desc, D_MM desc, D_DD desc, T_HH desc, T_MI desc";
                return context.Database.SqlQuery<Prom_NatHistAndSostav>(sql_nathist_sostav_select + " where h.N_VAG = " + num.ToString() + " order by D_YY desc, D_MM desc, D_DD desc, T_HH desc, T_MI desc").AsQueryable();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetProm_NatHistAndSostav(num={0})", num), eventID);
                return null;
            }
        }
        #endregion

        #region Prom_Vagon

        //public IQueryable<Prom_Vagon> GetProm_Vagon()
        //{
        //    try
        //    {
        //        return context.Database.SqlQuery<Prom_Vagon>(sql_Vagon).AsQueryable();
        //    }
        //    catch (Exception e)
        //    {
        //        e.WriteErrorMethod(String.Format("GetProm_Vagon()"), eventID);
        //        return null;
        //    }
        //}
        /// <summary>
        /// Создать Prom_Vagon по Prom_NatHist
        /// </summary>
        /// <param name="pnh"></param>
        /// <returns></returns>
        public Prom_Vagon CreateProm_Vagon(Prom_NatHist pnh)
        {
            if (pnh == null) return null;
            return new Prom_Vagon()
            {
                ID = pnh.ID,
                DT_PR = pnh.DT_PR,
                DT_SD = pnh.DT_SD,
                D_PR_DD = pnh.D_PR_DD,
                D_PR_MM = pnh.D_PR_MM,
                D_PR_YY = pnh.D_PR_YY,
                T_PR_HH = pnh.T_PR_HH,
                T_PR_MI = pnh.T_PR_MI,
                //
                D_SD_DD = pnh.D_SD_DD,
                D_SD_MM = pnh.D_SD_MM,
                D_SD_YY = pnh.D_SD_YY,
                T_SD_HH = pnh.T_SD_HH,
                T_SD_MI = pnh.T_SD_MI,
                //
                N_VAG = pnh.N_VAG,
                NPP = pnh.NPP,
                GODN = pnh.GODN,
                K_ST = pnh.K_ST,
                K_ST_KMK = pnh.K_ST_KMK,
                K_POL_GR = pnh.K_POL_GR,
                N_VED_PR = pnh.N_VED_PR,
                //
                N_NAK_MPS = pnh.N_NAK_MPS,
                OTPRAV = pnh.OTPRAV,
                PRIM_GR = pnh.PRIM_GR,
                K_GR = pnh.K_GR,
                WES_GR = pnh.WES_GR,
                N_NATUR = pnh.N_NATUR,
                N_PUT = pnh.N_PUT,
                K_OP = pnh.K_OP,
                K_FRONT = pnh.K_FRONT,
                N_NATUR_T = pnh.N_NATUR_T,
                GODN_T = pnh.GODN_T,
                K_GR_T = pnh.K_GR_T,
                WES_GR_T = pnh.WES_GR_T,
                K_OTPR_GR = pnh.K_OTPR_GR,
                K_ST_NAZN = pnh.K_ST_NAZN,
                K_ST_OTPR = pnh.K_ST_OTPR,
                ST_OTPR = pnh.ST_OTPR,
                ZADER = pnh.ZADER,
                NEPR = pnh.NEPR,
                UDOST = pnh.UDOST,
                SERTIF = pnh.SERTIF,
                KOD_STRAN = pnh.KOD_STRAN,
                KOD_SD = pnh.KOD_SD,
                NETO = pnh.NETO,
                BRUTO = pnh.BRUTO,
                TARA = pnh.TARA,
                DAT_VVOD = pnh.DAT_VVOD
            };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="natur"></param>
        /// <param name="station"></param>
        /// <param name="day"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public IQueryable<Prom_Vagon> GetArrivalProm_VagonOfNaturStationDate(int natur, int station, int day, int month, int year, bool? sort)
        {
            try
            {
                string sql = sql_vagon_select +
                    " WHERE v.N_NATUR = " + natur.ToString() +
                    " AND v.K_ST = " + station.ToString() +
                    " AND v.D_PR_DD = " + day.ToString() +
                    " AND v.D_PR_MM = " + month.ToString() +
                    " AND v.D_PR_YY = " + year.ToString();
                if (sort != null)
                {
                    sql += ((bool)sort ? " ORDER BY v.NPP DESC " : " ORDER BY v.NPP ");
                }
                return context.Database.SqlQuery<Prom_Vagon>(sql).AsQueryable();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetArrivalProm_VagonOfNaturStationDate(natur={0}, station={1}, day={2}, month={3}, year={4}, sort={5})", natur, station, day, month, year, sort), eventID);
                return null;
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="natur"></param>
        /// <param name="station"></param>
        /// <param name="day"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public IQueryable<Prom_Vagon> GetArrivalProm_VagonOfNaturStationDate(int natur, int station, int day, int month, int year)
        {
            try
            {
                return GetArrivalProm_VagonOfNaturStationDate(natur,station,day,month,year,null);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetArrivalProm_VagonOfNaturStationDate(natur={0}, station={1}, day={2}, month={3}, year={4})", natur, station, day, month, year), eventID);
                return null;
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="natur"></param>
        /// <param name="num_vag"></param>
        /// <param name="station"></param>
        /// <param name="day"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public Prom_Vagon GetArrivalProm_VagonOfNaturNumStationDate(int natur, int num_vag, int station, int day, int month, int year)
        {
            try
            {
                string sql = sql_vagon_select +
                    " WHERE v.N_NATUR = " + natur.ToString() +
                    " AND v.N_VAG = " + num_vag.ToString() +
                    " AND v.K_ST = " + station.ToString() +
                    " AND v.D_PR_DD = " + day.ToString() +
                    " AND v.D_PR_MM = " + month.ToString() +
                    " AND v.D_PR_YY = " + year.ToString();
                return context.Database.SqlQuery<Prom_Vagon>(sql).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetArrivalProm_VagonOfNaturStationDate(natur={0}, station={1}, day={2}, month={3}, year={4})", natur, station, day, month, year), eventID);
                return null;
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="natur"></param>
        /// <param name="day"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <returns></returns>
        public IQueryable<Prom_Vagon> GetArrivalProm_VagonOfNaturDateTime(int natur, int day, int month, int year, int hour, int minute, bool? sort)
        {
            try
            {
                string sql = sql_vagon_select +
                " WHERE v.N_NATUR = " + natur.ToString() +
                " AND v.D_PR_DD = " + day.ToString() +
                " AND v.D_PR_MM = " + month.ToString() +
                " AND v.D_PR_YY = " + year.ToString() +
                " AND v.T_PR_HH = " + hour.ToString() +
                " AND v.T_PR_MI = " + minute.ToString();
                if (sort != null)
                {
                    sql += ((bool)sort ? " ORDER BY v.NPP DESC " : " ORDER BY v.NPP ");
                }
                return context.Database.SqlQuery<Prom_Vagon>(sql).AsQueryable();
                //return context.Database.SqlQuery<Prom_Vagon>(sql_Vagon + " WHERE N_NATUR = " + natur.ToString() + " AND D_PR_DD = " + day.ToString() + " AND D_PR_MM = " + month.ToString() + " AND D_PR_YY = " + year.ToString() + " AND T_PR_HH = " + hour.ToString() + " AND T_PR_MI = " + minute.ToString() + " ORDER BY NPP").AsQueryable();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetArrivalProm_VagonOfNaturDateTime(natur={0}, day={1}, month={2}, year={3}, hour={4}, minute={5})", natur, day, month, year, hour, minute), eventID);
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="natur"></param>
        /// <param name="day"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <returns></returns>
        public IQueryable<Prom_Vagon> GetSendingProm_VagonOfNaturDateTime(int natur, int day, int month, int year, int hour, int minute, bool? sort)
        {
            try
            {
                string sql = sql_vagon_select +
                " WHERE v.N_NATUR = " + natur.ToString() +
                " AND v.D_SD_DD = " + day.ToString() +
                " AND v.D_SD_MM = " + month.ToString() +
                " AND v.D_SD_YY = " + year.ToString() +
                " AND v.T_SD_HH = " + hour.ToString() +
                " AND v.T_SD_MI = " + minute.ToString();
                if (sort != null)
                {
                    sql += ((bool)sort ? " ORDER BY v.NPP DESC " : " ORDER BY v.NPP ");
                }
                return context.Database.SqlQuery<Prom_Vagon>(sql).AsQueryable();                
                //return context.Database.SqlQuery<Prom_Vagon>("SELECT ROWNUM as ID,N_VAG,NPP,D_PR_DD,D_PR_MM,D_PR_YY,T_PR_HH,T_PR_MI,D_SD_DD,D_SD_MM,D_SD_YY,T_SD_HH,T_SD_MI,GODN,K_ST_KMK,K_POL_GR,K_GR,N_VED_PR,N_NAK_MPS,OTPRAV,PRIM_GR,WES_GR,N_NATUR,N_PUT,K_ST,K_OP,K_FRONT,N_NATUR_T,GODN_T,K_GR_T,WES_GR_T,K_OTPR_GR,K_ST_OTPR,K_ST_NAZN,ST_OTPR,ZADER,NEPR,UDOST,SERTIF,KOD_STRAN,KOD_SD,NETO,BRUTO,TARA,DAT_VVOD FROM PROM.VAGON "+
                //return context.Database.SqlQuery<Prom_Vagon>(sql_Vagon + " WHERE N_NATUR = " + natur.ToString() + " AND D_SD_DD = " + day.ToString() + " AND D_SD_MM = " + month.ToString() + " AND D_SD_YY = " + year.ToString() + " AND T_SD_HH = " + hour.ToString() + " AND T_SD_MI = " + minute.ToString() + " ORDER BY NPP").AsQueryable();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetSendingProm_VagonOfNaturDateTime(natur={0}, day={1}, month={2}, year={3}, hour={4}, minute={5})", natur, day, month, year, hour, minute), eventID);
                return null;
            }
        }

        #endregion

        #region Prom_VagonAndSostav

        public IQueryable<Prom_VagonAndSostav> GetProm_VagonAndSostav()
        {
            try
            {
                return context.Database.SqlQuery<Prom_VagonAndSostav>(sql_vagon_sostav_select).AsQueryable();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetProm_VagonAndSostavn()"), eventID);
                return null;
            }
        }

        public IQueryable<Prom_VagonAndSostav> GetProm_VagonAndSostav(int num)
        {
            try
            {
                return context.Database.SqlQuery<Prom_VagonAndSostav>(sql_vagon_sostav_select + " where v.N_VAG = " + num.ToString() + " order by D_YY desc, D_MM desc, D_DD desc, T_HH desc, T_MI desc").AsQueryable();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetProm_VagonAndSostavn(num={0})", num), eventID);
                return null;
            }
        }
        #endregion

        #region PROM.CEX
        public IQueryable<PromCex> PromCex
        {
            get { return context.PromCex; }
        }
        /// <summary>
        /// Получить перечень всех цехов
        /// </summary>
        /// <returns></returns>
        public IQueryable<PromCex> GetCex()
        {
            try
            {
                return PromCex;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetCex()"), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить цех по id
        /// </summary>
        /// <param name="k_podr"></param>
        /// <returns></returns>
        public PromCex GetCex(int k_podr)
        {
            try
            {
                return GetCex().Where(c => c.K_PODR == k_podr).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetCex(k_podr={0})", k_podr), eventID);
                return null;
            }
        }
        #endregion

        #endregion


        #region NUM_VAG (Информация по вагонам)

        #region NumVagStpr1Gr (Справочник грузов по вагонам)
        /// <summary>
        /// список грузов
        /// </summary>
        /// <returns></returns>
        public IQueryable<NumVagStpr1Gr> GetSTPR1GR()
        {
            try
            {
                return context.NumVagStpr1Gr;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetSTPR1GR()"), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить груз по kod_gr
        /// </summary>
        /// <param name="kod_gr"></param>
        /// <returns></returns>
        public NumVagStpr1Gr GetSTPR1GR(int kod_gr)
        {
            try
            {
                return GetSTPR1GR().Where(g => g.KOD_GR == kod_gr).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetSTPR1GR(kod_gr={0})", kod_gr), eventID);
                return null;
            }
        }
        #endregion

        #region NumVagStpr1InStDoc (Составы по прибытию)

        public IQueryable<NumVagStpr1InStDoc> NumVagStpr1InStDoc
        {
            get { return context.NumVagStpr1InStDoc; }
        }
        /// <summary>
        /// Получить список операций перемещений по прибытию с других станций
        /// </summary>
        /// <returns></returns>
        public IQueryable<NumVagStpr1InStDoc> GetSTPR1InStDoc()
        {
            try
            {
                return NumVagStpr1InStDoc;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetSTPR1InStDoc()"), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить строку по номеру документа
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public NumVagStpr1InStDoc GetSTPR1InStDoc(int doc)
        {
            try
            {
                return GetSTPR1InStDoc().Where(i => i.ID_DOC == doc).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetSTPR1InStDoc(doc={0})", doc), eventID);
                return null;
            }
        }
        /// <summary>
        /// Показать за указаный период
        /// </summary>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        public IQueryable<NumVagStpr1InStDoc> GetSTPR1InStDoc(DateTime start, DateTime stop)
        {
            try
            {
                return GetSTPR1InStDoc().Where(v => v.DATE_IN_ST >= start & v.DATE_IN_ST <= stop);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetSTPR1InStDoc(start={0}, stop={1})", start, stop), eventID);
                return null;
            }
        }
        /// <summary>
        /// Показать за указаный период с сортировкой
        /// </summary>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public IQueryable<NumVagStpr1InStDoc> GetSTPR1InStDoc(DateTime start, DateTime stop, bool order)
        {
            try
            {
                if (order)
                { return GetSTPR1InStDoc(start, stop).OrderByDescending(i => i.DATE_IN_ST); }
                else
                { return GetSTPR1InStDoc(start, stop).OrderBy(i => i.DATE_IN_ST); }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetSTPR1InStDoc(start={0}, stop={1}, order={2})", start, stop, order), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить список операций перемещений по прибытию с других станций c дополнительной выборкой
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public IQueryable<NumVagStpr1InStDoc> GetSTPR1InStDocOfAmkr(string where)
        {
            try
            {
                string sql = "select a.* from NUM_VAG.STPR1_IN_ST_DOC a inner join NUM_VAG.STAN b on a.ST_IN_ST=b.K_STAN and b.MPS=0 " + (!String.IsNullOrWhiteSpace(where) ? " WHERE " + where : "");
                return context.Database.SqlQuery<NumVagStpr1InStDoc>(sql).AsQueryable();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetSTPR1InStDocOfAmkr(where={0})", where), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить список операций перемещений по прибытию с других станций на станции АМКР
        /// </summary>
        /// <returns></returns>
        public IQueryable<NumVagStpr1InStDoc> GetSTPR1InStDocOfAmkr()
        { return GetSTPR1InStDocOfAmkr(null); }
        #endregion


        #region NumVag_Stpr1OutStVag

        public IQueryable<NumVag_Stpr1OutStVag> GetNumVag_Stpr1OutStVag()
        {
            try
            {
                return context.Database.SqlQuery<NumVag_Stpr1OutStVag>(sql_NumVagStpr1OutStVag_select).AsQueryable();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetNumVag_Stpr1OutStVag()"), eventID);
                return null;
            }
        }

        public IQueryable<NumVag_Stpr1OutStVag> GetNumVag_Stpr1OutStVag(int natur, bool napr)
        {
            try
            {
                return context.Database.SqlQuery<NumVag_Stpr1OutStVag>(sql_NumVagStpr1OutStVag_select + " where v.ID_DOC = " + natur.ToString() + " order by N_OUT_ST" + (napr ? " desc" : "")).AsQueryable();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetNumVag_Stpr1OutStVag(num={0}, napr={1})", natur, napr), eventID);
                return null;
            }
        }

        #endregion

        #region NumVagStpr1InStVag (вагоны по прибытию)
        public IQueryable<NumVagStpr1InStVag> NumVagStpr1InStVag
        {
            get { return context.NumVagStpr1InStVag; }
        }
        /// <summary>
        /// Получить список вагонов перемещений по внутреним станциям по прибытию
        /// </summary>
        /// <returns></returns>
        public IQueryable<NumVagStpr1InStVag> GetSTPR1InStVag()
        {
            try
            {
                return NumVagStpr1InStVag;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetSTPR1InStVag()"), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить список вагонов перемещений по внутреним станциям по прибытию по указаномку документу
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public IQueryable<NumVagStpr1InStVag> GetSTPR1InStVag(int doc)
        {
            try
            {
                return GetSTPR1InStVag().Where(v => v.ID_DOC == doc);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetSTPR1InStVag(doc={0})", doc), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить список вагонов перемещений по внутреним станциям по прибытию по указаному документу c сортировкой вагонов по порядку прибывания
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public IQueryable<NumVagStpr1InStVag> GetSTPR1InStVag(int doc, bool sort)
        {
            if (sort) { return GetSTPR1InStVag(doc).OrderByDescending(v => v.N_IN_ST); }
            else { return GetSTPR1InStVag(doc).OrderBy(v => v.N_IN_ST); }
        }
        /// <summary>
        /// Получить количество вагонов
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public int GetCountSTPR1InStVag(int doc)
        {
            return GetSTPR1InStVag(doc).Count();
        }
        #endregion

        #region NumVagStpr1OutStDoc (Составы по отправке)
        /// <summary>
        /// 
        /// </summary>
        public IQueryable<NumVagStpr1OutStDoc> NumVagStpr1OutStDoc
        {
            get { return context.NumVagStpr1OutStDoc; }
        }
        /// <summary>
        /// Показать все составы
        /// </summary>
        /// <returns></returns>
        public IQueryable<NumVagStpr1OutStDoc> GetSTPR1OutStDoc()
        {
            try
            {
                return NumVagStpr1OutStDoc;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetSTPR1OutStDoc()"), eventID);
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        public NumVagStpr1OutStDoc GetSTPR1OutStDoc(int doc)
        {
            try
            {
                return GetSTPR1OutStDoc().Where(v => v.ID_DOC == doc).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetSTPR1OutStDoc(doc={0})", doc), eventID);
                return null;
            }
        }
        /// <summary>
        /// Показать отправленные составы за указанное время
        /// </summary>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        public IQueryable<NumVagStpr1OutStDoc> GetSTPR1OutStDoc(DateTime start, DateTime stop)
        {
            try
            {
                return GetSTPR1OutStDoc().Where(v => v.DATE_OUT_ST >= start & v.DATE_OUT_ST <= stop);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetSTPR1OutStDoc(start={0}, stop={1})", start, stop), eventID);
                return null;
            }
        }
        /// <summary>
        /// Показать за указаный период с сортировкой
        /// </summary>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public IQueryable<NumVagStpr1OutStDoc> GetSTPR1OutStDoc(DateTime start, DateTime stop, bool order)
        {
            try
            {
                if (order)
                { return GetSTPR1OutStDoc(start, stop).OrderByDescending(i => i.DATE_OUT_ST); }
                else
                { return GetSTPR1OutStDoc(start, stop).OrderBy(i => i.DATE_OUT_ST); }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetSTPR1OutStDoc(start={0}, stop={1}, order={2})", start, stop, order), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить список операций перемещений по отправке с других станций c дополнительной выборкой
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public IQueryable<NumVagStpr1OutStDoc> GetSTPR1OutStDocOfAmkr(string where)
        {
            try
            {
                string sql = "select a.* from NUM_VAG.STPR1_OUT_ST_DOC a inner join NUM_VAG.STAN b on a.st_out_st=b.K_STAN and b.MPS=0 " + (!String.IsNullOrWhiteSpace(where) ? " WHERE " + where : "");
                return context.Database.SqlQuery<NumVagStpr1OutStDoc>(sql).AsQueryable();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetSTPR1OutStDocOfAmkr(where={0})", where), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить список операций перемещений по отправке с других станций на станции АМКР
        /// </summary>
        /// <returns></returns>
        public IQueryable<NumVagStpr1OutStDoc> GetSTPR1OutStDocOfAmkr()
        { return GetSTPR1OutStDocOfAmkr(null); }
        #endregion

        #region NumVagStpr1OutStVag (вагоны по отправке)
        /// <summary>
        /// 
        /// </summary>
        public IQueryable<NumVagStpr1OutStVag> NumVagStpr1OutStVag
        {
            get { return context.NumVagStpr1OutStVag; }
        }
        /// <summary>
        /// Показать все вагоны
        /// </summary>
        /// <returns></returns>
        public IQueryable<NumVagStpr1OutStVag> GetSTPR1OutStVag()
        {
            try
            {
                return NumVagStpr1OutStVag;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetSTPR1OutStVag()"), eventID);
                return null;
            }
        }
        /// <summary>
        /// Вернуть список вагонов по номеру документа
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public IQueryable<NumVagStpr1OutStVag> GetSTPR1OutStVag(int doc)
        {
            try
            {
                return GetSTPR1OutStVag().Where(v => v.ID_DOC == doc);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetSTPR1OutStVag(doc={0})", doc), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить список вагонов перемещений по внутреним станциям по отправке по указаному документу c сортировкой вагонов по порядку прибывания
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public IQueryable<NumVagStpr1OutStVag> GetSTPR1OutStVag(int doc, bool sort)
        {
            if (sort) { return GetSTPR1OutStVag(doc).OrderByDescending(v => v.N_OUT_ST); }
            else { return GetSTPR1OutStVag(doc).OrderBy(v => v.N_OUT_ST); }
        }
        /// <summary>
        /// Вернуть ко вагонов по номеру документа
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public int GetCountSTPR1OutStVag(int doc)
        {
            try
            {
                return GetSTPR1InStVag(doc).Count();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetCountSTPR1OutStVag(doc={0})", doc), eventID);
                return -1;
            }
        }

        public IQueryable<NumVagStpr1OutStVag> GetSTPR1OutStDocOfCarAndDoc(int[] nums_car, int[] nums_doc)
        {
            try
            {

                string s_nums_car = nums_car.IntsToString(',');
                string s_nums_doc = nums_doc.IntsToString(',');
                string sql = " SELECT *  FROM NUM_VAG.STPR1_OUT_ST_VAG where ID_DOC in (" + s_nums_doc + ")  and N_VAG in (" + s_nums_car + ")";
                return context.Database.SqlQuery<NumVagStpr1OutStVag>(sql).AsQueryable();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetSTPR1OutStDocOfCarAndDoc(nums_car={0}, nums_doc={1})", nums_car.IntsToString(','), nums_doc.IntsToString(',')), eventID);
                return null;
            }
        }

        #endregion

        #region NumVagStan (справочник станций)

        public IQueryable<NumVagStan> NumVagStan
        {
            get { return context.NumVagStan; }
        }
        /// <summary>
        /// Получить все станции КИС
        /// </summary>
        /// <returns></returns>
        public IQueryable<NumVagStan> GetNumVagStations()
        {
            return NumVagStan;
        }
        /// <summary>
        /// Получить информацию по станции по  id
        /// </summary>
        /// <param name="id_stan"></param>
        /// <returns></returns>
        public NumVagStan GetNumVagStations(int id_stan)
        {
            return GetNumVagStations().Where(s => s.K_STAN == id_stan).FirstOrDefault();
        }
        #endregion

        #region NumVagStpr1Tupik (справочник тупиков)

        public IQueryable<NumVagStpr1Tupik> NumVagStpr1Tupik
        {
            get { return context.NumVagStpr1Tupik; }
        }
        /// <summary>
        /// Получить все тупики
        /// </summary>
        /// <returns></returns>
        public IQueryable<NumVagStpr1Tupik> GetNumVagStpr1Tupik()
        {
            return NumVagStpr1Tupik;
        }
        /// <summary>
        /// Получить информацию по станции по  id
        /// </summary>
        /// <param name="id_stan"></param>
        /// <returns></returns>
        public NumVagStpr1Tupik GetNumVagStpr1Tupik(int id_tupik)
        {
            return GetNumVagStpr1Tupik().Where(t => t.ID_CEX_TUPIK == id_tupik).FirstOrDefault();
        }
        #endregion

        #region NumVagStran (справочник стран)
        public IQueryable<NumVagStran> NumVagStran
        {
            get { return context.NumVagStran; }
        }
        /// <summary>
        /// Получить все страны
        /// </summary>
        /// <returns></returns>
        public IQueryable<NumVagStran> GetNumVagStran()
        {
            return NumVagStran;
        }
        /// <summary>
        /// Получить информацию по стране по  id
        /// </summary>
        /// <param name="id_stan"></param>
        /// <returns></returns>
        public NumVagStran GetNumVagStran(int npp)
        {
            return GetNumVagStran().Where(s => s.NPP == npp).FirstOrDefault();
        }
        /// <summary>
        /// Получить информацию по стране по коду iso
        /// </summary>
        /// <param name="id_stan"></param>
        /// <returns></returns>
        public NumVagStran GetNumVagStranOfCodeISO(int code)
        {
            return GetNumVagStran().Where(s => s.KOD_STRAN == code).FirstOrDefault();
        }
        /// <summary>
        /// Получить информацию по стране по коду europe
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public NumVagStran GetNumVagStranOfCodeEurope(int code)
        {
            return GetNumVagStran().Where(s => s.KOD_EUROP == code).FirstOrDefault();
        }
        /// <summary>
        /// Получить информацию по стране по коду ЕТ СНГ (олд)
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public NumVagStran GetNumVagStranOfCodeSNG(int? code)
        {
            return GetNumVagStran().Where(s => s.KOD_OLD == code).FirstOrDefault();
        }
        #endregion

        #endregion

    }
}
//#region PROM.Vagon

//public IQueryable<PromVagon> PromVagon
//{
//    get { return context.PromVagon; }
//}
///// <summary>
///// Получить список всех вагонов станции Промышленная
///// </summary>
///// <returns></returns>
//public IQueryable<PromVagon> GetVagon()
//{
//    try
//    {
//        return PromVagon;
//    }
//    catch (Exception e)
//    {
//        e.WriteErrorMethod(String.Format("GetVagon()"), eventID);
//        return null;
//    }
//}
//GetArrivalProm_VagonOfNaturStationDate
// <summary>
// Получить список вагонов по натурному листу станции и дате поступления c сортировкой true- npp по убыванию false- npp по возрастанию
// </summary>
// <param name="natur"></param>
// <param name="station"></param>
// <param name="day"></param>
// <param name="month"></param>
// <param name="year"></param>
// <returns></returns>
//public IQueryable<PromVagon> GetVagon(int natur, int station, int day, int month, int year, bool? sort)
//{
//    try
//    {
//        if (sort == null)
//        {
//            return GetVagon().Where(n => n.N_NATUR == natur & n.K_ST == station & n.D_PR_DD == day & n.D_PR_MM == month & n.D_PR_YY == year);
//        }
//        if ((bool)sort)
//        {
//            return GetVagon().Where(n => n.N_NATUR == natur & n.K_ST == station & n.D_PR_DD == day & n.D_PR_MM == month & n.D_PR_YY == year).OrderByDescending(n => n.NPP);
//        }
//        else
//        {
//            return GetVagon().Where(n => n.N_NATUR == natur & n.K_ST == station & n.D_PR_DD == day & n.D_PR_MM == month & n.D_PR_YY == year).OrderBy(n => n.NPP);
//        }
//    }
//    catch (Exception e)
//    {
//        e.WriteErrorMethod(String.Format("GetVagon(natur={0}, station={1}, day={2}, month={3}, year={4}, sort={5})", natur, station, day, month, year, sort), eventID);
//        return null;
//    }

//}
////GetArrivalProm_VagonOfNaturStationDate
///// <summary>
///// Получить список вагонов по натурному листу станции и дате поступления
///// </summary>
///// <param name="natur"></param>
///// <param name="station"></param>
///// <param name="day"></param>
///// <param name="month"></param>
///// <param name="year"></param>
///// <returns></returns>
//public IQueryable<PromVagon> GetVagon(int natur, int station, int day, int month, int year)
//{
//    try
//    {
//        return GetVagon(natur, station, day, month, year, null);
//    }
//    catch (Exception e)
//    {
//        e.WriteErrorMethod(String.Format("GetVagon(natur={0}, station={1}, day={2}, month={3}, year={4})", natur, station, day, month, year), eventID);
//        return null;
//    }
//}
///// <summary>
///// Получить количество вагонов по натурному листу станции и дате поступления
///// </summary>
///// <param name="natur"></param>
///// <param name="station"></param>
///// <param name="day"></param>
///// <param name="month"></param>
///// <param name="year"></param>
///// <returns></returns>
//public int? CountWagonsVagon(int natur, int station, int day, int month, int year)
//{

//    try
//    {
//        IQueryable<PromVagon> pv = GetVagon(natur, station, day, month, year);
//        return pv == null ? (int?)pv.Count() : null;
//    }
//    catch (Exception e)
//    {
//        e.WriteErrorMethod(String.Format("CountWagonsVagon(natur={0}, station={1}, day={2}, month={3}, year={4})", natur, station, day, month, year), eventID);
//        return null;
//    }
//}
//GetArrivalProm_VagonOfNaturStationDateNum
// <summary>
// Получить информацию по вагону
// </summary>
// <param name="natur"></param>
// <param name="station"></param>
// <param name="day"></param>
// <param name="month"></param>
// <param name="year"></param>
// <param name="num"></param>
// <returns></returns>
//public PromVagon GetVagon(int natur, int station, int day, int month, int year, int num)
//{
//    try
//    {
//        return GetVagon(natur, station, day, month, year).Where(p => p.N_VAG == num).FirstOrDefault();
//    }
//    catch (Exception e)
//    {
//        e.WriteErrorMethod(String.Format("GetVagon(natur={0}, station={1}, day={2}, month={3}, year={4}, num={5})", natur, station, day, month, year, num), eventID);
//        return null;
//    }
//}
//#endregion

        //#region PROM.Nat_Hist
        //public IQueryable<PromNatHist> PromNatHist
        //{
        //    get { return context.PromNatHist; }
        //}

        //public IQueryable<PromNatHist> GetPromNatHist()
        //{
        //    try
        //    {
        //        return PromNatHist;
        //    }
        //    catch (Exception e)
        //    {
        //        e.WriteErrorMethod(String.Format("GetPromNatHist()"), eventID);
        //        return null;
        //    }
        //}
        //GetArrivalProm_NatHistOfNaturStationDate
        ///// <summary>
        ///// Получить список вагонов по натурному листу станции и дате поступления c сортировкой true- npp по убыванию false- npp по возрастанию
        ///// </summary>
        ///// <param name="natur"></param>
        ///// <param name="station"></param>
        ///// <param name="day"></param>
        ///// <param name="month"></param>
        ///// <param name="year"></param>
        ///// <returns></returns>
        //public IQueryable<PromNatHist> GetNatHistPR(int natur, int station, int day, int month, int year, bool? sort)
        //{
        //    try
        //    {
        //        if (sort == null)
        //        {
        //            return GetPromNatHist().Where(n => n.N_NATUR == natur & n.K_ST == station & n.D_PR_DD == day & n.D_PR_MM == month & n.D_PR_YY == year);
        //        }
        //        if ((bool)sort)
        //        {
        //            return GetPromNatHist().Where(n => n.N_NATUR == natur & n.K_ST == station & n.D_PR_DD == day & n.D_PR_MM == month & n.D_PR_YY == year).OrderByDescending(n => n.NPP);
        //        }
        //        else
        //        {
        //            return GetPromNatHist().Where(n => n.N_NATUR == natur & n.K_ST == station & n.D_PR_DD == day & n.D_PR_MM == month & n.D_PR_YY == year).OrderBy(n => n.NPP);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        e.WriteErrorMethod(String.Format("GetVagon(natur={0}, station={1}, day={2}, month={3}, year={4}, sort={5})", natur, station, day, month, year, sort), eventID);
        //        return null;
        //    }

        //}
        ////GetArrivalProm_NatHistOfNaturStationDate
        ///// <summary>
        ///// Получить список вагонов по натурному листу станции и дате поступления
        ///// </summary>
        ///// <param name="natur"></param>
        ///// <param name="station"></param>
        ///// <param name="day"></param>
        ///// <param name="month"></param>
        ///// <param name="year"></param>
        ///// <returns></returns>
        //public IQueryable<PromNatHist> GetNatHistPR(int natur, int station, int day, int month, int year)
        //{
        //    return GetNatHistPR(natur, station, day, month, year, null);
        //}
        //GetArrivalProm_NatHistOfNaturStationDateNum
        // <summary>
        // Получить вагон по натурному листу станции и дате поступления
        // </summary>
        // <param name="natur"></param>
        // <param name="station"></param>
        // <param name="day"></param>
        // <param name="month"></param>
        // <param name="year"></param>
        // <param name="wag"></param>
        // <returns></returns>
        //public PromNatHist GetNatHistPR(int natur, int station, int day, int month, int year, int wag)
        //{
        //    try
        //    {
        //        return GetNatHistPR(natur, station, day, month, year, null).Where(h => h.N_VAG == wag).FirstOrDefault();
        //    }
        //    catch (Exception e)
        //    {
        //        e.WriteErrorMethod(String.Format("GetNatHist(natur={0}, station={1}, day={2}, month={3}, year={4}, wag={5})", natur, station, day, month, year, wag), eventID);
        //        return null;
        //    }

        //}
        //GetSendingProm_NatHistOfNaturDate
        ///// <summary>
        ///// Получить список вагонов по натурному листу и дате сдачи вагона с сортировкой
        ///// </summary>
        ///// <param name="natur"></param>
        ///// <param name="day"></param>
        ///// <param name="month"></param>
        ///// <param name="year"></param>
        ///// <param name="sort"></param>
        ///// <returns></returns>
        //public IQueryable<PromNatHist> GetNatHistSD(int natur, int day, int month, int year, bool? sort)
        //{
        //    try
        //    {
        //        if (sort == null)
        //        {
        //            return GetNatHistSD(natur, day, month, year);
        //        }
        //        if ((bool)sort)
        //        {
        //            return GetNatHistSD(natur, day, month, year).OrderByDescending(n => n.NPP);
        //        }
        //        else
        //        {
        //            return GetNatHistSD(natur, day, month, year).OrderBy(n => n.NPP);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        e.WriteErrorMethod(String.Format("GetNatHistSendingOfNaturAndDate(natur={0}, day={1}, month={2}, year={3}, sort={4})", natur, day, month, year, sort), eventID);
        //        return null;
        //    }
        //}
        //GetSendingProm_NatHistOfNaturDate
        ///// <summary>
        ///// Получить список вагонов по натурному листу и дате сдачи вагона
        ///// </summary>
        ///// <param name="natur"></param>
        ///// <param name="day"></param>
        ///// <param name="month"></param>
        ///// <param name="year"></param>
        ///// <returns></returns>
        //public IQueryable<PromNatHist> GetNatHistSD(int natur, int day, int month, int year)
        //{
        //    try
        //    {
        //        return GetPromNatHist().Where(n => n.N_NATUR == natur & n.D_SD_DD == day & n.D_SD_MM == month & n.D_SD_YY == year);
        //    }
        //    catch (Exception e)
        //    {
        //        e.WriteErrorMethod(String.Format("GetNatHistSendingOfNaturAndDate(natur={0}, day={1}, month={2}, year={3})", natur, day, month, year, eventID));
        //        return null;
        //    }
        //}
        //GetSendingProm_NatHistOfNaturDateTime
        ///// <summary>
        ///// Получить список вагонов по натурному листу и дате сдачи вагона
        ///// </summary>
        ///// <param name="natur"></param>
        ///// <param name="day"></param>
        ///// <param name="month"></param>
        ///// <param name="year"></param>
        ///// <param name="hour"></param>
        ///// <param name="minute"></param>
        ///// <param name="sort"></param>
        ///// <returns></returns>
        //public IQueryable<PromNatHist> GetNatHistSD(int natur, int day, int month, int year, int hour, int minute, bool? sort)
        //{
        //    try
        //    {
        //        if (sort == null)
        //        {
        //            return GetPromNatHist().Where(n => n.N_NATUR == natur & n.D_SD_DD == day & n.D_SD_MM == month & n.D_SD_YY == year & n.T_SD_HH == hour & n.T_SD_MI == minute);
        //        }
        //        if ((bool)sort)
        //        {
        //            return GetPromNatHist().Where(n => n.N_NATUR == natur & n.D_SD_DD == day & n.D_SD_MM == month & n.D_SD_YY == year & n.T_SD_HH == hour & n.T_SD_MI == minute).OrderByDescending(n => n.NPP);
        //        }
        //        else
        //        {
        //            return GetPromNatHist().Where(n => n.N_NATUR == natur & n.D_SD_DD == day & n.D_SD_MM == month & n.D_SD_YY == year & n.T_SD_HH == hour & n.T_SD_MI == minute).OrderBy(n => n.NPP);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        e.WriteErrorMethod(String.Format("GetNatHistOfNaturAndDT(natur={0}, day={1}, month={2}, year={3}, hour={4}, minute={5} sort={6})", natur, day, month, year, hour, minute, sort), eventID);
        //        return null;
        //    }
        //}
        //GetSendingProm_NatHistOfNaturNumDateTime
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="natur"></param>
        ///// <param name="num"></param>
        ///// <param name="day"></param>
        ///// <param name="month"></param>
        ///// <param name="year"></param>
        ///// <param name="hour"></param>
        ///// <param name="minute"></param>
        ///// <returns></returns>
        //public PromNatHist GetNatHistSD(int natur, int num, int day, int month, int year, int hour, int minute)
        //{
        //    try
        //    {
        //        return GetPromNatHist().Where(n => n.N_NATUR == natur & n.N_VAG == num & n.D_SD_DD == day & n.D_SD_MM == month & n.D_SD_YY == year & n.T_SD_HH == hour & n.T_SD_MI == minute).FirstOrDefault();
        //    }
        //    catch (Exception e)
        //    {
        //        e.WriteErrorMethod(String.Format("GetNatHistSendingOfNaturNumDT(natur={0}, num={1}, day={2}, month={3}, year={4}, hour={5}, minute={6})", natur, num, day, month, year, hour, minute), eventID);
        //        return null;
        //    }
        //}
        //GetSendingProm_NatHistOfNumDateTime
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="num"></param>
        ///// <param name="day"></param>
        ///// <param name="month"></param>
        ///// <param name="year"></param>
        ///// <param name="hour"></param>
        ///// <param name="minute"></param>
        ///// <returns></returns>
        //public PromNatHist GetNatHistSD(int num, int day, int month, int year, int hour, int minute)
        //{
        //    try
        //    {
        //        return GetPromNatHist().Where(n => n.N_VAG == num & n.D_SD_DD == day & n.D_SD_MM == month & n.D_SD_YY == year & n.T_SD_HH == hour & n.T_SD_MI == minute).FirstOrDefault();
        //    }
        //    catch (Exception e)
        //    {
        //        e.WriteErrorMethod(String.Format("GetNatHistSendingOfNaturNumDT(num={0}, day={1}, month={2}, year={3}, hour={4}, minute={5})", num, day, month, year, hour, minute), eventID);
        //        return null;
        //    }
        //}

        // перен
        ///// <summary>
        ///// Получить список вагонов по номеру вагона
        ///// </summary>
        ///// <param name="num_vag"></param>
        ///// <returns></returns>
        //public IQueryable<PromNatHist> GetNatHistOfVagon(int num_vag)
        //{
        //    try
        //    {
        //        return GetPromNatHist().Where(n => n.N_VAG == num_vag);
        //    }
        //    catch (Exception e)
        //    {
        //        e.WriteErrorMethod(String.Format("GetNatHistOfVagon(num_vag={0})", num_vag), eventID);
        //        return null;
        //    }
        //}
        // перен
        ///// <summary>
        ///// Получить список вагонов по номеру вагона и датой отправки больше указаной даты
        ///// </summary>
        ///// <param name="num_vag"></param>
        ///// <param name="start"></param>
        ///// <returns></returns>
        //public IQueryable<PromNatHist> GetNatHistOfVagonMoreSD(int num_vag, DateTime start)
        //{
        //    try
        //    {
        //        List<PromNatHist> list = GetNatHistOfVagon(num_vag).ToList();
        //        return list.ToArray().FilterArrayOfFilterFrom(Filters.IsMoreSD, start).AsQueryable();
        //    }
        //    catch (Exception e)
        //    {
        //        e.WriteErrorMethod(String.Format("GetNatHistOfVagonMoreSD(num_vag={0}, start={1})", num_vag, start), eventID);
        //        return null;
        //    }
        //}
        ///// <summary>
        ///// Получить список вагонов по номеру вагона и датой отправки больше указаной даты с сортировкой
        ///// </summary>
        ///// <param name="num_vag"></param>
        ///// <param name="start"></param>
        ///// <param name="sort"></param>
        ///// <returns></returns>
        //public IQueryable<PromNatHist> GetNatHistOfVagonMoreSD(int num_vag, DateTime start, bool sort)
        //{
        //    try
        //    {
        //        if (sort)
        //        {
        //            return GetNatHistOfVagonMoreSD(num_vag, start)
        //                .OrderByDescending(p => p.D_PR_YY)
        //                .ThenByDescending(p => p.D_PR_MM)
        //                .ThenByDescending(p => p.D_PR_DD)
        //                .ThenByDescending(p => p.T_PR_HH)
        //                .ThenByDescending(p => p.T_PR_MI);
        //        }
        //        else
        //        {
        //            return GetNatHistOfVagonMoreSD(num_vag, start)
        //                .OrderBy(p => p.D_PR_YY)
        //                .ThenBy(p => p.D_PR_MM)
        //                .ThenBy(p => p.D_PR_DD)
        //                .ThenBy(p => p.T_PR_HH)
        //                .ThenBy(p => p.T_PR_MI);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        e.WriteErrorMethod(String.Format("GetNatHistOfVagonMoreSD(num_vag={0}, start={1}, sort={2})", num_vag, start, sort), eventID);
        //        return null;
        //    }
        //}
        // перен
        ///// <summary>
        ///// Получить список вагонов по номеру вагона и датой прибытия меньше указаной даты
        ///// </summary>
        ///// <param name="num_vag"></param>
        ///// <param name="start"></param>
        ///// <returns></returns>
        //public IQueryable<PromNatHist> GetNatHistOfVagonLessPR(int num_vag, DateTime start)
        //{
        //    try
        //    {
        //        List<PromNatHist> list = GetNatHistOfVagon(num_vag).ToList();
        //        return list.ToArray().FilterArrayOfFilterFrom(Filters.IsLessPR, start).AsQueryable();
        //    }
        //    catch (Exception e)
        //    {
        //        e.WriteErrorMethod(String.Format("GetNatHistOfVagonLess(num_vag={0}, start={1})", num_vag, start), eventID);
        //        return null;
        //    }
        //}
        ///// <summary>
        ///// Получить список вагонов по номеру вагона и датой прибытия меньше указаной даты с сортировкой
        ///// </summary>
        ///// <param name="num_vag"></param>
        ///// <param name="start"></param>
        ///// <param name="sort"></param>
        ///// <returns></returns>
        //public IQueryable<PromNatHist> GetNatHistOfVagonLessPR(int num_vag, DateTime start, bool sort)
        //{
        //    try
        //    {
        //        if (sort)
        //        {
        //            return GetNatHistOfVagonLessPR(num_vag, start)
        //                .OrderByDescending(p => p.D_PR_YY)
        //                .ThenByDescending(p => p.D_PR_MM)
        //                .ThenByDescending(p => p.D_PR_DD)
        //                .ThenByDescending(p => p.T_PR_HH)
        //                .ThenByDescending(p => p.T_PR_MI);
        //        }
        //        else
        //        {
        //            return GetNatHistOfVagonLessPR(num_vag, start)
        //                .OrderBy(p => p.D_PR_YY)
        //                .ThenBy(p => p.D_PR_MM)
        //                .ThenBy(p => p.D_PR_DD)
        //                .ThenBy(p => p.T_PR_HH)
        //                .ThenBy(p => p.T_PR_MI);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        e.WriteErrorMethod(String.Format("GetNatHistOfVagonLess(num_vag={0}, start={1}, sort={2})", num_vag, start, sort), eventID);
        //        return null;
        //    }
        //}
        // перен
        ///// <summary>
        ///// Получить список вагонов по номеру вагона и датой прибытия меньше или равно указаной даты
        ///// </summary>
        ///// <param name="num_vag"></param>
        ///// <param name="start"></param>
        ///// <returns></returns>
        //public IQueryable<PromNatHist> GetNatHistOfVagonLessEqualPR(int num_vag, DateTime start)
        //{
        //    return GetNatHistOfVagon(num_vag).ToArray().FilterArrayOfFilterFrom(Filters.IsLessOrEqualPR, start).AsQueryable();
        //}
        ///// <summary>
        ///// Получить список вагонов по номеру вагона и датой прибытия меньше указаной даты с сортировкой
        ///// </summary>
        ///// <param name="num_vag"></param>
        ///// <param name="start"></param>
        ///// <param name="sort"></param>
        ///// <returns></returns>
        //public IQueryable<PromNatHist> GetNatHistOfVagonLessEqualPR(int num_vag, DateTime start, bool sort)
        //{
        //    if (sort)
        //    {
        //        return GetNatHistOfVagonLessEqualPR(num_vag, start)
        //            .OrderByDescending(p => p.D_PR_YY)
        //            .ThenByDescending(p => p.D_PR_MM)
        //            .ThenByDescending(p => p.D_PR_DD)
        //            .ThenByDescending(p => p.T_PR_HH)
        //            .ThenByDescending(p => p.T_PR_MI);
        //    }
        //    else
        //    {
        //        return GetNatHistOfVagonLessEqualPR(num_vag, start)
        //            .OrderBy(p => p.D_PR_YY)
        //            .ThenBy(p => p.D_PR_MM)
        //            .ThenBy(p => p.D_PR_DD)
        //            .ThenBy(p => p.T_PR_HH)
        //            .ThenBy(p => p.T_PR_MI);
        //    }

        //}

        ///// <summary>
        ///// Получить список вагонов по номеру вагона и датой прибытия болше или равно указаной даты
        ///// </summary>
        ///// <param name="num_vag"></param>
        ///// <param name="start"></param>
        ///// <returns></returns>
        //public IQueryable<PromNatHist> GetNatHistOfVagonGreaterEqualPR(int num_vag, DateTime start)
        //{
        //    return GetNatHistOfVagon(num_vag).ToArray().FilterArrayOfFilterFrom(Filters.IsGreaterOrEqualPR, start).AsQueryable();
        //}
        ///// <summary>
        ///// Получить список вагонов по номеру вагона и датой прибытия больше или равно указаной даты с сортировкой
        ///// </summary>
        ///// <param name="num_vag"></param>
        ///// <param name="start"></param>
        ///// <param name="sort"></param>
        ///// <returns></returns>
        //public IQueryable<PromNatHist> GetNatHistOfVagonGreaterEqualPR(int num_vag, DateTime start, bool sort)
        //{
        //    if (sort)
        //    {
        //        return GetNatHistOfVagonGreaterEqualPR(num_vag, start)
        //            .OrderByDescending(p => p.D_PR_YY)
        //            .ThenByDescending(p => p.D_PR_MM)
        //            .ThenByDescending(p => p.D_PR_DD)
        //            .ThenByDescending(p => p.T_PR_HH)
        //            .ThenByDescending(p => p.T_PR_MI);
        //    }
        //    else
        //    {
        //        return GetNatHistOfVagonGreaterEqualPR(num_vag, start)
        //            .OrderBy(p => p.D_PR_YY)
        //            .ThenBy(p => p.D_PR_MM)
        //            .ThenBy(p => p.D_PR_DD)
        //            .ThenBy(p => p.T_PR_HH)
        //            .ThenBy(p => p.T_PR_MI);
        //    }

        //}
        //#endregion
//#region PROM.SOSTAV

//public IQueryable<PromSostav> PromSostav
//{
//    get { return context.PromSostav; }
//}

//public IQueryable<PromSostav> GetPromSostav()
//{
//    try
//    {
//        //return PromSostav.Where(p => p.D_PR_DD != null & p.D_PR_MM != null & p.D_PR_YY != null);
//        return PromSostav;
//    }
//    catch (Exception e)
//    {
//        e.WriteErrorMethod(String.Format("GetPromSostav()"), eventID);
//        return null;
//    }
//}
/// <summary>
/// 
/// </summary>
/// <param name="start"></param>
/// <param name="stop"></param>
/// <returns></returns>
//public IQueryable<PromSostav> GetPromSostav(DateTime start, DateTime stop)
//{
//    try
//    {
//        return GetPromSostav().Where(p => p.P_OT != null & p.V_P == 1 & p.K_ST != null & p.D_PR_YY != null & p.D_PR_MM != null & p.D_PR_DD != null & p.T_PR_HH != null & p.T_PR_MI != null).ToArray().FilterArrayOfFilterFromTo(Filters.IsGreaterOrequalToLessOrEqual, start, stop).AsQueryable();
//        //return GetPromSostav().Where(p => p.DT >= start & p.DT <= stop);
//    }
//    catch (Exception e)
//    {
//        e.WriteErrorMethod(String.Format("GetPromSostav(start={0}, stop={1})", start, stop), eventID);
//        return null;
//    }
//}
/// <summary>
/// Вернуть составы прибывшие на станцию промышленую
/// </summary>
/// <returns></returns>
//public IQueryable<PromSostav> GetInputPromSostav()
//{
//    try
//    {
//        return GetPromSostav().Where(p => p.P_OT == 0 & p.V_P == 1 & p.K_ST != null);
//    }
//    catch (Exception e)
//    {
//        e.WriteErrorMethod(String.Format("GetInputPromSostav()"), eventID);
//        return null;
//    }
//}
/// <summary>
/// Вернуть составы прибывшие на станцию промышленую за указанный период
/// </summary>
/// <param name="start"></param>
/// <param name="stop"></param>
/// <returns></returns>
//public IQueryable<PromSostav> GetInputPromSostav(DateTime start, DateTime stop)
//{
//    try
//    {
//        return GetInputPromSostav().ToArray().FilterArrayOfFilterFromTo(Filters.IsGreaterOrequalToLessOrEqual, start, stop).AsQueryable();
//    }
//    catch (Exception e)
//    {
//        e.WriteErrorMethod(String.Format("GetInputPromSostav(start={0}, stop={1})", start, stop), eventID);
//        return null;
//    }
//}
/// <summary>
/// Вернуть составы прибывшие на станцию промышленую за указанный период с сортировкой true - по убывания false - по возростанию
/// </summary>
/// <param name="start"></param>
/// <param name="stop"></param>
/// <param name="sort"></param>
/// <returns></returns>
//public IQueryable<PromSostav> GetInputPromSostav(DateTime start, DateTime stop, bool sort)
//{
//    try
//    {
//        if (sort)
//        {
//            return GetInputPromSostav(start, stop)
//                .OrderByDescending(p => p.D_YY)
//                .ThenByDescending(p => p.D_MM)
//                .ThenByDescending(p => p.D_DD)
//                .ThenByDescending(p => p.T_HH)
//                .ThenByDescending(p => p.T_MI);
//        }
//        else
//        {
//            return GetInputPromSostav(start, stop)
//                .OrderBy(p => p.D_YY)
//                .ThenBy(p => p.D_MM)
//                .ThenBy(p => p.D_DD)
//                .ThenBy(p => p.T_HH)
//                .ThenBy(p => p.T_MI);
//        }
//    }
//    catch (Exception e)
//    {
//        e.WriteErrorMethod(String.Format("GetInputPromSostav(start={0}, stop={1}, sort={2})", start, stop, sort), eventID);
//        return null;
//    }

//}
/// <summary>
/// Вернуть составы отправленные на станции УЗ
/// </summary>
/// <returns></returns>
//public IQueryable<PromSostav> GetOutputPromSostav()
//{
//    try
//    {
//        //return GetPromSostav().Where(p => p.P_OT == 1 & p.K_ST != null);
//        return GetPromSostav().Where(p => p.P_OT == 1 & p.T_PR_HH != null & p.T_PR_MI != null);
//    }
//    catch (Exception e)
//    {
//        e.WriteErrorMethod(String.Format("GetOutputPromSostav()"), eventID);
//        return null;
//    }
//}
/// <summary>
/// Вернуть составы отправленные на станции УЗ за указанный период
/// </summary>
/// <param name="start"></param>
/// <param name="stop"></param>
/// <returns></returns>
//public IQueryable<PromSostav> GetOutputPromSostav(DateTime start, DateTime stop)
//{
//    try
//    {
//        return GetOutputPromSostav().ToArray().FilterArrayOfFilterFromTo(Filters.IsGreaterOrequalToLessOrEqual, start, stop).AsQueryable();
//    }
//    catch (Exception e)
//    {
//        e.WriteErrorMethod(String.Format("GetOutputPromSostav(start={0}, stop={1})", start, stop), eventID);
//        return null;
//    }
//}
/// <summary>
/// Вернуть составы отправленные на станции УЗ за указанный период с сортировкой
/// </summary>
/// <param name="start"></param>
/// <param name="stop"></param>
/// <param name="sort"></param>
/// <returns></returns>
//public IQueryable<PromSostav> GetOutputPromSostav(DateTime start, DateTime stop, bool sort)
//{
//    try
//    {
//        if (sort)
//        {
//            return GetOutputPromSostav(start, stop)
//                .OrderByDescending(p => p.D_YY)
//                .ThenByDescending(p => p.D_MM)
//                .ThenByDescending(p => p.D_DD)
//                .ThenByDescending(p => p.T_HH)
//                .ThenByDescending(p => p.T_MI);
//        }
//        else
//        {
//            return GetOutputPromSostav(start, stop)
//                .OrderBy(p => p.D_YY)
//                .ThenBy(p => p.D_MM)
//                .ThenBy(p => p.D_DD)
//                .ThenBy(p => p.T_HH)
//                .ThenBy(p => p.T_MI);
//        }
//    }
//    catch (Exception e)
//    {
//        e.WriteErrorMethod(String.Format("GetOutputPromSostav(start={0}, stop={1}, sort={2})", start, stop, sort), eventID);
//        return null;
//    }

//}

///// <summary>
///// Выбрать строки с указанием направления
///// </summary>
///// <returns></returns>
//public IQueryable<PromSostav> GetPromSostav(bool direction)
//{
//    try
//    {
//        string sql = "SELECT N_NATUR,D_DD,D_MM,D_YY,T_HH,T_MI,K_ST,N_PUT,NAPR,P_OT,V_P,K_ST_OTPR,K_ST_PR,N_VED_PR,N_SOST_OT,N_SOST_PR,DAT_VVOD FROM PROM.SOSTAV ";
//        if (direction)
//        {
//            sql += "WHERE (P_OT = 1 and K_ST_PR is not null)";
//        }
//        else
//        {

//            sql += "WHERE ( P_OT = 0 and K_ST_OTPR is not null)";
//        }

//        return rep_ps.db.SqlQuery<PromSostav>(sql).AsQueryable();
//    }
//    catch (Exception e)
//    {
//        ServicesEventLog.LogError(e, "GetPromSostav(1)", eventID);
//        return null;
//    }
//}
///// <summary>
///// Выбрать строки с указанием направления и временного диапазона
///// </summary>
///// <param name="start"></param>
///// <param name="stop"></param>
///// <param name="direction"></param>
///// <returns></returns>
//public IQueryable<PromSostav> GetPromSostav(DateTime start, DateTime stop, bool direction)
//{
//    return GetPromSostav(direction).ToArray().FilterArrayOfFilterFromTo(Filters.IsGreaterOrequalToLessOrEqual, start, stop).AsQueryable();
//}

///// <summary>
///// Вернуть все прибывшие составы
///// </summary>
///// <returns></returns>
//public IQueryable<PromSostav> GetArrivalPromSostav()
//{
//    return rep_ps.PromSostav.Where(p => p.P_OT == 0 & p.K_ST_OTPR != null);
//}
///// <summary>
///// Вернуть все отправленные составы
///// </summary>
///// <returns></returns>
//public IQueryable<PromSostav> GetDeparturePromSostav()
//{
//    return rep_ps.PromSostav.Where(p => p.P_OT == 1 & p.K_ST_PR != null);
//}

///// <summary>
///// Вернуть состав прибывший на станцию промышленую по натурке
///// </summary>
///// <param name="natur"></param>
///// <returns></returns>
//public PromSostav GetInputPromSostavToNatur(int natur)
//{
//    return GetInputPromSostav().Where(p => p.N_NATUR == natur).FirstOrDefault();
//}
///// <summary>
///// Вернуть состав прибывший на станцию промышленую по натурке и дате
///// </summary>
///// <param name="natur"></param>
///// <param name="station"></param>
///// <param name="day"></param>
///// <param name="month"></param>
///// <param name="year"></param>
///// <returns></returns>
//public PromSostav GetArrivalPromSostavToNatur(int natur, int station, int day, int month, int year)
//{
//    return GetInputPromSostav().Where(p => p.N_NATUR == natur & p.K_ST == station & p.D_DD == day & p.D_MM == month & p.D_YY == year).FirstOrDefault();
//}

//public PromSostav GetInputPromSostavToNatur(int natur, int station, int day, int month, int year)
//{
//    return rep_ps.PromSostav.Where(p => p.N_NATUR == natur & p.K_ST == station & p.D_DD == day & p.D_MM == month & p.D_YY == year).FirstOrDefault();
//}
///// <summary>
///// Вернуть все составы на станции промышленая за указанный период
///// </summary>
///// <param name="start"></param>
///// <param name="stop"></param>
///// <returns></returns>
//public IQueryable<PromSostav> GetArrivalPromSostav(DateTime start, DateTime stop)
//{
//    return GetArrivalPromSostav().ToArray().FilterArrayOfFilterFromTo(Filters.IsGreaterOrequalToLessOrEqual, start, stop).AsQueryable();
//}
///// <summary>
///// Вернуть все составы на станции промышленая за указанный период
///// </summary>
///// <param name="start"></param>
///// <param name="stop"></param>
///// <returns></returns>
//public IQueryable<PromSostav> GetDeparturePromSostav(DateTime start, DateTime stop)
//{
//    return GetDeparturePromSostav().ToArray().FilterArrayOfFilterFromTo(Filters.IsGreaterOrequalToLessOrEqual, start, stop).AsQueryable();
//}

//#endregion Prom_Sostav