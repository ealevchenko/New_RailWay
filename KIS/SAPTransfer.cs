using EFKIS.Concrete;
using EFKIS.Entities;
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
        public bool CheckingWagonToSAPSupply(int id_sostav, PromVagon pv)
        {
            EFSAP ef_sap = new EFSAP();
            EFWagons ef_wag = new EFWagons();
            // Проверим есть строка в справочнеке САП поставки
            if (!ef_sap.IsWagonToSAPSupply(id_sostav, pv.N_VAG))
            {
                // Определим код груза
                int IDCargo = 0;
                if (pv.K_GR != null)
                {
                    PromGruzSP pg = ef_wag.GetGruzSP((int)pv.K_GR);
                    IDCargo = pg != null ? pg.TAR_GR != null ? (int)pg.TAR_GR : 0 : 0;
                }
                // Создадим строку в САП (id состава)
                trWagon wag = new trWagon()
                {
                    Position = pv != null ? pv.NPP : 0,
                    CarriageNumber = pv.N_VAG,
                    CountryCode = pv != null ? pv.KOD_STRAN != null ? ((int)pv.KOD_STRAN * 10) + 1 : 0 : 0,
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

                SetWagonToSAPSupply(wag, id_sostav);
                return false;
            }
            return true;
        }

    }
}
