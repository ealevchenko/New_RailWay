using EFAccess.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFAccess.Concrete
{
    public class EFDbContext : DbContext
    {
        public EFDbContext()
            : base("name=RW")
        {
        }
        public virtual DbSet<WebAccess> WebAccess { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

        }
    }
}
