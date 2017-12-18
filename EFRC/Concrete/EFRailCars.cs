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
using RWConversionFunctions;


namespace EFRC.Concrete
{
    public class EFRailCars : IRC
    {
        private eventID eventID = eventID.EFRailCars;

        protected EFDbContext context = new EFDbContext();

        #region VAGON_OPERATIONS

        #region Общие
        public IQueryable<VAGON_OPERATIONS> VAGON_OPERATIONS
        {
            get { return context.VAGON_OPERATIONS; }
        }

        public IQueryable<VAGON_OPERATIONS> GetVAGON_OPERATIONS()
        {
            try
            {
                return VAGON_OPERATIONS;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetVAGON_OPERATIONS()"), eventID);
                return null;
            }
        }

        public VAGON_OPERATIONS GetVAGON_OPERATIONS(int id_oper)
        {
            try
            {
                return GetVAGON_OPERATIONS().Where(c => c.id_oper == id_oper).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetVAGON_OPERATIONS(id_oper={0})", id_oper), eventID);
                return null;
            }
        }

        public int SaveVAGON_OPERATIONS(VAGON_OPERATIONS VAGON_OPERATIONS)
        {
            VAGON_OPERATIONS dbEntry;
            try
            {
                if (VAGON_OPERATIONS.id_oper == 0)
                {
                    dbEntry = new VAGON_OPERATIONS()
                    {
                        id_oper = 0,
                        dt_uz = VAGON_OPERATIONS.dt_uz,
                        dt_amkr = VAGON_OPERATIONS.dt_amkr,
                        dt_out_amkr = VAGON_OPERATIONS.dt_out_amkr,
                        n_natur = VAGON_OPERATIONS.n_natur,
                        id_vagon = VAGON_OPERATIONS.id_vagon,
                        id_stat = VAGON_OPERATIONS.id_stat,
                        dt_from_stat = VAGON_OPERATIONS.dt_from_stat,
                        dt_on_stat = VAGON_OPERATIONS.dt_on_stat,
                        id_way = VAGON_OPERATIONS.id_way,
                        dt_from_way = VAGON_OPERATIONS.dt_from_way,
                        dt_on_way = VAGON_OPERATIONS.dt_on_way,
                        num_vag_on_way = VAGON_OPERATIONS.num_vag_on_way,
                        is_present = VAGON_OPERATIONS.is_present,
                        id_locom = VAGON_OPERATIONS.id_locom,
                        id_locom2 = VAGON_OPERATIONS.id_locom2,
                        id_cond2 = VAGON_OPERATIONS.id_cond2,
                        id_gruz = VAGON_OPERATIONS.id_gruz,
                        id_gruz_amkr = VAGON_OPERATIONS.id_gruz_amkr,
                        id_shop_gruz_for = VAGON_OPERATIONS.id_shop_gruz_for,
                        weight_gruz = VAGON_OPERATIONS.weight_gruz,
                        id_tupik = VAGON_OPERATIONS.id_tupik,
                        id_nazn_country = VAGON_OPERATIONS.id_nazn_country,
                        id_gdstait = VAGON_OPERATIONS.id_gdstait,
                        id_cond = VAGON_OPERATIONS.id_cond,
                        note = VAGON_OPERATIONS.note,
                        is_hist = VAGON_OPERATIONS.is_hist,
                        id_oracle = VAGON_OPERATIONS.id_oracle,
                        lock_id_way = VAGON_OPERATIONS.lock_id_way,
                        lock_order = VAGON_OPERATIONS.lock_order,
                        lock_side = VAGON_OPERATIONS.lock_side,
                        lock_id_locom = VAGON_OPERATIONS.lock_id_locom,
                        st_lock_id_stat = VAGON_OPERATIONS.st_lock_id_stat,
                        st_lock_order = VAGON_OPERATIONS.st_lock_order,
                        st_lock_train = VAGON_OPERATIONS.st_lock_train,
                        st_lock_side = VAGON_OPERATIONS.st_lock_side,
                        st_gruz_front = VAGON_OPERATIONS.st_gruz_front,
                        st_shop = VAGON_OPERATIONS.st_shop,
                        oracle_k_st = VAGON_OPERATIONS.oracle_k_st,
                        st_lock_locom1 = VAGON_OPERATIONS.st_lock_locom1,
                        st_lock_locom2 = VAGON_OPERATIONS.st_lock_locom2,
                        id_oper_parent = VAGON_OPERATIONS.id_oper_parent,
                        grvu_SAP = VAGON_OPERATIONS.grvu_SAP,
                        ngru_SAP = VAGON_OPERATIONS.ngru_SAP,
                        id_ora_23_temp = VAGON_OPERATIONS.id_ora_23_temp,
                        edit_user = VAGON_OPERATIONS.edit_user,
                        edit_dt = VAGON_OPERATIONS.edit_dt,
                        IDSostav = VAGON_OPERATIONS.IDSostav,
                        num_vagon = VAGON_OPERATIONS.num_vagon,
                    };
                    context.VAGON_OPERATIONS.Add(dbEntry);
                }
                else
                {
                    dbEntry = context.VAGON_OPERATIONS.Find(VAGON_OPERATIONS.id_oper);
                    if (dbEntry != null)
                    {
                        dbEntry.dt_uz = VAGON_OPERATIONS.dt_uz;
                        dbEntry.dt_amkr = VAGON_OPERATIONS.dt_amkr;
                        dbEntry.dt_out_amkr = VAGON_OPERATIONS.dt_out_amkr;
                        dbEntry.n_natur = VAGON_OPERATIONS.n_natur;
                        dbEntry.id_vagon = VAGON_OPERATIONS.id_vagon;
                        dbEntry.id_stat = VAGON_OPERATIONS.id_stat;
                        dbEntry.dt_from_stat = VAGON_OPERATIONS.dt_from_stat;
                        dbEntry.dt_on_stat = VAGON_OPERATIONS.dt_on_stat;
                        dbEntry.id_way = VAGON_OPERATIONS.id_way;
                        dbEntry.dt_from_way = VAGON_OPERATIONS.dt_from_way;
                        dbEntry.dt_on_way = VAGON_OPERATIONS.dt_on_way;
                        dbEntry.num_vag_on_way = VAGON_OPERATIONS.num_vag_on_way;
                        dbEntry.is_present = VAGON_OPERATIONS.is_present;
                        dbEntry.id_locom = VAGON_OPERATIONS.id_locom;
                        dbEntry.id_locom2 = VAGON_OPERATIONS.id_locom2;
                        dbEntry.id_cond2 = VAGON_OPERATIONS.id_cond2;
                        dbEntry.id_gruz = VAGON_OPERATIONS.id_gruz;
                        dbEntry.id_gruz_amkr = VAGON_OPERATIONS.id_gruz_amkr;
                        dbEntry.id_shop_gruz_for = VAGON_OPERATIONS.id_shop_gruz_for;
                        dbEntry.weight_gruz = VAGON_OPERATIONS.weight_gruz;
                        dbEntry.id_tupik = VAGON_OPERATIONS.id_tupik;
                        dbEntry.id_nazn_country = VAGON_OPERATIONS.id_nazn_country;
                        dbEntry.id_gdstait = VAGON_OPERATIONS.id_gdstait;
                        dbEntry.id_cond = VAGON_OPERATIONS.id_cond;
                        dbEntry.note = VAGON_OPERATIONS.note;
                        dbEntry.is_hist = VAGON_OPERATIONS.is_hist;
                        dbEntry.id_oracle = VAGON_OPERATIONS.id_oracle;
                        dbEntry.lock_id_way = VAGON_OPERATIONS.lock_id_way;
                        dbEntry.lock_order = VAGON_OPERATIONS.lock_order;
                        dbEntry.lock_side = VAGON_OPERATIONS.lock_side;
                        dbEntry.lock_id_locom = VAGON_OPERATIONS.lock_id_locom;
                        dbEntry.st_lock_id_stat = VAGON_OPERATIONS.st_lock_id_stat;
                        dbEntry.st_lock_order = VAGON_OPERATIONS.st_lock_order;
                        dbEntry.st_lock_train = VAGON_OPERATIONS.st_lock_train;
                        dbEntry.st_lock_side = VAGON_OPERATIONS.st_lock_side;
                        dbEntry.st_gruz_front = VAGON_OPERATIONS.st_gruz_front;
                        dbEntry.st_shop = VAGON_OPERATIONS.st_shop;
                        dbEntry.oracle_k_st = VAGON_OPERATIONS.oracle_k_st;
                        dbEntry.st_lock_locom1 = VAGON_OPERATIONS.st_lock_locom1;
                        dbEntry.st_lock_locom2 = VAGON_OPERATIONS.st_lock_locom2;
                        dbEntry.id_oper_parent = VAGON_OPERATIONS.id_oper_parent;
                        dbEntry.grvu_SAP = VAGON_OPERATIONS.grvu_SAP;
                        dbEntry.ngru_SAP = VAGON_OPERATIONS.ngru_SAP;
                        dbEntry.id_ora_23_temp = VAGON_OPERATIONS.id_ora_23_temp;
                        dbEntry.edit_user = VAGON_OPERATIONS.edit_user;
                        dbEntry.edit_dt = VAGON_OPERATIONS.edit_dt;
                        dbEntry.IDSostav = VAGON_OPERATIONS.IDSostav;
                        dbEntry.num_vagon = VAGON_OPERATIONS.num_vagon;
                    }
                }
                context.SaveChanges();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveVAGON_OPERATIONS(VAGON_OPERATIONS={0})", VAGON_OPERATIONS.GetFieldsAndValue()), eventID);
                return -1;
            }
            return dbEntry.id_oper;
        }

