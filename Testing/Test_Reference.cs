using EFRailWay.Concrete.Reference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFReference.Concrete;
using EFRailWay.Entities.Reference;
using EFReference.Entities;

namespace Testing
{
    public class Test_Reference
    {
        public Test_Reference() { 
        
        }

        public void Cargo_Copy()
        {
            try
            {
                EFReference.Concrete.EFReference ef_ref = new EFReference.Concrete.EFReference();
                EFCodeCargoRepository old = new EFCodeCargoRepository();
                foreach (Code_Cargo old_cargo in old.Code_Cargo)
                {
                    Console.WriteLine(String.Format("Переносим груз {0}", old_cargo.ETSNG));
                    Cargo new_cargo = new Cargo() { code_etsng = old_cargo.IDETSNG, name_etsng = old_cargo.ETSNG, code_gng = old_cargo.IDGNG, name_gng = old_cargo.GNG, id_sap = old_cargo.IDSAP };
                    Console.WriteLine(String.Format("Результат {0}", ef_ref.SaveCargo(new_cargo))); 

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }

        public void GetCorrectCargo() {
            EFReference.Concrete.EFReference ef_ref = new EFReference.Concrete.EFReference();
            int icargo = 1500;
            Console.WriteLine(String.Format("код => {0} => {1}", icargo, ef_ref.GetCorrectCargo(icargo).code_etsng)); 
        }
    }
}
