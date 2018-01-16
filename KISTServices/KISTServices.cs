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
        private service thread_close_arrival = service.CloseArrivalSostavKIS;

        private service thread_copy_input = service.CopyInputSostavKIS;
        private service thread_input = service.TransferInputKIS;

        private service thread_copy_output = service.CopyOutputSostavKIS;
        private service thread_output = service.TransferOutputKIS;

        private int interval_copy_arrival = 60; // секунд
        private int interval_transfer_arrival = 300; // секунд
        private int interval_close_arrival = 3600; // секунд

        private int interval_copy_input = 60; // секунд
        private int interval_input = 120; // секунд
        private int interval_copy_output = 60; // секунд
        private int interval_output = 120; // секунд

        bool active_copy_arrival = true;
        bool active_transfer_arrival = true;
        bool active_close_arrival = true;

        bool active_copy_input = true;
        bool active_input = true;
        bool active_copy_output = true;
        bool active_output = true;

        System.Timers.Timer timer_services = new System.Timers.Timer();
        System.Timers.Timer timer_services_copy_arrival = new System.Timers.Timer();
        System.Timers.Timer timer_services_arrival = new System.Timers.Timer();
        System.Timers.Timer timer_services_close_arrival = new System.Timers.Timer();

        System.Timers.Timer timer_services_copy_input = new System.Timers.Timer();
        System.Timers.Timer timer_services_input = new System.Timers.Timer();
        System.Timers.Timer timer_services_copy_output = new System.Timers.Timer();
        System.Timers.Timer timer_services_output = new System.Timers.Timer();

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

        #region Управление службой
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
                this.interval_close_arrival = RWSetting.GetDB_Config_DefaultSetting("IntervalCloseArrivalSostavKIS", this.thread_close_arrival, this.interval_close_arrival, true);

                this.interval_copy_input = RWSetting.GetDB_Config_DefaultSetting("IntervalCopyInputSostavKIS", this.thread_copy_input, this.interval_copy_input, true);
                this.interval_input = RWSetting.GetDB_Config_DefaultSetting("IntervalTransferInputKIS", this.thread_input, this.interval_input, true);
                this.interval_copy_output = RWSetting.GetDB_Config_DefaultSetting("IntervalCopyOutputSostavKIS", this.thread_copy_output, this.interval_copy_output, true);

                // состояние активности
                this.active_copy_arrival = RWSetting.GetDB_Config_DefaultSetting("ActiveCopyArrivalSostavKIS", this.thread_copy_arrival, this.active_copy_arrival, true);
                this.active_transfer_arrival = RWSetting.GetDB_Config_DefaultSetting("ActiveTransferArrivalKIS", this.thread_arrival, this.active_transfer_arrival, true);
                this.active_close_arrival = RWSetting.GetDB_Config_DefaultSetting("ActiveCloseArrivalSostavKIS", this.thread_close_arrival, this.active_close_arrival, true);

                this.active_copy_input = RWSetting.GetDB_Config_DefaultSetting("ActiveCopyInputSostavKIS", this.thread_copy_input, this.active_copy_input, true);
                this.active_input = RWSetting.GetDB_Config_DefaultSetting("ActiveTransferInputKIS", this.thread_input, this.active_input, true);
                this.active_copy_output = RWSetting.GetDB_Config_DefaultSetting("ActiveCopyOutputSostavKIS", this.thread_copy_output, this.active_copy_output, true);
                
                //// Настроем таймер контроля выполнения сервиса
                timer_services.Interval = 30000;
                timer_services.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimerServices);

                timer_services_copy_arrival.Interval = this.interval_copy_arrival * 1000;
                timer_services_copy_arrival.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimerServicesCopyBufferArrivalSostav);

                timer_services_arrival.Interval = this.interval_transfer_arrival * 1000;
                timer_services_arrival.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimerServicesTransferArrivalKIS);

                timer_services_close_arrival.Interval = this.interval_close_arrival * 1000;
                timer_services_close_arrival.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimerServicesCloseBufferArrivalSostav);

                timer_services_copy_input.Interval = this.interval_copy_input * 1000;
                timer_services_copy_input.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimerServicesCopyBufferInputSostav);

                timer_services_input.Interval = this.interval_input * 1000;
                timer_services_input.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimerServicesTransferInputKIS);

                timer_services_copy_output.Interval = this.interval_copy_output * 1000;
                timer_services_copy_output.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimerServicesCopyBufferOutputSostav);

                timer_services_output.Interval = this.interval_output * 1000;
                timer_services_output.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimerServicesTransferOutputKIS);

                //Добавить инициализацию других таймеров
                //...............
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("InitializeService()"), this.servece_name, eventID);
                return;
            } 
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
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
            RunTimerCloseBufferArrivalSostav();

            RunTimerCopyInput();
            RunTimerTransferInputKIS();
            RunTimerCopyOutput();
            RunTimerTransferOutputKIS();
            //TODO: Добавить запуск других таймеров
            //...............

            // Update the service state to Running.
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            // Отправить сообщение
            string mes_service_start = String.Format("Сервис : {0} - запущен. Интервал выполнения сервиса {1}-{2} сек, сервиса {3}-{4} сек, сервиса {5}-{6} сек, сервиса {7}-{8} сек, сервиса {9}-{10} сек, сервиса {11}-{12} сек, сервиса {13}-{14} сек.",
            this.servece_name, 
            this.thread_copy_arrival, this.interval_copy_arrival, 
            this.thread_arrival, this.interval_transfer_arrival, 
            this.thread_close_arrival, this.interval_close_arrival,
            this.thread_copy_input, this.interval_copy_input,
            this.thread_input, this.interval_input,
            this.thread_copy_output, this.interval_copy_output,
            this.thread_output, this.interval_output
            );
            mes_service_start.WriteEvents(EventStatus.Ok, servece_name, eventID);
            this.thread_copy_arrival.WriteLogStatusServices();
            this.thread_arrival.WriteLogStatusServices();
            this.thread_close_arrival.WriteLogStatusServices();

            this.thread_copy_input.WriteLogStatusServices();
            this.thread_input.WriteLogStatusServices();
            this.thread_copy_output.WriteLogStatusServices();
            this.thread_output.WriteLogStatusServices();
        }
        /// <summary>
        /// 
        /// </summary>
        protected override void OnStop()
        {
            // Добавить остановку других таймеров
            //...............
            // Отправить сообщение
            string mes_service_stop = String.Format("Сервис : {0} - остановлен.", this.servece_name);
            mes_service_stop.WriteEvents(EventStatus.Ok, servece_name, eventID);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void OnTimerServices(object sender, System.Timers.ElapsedEventArgs args)
        {
            //String.Format("Сервис : {0} сработал таймер контроля сервиса.", this.servece_name).WriteInformation(servece_name, eventID);            
            try
            {
                // CopyArrivalSostavKIS
                RestartServices("ActiveCopyArrivalSostavKIS", "IntervalCopyArrivalSostavKIS", this.thread_copy_arrival, this.active_copy_arrival, this.interval_copy_arrival, timer_services_copy_arrival);
                //bool active_acopy = RWSetting.GetDB_Config_DefaultSetting("ActiveCopyArrivalSostavKIS", this.thread_copy_arrival, this.active_copy_arrival, true);
                //int interval_acopy = RWSetting.GetDB_Config_DefaultSetting("IntervalCopyArrivalSostavKIS", this.thread_copy_arrival, this.interval_copy_arrival, true);
                //if (active_acopy & interval_acopy != this.interval_copy_arrival)
                //{
                //    this.interval_copy_arrival = interval_acopy;                    
                //    timer_services_copy_arrival.Stop();
                //    timer_services_copy_arrival.Interval = this.interval_copy_arrival * 1000;
                //    RunTimerCopyArrival();
                //    string mes_service_start = String.Format("Сервис : {0}, интервал выполнения потока {1} изменен на {2} сек.", this.servece_name, this.thread_copy_arrival, this.interval_copy_arrival);
                //    mes_service_start.WriteEvents(EventStatus.Ok, servece_name, eventID);
                //}
                // TransferArrivalKIS
                RestartServices("ActiveTransferArrivalKIS", "IntervalTransferArrivalKIS", this.thread_arrival, this.active_transfer_arrival, this.interval_transfer_arrival, timer_services_arrival);
                //bool active_arr = RWSetting.GetDB_Config_DefaultSetting("ActiveTransferArrivalKIS", this.thread_arrival, this.active_transfer_arrival, true);
                //int interval_arr = RWSetting.GetDB_Config_DefaultSetting("IntervalTransferArrivalKIS", this.thread_arrival, this.interval_transfer_arrival, true);
                //if (active_arr & interval_arr != this.interval_transfer_arrival)
                //{
                //    this.interval_transfer_arrival = interval_arr;
                //    timer_services_arrival.Stop();
                //    timer_services_arrival.Interval = this.interval_transfer_arrival * 1000;
                //    RunTimerTransferArrivalKIS();
                //    string mes_service_start = String.Format("Сервис : {0}, интервал выполнения потока {1} изменен на {2} сек.", this.servece_name, this.thread_arrival, this.interval_transfer_arrival);
                //    mes_service_start.WriteEvents(EventStatus.Ok, servece_name, eventID);
                //}

                // CloseBufferArrivalSostav
                RestartServices("ActiveCloseArrivalSostavKIS", "IntervalCloseArrivalSostavKIS", this.thread_close_arrival, this.active_close_arrival, this.interval_close_arrival, timer_services_close_arrival);
                //bool active_close = RWSetting.GetDB_Config_DefaultSetting("ActiveCloseArrivalSostavKIS", this.thread_close_arrival, this.active_close_arrival, true);
                //int interval_close = RWSetting.GetDB_Config_DefaultSetting("IntervalCloseArrivalSostavKIS", this.thread_close_arrival, this.interval_close_arrival, true);
                //if (active_close & interval_close != this.interval_close_arrival)
                //{
                //    this.interval_close_arrival = interval_close;
                //    timer_services_close_arrival.Stop();
                //    timer_services_close_arrival.Interval = this.interval_close_arrival * 1000;
                //    RunTimerCloseBufferArrivalSostav();
                //    string mes_service_start = String.Format("Сервис : {0}, интервал выполнения потока {1} изменен на {2} сек.", this.servece_name, this.thread_close_arrival, this.interval_close_arrival);
                //    mes_service_start.WriteEvents(EventStatus.Ok, servece_name, eventID);
                //}
                // CopyInputSostavKIS
                RestartServices("ActiveCopyInputSostavKIS", "IntervalCopyInputSostavKIS", this.thread_copy_input, this.active_copy_input, this.interval_copy_input, timer_services_copy_input);
                //bool active_icopy = RWSetting.GetDB_Config_DefaultSetting("ActiveCopyInputSostavKIS", this.thread_copy_input, this.active_copy_input, true);
                //int interval_icopy = RWSetting.GetDB_Config_DefaultSetting("IntervalCopyInputSostavKIS", this.thread_copy_input, this.interval_copy_input, true);
                //if (active_icopy & interval_icopy != this.interval_copy_input)
                //{
                //    this.interval_copy_input = interval_icopy;
                //    timer_services_copy_input.Stop();
                //    timer_services_copy_input.Interval = this.interval_copy_input * 1000;
                //    RunTimerCopyInput();
                //    string mes_service_start = String.Format("Сервис : {0}, интервал выполнения потока {1} изменен на {2} сек.", this.servece_name, this.thread_copy_input, this.interval_copy_input);
                //    mes_service_start.WriteEvents(EventStatus.Ok, servece_name, eventID);
                //}
                // TransferInputKIS
                RestartServices("ActiveTransferInputKIS", "IntervalTransferInputKIS", this.thread_input, this.active_input, this.interval_input, timer_services_input);
                // CopyOutputSostavKIS
                RestartServices("ActiveCopyOutputSostavKIS", "IntervalCopyOutputSostavKIS", this.thread_copy_output, this.active_copy_output, this.interval_copy_output, timer_services_copy_output);
                //bool active_ocopy = RWSetting.GetDB_Config_DefaultSetting("ActiveCopyOutputSostavKIS", this.thread_copy_output, this.active_copy_output, true);
                //int interval_ocopy = RWSetting.GetDB_Config_DefaultSetting("IntervalCopyOutputSostavKIS", this.thread_copy_output, this.interval_copy_output, true);
                //if (active_ocopy & interval_ocopy != this.interval_copy_output)
                //{
                //    this.interval_copy_output = interval_ocopy;
                //    timer_services_copy_output.Stop();
                //    timer_services_copy_output.Interval = this.interval_copy_output * 1000;
                //    RunTimerCopyOutput();
                //    string mes_service_start = String.Format("Сервис : {0}, интервал выполнения потока {1} изменен на {2} сек.", this.servece_name, this.thread_copy_output, this.interval_copy_output);
                //    mes_service_start.WriteEvents(EventStatus.Ok, servece_name, eventID);
                //}
                // TransferOutputKIS
                RestartServices("ActiveTransferOutputKIS", "IntervalTransferOutputKIS", this.thread_output, this.active_output, this.interval_output, timer_services_output);

            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("OnTimerServices(sender={0}, args={1})", sender, args.ToString()), this.servece_name, eventID);
            }
        }
        #endregion

        private void RestartServices(string active_name, string interval_name, service service, bool active, int interval, System.Timers.Timer timer)
        {

            bool active_tmp = RWSetting.GetDB_Config_DefaultSetting(active_name, service, active, true);
            int interval_tmp = RWSetting.GetDB_Config_DefaultSetting(interval_name, service, interval, true);
            if (active_tmp & interval_tmp != interval)
            {
                interval = interval_tmp;
                timer_services_copy_input.Stop();
                timer_services_copy_input.Interval = interval * 1000;
                switch (service){
                    case service.CopyArrivalSostavKIS: RunTimerCopyArrival(); break;
                    case service.TransferArrivalKIS: RunTimerTransferArrivalKIS(); break;
                    case service.CloseArrivalSostavKIS: RunTimerCloseBufferArrivalSostav(); break;
                    case service.CopyInputSostavKIS: RunTimerCopyInput(); break;
                    case service.TransferInputKIS: RunTimerTransferInputKIS(); break;
                    case service.CopyOutputSostavKIS: RunTimerCopyOutput(); break;
                    case service.TransferOutputKIS: RunTimerTransferOutputKIS(); break;
                }

                string mes_service_start = String.Format("Сервис : {0}, интервал выполнения потока {1} изменен на {2} сек.", this.servece_name, service, interval);
                mes_service_start.WriteEvents(EventStatus.Ok, servece_name, eventID);
            }
        }

        #region CopyArrival
        /// <summary>
        /// Запустить поток с учетом бита активности выполнения
        /// </summary>
        /// <param name="active"></param>
        protected void StartCopyArrival(bool active)
        {
            if (active)
            {
                kist.StartCopyBufferArrivalSostav();
            }
        }
        /// <summary>
        /// Запустить поток
        /// </summary>
        protected void StartCopyArrival()
        {
            bool active_app = RWSetting.GetDB_Config_DefaultSetting("ActiveCopyBufferArrivalSostav", this.thread_copy_arrival, this.active_copy_arrival, true);
            StartCopyArrival(active_app);
        }
        /// <summary>
        /// Первый запуск или перезапуск потока
        /// </summary>
        protected void RunTimerCopyArrival()
        {
            StartCopyArrival();
            timer_services_copy_arrival.Start();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnTimerServicesCopyBufferArrivalSostav(object sender, System.Timers.ElapsedEventArgs args)
        {
            //String.Format("Сервис : {0} сработал таймер Approaches.", this.servece_name).WriteInformation(servece_name, eventID);
            try
            {
                bool active_copy = RWSetting.GetDB_Config_DefaultSetting("ActiveCopyArrivalSostavKIS", this.thread_copy_arrival, this.active_copy_arrival, true);
                StartCopyArrival(active_copy);
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
        #endregion

        #region TransferArrivalKIS
        /// <summary>
        /// Запустить поток с учетом бита активности выполнения
        /// </summary>
        /// <param name="active"></param>
        protected void StartTransferArrivalKIS(bool active)
        {
            if (active)
            {
                // Проверка выполнения службы переноса из Хост
                int interval_host = RWSetting.GetDB_Config_DefaultSetting("IntervalTransferHost", service.TransferHost, 300, false);
                int? code_run_service = service.TransferHost.GetCodeReturnServices(interval_host + 60);
                if (code_run_service == null)
                {
                    String.Format("Поток {0} сервиса {1} - заблокирован, из-за остановки службы {2}", this.thread_arrival, this.servece_name, service.TransferHost).WriteError(servece_name, eventID);
                    service.TransferArrivalKIS.WriteServices(DateTime.Now, DateTime.Now, -2);
                    return;
                }
                if (code_run_service < 0)
                {
                    String.Format("Поток {0} сервиса {1} - заблокирован, из-за ошибки {2} последнего выполнения службы {3}", this.thread_arrival, this.servece_name, code_run_service, service.TransferHost).WriteError(servece_name, eventID);
                    service.TransferArrivalKIS.WriteServices(DateTime.Now, DateTime.Now, -2);                    
                    return;
                }
                kist.StartTransferArrivalOfKIS();
            }
        }        
        /// <summary>
        /// Запустить поток
        /// </summary>
        protected void StartTransferArrivalKIS()
        {
            bool active_arr = RWSetting.GetDB_Config_DefaultSetting("ActiveTransferArrivalKIS", this.thread_arrival, this.active_transfer_arrival, true);
            StartTransferArrivalKIS(active_arr);
        }
        /// <summary>
        /// Первый запуск или перезапуск потока
        /// </summary>
        protected void RunTimerTransferArrivalKIS()
        {
            StartTransferArrivalKIS();
            timer_services_arrival.Start();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnTimerServicesTransferArrivalKIS(object sender, System.Timers.ElapsedEventArgs args)
        {
            //String.Format("Сервис : {0} сработал таймер Approaches.", this.servece_name).WriteInformation(servece_name, eventID);
            try
            {
                bool active_arr = RWSetting.GetDB_Config_DefaultSetting("ActiveTransferArrivalKIS", this.thread_arrival, this.active_transfer_arrival, true);
                StartTransferArrivalKIS(active_arr);
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
        #endregion

        #region CloseBufferArrivalSostav
        /// <summary>
        /// Запустить поток с учетом бита активности выполнения
        /// </summary>
        /// <param name="active"></param>
        protected void StartCloseBufferArrivalSostav(bool active)
        {
            if (active)
            {
                kist.StartCloseBufferArrivalSostav();
            }
        }
        /// <summary>
        /// Запустить поток
        /// </summary>
        protected void StartCloseBufferArrivalSostav()
        {
            bool active_cls = RWSetting.GetDB_Config_DefaultSetting("ActiveCloseArrivalSostavKIS", this.thread_close_arrival, this.active_close_arrival, true);
            StartCloseBufferArrivalSostav(active_cls);
        }
        /// <summary>
        /// Первый запуск или перезапуск потока
        /// </summary>
        protected void RunTimerCloseBufferArrivalSostav()
        {
            StartCloseBufferArrivalSostav();
            timer_services_close_arrival.Start();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnTimerServicesCloseBufferArrivalSostav(object sender, System.Timers.ElapsedEventArgs args)
        {
            //String.Format("Сервис : {0} сработал таймер Approaches.", this.servece_name).WriteInformation(servece_name, eventID);
            try
            {
                bool active_close = RWSetting.GetDB_Config_DefaultSetting("ActiveCloseArrivalSostavKIS", this.thread_close_arrival, this.active_close_arrival, true);
                StartCloseBufferArrivalSostav(active_close);
                if (active_close != this.active_close_arrival) {
                    this.active_close_arrival = active_close;
                    string mes_service_start = String.Format("Сервис : {0}, выполнение потока {1} - {2}", this.servece_name, this.thread_close_arrival, active_close ? "возабновленно":"остановленно");
                    mes_service_start.WriteEvents(EventStatus.Ok, servece_name, eventID);
                }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("OnTimerServicesCopyBufferArrivalSostav(sender={0}, args={1})", sender, args.ToString()), this.servece_name, eventID);
            }
        }
        #endregion

        #region CopyInput
        /// <summary>
        /// Запустить поток с учетом бита активности выполнения
        /// </summary>
        /// <param name="active"></param>
        protected void StartCopyInput(bool active)
        {
            if (active)
            {
                kist.StartCopyBufferInputSostav();
            }
        }
        /// <summary>
        /// Запустить поток
        /// </summary>
        protected void StartCopyInput()
        {
            bool active_app = RWSetting.GetDB_Config_DefaultSetting("ActiveCopyInputSostavKIS", this.thread_copy_input, this.active_copy_input, true);
            StartCopyInput(active_app);
        }
        /// <summary>
        /// Первый запуск или перезапуск потока
        /// </summary>
        protected void RunTimerCopyInput()
        {
            StartCopyInput();
            timer_services_copy_input.Start();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnTimerServicesCopyBufferInputSostav(object sender, System.Timers.ElapsedEventArgs args)
        {
            //String.Format("Сервис : {0} сработал таймер Approaches.", this.servece_name).WriteInformation(servece_name, eventID);
            try
            {
                bool active_copy = RWSetting.GetDB_Config_DefaultSetting("ActiveCopyInputSostavKIS", this.thread_copy_input, this.active_copy_input, true);
                StartCopyInput(active_copy);
                if (active_copy != this.active_copy_input)
                {
                    this.active_copy_input = active_copy;
                    string mes_service_start = String.Format("Сервис : {0}, выполнение потока {1} - {2}", this.servece_name, this.thread_copy_input, active_copy ? "возабновленно" : "остановленно");
                    mes_service_start.WriteEvents(EventStatus.Ok, servece_name, eventID);
                }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("OnTimerServicesCopyBufferInputSostav(sender={0}, args={1})", sender, args.ToString()), this.servece_name, eventID);
            }
        }
        #endregion

        #region CopyOutput
        /// <summary>
        /// Запустить поток с учетом бита активности выполнения
        /// </summary>
        /// <param name="active"></param>
        protected void StartCopyOutput(bool active)
        {
            if (active)
            {
                kist.StartCopyBufferOutputSostav();
            }
        }
        /// <summary>
        /// Запустить поток
        /// </summary>
        protected void StartCopyOutput()
        {
            bool active_app = RWSetting.GetDB_Config_DefaultSetting("ActiveCopyOutputSostavKIS", this.thread_copy_output, this.active_copy_output, true);
            StartCopyOutput(active_app);
        }
        /// <summary>
        /// Первый запуск или перезапуск потока
        /// </summary>
        protected void RunTimerCopyOutput()
        {
            StartCopyOutput();
            timer_services_copy_output.Start();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnTimerServicesCopyBufferOutputSostav(object sender, System.Timers.ElapsedEventArgs args)
        {
            try
            {
                bool active_copy = RWSetting.GetDB_Config_DefaultSetting("ActiveCopyOutputSostavKIS", this.thread_copy_output, this.active_copy_output, true);
                StartCopyOutput(active_copy);
                if (active_copy != this.active_copy_output)
                {
                    this.active_copy_output = active_copy;
                    string mes_service_start = String.Format("Сервис : {0}, выполнение потока {1} - {2}", this.servece_name, this.thread_copy_output, active_copy ? "возабновленно" : "остановленно");
                    mes_service_start.WriteEvents(EventStatus.Ok, servece_name, eventID);
                }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("OnTimerServicesCopyBufferInputSostav(sender={0}, args={1})", sender, args.ToString()), this.servece_name, eventID);
            }
        }
        #endregion

        #region TransferInputKIS
        /// <summary>
        /// Запустить поток с учетом бита активности выполнения
        /// </summary>
        /// <param name="active"></param>
        protected void StartTransferInputKIS(bool active)
        {
            if (active)
            {
                kist.StartTransferInputKIS();
            }
        }
        /// <summary>
        /// Запустить поток
        /// </summary>
        protected void StartTransferInputKIS()
        {
            bool active_app = RWSetting.GetDB_Config_DefaultSetting("ActiveTransferInputKIS", this.thread_input, this.active_input, true);
            StartTransferInputKIS(active_app);
        }
        /// <summary>
        /// Первый запуск или перезапуск потока
        /// </summary>
        protected void RunTimerTransferInputKIS()
        {
            StartTransferInputKIS();
            timer_services_input.Start();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnTimerServicesTransferInputKIS(object sender, System.Timers.ElapsedEventArgs args)
        {
            //String.Format("Сервис : {0} сработал таймер Approaches.", this.servece_name).WriteInformation(servece_name, eventID);
            try
            {
                bool active_imp = RWSetting.GetDB_Config_DefaultSetting("ActiveTransferInputKIS", this.thread_input, this.active_input, true);
                StartTransferInputKIS(active_imp);
                if (active_imp != this.active_input)
                {
                    this.active_input = active_imp;
                    string mes_service_start = String.Format("Сервис : {0}, выполнение потока {1} - {2}", this.servece_name, this.thread_input, active_imp ? "возабновленно" : "остановленно");
                    mes_service_start.WriteEvents(EventStatus.Ok, servece_name, eventID);
                }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("OnTimerServicesTransferInputKIS(sender={0}, args={1})", sender, args.ToString()), this.servece_name, eventID);
            }
        }
        #endregion

        #region TransferOutputKIS
        /// <summary>
        /// Запустить поток с учетом бита активности выполнения
        /// </summary>
        /// <param name="active"></param>
        protected void StartTransferOutputKIS(bool active)
        {
            if (active)
            {
                kist.StartTransferOutputKIS();
            }
        }
        /// <summary>
        /// Запустить поток
        /// </summary>
        protected void StartTransferOutputKIS()
        {
            bool active_app = RWSetting.GetDB_Config_DefaultSetting("ActiveTransferOutputKIS", this.thread_output, this.active_output, true);
            StartTransferOutputKIS(active_app);
        }
        /// <summary>
        /// Первый запуск или перезапуск потока
        /// </summary>
        protected void RunTimerTransferOutputKIS()
        {
            StartTransferOutputKIS();
            timer_services_output.Start();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnTimerServicesTransferOutputKIS(object sender, System.Timers.ElapsedEventArgs args)
        {
            try
            {
                bool active_tmp = RWSetting.GetDB_Config_DefaultSetting("ActiveTransferOutputKIS", this.thread_output, this.active_output, true);
                StartTransferOutputKIS(active_tmp);
                if (active_tmp != this.active_output)
                {
                    this.active_output = active_tmp;
                    string mes_service_start = String.Format("Сервис : {0}, выполнение потока {1} - {2}", this.servece_name, this.thread_output, active_tmp ? "возабновленно" : "остановленно");
                    mes_service_start.WriteEvents(EventStatus.Ok, servece_name, eventID);
                }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("OnTimerServicesCopyBufferInputSostav(sender={0}, args={1})", sender, args.ToString()), this.servece_name, eventID);
            }
        }
        #endregion

    }
}
