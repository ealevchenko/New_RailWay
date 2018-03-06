namespace EFRW.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RailWay.Cars")]
    public partial class Cars
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Cars()
        {
            CarOperations = new HashSet<CarOperations>();
            CarsOutDelivery = new HashSet<CarsOutDelivery>();
            CarsInpDelivery = new HashSet<CarsInpDelivery>();
        }

        public int id { get; set; }

        public int id_sostav { get; set; }

        public int id_arrival { get; set; }

        public int num { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? dt_uz { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? dt_inp_amkr { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? dt_out_amkr { get; set; }

        public int? natur { get; set; }

        [StringLength(50)]
        public string user { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? dt_user { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CarOperations> CarOperations { get; set; }

        public virtual ReferenceCars ReferenceCars { get; set; }

        //public virtual CarsInpDelivery CarsInpDelivery { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CarsInpDelivery> CarsInpDelivery { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CarsOutDelivery> CarsOutDelivery { get; set; }
    }
}
