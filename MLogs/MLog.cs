using MessageLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageLog
{

    public static class MLog
    {
  
        // Вкл\Отк логирования в журналы Windows
        static bool _eLogs = false;
        static bool _eLogErrors = false;

        // Вкл\Отк логирования в базу данных
        static bool _dbLogs = false;
        static bool _dbLogErrors = false;

        // Вкл\Отк логирования в файл на диске
        static bool _fLogs = false;
        static bool _fLogErrors = false;





        static MLog()
        {
            FileLogs.InitLogger();
            try
            {
                _eLogs = bool.Parse(ConfigurationManager.AppSettings["Logs"].ToString());
                _eLogErrors = bool.Parse(ConfigurationManager.AppSettings["LogErrors"].ToString());
                _dbLogs = bool.Parse(ConfigurationManager.AppSettings["dbLogs"].ToString());
                _dbLogErrors = bool.Parse(ConfigurationManager.AppSettings["dbLogErrors"].ToString());
                _fLogs = bool.Parse(ConfigurationManager.AppSettings["fLogs"].ToString());
                _fLogErrors = bool.Parse(ConfigurationManager.AppSettings["fLogErrors"].ToString());
            }
            catch (Exception e)
            {
                LogError(e, String.Format("Ошибка чтения AppSettings:(Logs,LogErrors,dbLogs,dbLogErrors,fLogs,fLogErrors)"));
            }
        }

        public static void InitRWLogs(bool eLogs, bool eLogErrors, bool dbLogs, bool dbLogErrors, bool fLogs, bool fLogErrors)
        {
            _eLogs = eLogs;
            _eLogErrors = eLogErrors;
            _dbLogs = dbLogs;
            _dbLogErrors = dbLogErrors;
            _fLogs = fLogs;
            _fLogErrors = fLogErrors;
        }

        public static void LogError(Exception e, string message)
        {
            Console.WriteLine(e.ToString());
            FileLogs.SaveError(message, e);
        }

        #region Information

        public static void WriteInformation(this string message, service service, eventID eventID, bool elog, bool dblog, bool flog)
        {
            if (elog) message.SaveInformationToEventLogs(service, eventID);
            if (dblog) message.SaveInformationToDB(service, eventID);
            if (flog) message.SaveInformationToFile(service, eventID);
        }

        public static void WriteInformation(this string message, service service, bool elog, bool dblog, bool flog)
        {
            if (elog) message.SaveInformationToEventLogs(service);
            if (dblog) message.SaveInformationToDB(service);
            if (flog) message.SaveInformationToFile(service);
        }

        public static void WriteInformation(this string message, eventID eventID, bool elog, bool dblog, bool flog)
        {
            if (elog) message.SaveInformationToEventLogs(eventID);
            if (dblog) message.SaveInformationToDB(eventID);
            if (flog) message.SaveInformationToFile(eventID);
        }

        public static void WriteInformation(this string message, bool elog, bool dblog, bool flog)
        {
            if (elog) message.SaveInformationToEventLogs();
            if (dblog) message.SaveInformationToDB();
            if (flog) message.SaveInformationToFile();
        }

        public static void WriteInformation(this string message, service service, eventID eventID)
        {
            WriteInformation(message, service, eventID, _eLogs, _dbLogs, _fLogs);
        }

        public static void WriteInformation(this string message, service service)
        {
            WriteInformation(message, service, _eLogs, _dbLogs, _fLogs);
        }

        public static void WriteInformation(this string message, eventID eventID)
        {
            WriteInformation(message,eventID, _eLogs, _dbLogs, _fLogs);
        }

        public static void WriteInformation(this string message)
        {
            WriteInformation(message,_eLogs, _dbLogs, _fLogs);
        }

        #endregion

        #region Warning

        public static void WriteWarning(this string message, service service, eventID eventID, bool elog, bool dblog, bool flog)
        {
            if (elog) message.SaveWarningToEventLogs(service, eventID);
            if (dblog) message.SaveWarningToDB(service, eventID);
            if (flog) message.SaveWarningToFile(service, eventID);
        }

        public static void WriteWarning(this string message, service service, bool elog, bool dblog, bool flog)
        {
            if (elog) message.SaveWarningToEventLogs(service);
            if (dblog) message.SaveWarningToDB(service);
            if (flog) message.SaveWarningToFile(service);
        }

        public static void WriteWarning(this string message, eventID eventID, bool elog, bool dblog, bool flog)
        {
            if (elog) message.SaveWarningToEventLogs(eventID);
            if (dblog) message.SaveWarningToDB(eventID);
            if (flog) message.SaveWarningToFile(eventID);
        }

        public static void WriteWarning(this string message, bool elog, bool dblog, bool flog)
        {
            if (elog) message.SaveWarningToEventLogs();
            if (dblog) message.SaveWarningToDB();
            if (flog) message.SaveWarningToFile();
        }

        public static void WriteWarning(this string message, service service, eventID eventID)
        {
            WriteWarning(message,service, eventID, _eLogs, _dbLogs, _fLogs);
        }

        public static void WriteWarning(this string message, service service)
        {
            WriteWarning(message,service, _eLogs, _dbLogs, _fLogs);
        }

        public static void WriteWarning(this string message, eventID eventID)
        {
            WriteWarning(message,eventID, _eLogs, _dbLogs, _fLogs);
        }

        public static void WriteWarning(this string message)
        {
            WriteWarning(message,_eLogs, _dbLogs, _fLogs);
        }

        #endregion

        #region Error

        public static void WriteError(this string message, service service, eventID eventID, bool elog, bool dblog, bool flog)
        {
            Console.WriteLine(String.Format("/nError:{0}", message));
            if (elog) message.SaveErrorToEventLogs(service, eventID);
            if (dblog) message.SaveErrorToDB(service, eventID);
            if (flog) message.SaveErrorToFile(service, eventID);
        }


        public static void WriteError(this string message, service service, bool elog, bool dblog, bool flog)
        {
            Console.WriteLine(String.Format("/nError:{0}", message));            
            if (elog) message.SaveErrorToEventLogs(service);
            if (dblog) message.SaveErrorToDB(service);
            if (flog) message.SaveErrorToFile(service);
        }

        public static void WriteError(this string message, eventID eventID, bool elog, bool dblog, bool flog)
        {
            Console.WriteLine(String.Format("/nError:{0}", message));              
            if (elog) message.SaveErrorToEventLogs(eventID);
            if (dblog) message.SaveErrorToDB(eventID);
            if (flog) message.SaveErrorToFile(eventID);
        }

        public static void WriteError(this string message, bool elog, bool dblog, bool flog)
        {
            Console.WriteLine(String.Format("/nError:{0}", message));              
            if (elog) message.SaveErrorToEventLogs();
            if (dblog) message.SaveErrorToDB();
            if (flog) message.SaveErrorToFile();
        }

        public static void WriteError(this string message, service service, eventID eventID)
        {
            WriteError(message, service, eventID, _eLogs, _dbLogs, _fLogs);
        }

        public static void WriteError(this string message, service service)
        {
            WriteError(message, service, _eLogs, _dbLogs, _fLogs);
        }

        public static void WriteError(this string message, eventID eventID)
        {
            WriteError(message, eventID, _eLogs, _dbLogs, _fLogs);
        }

        public static void WriteError(this string message)
        {
            WriteError(message, _eLogs, _dbLogs, _fLogs);
        }

        #endregion

        #region Exception

        public static void WriteError(this Exception ex, string message, service service, eventID eventID, bool elog, bool dblog, bool flog)
        {
            Console.WriteLine(String.Format("/nMessage:{0}/nException{1}",message,ex.ToString()) );
            if (elog) ex.SaveErrorToEventLogs(message, service, eventID);
            if (dblog) ex.SaveErrorToDB(message, service, eventID);
            if (flog) ex.SaveErrorToFile(message, service, eventID);
        }

        public static void WriteError(this Exception ex, service service, eventID eventID, bool elog, bool dblog, bool flog)
        {
            Console.WriteLine(String.Format("/nException{0}", ex.ToString()));
            if (elog) ex.SaveErrorToEventLogs(service, eventID);
            if (dblog) ex.SaveErrorToDB(service, eventID);
            if (flog) ex.SaveErrorToFile(service, eventID);
        }

        public static void WriteError(this Exception ex, string message, service service, bool elog, bool dblog, bool flog)
        {
            Console.WriteLine(String.Format("/nMessage:{0}/nException{1}", message, ex.ToString()));
            if (elog) ex.SaveErrorToEventLogs(message, service);
            if (dblog) ex.SaveErrorToDB(message, service);
            if (flog) ex.SaveErrorToFile(message, service);
        }

        public static void WriteError(this Exception ex, service service, bool elog, bool dblog, bool flog)
        {
            Console.WriteLine(String.Format("/nException{0}", ex.ToString()));
            if (elog) ex.SaveErrorToEventLogs(service);
            if (dblog) ex.SaveErrorToDB(service);
            if (flog) ex.SaveErrorToFile(service);
        }

        public static void WriteError(this Exception ex, string message, eventID eventID, bool elog, bool dblog, bool flog)
        {
            Console.WriteLine(String.Format("/nMessage:{0}/nException{1}", message, ex.ToString()));
            if (elog) ex.SaveErrorToEventLogs(message, eventID);
            if (dblog) ex.SaveErrorToDB(message, eventID);
            if (flog) ex.SaveErrorToFile(message, eventID);
        }

        public static void WriteError(this Exception ex, eventID eventID, bool elog, bool dblog, bool flog)
        {
            Console.WriteLine(String.Format("/nException{0}", ex.ToString()));
            if (elog) ex.SaveErrorToEventLogs(eventID);
            if (dblog) ex.SaveErrorToDB(eventID);
            if (flog) ex.SaveErrorToFile(eventID);
        }

        public static void WriteError(this Exception ex, string message, bool elog, bool dblog, bool flog)
        {
            Console.WriteLine(String.Format("/nMessage:{0}/nException{1}", message, ex.ToString()));
            if (elog) ex.SaveErrorToEventLogs(message);
            if (dblog) ex.SaveErrorToDB(message);
            if (flog) ex.SaveErrorToFile(message);
        }

        public static void WriteError(this Exception ex, bool elog, bool dblog, bool flog)
        {
            Console.WriteLine(String.Format("/nException{0}", ex.ToString()));
            if (elog) ex.SaveErrorToEventLogs();
            if (dblog) ex.SaveErrorToDB();
            if (flog) ex.SaveErrorToFile();
        }

        public static void WriteError(this Exception ex, string message, service service, eventID eventID)
        {
            WriteError(ex, message, service, eventID, _eLogs, _dbLogs, _fLogs);
        }

        public static void WriteError(this Exception ex, service service, eventID eventID)
        {
            WriteError(ex, service, eventID, _eLogs, _dbLogs, _fLogs);
        }

        public static void WriteError(this Exception ex, string message, service service)
        {
            WriteError(ex, message, service, _eLogs, _dbLogs, _fLogs);
        }

        public static void WriteError(this Exception ex, service service)
        {
            WriteError(ex, service, _eLogs, _dbLogs, _fLogs);
        }

        public static void WriteError(this Exception ex, string message, eventID eventID)
        {
            WriteError(ex, message, eventID, _eLogs, _dbLogs, _fLogs);
        }

        public static void WriteError(this Exception ex, eventID eventID)
        {
            WriteError(ex,  eventID, _eLogs, _dbLogs, _fLogs);
        }

        public static void WriteError(this Exception ex, string message)
        {
            WriteError(ex, message, _eLogs, _dbLogs, _fLogs);
        }

        public static void WriteError(this Exception ex)
        {
            WriteError(ex, _eLogs, _dbLogs, _fLogs);
        }


        public static string GetMessageMethod(string method)
        {
            return String.Format("Ошибка выполнения метода {0}", method);
        }

        public static void WriteErrorMethod(this Exception e, string method, service service, eventID eventID)
        {
            WriteError(e, GetMessageMethod(method), service, eventID);
        }

        public static void WriteErrorMethod(this Exception e, string method, service service)
        {
            WriteError(e, GetMessageMethod(method), service);
        }

        public static void WriteErrorMethod(this Exception e, string method, eventID eventID)
        {
            WriteError(e, GetMessageMethod(method), eventID);
        }

        public static void WriteErrorMethod(this Exception e, string method)
        {
            WriteError(e, GetMessageMethod(method));
        }

        #endregion

    }
}
