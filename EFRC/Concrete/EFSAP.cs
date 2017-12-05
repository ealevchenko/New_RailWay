using EFRC.Abstract;
using EFRC.Entities;
using MessageLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libClass;
using System.Data.SqlClient;

namespace EFRC.Concrete
{
    public class EFSAP:ISAP
    {
        private eventID eventID = eventID.EFSAP;

        protected EFDbContext context = new EFDbContext();

        #region Общие

        public IQueryable<SAPIncSupply> SAPIncSupply
        {
            get { return context.SAPIncSupply; }
        }

        public IQueryable<SAPIncSupply> GetSAPIncSupply()
        {
            try
            {
                return SAPIncSupply;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetSAPIncSupply()"), eventID);
                return null;
            }
        }

        public SAPIncSupply GetSAPIncSupply(int id)
        {
            try
            {
                return GetSAPIncSupply().Where(s => s.ID == id).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetSAPIncSupply(id={0})", id), eventID);
                return null;
            }
        }

        public int SaveSAPIncSupply(SAPIncSupply SAPIncSupply)
        {
            SAPIncSupply dbEntry;
            try
            {
                if (SAPIncSupply.ID == 0)
                {
                    dbEntry = new SAPIncSupply()
                    {
                        ID = SAPIncSupply.ID,
                        DateTime = SAPIncSupply.DateTime,
                        CompositionIndex = SAPIncSupply.CompositionIndex,
                        IDMTSostav = SAPIncSupply.IDMTSostav,
                        CarriageNumber = SAPIncSupply.CarriageNumber,
                        Position = SAPIncSupply.Position,
                        NumNakl = SAPIncSupply.NumNakl,
                        CountryCode = SAPIncSupply.CountryCode,
                        IDCountry = SAPIncSupply.IDCountry,
                        WeightDoc = SAPIncSupply.WeightDoc,
                        DocNumReweighing = SAPIncSupply.DocNumReweighing,
                        DocDataReweighing = SAPIncSupply.DocDataReweighing,
                        WeightReweighing = SAPIncSupply.WeightReweighing,
                        DateTimeReweighing = SAPIncSupply.DateTimeReweighing,
                        PostReweighing = SAPIncSupply.PostReweighing,
                        CodeCargo = SAPIncSupply.CodeCargo,
                        IDCargo = SAPIncSupply.IDCargo,
                        CodeMaterial = SAPIncSupply.CodeMaterial,
                        NameMaterial = SAPIncSupply.NameMaterial,
                        CodeStationShipment = SAPIncSupply.CodeStationShipment,
                        NameStationShipment = SAPIncSupply.NameStationShipment,
                        CodeShop = SAPIncSupply.CodeShop,
                        NameShop = SAPIncSupply.NameShop,
                        CodeNewShop = SAPIncSupply.CodeNewShop,
                        NameNewShop = SAPIncSupply.NameNewShop,
                        PermissionUnload = SAPIncSupply.PermissionUnload,
                        Step1 = SAPIncSupply.Step1,
                        Step2 = SAPIncSupply.Step2
                    };
                    context.SAPIncSupply.Add(dbEntry);
                }
                else
                {
                    dbEntry = context.SAPIncSupply.Find(SAPIncSupply.ID);
                    if (dbEntry != null)
                    {
                        dbEntry.DateTime = SAPIncSupply.DateTime;
                        dbEntry.CompositionIndex = SAPIncSupply.CompositionIndex;
                        dbEntry.IDMTSostav = SAPIncSupply.IDMTSostav;
                        dbEntry.CarriageNumber = SAPIncSupply.CarriageNumber;
                        dbEntry.Position = SAPIncSupply.Position;
                        dbEntry.NumNakl = SAPIncSupply.NumNakl;
                        dbEntry.CountryCode = SAPIncSupply.CountryCode;
                        dbEntry.IDCountry = SAPIncSupply.IDCountry;
                        dbEntry.WeightDoc = SAPIncSupply.WeightDoc;
                        dbEntry.DocNumReweighing = SAPIncSupply.DocNumReweighing;
                        dbEntry.DocDataReweighing = SAPIncSupply.DocDataReweighing;
                        dbEntry.WeightReweighing = SAPIncSupply.WeightReweighing;
                        dbEntry.DateTimeReweighing = SAPIncSupply.DateTimeReweighing;
                        dbEntry.PostReweighing = SAPIncSupply.PostReweighing;
                        dbEntry.CodeCargo = SAPIncSupply.CodeCargo;
                        dbEntry.IDCargo = SAPIncSupply.IDCargo;
                        dbEntry.CodeMaterial = SAPIncSupply.CodeMaterial;
                        dbEntry.NameMaterial = SAPIncSupply.NameMaterial;
                        dbEntry.CodeStationShipment = SAPIncSupply.CodeStationShipment;
                        dbEntry.NameStationShipment = SAPIncSupply.NameStationShipment;
                        dbEntry.CodeShop = SAPIncSupply.CodeShop;
                        dbEntry.NameShop = SAPIncSupply.NameShop;
                        dbEntry.CodeNewShop = SAPIncSupply.CodeNewShop;
                        dbEntry.NameNewShop = SAPIncSupply.NameNewShop;
                        dbEntry.PermissionUnload = SAPIncSupply.PermissionUnload;
                        dbEntry.Step1 = SAPIncSupply.Step1;
                        dbEntry.Step2 = SAPIncSupply.Step2;
                    }
                }
                context.SaveChanges();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveSAPIncSupply(SAPIncSupply={0})", SAPIncSupply.GetFieldsAndValue()), eventID);
                return -1;
            }
            return dbEntry.ID;
        }

