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
            mtt.FromPath = @"D:\xlm_new";
            mtt.DeleteFile = false;
            mtt.ArrivalToRailWay = false;
            int res_transfer = mtt.TransferArrival();
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

    }
}
