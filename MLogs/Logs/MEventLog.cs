using MessageLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageLog
{
    public static class MEventLog
    {
        static private string eventSourceName;
        static private string logName;

        static MEventLog()
        {
            FileLogs.InitLogger();
            try
            {
                eventSourceName = ConfigurationManager.AppSettings["eventSourceName"].ToString();
                logName = ConfigurationManager.AppSettings["logName"].ToString();
            }
            catch (Exception e)
            {
                eventSourceName = "RailWay";
                logName = "RailWayLogFile1";
                LogError(e, String.Format("Ошибка чтения AppSettings:(eventSourceName,logName)"));
            }

            try
            {
                EventLogs.InitEventLogs(eventSourceName, logName);
            }
            catch (Exception e)
            {
                LogError(e, String.Format("Ошибка выполнения метода EventLogs.InitEventLogs(eventSourceName={0}, logName={1})",eventSourceName,logName));
            }
        }

        public static void LogError(Exception e, string message)
        {
            Console.WriteLine(e.ToString());
            FileLogs.SaveError(message, e);
        }
        
        #region Logs

            #region Information

            public static void SaveInformationToEventLogs(this string log)
            {
                EventLogs.SaveInformation(log);
            }

            public static void SaveInformationToEventLogs(this string log, service service, eventID eventID)
            {
                EventLogs.SaveInformation(log, (service == service.Null ? (int?)null : (int)service), (eventID == eventID.Null ? (int?)null : (int)eventID));
            }

            public static void SaveInformationToEventLogs(this string log, service service)
            {
                log.SaveInformationToEventLogs(service, eventID.Null);
            }

            public static void SaveInformationToEventLogs(this string log, eventID eventID)
            {
                log.SaveInformationToEventLogs(service.Null, eventID);
            }

            #endregion

            #region Warning

            public static void SaveWarningToEventLogs(this string log)
            {
                EventLogs.SaveWarning(log);
            }

            public static void SaveWarningToEventLogs(this string log, service service, eventID eventID)
            {
                EventLogs.SaveWarning(log, (service == service.Null ? (int?)null : (int)service), (eventID == eventID.Null ? (int?)null : (int)eventID));
            }

            public static void SaveWarningToEventLogs(this string log, service service)
            {
                log.SaveWarningToEventLogs(service, eventID.Null);
            }

            public static void SaveWarningToEventLogs(this string log, eventID eventID)
            {
                log.SaveWarningToEventLogs(service.Null, eventID);
            }

            #endregion

            #region Error

            public static void SaveErrorToEventLogs(this string log)
            {
                EventLogs.SaveError(log);
            }

            public static void SaveErrorToEventLogs(this string log, service service, eventID eventID)
            {
                EventLogs.SaveError(log, (service == service.Null ? (int?)null : (int)service), (eventID == eventID.Null ? (int?)null : (int)eventID));
            }

            public static void SaveErrorToEventLogs(this string log, service service)
            {
                log.SaveErrorToEventLogs(service, eventID.Null);
            }

            public static void SaveErrorToEventLogs(this string log, eventID eventID)
            {
                log.SaveErrorToEventLogs(service.Null, eventID);
            }

            #endregion

            //TODO: По необходимости описать логирование событий SuccessAudit = 8, FailureAudit = 16

        #endregion

        #region LogsErors

            public static void SaveErrorToEventLogs(this Exception ex, string user_message, service service, eventID eventID)
            {
                EventLogs.SaveError(ex, user_message, (service == service.Null ? (int?)null : (int)service), (eventID == eventID.Null ? (int?)null : (int)eventID));
            }

            public static void SaveErrorToEventLogs(this Exception ex, service service, eventID eventID)
            {
                EventLogs.SaveError(ex, (service == service.Null ? (int?)null : (int)service), (eventID == eventID.Null ? (int?)null : (int)eventID));
            }

            public static void SaveErrorToEventLogs(this Exception ex, string user_message, service service)
            {
                ex.SaveErrorToEventLogs(user_message, service, eventID.Null);
            }

            public static void SaveErrorToEventLogs(this Exception ex, service service)
            {
                ex.SaveErrorToEventLogs(service, eventID.Null);
            }

            public static void SaveErrorToEventLogs(this Exception ex, string user_message, eventID eventID)
            {
                ex.SaveErrorToEventLogs(user_message, service.Null, eventID);
            }

            public static void SaveErrorToEventLogs(this Exception ex, eventID eventID)
            {
                ex.SaveErrorToEventLogs(service.Null, eventID);
            }

            public static void SaveErrorToEventLogs(this Exception ex, string user_message)
            {
                EventLogs.SaveError(ex,user_message);
            }

            public static void SaveErrorToEventLogs(this Exception ex)
            {
                EventLogs.SaveError(ex);
            }

            #endregion
    }
}
