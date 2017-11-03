using MessageLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MessageLog
{
    public static class MFileLogs
    {
      
        static MFileLogs()
        {
            FileLogs.InitLogger();
        }

        public static string GetSource(service service,  eventID eventID)
        {
            return String.Format("[service.{0}, eventID.{1}] ", service, eventID);
        }

        #region Information

        public static void SaveInformationToFile(this string log)
        {
            FileLogs.SaveInformation(log);
        }

        public static void SaveInformationToFile(this string log, service service, eventID eventID)
        {
            (GetSource(service, eventID) + log).SaveInformationToFile();
        }

        public static void SaveInformationToFile(this string log, service service)
        {
            (GetSource(service, eventID.Null) + log).SaveInformationToFile();
        }

        public static void SaveInformationToFile(this string log, eventID eventID)
        {
            (GetSource(service.Null, eventID) + log).SaveInformationToFile();
        }

        #endregion

        #region Warning

        public static void SaveWarningToFile(this string log)
        {
            FileLogs.SaveWarning(log);
        }

        public static void SaveWarningToFile(this string log, service service, eventID eventID)
        {
            (GetSource(service, eventID) + log).SaveWarningToFile();
        }

        public static void SaveWarningToFile(this string log, service service)
        {
            (GetSource(service, eventID.Null) + log).SaveWarningToFile();
        }

        public static void SaveWarningToFile(this string log, eventID eventID)
        {
            (GetSource(service.Null, eventID) + log).SaveWarningToFile();
        }

        #endregion

        #region Error

        public static void SaveErrorToFile(this string log)
        {
            FileLogs.SaveError(log);
        }

        public static void SaveErrorToFile(this string log, service service, eventID eventID)
        {
            (GetSource(service, eventID) + log).SaveErrorToFile();
        }

        public static void SaveErrorToFile(this string log, service service)
        {
            (GetSource(service, eventID.Null) + log).SaveErrorToFile();
        }

        public static void SaveErrorToFile(this string log, eventID eventID)
        {
            (GetSource(service.Null, eventID) + log).SaveErrorToFile();
        }

        #endregion

        #region Exception

        public static void SaveErrorToFile(this Exception ex, string user_message, service service, eventID eventID)
        {
            (GetSource(service, eventID) + user_message).SaveError(ex);
        }

        public static void SaveErrorToFile(this Exception ex, service service, eventID eventID)
        {
            GetSource(service, eventID).SaveError(ex);
        }

        public static void SaveErrorToFile(this Exception ex, string user_message, service service)
        {
            (GetSource(service, eventID.Null) + user_message).SaveError(ex);
        }

        public static void SaveErrorToFile(this Exception ex, service service)
        {
            GetSource(service, eventID.Null).SaveError(ex);
        }

        public static void SaveErrorToFile(this Exception ex, string user_message, eventID eventID)
        {
            (GetSource(service.Null, eventID) + user_message).SaveError(ex);
        }

        public static void SaveErrorToFile(this Exception ex, eventID eventID)
        {
            GetSource(service.Null, eventID).SaveError(ex);
        }

        public static void SaveErrorToFile(this Exception ex, string user_message)
        {
            user_message.SaveError(ex);
        }

        public static void SaveErrorToFile(this Exception ex)
        {
            FileLogs.SaveError("", ex);
        }

        #endregion
    }
}