        public SAPIncSupply DeleteSAPIncSupply(int id)
        {
            SAPIncSupply dbEntry = context.SAPIncSupply.Find(id);
            if (dbEntry != null)
            {
                try
                {
                    context.SAPIncSupply.Remove(dbEntry);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    e.WriteErrorMethod(String.Format("DeleteSAPIncSupply(id={0})", id), eventID);
                    return null;
                }
            }
            return dbEntry;
        }

        #endregion
        /// <summary>
        /// Получить список вагонов из справочника пренадлежащих составу
        /// </summary>
        /// <param name="idsostav"></param>
        /// <returns></returns>
        public IQueryable<SAPIncSupply> GetSAPIncSupplyOfSostav(int idsostav)
        {
            try
            {
                return GetSAPIncSupply().Where(s => s.IDMTSostav == idsostav).OrderBy(s => s.Position);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetSAPIncSupplyOfSostav(idsostav={0})", idsostav), eventID);
                return null;
            }
        }
        /// <summary>
        /// Вернуть строку по id сотава и номеру вагона
        /// </summary>
        /// <param name="idsostav"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public SAPIncSupply GetSAPIncSupply(int idsostav, int num)
        {
            try
            {
                return GetSAPIncSupplyOfSostav(idsostav).Where(s => s.CarriageNumber == num).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetSAPIncSupply(idsostav={0}, num={1})", idsostav, num), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить список номеров вагонов из справочника пренадлежащих составу
        /// </summary>
        /// <param name="idsostav"></param>
        /// <returns></returns>
        public List<int> GetSAPIncSupplyToNumWagons(int idsostav)
        {
            List<int> wagons = new List<int>();
            foreach (SAPIncSupply sap in GetSAPIncSupplyOfSostav(idsostav))
            {
                wagons.Add(sap.CarriageNumber);
            }
            return wagons;
        }
        /// <summary>
        /// Обновить id состава
        /// </summary>
        /// <param name="new_idsostav"></param>
        /// <param name="old_idsostav"></param>
        /// <returns></returns>
        public int UpdateSAPIncSupplyIDSostav(int new_idsostav, int old_idsostav)
        {
            try
            {
                SqlParameter new_id = new SqlParameter("@new", new_idsostav);
                SqlParameter old_id = new SqlParameter("@old", old_idsostav);
                return context.Database.ExecuteSqlCommand("UPDATE RailWay.SAP_Inc_Supply set IDMTSostav=@new where IDMTSostav = @old", new_id, old_id);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("UpdateSAPIncSupplyIDSostav(new_idsostav={0}, old_idsostav={1})", new_idsostav, old_idsostav), eventID);
                return -1;
            }
        }
        /// <summary>
        /// обновить позицию вагона в составе
        /// </summary>
        /// <param name="idsostav"></param>
        /// <param name="numvag"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public int UpdateSAPIncSupplyPosition(int idsostav, int numvag, int position)
        {
            try
            {
                SqlParameter id = new SqlParameter("@idsostav", idsostav);
                SqlParameter num = new SqlParameter("@num", numvag);
                SqlParameter pos = new SqlParameter("@pos", position);
                return context.Database.ExecuteSqlCommand("UPDATE RailWay.SAP_Inc_Supply set Position=@pos where IDMTSostav = @idsostav and CarriageNumber = @num", pos, id, num);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("UpdateSAPIncSupplyPosition(idsostav={0}, numvag={1}, position={2})", idsostav, numvag, position), eventID);
                return -1;
            }
        }
        /// <summary>
        /// Удалить строку справочника по номеру состава
        /// </summary>
        /// <param name="idsostav"></param>
        /// <param name="numvag"></param>
        /// <returns></returns>
        public int DeleteSAPIncSupplySostav(int idsostav)
        {
            try
            {
                SqlParameter id = new SqlParameter("@idsostav", idsostav);
                return context.Database.ExecuteSqlCommand("Delete RailWay.SAP_Inc_Supply where IDMTSostav = @idsostav", id);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("DeleteSAPIncSupplySostav(idsostav={0})", idsostav), eventID);
                return -1;
            }
        }
        /// <summary>
        /// Удалить строку справочника по номеру состава и вагона
        /// </summary>
        /// <param name="idsostav"></param>
        /// <param name="numvag"></param>
        /// <returns></returns>
        public int DeleteSAPIncSupply(int idsostav, int numvag)
        {
            try
            {
                SqlParameter id = new SqlParameter("@idsostav", idsostav);
                SqlParameter num = new SqlParameter("@num", numvag);
                return context.Database.ExecuteSqlCommand("Delete RailWay.SAP_Inc_Supply where IDMTSostav = @idsostav and CarriageNumber = @num", id, num);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("DeleteSAPIncSupply(idsostav={0}, numvag={1})", idsostav, numvag), eventID);
                return -1;
            }
        }
        /// <summary>
        /// Вернуть количество записей по составу
        /// </summary>
        /// <param name="idsostav"></param>
        /// <returns></returns>
        public int CountSAPIncSupply(int idsostav)
        {
            IQueryable<SAPIncSupply> list = GetSAPIncSupplyOfSostav(idsostav);
            return list != null ? list.Count() : 0;
        }
        /// <summary>
        /// Получить IDSostav по умолчанию (если в таблицах MT нет данного состава тогда ему присваивается id по умолчанию (отрицательное)
        /// </summary>
        /// <returns></returns>
        public int GetDefaultIDSAPIncSupply()
        {
            try
            {
                SAPIncSupply sap_s = GetSAPIncSupply().Where(s => s.IDMTSostav < 0).OrderBy(s => s.IDMTSostav).FirstOrDefault();
                return sap_s != null ? sap_s.IDMTSostav - 1 : -1;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetDefaultIDSAPIncSupply()"), eventID);
                return -1;
            }
        }
        /// <summary>
        /// Проверка наличия вагона в справочнике САП
        /// </summary>
        /// <param name="idsostav"></param>
        /// <param name="vagon"></param>
        /// <returns></returns>
        public bool IsWagonToSAPSupply(int idsostav, int vagon)
        {
            SAPIncSupply sap = GetSAPIncSupply(idsostav, vagon);
            return sap != null ? true : false;
        }

    }
}
