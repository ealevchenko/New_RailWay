using MessageLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libClass;
using EFReference.Abstract;
using EFReference.Entities;

namespace EFReference.Concrete
{
    public class EFReference : IReference
    {
        private eventID eventID = eventID.EFReference;

        protected EFDbContext context = new EFDbContext();
        
        #region Cargo
        public IQueryable<Cargo> Cargo
        {
            get { return context.Cargo; }
        }
        /// <summary>
        /// Получить все строки справочника грузов
        /// </summary>
        /// <returns></returns>
        public IQueryable<Cargo> GetCargo()
        {
            try
            {
                return Cargo;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetCargo()"), eventID);
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_cargo"></param>
        /// <returns></returns>
        public Cargo GetCargo(int id)
        {
            return GetCargo().Where(c => c.id == id).FirstOrDefault();
        }

        public int SaveCargo(Cargo Cargo)
        {
            Cargo dbEntry;
            try
            {
                if (Cargo.id == 0)
                {
                    dbEntry = new Cargo()
                    {
                        id = 0,
                        code_etsng = Cargo.code_etsng,
                        name_etsng = Cargo.name_etsng,
                        code_gng = Cargo.code_gng,
                        name_gng = Cargo.name_gng,
                        id_sap = Cargo.id_sap
                    };
                    context.Cargo.Add(dbEntry);
                }
                else
                {
                    dbEntry = context.Cargo.Find(Cargo.id);
                    if (dbEntry != null)
                    {
                        dbEntry.code_etsng = Cargo.code_etsng;
                        dbEntry.name_etsng = Cargo.name_etsng;
                        dbEntry.code_gng = Cargo.code_gng;
                        dbEntry.name_gng = Cargo.name_gng;
                        dbEntry.id_sap = Cargo.id_sap;
                    }
                }
                context.SaveChanges();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveCargo(Cargo={0})", Cargo.GetFieldsAndValue()), eventID);
                return -1;
            }
            return dbEntry.id;
        }

        public Cargo DeleteCargo(int id)
        {
            Cargo dbEntry = context.Cargo.Find(id);
            if (dbEntry != null)
            {
                try
                {
                    context.Cargo.Remove(dbEntry);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    e.WriteErrorMethod(String.Format("DeleteCargo(id={0})", id), eventID);
                    return null;
                }
            }
            return dbEntry;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code_etsng"></param>
        /// <returns></returns>
        public Cargo GetCargoOfCodeETSNG(int code_etsng)
        {
            return GetCargo().Where(c => c.code_etsng == code_etsng).FirstOrDefault();
        }
        /// <summary>
        /// Вернуть список грузов по указаному деапазону кодов ETSNG
        /// </summary>
        /// <param name="code_start"></param>
        /// <param name="code_stop"></param>
        /// <returns></returns>
        public IQueryable<Cargo> GetCargoOfCodeETSNG(int code_etsng_start, int code_etsng_stop)
        {
            return GetCargo().Where(c => c.code_etsng >= code_etsng_start & c.code_etsng <= code_etsng_stop);
        }
        /// <summary>
        /// Вернуть уточненую строку ReferenceCargo
        /// </summary>
        /// <param name="code_etsng"></param>
        /// <returns></returns>
        public Cargo GetCorrectCargo(int code_etsng)
        {
            Cargo ref_cargo = GetCargoOfCodeETSNG(code_etsng);
            if (ref_cargo == null)
            {
                ref_cargo = GetCargoOfCodeETSNG(code_etsng * 10, (code_etsng * 10) + 9).FirstOrDefault();
            }
            return ref_cargo;
        }
        /// <summary>
        /// Вернуть откорректированный код ETSNG
        /// </summary>
        /// <param name="code_etsng"></param>
        /// <returns></returns>
        public int GetCodeCorrectCargo(int code_etsng)
        {
            Cargo ref_cargo = GetCorrectCargo(code_etsng);
            return ref_cargo != null ? ref_cargo.code_etsng : code_etsng;
        }
        #endregion
    }
}
