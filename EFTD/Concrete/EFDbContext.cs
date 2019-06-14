namespace EFTD.Concrete
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using EFTD.Entities;

    public partial class EFDbContext : DbContext
    {
        public EFDbContext()
            : base("name=TD")
        {
        }

        public virtual DbSet<MarriageCause> MarriageCause { get; set; }
        public virtual DbSet<MarriageClassification> MarriageClassification { get; set; }
        public virtual DbSet<MarriageNature> MarriageNature { get; set; }
        public virtual DbSet<MarriagePlace> MarriagePlace { get; set; }
        public virtual DbSet<MarriageSubdivision> MarriageSubdivision { get; set; }
        public virtual DbSet<MarriageWork> MarriageWork { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MarriageCause>()
                .HasMany(e => e.MarriageWork)
                .WithRequired(e => e.MarriageCause)
                .HasForeignKey(e => e.id_cause)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MarriageClassification>()
                .HasMany(e => e.MarriageWork)
                .WithRequired(e => e.MarriageClassification)
                .HasForeignKey(e => e.id_classification)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MarriageNature>()
                .HasMany(e => e.MarriageWork)
                .WithOptional(e => e.MarriageNature)
                .HasForeignKey(e => e.id_nature);

            modelBuilder.Entity<MarriagePlace>()
                .HasMany(e => e.MarriageWork)
                .WithRequired(e => e.MarriagePlace)
                .HasForeignKey(e => e.id_place)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MarriageSubdivision>()
                .HasMany(e => e.MarriageWork)
                .WithRequired(e => e.MarriageSubdivision)
                .HasForeignKey(e => e.id_subdivision)
                .WillCascadeOnDelete(false);
        }
    }
}
