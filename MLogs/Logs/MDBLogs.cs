using MessageLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageLog
{
    public static class MDBLogs
    {
       
        #region Logs

            #region Information

            public static long SaveInformationToDB(this string log)
            {
                return DBLogs.SaveInformation(log);
            }
    
            public static long SaveInformationToDB(this string log,service service, eventID eventID)
            {
                return DBLogs.SaveInformation(log, (service ==service.Null ? (int?)null : (int)service), (eventID ==eventID.Null ? (int?)null : (int)eventID));
            }

            public static long SaveInformationToDB(this string log,service service)
            {
                return log.SaveInformationToDB(service,eventID.Null);
            }

            public static long SaveInformationToDB(this string log,eventID eventID)
            {
                return log.SaveInformationToDB(service.Null, eventID);
            }

            #endregion

            #region Warning

            public static long SaveWarningToDB(this string log)
            {
                return DBLogs.SaveWarning(log);
            }

            public static long SaveWarningToDB(this string log,service service,eventID eventID)
            {
                return DBLogs.SaveWarning(log, (service ==service.Null ? (int?)null : (int)service), (eventID ==eventID.Null ? (int?)null : (int)eventID));
            }

            public static long SaveWarningToDB(this string log,service service)
            {
                return log.SaveWarningToDB(service,eventID.Null);
            }

            public static long SaveWarningToDB(this string log,eventID eventID)
            {
                return log.SaveWarningToDB(service.Null, eventID);
            }

            #endregion

            #region Error

            public static long SaveErrorToDB(this string log)
            {
                return DBLogs.SaveError(log);
            }

            public static long SaveErrorToDB(this string log,service service,eventID eventID)
            {
                return DBLogs.SaveError(log, (service ==service.Null ? (int?)null : (int)service), (eventID ==eventID.Null ? (int?)null : (int)eventID));
            }

            public static long SaveErrorToDB(this string log,service service)
            {
                return log.SaveErrorToDB(service,eventID.Null);
            }

            public static long SaveErrorToDB(this string log,eventID eventID)
            {
                return log.SaveErrorToDB(service.Null, eventID);
            }

            #endregion

            #region Critical

            public static long SaveCriticalToDB(this string log)
            {
                return DBLogs.SaveCritical(log);
            }

            public static long SaveCriticalToDB(this string log,service service,eventID eventID)
            {
                return log.SaveCritical((service ==service.Null ? (int?)null : (int)service), (eventID ==eventID.Null ? (int?)null : (int)eventID));
            }

            public static long SaveCriticalToDB(this string log,service service)
            {
                return log.SaveCriticalToDB(service,eventID.Null);
            }

            public static long SaveCriticalToDB(this string log,eventID eventID)
            {
                return log.SaveCriticalToDB(service.Null, eventID);
            }

            #endregion

        #endregion

        #region LogsErors

            public static long SaveErrorToDB(this Exception ex, string user_message,service service,eventID eventID)
            {
                return DBLogs.SaveError(ex,user_message,(service ==service.Null ? (int?)null : (int)service), (eventID ==eventID.Null ? (int?)null : (int)eventID));
            }

            public static long SaveErrorToDB(this Exception ex,service service,eventID eventID)
            {
                return DBLogs.SaveError(ex,(service ==service.Null ? (int?)null : (int)service), (eventID ==eventID.Null ? (int?)null : (int)eventID));
            }

            public static long SaveErrorToDB(this Exception ex, string user_message,service service)
            {
                return ex.SaveErrorToDB(user_message, service,eventID.Null);
            }

            public static long SaveErrorToDB(this Exception ex,service service)
            {
                return ex.SaveErrorToDB(service,eventID.Null);
            }

            public static long SaveErrorToDB(this Exception ex, string user_message,eventID eventID)
            {
                return ex.SaveErrorToDB(user_message,service.Null, eventID);
            }

            public static long SaveErrorToDB(this Exception ex,eventID eventID)
            {
                return ex.SaveErrorToDB(service.Null, eventID);
            }

            public static long SaveErrorToDB(this Exception ex, string user_message)
            {
                return DBLogs.SaveError(ex,user_message);
            }

            public static long SaveErrorToDB(this Exception ex)
            {
                return DBLogs.SaveError(ex);
            }

        #endregion

    }
}
