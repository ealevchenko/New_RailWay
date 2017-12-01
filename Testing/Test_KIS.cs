using KIS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testing
{
    public class Test_KIS
    {
        public Test_KIS() { }

        public void Test_KISTransfer_TransferArrivalKISToRailWay()
        {
            KISTransfer kis_trans = new KISTransfer();
            kis_trans.CopyArrivalKISToRailWay(1);
        }
    }
}
