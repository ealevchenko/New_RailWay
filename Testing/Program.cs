using libClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Testing
{




    class Program
    {

        public class TestClass
        {
            public long ID { get; set; }
            public DateTime DateTime { get; set; }
            public string UserName { get; set; }
            public string UserHostName { get; set; }
            public string UserHostAddress { get; set; }
            public string PhysicalPath { get; set; }
            public int? Service { get; set; }
            public int? EventID { get; set; }
            public int? Level { get; set; }
            public string Log { get; set; }
        }

        static void Main(string[] args)
        {

            #region Test_RWWebAPI
            Test_RWWebAPI tst_rw_api = new Test_RWWebAPI();
            //tst_rw_api.RWReference_CorrectCargo();
            //tst_rw_api.Reference_CorrectCargo(); //
            #endregion

            #region Test_MetallurgTrans
            Test_MetallurgTrans tst_tr_app = new Test_MetallurgTrans();
            //tst_tr_app.TransferApproaches();
            tst_tr_app.TransferArrival();
            //tst_tr_app.EFMetallurgTrans_Consignee(); //
            #endregion

            #region Test_Reference
            Test_Reference tst_ref = new Test_Reference();
            //tst_ref.Cargo_Copy(); // Перенос справочника ЕТСНГ
            //tst_ref.GetCorrectCargo();// Проверка корректного кода етснг

            #endregion

            #region Test_RW_Reference
            Test_RW_Reference tst_rw_ref = new Test_RW_Reference();
            //tst_rw_ref.ReferenceCargo_Copy();
            //tst_rw_ref.ReferenceCargo_Get();
            
            #endregion


            #region Test_Logs тест логирования
            Test_Logs test = new Test_Logs();
            //RWDBLogs
            //test.RWLogs_InformationToDB(); // сохранить информацию в лог БД
            //test.RWLogs_WarningToDB(); // сохранить информацию в warning БД
            //test.RWLogs_ErrorToDB(); // сохранить информацию в error БД
            //test.RWLogs_CriticalToDB(); // сохранить информацию в critical БД
            //test.RWLogs_ExceptionToDB(); // сохранить информацию в exception БД
            //RWEventLogs
            //test.RWLogs_InformationToEventLogs(); // сохранить информацию в лог windows
            //test.RWLogs_WarningToEventLogs(); // сохранить warning в лог windows
            //test.RWLogs_ErrorToEventLogs(); // сохранить error в лог windows
            //test.RWLogs_ExceptionToEventLogs(); // сохранить exception в лог windows
            //RWLogs_FileLogs()
            //test.RWLogs_FileLogs(); // сохранить в файл на диске
            //RWLogs_FileLogs()
            //test.RWLog_WriteInformation();
            //test.RWLog_WriteWarning();
            //test.RWLog_WriteError();
            //test.RWLog_WriteException();
            #endregion

            #region Test_Settings тест настроек
            Test_Settings testSettings = new Test_Settings();
            //RWSettings
            //testSettings.RWSettings_GetSetting();
            //testSettings.RWSettings_SetSetting();

            #endregion

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
