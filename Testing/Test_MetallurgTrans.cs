using EFMT.Concrete;
using MessageLog;
using MetallurgTrans;
using EFMT.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using TransferRailCars;
using EFRC.Concrete;
using EFRC.Entities;
using RWConversionFunctions;

namespace Testing
{
    public class Test_MetallurgTrans
    {
        MTThread mth_timer = new MTThread(service.ServicesMT);
        
        public Test_MetallurgTrans() { }

        public void TransferApproaches()
        {
            MTTransfer mtt = new MTTransfer();
            mtt.FromPath = @"D:\txt_new";
            mtt.DeleteFile = true;
            int res_transfer = mtt.TransferApproaches();
        }

        public void TransferArrival()
        {
            MTTransfer mtt = new MTTransfer();
            mtt.DayRangeArrivalCars = 10; // 10
            mtt.ArrivalToRailWay = true;        // true
            mtt.ArrivalToRailCars = false;    //false
            mtt.FromPath = @"D:\xlm_new";                // C:\RailWay\temp_xml
            mtt.DeleteFile = true;             // 10
            int result = mtt.TransferArrival();

        }

        public void EFMetallurgTrans_Consignee()
        {
            EFMetallurgTrans efmt = new EFMetallurgTrans();

            Consignee code_amkr = new Consignee() { 
             code = 3437, description ="Основной код ПАО АМКР", consignee1 = 1, send =false
            };
            Consignee code_send6302 = new Consignee()
            {
                code = 6302,
                description = "Вспомогательный код ПАО АМКР при отправке досылочных грузов (добавлен в регламент)",
                consignee1 = 1,
                send = true
            };
            Consignee code_send9999 = new Consignee()
            {
                code = 9999,
                description = "Вспомогательный код ПАО АМКР при отправке досылочных грузов (добавлен в регламент)",
                consignee1 = 1,
                send = true
            };
            Console.WriteLine("Добавим код - 3437 -> {0}", efmt.SaveConsignee(code_amkr));
            Console.WriteLine("Добавим код - 6302 -> {0}", efmt.SaveConsignee(code_send6302));
            Console.WriteLine("Добавим код - 9999 -> {0}", efmt.SaveConsignee(code_send9999));
        }
        /// <summary>
        /// Поставить состав в прибытие системы RailCars
        /// </summary>
        public void TRailCars_ArrivalToRailCars() {
            TRailCars trc = new TRailCars();
            trc.ArrivalToRailCars(10564);

        }

        public void MTThread_TransferApproaches() {

            MTThread mth = new MTThread(service.ServicesMT);
            mth.StartTransferApproaches();
            //mth.StartTransferApproaches(30000);
            //mth.StopTransferApproaches();
            //System.Threading.Thread.Sleep(15);
            //mth.StartTransferApproaches(30000);
            //mth.StartTransferApproaches();
        }

        public void MTThread_Timer_TransferApproaches() {

            Timer timer_TransferApproaches = new Timer();
            timer_TransferApproaches.Interval = 30000;
            timer_TransferApproaches.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimerTransferApproaches);
            timer_TransferApproaches.Start();
            //MTThread mth = new MTThread(service.ServicesMT);
            //mth.StartTransferApproaches();
            //mth.StartTransferApproaches();
        }

        private void OnTimerTransferApproaches(object sender, ElapsedEventArgs e)
        {
            try
            {
                    //mth_timer.StartTransferApproaches();
            }
            catch (Exception ex)
            {
                return;
            }
        }

        public void MTThread_TransferArrival()
        {

            MTThread mth = new MTThread(service.ServicesMT);
            mth.StartTransferArrival();
        }

        public void MTTransfer_CloseApproachesCars()
        {
            MTTransfer mtt = new MTTransfer();
            int res = mtt.CloseApproachesCars();
            Console.WriteLine("Закрыто {0}", res);
        }

        public void MTThread_CloseApproachesCars()
        {
            MTThread mth = new MTThread(service.ServicesMT);
            mth.StartCloseApproachesCars();
        }

        public void MTTransfer_CorrectCloseArrivalSostav()
        {
            MTTransfer mtt = new MTTransfer();
            mtt.CorrectCloseArrivalSostav(120,10);
        }

        public void MTTransfer_CorrectCloseArrivalSostav(int id)
        {
            MTTransfer mtt = new MTTransfer();
            mtt.CorrectCloseArrivalSostav(id);
        }

        public void EFMetallurgTrans_GetWagonsTracking()
        {
            EFMetallurgTrans efmt = new EFMetallurgTrans();
            List<WagonsTracking> list =  efmt.GetWagonsTracking().ToList();
        }
        /// <summary>
        /// Тест проверки переноса информации слежения по вагонам АМКР
        /// </summary>
        public void MTTransfer_TransferWagonsTracking()
        {
            MTTransfer mtt = new MTTransfer();
            mtt.APIWagonsTracking = "/api/WagonsTracking";
            mtt.URLWagonsTracking = "http://umtrans.com.ua:81";
            mtt.UserWagonsTracking = "RailWayAMKR";
            mtt.PSWWagonsTracking = "Lbvrf_2709";
            int res = mtt.TransferWagonsTracking();
            Console.WriteLine("Перенесено {0}", res);
        }
        /// <summary>
        /// Тест выполнения потока переноса информации слежения по вагонам АМКР
        /// </summary>
        public void MTThread_TransferWagonsTracking()
        {
            MTThread mth = new MTThread(service.ServicesMT);
            mth.StartTransferWagonsTracking();
        }

