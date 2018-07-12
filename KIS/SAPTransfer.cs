using EFKIS.Concrete;
using EFKIS.Entities;
using EFMT.Entities;
using EFRC.Concrete;
using EFRC.Entities;
using MessageLog;
using RCReferences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KIS
{

    public class trWagon
    {
        public int Position { get; set; }
        public int CarriageNumber { get; set; }
        public int CountryCode { get; set; }
        public float Weight { get; set; }
        public int IDCargo { get; set; }
        public string Cargo { get; set; }
        public int IDStation { get; set; }
        public string Station { get; set; }
        public int Consignee { get; set; }
        public string Operation { get; set; }
        public string CompositionIndex { get; set; }
        public DateTime DateOperation { get; set; }
        public int TrainNumber { get; set; }
        public int Conditions { get; set; }
    }

    public class trSostav
    {
        public int id { get; set; }
        public int? codecs_in_station { get; set; } // Станция получатель состава
        public int? codecs_from_station { get; set; } // Станция отравитель состава
        //public string FileName { get; set; }
        //public string CompositionIndex { get; set; }
        public DateTime DateTime_on { get; set; }
        public DateTime DateTime_from { get; set; }
        //public int Operation { get; set; }
        //public DateTime Create { get; set; }
        //public DateTime? Close { get; set; }
        public int? ParentID { get; set; }
        public trWagon[] Wagons { get; set; }
    }
    
    public class SAPTransfer
    {
        private eventID eventID = eventID.KIS_SAPTransfer;
        protected service servece_owner = service.Null;

        public SAPTransfer()
        {

        }

        public SAPTransfer(service servece_owner)
        {
            this.servece_owner = servece_owner;
        }

        //#region Справочник САП входящие поставки
        ///// <summary>
        ///// Проверка наличия вагона в справочнеке входящие поставки, если нет создать
        ///// </summary>
        ///// <param name="id_sostav"></param>
        ///// <param name="pnh"></param>
        ///// <returns></returns>
        //public bool CheckingWagonToSAPSupply(int id_sostav, PromNatHist pnh)
        //{
        //    EFSAP ef_sap = new EFSAP();
        //    EFWagons ef_wag = new EFWagons();
        //    // Проверим есть строка в справочнеке САП поставки
        //    if (!ef_sap.IsWagonToSAPSupply(id_sostav, pnh.N_VAG))
        //    {
        //        // Определим код груза
        //        int IDCargo = 0;
        //        if (pnh.K_GR != null)
        //        {
        //            PromGruzSP pg = ef_wag.GetGruzSP((int)pnh.K_GR);
        //            IDCargo = pg != null ? pg.TAR_GR != null ? (int)pg.TAR_GR : 0 : 0;
        //        }
        //        // Создадим строку в САП (id состава)
        //        trWagon wag = new trWagon()
        //        {
        //            Position = pnh != null ? (int)pnh.NPP : 0,
        //            CarriageNumber = pnh.N_VAG,
        //            CountryCode = pnh != null ? pnh.KOD_STRAN != null ? ((int)pnh.KOD_STRAN * 10) + 1 : 0 : 0,
        //            Weight = pnh != null ? pnh.WES_GR != null ? (float)pnh.WES_GR : 0 : 0,
        //            IDCargo = IDCargo,
        //            Cargo = null,
        //            IDStation = 0,
        //            Station = null,
        //            Consignee = 0,
        //            Operation = null,
        //            CompositionIndex = "N:" + pnh.N_NATUR + " D:" + pnh.D_PR_DD + "." + pnh.D_PR_MM + "." + pnh.D_PR_YY + " " + pnh.T_PR_HH + "-" + pnh.T_PR_MI,
        //            DateOperation = DateTime.Now,
        //            TrainNumber = 0,
        //            Conditions = 0,
        //        };

        //        SetWagonToSAPSupply(wag, id_sostav);
        //        return false;
        //    }
        //    return true;
        //}
        ///// <summary>
        ///// Проверка наличия вагона в справочнеке входящие поставки, если нет создать
        ///// </summary>
        ///// <param name="id_sostav"></param>
        ///// <param name="pv"></param>
        ///// <returns></returns>
        //public bool CheckingWagonToSAPSupply(int id_sostav, PromVagon pv)
        //{
        //    EFSAP ef_sap = new EFSAP();
        //    EFWagons ef_wag = new EFWagons();
        //    // Проверим есть строка в справочнеке САП поставки
        //    if (!ef_sap.IsWagonToSAPSupply(id_sostav, pv.N_VAG))
        //    {
        //        // Определим код груза
        //        int IDCargo = 0;
        //        if (pv.K_GR != null)
        //        {
        //            PromGruzSP pg = ef_wag.GetGruzSP((int)pv.K_GR);
        //            IDCargo = pg != null ? pg.TAR_GR != null ? (int)pg.TAR_GR : 0 : 0;
        //        }
        //        // Создадим строку в САП (id состава)
        //        trWagon wag = new trWagon()
        //        {
        //            Position = pv != null ? pv.NPP : 0,
        //            CarriageNumber = pv.N_VAG,
        //            CountryCode = pv != null ? pv.KOD_STRAN != null ? ((int)pv.KOD_STRAN * 10) + 1 : 0 : 0,
        //            Weight = pv != null ? pv.WES_GR != null ? (float)pv.WES_GR : 0 : 0,
        //            IDCargo = IDCargo,
        //            Cargo = null,
        //            IDStation = 0,
        //            Station = null,
        //            Consignee = 0,
        //            Operation = null,
        //            CompositionIndex = "N:" + pv.N_NATUR + " D:" + pv.D_PR_DD + "." + pv.D_PR_MM + "." + pv.D_PR_YY + " " + pv.T_PR_HH + "-" + pv.T_PR_MI,
        //            DateOperation = DateTime.Now,
        //            TrainNumber = 0,
        //            Conditions = 0,
        //        };

        //        SetWagonToSAPSupply(wag, id_sostav);
        //        return false;
        //    }
        //    return true;
        //}
        //#endregion

        /// <summary>
        /// Получить строку SAPIncSupply из trWagon
        /// </summary>
        /// <param name="wagon"></param>
        /// <param name="idsostav"></param>
        /// <returns></returns>
        public SAPIncSupply ConvertWagonToSAPSupply(trWagon wagon, int idsostav)
        {
            try
            {
                if (wagon == null) return null;
                RCReference rc_ref = new RCReference();
                //Определим страну по общему справочнику
                int id_country = 0;
                if (wagon.CountryCode > 0)
                {
                    int country = 0;
                    country = int.Parse(wagon.CountryCode.ToString().Substring(0, 2));
                    id_country = rc_ref.DefinitionIDCountrySNG(country);
                }
                //Определим груз по общему справочнику
                int id_cargo = rc_ref.DefinitionIDCargo(wagon.IDCargo);

                SAPIncSupply sap_Supply = new SAPIncSupply()
                {
                    ID = 0,
                    DateTime = wagon.DateOperation,
                    CompositionIndex = wagon.CompositionIndex,
                    IDMTSostav = idsostav,
                    CarriageNumber = wagon.CarriageNumber,
                    Position = wagon.Position,
                    NumNakl = null,
                    CountryCode = wagon.CountryCode,
                    IDCountry = id_country,
                    WeightDoc = (decimal?)wagon.Weight,
                    DocNumReweighing = null,
                    DocDataReweighing = null,
                    WeightReweighing = null,
                    DateTimeReweighing = null,
                    PostReweighing = null,
                    CodeCargo = wagon.IDCargo,
                    IDCargo = id_cargo,
                    CodeMaterial = null,
                    NameMaterial = null,
                    CodeStationShipment = null,
                    NameStationShipment = null,
                    CodeShop = null,
                    NameShop = null,
                    CodeNewShop = null,
                    NameNewShop = null,
                    PermissionUnload = null,
                    Step1 = null,
                    Step2 = null

                };
                return sap_Supply;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("ConvertWagonToSAPSupply(wagon={0}, idsostav={1})", wagon, idsostav), eventID);
                return null;
            }
        }
        /// <summary>
        /// Записать строку с вагоном в справочник САП
        /// </summary>
        /// <param name="wagon"></param>
        /// <param name="idsostav"></param>
        /// <returns></returns>
        public int SetWagonToSAPSupply(trWagon wagon, int idsostav)
        {
            EFSAP ef_sap = new EFSAP();
            SAPIncSupply saps = ConvertWagonToSAPSupply(wagon, idsostav);
            if (saps != null) return ef_sap.SaveSAPIncSupply(saps);
            return 0;
        }
        /// <summary>
        /// Проверка наличия вагона в справочнеке входящие поставки, если нет создать
        /// </summary>
        /// <param name="id_sostav"></param>
        /// <param name="pv"></param>
        /// <returns></returns>
        public bool CheckingWagonToSAPSupply(int id_sostav, Prom_Vagon pv, ArrivalCars cars )
        {
            EFSAP ef_sap = new EFSAP();
            EFWagons ef_wag = new EFWagons();
            // Проверим есть строка в справочнеке САП поставки
            if (!ef_sap.IsWagonToSAPSupply(id_sostav, pv.N_VAG))
            {
                trWagon new_wag;
                //TODO: Тест исправление вагоны с определенным IDsostav записывались как принятые по КИС
                if (id_sostav > 0 & cars != null)
                {
                    new_wag = new trWagon()
                        {
                            Position = cars.Position,
                            CarriageNumber = cars.Num,
                            CountryCode = cars.CountryCode,
                            Weight = cars.Weight,
                            IDCargo = cars.CargoCode,
                            Cargo = cars.Cargo,
                            IDStation = cars.StationCode,
                            Station = cars.Station,
                            Consignee = cars.Consignee,
                            Operation = cars.Operation,
                            CompositionIndex = cars.CompositionIndex,
                            DateOperation = cars.DateOperation,
                            TrainNumber = cars.TrainNumber,
                            Conditions = 15, // прибывший с УЗ
                        };
                }
                else
                {
                    // Определим код груза
                    int IDCargo = 0;
                    if (pv.K_GR != null)
                    {
                        PromGruzSP pg = ef_wag.GetGruzSP((int)pv.K_GR);
                        IDCargo = pg != null ? pg.TAR_GR != null ? (int)pg.TAR_GR : 0 : 0;
                    }
                    // Создадим строку в САП (id состава)
                    new_wag = new trWagon()
                    {
                        Position = pv != null && pv.NPP !=null ? (int)pv.NPP : 0,
                        CarriageNumber = pv.N_VAG,
                        CountryCode = pv != null ? pv.KOD_STRAN != null ? (int)pv.KOD_STRAN : 0 : 0,
                        Weight = pv != null ? pv.WES_GR != null ? (float)pv.WES_GR : 0 : 0,
                        IDCargo = IDCargo,
                        Cargo = null,
                        IDStation = 0,
                        Station = null,
                        Consignee = 0,
                        Operation = null,
                        CompositionIndex = "N:" + pv.N_NATUR + " D:" + pv.D_PR_DD + "." + pv.D_PR_MM + "." + pv.D_PR_YY + " " + pv.T_PR_HH + "-" + pv.T_PR_MI,
                        DateOperation = DateTime.Now,
                        TrainNumber = 0,
                        Conditions = 0,
                    };
                }
                SetWagonToSAPSupply(new_wag, id_sostav);
                return false;
            }
            return true;
        }
        /// Проверка наличия вагона в справочнеке входящие поставки, если нет создать
        public bool CheckingWagonToSAPSupply(int id_sostav, Prom_NatHist pnh, ArrivalCars cars)
        {
            EFSAP ef_sap = new EFSAP();
            EFWagons ef_wag = new EFWagons();
            // Проверим есть строка в справочнеке САП поставки
            if (!ef_sap.IsWagonToSAPSupply(id_sostav, pnh.N_VAG))
            {
                trWagon new_wag;
                //TODO: Тест исправление вагоны с определенным IDsostav записывались как принятые по КИС
                if (id_sostav > 0 & cars != null)
                {
                    new_wag = new trWagon()
                        {
                            Position = cars.Position,
                            CarriageNumber = cars.Num,
                            CountryCode = cars.CountryCode,
                            Weight = cars.Weight,
                            IDCargo = cars.CargoCode,
                            Cargo = cars.Cargo,
                            IDStation = cars.StationCode,
                            Station = cars.Station,
                            Consignee = cars.Consignee,
                            Operation = cars.Operation,
                            CompositionIndex = cars.CompositionIndex,
                            DateOperation = cars.DateOperation,
                            TrainNumber = cars.TrainNumber,
                            Conditions = 15, // прибывший с УЗ
                        };
                }
                else
                {
                    // Определим код груза
                    int IDCargo = 0;
                    if (pnh.K_GR != null)
                    {
                        PromGruzSP pg = ef_wag.GetGruzSP((int)pnh.K_GR);
                        IDCargo = pg != null ? pg.TAR_GR != null ? (int)pg.TAR_GR : 0 : 0;
                    }
                    // Создадим строку в САП (id состава)
                    new_wag = new trWagon()
                    {
                        Position = pnh != null ? (int)pnh.NPP : 0,
                        CarriageNumber = pnh.N_VAG,
                        CountryCode = pnh != null ? pnh.KOD_STRAN != null ? (int)pnh.KOD_STRAN : 0 : 0,
                        Weight = pnh != null ? pnh.WES_GR != null ? (float)pnh.WES_GR : 0 : 0,
                        IDCargo = IDCargo,
                        Cargo = null,
                        IDStation = 0,
                        Station = null,
                        Consignee = 0,
                        Operation = null,
                        CompositionIndex = "N:" + pnh.N_NATUR + " D:" + pnh.D_PR_DD + "." + pnh.D_PR_MM + "." + pnh.D_PR_YY + " " + pnh.T_PR_HH + "-" + pnh.T_PR_MI,
                        DateOperation = DateTime.Now,
                        TrainNumber = 0,
                        Conditions = 0,
                    };
                }
                SetWagonToSAPSupply(new_wag, id_sostav);
                return false;
            }
            return true;
        }

    }
}
