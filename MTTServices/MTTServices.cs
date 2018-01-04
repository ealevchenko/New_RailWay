using MessageLog;
using MetallurgTrans;
using RWSettings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MTTServices
{
    public partial class MTTServices : ServiceBase
    {
        private eventID eventID = eventID.MTTServices;
        private service servece_name = service.TransferMT;
        private service thread_host = service.TransferHost;
        private service thread_approaches = service.TransferApproaches;
        private service thread_arrival = service.TransferArrival;
        private service thread_close_approaches = service.CloseTransferApproaches;

        private int interval_transfer_host = 300; // секунд
        private int interval_transfer_approaches = 300; // секунд
        private int interval_transfer_arrival = 300; // секунд
        private int interval_close_approaches = 3600; // 1раз в час

        bool active_transfer_host = true;        
        bool active_transfer_approaches = true;
        bool active_transfer_arrival = true;
        bool active_close_approaches = true;

        System.Timers.Timer timer_services = new System.Timers.Timer();
        System.Timers.Timer timer_services_host = new System.Timers.Timer();
        System.Timers.Timer timer_services_approaches = new System.Timers.Timer();
        System.Timers.Timer timer_services_arrival = new System.Timers.Timer();
        System.Timers.Timer timer_services_close_approaches = new System.Timers.Timer();

        private MTThread mtt = new MTThread(service.TransferMT);

        public enum ServiceState
        {
            SERVICE_STOPPED = 0x00000001,
            SERVICE_START_PENDING = 0x00000002,
            SERVICE_STOP_PENDING = 0x00000003,
            SERVICE_RUNNING = 0x00000004,
            SERVICE_CONTINUE_PENDING = 0x00000005,
            SERVICE_PAUSE_PENDING = 0x00000006,
            SERVICE_PAUSED = 0x00000007,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ServiceStatus
        {
            public long dwServiceType;
            public ServiceState dwCurrentState;
            public long dwControlsAccepted;
            public long dwWin32ExitCode;
            public long dwServiceSpecificExitCode;
            public long dwCheckPoint;
            public long dwWaitHint;
        };

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);

        public MTTServices(string[] args)
        {
            InitializeComponent();
            InitializeService();
        }

        #region Управление службой
        /// <summary>
        /// Инициализация сервиса (проверка данных в БД и создание settings)
        /// </summary>
        public void InitializeService() 
        {
            try
            {
                // интервалы
                this.interval_transfer_host = RWSetting.GetDB_Config_DefaultSetting("IntervalTransferHost", this.thread_host, this.interval_transfer_host, true);
                this.interval_transfer_approaches = RWSetting.GetDB_Config_DefaultSetting("IntervalTransferApproaches", this.thread_approaches, this.interval_transfer_approaches, true);
                this.interval_transfer_arrival = RWSetting.GetDB_Config_DefaultSetting("IntervalTransferArrival", this.thread_arrival, this.interval_transfer_arrival, true);
                this.interval_close_approaches = RWSetting.GetDB_Config_DefaultSetting("IntervalCloseApproachesCars", this.thread_close_approaches, this.interval_close_approaches, true);

                // состояние активности
                this.active_transfer_host = RWSetting.GetDB_Config_DefaultSetting("ActiveTransferHost", this.thread_host, this.active_transfer_host, true);
                this.active_transfer_approaches = RWSetting.GetDB_Config_DefaultSetting("ActiveTransferApproaches", this.thread_approaches, this.active_transfer_approaches, true);
                this.active_transfer_arrival = RWSetting.GetDB_Config_DefaultSetting("ActiveTransferArrival", this.thread_arrival, this.active_transfer_arrival, true);
                this.active_close_approaches  = RWSetting.GetDB_Config_DefaultSetting("ActiveCloseApproachesCars", this.thread_close_approaches, this.active_close_approaches, true);
                
                //// Настроем таймер контроля выполнения сервиса
                timer_services.Interval = 30000;
                timer_services.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimerServices);

                timer_services_host.Interval = this.interval_transfer_host * 1000;
                timer_services_host.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimerServicesHost);
                
                timer_services_approaches.Interval = this.interval_transfer_approaches * 1000;
                timer_services_approaches.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimerServicesApproaches);

                timer_services_arrival.Interval = this.interval_transfer_arrival * 1000;
                timer_services_arrival.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimerServicesArrival);

                timer_services_close_approaches.Interval = this.interval_close_approaches * 1000;
                timer_services_close_approaches.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimerServicesCloseApproachesCars);

                //Добавить инициализацию других таймеров
                //...............
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("InitializeService()"), this.servece_name, eventID);
                return;
            } 
        }

        protected override void OnStart(string[] args)
        {
            // Update the service state to Start Pending.
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            // Запустить таймер контроля сервиса
            timer_services.Start();
            // Запустить таймера потоков
            RunTimerTransferHost();
            RunTimerTransferApproaches();
            RunTimerTransferArrival();
            RunTimerCloseApproachesCars();
            //TODO: Добавить запуск других таймеров
            //...............

            // Update the service state to Running.
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            // Отправить сообщение
            string mes_service_start = String.Format("Сервис : {0} - запущен. Интервал выполнения", this.servece_name);
            mes_service_start += String.Format(" сервиса {0}-{1} сек.,", this.thread_host, this.interval_transfer_host);
            mes_service_start += String.Format(" сервиса {0}-{1} сек.,", this.thread_approaches, this.interval_transfer_approaches);
            mes_service_start += String.Format(" сервиса {0}-{1} сек.,", this.thread_arrival, this.interval_transfer_arrival);
            mes_service_start += String.Format(" сервиса {0}-{1} сек.,", this.thread_close_approaches, this.interval_close_approaches);  
            mes_service_start.WriteEvents(EventStatus.Ok, servece_name, eventID);
            // лог запуска
            this.thread_host.WriteLogStatusServices();
            this.thread_approaches.WriteLogStatusServices();
            this.thread_arrival.WriteLogStatusServices();
            this.thread_close_approaches.WriteLogStatusServices();
        }

        protected override void OnStop()
        {
            // Добавить остановку других таймеров
            //...............
            // Отправить сообщение
            string mes_service_stop = String.Format("Сервис : {0} - остановлен.", this.servece_name);
            mes_service_stop.WriteEvents(EventStatus.Ok, servece_name, eventID);

        }

        public void OnTimerServices(object sender, System.Timers.ElapsedEventArgs args)
        {
            //String.Format("Сервис : {0} сработал таймер OnTimerServices контроля сервиса.", this.servece_name).WriteInformation(servece_name, eventID);            
            try
            {
                // TransferHost
                bool active_host = RWSetting.GetDB_Config_DefaultSetting("ActiveTransferHost", this.thread_host, this.active_transfer_host, true);
                int interval_host = RWSetting.GetDB_Config_DefaultSetting("IntervalTransferHost", this.thread_host, this.interval_transfer_host, true);
                if (active_host & interval_host != this.interval_transfer_host)
                {
                    this.interval_transfer_host = interval_host;
                    timer_services_host.Stop();
                    timer_services_host.Interval = this.interval_transfer_host * 1000;
                    RunTimerTransferHost();
                    string mes_service_start = String.Format("Сервис : {0}, интервал выполнения потока {1} изменен на {2} сек.", this.servece_name, this.thread_host, this.interval_transfer_host);
                    mes_service_start.WriteEvents(EventStatus.Ok, servece_name, eventID);
                }   
             
                // TransferApproaches
                bool active_app = RWSetting.GetDB_Config_DefaultSetting("ActiveTransferApproaches", this.thread_approaches, this.active_transfer_approaches, true);
                int interval_app = RWSetting.GetDB_Config_DefaultSetting("IntervalTransferApproaches", this.thread_approaches, this.interval_transfer_approaches, true);
                if (active_app & interval_app != this.interval_transfer_approaches)
                {
                    this.interval_transfer_approaches = interval_app;
                    timer_services_approaches.Stop();
                    timer_services_approaches.Interval = this.interval_transfer_approaches * 1000;
                    RunTimerTransferApproaches();
                    string mes_service_start = String.Format("Сервис : {0}, интервал выполнения потока {1} изменен на {2} сек.", this.servece_name, this.thread_approaches, this.interval_transfer_approaches);
                    mes_service_start.WriteEvents(EventStatus.Ok, servece_name, eventID);
                }
                // TransferArrival
                bool active_arr = RWSetting.GetDB_Config_DefaultSetting("ActiveTransferArrival", this.thread_arrival, this.active_transfer_arrival, true);
                int interval_arr = RWSetting.GetDB_Config_DefaultSetting("IntervalTransferArrival", this.thread_arrival, this.interval_transfer_arrival, true);
                if (active_arr & interval_arr != this.interval_transfer_arrival)
                {
                    this.interval_transfer_arrival = interval_arr;
                    timer_services_arrival.Stop();
                    timer_services_arrival.Interval = this.interval_transfer_arrival * 1000;
                    RunTimerTransferArrival();
                    string mes_service_start = String.Format("Сервис : {0}, интервал выполнения потока {1} изменен на {2} сек.", this.servece_name, this.thread_arrival, this.interval_transfer_arrival);
                    mes_service_start.WriteEvents(EventStatus.Ok, servece_name, eventID);
                }
                // CloseApproachesCars
                bool active_close = RWSetting.GetDB_Config_DefaultSetting("ActiveCloseApproachesCars", this.thread_close_approaches, this.active_close_approaches, true);
                int interval_close = RWSetting.GetDB_Config_DefaultSetting("IntervalCloseApproachesCars", this.thread_close_approaches, this.interval_close_approaches, true);
                if (active_close & interval_close != this.interval_close_approaches)
                {
                    this.interval_close_approaches = interval_close;
                    timer_services_close_approaches.Stop();
                    timer_services_close_approaches.Interval = this.interval_close_approaches * 1000;
                    RunTimerCloseApproachesCars();
                    string mes_service_start = String.Format("Сервис : {0}, интервал выполнения потока {1} изменен на {2} сек.", this.servece_name, this.thread_close_approaches, this.interval_close_approaches);
                    mes_service_start.WriteEvents(EventStatus.Ok, servece_name, eventID);
                }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("OnTimerServices(sender={0}, args={1})", sender, args.ToString()), this.servece_name, eventID);
            }
        }
        #endregion

        #region TransferHost
        protected void StartTransferHost(bool active)
        {
            if (active)
            {
                mtt.StartTransferHost();
            }
        }

        protected void StartTransferHost()
        {
            bool active_app = RWSetting.GetDB_Config_DefaultSetting("ActiveTransferHost", this.thread_host, this.active_transfer_host, true);
            StartTransferHost(active_app);
        }

        protected void RunTimerTransferHost()
        {
            StartTransferHost();
            timer_services_host.Start();
        }

        private void OnTimerServicesHost(object sender, System.Timers.ElapsedEventArgs args)
        {
            //String.Format("Сервис : {0} сработал таймер OnTimerServicesApproaches.", this.servece_name).WriteInformation(servece_name, eventID);
            try
            {
                bool active_app = RWSetting.GetDB_Config_DefaultSetting("ActiveTransferHost", this.thread_host, this.active_transfer_host, true);
                StartTransferHost(active_app);
                if (active_app != this.active_transfer_host) {
                    this.active_transfer_host = active_app;
                    string mes_service_start = String.Format("Сервис : {0}, выполнение потока {1} - {2}", this.servece_name, this.thread_host, active_app ? "возабновленно" : "остановленно");
                    mes_service_start.WriteEvents(EventStatus.Ok, servece_name, eventID);
                }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("OnTimerServicesHost(sender={0}, args={1})", sender, args.ToString()), this.servece_name, eventID);
            }
        }
        #endregion

        #region TransferApproaches
        protected void StartTransferApproaches(bool active)
        {
            if (active)
            {
                mtt.StartTransferApproaches();
            }
        }

        protected void StartTransferApproaches()
        {
            bool active_app = RWSetting.GetDB_Config_DefaultSetting("ActiveTransferApproaches", this.thread_approaches, this.active_transfer_approaches, true);
            StartTransferApproaches(active_app);
        }

        protected void RunTimerTransferApproaches()
        {
            StartTransferApproaches();
            timer_services_approaches.Start();
        }

        private void OnTimerServicesApproaches(object sender, System.Timers.ElapsedEventArgs args)
        {
            //String.Format("Сервис : {0} сработал таймер OnTimerServicesApproaches.", this.servece_name).WriteInformation(servece_name, eventID);
            try
            {
                bool active_app = RWSetting.GetDB_Config_DefaultSetting("ActiveTransferApproaches", this.thread_approaches, this.active_transfer_approaches, true);
                StartTransferApproaches(active_app);
                if (active_app != this.active_transfer_approaches) {
                    this.active_transfer_approaches = active_app;
                    string mes_service_start = String.Format("Сервис : {0}, выполнение потока {1} - {2}", this.servece_name, this.thread_approaches, active_app ? "возабновленно":"остановленно");
                    mes_service_start.WriteEvents(EventStatus.Ok, servece_name, eventID);
                }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("OnTimerServicesApproaches(sender={0}, args={1})", sender, args.ToString()), this.servece_name, eventID);
            }
        }
        #endregion

        #region TransferArrival
        protected void StartTransferArrival(bool active)
        {
            if (active)
            {
                mtt.StartTransferArrival();
            }
        }

        protected void StartTransferArrival()
        {
            bool active_arr = RWSetting.GetDB_Config_DefaultSetting("ActiveTransferArrival", this.thread_arrival, this.active_transfer_arrival, true);
            StartTransferArrival(active_arr);
        }

        protected void RunTimerTransferArrival()
        {
            StartTransferArrival();
            timer_services_arrival.Start();
        }

        private void OnTimerServicesArrival(object sender, System.Timers.ElapsedEventArgs args)
        {
            //String.Format("Сервис : {0} сработал таймер OnTimerServicesArrival.", this.servece_name).WriteInformation(servece_name, eventID);
            try
            {
                bool active_arr = RWSetting.GetDB_Config_DefaultSetting("ActiveTransferArrival", this.thread_arrival, this.active_transfer_arrival, true);
                StartTransferArrival(active_arr);
                if (active_arr != this.active_transfer_arrival)
                {
                    this.active_transfer_arrival = active_arr;
                    string mes_service_start = String.Format("Сервис : {0}, выполнение потока {1} - {2}", this.servece_name, this.thread_arrival, active_arr ? "возабновленно" : "остановленно");
                    mes_service_start.WriteEvents(EventStatus.Ok, servece_name, eventID);
                }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("OnTimerServicesArrival(sender={0}, args={1})", sender, args.ToString()), this.servece_name, eventID);
            }
        }
        #endregion

        #region CloseApproachesCars
        protected void StartCloseApproachesCars(bool active)
        {
            if (active)
            {
                mtt.StartCloseApproachesCars();
            }
        }

        protected void StartCloseApproachesCars()
        {
            bool active_close = RWSetting.GetDB_Config_DefaultSetting("ActiveCloseApproachesCars", this.thread_close_approaches, this.active_close_approaches, true);
            StartCloseApproachesCars(active_close);
        }

        protected void RunTimerCloseApproachesCars()
        {
            StartCloseApproachesCars();
            timer_services_close_approaches.Start();
        }

        private void OnTimerServicesCloseApproachesCars(object sender, System.Timers.ElapsedEventArgs args)
        {
            //String.Format("Сервис : {0} сработал таймер OnTimerServicesArrival.", this.servece_name).WriteInformation(servece_name, eventID);
            try
            {
                bool active_close = RWSetting.GetDB_Config_DefaultSetting("ActiveCloseApproachesCars", this.thread_close_approaches, this.active_close_approaches, true);
                StartCloseApproachesCars(active_close);
                if (active_close != this.active_close_approaches)
                {
                    this.active_close_approaches = active_close;
                    string mes_service_start = String.Format("Сервис : {0}, выполнение потока {1} - {2}", this.servece_name, this.thread_close_approaches, active_close ? "возабновленно" : "остановленно");
                    mes_service_start.WriteEvents(EventStatus.Ok, servece_name, eventID);
                }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("OnTimerServicesCloseApproachesCars(sender={0}, args={1})", sender, args.ToString()), this.servece_name, eventID);
            }
        }
        #endregion


    }
}
