using KIS;
using MessageLog;
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

namespace KISTServices
{
    public partial class KISTServices : ServiceBase
    {
        private eventID eventID = eventID.KISTServices;
        private service servece_name = service.ServicesKIS;
        private service thread_copy_arrival = service.CopyArrivalSostavKIS;
        private service thread_arrival = service.TransferArrivalKIS;
        private service thread_close = service.CloseTransfer;

        private int interval_copy_arrival = 60; // секунд
        private int interval_transfer_arrival = 300; // секунд
        private int interval_close_transfer = 60; // секунд

        bool active_copy_arrival = true;
        bool active_transfer_arrival = true;

        System.Timers.Timer timer_services = new System.Timers.Timer();
        System.Timers.Timer timer_services_copy_arrival = new System.Timers.Timer();
        System.Timers.Timer timer_services_arrival = new System.Timers.Timer();

        private KISThread kist = new KISThread(service.ServicesKIS);

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

        public KISTServices(string[] args)
        {
            InitializeComponent();
            InitializeService();
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
            RunTimerCopyArrival();
            RunTimerTransferArrivalKIS();
            //TODO: Добавить запуск других таймеров
            //...............

            // Update the service state to Running.
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            // Отправить сообщение
            string mes_service_start = String.Format("Сервис : {0} - запущен. Интервал выполнения сервиса {1}-{2} сек, сервиса {3}-{4} сек., сервиса {5}-{6} сек.", 
            this.servece_name, this.thread_copy_arrival, this.interval_copy_arrival, this.thread_arrival, this.interval_transfer_arrival, this.thread_close, this.interval_close_transfer);
            mes_service_start.WriteEvents(EventStatus.Ok, servece_name, eventID);
            this.thread_copy_arrival.WriteLogStatusServices();
            this.thread_arrival.WriteLogStatusServices();
            this.thread_close.WriteLogStatusServices();
        }

        protected void RunTimerCopyArrival()
        {
            bool active_app = RWSetting.GetDB_Config_DefaultSetting("ActiveCopyBufferArrivalSostav", this.thread_copy_arrival, this.active_copy_arrival, true);
            if (active_app)
            {
                kist.StartCopyBufferArrivalSostav();
                timer_services_copy_arrival.Start();
            }
            timer_services_copy_arrival.Start();
        }

        protected void RunTimerTransferArrivalKIS()
        {
            bool active_arr = RWSetting.GetDB_Config_DefaultSetting("ActiveTransferArrivalKIS", this.thread_arrival, this.active_transfer_arrival, true);
            if (active_arr)
            {
                kist.StartTransferArrivalOfKIS();
                timer_services_arrival.Start();
            }
            timer_services_arrival.Start();
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
            //String.Format("Сервис : {0} сработал таймер контроля сервиса.", this.servece_name).WriteInformation(servece_name, eventID);            
            try
            {
                // CopyArrivalSostavKIS
                bool active_copy = RWSetting.GetDB_Config_DefaultSetting("ActiveCopyArrivalSostavKIS", this.thread_copy_arrival, this.active_copy_arrival, true);
                int interval_copy = RWSetting.GetDB_Config_DefaultSetting("IntervalCopyArrivalSostavKIS", this.thread_copy_arrival, this.interval_copy_arrival, true);
                if (active_copy & interval_copy != this.interval_copy_arrival)
                {
                    this.interval_copy_arrival = interval_copy;                    
                    timer_services_copy_arrival.Stop();
                    timer_services_copy_arrival.Interval = this.interval_copy_arrival * 1000;
                    RunTimerCopyArrival();
                    string mes_service_start = String.Format("Сервис : {0}, интервал выполнения потока {1} изменен на {2} сек.", this.servece_name, this.thread_copy_arrival, this.interval_copy_arrival);
                    mes_service_start.WriteEvents(EventStatus.Ok, servece_name, eventID);
                }
                // TransferArrivalKIS
                bool active_arr = RWSetting.GetDB_Config_DefaultSetting("ActiveTransferArrivalKIS", this.thread_arrival, this.active_transfer_arrival, true);
                int interval_arr = RWSetting.GetDB_Config_DefaultSetting("IntervalTransferArrivalKIS", this.thread_arrival, this.interval_transfer_arrival, true);
                if (active_arr & interval_arr != this.interval_transfer_arrival)
                {
                    this.interval_transfer_arrival = interval_arr;
                    timer_services_arrival.Stop();
                    timer_services_arrival.Interval = this.interval_transfer_arrival * 1000;
                    RunTimerTransferArrivalKIS();
                    string mes_service_start = String.Format("Сервис : {0}, интервал выполнения потока {1} изменен на {2} сек.", this.servece_name, this.thread_arrival, this.interval_transfer_arrival);
                    mes_service_start.WriteEvents(EventStatus.Ok, servece_name, eventID);
                }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("OnTimerServices(sender={0}, args={1})", sender, args.ToString()), this.servece_name, eventID);
            }
        }

