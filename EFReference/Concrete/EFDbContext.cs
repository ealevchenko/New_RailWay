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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }

    }
}
