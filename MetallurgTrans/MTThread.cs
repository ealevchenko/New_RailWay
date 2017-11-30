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

        protected static object locker_setting = new object();
        protected static object locker_sftp = new object();
        protected static object locker_db_approaches = new object();
        protected static object locker_db_arrival = new object();

        protected Thread thTransferApproaches = new Thread(new ParameterizedThreadStart(TransferApproaches));
        public bool statusTransferApproaches { get { return thTransferApproaches.IsAlive; } }

        protected static bool run_approaches = false;

        protected Thread thTransferArrival = new Thread(new ParameterizedThreadStart(TransferArrival));
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
        /// <param name="delay"></param>
        /// <returns></returns>
        public bool StartTransferApproaches(int delay)
        {
            service service = service.TransferApproaches;
            string mes_service_start = String.Format("Поток : {0} сервиса : {1}", service.ToString(), servece_owner);
            try
            {
                if (!thTransferApproaches.IsAlive)
                {
                    run_approaches = true;
                    if (thTransferApproaches.ThreadState == ThreadState.Unstarted)
                    {
                        thTransferApproaches.Name = service.ToString();
                    }
                    thTransferApproaches.Start(delay * 1000);
                    mes_service_start += " - запущен.";
                    mes_service_start.WriteEvents(EventStatus.Ok, servece_owner, eventID);
                }
                return true;
            }
            catch (Exception ex)
            {
                mes_service_start += " - ошибка запуска.";
                ex.WriteError(mes_service_start, servece_owner, eventID);
                mes_service_start.WriteEvents(EventStatus.Error, servece_owner, eventID);
                return false;
            }
            
        }
        /// <summary>
        /// Остановить поток переноса вагонов на подходах
        /// </summary>
        /// <returns></returns>
        public bool StopTransferApproaches()
        {
            service service = service.TransferApproaches;
            string mes_service_stop = String.Format("Поток : {0} сервиса : {1}", service.ToString(), servece_owner);
            try
            {
                //thTransferApproaches.Abort();
                //thTransferApproaches.Join(1000);
                run_approaches = false;
                //mes_service_stop += " - остановлен.";
                //mes_service_stop.WriteEvents(EventStatus.Ok, servece_owner, eventID);
                return true;
            }
            catch (Exception ex)
            {
                mes_service_stop += " - ошибка остановки.";
                ex.WriteError(mes_service_stop, servece_owner, eventID);
                mes_service_stop.WriteEvents(EventStatus.Error, servece_owner, eventID);
                return false;
            }
        }
        /// <summary>
        /// Запустить поток переноса вагонов по прибытию
        /// </summary>
        /// <param name="delay"></param>
        /// <returns></returns>
        public bool StartTransferArrival(int delay)
        {
            service service = service.TransferArrival;
            string mes_service_start = String.Format("Поток : {0} сервиса : {1}", service.ToString(), servece_owner);
            try
            {
                if (!thTransferArrival.IsAlive)
                {
                    thTransferArrival.Name = service.ToString();
                    thTransferArrival.Start(delay * 1000);
                    mes_service_start += " - запущен.";
                    mes_service_start.WriteEvents(EventStatus.Ok, servece_owner, eventID);
                }
                return true;
            }
            catch (Exception ex)
            {
                mes_service_start += " - ошибка запуска.";
                ex.WriteError(mes_service_start, servece_owner, eventID);
                mes_service_start.WriteEvents(EventStatus.Error, servece_owner, eventID);
                return false;
            }
        }
        /// <summary>
        /// Остановить поток переноса вагонов по прибытию
        /// </summary>
        /// <returns></returns>
        public bool StopTransferArrival()
        {
            service service = service.TransferArrival;
            string mes_service_stop = String.Format("Поток : {0} сервиса : {1}", service.ToString(), servece_owner);
            try
            {
                thTransferArrival.Abort();
                thTransferArrival.Join(1000);
                mes_service_stop += " - остановлен.";
                mes_service_stop.WriteEvents(EventStatus.Ok, servece_owner, eventID);
                return true;
            }
            catch (Exception ex)
            {
                mes_service_stop += " - ошибка останова.";
                ex.WriteError(mes_service_stop, servece_owner, eventID);
                mes_service_stop.WriteEvents(EventStatus.Error, servece_owner, eventID);
                return false;
            }
        }
        /// <summary>
        /// Поток переноса вагонов на подходах
        /// </summary>
        /// <param name="delay"></param>
        private static void TransferApproaches(object delay)
        {
            bool abort = true;
            int delay_transfer = (int)delay;
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
                lock (locker_setting)
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
                while (run_approaches) // слушаем всегда
                {
                    "TransferApproaches - работаю".WriteInformation(servece_owner, eventID);
                    try
                    {
                        dt_start = DateTime.Now;
                        int count_copy = 0;
                        int res_transfer = 0;
                        lock (locker_sftp)
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

                        lock (locker_db_approaches)
                        {
                            MTTransfer mtt = new MTTransfer(service);
                            mtt.FromPath = toTMPDirPath;
                            mtt.DeleteFile = deleteFileMT;
                            res_transfer = mtt.TransferApproaches();
                        }
                        TimeSpan ts = DateTime.Now - dt_start;
                        int sleep = delay_transfer - (int)ts.TotalMilliseconds; // Определим время для задержки, отнимим от времени задержки по умолчанию время выполнения переноса.
                        string mes_service_exec = String.Format("Поток {0} сервиса {1} - время выполнения: {2}:{3}:{4}({5}), код выполнения: count_copy:{6} res_transfer:{7}, следующее выполнение через {8} милисек.", service.ToString(), servece_owner, ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds, count_copy, res_transfer, sleep);
                        mes_service_exec.WriteInformation(servece_owner, eventID);
                        service.WriteServices(dt_start, DateTime.Now, res_transfer);
                        if (sleep > 0) 
                        while (sleep > 0) { 
                            Thread.Sleep(1);
                            sleep--;
                            if (!run_approaches) break;
                        }
                    }
                    catch (ThreadAbortException exc)
                    {
                        if (abort)
                        {
                            String.Format("Поток {0} сервиса {1} - прерван по событию ThreadAbortException={2}", service.ToString(), servece_owner, exc).WriteWarning(servece_owner, eventID);
                            abort = false;
                            run_approaches = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        ex.WriteError(String.Format("Ошибка выполнения цикла переноса, потока {0} сервиса {1}", service.ToString(), servece_owner), servece_owner, eventID);
                        service.WriteServices(dt_start, DateTime.Now, -1);
                    }
                } // {end слушаем всегда}
                String.Format("Поток {0} сервиса {1} - остановлен, run_approaches={2}", service.ToString(), servece_owner, run_approaches).WriteWarning(servece_owner, eventID);
            }
            catch (ThreadAbortException exc)
            {
                if (abort)
                {
                    String.Format("Поток {0} сервиса {1} - прерван по событию ThreadAbortException={2}", service.ToString(), servece_owner, exc).WriteWarning(servece_owner, eventID);
                    abort = false;
                    run_approaches = false;
                }
            }
            catch (Exception ex)
            {
                ex.WriteError(String.Format("Ошибка выполнения потока {0} сервиса {1}", service.ToString(), servece_owner), servece_owner, eventID);
                service.WriteServices(dt_start, DateTime.Now, -1);
                run_approaches = false;
            }

        }
        /// <summary>
        /// Поток переноса вагонов по прибытию
        /// </summary>
        /// <param name="delay"></param>
        private static void TransferArrival(object delay)
        {
            bool abort = true;
            int delay_transfer = (int)delay;
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
                lock (locker_setting)
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
                while (true) // слушаем всегда
                {
                    try
                    {
                        //_eventArrival.WaitOne(); // Здесь остановится
                        dt_start = DateTime.Now;
                        int count_copy = 0;
                        int res_transfer = 0;
                        lock (locker_sftp)
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
                        lock (locker_db_arrival)
                        {

                            MTTransfer mtt = new MTTransfer(service);
                            mtt.ArrivalToRailWay = arrivalToRailWay;
                            mtt.FromPath = toTMPDirPath;
                            mtt.DeleteFile = deleteFileMT;
                            res_transfer = mtt.TransferArrival();
                        }
                        TimeSpan ts = DateTime.Now - dt_start;
                        int sleep = delay_transfer - (int)ts.TotalMilliseconds; // Определим время для задержки, отнимим от времени задержки по умолчанию время выполнения переноса.
                        string mes_service_exec = String.Format("Поток {0} сервиса {1} - время выполнения: {2}:{3}:{4}({5}), код выполнения: count_copy:{6} res_transfer:{7}, следующее выполнение через {8} милисек.", service.ToString(), servece_owner, ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds, count_copy, res_transfer, sleep);
                        mes_service_exec.WriteInformation(servece_owner, eventID);
                        service.WriteServices(dt_start, DateTime.Now, res_transfer);
                        if (sleep > 0) Thread.Sleep(sleep);
                    }
                    catch (ThreadAbortException exc)
                    {
                        if (abort)
                        {
                            String.Format("Поток {0} сервиса {1} - прерван по событию ThreadAbortException={2}", service.ToString(), servece_owner, exc).WriteWarning(servece_owner, eventID);
                            abort = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        ex.WriteError(String.Format("Ошибка выполнения цикла переноса, потока {0} сервиса {1}", service.ToString(), servece_owner), servece_owner, eventID);
                        service.WriteServices(dt_start, DateTime.Now, -1);
                    }
                } // {end слушаем всегда}
            }
            catch (ThreadAbortException exc)
            {
                if (abort)
                {
                    String.Format("Поток {0} сервиса {1} - прерван по событию ThreadAbortException={2}", service.ToString(), servece_owner, exc).WriteWarning(servece_owner, eventID);
                    abort = false;
                }
            }
            catch (Exception ex)
            {
                ex.WriteError(String.Format("Ошибка выполнения цикла переноса, потока {0} сервис {1}", service.ToString(), servece_owner), servece_owner, eventID);
                service.WriteServices(dt_start, DateTime.Now, -1);
            }
        }
    }
}
