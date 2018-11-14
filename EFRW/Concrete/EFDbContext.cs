namespace EFRW.Concrete
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using EFRW.Entities;

    public partial class EFDbContext : DbContext
    {
        public EFDbContext()
            : base("name=RW")
        {
        }
        // Вагоны
        public virtual DbSet<CarConditions> CarConditions { get; set; }
        public virtual DbSet<CarInboundDelivery> CarInboundDelivery { get; set; }
        public virtual DbSet<CarOperations> CarOperations { get; set; }
        public virtual DbSet<CarOutboundDelivery> CarOutboundDelivery { get; set; }
        public virtual DbSet<CarsInternal> CarsInternal { get; set; }
        public virtual DbSet<CarStatus> CarStatus { get; set; }
        // Справочники
        public virtual DbSet<Directory_GroupCargo> Directory_GroupCargo { get; set; }        
        public virtual DbSet<Directory_TypeCargo> Directory_TypeCargo { get; set; }
        public virtual DbSet<Directory_Cargo> Directory_Cargo { get; set; }

        public virtual DbSet<Directory_GroupCars> Directory_GroupCars { get; set; }
        public virtual DbSet<Directory_TypeCars> Directory_TypeCars { get; set; }
        public virtual DbSet<Directory_Cars> Directory_Cars { get; set; }

        public virtual DbSet<Directory_OwnerCars> Directory_OwnerCars { get; set; }
        public virtual DbSet<Directory_Owners> Directory_Owners { get; set; }

        public virtual DbSet<Directory_InternalStations> Directory_InternalStations { get; set; }
        public virtual DbSet<Directory_Shops> Directory_Shops { get; set; }
        public virtual DbSet<Directory_Overturn> Directory_Overturn { get; set; }

        public virtual DbSet<Directory_TypeWays> Directory_TypeWays { get; set; }
        public virtual DbSet<Directory_Ways> Directory_Ways { get; set; }

        public virtual DbSet<Directory_Consignee> Directory_Consignee { get; set; }
        public virtual DbSet<Directory_Country> Directory_Country { get; set; }
        public virtual DbSet<Directory_ExternalStations> Directory_ExternalStations { get; set; }






        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CarConditions>()
                .HasMany(e => e.CarOperations)
                .WithOptional(e => e.CarConditions)
                .HasForeignKey(e => e.id_car_conditions);

            modelBuilder.Entity<CarInboundDelivery>()
                .Property(e => e.weight_cargo)
                .HasPrecision(18, 3);

            modelBuilder.Entity<CarInboundDelivery>()
                .Property(e => e.weight_reweighing_sap)
                .HasPrecision(18, 3);

            modelBuilder.Entity<CarOperations>()
                .HasMany(e => e.CarOperations1)
                .WithOptional(e => e.CarOperations2)
                .HasForeignKey(e => e.parent_id);

            modelBuilder.Entity<CarOutboundDelivery>()
                .Property(e => e.weight_cargo)
                .HasPrecision(18, 3);

            modelBuilder.Entity<CarOutboundDelivery>()
                .Property(e => e.weight_reweighing_sap)
                .HasPrecision(18, 3);

            modelBuilder.Entity<CarsInternal>()
                .HasMany(e => e.CarInboundDelivery)
                .WithRequired(e => e.CarsInternal)
                .HasForeignKey(e => e.id_car_internal)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CarsInternal>()
                .HasMany(e => e.CarOperations)
                .WithRequired(e => e.CarsInternal)
                .HasForeignKey(e => e.id_car_internal)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CarsInternal>()
                .HasMany(e => e.CarOutboundDelivery)
                .WithRequired(e => e.CarsInternal)
                .HasForeignKey(e => e.id_car_internal)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CarsInternal>()
                .HasMany(e => e.CarsInternal1)
                .WithOptional(e => e.CarsInternal2)
                .HasForeignKey(e => e.parent_id);

            modelBuilder.Entity<CarStatus>()
                .HasMany(e => e.CarOperations)
                .WithOptional(e => e.CarStatus)
                .HasForeignKey(e => e.id_car_status);

            modelBuilder.Entity<Directory_Cargo>()
                .HasMany(e => e.CarInboundDelivery)
                .WithOptional(e => e.Directory_Cargo)
                .HasForeignKey(e => e.id_cargo);

            modelBuilder.Entity<Directory_Cargo>()
                .HasMany(e => e.CarOutboundDelivery)
                .WithOptional(e => e.Directory_Cargo)
                .HasForeignKey(e => e.id_cargo);

            modelBuilder.Entity<Directory_Cars>()
                .Property(e => e.lifting_capacity)
                .HasPrecision(18, 3);

            modelBuilder.Entity<Directory_Cars>()
                .Property(e => e.tare)
                .HasPrecision(18, 3);

            modelBuilder.Entity<Directory_Cars>()
                .HasMany(e => e.CarsInternal)
                .WithRequired(e => e.Directory_Cars)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Directory_Cars>()
                .HasMany(e => e.Directory_OwnerCars)
                .WithRequired(e => e.Directory_Cars)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Directory_Consignee>()
                .HasMany(e => e.CarInboundDelivery)
                .WithOptional(e => e.Directory_Consignee)
                .HasForeignKey(e => e.id_consignee);

            modelBuilder.Entity<Directory_Country>()
                .HasMany(e => e.CarInboundDelivery)
                .WithOptional(e => e.Directory_Country)
                .HasForeignKey(e => e.id_country);

            modelBuilder.Entity<Directory_Country>()
                .HasMany(e => e.CarOutboundDelivery)
                .WithOptional(e => e.Directory_Country)
                .HasForeignKey(e => e.id_country_out);

            modelBuilder.Entity<Directory_Country>()
                .HasMany(e => e.Directory_Cars)
                .WithOptional(e => e.Directory_Country)
                .HasForeignKey(e => e.id_country);

            modelBuilder.Entity<Directory_ExternalStations>()
                .HasMany(e => e.CarOutboundDelivery)
                .WithOptional(e => e.Directory_ExternalStations)
                .HasForeignKey(e => e.id_station_out);

            modelBuilder.Entity<Directory_GroupCargo>()
                .HasMany(e => e.Directory_TypeCargo)
                .WithRequired(e => e.Directory_GroupCargo)
                .HasForeignKey(e => e.id_group)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Directory_GroupCars>()
                .HasMany(e => e.Directory_TypeCars)
                .WithRequired(e => e.Directory_GroupCars)
                .HasForeignKey(e => e.id_group)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Directory_InternalStations>()
                .HasMany(e => e.Directory_Ways)
                .WithRequired(e => e.Directory_InternalStations)
                .HasForeignKey(e => e.id_station)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Directory_InternalStations>()
                .HasMany(e => e.Directory_Ways1)
                .WithOptional(e => e.Directory_InternalStations1)
                .HasForeignKey(e => e.id_station_end);

            modelBuilder.Entity<Directory_Overturn>()
                .HasMany(e => e.Directory_Ways)
                .WithOptional(e => e.Directory_Overturn)
                .HasForeignKey(e => e.id_overturn_end);

            modelBuilder.Entity<Directory_Owners>()
                .HasMany(e => e.Directory_OwnerCars)
                .WithRequired(e => e.Directory_Owners)
                .HasForeignKey(e => e.id_owner)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Directory_Shops>()
                .HasMany(e => e.Directory_Consignee)
                .WithOptional(e => e.Directory_Shops)
                .HasForeignKey(e => e.id_shop);

            modelBuilder.Entity<Directory_Shops>()
                .HasMany(e => e.Directory_Shops1)
                .WithOptional(e => e.Directory_Shops2)
                .HasForeignKey(e => e.parent_id);

            modelBuilder.Entity<Directory_Shops>()
                .HasMany(e => e.Directory_Ways)
                .WithOptional(e => e.Directory_Shops)
                .HasForeignKey(e => e.id_shop_end);

            modelBuilder.Entity<Directory_TypeCargo>()
                .HasMany(e => e.Directory_Cargo)
                .WithRequired(e => e.Directory_TypeCargo)
                .HasForeignKey(e => e.id_type)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Directory_TypeCars>()
                .HasMany(e => e.Directory_Cars)
                .WithOptional(e => e.Directory_TypeCars)
                .HasForeignKey(e => e.id_type);

            modelBuilder.Entity<Directory_TypeWays>()
                .HasMany(e => e.Directory_Ways)
                .WithRequired(e => e.Directory_TypeWays)
                .HasForeignKey(e => e.id_type_way)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Directory_Ways>()
                .HasMany(e => e.CarOperations)
                .WithOptional(e => e.Directory_Ways)
                .HasForeignKey(e => e.id_way);
        }
    }
}
