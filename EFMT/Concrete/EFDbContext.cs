using EFMT.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMT.Concrete
{
    public class EFDbContext : DbContext
    {
        public EFDbContext()
            : base("name=MT")
        {
        }


        public virtual DbSet<ApproachesCars> ApproachesCars { get; set; }
        public virtual DbSet<ApproachesSostav> ApproachesSostav { get; set; }
        public virtual DbSet<ArrivalCars> ArrivalCars { get; set; }
        public virtual DbSet<ArrivalSostav> ArrivalSostav { get; set; }
        public virtual DbSet<Consignee> Consignee { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApproachesSostav>()
                .HasMany(e => e.ApproachesCars)
                .WithRequired(e => e.ApproachesSostav)
                .HasForeignKey(e => e.IDSostav)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ArrivalSostav>()
                .HasMany(e => e.ArrivalCars)
                .WithRequired(e => e.ArrivalSostav)
                .HasForeignKey(e => e.IDSostav)
                .WillCascadeOnDelete(false);
        }

    }
}
