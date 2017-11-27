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
        private service thread_approaches = service.TransferApproaches;
        private service thread_arrival = service.TransferArrival;
        private service thread_close = service.CloseTransfer;

        private int interval_transfer_approaches = 300; // секунд
        private int interval_transfer_arrival = 300; // секунд
        private int interval_close_transfer = 60; // секунд

        System.Timers.Timer timer_services = new System.Timers.Timer();
        System.Timers.Timer timer_TransferApproaches = new System.Timers.Timer();
        System.Timers.Timer timer_TransferArrival = new System.Timers.Timer();
        System.Timers.Timer timer_CloseTransfer = new System.Timers.Timer();

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

        protected override void OnStart(string[] args)
        {
            // Update the service state to Start Pending.
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            // Запустить таймер контроля сервиса
            timer_services.Start();
            timer_TransferApproaches.Start();
            timer_TransferArrival.Start();
            timer_CloseTransfer.Start();
            //TODO: Добавить запуск других таймеров
            //...............

            // Update the service state to Running.
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            // Отправить сообщение
            string mes_service_start = String.Format("Основной сервис {0} - запущен. Интервал выполнения сервиса {1}-{2} сек, сервиса {3}-{4} сек., сервиса {5}-{6} сек.", 
                this.servece_name, this.thread_approaches, this.interval_transfer_approaches, this.thread_arrival, this.interval_transfer_arrival, this.thread_close, this.interval_close_transfer);
            mes_service_start.WriteWarning(servece_name, this.eventID);
            mes_service_start.WriteEvents(EventStatus.Ok, servece_name, eventID);
            this.thread_approaches.WriteLogStatusServices();
            this.thread_arrival.WriteLogStatusServices();
            this.thread_close.WriteLogStatusServices();
        }

        protected override void OnStop()
        {
            timer_services.Stop();
            timer_TransferApproaches.Stop();
            timer_TransferArrival.Stop();
            timer_CloseTransfer.Stop();
            //TODO: Добавить остановку других таймеров
            //...............
            // Отправить сообщение
            string mes_service_stop = String.Format("Основной сервис {0} - остановлен.", this.servece_name);
            mes_service_stop.WriteWarning(servece_name, this.eventID);
            mes_service_stop.WriteEvents(EventStatus.Ok, servece_name, eventID);

        }

        public void OnTimerServices(object sender, System.Timers.ElapsedEventArgs args)
        {
            try
            {
                // Проверка изменения интервала выполнения
                int interval_app = RWSetting.GetDB_Config_DefaultSetting("IntervalTransferApproaches", this.thread_approaches, this.interval_transfer_approaches, true);
                if (interval_app != this.interval_transfer_approaches) {
                    this.interval_transfer_approaches = interval_app;
                    timer_TransferApproaches.Interval = interval_app*1000;
                    string mes_service_start = String.Format("Основной сервис {0} - интервал выполнения сервиса {1} изменен на {2} сек.", this.servece_name, this.thread_approaches, timer_TransferApproaches.Interval / 1000);
                    mes_service_start.WriteWarning(servece_name, this.eventID);
                    mes_service_start.WriteEvents(EventStatus.Ok, servece_name, eventID);
                }
                int interval_arr = RWSetting.GetDB_Config_DefaultSetting("IntervalTransferArrival", this.thread_arrival, this.interval_transfer_arrival, true);
                if (interval_arr != this.interval_transfer_arrival)
                {
                    this.interval_transfer_arrival = interval_arr;
                    timer_TransferArrival.Interval = interval_arr*1000;
                    string mes_service_start = String.Format("Основной сервис {0} - интервал выполнения сервиса {1} изменен на {2} сек.", this.servece_name, this.thread_arrival, timer_TransferArrival.Interval / 1000);
                    mes_service_start.WriteWarning(servece_name, this.eventID);
                    mes_service_start.WriteEvents(EventStatus.Ok, servece_name, eventID);
                }

                int interval_cls = RWSetting.GetDB_Config_DefaultSetting("IntervalCloseTransfer", this.thread_close, this.interval_close_transfer, true);
                if (interval_cls != this.interval_close_transfer)
                {
                    this.interval_close_transfer = interval_cls;
                    timer_CloseTransfer.Interval = interval_cls * 1000;
                    string mes_service_start = String.Format("Основной сервис {0} - интервал выполнения сервиса {1} изменен на {2} сек.", this.servece_name, this.thread_close, timer_CloseTransfer.Interval / 1000);
                    mes_service_start.WriteWarning(servece_name, this.eventID);
                    mes_service_start.WriteEvents(EventStatus.Ok, servece_name, eventID);                    
                }

            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("OnTimerServices(sender={0}, args={1})", sender, args.ToString()), this.servece_name, eventID);
            }
        }
        /// <summary>
        /// Запущено TransferApproaches
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void OnTimerTransferApproaches(object sender, System.Timers.ElapsedEventArgs args)
        {
            try
            {
                // Проверка активности выполнения
                bool active = RWSetting.GetDB_Config_DefaultSetting("ActiveTransferApproaches", this.thread_approaches, true, true);
                if (active)
                {
                    mtt.StartTransferApproaches();
                }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("OnTimerTransferApproaches(sender={0}, args={1})", sender, args.ToString()), this.servece_name, eventID);
            }
        }
        /// <summary>
        /// Запущено TransferArrival
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void OnTimerTransferArrival(object sender, System.Timers.ElapsedEventArgs args)
        {
            try
            {
                // Проверка активности выполнения
                bool active = RWSetting.GetDB_Config_DefaultSetting("ActiveTransferArrival", this.thread_arrival, true, true);
                if (active)
                {
                    mtt.StartTransferArrival();
                }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("OnTimerTransferArrival(sender={0}, args={1})", sender, args.ToString()), this.servece_name, eventID);
            }
        }

        public void OnTimerCloseTransfer(object sender, System.Timers.ElapsedEventArgs args)
        {
            try
            {
                // Проверка активности выполнения
                bool active = RWSetting.GetDB_Config_DefaultSetting("ActiveCloseTransfer", this.thread_close, true, true);
                if (active)
                {
                    //mtt.StartCloseTransfer();
                }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("OnTimerCloseTransfer(sender={0}, args={1})", sender, args.ToString()), this.servece_name, eventID);
            }
        }
        /// <summary>
        /// Инициализация сервиса (проверка данных в БД и создание settings)
        /// </summary>
        public void InitializeService() 
        {
            try
            {
                // Настроем таймер контроля выполнения сервиса
                timer_services.Interval = 1000;
                timer_services.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimerServices);

                this.interval_transfer_approaches = RWSetting.GetDB_Config_DefaultSetting("IntervalTransferApproaches", this.thread_approaches, this.interval_transfer_approaches, true); // Интервал в секундах
                timer_TransferApproaches.Interval = this.interval_transfer_approaches*1000;
                timer_TransferApproaches.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimerTransferApproaches);

                this.interval_transfer_arrival = RWSetting.GetDB_Config_DefaultSetting("IntervalTransferArrival", this.thread_arrival, this.interval_transfer_arrival, true); // Интервал в секундах
                timer_TransferArrival.Interval = this.interval_transfer_arrival * 1000;
                timer_TransferArrival.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimerTransferArrival);

                this.interval_close_transfer = RWSetting.GetDB_Config_DefaultSetting("IntervalCloseTransfer", this.thread_close, this.interval_close_transfer, true); // Интервал в секундах
                timer_CloseTransfer.Interval = this.interval_close_transfer * 1000;
                timer_CloseTransfer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimerCloseTransfer);

                //TODO: Добавить инициализацию других таймеров
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
