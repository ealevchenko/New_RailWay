using EFKIS.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFKIS.Concrete
{
    public class EFTRCdbContext : DbContext
    {
        public EFTRCdbContext()
            : base("name=TRCKIS")
        {

        }

        public virtual DbSet<RCBufferArrivalSostav> RCBufferArrivalSostav { get; set; }
        public virtual DbSet<RCBufferInputSostav> RCBufferInputSostav { get; set; }
        public virtual DbSet<RCBufferOutputSostav> RCBufferOutputSostav { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

        }

    }
}
