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

        protected Thread thTransferApproaches = new Thread(TransferApproaches);
        public bool statusTransferApproaches { get { return thTransferApproaches.IsAlive; } }

        static ManualResetEvent _eventApproaches = new ManualResetEvent(true);

        public MTThread()
        { 
       
        }

        public MTThread(service servece_name) {
            servece_owner = servece_name;
        }

        public bool StartTransferApproaches()
        {
            bool res = false;
            service service = service.TransferApproaches;
            string mes_service_start = String.Format("Поток {0} сервиса {1}", service.ToString(), servece_owner);
            try
            {
                //string stat = mes_service_start + String.Format(" IsAlive = {0}, ThreadState = {1}", thTransferApproaches.IsAlive.ToString(), thTransferApproaches.ThreadState.ToString());
                //ServicesEventLog.LogWarning(stat,servece_owner, eventID);
                if (!thTransferApproaches.IsAlive)
                {
                    thTransferApproaches.Name = service.ToString();
                    thTransferApproaches.Start();
                }
                else
                {
                    if (thTransferApproaches.ThreadState == ThreadState.WaitSleepJoin)
                    {
                        //string run = mes_service_start +". Выполняю _event.Set()";
                        //ServicesEventLog.LogWarning(run,servece_owner, eventID);
                        _eventApproaches.Set();
                    }
                }
                res = true;
            }
            catch (Exception ex)
            {
                mes_service_start += " - ошибка запуска.";
                ex.WriteError(mes_service_start, servece_owner, eventID);
                res = false;
                mes_service_start.SaveEvents(EventStatus.Error, servece_owner, eventID);
            }
            return res;
        }

        private static void TransferApproaches()
        {
            service service = service.TransferApproaches;
            bool error_setting = false;
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
                        fromPathHost = RWSetting.GetDB_Config_DefaultSetting("fromPathHostTransferApproaches", service.TransferApproaches, "/inbox", true);
                        // Фильтр файлов из host
                        fileFiltrHost = RWSetting.GetDB_Config_DefaultSetting("FileFiltrHostTransferApproaches", service.TransferApproaches, "*.txt", true);
                        // Путь для записи файлов из host для постоянного хранения
                        toDirPath = RWSetting.GetDB_Config_DefaultSetting("toDirPathTransferApproaches", service.TransferApproaches, @"C:\txt", true);
                        // Путь к временной папки для записи файлов из host для дальнейшей обработки
                        toTMPDirPath = RWSetting.GetDB_Config_DefaultSetting("toTMPDirPathTransferApproaches", service.TransferApproaches, @"C:\RailWay\temp_txt", true);
                        // Признак удалять файлы после переноса
                        deleteFileHost = RWSetting.GetDB_Config_DefaultSetting("DeleteFileHostTransferApproaches", service.TransferApproaches, true, true);
                        // Признак удалять файлы после переноса
                        deleteFileMT = RWSetting.GetDB_Config_DefaultSetting("DeleteFileTransferApproaches", service.TransferApproaches, true, true);
                        // Признак перезаписывать файлы при переносе
                        rewriteFile = RWSetting.GetDB_Config_DefaultSetting("RewriteFileTransferApproaches", service.TransferApproaches, false, true);

                    }
                    catch (Exception ex)
                    {
                        ex.WriteError(String.Format("Ошибка выполнения считывания настроек потока {0}, сервиса {1}", service.ToString(), servece_owner), servece_owner, eventID);
                        error_setting = true;
                    }
                }
                while (true) // слушаем всегда
                {
                    _eventApproaches.WaitOne(); // Здесь остановится
                    dt_start = DateTime.Now;
                    if (!error_setting)
                    {
                        int count_copy = 0;
                        int res_transfer = 0;
                        lock (locker_sftp)
                        {

                            // подключится считать и закрыть соединение
                            SFTPClient csftp = new SFTPClient(connect_SFTP, service.TransferApproaches);
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
                        string mes_service_exec = String.Format("Поток {0} сервиса {1} - время выполнения: {2} мин {3} сек {4} мсек, код выполнения error_setting:{5} count_copy:{6} res_transfer:{7}", service.ToString(), servece_owner, ts.Minutes, ts.Seconds, ts.Milliseconds, error_setting, count_copy, res_transfer);
                        mes_service_exec.WriteInformation(servece_owner, eventID);

                        mes_service_exec.SaveEvents(error_setting | count_copy < 0 | res_transfer < 0 ? EventStatus.Error : EventStatus.Ok, servece_owner, eventID);
                        service.WriteServices(dt_start, DateTime.Now, res_transfer);
                    }
                    _eventApproaches.Reset(); // Останавливаем и ждем 
                } // {end слушаем всегда}
            }
            catch (Exception ex)
            {
                ex.WriteError(String.Format("Ошибка выполнения цикла переноса, потока {0} сервис {1}", service.ToString(), servece_owner), servece_owner, eventID);
                service.WriteServices(dt_start, DateTime.Now, -1);
            }
        }
    }
}
