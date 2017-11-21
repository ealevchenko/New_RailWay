using EFMT.Concrete;
using MessageLog;
using MetallurgTrans;
using MT.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using TransferRailCars;

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
            mtt.DeleteFile = true;
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
            //trc.ArrivalToRailCars(4113);
            trc.ArrivalToRailCars(4114);

        }

        public void MTThread_TransferApproaches() {

            MTThread mth = new MTThread(service.ServicesMT);
            mth.StartTransferApproaches();
            mth.StartTransferApproaches();
        }

        public void MTThread_Timer_TransferApproaches() {

            Timer timer_TransferApproaches = new Timer();
            timer_TransferApproaches.Interval = 60000;
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
                    mth_timer.StartTransferApproaches();
            }
            catch (Exception ex)
            {
                return;
            }
        }
    }
}
