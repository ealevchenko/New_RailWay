using EFMT.Concrete;
using MetallurgTrans;
using MT.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testing
{
    public class Test_MetallurgTrans
    {
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
    }
}