        public VAGON_OPERATIONS DeleteVAGON_OPERATIONS(int id_oper)
        {
            VAGON_OPERATIONS dbEntry = context.VAGON_OPERATIONS.Find(id_oper);
            if (dbEntry != null)
            {
                try
                {
                    context.VAGON_OPERATIONS.Remove(dbEntry);

                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    e.WriteErrorMethod(String.Format("DeleteVAGON_OPERATIONS(id_oper={0})", id_oper), eventID);
                    return null;
                }
            }
            return dbEntry;
        }
        #endregion

        /// <summary>
        /// Удалить вагоны пренадлежащие составу перенесеному по данным металлург транс 
        /// </summary>
        /// <param name="id_sostav"></param>
        /// <returns></returns>
        public int DeleteVagonsToInsertMT(int id_sostav)
        {
            try
            {
                return context.Database.ExecuteSqlCommand("DELETE FROM dbo.VAGON_OPERATIONS WHERE IDSostav=" + id_sostav.ToString());
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("DeleteVagonsToInsertMT(id_sostav:{0})", id_sostav), eventID);
                return -1;
            }
        }
        /// <summary>
        /// Вернуть операции над вагонами по указаному id составу МТ и дате захода на АМКР
        /// </summary>
        /// <param name="id_sostav"></param>
        /// <param name="dt_amkr"></param>
        /// <returns></returns>
        public IQueryable<VAGON_OPERATIONS> GetVagonsOperationsToMTSostav(int id_sostav, DateTime dt_amkr)
        {
            try
            {
                return GetVAGON_OPERATIONS().Where(o => o.IDSostav == id_sostav & o.dt_amkr == dt_amkr).OrderBy(o => o.num_vag_on_way);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetVagonsOperationsToMTSostav(id_sostav:{0}, dt_amkr:{1})", id_sostav, dt_amkr), eventID);
                return null;
            }

        }
        /// <summary>
        /// Вернуть операции над вагонами по указаному id составу МТ и дате захода на АМКР
        /// </summary>
        /// <param name="id_sostav"></param>
        /// <param name="dt_amkr"></param>
        /// <param name="id_vagon"></param>
        /// <returns></returns>
        public VAGON_OPERATIONS GetVagonsOperationsToMTSostav(int id_sostav, DateTime dt_amkr, int id_vagon)
        {
            try
            {
                return GetVagonsOperationsToMTSostav(id_sostav, dt_amkr).Where(o => o.id_vagon == id_vagon).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetVagonsOperationsToMTSostav(id_sostav:{0}, dt_amkr:{1}, id_vagon:{2})", id_sostav, dt_amkr, id_vagon), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить вагоны по составу
        /// </summary>
        /// <param name="id_sostav"></param>
        /// <returns></returns>
        public IQueryable<VAGON_OPERATIONS> GetVagonsOperations(int id_sostav)
        {
            try
            {
                return GetVAGON_OPERATIONS().Where(o => o.IDSostav == id_sostav);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetVagonsOperations(id_sostav:{0})", id_sostav), eventID);
                return null;
            }
        }
        /// <summary>
        /// Вернуть операции по указаному составу с группировкой по вагонам
        /// </summary>
        /// <param name="id_sostav"></param>
        /// <returns></returns>
        public IQueryable<IGrouping<int?, VAGON_OPERATIONS>> GetVagonsOperationsGroupingVagon(int id_sostav)
        {
            return GetVagonsOperations(id_sostav).GroupBy(o => o.num_vagon);
        }
        /// <summary>
        /// Получить список операций над указаным вагоном по указаному составу с сортировкой по убыванию 
        /// </summary>
        /// <param name="id_sostav"></param>
        /// <param name="num_vag"></param>
        /// <returns></returns>
        public IQueryable<VAGON_OPERATIONS> GetVagonsOperations(int id_sostav, int num_vag)
        {
            try
            {
                return GetVagonsOperations(id_sostav).Where(o => o.num_vagon == num_vag).OrderByDescending(o => o.id_oper);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetVagonsOperations(id_sostav:{0}, num_vag:{1})", id_sostav, num_vag), eventID);
                return null;
            }
        }
        /// <summary>
        /// Удалить операции пренадлежащие указаному составу и вагону 
        /// </summary>
        /// <param name="id_sostav"></param>
        /// <param name="num_vag"></param>
        /// <returns></returns>
        public int DeleteVagonsOperations(int id_sostav, int num_vag)
        {
            try
            {
                //TODO: Now При удалении операций с вагонами можно предусматреть коррекцию их на пути
                SqlParameter IDSostav = new SqlParameter("@IDSostav", id_sostav);
                SqlParameter NumVag = new SqlParameter("@NumVag", num_vag);
                return context.Database.ExecuteSqlCommand("DELETE FROM [dbo].[VAGON_OPERATIONS] where [IDSostav] = @IDSostav and [num_vagon]= @NumVag", IDSostav, NumVag);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("DeleteVagonsOperations(id_sostav={0}, num_vag={1})", id_sostav, num_vag), eventID);
                return -1;
            }
        }
        /// <summary>
        /// Обновить номер состава 
        /// </summary>
        /// <param name="id_sostav"></param>
        /// <param name="num_vag"></param>
        /// <param name="new_id_sostav"></param>
        /// <returns></returns>
        public int UpdateIDSostavVagonsOperations(int id_sostav, int num_vag, int new_id_sostav)
        {
            try
            {
                SqlParameter IDSostav = new SqlParameter("@IDSostav", id_sostav);
                SqlParameter NumVag = new SqlParameter("@NumVag", num_vag);
                SqlParameter NewIDSostav = new SqlParameter("@NewIDSostav", new_id_sostav);
                return context.Database.ExecuteSqlCommand("UPDATE [dbo].[VAGON_OPERATIONS] SET [IDSostav] = @NewIDSostav WHERE [IDSostav] = @IDSostav and [num_vagon]= @NumVag", NewIDSostav, IDSostav, NumVag);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("UpdateIDSostavVagonsOperations(id_sostav={0}, num_vag={1}, new_id_sostav={2})", id_sostav, num_vag, new_id_sostav), eventID);
                return -1;
            }
        }
        /// <summary>
        /// Операция над вагоном с указаным id состава МТ и датой захода существует? 
        /// </summary>
        /// <param name="id_sostav"></param>
        /// <param name="dt_amkr"></param>
        /// <param name="id_vagon"></param>
        /// <returns></returns>
        public bool IsVagonOperationMT(int id_sostav, DateTime dt_amkr, int id_vagon)
        {
            VAGON_OPERATIONS vo = GetVagonsOperationsToMTSostav(id_sostav, dt_amkr, id_vagon);
            return vo != null ? true : false;
        }
        /// <summary>
        /// Вернуть последний по порядку вагон на пути
        /// </summary>
        /// <param name="id_way"></param>
        /// <returns></returns>
        public int? MaxPositionWay(int id_way)
        {
            try
            {
                return GetVAGON_OPERATIONS().Where(o => o.id_way == id_way & o.is_present == 1).Max(o => o.num_vag_on_way);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("MaxPositionWay(id_way:{0})", id_way), eventID);
                return null;
            }

        }
        /// <summary>
        /// Поставить вагон на станцию по данным КИС
        /// </summary>
        /// <param name="natur"></param>
        /// <param name="dt_amkr"></param>
        /// <param name="id_vagon"></param>
        /// <param name="id_station"></param>
        /// <param name="id_way"></param>
        /// <param name="id_stat_kis"></param>
        /// <returns></returns>
        public int InsertVagon(int natur, DateTime dt_amkr, int id_vagon, int num_vagon, int? id_sostav, DateTime? dt_uz, int id_station, int id_way, int id_stat_kis, int? id_cond, int? id_cond2)
        {
            int? position = MaxPositionWay(id_way);
            if (position != null)
            { position++; }
            else { position = 1; }
            VAGON_OPERATIONS vo = new VAGON_OPERATIONS()
            {
                id_oper = 0,
                dt_uz = dt_uz,
                dt_amkr = dt_amkr,
                dt_out_amkr = null,
                n_natur = natur,
                id_vagon = id_vagon,
                id_stat = id_station,
                dt_from_stat = null,
                dt_on_stat = dt_amkr,
                id_way = id_way,
                dt_from_way = null,
                dt_on_way = dt_amkr,
                num_vag_on_way = position,
                is_present = 1,
                id_locom = null,
                id_locom2 = null,
                id_cond2 = id_cond2,
                id_gruz = null,
                id_gruz_amkr = null,
                id_shop_gruz_for = null,
                weight_gruz = null,
                id_tupik = null,
                id_nazn_country = null,
                id_gdstait = null,
                id_cond = id_cond,
                note = null,
                is_hist = 0,
                id_oracle = null,
                lock_id_way = null,
                lock_order = null,
                lock_side = null,
                lock_id_locom = null,
                st_lock_id_stat = null,
                st_lock_order = null,
                st_lock_train = null,
                st_lock_side = null,
                st_gruz_front = null,
                st_shop = null,
                oracle_k_st = null,
                st_lock_locom1 = null,
                st_lock_locom2 = null,
                id_oper_parent = null,
                grvu_SAP = null,
                ngru_SAP = null,
                id_ora_23_temp = null,
                edit_user = null,
                edit_dt = null,
                IDSostav = id_sostav,
                num_vagon = num_vagon,
            };
            return SaveVAGON_OPERATIONS(vo);
        }
        /// <summary>
        /// Поставить вагон в прибитие на станции АМКР из станций Кривого Рога
        /// </summary>
        /// <param name="IDSostav"></param>
        /// <param name="id_vagon"></param>
        /// <param name="dt_amkr"></param>
        /// <param name="id_station_from"></param>
        /// <param name="position"></param>
        /// <param name="id_gruz"></param>
        /// <param name="weight_gruz"></param>
        /// <param name="id_station_in"></param>
        /// <param name="num_train"></param>
        /// <returns></returns>
        public int InsertVagon(int IDSostav, int id_vagon, int num_vagon, DateTime dt_uz_on, DateTime dt_uz_from, int id_station_from, int position, int? id_gruz, decimal weight_gruz, int id_station_in, int num_train, int id_cond2, int way_from)
        {
            //TODO: !!ДОРАБОТАТЬ (ДОБАВИТЬ В ПРИБЫТИЕ С УЗ) - убрать id_vagon,id_gruz,weight_gruz (эти данные берутся из справочника САП входящие поставки по (dt_uz)dt_amkr и num_vagon)
            VAGON_OPERATIONS vo = new VAGON_OPERATIONS()
            {
                id_oper = 0,
                dt_uz = dt_uz_from,
                dt_amkr = null, // 
                dt_out_amkr = null,
                n_natur = null,
                id_vagon = id_vagon,
                id_stat = id_station_from,
                dt_from_stat = dt_uz_from,
                dt_on_stat = dt_uz_on,
                id_way = way_from,
                dt_from_way = dt_uz_from,
                dt_on_way = dt_uz_on,
                num_vag_on_way = position,
                is_present = 0,
                id_locom = null,
                id_locom2 = null,
                id_cond2 = id_cond2, // 15
                id_gruz = id_gruz,
                id_gruz_amkr = id_gruz,
                id_shop_gruz_for = null,
                weight_gruz = weight_gruz,
                id_tupik = null,
                id_nazn_country = null,
                id_gdstait = null,
                id_cond = null,
                note = null,
                is_hist = 0,
                id_oracle = null,
                lock_id_way = null,
                lock_order = null,
                lock_side = null,
                lock_id_locom = null,
                st_lock_id_stat = id_station_in,
                st_lock_order = position,
                st_lock_train = num_train,
                st_lock_side = null,
                st_gruz_front = null,
                st_shop = null,
                oracle_k_st = null,
                st_lock_locom1 = null,
                st_lock_locom2 = null,
                id_oper_parent = null,
                grvu_SAP = null,
                ngru_SAP = null,
                id_ora_23_temp = null,
                edit_user = null,
                edit_dt = null,
                IDSostav = IDSostav,
                num_vagon = num_vagon,
            };
            return SaveVAGON_OPERATIONS(vo);
        }
        /// <summary>
        /// Обновить информацию по вагону поставленному на путь или принятому вручную.
        /// </summary>
        /// <param name="dt_amkr"></param>
        /// <param name="num_vagon"></param>
        /// <param name="natur"></param>
        /// <param name="idstation_amkr"></param>
        /// <param name="id_gruz"></param>
        /// <param name="id_shop"></param>
        /// <param name="id_cond"></param>
        /// <returns></returns>
        public int UpdateVagon(DateTime dt_amkr, int num_vagon, int natur, int[] idstation_amkr, int id_gruz, int id_shop, int? id_cond)
        {
            try
            {
                string idstation_amkr_s = idstation_amkr.IntsToString(',');
                string sql = "update  dbo.VAGON_OPERATIONS " +
                                "set id_gruz = " + id_gruz.ToString() + ", id_gruz_amkr = " + id_gruz.ToString() + ", id_shop_gruz_for = " + id_shop.ToString() +
                                ", id_cond = " + (id_cond != null ? id_cond.ToString() : "null ") +
                                " where n_natur= " + natur.ToString() +
                                " and num_vagon= " + num_vagon.ToString() +
                                " and convert(smalldatetime,dt_amkr,120) ='" + dt_amkr.ToString("yyyy-MM-dd HH:mm:ss") + "'" +
                                " and id_stat in(" + idstation_amkr_s + ")";
                return context.Database.ExecuteSqlCommand(sql);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("UpdateVagon(dt_amkr={0}, num_vagon={1}, natur={2}, idstation_amkr={3}, id_gruz={4}, id_shop={5}, id_shop={6})",
                    dt_amkr, num_vagon, natur, idstation_amkr.IntsToString(','), id_gruz, id_shop, id_cond), eventID);
                return -1;
            }
        }
        /// <summary>
        /// Обновить информацию по вагону принятому в ручну на станции
        /// </summary>
        /// <param name="id_sostav"></param>
        /// <param name="num_vagon"></param>
        /// <param name="dt_amkr"></param>
        /// <param name="id_cond"></param>
        /// <param name="natur"></param>
        /// <returns></returns>
        public int UpdateVagon(int id_sostav, int num_vagon, int[] idstation_amkr, DateTime dt_amkr, int? id_cond, int natur)
        {
            try
            {
                string idstation_amkr_s = idstation_amkr.IntsToString(',');
                string sql = "update  dbo.VAGON_OPERATIONS " +
                                "set dt_amkr = Convert(datetime,'" + dt_amkr.ToString("yyyy-MM-dd HH:mm:ss") + "',120)" +
                                ", id_cond = " + (id_cond != null ? id_cond.ToString() : "null ") +
                                ", n_natur = " + natur.ToString() +
                                " where IDSostav = " + id_sostav.ToString() +
                                " and num_vagon= " + num_vagon.ToString() +
                                " and id_stat in(" + idstation_amkr_s + ")";
                return context.Database.ExecuteSqlCommand(sql);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("UpdateVagon(id_sostav={0}, num_vagon={1}, idstation_amkr={2}, dt_amkr={3}, id_cond={4}, natur={5})", 
                    id_sostav, num_vagon, idstation_amkr.IntsToString(','), dt_amkr, id_cond, natur), eventID);
                return -1;
            }
        }
        /// <summary>
        /// Вернуть вагон прибывающий из станций УЗ на станцию АМКР по Id состава и номера вагона
        /// </summary>
        /// <param name="id_mtsostav"></param>
        /// <param name="num"></param>
        /// <param name="idstation_uz"></param>
        /// <param name="idstation"></param>
        /// <returns></returns>
        public VAGON_OPERATIONS GetVagonsOfArrivalUZ(int id_mtsostav, int num, int[] idstation_uz, int idstation)
        {
            try
            {
                string station_uz_s = idstation_uz.IntsToString(',');
                string sql = "SELECT * FROM dbo.VAGON_OPERATIONS where [IDSostav]=" + id_mtsostav.ToString() + " and [num_vagon] = " + num.ToString() + " and [id_stat] in(" + station_uz_s + ") and [st_lock_id_stat] = " + idstation.ToString();
                return context.Database.SqlQuery<VAGON_OPERATIONS>(sql).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetVagonsOfArrivalUZ(id_mtsostav={0}, num={1}, idstation_uz={2}, idstation={3})", id_mtsostav, num, idstation_uz.IntsToString(','), idstation), eventID);
                return null;
            }
        }
        /// <summary>
        /// Вернуть вагоны прибывающие из станций (int[] idstation_uz) по Id состава и номера вагона
        /// </summary>
        /// <param name="id_mtsostav"></param>
        /// <param name="num"></param>
        /// <param name="idstation_uz"></param>
        /// <returns></returns>
        public IQueryable<VAGON_OPERATIONS> GetVagonsOfArrival(int id_mtsostav, int num, int[] idstation_uz)
        {
            try
            {
                string station_uz_s = idstation_uz.IntsToString(',');
                string sql = "SELECT * FROM dbo.VAGON_OPERATIONS where [IDSostav]=" + id_mtsostav.ToString() + " and [num_vagon] = " + num.ToString() + " and [id_stat] in(" + station_uz_s + ") and [st_lock_id_stat] >0 ";
                return context.Database.SqlQuery<VAGON_OPERATIONS>(sql).AsQueryable();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetVagonsOfArrival(id_mtsostav={0}, num={1}, idstation_uz={2})", id_mtsostav, num, idstation_uz.IntsToString(',')), eventID);
                return null;
            }
        }
        /// <summary>
        /// Вернуть вагоны находящиеся на станциях АМКР принятые вручную
        /// </summary>
        /// <param name="id_mtsostav"></param>
        /// <param name="num"></param>
        /// <param name="idstation_amkr"></param>
        /// <returns></returns>
        public IQueryable<VAGON_OPERATIONS> GetVagonsOfStationAMKR(int id_mtsostav, int num, int[] idstation_amkr)
        {
            try
            {
                string station_amkr_s = idstation_amkr.IntsToString(',');
                string sql = "SELECT * FROM dbo.VAGON_OPERATIONS where [IDSostav]=" + id_mtsostav.ToString() + " and [num_vagon] = " + num.ToString() + " and [id_stat] in(" + station_amkr_s + ")";
                return context.Database.SqlQuery<VAGON_OPERATIONS>(sql).AsQueryable();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetVagonsOfStationAMKR(id_mtsostav={0}, num={1}, idstation_amkr={2})", id_mtsostav, num, idstation_amkr.IntsToString(',')), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить все вагоны находящиеся на станциях УЗ
        /// </summary>
        /// <param name="idstation_uz"></param>
        /// <returns></returns>
        public IQueryable<VAGON_OPERATIONS> GetVagonsOfArrival(int[] idstation_uz)
        {
            try
            {
                string station_uz_s = idstation_uz.IntsToString(',');
                string sql = "SELECT * FROM dbo.VAGON_OPERATIONS where [id_stat] in(" + station_uz_s + ")";
                return context.Database.SqlQuery<VAGON_OPERATIONS>(sql).AsQueryable();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetVagonsOfArrival(idstation_uz={0})", idstation_uz.IntsToString(',')), eventID);
                return null;
            }
        }
        /// <summary>
        /// Удалить записи вагонов прибывающие из станций (int[] idstation_uz) по Id состава и номера вагона
        /// </summary>
        /// <param name="id_mtsostav"></param>
        /// <param name="num"></param>
        /// <param name="idstation_uz"></param>
        /// <returns></returns>
        public int DeleteVagonsOfArrival(int id_mtsostav, int num, int[] idstation_uz)
        {
            try
            {
                string station_uz_s = idstation_uz.IntsToString(',');
                string sql = "DELETE FROM dbo.VAGON_OPERATIONS where [IDSostav]=" + id_mtsostav.ToString() + " and [num_vagon] = " + num.ToString() + " and [id_stat] in(" + station_uz_s + ") and [st_lock_id_stat] >0 ";
                return context.Database.ExecuteSqlCommand(sql);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("DeleteVagonsOfArrival(id_mtsostav={0}, num={1}, idstation_uz={2})", id_mtsostav, num, idstation_uz.IntsToString(',')), eventID);
                return -1;
            }
        }
        /// <summary>
        /// Принять вагон на станцию АМКР из станции УЗ
        /// </summary>
        /// <param name="vagon"></param>
        /// <param name="natur"></param>
        /// <param name="dt_amkr"></param>
        /// <param name="id_stations"></param>
        /// <param name="id_ways"></param>
        /// <returns></returns>
        public int TakeVagonOfUZ(VAGON_OPERATIONS vagon, int natur, DateTime dt_amkr, int id_stations, int id_ways, int id_cond)
        {
            int? position = MaxPositionWay(id_ways);
            if (position != null)
            { position++; }
            else { position = 1; }
            VAGON_OPERATIONS new_vagon = new VAGON_OPERATIONS()
            {
                id_oper = 0,
                dt_uz = vagon.dt_uz,
                dt_amkr = dt_amkr,
                dt_out_amkr = null,
                n_natur = natur,
                id_vagon = vagon.id_vagon,
                id_stat = id_stations,
                dt_from_stat = null,
                dt_on_stat = dt_amkr,
                id_way = id_ways,
                dt_from_way = null,
                dt_on_way = dt_amkr,
                num_vag_on_way = position,
                is_present = 1,
                id_locom = null,
                id_locom2 = null,
                id_cond2 = 15,
                id_gruz = vagon.id_gruz,
                id_gruz_amkr = vagon.id_gruz_amkr,
                id_shop_gruz_for = null,
                weight_gruz = vagon.weight_gruz,
                id_tupik = null,
                id_nazn_country = null,
                id_gdstait = null,
                id_cond = id_cond,
                note = null,
                is_hist = 0,
                id_oracle = null,
                lock_id_way = null,
                lock_order = null,
                lock_side = null,
                lock_id_locom = null,
                st_lock_id_stat = null,
                st_lock_order = null,
                st_lock_train = null,
                st_lock_side = null,
                st_gruz_front = null,
                st_shop = null,
                oracle_k_st = null,
                st_lock_locom1 = null,
                st_lock_locom2 = null,
                id_oper_parent = null,
                grvu_SAP = null,
                ngru_SAP = null,
                id_ora_23_temp = null,
                IDSostav = vagon.IDSostav,
                num_vagon = vagon.num_vagon
            };
            int res = SaveVAGON_OPERATIONS(new_vagon);
            if (res > 0)
            {
                vagon.is_hist = 1;
                vagon.st_lock_id_stat = null;
                vagon.st_lock_order = null;
                vagon.st_lock_train = null;
                vagon.st_lock_side = null;
                vagon.st_lock_locom1 = null;
                vagon.st_lock_locom2 = null;
                vagon.n_natur = natur;
                SaveVAGON_OPERATIONS(vagon);
            }
            return res;
        }
        /// <summary>
        /// Принять вагон на станцию АМКР из станции УЗ
        /// </summary>
        /// <param name="id_mtsostav"></param>
        /// <param name="num"></param>
        /// <param name="idstation_uz"></param>
        /// <param name="natur"></param>
        /// <param name="dt_amkr"></param>
        /// <param name="id_stations"></param>
        /// <param name="id_ways"></param>
        /// <returns></returns>
        public int TakeVagonOfUZ(int id_mtsostav, int num, int[] idstation_uz, int natur, DateTime dt_amkr, int id_stations, int id_ways, int id_cond)
        {
            int res = 0;
            VAGON_OPERATIONS vagon = GetVagonsOfArrivalUZ(id_mtsostav, num, idstation_uz, id_stations);
            if (vagon != null)
            {
                res = TakeVagonOfUZ(vagon, natur, dt_amkr, id_stations, id_ways, id_cond); // Примем вагон на станцию АМКР
                DeleteVagonsOfArrival(id_mtsostav, num, idstation_uz);             // Удалим с прибытия вагоны кроме принятого

            }
            return res;
        }
        /// <summary>
        /// Принять вагон на станцию АМКР из станции УЗ ( проверка идет по всем станциям УЗ)
        /// </summary>
        /// <param name="id_mtsostav"></param>
        /// <param name="num"></param>
        /// <param name="idstation_uz"></param>
        /// <param name="natur"></param>
        /// <param name="dt_amkr"></param>
        /// <param name="id_stations"></param>
        /// <param name="id_ways"></param>
        /// <param name="id_cond"></param>
        /// <returns></returns>
        public int TakeVagonOfAllUZ(int id_mtsostav, int num, int[] idstation_uz, int natur, DateTime dt_amkr, int id_stations, int id_ways, int id_cond)
        {
            int res = 0;
            IQueryable<VAGON_OPERATIONS> vagons_uz = GetVagonsOfArrival(id_mtsostav, num, idstation_uz);
            if (vagons_uz.Count() > 0)
            {
                res = TakeVagonOfUZ(vagons_uz.First(), natur, dt_amkr, id_stations, id_ways, id_cond); // Примем вагон на станцию АМКР
                DeleteVagonsOfArrival(id_mtsostav, num, idstation_uz);             // Удалим с прибытия вагоны кроме принятого
            }
            return res;
        }
        /// <summary>
        /// Получить вагоны на указаном пути
        /// </summary>
        /// <param name="way"></param>
        /// <returns></returns>
        public IQueryable<VAGON_OPERATIONS> GetWagonsOfWay(int way)
        {
            return GetVAGON_OPERATIONS().Where(o => o.id_way == way & o.is_present == 1);
        }
        /// <summary>
        /// Получить вагоны на указаной станции
        /// </summary>
        /// <param name="id_stat"></param>
        /// <returns></returns>
        public IQueryable<VAGON_OPERATIONS> GetWagonsOfStation(int id_stat)
        {
            return GetVAGON_OPERATIONS().Where(o => o.id_stat == id_stat & o.is_present == 1);
        }
        /// <summary>
        /// смищение(выравнивание) вагонов на пути с начальным номером
        /// </summary>
        /// <param name="way"></param>
        /// <param name="start_num"></param>
        public int OffSetCars(int way, int start_num)
        {
            try
            {
                int result = 0;
                List<VAGON_OPERATIONS> list = new List<VAGON_OPERATIONS>();
                list = GetWagonsOfWay(way).Where(o => o.lock_id_way == null & o.is_hist == 0).OrderBy(o => o.num_vag_on_way).ToList();

                foreach (VAGON_OPERATIONS wag in list)
                {
                    if (wag.num_vag_on_way != start_num)
                    {
                        wag.num_vag_on_way = start_num;
                        int res = SaveVAGON_OPERATIONS(wag);
                        if (res > 0) result++;
                        if (res < 0)
                        {
                            String.Format("Ошибка выполнения метода OffSetCars(way:{0},start_num:{1}) выравнивания позиции вагона №{2}, id_oper {3}", way, start_num, wag.num_vagon, wag.id_oper).WriteError(eventID);
                        }
                    }
                    start_num++;
                }
                return result;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("OffSetCars(way={0}, start_num={1})", way, start_num), eventID);
                return -1;
            }

        }
        /// <summary>
        /// Получить список вагонов отправленых на УЗ со станций АМКР
        /// </summary>
        /// <param name="idstation_uz"></param>
        /// <returns></returns>
        public IQueryable<VAGON_OPERATIONS> GetVagonsAMKRToUZ(int[] idstation_uz)
        {
            try
            {
                string station_uz_s = idstation_uz.IntsToString(',');
                string sql = "SELECT * FROM dbo.VAGON_OPERATIONS where [st_lock_id_stat] in(" + station_uz_s + ") and [is_present]=0 and [is_hist]=0";
                return context.Database.SqlQuery<VAGON_OPERATIONS>(sql).AsQueryable();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetVagonsAMKRToUZ(idstation_uz={0})", idstation_uz.IntsToString(',')), eventID);
                return null;
            }
        }
        /// <summary>
        /// Вернуть операции над вагонами по указаному натурному листу и дате захода на АМКР
        /// </summary>
        /// <param name="natur"></param>
        /// <param name="dt_amkr"></param>
        /// <returns></returns>
        public IQueryable<VAGON_OPERATIONS> GetVagonsOperationsToNatur(int natur, DateTime dt_amkr)
        {
            try
            {
                return GetVAGON_OPERATIONS().Where(o => o.n_natur == natur & o.dt_amkr == dt_amkr).OrderBy(o => o.num_vag_on_way);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetVagonsOperationsToNatur(natur={0}, dt_amkr={1})", natur, dt_amkr), eventID);
                return null;
            }
        }
        /// </summary>        
        /// Вернуть операцию по вагону по указаному натурному листу и дате захода на АМКР
        /// </summary>     
        /// <param name="natur"></param>
        /// <param name="dt_amkr"></param>
        /// <param name="num_vagon"></param>
        /// <returns></returns>
        public VAGON_OPERATIONS GetVagonsOperationsToNatur(int natur, DateTime dt_amkr, int num_vagon)
        {
            try
            {
                return GetVagonsOperationsToNatur(natur, dt_amkr).Where(o => o.num_vagon == num_vagon).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetVagonsOperationsToNatur(natur={0}, dt_amkr={1}, dt_amkr={2})", natur, dt_amkr, num_vagon), eventID);
                return null;
            }
        }
        /// <summary>
        /// Операция над вагоном с указаным натуральным листом и датой захода существует?
        /// </summary>
        /// <param name="natur"></param>
        /// <param name="dt_amkr"></param>
        /// <param name="num_vagon"></param>
        /// <returns></returns>
        public bool IsVagonOperationKIS(int natur, DateTime dt_amkr, int num_vagon)
        {
            try
            {
                VAGON_OPERATIONS vo = GetVagonsOperationsToNatur(natur, dt_amkr, num_vagon);
                return vo != null ? true : false;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("IsVagonOperationKIS(natur={0}, dt_amkr={1}, dt_amkr={2})", natur, dt_amkr, num_vagon), eventID);
                return false;
            }
        }
        #endregion

        #region VAGONS

        #region Общие
        public IQueryable<VAGONS> VAGONS
        {
            get { return context.VAGONS; }
        }

        public IQueryable<VAGONS> GetVAGONS()
        {
            try
            {
                return VAGONS;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetVAGONS()"), eventID);
                return null;
            }
        }

        public VAGONS GetVAGONS(int id_vag)
        {
            try
            {
                return GetVAGONS().Where(v => v.id_vag == id_vag).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetVAGONS(id_vag={0})", id_vag), eventID);
                return null;
            }
        }

        public int SaveVAGONS(VAGONS VAGONS)
        {
            VAGONS dbEntry;
            try
            {
                if (VAGONS.id_vag == 0)
                {
                    dbEntry = new VAGONS()
                    {
                        id_vag = VAGONS.id_vag,
                        num = VAGONS.num,
                        id_ora = VAGONS.id_ora,
                        id_owner = VAGONS.id_owner,
                        id_stat = VAGONS.id_stat,
                        is_locom = VAGONS.is_locom,
                        locom_seria = VAGONS.locom_seria,
                        rod = VAGONS.rod,
                        st_otpr = VAGONS.st_otpr,
                        date_ar = VAGONS.date_ar,
                        date_end = VAGONS.date_end,
                        date_in = VAGONS.date_in,
                        IDSostav = VAGONS.IDSostav,
                        Natur = VAGONS.Natur,
                        Transit = VAGONS.Transit
                    };
                    context.VAGONS.Add(dbEntry);
                }
                else
                {
                    dbEntry = context.VAGONS.Find(VAGONS.id_vag);
                    if (dbEntry != null)
                    {
                        dbEntry.num = VAGONS.num;
                        dbEntry.id_ora = VAGONS.id_ora;
                        dbEntry.id_owner = VAGONS.id_owner;
                        dbEntry.id_stat = VAGONS.id_stat;
                        dbEntry.is_locom = VAGONS.is_locom;
                        dbEntry.locom_seria = VAGONS.locom_seria;
                        dbEntry.rod = VAGONS.rod;
                        dbEntry.st_otpr = VAGONS.st_otpr;
                        dbEntry.date_ar = VAGONS.date_ar;
                        dbEntry.date_end = VAGONS.date_end;
                        dbEntry.date_in = VAGONS.date_in;
                        dbEntry.IDSostav = VAGONS.IDSostav;
                        dbEntry.Natur = VAGONS.Natur;
                        dbEntry.Transit = VAGONS.Transit;
                    }
                }
                context.SaveChanges();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveVAGONS(VAGONS={0})", VAGONS.GetFieldsAndValue()), eventID);
                return -1;
            }
            return dbEntry.id_vag;
        }

        public VAGONS DeleteVAGONS(int id_vag)
        {
            VAGONS dbEntry = context.VAGONS.Find(id_vag);
            if (dbEntry != null)
            {
                try
                {
                    context.VAGONS.Remove(dbEntry);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    e.WriteErrorMethod(String.Format("DeleteVAGONS(id_vag={0})", id_vag), eventID);
                    return null;
                }
            }
            return dbEntry;
        }
        #endregion
        /// <summary>
        /// Получить информацию по вагону по указаному номеру
        /// </summary>
        /// <param name="num_vag"></param>
        /// <returns></returns>
        public IQueryable<VAGONS> GetVagons(int num_vag)
        {
            try
            {
                return GetVAGONS().Where(v => v.num == num_vag).OrderByDescending(v => v.date_ar);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetVagons(num_vag={0})", num_vag), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить вагон по номеру и дате захода на АМКР
        /// </summary>
        /// <param name="num_vag"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public VAGONS GetVagons(int num_vag, DateTime dt)
        {
            try
            {
                return GetVagons(num_vag).Where(v => v.date_ar <= dt & v.date_end == null).OrderByDescending(v => v.date_ar).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetVagons(num_vag={0}, dt={1})", num_vag, dt), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить ID вагона по номеру и дате захода на АМКР
        /// </summary>
        /// <param name="num_vag"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public int? GetIDVagons(int num_vag, DateTime dt)
        {
            VAGONS vg = GetVagons(num_vag, dt);
            return vg != null ? (int?)vg.id_vag : null;
        }
        /// <summary>
        /// Получить времено созданый вагон по номеру и дате захода на АМКР
        /// </summary>
        /// <param name="num_vag"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public VAGONS GetNewVagons(int num_vag, DateTime dt)
        {
            try
            {
                return GetVagons(num_vag).Where(v => v.date_in <= dt & v.date_ar == null & v.date_end == null).OrderByDescending(v => v.date_in).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetNewVagons(num_vag={0}, dt={1})", num_vag, dt), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить ID временно созданого вагона по номеру и дате захода на АМКР
        /// </summary>
        /// <param name="num_vag"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public int? GetIDNewVagons(int num_vag, DateTime dt)
        {
            VAGONS vg = GetNewVagons(num_vag, dt);
            return vg != null ? (int?)vg.id_vag : null;
        }
        #endregion

        #region OWNERS
        #region Общие
        public IQueryable<OWNERS> OWNERS
        {
            get { return context.OWNERS; }
        }

        public IQueryable<OWNERS> GetOWNERS()
        {
            try
            {
                return OWNERS;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetOWNERS()"), eventID);
                return null;
            }
        }

        public OWNERS GetOWNERS(int id_owner)
        {
            try
            {
                return GetOWNERS().Where(o => o.id_owner == id_owner).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetOWNERS(id_owner={0})", id_owner), eventID);
                return null;
            }
        }

        public int SaveOWNERS(OWNERS OWNERS)
        {
            OWNERS dbEntry;
            try
            {
                if (OWNERS.id_owner == 0)
                {
                    dbEntry = new OWNERS()
                    {
                        id_owner = OWNERS.id_owner,
                        name = OWNERS.name,
                        abr = OWNERS.abr,
                        id_country = OWNERS.id_country,
                        id_ora = OWNERS.id_ora,
                        id_ora_temp = OWNERS.id_ora_temp
                    };
                    context.OWNERS.Add(dbEntry);
                }
                else
                {
                    dbEntry = context.OWNERS.Find(OWNERS.id_owner);
                    if (dbEntry != null)
                    {
                        dbEntry.id_owner = OWNERS.id_owner;
                        dbEntry.name = OWNERS.name;
                        dbEntry.abr = OWNERS.abr;
                        dbEntry.id_country = OWNERS.id_country;
                        dbEntry.id_ora = OWNERS.id_ora;
                        dbEntry.id_ora_temp = OWNERS.id_ora_temp;
                    }
                }
                context.SaveChanges();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveOWNERS(OWNERS={0})", OWNERS.GetFieldsAndValue()), eventID);
                return -1;
            }
            return dbEntry.id_owner;
        }

        public OWNERS DeleteOWNERS(int id_owner)
        {
            OWNERS dbEntry = context.OWNERS.Find(id_owner);
            if (dbEntry != null)
            {
                try
                {
                    context.OWNERS.Remove(dbEntry);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    e.WriteErrorMethod(String.Format("DeleteOWNERS(id_owner={0})", id_owner), eventID);
                    return null;
                }
            }
            return dbEntry;
        }
        #endregion
        /// <summary>
        /// Получить владельца по id системы КИС
        /// </summary>
        /// <param name="num_vag"></param>
        /// <returns></returns>
        public OWNERS GetOwnersOfKis(int id_sob_kis)
        {
            return GetOWNERS().Where(o => o.id_ora == id_sob_kis).FirstOrDefault();
        }
        /// <summary>
        /// Получить Id владельца по id системы КИС
        /// </summary>
        /// <param name="id_sob_kis"></param>
        /// <returns></returns>
        public int? GetIDOwnersOfKis(int id_sob_kis)
        {
            OWNERS ow = GetOwnersOfKis(id_sob_kis);
            return ow != null ? (int?)ow.id_owner : null;
        }
        #endregion

        #region GRUZS
        #region Общие
        public IQueryable<GRUZS> GRUZS
        {
            get { return context.GRUZS; }
        }

        public IQueryable<GRUZS> GetGRUZS()
        {
            try
            {
                return GRUZS;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetGRUZS()"), eventID);
                return null;
            }
        }

        public GRUZS GetGRUZS(int id_gruz)
        {
            try
            {
                return GetGRUZS().Where(g => g.id_gruz == id_gruz).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetGRUZS(id_gruz={0})", id_gruz), eventID);
                return null;
            }
        }

        public int SaveGRUZS(GRUZS GRUZS)
        {
            GRUZS dbEntry;
            try
            {
                if (GRUZS.id_gruz == 0)
                {
                    dbEntry = new GRUZS()
                    {
                        id_gruz = 0,
                        name = GRUZS.name,
                        name_full = GRUZS.name_full,
                        id_ora = GRUZS.id_ora,
                        id_ora2 = GRUZS.id_ora2,
                        ETSNG = GRUZS.ETSNG
                    };
                    context.GRUZS.Add(dbEntry);
                }
                else
                {
                    dbEntry = context.GRUZS.Find(GRUZS.id_gruz);
                    if (dbEntry != null)
                    {
                        dbEntry.name = GRUZS.name;
                        dbEntry.name_full = GRUZS.name_full;
                        dbEntry.id_ora = GRUZS.id_ora;
                        dbEntry.id_ora2 = GRUZS.id_ora2;
                        dbEntry.ETSNG = GRUZS.ETSNG;
                    }
                }
                context.SaveChanges();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveGRUZS(GRUZS={0})", GRUZS.GetFieldsAndValue()), eventID);
                return -1;
            }
            return dbEntry.id_gruz;
        }

        public GRUZS DeleteGRUZS(int id_gruz)
        {
            GRUZS dbEntry = context.GRUZS.Find(id_gruz);
            if (dbEntry != null)
            {
                try
                {
                    context.GRUZS.Remove(dbEntry);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    e.WriteErrorMethod(String.Format("DeleteGRUZS(id_gruz={0})", id_gruz), eventID);
                    return null;
                }
            }
            return dbEntry;
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_gruz_prom_kis"></param>
        /// <param name="id_gruz_vag_kis"></param>
        /// <returns></returns>
        public GRUZS GetGruzs(int? id_gruz_prom_kis, int? id_gruz_vag_kis)
        {
            try
            {
                if (id_gruz_prom_kis != null & id_gruz_vag_kis == null)
                {
                    return GetGRUZS().Where(g => g.id_ora == id_gruz_prom_kis).FirstOrDefault();
                }
                if (id_gruz_prom_kis == null & id_gruz_vag_kis != null)
                {
                    return GetGRUZS().Where(g => g.id_ora2 == id_gruz_vag_kis).FirstOrDefault();
                }
                if (id_gruz_prom_kis != null & id_gruz_vag_kis != null)
                {
                    return GetGRUZS().Where(g => g.id_ora == id_gruz_prom_kis & g.id_ora2 == id_gruz_vag_kis).FirstOrDefault();
                }
                return null;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetGruzs(id_gruz_prom_kis={0}, id_gruz_vag_kis={1})", id_gruz_prom_kis, id_gruz_vag_kis), eventID);
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_gruz_prom_kis"></param>
        /// <param name="id_gruz_vag_kis"></param>
        /// <returns></returns>
        public int? GetIDGruzs(int? id_gruz_prom_kis, int? id_gruz_vag_kis)
        {
            GRUZS gr = GetGruzs(id_gruz_prom_kis, id_gruz_vag_kis);
            return gr != null ? (int?)gr.id_gruz : null;
        }
        /// <summary>
        /// Получить груз по коду ЕТ СНГ
        /// </summary>
        /// <param name="etsng"></param>
        /// <returns></returns>
        public GRUZS GetGruzsToETSNG(int? etsng)
        {
            try
            {
                return GetGRUZS().Where(g => g.ETSNG == etsng).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetGruzsToETSNG(etsng={0})", etsng), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить ID груза по коду ЕТ СНГ
        /// </summary>
        /// <param name="etsng"></param>
        /// <returns></returns>
        public int? GetIDGruzsToETSNG(int? etsng)
        {
            GRUZS gr = GetGruzsToETSNG(etsng);
            return gr != null ? (int?)gr.id_gruz : null;
        }
        #endregion

        #region STATIONS

        public IQueryable<STATIONS> STATIONS
        {
            get { return context.STATIONS; }
        }

        public IQueryable<STATIONS> GetSTATIONS()
        {
            try
            {
                return STATIONS;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetSTATIONS()"), eventID);
                return null;
            }
        }

        public STATIONS GetSTATIONS(int id_stat)
        {
            try
            {
                return GetSTATIONS().Where(g => g.id_stat == id_stat).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetSTATIONS(id_stat={0})", id_stat), eventID);
                return null;
            }
        }

        public int SaveSTATIONS(STATIONS STATIONS)
        {
            STATIONS dbEntry;
            try
            {
                if (STATIONS.id_stat == 0)
                {
                    dbEntry = new STATIONS()
                    {
                        id_stat = STATIONS.id_stat,
                        name = STATIONS.name,
                        id_ora = STATIONS.id_ora,
                        outer_side = STATIONS.outer_side,
                        is_uz = STATIONS.is_uz,
                    };
                    context.STATIONS.Add(dbEntry);
                }
                else
                {
                    dbEntry = context.STATIONS.Find(STATIONS.id_stat);
                    if (dbEntry != null)
                    {
                        dbEntry.name = STATIONS.name;
                        dbEntry.id_ora = STATIONS.id_ora;
                        dbEntry.outer_side = STATIONS.outer_side;
                        dbEntry.is_uz = STATIONS.is_uz;
                    }
                }
                context.SaveChanges();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveSTATIONS(STATIONS={0})", STATIONS.GetFieldsAndValue()), eventID);
                return -1;
            }
            return dbEntry.id_stat;
        }

        public STATIONS DeleteSTATIONS(int id_stat)
        {
            STATIONS dbEntry = context.STATIONS.Find(id_stat);
            if (dbEntry != null)
            {
                try
                {
                    context.STATIONS.Remove(dbEntry);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    e.WriteErrorMethod(String.Format("DeleteSTATIONS(id_stat={0})", id_stat), eventID);
                    return null;
                }
            }
            return dbEntry;
        }
        /// <summary>
        /// Вернуть станцию по id системы КИС
        /// </summary>
        /// <param name="id_station_kis"></param>
        /// <returns></returns>
        public STATIONS GetStationsOfKis(int id_station_kis)
        {
            try
            {
                return GetSTATIONS().Where(s => s.id_ora == id_station_kis).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetStationsOfKis(id_station_kis={0})", id_station_kis), eventID);
                return null;
            }
        }
        /// <summary>
        /// Вернуть ID станции системы Railcars
        /// </summary>
        /// <param name="id_station_kis"></param>
        /// <returns></returns>
        public int? GetIDStationsOfKis(int id_station_kis)
        {
            try
            {
                STATIONS st = GetStationsOfKis(id_station_kis);
                if (st != null) return st.id_stat;
                return null;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetStationsOfKis(id_station_kis={0})", id_station_kis), eventID);
                return null;
            }
        }
        /// <summary>
        /// Вернуть список станций УЗ
        /// </summary>
        /// <returns></returns>
        public IQueryable<STATIONS> GetUZStations() 
        {
            try
            {
                return GetSTATIONS().Where(s => s.is_uz == 1);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetUZStations()"), eventID);
                return null;
            }
        }
        /// <summary>
        /// Вернуть список станций АМКР
        /// </summary>
        /// <returns></returns>
        public IQueryable<STATIONS> GetAMKRStations() 
        {
            try
            {
                return GetSTATIONS().Where(s => s.is_uz == 0);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetAMKRStations()"), eventID);
                return null;
            }
        }
        /// <summary>
        /// Вернуть список ID станций uz
        /// </summary>
        /// <returns></returns>
        public List<int> GetUZStationsToID() 
        {
            try
            {
                IQueryable<STATIONS> stations = GetUZStations();
                return GetListStations(stations);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetUZStationsToID()"), eventID);
                return null;
            }
        }
        /// <summary>
        /// Вернуть список ID станций AMKR
        /// </summary>
        /// <returns></returns>
        public List<int> GetAMKRStationsToID() 
        {
            try
            {
                IQueryable<STATIONS> stations = GetAMKRStations();
                return GetListStations(stations);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetAMKRStationsToID()"), eventID);
                return null;
            }
        }
        /// <summary>
        /// Вернуть список ID станций
        /// </summary>
        /// <param name="stations"></param>
        /// <returns></returns>
        public List<int> GetListStations(IQueryable<STATIONS> stations) 
        { 
            try
            {
                List<int> list = new List<int>();
                if (stations != null)
                {
                    foreach (STATIONS st in stations)
                    {
                        list.Add(st.id_stat);
                    }
                }
                return list;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetListStations(stations={0})", stations), eventID);
                return null;
            }            
        }
        /// <summary>
        /// Станция пренадлежит УЗ
        /// </summary>
        /// <param name="id_stat"></param>
        /// <returns></returns>
        public bool IsUZ(int id_stat) 
        { 
            try
            {
                STATIONS stat = GetSTATIONS(id_stat);
                return (stat != null & stat.is_uz == 1) ? true : false;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("IsUZ(id_stat={0})", id_stat), eventID);
                return false;
            }
        }
        /// <summary>
        /// Станция пренадлежит АМКР
        /// </summary>
        /// <param name="id_stat"></param>
        /// <returns></returns>
        public bool IsAMKR(int id_stat) 
        { 
            try
            {
                STATIONS stat = GetSTATIONS(id_stat);
                return (stat != null & stat.is_uz == 0) ? true : false;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("IsAMKR(id_stat={0})", id_stat), eventID);
                return false;
            }
        }
        /// <summary>
        /// Вернуть список станций пренадлежащих списку
        /// </summary>
        /// <param name="id_statuins"></param>
        /// <returns></returns>
        public IQueryable<STATIONS> GetStationOfListID(int[] id_statuins)
        {
            try
            {
                string station_uz_s = id_statuins.IntsToString(',');
                string sql = "SELECT * FROM dbo.STATIONS where [id_stat] in(" + station_uz_s + ") ";
                return context.Database.SqlQuery<STATIONS>(sql).AsQueryable();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetStationOfListID(id_stat={0})", id_statuins.IntsToString(',')), eventID);
                return null;
            }
        }
        /// <summary>
        /// Вернуть список станций не пренадлежащих списку
        /// </summary>
        /// <param name="id_statuins"></param>
        /// <returns></returns>
        public IQueryable<STATIONS> GetStationOfNotListID(int[] id_statuins)
        {
            try
            {
                
                string station_uz_s = id_statuins.Count() > 0 ? id_statuins.IntsToString(','): "0";
                string sql = "SELECT * FROM dbo.STATIONS where [id_stat] not in(" + station_uz_s + ") ";
                return context.Database.SqlQuery<STATIONS>(sql).AsQueryable();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetStationOfNotListID(id_stat={0})", id_statuins.IntsToString(',')), eventID);
                return null;
            }
        }
        #endregion

        #region WAYS

        public IQueryable<WAYS> WAYS
        {
            get { return context.WAYS; }
        }
        /// <summary>
        /// Получить список всех путей
        /// </summary>
        /// <returns></returns>
        public IQueryable<WAYS> GetWAYS()
        {
            try
            {
                return WAYS;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetWays()"), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить путь по id
        /// </summary>
        /// <param name="id_way"></param>
        /// <returns></returns>
        public WAYS GetWAYS(int id_way)
        {
            try
            {
                return GetWAYS().Where(w => w.id_way == id_way).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetWays(id_way={0})", id_way), eventID);
                return null;
            }
        }

        public int SaveWAYS(WAYS WAYS)
        {
            WAYS dbEntry;
            try
            {
                if (WAYS.id_way == 0)
                {
                    dbEntry = new WAYS()
                    {
                        id_way = 0,
                        id_stat = WAYS.id_stat,
                        id_park = WAYS.id_park,
                        num = WAYS.num,
                        name = WAYS.name,
                        vag_capacity = WAYS.vag_capacity,
                        order = WAYS.order,
                        bind_id_cond = WAYS.bind_id_cond,
                        for_rospusk = WAYS.for_rospusk, 
                    };
                    context.WAYS.Add(dbEntry);
                }
                else
                {
                    dbEntry = context.WAYS.Find(WAYS.id_way);
                    if (dbEntry != null)
                    {
                        dbEntry.id_stat = WAYS.id_stat;
                        dbEntry.id_park = WAYS.id_park;
                        dbEntry.num = WAYS.num;
                        dbEntry.name = WAYS.name;
                        dbEntry.vag_capacity = WAYS.vag_capacity;
                        dbEntry.order = WAYS.order;
                        dbEntry.bind_id_cond = WAYS.bind_id_cond;
                        dbEntry.for_rospusk = WAYS.for_rospusk;
                    }
                }
                context.SaveChanges();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveWAYS(WAYS={0})", WAYS.GetFieldsAndValue()), eventID);
                return -1;
            }
            return dbEntry.id_way;
        }

        public WAYS DeleteWAYS(int id_way)
        {
            WAYS dbEntry = context.WAYS.Find(id_way);
            if (dbEntry != null)
            {
                try
                {
                    context.WAYS.Remove(dbEntry);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    e.WriteErrorMethod(String.Format("DeleteWAYS(id_way={0})", id_way), eventID);
                    return null;
                }
            }
            return dbEntry;
        }

        /// <summary>
        /// Вернуть все пути на станции
        /// </summary>
        /// <param name="id_station"></param>
        /// <returns></returns>
        public IQueryable<WAYS> GetWaysOfStations(int id_station)
        {
            try
            {
                return GetWAYS().Where(w => w.id_stat == id_station);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetWaysOfStations(id_station={0})", id_station), eventID);
                return null;
            }
        }
        /// <summary>
        /// Вернуть путь по указанной станции и номеру пути
        /// </summary>
        /// <param name="id_station_kis"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public WAYS GetWaysOfStations(int id_station, string num)
        {
            try
            {
                return GetWaysOfStations(id_station).Where(w => w.num.ToUpper() == num.ToUpper()).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetWaysOfStations(id_station={0}, num={1})", id_station, num), eventID);
                return null;
            }
        }

        public int? GetIDWaysToStations(int id_station, string num)
        {
            try
            {
                WAYS ws = GetWaysOfStations(id_station, num);
                return ws != null ? (int?)ws.id_way : null;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetIDWaysToStations(id_station={0}, num={1})", id_station, num), eventID);
                return null;
            }
        }
        /// <summary>
        /// Вернуть ID станции
        /// </summary>
        /// <param name="id_way"></param>
        /// <returns></returns>
        public int? GetIDStationOfWay(int id_way)
        {
            try
            {
                WAYS way = GetWAYS(id_way);
                return way != null ? way.id_stat : null;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetIDStationOfWay(id_way={0})", id_way), eventID);
                return null;
            }
        }
        #endregion

        #region SHOPS
        /// <summary>
        /// Получить
        /// </summary>
        public IQueryable<SHOPS> SHOPS
        {
            get { return context.SHOPS; }
        }
        public IQueryable<SHOPS> GetSHOPS()
        {
            try
            {
                return SHOPS;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetSHOPS()"), eventID);
                return null;
            }
        }
        public SHOPS GetSHOPS(int id_shop)
        {
            try
            {
                return GetSHOPS().Where(s => s.id_shop == id_shop).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetSHOPS(id_shop={0})", id_shop), eventID);
                return null;
            }
        }
        /// <summary>
        /// Добавить или править
        /// </summary>
        /// <param name="SHOPS"></param>
        /// <returns></returns>
        public int SaveSHOPS(SHOPS SHOPS)
        {
            SHOPS dbEntry;
            try
            {
                if (SHOPS.id_shop == 0)
                {
                    dbEntry = new SHOPS()
                    {
                        id_shop = 0,
                        name = SHOPS.name,
                        name_full = SHOPS.name_full,
                        id_stat = SHOPS.id_stat,
                        id_ora = SHOPS.id_ora

                    };
                    context.SHOPS.Add(dbEntry);
                }
                else
                {
                    dbEntry = context.SHOPS.Find(SHOPS.id_shop);
                    if (dbEntry != null)
                    {
                        dbEntry.name = SHOPS.name;
                        dbEntry.name_full = SHOPS.name_full;
                        dbEntry.id_stat = SHOPS.id_stat;
                        dbEntry.id_ora = SHOPS.id_ora;
                    }
                }

                context.SaveChanges();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveSHOPS(SHOPS={0})", SHOPS.GetFieldsAndValue()), eventID);
                return -1;
            }
            return dbEntry.id_shop;
        }
        /// <summary>
        /// Удалить
        /// </summary>
        /// <param name="id_shop"></param>
        /// <returns></returns>
        public SHOPS DeleteSHOPS(int id_shop)
        {
            SHOPS dbEntry = context.SHOPS.Find(id_shop);
            if (dbEntry != null)
            {
                try
                {
                    context.SHOPS.Remove(dbEntry);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    e.WriteErrorMethod(String.Format("DeleteSHOPS(id_shop={0})", id_shop), eventID);
                    return null;
                }
            }
            return dbEntry;
        }

        public SHOPS GetShopsOfKis(int id_shop_kis)
        {
            try
            {
                return GetSHOPS().Where(s => s.id_ora == id_shop_kis).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetShopsOfKis(id_shop_kis={0})", id_shop_kis), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить ID цеха системы RailCars по id системы КИС
        /// </summary>
        /// <param name="id_shop_kis"></param>
        /// <returns></returns>
        public int? GetIDShopsOfKis(int id_shop_kis)
        {
            try
            {
                SHOPS shop = GetShopsOfKis(id_shop_kis);
                return shop != null ? (int?)shop.id_shop : null;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetIDShopsOfKis(id_shop_kis={0})", id_shop_kis), eventID);
                return null;
            }
        }
        #endregion
    }
}
