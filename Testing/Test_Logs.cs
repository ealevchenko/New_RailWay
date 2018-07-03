using MessageLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testing
{
    public class Test_Logs
    {
        public Test_Logs() { }

        #region RWDBLogs

        public void RWLogs_InformationToDB(){
            //string log = "Тест сохрнения информации";
            //Console.WriteLine("log.SaveInformationToDB()  ->  {0}", log.SaveInformationToDB());
            //Console.WriteLine("log.SaveInformationToDB(service.TransferMT)  ->  {0}", log.SaveInformationToDB(service.TransferMT));
            //Console.WriteLine("log.SaveInformationToDB(eventID.EFMetallurgTrans)  ->  {0}", log.SaveInformationToDB(eventID.EFMetallurgTrans));
            //Console.WriteLine("log.SaveInformationToDB(service.TransferMT,eventID.EFMetallurgTrans)  ->  {0}", log.SaveInformationToDB(service.TransferMT, eventID.EFMetallurgTrans));          
        }

        public void RWLogs_WarningToDB(){
            //string log = "Тест сохрнения Warning";
            //Console.WriteLine("log.SaveWarningToDB()  ->  {0}", log.SaveWarningToDB());
            //Console.WriteLine("log.SaveWarningToDB(service.TransferMT)  ->  {0}", log.SaveWarningToDB(service.TransferMT));
            //Console.WriteLine("log.SaveWarningToDB(eventID.EFMetallurgTrans)  ->  {0}", log.SaveWarningToDB(eventID.EFMetallurgTrans));
            //Console.WriteLine("log.SaveWarningToDB(service.TransferMT,eventID.EFMetallurgTrans)  ->  {0}", log.SaveWarningToDB(service.TransferMT, eventID.EFMetallurgTrans));          
        }

        public void RWLogs_ErrorToDB()
        {
            //string log = "Тест сохрнения Error";
            //Console.WriteLine("log.SaveErrorToDB()  ->  {0}", log.SaveErrorToDB());
            //Console.WriteLine("log.SaveErrorToDB(service.TransferMT)  ->  {0}", log.SaveErrorToDB(service.TransferMT));
            //Console.WriteLine("log.SaveErrorToDB(eventID.EFMetallurgTrans)  ->  {0}", log.SaveErrorToDB(eventID.EFMetallurgTrans));
            //Console.WriteLine("log.SaveErrorToDB(service.TransferMT,eventID.EFMetallurgTrans)  ->  {0}", log.SaveErrorToDB(service.TransferMT, eventID.EFMetallurgTrans));          
        }

        public void RWLogs_CriticalToDB()
        {
            //string log = "Тест сохрнения Critical";
            //Console.WriteLine("log.SaveCriticalToDB()  ->  {0}", log.SaveCriticalToDB());
            //Console.WriteLine("log.SaveCriticalToDB(service.TransferMT)  ->  {0}", log.SaveCriticalToDB(service.TransferMT));
            //Console.WriteLine("log.SaveCriticalToDB(eventID.EFMetallurgTrans)  ->  {0}", log.SaveCriticalToDB(eventID.EFMetallurgTrans));
            //Console.WriteLine("log.SaveCriticalToDB(service.TransferMT,eventID.EFMetallurgTrans)  ->  {0}", log.SaveCriticalToDB(service.TransferMT, eventID.EFMetallurgTrans));          
        }

        public void RWLogs_ExceptionToDB()
        {
            Exception ex = new Exception("error1", new Exception("error0"));

            string log = "Тест сохрнения Exception";

            //Console.WriteLine("ex.SaveErrorToDB()  ->  {0}", ex.SaveErrorToDB());
            //Console.WriteLine("ex.SaveErrorToDB(log)  ->  {0}", ex.SaveErrorToDB(log));
            //Console.WriteLine("ex.SaveErrorToDB(service.TransferMT)  ->  {0}", ex.SaveErrorToDB(service.TransferMT));
            //Console.WriteLine("ex.SaveErrorToDB(log, service.TransferMT)  ->  {0}", ex.SaveErrorToDB(log, service.TransferMT));
            //Console.WriteLine("ex.SaveErrorToDB(eventID.RWService_KIS_Transfer)  ->  {0}", ex.SaveErrorToDB(eventID.RWService_KIS_Transfer));
            //Console.WriteLine("ex.SaveErrorToDB(log, eventID.RWService_KIS_Transfer)  ->  {0}", ex.SaveErrorToDB(log, eventID.RWService_KIS_Transfer));
            //Console.WriteLine("ex.SaveErrorToDB(service.TransferMT,eventID.EFMetallurgTrans_EFMTArrival)  ->  {0}", ex.SaveErrorToDB(service.TransferMT,eventID.EFMetallurgTrans_EFMTArrival));
            //Console.WriteLine("ex.SaveErrorToDB(log, service.TransferMT,eventID.EFMetallurgTrans_EFMTArrival)  ->  {0}", ex.SaveErrorToDB(log, service.TransferMT,eventID.EFMetallurgTrans_EFMTArrival));

        }

        #endregion

        #region EventLogs

        public void RWLogs_InformationToEventLogs()
        {
            string log = "Тест сохрнения информации";
            //log.SaveInformationToEventLogs();
            //log.SaveInformationToEventLogs(service.TransferMT);
            //log.SaveInformationToEventLogs(eventID.EFMetallurgTrans);
            //log.SaveInformationToEventLogs(service.TransferMT,eventID.EFMetallurgTrans);
        
        }

        public void RWLogs_WarningToEventLogs()
        {
            string log = "Тест сохрнения Warning";
            //log.SaveWarningToEventLogs();
            //log.SaveWarningToEventLogs(service.TransferMT);
            //log.SaveWarningToEventLogs(eventID.EFMetallurgTrans);
            //log.SaveWarningToEventLogs(service.TransferMT, eventID.EFMetallurgTrans);
        }

        public void RWLogs_ErrorToEventLogs()
        {
            string log = "Тест сохрнения Error";
            //log.SaveErrorToEventLogs();
            //log.SaveErrorToEventLogs(service.TransferMT);
            //log.SaveErrorToEventLogs(eventID.EFMetallurgTrans);
            //log.SaveErrorToEventLogs(service.TransferMT, eventID.EFMetallurgTrans);
        }

        public void RWLogs_ExceptionToEventLogs()
        {
            Exception ex1 = new Exception("error1_1", new Exception("error1_0"));
            Exception ex2 = new Exception("error2_1", new Exception("error2_0"));
            Exception ex3 = new Exception("error3_1", new Exception("error3_0"));
            Exception ex4 = new Exception("error4_1", new Exception("error4_0"));

            string log = "Тест сохрнения Exception";
            //ex1.SaveErrorToEventLogs();
            //ex1.SaveErrorToEventLogs(log);
            //ex2.SaveErrorToEventLogs(service.TransferMT);
            //ex2.SaveErrorToEventLogs(log, service.TransferMT);
            //ex3.SaveErrorToEventLogs(eventID.RWService_KIS_Transfer);
            //ex3.SaveErrorToEventLogs(log, eventID.RWService_KIS_Transfer);
            //ex4.SaveErrorToEventLogs(service.TransferMT, eventID.RWService_KIS_Transfer);
            //ex4.SaveErrorToEventLogs(log, service.TransferMT, eventID.RWService_KIS_Transfer);

        }

        #endregion

        #region FileLogs

        public void RWLogs_FileLogs() {
            Exception ex = new Exception("error1", new Exception("error0"));

            FileLogs.InitLogger();
            FileLogs.Log.Error("Тест сохрнения FileLogs(error)");
            FileLogs.Log.Error("Тест сохрнения FileLogs + Exception", ex);
            FileLogs.Log.Info("Тест сохрнения FileLogs(Info)");
            FileLogs.Log.Debug("Тест сохрнения FileLogs(Debug)");
            FileLogs.Log.Fatal("Тест сохрнения FileLogs(Fatal)");
            FileLogs.Log.Warn("Тест сохрнения FileLogs(Warn)");

        }

        #endregion

        #region RWLog

        public void RWLog_WriteInformation() {

            string log = "Тест сохрнения Information библиотеки RWLog";
            MLog.InitRWLogs(true, true, true, true, true, true);
            log.WriteInformation();
            log.WriteInformation(service.ServicesMT);
            log.WriteInformation(eventID.RailWay);
            log.WriteInformation(service.ServicesMT, eventID.RailWay);
       }

        public void RWLog_WriteWarning()
        {
            string log = "Тест сохрнения Warning библиотеки RWLog";
            MLog.InitRWLogs(true, true, true, true, true, true);
            log.WriteWarning();
            log.WriteWarning(service.ServicesMT);
            log.WriteWarning(eventID.RailWay);
            log.WriteWarning(service.ServicesMT, eventID.RailWay);

        }

        public void RWLog_WriteError()
        {
            string log = "Тест сохрнения Error библиотеки RWLog";
            MLog.InitRWLogs(true, true, true, true, true, true);
            log.WriteError();
            log.WriteError(service.ServicesMT);
            log.WriteError(eventID.RailWay);
            log.WriteError(service.ServicesMT, eventID.RailWay);

        }

        public void RWLog_WriteException()
        {
            Exception ex = new Exception("error1", new Exception("error0"));

            string log = "Тест сохрнения Exception библиотеки RWLog";
            MLog.InitRWLogs(true, true, true, true, true, true);
            ex.WriteError();
            ex.WriteError(log);
            ex.WriteError(service.ServicesMT);
            ex.WriteError(log, service.ServicesMT);
            ex.WriteError(eventID.RailWay);
            ex.WriteError(log, eventID.RailWay);
            ex.WriteError(service.ServicesMT, eventID.RailWay);
            ex.WriteError(log, service.ServicesMT, eventID.RailWay);
        }
        #endregion

        #region Events

        public void MLog_WriteEvents() { 
            string events = "eventsTest";
            string status = "statusTest";
            service service = MessageLog.service.ServicesMT;
            eventID eventID = MessageLog.eventID.EFRCReference;
            events.WriteEvents(status,service,eventID);
            events.WriteEvents(status,service);
            events.WriteEvents(status,eventID);
            events.WriteEvents(status);
            EventStatus status1 = EventStatus.No_Actions;
            service service1 = MessageLog.service.ServicesMT;
            eventID eventID1 = MessageLog.eventID.EFSAP;
            events.WriteEvents(status1,service1,eventID1);
            events.WriteEvents(status1,service1);
            events.WriteEvents(status1,eventID1);
            events.WriteEvents(status1);
        }

        public void MLog_WriteLogServices()
        {
            service service = MessageLog.service.ServicesMT;
            service.WriteServices(DateTime.Now.AddDays(-1), DateTime.Now, 5);
        }

        public void MLog_WriteStatusServices()
        {
            service service = MessageLog.service.ServicesMT;
            service.WriteStatusServices(DateTime.Now.AddDays(-1), DateTime.Now);
            MessageLog.service.TransferApproaches.WriteStatusServices(DateTime.Now.AddDays(-1));
        }

        public void MLog_WriteLogStatusServices()
        {
            service service = MessageLog.service.ServicesMT;
            service.WriteLogStatusServices();
            service.WriteLogStatusServices(DateTime.Now.AddHours(-1), DateTime.Now);
        }

        #endregion

    }
}
