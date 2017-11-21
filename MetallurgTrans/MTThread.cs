using MessageLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MetallurgTrans
{
    public class MTThread
    {
        private static eventID eventID = eventID.MetallurgTrans_MTThread;
        protected static service servece_owner = service.Null;

        protected Thread thTransferApproaches = new Thread(TransferApproaches);


        public bool statusTransferApproaches { get { return thTransferApproaches.IsAlive; } }


        public MTThread()
        { 
       
        }

        public MTThread(service servece_name) {
            servece_owner = servece_name;
        }

        private static void TransferApproaches()
        {
            try
            {


            }
            catch (Exception ex)
            {
                ex.WriteError(String.Format("Ошибка выполнения цикла переноса, потока {0} сервис {1}", service.TransferApproaches.ToString(), servece_owner), servece_owner, eventID);
                //service.TransferApproaches.SetStatusExecution(dt_start, DateTime.Now, -1);
            }
        }
    }
}
