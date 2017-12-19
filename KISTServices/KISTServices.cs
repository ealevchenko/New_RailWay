﻿using KIS;
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

        private int interval_copy_arrival = 60; // секунд
        private int interval_transfer_arrival = 300; // секунд
        private int interval_close_arrival = 3600; // секунд

        bool active_copy_arrival = true;
        bool active_transfer_arrival = true;
        bool active_close_arrival = true;

        System.Timers.Timer timer_services = new System.Timers.Timer();
        System.Timers.Timer timer_services_copy_arrival = new System.Timers.Timer();
        System.Timers.Timer timer_services_arrival = new System.Timers.Timer();
        System.Timers.Timer timer_services_close_arrival = new System.Timers.Timer();

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
                // состояние активности
                this.active_copy_arrival = RWSetting.GetDB_Config_DefaultSetting("ActiveCopyArrivalSostavKIS", this.thread_copy_arrival, this.active_copy_arrival, true);
                this.active_transfer_arrival = RWSetting.GetDB_Config_DefaultSetting("ActiveTransferArrivalKIS", this.thread_arrival, this.active_transfer_arrival, true);
                this.active_close_arrival = RWSetting.GetDB_Config_DefaultSetting("ActiveCloseArrivalSostavKIS", this.thread_close_arrival, this.active_close_arrival, true);
                
                //// Настроем таймер контроля выполнения сервиса
                timer_services.Interval = 30000;
                timer_services.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimerServices);

                timer_services_copy_arrival.Interval = this.interval_copy_arrival * 1000;
                timer_services_copy_arrival.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimerServicesCopyBufferArrivalSostav);

                timer_services_arrival.Interval = this.interval_transfer_arrival * 1000;
                timer_services_arrival.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimerServicesTransferArrivalKIS);

                timer_services_close_arrival.Interval = this.interval_close_arrival * 1000;
                timer_services_close_arrival.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimerServicesCloseBufferArrivalSostav);

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
            //TODO: Добавить запуск других таймеров
            //...............

            // Update the service state to Running.
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            // Отправить сообщение
            string mes_service_start = String.Format("Сервис : {0} - запущен. Интервал выполнения сервиса {1}-{2} сек, сервиса {3}-{4} сек, сервиса {5}-{6} сек.",
            this.servece_name, this.thread_copy_arrival, this.interval_copy_arrival, this.thread_arrival, this.interval_transfer_arrival, this.thread_close_arrival, this.interval_close_arrival);
            mes_service_start.WriteEvents(EventStatus.Ok, servece_name, eventID);
            this.thread_copy_arrival.WriteLogStatusServices();
            this.thread_arrival.WriteLogStatusServices();
            this.thread_close_arrival.WriteLogStatusServices();
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

                // CloseBufferArrivalSostav
                bool active_close = RWSetting.GetDB_Config_DefaultSetting("ActiveCloseArrivalSostavKIS", this.thread_close_arrival, this.active_close_arrival, true);
                int interval_close = RWSetting.GetDB_Config_DefaultSetting("IntervalCloseArrivalSostavKIS", this.thread_close_arrival, this.interval_close_arrival, true);
                if (active_close & interval_close != this.interval_close_arrival)
                {
                    this.interval_close_arrival = interval_close;
                    timer_services_close_arrival.Stop();
                    timer_services_close_arrival.Interval = this.interval_close_arrival * 1000;
                    RunTimerCloseBufferArrivalSostav();
                    string mes_service_start = String.Format("Сервис : {0}, интервал выполнения потока {1} изменен на {2} сек.", this.servece_name, this.thread_close_arrival, this.interval_close_arrival);
                    mes_service_start.WriteEvents(EventStatus.Ok, servece_name, eventID);
                }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("OnTimerServices(sender={0}, args={1})", sender, args.ToString()), this.servece_name, eventID);
            }
        }
        #endregion

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
                int interval_arr = RWSetting.GetDB_Config_DefaultSetting("IntervalTransferArrival", service.TransferArrival, 300, false);
                int? code_run_service = service.TransferArrival.GetCodeReturnServices(interval_arr+60);
                if (code_run_service == null)
                {
                    String.Format("Поток {0} сервиса {1} - заблокирован, из-за остановки службы {2}", this.thread_arrival, this.servece_name, service.TransferArrival).WriteError(servece_name, eventID);
                    return;
                }
                if (code_run_service < 0)
                {
                    String.Format("Поток {0} сервиса {1} - заблокирован, из-за ошибки {2} последнего выполнения службы {3}", this.thread_arrival, this.servece_name, code_run_service, service.TransferArrival).WriteError(servece_name, eventID);
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
                StartCopyArrival(active_close);
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

    }
}
