using EFRW.Concrete;
using MessageLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW
{
    
    /// <summary>
    /// Класс Вагоны систмы RailWay
    /// </summary>
    public class RWCars
    {
        private eventID eventID = eventID.RW_RWCars;
        private service servece_owner;
        private EFDbContext db;
        private bool log_detali = false;


        public RWCars()
        {
            this.db = new EFDbContext();
            this.servece_owner = service.Null;
        }

        public RWCars(EFDbContext db)
        {
            this.db = db;
            this.servece_owner = service.Null;
        }

        public RWCars(service servece_owner)
        {
            this.servece_owner = servece_owner;
            this.db = new EFDbContext();
        }

        public RWCars(EFDbContext db, service servece_owner)
        {
            this.servece_owner = servece_owner;
            this.db = db;
        }



    }
}