        public void MTArrivalCarsClose()
        {
            EFMetallurgTrans efmt = new EFMetallurgTrans();
            EFRailCars ef_rc = new EFRailCars();
            List<ArrivalCars> list = efmt.ArrivalCars.Where(c=>c.Arrival == null & c.IDSostav>7234).OrderBy(c=>c.ID).ToList();
            foreach (ArrivalCars car in list) {
                VAGON_OPERATIONS vag = ef_rc.VAGON_OPERATIONS.Where(o => o.IDSostav == car.IDSostav & o.num_vagon == car.Num & o.n_natur != null).FirstOrDefault();
                    if (vag!=null){
                        car.Arrival = vag.dt_amkr;
                        car.NumDocArrival = vag.n_natur;
                        int res = efmt.SaveArrivalCars(car);
                        Console.WriteLine("Добавим закроем вагон {0}, состава {1} - {2}", car.Num, car.IDSostav, res);
                    }
            }

        }

        public void EFMetallurgTrans_GetRouteWagonsTrackingOfReports()
        {
            EFMetallurgTrans efmt = new EFMetallurgTrans();
            DateTime start = new DateTime(2018, 4, 1, 0, 0, 0);
            DateTime stop = new DateTime(2018, 4, 30, 23, 59, 59);
            List<RouteWagonTracking> list = efmt.GetRouteWagonTrackingOfReports(2, start, stop);
        }

        public void EFMetallurgTrans_GetLastRouteWagonsTrackingOfReports()
        {
            EFMetallurgTrans efmt = new EFMetallurgTrans();
            DateTime start = new DateTime(2018, 4, 1, 0, 0, 0);
            DateTime stop = new DateTime(2018, 4, 30, 23, 59, 59);
            List<RouteWagonTracking> list = efmt.GetLastRouteWagonTrackingOfReports(2, start, stop);
        }

        public void EFMetallurgTrans_GetLastWagonTrackingOfReports()
        {
            EFMetallurgTrans efmt = new EFMetallurgTrans();
            DateTime start = new DateTime(2018, 4, 1, 0, 0, 0);
            DateTime stop = new DateTime(2018, 4, 30, 23, 59, 59);
            efmt.GetLastWagonTrackingOfReports(2, start, stop);
        }

        public void EFMetallurgTrans_GetOperationWagonsTrackingOfNumCar()
        {
            EFMetallurgTrans efmt = new EFMetallurgTrans();
            List<OperationWagonsTracking> list1 = efmt.GetOperationWagonsTrackingOfNumCar(52921079, 145741, 153672);
            List<OperationWagonsTracking> list2 = efmt.GetOperationWagonsTrackingOfNumCar(52921079, null, null);
            List<OperationWagonsTracking> list3 = efmt.GetOperationWagonsTrackingOfNumCar(52921079, null, 153672);
            List<OperationWagonsTracking> list4 = efmt.GetOperationWagonsTrackingOfNumCar(52921079, 145741, null);
        }

        public void EFMetallurgTrans_GetHistoryArrivalCarsOfNum()
        {
            EFMetallurgTrans efmt = new EFMetallurgTrans();

            List<HistoryArrivalCars> history = efmt.GetHistoryArrivalCarsOfNum(52921004);

        }

        public void EFMetallurgTrans_GetWagonsTrackingOfNumCars()
        {
            EFMetallurgTrans efmt = new EFMetallurgTrans();

            List<WagonsTracking> list = efmt.GetWagonsTrackingOfNumCars(52921004).ToList();

        }
        /// <summary>
        /// Удалить соответствия между списками вагонов
        /// </summary>
        public void EFMetallurgTrans_RemoveMatchingArrivalCars()
        {
            EFMetallurgTrans efmt = new EFMetallurgTrans();

            List<ArrivalCars> list_new = efmt.GetArrivalSostav(13443).ArrivalCars.ToList();
            List<ArrivalCars> list_old = efmt.GetArrivalSostav(13427).ArrivalCars.ToList();
            efmt.RemoveMatchingArrivalCars(ref list_new, ref list_old);
        }


        #region  WTCycle Формирование циклограмм движения вагонов по данным МетТранса (TransferWagonsTracking)


        public void MTTransfer_TransferWTCycle()
        {
            MTTransfer mtt = new MTTransfer();
            int res = mtt.TransferWTCycle();
            Console.WriteLine("Перенесено {0}", res);
        }

        public void MTTransfer_TransferWTCycleOfNumm()
        {
            MTTransfer mtt = new MTTransfer();
            int res = mtt.TransferWTCycle(63532535);
            Console.WriteLine("Перенесено {0}", res);
        }

        public void MTTransfer_TransferWTCycle_StationOfNumm()
        {
            MTTransfer mtt = new MTTransfer();
            int res = mtt.TransferWTCycle_Station(63530661);
            Console.WriteLine("Перенесено {0}", res);
        }
        #endregion
    }
}
