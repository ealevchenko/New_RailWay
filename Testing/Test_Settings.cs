using MessageLog;
using RWSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testing
{
    public class Test_Settings
    {
        public Test_Settings() { }

        #region RWSettings

        public void RWSettings_GetSetting()
        {
            RWSetting.InitLog(true); // Включим логирование            
            // Проверка string
            string aval = "Def";
            Console.WriteLine("GetCongigSetting('sVal', aval)  ->  {0}", RWSetting.GetCongigSetting("sVal", aval));
            Console.WriteLine("GetCongigSetting('errorVal', aval)  ->  {0}", RWSetting.GetCongigSetting("errorVal", aval));
            Console.WriteLine("GetDBSetting<string>('sVal', service.ServicesMT)  ->  {0}", RWSetting.GetDBSetting<string>("sVal", service.ServicesMT));
            Console.WriteLine("GetDBSetting<string>('errorVal', service.ServicesMT)  ->  {0}", RWSetting.GetDBSetting<string>("errorVal", service.ServicesMT));
            Console.WriteLine("GetDBSetting<string>('errorVal', service.ServicesMT, 'DefTest')  ->  {0}", RWSetting.GetDBSetting<string>("errorVal", service.ServicesMT, "DefTest"));
            Console.WriteLine("GetDB_Config_DefaultSetting()  ->  {0}", RWSetting.GetDB_Config_DefaultSetting("sVal", service.Null, aval, true));
             //Проверка bool
            bool bval = true;
            Console.WriteLine("GetCongigSetting('bVal', bval)  ->  {0}", RWSetting.GetCongigSetting("bVal", bval));
            Console.WriteLine("GetCongigSetting('errorVal', bval)  ->  {0}", RWSetting.GetCongigSetting("errorVal", bval));
            Console.WriteLine("GetDBSetting<bool>('bVal', service.ServicesMT)  ->  {0}", RWSetting.GetDBSetting<bool>("bVal", service.Null));
            Console.WriteLine("GetDBSetting<bool>('errorVal', service.ServicesMT)  ->  {0}", RWSetting.GetDBSetting<bool>("errorVal", service.ServicesMT));
            Console.WriteLine("GetDBSetting<bool>('errorVal', service.ServicesMT, true)  ->  {0}", RWSetting.GetDBSetting<bool>("errorVal", service.ServicesMT, true));
            Console.WriteLine("GetDB_Config_DefaultSetting()  ->  {0}", RWSetting.GetDB_Config_DefaultSetting("bVal", service.Null, bval, true));
             //Проверка int
            int ival = 100;
            Console.WriteLine("GetCongigSetting('iVal', bval)  ->  {0}", RWSetting.GetCongigSetting("iVal", ival));
            Console.WriteLine("GetCongigSetting('errorVal', ival)  ->  {0}", RWSetting.GetCongigSetting("errorVal", ival));
            Console.WriteLine("GetDBSetting<int>('iVal', service.ServicesMT)  ->  {0}", RWSetting.GetDBSetting<int>("iVal", service.ServicesMT));
            Console.WriteLine("GetDBSetting<int>('errorVal', service.ServicesMT)  ->  {0}", RWSetting.GetDBSetting<int>("errorVal", service.ServicesMT));
            Console.WriteLine("GetDBSetting<int>('errorVal', service.ServicesMT, true)  ->  {0}", RWSetting.GetDBSetting<int>("errorVal", service.ServicesMT, 200));
            Console.WriteLine("GetDB_Config_DefaultSetting()  ->  {0}", RWSetting.GetDB_Config_DefaultSetting("iVal", service.ServicesMT, ival, true));
             //Проверка double
            double dval = 22.33;
            Console.WriteLine("GetCongigSetting('dval', dval)  ->  {0}", RWSetting.GetCongigSetting("dval", dval));
            Console.WriteLine("GetCongigSetting('errorVal', dval)  ->  {0}", RWSetting.GetCongigSetting("errorVal", dval));
            Console.WriteLine("GetDBSetting<double>('dval', service.ServicesMT)  ->  {0}", RWSetting.GetDBSetting<double>("dval", service.ServicesMT));
            Console.WriteLine("GetDBSetting<double>('errorVal', service.ServicesMT)  ->  {0}", RWSetting.GetDBSetting<double>("errorVal", service.ServicesMT));
            Console.WriteLine("GetDBSetting<double>('errorVal', service.ServicesMT, true)  ->  {0}", RWSetting.GetDBSetting<double>("errorVal", service.ServicesMT, 33.55));
            Console.WriteLine("GetDB_Config_DefaultSetting()  ->  {0}", RWSetting.GetDB_Config_DefaultSetting("dval", service.ServicesMT, dval, true));

        }

        public void RWSettings_SetSetting()
        {
            RWSetting.InitLog(true); // Включим логирование            
            // Проверка string
            string sval = "Test SetSetting";
            bool bval = true;
            int ival = 10;
            double dval = 10.44;


            Console.WriteLine("SetCongigSetting string  ->  {0}", sval.SetCongigSetting("String_Test"));
            Console.WriteLine("SetCongigSetting bool  ->  {0}", bval.SetCongigSetting("bool_test"));
            Console.WriteLine("SetCongigSetting int  ->  {0}", ival.SetCongigSetting("int_test"));
            Console.WriteLine("SetCongigSetting double  ->  {0}", dval.SetCongigSetting("double_test"));

            Console.WriteLine("SetDBSetting string  ->  {0}", sval.SetDBSetting("String_Test", service.HostMT));
            Console.WriteLine("SetDBSetting bool  ->  {0}", bval.SetDBSetting("bool_test", service.HostMT));
            Console.WriteLine("SetDBSetting int  ->  {0}", ival.SetDBSetting("int_test", service.HostMT));
            Console.WriteLine("SetDBSetting double  ->  {0}", dval.SetDBSetting("double_test", service.HostMT));

            Console.WriteLine("SetDBSetting string  ->  {0}", sval.SetDBSetting("String_Test_desc", service.HostMT, "descr"));
            Console.WriteLine("SetDBSetting bool  ->  {0}", bval.SetDBSetting("bool_test_desc", service.HostMT, "descr"));
            Console.WriteLine("SetDBSetting int  ->  {0}", ival.SetDBSetting("int_test_desc", service.HostMT, "descr"));
            Console.WriteLine("SetDBSetting double  ->  {0}", dval.SetDBSetting("double_test_desc", service.HostMT, "descr"));
        }

        #endregion
    }
}
