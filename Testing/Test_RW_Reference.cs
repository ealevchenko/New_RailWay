//using EFRailWay.Entities;
//using EFRailWay.References;
using EFRW.Concrete;
using EFRW.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libClass;

namespace Testing
{
    class Test_RW_Reference
    {
        public Test_RW_Reference() { }

        public void ReferenceCargo_Copy() {
            try
            {
                //EFRW.Concrete.EFReference ef_ref = new EFRW.Concrete.EFReference();
                //GeneralReferences old = new GeneralReferences();

                //List<TypeCargo> list_old_type = old.GetTypeCargo().ToList();
                //foreach (TypeCargo tc in list_old_type)
                //{
                //    ReferenceTypeCargo rtc = new ReferenceTypeCargo()
                //    {
                //        id = 0,
                //        type_name_ru = tc.TypeCargoRU,
                //        type_name_en = tc.TypeCargoEN
                //    };
                //    int res_type = ef_ref.SaveReferenceTypeCargo(rtc);
                //    Console.WriteLine(String.Format("Тип id={0}",res_type));
                //    if (res_type > 0)
                //    {
                //        List<EFRailWay.Entities.ReferenceCargo> list_old_cargo = old.GetReferenceCargo().Where(r => r.TypeCargo == tc.ID).ToList();
                //        foreach (EFRailWay.Entities.ReferenceCargo c_old in list_old_cargo)
                //        {
                //            EFRW.Entities.ReferenceCargo c_new = new EFRW.Entities.ReferenceCargo()
                //            {
                //                id = 0,
                //                id_type = res_type,
                //                datetime = c_old.DateTime,
                //                etsng = c_old.ETSNG,
                //                name_ru = c_old.Name,
                //                name_full_ru = c_old.NameFull,
                //                name_en = c_old.Name,
                //                name_full_en = c_old.NameFull
                //            };
                //            int res_cargo = ef_ref.SaveReferenceCargo(c_new);
                //            Console.WriteLine(String.Format("  Груз id={0}", res_cargo));
                //        }
                //    }
                //}
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }

        public void ReferenceCargo_Get() {
            try
            {
                EFRW.Concrete.EFReference ef_ref = new EFRW.Concrete.EFReference();
                foreach (EFRW.Entities.ReferenceCargo cargo in ef_ref.GetReferenceCargo().ToList()) {
                    Console.WriteLine(cargo.GetFieldsAndValue());                
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }
    }
}
