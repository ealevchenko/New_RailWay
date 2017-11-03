using EFRW.Abstract;
using EFRW.Entities;
using MessageLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libClass;

namespace EFRW.Concrete
{
    public class EFReference : IReference
    {
        private eventID eventID = eventID.EFRW_EFReference;

        protected EFDbContext context = new EFDbContext();
        
        #region ReferenceCargo
        public IQueryable<ReferenceCargo> ReferenceCargo
        {
            get { return context.ReferenceCargo; }
        }

        public int SaveReferenceCargo(ReferenceCargo ReferenceCargo)
        {
            ReferenceCargo dbEntry;
            try
            {
                if (ReferenceCargo.id == 0)
                {
                    dbEntry = new ReferenceCargo()
                    {
                        id = 0,
                        name_ru = ReferenceCargo.name_ru,
                        name_en = ReferenceCargo.name_en,
                        name_full_ru = ReferenceCargo.name_full_ru,
                        name_full_en = ReferenceCargo.name_full_en,
                        etsng = ReferenceCargo.etsng,
                        id_type = ReferenceCargo.id_type,
                        datetime = ReferenceCargo.datetime

                    };
                    context.ReferenceCargo.Add(dbEntry);
                }
                else
                {
                    dbEntry = context.ReferenceCargo.Find(ReferenceCargo.id);
                    if (dbEntry != null)
                    {
                        dbEntry.name_ru = ReferenceCargo.name_ru;
                        dbEntry.name_en = ReferenceCargo.name_en;
                        dbEntry.name_full_ru = ReferenceCargo.name_full_ru;
                        dbEntry.name_full_en = ReferenceCargo.name_full_en;
                        dbEntry.etsng = ReferenceCargo.etsng;
                        dbEntry.id_type = ReferenceCargo.id_type;
                        dbEntry.datetime = ReferenceCargo.datetime;
                    }
                }
                context.SaveChanges();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveReferenceCargo(ReferenceCargo={0})", ReferenceCargo.GetFieldsAndValue()), eventID);
                return -1;
            }
            return dbEntry.id;
        }

        public ReferenceCargo DeleteReferenceCargo(int id)
        {
            ReferenceCargo dbEntry = context.ReferenceCargo.Find(id);
            if (dbEntry != null)
            {
                try
                {
                    context.ReferenceCargo.Remove(dbEntry);

                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    e.WriteErrorMethod(String.Format("DeleteReferenceCargo(id={0})", id), eventID);
                    return null;
                }
            }
            return dbEntry;
        }
        /// <summary>
        /// Получить все строки справочника грузов
        /// </summary>
        /// <returns></returns>
        public IQueryable<ReferenceCargo> GetReferenceCargo()
        {
            try
            {
                return ReferenceCargo;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetReferenceCargo()"), eventID);
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_cargo"></param>
        /// <returns></returns>
        public ReferenceCargo GetReferenceCargo(int id)
        {
            return GetReferenceCargo().Where(c => c.id == id).FirstOrDefault();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code_etsng"></param>
        /// <returns></returns>
        public ReferenceCargo GetReferenceCargoOfCodeETSNG(int code_etsng)
        {
            return GetReferenceCargo().Where(c => c.etsng == code_etsng).FirstOrDefault();
        }
        /// <summary>
        /// Вернуть список грузов по указаному деапазону кодов ETSNG
        /// </summary>
        /// <param name="code_start"></param>
        /// <param name="code_stop"></param>
        /// <returns></returns>
        public IQueryable<ReferenceCargo> GetReferenceCargoOfCodeETSNG(int code_etsng_start, int code_etsng_stop)
        {
            return GetReferenceCargo().Where(c => c.etsng >= code_etsng_start & c.etsng <= code_etsng_stop);
        }
        /// <summary>
        /// Вернуть откорректированный код ETSNG
        /// </summary>
        /// <param name="code_etsng"></param>
        /// <returns></returns>
        public int GetCorectReferenceCargo(int code_etsng)
        {
            ReferenceCargo ref_cargo = GetReferenceCargoOfCodeETSNG(code_etsng);
            if (ref_cargo == null)
            {
                ref_cargo = GetReferenceCargoOfCodeETSNG(code_etsng * 10, (code_etsng * 10) + 9).FirstOrDefault();
            }
            return ref_cargo != null ? ref_cargo.etsng : code_etsng;
        }
        #endregion

        #region ReferenceTypeCargo
        public IQueryable<ReferenceTypeCargo> ReferenceTypeCargo
        {
            get { return context.ReferenceTypeCargo; }
        }

        public int SaveReferenceTypeCargo(ReferenceTypeCargo ReferenceTypeCargo)
        {
            ReferenceTypeCargo dbEntry;
            try
            {
                if (ReferenceTypeCargo.id == 0)
                {
                    dbEntry = new ReferenceTypeCargo()
                    {
                        id = 0,
                        type_name_ru = ReferenceTypeCargo.type_name_ru,
                        type_name_en = ReferenceTypeCargo.type_name_en
                    };
                    context.ReferenceTypeCargo.Add(dbEntry);
                }
                else
                {
                    dbEntry = context.ReferenceTypeCargo.Find(ReferenceTypeCargo.id);
                    if (dbEntry != null)
                    {
                        dbEntry.type_name_ru = ReferenceTypeCargo.type_name_ru;
                        dbEntry.type_name_en = ReferenceTypeCargo.type_name_en;
                    }
                }
                context.SaveChanges();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveReferenceTypeCargo(ReferenceTypeCargo={0})", ReferenceTypeCargo.GetFieldsAndValue()), eventID);
                return -1;
            }
            return dbEntry.id;
        }

        public ReferenceTypeCargo DeleteReferenceTypeCargo(int id)
        {
            ReferenceTypeCargo dbEntry = context.ReferenceTypeCargo.Find(id);
            if (dbEntry != null)
            {
                try
                {
                    context.ReferenceTypeCargo.Remove(dbEntry);

                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    e.WriteErrorMethod(String.Format("DeleteReferenceTypeCargo(id={0})", id), eventID);
                    return null;
                }
            }
            return dbEntry;
        }
        #endregion
    }
}
