using RWWebAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testing
{
    public class Test_RWWebAPI
    {
        public Test_RWWebAPI() { }

        public void RWReference_CorrectCargo() {
            RWReference rwr = new RWReference();
            ReferenceCargo new_rc = rwr.GetReferenceCargoOfCodeETSNG(32203);
            Console.WriteLine("cargo = {0}, corect cargo = {1}",32203,new_rc.etsng);
        }
    }
}
