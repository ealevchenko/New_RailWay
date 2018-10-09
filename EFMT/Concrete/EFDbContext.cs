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

        public virtual DbSet<WagonsTracking> WagonsTracking { get; set; }
        public virtual DbSet<ListWagonsTracking> ListWagonsTracking { get; set; }
        public virtual DbSet<WTCarsReports> WTCarsReports { get; set; }
        public virtual DbSet<WTReports> WTReports { get; set; }
        public virtual DbSet<WTCycle> WTCycle { get; set; }

        public virtual DbSet<BufferArrivalSostav> BufferArrivalSostav { get; set; }

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

            modelBuilder.Entity<WagonsTracking>()
                .Property(e => e.ves)
                .HasPrecision(18, 3);

            modelBuilder.Entity<WagonsTracking>()
                .HasMany(e => e.WTCycle)
                .WithRequired(e => e.WagonsTracking)
                .HasForeignKey(e => e.id_wt)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ListWagonsTracking>()
                .HasMany(e => e.WTCarsReports)
                .WithRequired(e => e.ListWagonsTracking)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<WTReports>()
                .HasMany(e => e.WTCarsReports)
                .WithRequired(e => e.WTReports)
                .HasForeignKey(e => e.id_wtreport)
                .WillCascadeOnDelete(false);


        }

    }
}
