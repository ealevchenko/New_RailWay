using EFKIS.Concrete;
using KIS;
using MessageLog;
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
            kis_trans.CopyBufferArrivalSostavOfKIS(1);
        }
        /// <summary>
        /// Поставить на путь
        /// </summary>
        public void Test_KISTransfer_PutCarsToStation()
        {
            KISTransfer kis_trans = new KISTransfer();
            EFTKIS ef_tkis = new EFTKIS();
            EFKIS.Entities.BufferArrivalSostav arr_s = ef_tkis.GetBufferArrivalSostav(44);
            kis_trans.PutCarsToStation(ref arr_s);
        }

        public void Test_KISTransfer_UpdateCarsToStation()
        {
            KISTransfer kis_trans = new KISTransfer();
            EFTKIS ef_tkis = new EFTKIS();
            EFKIS.Entities.BufferArrivalSostav arr_s = ef_tkis.GetBufferArrivalSostav(44);
            kis_trans.UpdateCarsToStation(ref arr_s);
        }

        public void Test_KISThread_StartCopyBufferArrivalSostav()
        {
            KISThread kis_t = new KISThread(service.ServicesKIS);
            kis_t.StartCopyBufferArrivalSostav();
            kis_t.StartCopyBufferArrivalSostav();
        }

        public void Test_KISThread_StartTransferArrivalOfKIS()
        {
            KISThread kis_t = new KISThread(service.ServicesKIS);
            kis_t.StartTransferArrivalOfKIS();
        }
    }
}