        private void OnTimerServicesCopyBufferArrivalSostav(object sender, System.Timers.ElapsedEventArgs args)
        {
            //String.Format("Сервис : {0} сработал таймер Approaches.", this.servece_name).WriteInformation(servece_name, eventID);
            try
            {
                bool active_copy = RWSetting.GetDB_Config_DefaultSetting("ActiveCopyArrivalSostavKIS", this.thread_copy_arrival, this.active_copy_arrival, true);
                if (active_copy)
                {
                    kist.StartCopyBufferArrivalSostav();
                }
                if (active_copy != this.active_copy_arrival) {
                    this.active_copy_arrival = active_copy;
                    string mes_service_start = String.Format("Сервис : {0}, выполнение потока {1} - {2}", this.servece_name, this.thread_copy_arrival, active_copy ? "возабновленно":"остановленно");
                    mes_service_start.WriteEvents(EventStatus.Ok, servece_name, eventID);
                }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("OnTimerServicesCopyBufferArrivalSostav(sender={0}, args={1})", sender, args.ToString()), this.servece_name, eventID);
            }
        }

        private void OnTimerServicesTransferArrivalKIS(object sender, System.Timers.ElapsedEventArgs args)
        {
            //String.Format("Сервис : {0} сработал таймер Approaches.", this.servece_name).WriteInformation(servece_name, eventID);
            try
            {
                bool active_arr = RWSetting.GetDB_Config_DefaultSetting("ActiveTransferArrivalKIS", this.thread_arrival, this.active_transfer_arrival, true);
                if (active_arr)
                {
                    kist.StartTransferArrivalOfKIS();
                }
                if (active_arr != this.active_transfer_arrival)
                {
                    this.active_transfer_arrival = active_arr;
                    string mes_service_start = String.Format("Сервис : {0}, выполнение потока {1} - {2}", this.servece_name, this.thread_arrival, active_arr ? "возабновленно" : "остановленно");
                    mes_service_start.WriteEvents(EventStatus.Ok, servece_name, eventID);
                }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("OnTimerServicesTransferArrivalKIS(sender={0}, args={1})", sender, args.ToString()), this.servece_name, eventID);
            }
        }
        /// <summary>
        /// Инициализация сервиса (проверка данных в БД и создание settings)
        /// </summary>
        public void InitializeService() 
        {
            try
            {
                // интервалы
                this.interval_copy_arrival = RWSetting.GetDB_Config_DefaultSetting("IntervalCopyArrivalSostavKIS", this.thread_copy_arrival, this.interval_copy_arrival, true);
                this.interval_transfer_arrival = RWSetting.GetDB_Config_DefaultSetting("IntervalTransferArrivalKIS", this.thread_arrival, this.interval_transfer_arrival, true);
                this.interval_close_transfer = RWSetting.GetDB_Config_DefaultSetting("IntervalCloseTransfer", this.thread_close, this.interval_close_transfer, true);
                // состояние активности
                this.active_copy_arrival = RWSetting.GetDB_Config_DefaultSetting("ActiveCopyArrivalSostavKIS", this.thread_copy_arrival, this.active_copy_arrival, true);
                this.active_transfer_arrival = RWSetting.GetDB_Config_DefaultSetting("ActiveTransferArrivalKIS", this.thread_arrival, this.active_transfer_arrival, true);
                
                //// Настроем таймер контроля выполнения сервиса
                timer_services.Interval = 30000;
                timer_services.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimerServices);

                timer_services_copy_arrival.Interval = this.interval_copy_arrival * 1000;
                timer_services_copy_arrival.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimerServicesCopyBufferArrivalSostav);

                timer_services_arrival.Interval = this.interval_transfer_arrival * 1000;
                timer_services_arrival.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimerServicesTransferArrivalKIS);

                //Добавить инициализацию других таймеров
                //...............
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("InitializeService()"), this.servece_name, eventID);
                return;
            } 
        }




    }
}
