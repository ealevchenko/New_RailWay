using EFReference.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFReference.Concrete
{
    public class EFDbContext : DbContext
    {
        public EFDbContext()
            : base("name=Reference")
        {
        }

        public virtual DbSet<Cargo> Cargo { get; set; }

        public virtual DbSet<Countrys> Countrys { get; set; }
        public virtual DbSet<InternalRailroad> InternalRailroad { get; set; }
        public virtual DbSet<States> States { get; set; }
        public virtual DbSet<Stations> Stations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InternalRailroad>()
                .HasMany(e => e.Stations)
                .WithOptional(e => e.InternalRailroad)
                .HasForeignKey(e => e.id_ir);

            modelBuilder.Entity<States>()
                .HasMany(e => e.Countrys)
                .WithOptional(e => e.States)
                .HasForeignKey(e => e.id_state);

            modelBuilder.Entity<States>()
                .HasMany(e => e.InternalRailroad)
                .WithRequired(e => e.States)
                .HasForeignKey(e => e.id_state)
                .WillCascadeOnDelete(false);
        }

    }
}
