using MessageLog;
using RWSettings;
using RWConversionFunctions;
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
        protected static object locker_xml_file = new object();
        protected static object locker_txt_file = new object();

        protected Thread thTransferHost = null;
        public bool statusTransferHost { get { return thTransferHost.IsAlive; } }

        protected Thread thTransferApproaches = null;
        public bool statusTransferApproaches { get { return thTransferApproaches.IsAlive; } }

        protected Thread thTransferArrival = null;
        public bool statusTransferArrival { get { return thTransferArrival.IsAlive; } }

        protected Thread thCloseTransferApproaches = null; 
        public bool statusCloseTransferApproaches { get { return thCloseTransferApproaches.IsAlive; } }

        public MTThread()
        { 
       
        }

        public MTThread(service servece_name) {
            servece_owner = servece_name;
        }

        #region TransferHost
        /// <summary>
        /// Запустить поток переноса вагонов на подходах
        /// </summary>
        /// <returns></returns>
        public bool StartTransferHost()
        {
            service service = service.TransferHost;
            string mes_service_start = String.Format("Поток : {0} сервиса : {1}", service.ToString(), servece_owner);
            try
            {
                if ((thTransferHost == null) || (!thTransferHost.IsAlive && thTransferHost.ThreadState == ThreadState.Stopped))
                {
                    thTransferHost = new Thread(TransferHost);
                    thTransferHost.Name = service.ToString();
                    thTransferHost.Start();
                    //mes_service_start += " - запущен.";
                    //mes_service_start.WriteInformation(servece_owner, eventID);
                }
                return thTransferHost.IsAlive;
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
        private static void TransferHost()
        {
            service service = service.TransferHost;
            service service_host_approaches = service.TransferApproaches;
            service service_host_arrival = service.TransferArrival;
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
                List<copyPropertySFTP> listProperty = new List<copyPropertySFTP>();
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
                        listProperty.Add(new copyPropertySFTP()
                        {
                            pathHost = RWSetting.GetDB_Config_DefaultSetting("fromPathHostTransferApproaches", service_host_approaches, "/inbox", true),
                            filtrHost = RWSetting.GetDB_Config_DefaultSetting("FileFiltrHostTransferApproaches", service_host_approaches, "*.txt", true),
                            pathReceiver = RWSetting.GetDB_Config_DefaultSetting("toDirPathTransferApproaches", service_host_approaches, @"C:\txt", true),
                            pathTempReceiver = RWSetting.GetDB_Config_DefaultSetting("toTMPDirPathTransferApproaches", service_host_approaches, @"C:\RailWay\temp_txt", true),
                            receiverDelete = RWSetting.GetDB_Config_DefaultSetting("DeleteFileHostTransferApproaches", service_host_approaches, true, true),
                            receiverRewrite = RWSetting.GetDB_Config_DefaultSetting("RewriteFileTransferApproaches", service_host_approaches, false, true)
                        });
                        listProperty.Add(new copyPropertySFTP()
                        {
                            pathHost = RWSetting.GetDB_Config_DefaultSetting("fromPathHostTransferArrival", service_host_arrival, "/xmlin", true),
                            filtrHost = RWSetting.GetDB_Config_DefaultSetting("FileFiltrHostTransferArrival", service_host_arrival, "*.xml", true),
                            pathReceiver = RWSetting.GetDB_Config_DefaultSetting("toDirPathTransferArrival", service_host_arrival, @"C:\xml", true),
                            pathTempReceiver = RWSetting.GetDB_Config_DefaultSetting("toTMPDirPathTransferArrival", service_host_arrival, @"C:\RailWay\temp_xml", true),
                            receiverDelete = RWSetting.GetDB_Config_DefaultSetting("DeleteFileHostTransferArrival", service_host_arrival, true, true),
                            receiverRewrite = RWSetting.GetDB_Config_DefaultSetting("RewriteFileTransferArrival", service_host_arrival, false, true)
                        });
                    }
                    catch (Exception ex)
                    {
                        ex.WriteError(String.Format("Ошибка выполнения считывания настроек потока {0}, сервиса {1}", service.ToString(), servece_owner), servece_owner, eventID);
                    }
                }
                dt_start = DateTime.Now;
                List<int> count_copy = null;
                lock (locker_xml_file) 
                {
                    lock (locker_txt_file)
                    {
                        // подключится считать и закрыть соединение
                        SFTPClient csftp = new SFTPClient(connect_SFTP.Host, connect_SFTP.Port, connect_SFTP.User, connect_SFTP.PSW, service);
                        count_copy = csftp.CopyToDir(listProperty);
                    }
                }
                TimeSpan ts = DateTime.Now - dt_start;
                string mes_service_exec = String.Format("Поток {0} сервиса {1} - время выполнения: {2}:{3}:{4}({5}), код выполнения: count_copy:{6}", service.ToString(), servece_owner, ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds, count_copy.IntsToString(';'));
                mes_service_exec.WriteInformation(servece_owner, eventID);
                int res = 0;
                if (count_copy != null)
                {
                    foreach (int result in count_copy)
                    {
                       if (result < 0) { res = result; break; }
                       if (result > 0) { res += result;}
                    }
                }
                service.WriteServices(dt_start, DateTime.Now, res);
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
        #endregion

        #region TransferApproaches
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
        /// Поток переноса вагонов на подходах
        /// </summary>
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
                string toTMPDirPath = @"C:\RailWay\temp_txt";
                bool deleteFileMT = true;
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
                        // Путь к временной папки для записи файлов из host для дальнейшей обработки
                        toTMPDirPath = RWSetting.GetDB_Config_DefaultSetting("toTMPDirPathTransferApproaches", service, toTMPDirPath, true);
                        // Признак удалять файлы после переноса
                        deleteFileMT = RWSetting.GetDB_Config_DefaultSetting("DeleteFileTransferApproaches", service, deleteFileMT, true);
                    }
                    catch (Exception ex)
                    {
                        ex.WriteError(String.Format("Ошибка выполнения считывания настроек потока {0}, сервиса {1}", service.ToString(), servece_owner), servece_owner, eventID);
                    }
                }
                dt_start = DateTime.Now;
                int res_transfer = 0;
                lock (locker_txt_file) 
                {
                    MTTransfer mtt = new MTTransfer(service);
                    mtt.FromPath = toTMPDirPath;
                    mtt.DeleteFile = deleteFileMT;
                    res_transfer = mtt.TransferApproaches();
                }
                TimeSpan ts = DateTime.Now - dt_start;
                string mes_service_exec = String.Format("Поток {0} сервиса {1} - время выполнения: {2}:{3}:{4}({5}), код выполнения: res_transfer:{6}", service.ToString(), servece_owner, ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds, res_transfer);
                mes_service_exec.WriteInformation(servece_owner, eventID);
                service.WriteServices(dt_start, DateTime.Now, res_transfer);
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
        #endregion

        #region TransferArrival
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
        /// Поток переноса вагонов по прибытию
        /// </summary>
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
                string toTMPDirPath = @"C:\RailWay\temp_xml";
                bool deleteFileMT = true;
                int dayrangeArrivalCars = 10;
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
                        // Путь к временной папки для записи файлов из host для дальнейшей обработки
                        toTMPDirPath = RWSetting.GetDB_Config_DefaultSetting("toTMPDirPathTransferArrival", service, toTMPDirPath, true);
                        // Признак удалять файлы после переноса
                        deleteFileMT = RWSetting.GetDB_Config_DefaultSetting("DeleteFileTransferArrival", service, deleteFileMT, true);
                        // Период для определения незакрытого состава и вагона 
                        dayrangeArrivalCars = RWSetting.GetDB_Config_DefaultSetting<int>("DayRangeArrivalCars", service, dayrangeArrivalCars, true);
                    }
                    catch (Exception ex)
                    {
                        ex.WriteError(String.Format("Ошибка выполнения считывания настроек потока {0}, сервиса {1}", service.ToString(), servece_owner), servece_owner, eventID);
                    }
                }
                        dt_start = DateTime.Now;
                        int res_transfer = 0;
                        lock (locker_xml_file)
                        {
                            MTTransfer mtt = new MTTransfer(service);
                            mtt.DayRangeArrivalCars = dayrangeArrivalCars;
                            mtt.ArrivalToRailWay = arrivalToRailWay;
                            mtt.FromPath = toTMPDirPath;
                            mtt.DeleteFile = deleteFileMT;
                            res_transfer = mtt.TransferArrival();
                        }
                        TimeSpan ts = DateTime.Now - dt_start;
                        string mes_service_exec = String.Format("Поток {0} сервиса {1} - время выполнения: {2}:{3}:{4}({5}), код выполнения: res_transfer:{6}.", service.ToString(), servece_owner, ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds, res_transfer);
                        mes_service_exec.WriteInformation(servece_owner, eventID);
                        service.WriteServices(dt_start, DateTime.Now, res_transfer);
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
        #endregion

        #region CloseApproachesCars
        /// <summary>
        /// Запустить поток закрытия вагонов на подходах
        /// </summary>
        /// <returns></returns>
        public bool StartCloseApproachesCars()
        {
            service service = service.CloseTransferApproaches;
            string mes_service_start = String.Format("Поток : {0} сервиса : {1}", service.ToString(), servece_owner);
            try
            {
                if ((thCloseTransferApproaches == null) || (!thCloseTransferApproaches.IsAlive && thCloseTransferApproaches.ThreadState == ThreadState.Stopped))
                {
                    thCloseTransferApproaches = new Thread(CloseApproachesCars);
                    thCloseTransferApproaches.Name = service.ToString();
                    thCloseTransferApproaches.Start();
                    //mes_service_start += " - запущен.";
                    //mes_service_start.WriteEvents(EventStatus.Ok, servece_owner, eventID);
                }
                return thCloseTransferApproaches.IsAlive;
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
        /// Поток закрытия вагонов на подходах
        /// </summary>
        private static void CloseApproachesCars()
        {
            service service = service.CloseTransferApproaches;
            DateTime dt_start = DateTime.Now;
            try
            {
                int day_range_approaches_cars = 30;
                int day_range_approaches_cars_arrival = 3;
                // считать настройки
                //lock (locker_setting)
                {
                    try
                    {   
                        // тайм аут (дней) по времени для вагонов на подходе
                        day_range_approaches_cars = RWSetting.GetDB_Config_DefaultSetting<int>("DayRangeApproachesCars", service, day_range_approaches_cars, true);
                        // тайм аут (дней) по времени для вагонов на подходе прибывших на конечную станцию
                        day_range_approaches_cars_arrival = RWSetting.GetDB_Config_DefaultSetting<int>("DayRangeApproachesCarsArrival", service, day_range_approaches_cars_arrival, true);
                    }
                    catch (Exception ex)
                    {
                        ex.WriteError(String.Format("Ошибка выполнения считывания настроек потока {0}, сервиса {1}", service.ToString(), servece_owner), servece_owner, eventID);
                    }
                }
                dt_start = DateTime.Now;
                int res_close = 0;
                {
                    MTTransfer mtt = new MTTransfer(service);
                    mtt.DayRangeApproachesCars = 30; // тайм аут (дней) по времени для вагонов на подходе
                    mtt.DayRangeApproachesCarsArrival = 3; // тайм аут (дней) по времени для вагонов на подходе прибывших на конечную станцию
                    res_close = mtt.CloseApproachesCars();
                }
                TimeSpan ts = DateTime.Now - dt_start;
                string mes_service_exec = String.Format("Поток {0} сервиса {1} - время выполнения: {2}:{3}:{4}({5}), код выполнения: res_close:{6}", service.ToString(), servece_owner, ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds, res_close);
                mes_service_exec.WriteInformation(servece_owner, eventID);
                int res = res_close;
                service.WriteServices(dt_start, DateTime.Now, res);
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
        #endregion
    }
}
