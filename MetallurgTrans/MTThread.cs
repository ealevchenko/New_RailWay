using MessageLog;
using RWSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MetallurgTrans
{
    public class MTThread
    {
        private static eventID eventID = eventID.MetallurgTrans_MTThread;
        protected static service servece_owner = service.Null;

        //protected static object locker_setting = new object();
        //protected static object locker_sftp = new object();
        //protected static object locker_db_approaches = new object();
        //protected static object locker_db_arrival = new object();

        protected Thread thTransferApproaches = null;
        public bool statusTransferApproaches { get { return thTransferApproaches.IsAlive; } }

        protected Thread thTransferArrival = null; 
        public bool statusTransferArrival { get { return thTransferArrival.IsAlive; } }

        public MTThread()
        { 
       
        }

        public MTThread(service servece_name) {
            servece_owner = servece_name;
        }

        /// <summary>
        /// Запустить поток переноса вагонов на подходах
        /// </summary>
        /// <returns></returns>
        public bool StartTransferApproaches()
        {
            service service = service.TransferApproaches;
            string mes_service_start = String.Format("Поток : {0} сервиса : {1}", service.ToString(), servece_owner);
            try
            {
                if ((thTransferApproaches == null) || (!thTransferApproaches.IsAlive && thTransferApproaches.ThreadState == ThreadState.Stopped))
                {
                    thTransferApproaches = new Thread(TransferApproaches);
                    thTransferApproaches.Name = service.ToString();
                    thTransferApproaches.Start();
                    //mes_service_start += " - запущен.";
                    //mes_service_start.WriteInformation(servece_owner, eventID);
                }
                return thTransferApproaches.IsAlive;
            }
            catch (Exception ex)
            {
                mes_service_start += " - ошибка запуска.";
                ex.WriteError(mes_service_start, servece_owner, eventID);
                //mes_service_start.WriteEvents(EventStatus.Error, servece_owner, eventID);
                return false;
            }
            
        }
        /// <summary>
        /// Запустить поток переноса вагонов по прибытию
        /// </summary>
        /// <param name="delay"></param>
        /// <returns></returns>
        public bool StartTransferArrival()
        {
            service service = service.TransferArrival;
            string mes_service_start = String.Format("Поток : {0} сервиса : {1}", service.ToString(), servece_owner);
            try
            {
                if ((thTransferArrival == null) || (!thTransferArrival.IsAlive && thTransferArrival.ThreadState == ThreadState.Stopped))
                {
                    thTransferArrival = new Thread(TransferArrival);
                    thTransferArrival.Name = service.ToString();
                    thTransferArrival.Start();
                    //mes_service_start += " - запущен.";
                    //mes_service_start.WriteEvents(EventStatus.Ok, servece_owner, eventID);
                }
                return thTransferApproaches.IsAlive;
            }
            catch (Exception ex)
            {
                mes_service_start += " - ошибка запуска.";
                ex.WriteError(mes_service_start, servece_owner, eventID);
                //mes_service_start.WriteEvents(EventStatus.Error, servece_owner, eventID);
                return false;
            }
        }
        /// <summary>
        /// Поток переноса вагонов на подходах
        /// </summary>
        /// <param name="delay"></param>
        private static void TransferApproaches()
        {
            service service = service.TransferApproaches;
            DateTime dt_start = DateTime.Now;
            try
            {
                connectSFTP connect_SFTP = new connectSFTP()
                {
                    Host = "www.metrans.com.ua",
                    Port = 222,
                    User = "arcelors",
                    PSW = "$fh#ER2J63"
                };
                string fromPathHost = "/inbox";
                string fileFiltrHost = "*.txt";
                string toDirPath = @"C:\txt";
                string toTMPDirPath = @"C:\RailWay\temp_txt";
                bool deleteFileHost = true;
                bool deleteFileMT = true;
                bool rewriteFile = false;
                // считать настройки
                //lock (locker_setting)
                {
                    try
                    {
                        // Если нет перенесем настройки в БД
                        connect_SFTP = new connectSFTP()
                        {
                            Host = RWSetting.GetDB_Config_DefaultSetting<string>("Host", service.HostMT, "www.metrans.com.ua", true),
                            Port = RWSetting.GetDB_Config_DefaultSetting<int>("Port", service.HostMT, 222, true),
                            User = RWSetting.GetDB_Config_DefaultSetting<string>("User", service.HostMT, "arcelors", true),
                            PSW = RWSetting.GetDB_Config_DefaultSetting<string>("PSW", service.HostMT, "$fh#ER2J63", true)
                        };
                        // Путь для чтения файлов из host
                        fromPathHost = RWSetting.GetDB_Config_DefaultSetting("fromPathHostTransferApproaches", service, "/inbox", true);
                        // Фильтр файлов из host
                        fileFiltrHost = RWSetting.GetDB_Config_DefaultSetting("FileFiltrHostTransferApproaches", service, "*.txt", true);
                        // Путь для записи файлов из host для постоянного хранения
                        toDirPath = RWSetting.GetDB_Config_DefaultSetting("toDirPathTransferApproaches", service, @"C:\txt", true);
                        // Путь к временной папки для записи файлов из host для дальнейшей обработки
                        toTMPDirPath = RWSetting.GetDB_Config_DefaultSetting("toTMPDirPathTransferApproaches", service, @"C:\RailWay\temp_txt", true);
                        // Признак удалять файлы после переноса
                        deleteFileHost = RWSetting.GetDB_Config_DefaultSetting("DeleteFileHostTransferApproaches", service, true, true);
                        // Признак удалять файлы после переноса
                        deleteFileMT = RWSetting.GetDB_Config_DefaultSetting("DeleteFileTransferApproaches", service, true, true);
                        // Признак перезаписывать файлы при переносе
                        rewriteFile = RWSetting.GetDB_Config_DefaultSetting("RewriteFileTransferApproaches", service, false, true);
                    }
                    catch (Exception ex)
                    {
                        ex.WriteError(String.Format("Ошибка выполнения считывания настроек потока {0}, сервиса {1}", service.ToString(), servece_owner), servece_owner, eventID);
                    }
                }
                //"TransferApproaches - работаю".WriteInformation(servece_owner, eventID);
                dt_start = DateTime.Now;
                int count_copy = 0;
                int res_transfer = 0;
                //lock (locker_sftp)
                {
                    // подключится считать и закрыть соединение
                    SFTPClient csftp = new SFTPClient(connect_SFTP, service);
                    csftp.fromPathsHost = fromPathHost;
                    csftp.FileFiltrHost = fileFiltrHost;
                    csftp.toDirPath = toDirPath;
                    csftp.toTMPDirPath = toTMPDirPath;
                    csftp.DeleteFileHost = deleteFileHost;
                    csftp.RewriteFile = rewriteFile;
                    count_copy = csftp.CopyToDir();
                }

                //lock (locker_db_approaches)
                {
                    MTTransfer mtt = new MTTransfer(service);
                    mtt.FromPath = toTMPDirPath;
                    mtt.DeleteFile = deleteFileMT;
                    res_transfer = mtt.TransferApproaches();
                }
                TimeSpan ts = DateTime.Now - dt_start;
                string mes_service_exec = String.Format("Поток {0} сервиса {1} - время выполнения: {2}:{3}:{4}({5}), код выполнения: count_copy:{6} res_transfer:{7}", service.ToString(), servece_owner, ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds, count_copy, res_transfer);
                mes_service_exec.WriteInformation(servece_owner, eventID);
                service.WriteServices(dt_start, DateTime.Now, res_transfer);
                //String.Format("Поток {0} сервиса {1} - выполнен.", service.ToString(), servece_owner).WriteWarning(servece_owner, eventID);
            }
            catch (ThreadAbortException exc)
            {
                String.Format("Поток {0} сервиса {1} - прерван по событию ThreadAbortException={2}", service.ToString(), servece_owner, exc).WriteWarning(servece_owner, eventID);
            }
            catch (Exception ex)
            {
                ex.WriteError(String.Format("Ошибка выполнения потока {0} сервиса {1}", service.ToString(), servece_owner), servece_owner, eventID);
                service.WriteServices(dt_start, DateTime.Now, -1);

            }
        }
        /// <summary>
        /// Поток переноса вагонов по прибытию
        /// </summary>
        /// <param name="delay"></param>
        private static void TransferArrival()
        {
            service service = service.TransferArrival;
            DateTime dt_start = DateTime.Now;
            try
            {
                bool arrivalToRailWay = true;
                connectSFTP connect_SFTP = new connectSFTP()
                {
                    Host = "www.metrans.com.ua",
                    Port = 222,
                    User = "arcelors",
                    PSW = "$fh#ER2J63"
                };

                string fromPathHost = "/xmlin";
                string fileFiltrHost = "*.xml";
                string toDirPath = @"C:\xml";
                string toTMPDirPath = @"C:\RailWay\temp_xml";
                bool deleteFileHost = true;
                bool deleteFileMT = true;
                bool rewriteFile = false;
                // считать настройки
                //lock (locker_setting)
                {
                    try
                    {
                        // Если нет перенесем настройки в БД
                        arrivalToRailWay = RWSetting.GetDB_Config_DefaultSetting("ArrivalToRailWay", service, arrivalToRailWay, true);
                        connect_SFTP = new connectSFTP()
                        {
                            Host = RWSetting.GetDB_Config_DefaultSetting<string>("Host", service.HostMT, "www.metrans.com.ua", true),
                            Port = RWSetting.GetDB_Config_DefaultSetting<int>("Port", service.HostMT, 222, true),
                            User = RWSetting.GetDB_Config_DefaultSetting<string>("User", service.HostMT, "arcelors", true),
                            PSW = RWSetting.GetDB_Config_DefaultSetting<string>("PSW", service.HostMT, "$fh#ER2J63", true)
                        };
                        // Путь для чтения файлов из host
                        fromPathHost = RWSetting.GetDB_Config_DefaultSetting("fromPathHostTransferArrival", service, fromPathHost, true);
                        // Фильтр файлов из host
                        fileFiltrHost = RWSetting.GetDB_Config_DefaultSetting("FileFiltrHostTransferArrival", service, fileFiltrHost, true);
                        // Путь для записи файлов из host для постоянного хранения
                        toDirPath = RWSetting.GetDB_Config_DefaultSetting("toDirPathTransferArrival", service, toDirPath, true);
                        // Путь к временной папки для записи файлов из host для дальнейшей обработки
                        toTMPDirPath = RWSetting.GetDB_Config_DefaultSetting("toTMPDirPathTransferArrival", service, toTMPDirPath, true);
                        // Признак удалять файлы после переноса
                        deleteFileHost = RWSetting.GetDB_Config_DefaultSetting("DeleteFileHostTransferArrival", service, deleteFileHost, true);
                        // Признак удалять файлы после переноса
                        deleteFileMT = RWSetting.GetDB_Config_DefaultSetting("DeleteFileTransferArrival", service, deleteFileMT, true);
                        // Признак перезаписывать файлы при переносе
                        rewriteFile = RWSetting.GetDB_Config_DefaultSetting("RewriteFileTransferArrival", service, rewriteFile, true);
                    }
                    catch (Exception ex)
                    {
                        ex.WriteError(String.Format("Ошибка выполнения считывания настроек потока {0}, сервиса {1}", service.ToString(), servece_owner), servece_owner, eventID);
                    }
                }
                        //_eventArrival.WaitOne(); // Здесь остановится
                        dt_start = DateTime.Now;
                        int count_copy = 0;
                        int res_transfer = 0;
                        //lock (locker_sftp)
                        {

                            // подключится считать и закрыть соединение
                            SFTPClient csftp = new SFTPClient(connect_SFTP, service);
                            csftp.fromPathsHost = fromPathHost;
                            csftp.FileFiltrHost = fileFiltrHost;
                            csftp.toDirPath = toDirPath;
                            csftp.toTMPDirPath = toTMPDirPath;
                            csftp.DeleteFileHost = deleteFileHost;
                            csftp.RewriteFile = rewriteFile;
                            count_copy = csftp.CopyToDir();
                        }
                        //lock (locker_db_arrival)
                        {

                            MTTransfer mtt = new MTTransfer(service);
                            mtt.ArrivalToRailWay = arrivalToRailWay;
                            mtt.FromPath = toTMPDirPath;
                            mtt.DeleteFile = deleteFileMT;
                            res_transfer = mtt.TransferArrival();
                        }
                        TimeSpan ts = DateTime.Now - dt_start;
                        string mes_service_exec = String.Format("Поток {0} сервиса {1} - время выполнения: {2}:{3}:{4}({5}), код выполнения: count_copy:{6} res_transfer:{7}.", service.ToString(), servece_owner, ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds, count_copy, res_transfer);
                        mes_service_exec.WriteInformation(servece_owner, eventID);
                        service.WriteServices(dt_start, DateTime.Now, res_transfer);
                        //String.Format("Поток {0} сервиса {1} - выполнен.", service.ToString(), servece_owner).WriteWarning(servece_owner, eventID);
            }
            catch (ThreadAbortException exc)
            {
                    String.Format("Поток {0} сервиса {1} - прерван по событию ThreadAbortException={2}", service.ToString(), servece_owner, exc).WriteWarning(servece_owner, eventID);
            }
            catch (Exception ex)
            {
                ex.WriteError(String.Format("Ошибка выполнения цикла переноса, потока {0} сервис {1}", service.ToString(), servece_owner), servece_owner, eventID);
                service.WriteServices(dt_start, DateTime.Now, -1);
            }
        }
    }
}
