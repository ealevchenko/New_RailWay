using EFRW.Entities1;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFRW.Concrete
{
    public class EFDbContext1 : DbContext
    {
        public EFDbContext1()
            : base("name=RW")
        {
        }

        //public override int SaveChanges()
        //{
        //    UpdateDates();
        //    return base.SaveChanges();
        //}

        //private void UpdateDates()
        //{
        //    foreach (var change in ChangeTracker.Entries<CarOperations>())
        //    {
        //        var values = change.CurrentValues;
        //        foreach (var name in values.PropertyNames)
        //        {
        //            var value = values[name];
        //            if (value is DateTime)
        //            {
        //                var date = (DateTime)value;
        //                if (date < SqlDateTime.MinValue.Value)
        //                {
        //                    values[name] = SqlDateTime.MinValue.Value;
        //                }
        //                else if (date > SqlDateTime.MaxValue.Value)
        //                {
        //                    values[name] = SqlDateTime.MaxValue.Value;
        //                }
        //            }
        //        }
        //    }
        //}


        // Справочники системы Railway
        // Грузы
        public virtual DbSet<ReferenceGroupCargo> ReferenceGroupCargo { get; set; }
        public virtual DbSet<ReferenceTypeCargo> ReferenceTypeCargo { get; set; }
        public virtual DbSet<ReferenceCargo> ReferenceCargo { get; set; }
        // страны
        public virtual DbSet<ReferenceCountry> ReferenceCountry { get; set; }
        // Станции отпракви
        public virtual DbSet<ReferenceStation> ReferenceStation { get; set; }
        // Вагоны
        public virtual DbSet<ReferenceGroupCars> ReferenceGroupCars { get; set; }
        public virtual DbSet<ReferenceTypeCars> ReferenceTypeCars { get; set; }
        public virtual DbSet<ReferenceCars> ReferenceCars { get; set; }
        // Владельцы
        public virtual DbSet<ReferenceOwnerCars> ReferenceOwnerCars { get; set; }
        public virtual DbSet<ReferenceOwners> ReferenceOwners { get; set; }
        // Грузополучатели на АМКР
        public virtual DbSet<ReferenceConsignee> ReferenceConsignee { get; set; }

        // таблицы Railway
        // Станции, узлы и пути, цеха, тупики
        public virtual DbSet<Stations> Stations { get; set; }
        public virtual DbSet<StationsNodes> StationsNodes { get; set; }
        public virtual DbSet<Ways> Ways { get; set; }
        public virtual DbSet<Shops> Shops { get; set; }
        public virtual DbSet<Deadlock> Deadlock { get; set; }
        // Вагоны и опрерации
        public virtual DbSet<Cars> Cars { get; set; }
        public virtual DbSet<CarOperations> CarOperations { get; set; }
        public virtual DbSet<CarStatus> CarStatus { get; set; }
        public virtual DbSet<CarConditions> CarConditions { get; set; }
        // Входящие и исходящие грузы
        public virtual DbSet<CarsInpDelivery> CarsInpDelivery { get; set; }
        public virtual DbSet<CarsOutDelivery> CarsOutDelivery { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CarConditions>()
                .HasMany(e => e.CarOperations)
                .WithOptional(e => e.CarConditions)
                .HasForeignKey(e => e.id_car_conditions);

            modelBuilder.Entity<Cars>()
                .HasMany(e => e.CarOperations)
                .WithRequired(e => e.Cars)
                .HasForeignKey(e => e.id_car)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Cars>()
                .HasMany(e => e.CarsInpDelivery)
                .WithRequired(e => e.Cars)
                .HasForeignKey(e => e.id_car)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Cars>()
                .HasMany(e => e.CarsOutDelivery)
                .WithRequired(e => e.Cars)
                .HasForeignKey(e => e.id_car)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CarsInpDelivery>()
                .Property(e => e.weight_cargo)
                .HasPrecision(18, 3);

            modelBuilder.Entity<CarsInpDelivery>()
                .Property(e => e.weight_reweighing_sap)
                .HasPrecision(18, 3);

            modelBuilder.Entity<CarsOutDelivery>()
                .Property(e => e.weight_cargo)
                .HasPrecision(18, 3);

            modelBuilder.Entity<CarsOutDelivery>()
                .Property(e => e.weight_reweighing_sap)
                .HasPrecision(18, 3);

            modelBuilder.Entity<CarStatus>()
                .HasMany(e => e.CarOperations)
                .WithOptional(e => e.CarStatus)
                .HasForeignKey(e => e.id_car_status);

            modelBuilder.Entity<ReferenceCargo>()
                .HasMany(e => e.CarsInpDelivery)
                .WithOptional(e => e.ReferenceCargo)
                .HasForeignKey(e => e.id_cargo);

            modelBuilder.Entity<ReferenceCargo>()
                .HasMany(e => e.CarsOutDelivery)
                .WithOptional(e => e.ReferenceCargo)
                .HasForeignKey(e => e.id_cargo);

            modelBuilder.Entity<ReferenceCars>()
                .Property(e => e.lifting_capacity)
                .HasPrecision(18, 3);

            modelBuilder.Entity<ReferenceCars>()
                .Property(e => e.tare)
                .HasPrecision(18, 3);

            modelBuilder.Entity<ReferenceCars>()
                .HasMany(e => e.Cars)
                .WithRequired(e => e.ReferenceCars)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReferenceCars>()
                .HasMany(e => e.ReferenceOwnerCars)
                .WithRequired(e => e.ReferenceCars)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReferenceCountry>()
                .HasMany(e => e.CarsOutDelivery)
                .WithOptional(e => e.ReferenceCountry)
                .HasForeignKey(e => e.id_country_out);

            modelBuilder.Entity<ReferenceCountry>()
                .HasMany(e => e.ReferenceCars)
                .WithOptional(e => e.ReferenceCountry)
                .HasForeignKey(e => e.id_country);

            modelBuilder.Entity<ReferenceGroupCargo>()
                .HasMany(e => e.ReferenceTypeCargo)
                .WithRequired(e => e.ReferenceGroupCargo)
                .HasForeignKey(e => e.id_group)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReferenceGroupCars>()
                .HasMany(e => e.ReferenceTypeCars)
                .WithRequired(e => e.ReferenceGroupCars)
                .HasForeignKey(e => e.id_group)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReferenceOwners>()
                .HasMany(e => e.ReferenceOwnerCars)
                .WithRequired(e => e.ReferenceOwners)
                .HasForeignKey(e => e.id_owner)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReferenceStation>()
                .HasMany(e => e.CarsOutDelivery)
                .WithOptional(e => e.ReferenceStation)
                .HasForeignKey(e => e.id_station_out);

            modelBuilder.Entity<ReferenceTypeCargo>()
                .HasMany(e => e.ReferenceCargo)
                .WithRequired(e => e.ReferenceTypeCargo)
                .HasForeignKey(e => e.id_type)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReferenceTypeCars>()
                .HasMany(e => e.ReferenceCars)
                .WithOptional(e => e.ReferenceTypeCars)
                .HasForeignKey(e => e.id_type);

            modelBuilder.Entity<Stations>()
                .HasMany(e => e.CarOperations)
                .WithOptional(e => e.Stations)
                .HasForeignKey(e => e.id_station);

            modelBuilder.Entity<Stations>()
                .HasMany(e => e.StationsNodes)
                .WithRequired(e => e.Stations)
                .HasForeignKey(e => e.id_station_from)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Stations>()
                .HasMany(e => e.StationsNodes1)
                .WithRequired(e => e.Stations1)
                .HasForeignKey(e => e.id_station_on)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Stations>()
                .HasMany(e => e.Ways)
                .WithRequired(e => e.Stations)
                .HasForeignKey(e => e.id_station)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Stations>()
                .HasMany(e => e.Shops)
                .WithOptional(e => e.Stations)
                .HasForeignKey(e => e.id_station);

            modelBuilder.Entity<Ways>()
                .HasMany(e => e.CarOperations)
                .WithOptional(e => e.Ways)
                .HasForeignKey(e => e.id_way);

            modelBuilder.Entity<ReferenceConsignee>()
                .HasMany(e => e.CarsInpDelivery)
                .WithOptional(e => e.Reference_Consignee)
                .HasForeignKey(e => e.id_consignee);

            modelBuilder.Entity<Shops>()
                .HasMany(e => e.Reference_Consignee)
                .WithOptional(e => e.Shops)
                .HasForeignKey(e => e.id_shop);

            modelBuilder.Entity<Shops>()
                .HasMany(e => e.Deadlock)
                .WithOptional(e => e.Shops)
                .HasForeignKey(e => e.id_shop);

            modelBuilder.Entity<Ways>()
                .HasMany(e => e.Deadlock)
                .WithOptional(e => e.Ways)
                .HasForeignKey(e => e.id_way);
        }

    }
}
