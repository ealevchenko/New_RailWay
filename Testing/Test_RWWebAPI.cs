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

        public void Wagons_GetKometaVagonSob()
        {
            Wagons w = new Wagons();
            List<KometaVagonSob> stat = w.GetKometaVagonSob(68823137);
            Console.WriteLine("Stations = {0}, Count = {1}", 68823137, stat.Count());
        }

        public void Wagons_GetKometaVagonSob(int num, DateTime dt)
        {
            Wagons w = new Wagons();
            KometaVagonSob stat = w.GetKometaVagonSob(num, dt);
            Console.WriteLine("Stations = {0}, Count = {1}", num, stat.N_VAGON);
        }

        public void Wagons_GetSobstvForNakl()
        {
            Wagons w = new Wagons();
            List<KometaSobstvForNakl> stat = w.GetSobstvForNakl();
            Console.WriteLine("Count = {0}", stat.Count());
        }

        public void Wagons_GetSobstvForNakl(int nak)
        {
            Wagons w = new Wagons();
            KometaSobstvForNakl stat = w.GetSobstvForNakl(nak);
            Console.WriteLine("NPLAT = {0}", stat.NPLAT);
        }

        public void Wagons_GetGruzSP(int kod)
        {
            Wagons w = new Wagons();
            PromGruzSP stat = w.GetGruzSP(kod);
            Console.WriteLine("NAME_GR = {0}", stat.NAME_GR);
        }

        public void Wagons_GetGruzSPToTarGR(int? kod, bool corect)
        {
            Wagons w = new Wagons();
            PromGruzSP stat = w.GetGruzSPToTarGR(kod, corect);
            Console.WriteLine("NAME_GR = {0}", stat.NAME_GR);
        }

        public void Wagons_GetSTPR1GR()
        {
            Wagons w = new Wagons();
            List<NumVagStpr1Gr> stat = w.GetSTPR1GR();
            Console.WriteLine("Count = {0}", stat.Count());
        }

        public void Wagons_GetSTPR1GR(int kod)
        {
            Wagons w = new Wagons();
            NumVagStpr1Gr stat = w.GetSTPR1GR(kod);
            Console.WriteLine("Count = {0}", stat.GR);
        }
    }
}
