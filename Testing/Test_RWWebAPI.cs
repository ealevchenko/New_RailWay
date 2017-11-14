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

        public void Reference_CorrectCargo() {
            Reference r = new Reference();
            Cargo new_rc = r.GetCargoOfCodeETSNG(32203);
            Console.WriteLine("cargo = {0}, corect cargo = {1}", 32203, new_rc.code_etsng);
        }

        public void Reference_StationsOfCode()
        {
            Reference r = new Reference();
            Stations stat = r.GetStationsOfCode(46700);
            Console.WriteLine("Stations = {0}, Stations = {1}", 46700, stat.code_cs);
        }
    }
}
