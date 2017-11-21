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

            DateTime dt = new DateTime();

            #region Test_RWWebAPI
            Test_RWWebAPI tst_rw_api = new Test_RWWebAPI();
            //tst_rw_api.RWReference_CorrectCargo();
            //tst_rw_api.Reference_CorrectCargo(); //
            //tst_rw_api.Reference_StationsOfCode();// Получить код Кривой рог главный
            //tst_rw_api.Wagons_GetKometaVagonSob(); // Получить собственника вагона
            //tst_rw_api.Wagons_GetKometaVagonSob(68823137, DateTime.Now); // Получить собственника вагона до указаной даты
            //tst_rw_api.Wagons_GetSobstvForNakl(); // Показать все накладные
            //tst_rw_api.Wagons_GetSobstvForNakl(10); // Показать все накладные
            //tst_rw_api.Wagons_GetGruzSP(307);
            //tst_rw_api.Wagons_GetGruzSPToTarGR(12502, false); 
            //tst_rw_api.Wagons_GetSTPR1GR();
            //Wagons_GetSTPR1GR(291);
            //tst_rw_api.Reference_GetCountryOfCodeSNG();

            #endregion

            #region Test_MetallurgTrans
            Test_MetallurgTrans tst_tr_app = new Test_MetallurgTrans();
            //tst_tr_app.TransferApproaches();
            //tst_tr_app.TransferArrival();
            //tst_tr_app.EFMetallurgTrans_Consignee(); //
            //tst_tr_app.TRailCars_ArrivalToRailCars(); //Поставить состав в прибытие системы Railcars
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

            #region Test_Wagons
            Test_Wagons tst_wag = new Test_Wagons();
            //tst_wag.Test_KometaContent_KometaVagonSob(); // все данные KometaVagonSob
            //tst_wag.Test_KometaContent_KometaVagonSob(68823137); // все данные KometaVagonSob
            //tst_wag.Test_KometaContent_KometaVagonSob(68823137, DateTime.Now); // все данные KometaVagonSob
            //tst_rw_ref.ReferenceCargo_Get();
            //tst_wag.Test_KometaContent_KometaSobstvForNakl(); // Накладные по собственникам 
            //tst_wag.Test_KometaContent_KometaSobstvForNakl(10); // Накладные по собственникам   
            //tst_wag.Test_PromContent_GetGruzSP();               // Грузы по промышленой     
            //tst_wag.Test_PromContent_GetGruzSP(307);               // Грузы по промышленой  
            //tst_wag.Test_PromContent_GetGruzSPToTarGR(12502, true); // Грузы по промышленой поиск по етснг с коррекцией 
            //tst_wag.Test_PromContent_GetGruzSPToTarGR(12502, false);// Грузы по промышленой поиск по етснг без коррекции 
            //tst_wag.Test_VagonsContent_GetSTPR1GR();
            //tst_wag.Test_VagonsContent_GetSTPR1GR(291);
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
            //test.MLog_WriteEvents();
            //test.MLog_WriteLogServices();
            test.MLog_WriteLogStatusServices();
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
