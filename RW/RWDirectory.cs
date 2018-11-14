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
    /// Класс справочной систмы RailWay
    /// </summary>
    public class RWDirectory
    {
        private eventID eventID = eventID.RW_RWDirectory;
        private service servece_owner;
        private EFDbContext db;
        private bool log_detali = false;


        public RWDirectory()
        {
            this.db = new EFDbContext();
            this.servece_owner = service.Null;
        }

        public RWDirectory(EFDbContext db)
        {
            this.db = db;
            this.servece_owner = service.Null;
        }

        public RWDirectory(service servece_owner)
        {
            this.servece_owner = servece_owner;
            this.db = new EFDbContext();
        }

        public RWDirectory(EFDbContext db, service servece_owner)
        {
            this.servece_owner = servece_owner;
            this.db = db;
        }
    }
}
