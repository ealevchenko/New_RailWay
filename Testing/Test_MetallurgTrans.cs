using MetallurgTrans;
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
    }
}
