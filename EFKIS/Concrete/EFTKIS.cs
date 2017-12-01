using MessageLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFKIS.Concrete
{
    public class EFTKIS
    {
        private eventID eventID = eventID.EFTKIS;

        protected EFTDbContext context = new EFTDbContext();
    }
}
