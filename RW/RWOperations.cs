using MessageLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW
{
    public class RWOperations : IRWOperations
    {
        private eventID eventID = eventID.RW_RWOperations;
        protected service servece_owner = service.Null;
        bool log_detali = false;                            // Признак детального логирования

        public RWOperations()
        {

        }

        public RWOperations(service servece_owner)
        {
            this.servece_owner = servece_owner;
        }


    }
}
