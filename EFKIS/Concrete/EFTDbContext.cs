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
    public class EFTDbContext : DbContext
    {
        public EFTDbContext()
            : base("name=TKIS")
        {

        }

        public virtual DbSet<BufferArrivalSostav> BufferArrivalSostav { get; set; }
        public virtual DbSet<BufferInputSostav> BufferInputSostav { get; set; }
        public virtual DbSet<BufferOutputSostav> BufferOutputSostav { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

        }

    }
}
