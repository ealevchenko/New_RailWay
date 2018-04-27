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
    public class EFTRWdbContext : DbContext
    {
        public EFTRWdbContext()
            : base("name=TRWKIS")
        {

        }

        public virtual DbSet<RWBufferArrivalSostav> RWBufferArrivalSostav { get; set; }
        public virtual DbSet<RWBufferInputSostav> RWBufferInputSostav { get; set; }
        public virtual DbSet<RWBufferOutputSostav> RWBufferOutputSostav { get; set; }

        public virtual DbSet<RWBufferSendingSostav> RWBufferSendingSostav { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

        }

    }
}
