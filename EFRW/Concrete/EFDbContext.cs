using EFRW.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFRW.Concrete
{
    public class EFDbContext : DbContext
    {
        public EFDbContext()
            : base("name=RW")
        {
        }

        // Справочники системы Railway
        public virtual DbSet<ReferenceGroupCargo> ReferenceGroupCargo { get; set; }
        public virtual DbSet<ReferenceTypeCargo> ReferenceTypeCargo { get; set; }
        public virtual DbSet<ReferenceCargo> ReferenceCargo { get; set; }
        public virtual DbSet<ReferenceCountry> ReferenceCountry { get; set; }
        public virtual DbSet<ReferenceStation> ReferenceStation { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

        }

    }
}
