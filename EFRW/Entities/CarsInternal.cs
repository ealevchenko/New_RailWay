namespace EFRW.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RW.CarsInternal")]
    public partial class CarsInternal
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CarsInternal()
        {
            CarInboundDelivery = new HashSet<CarInboundDelivery>();
            CarOperations = new HashSet<CarOperations>();
            CarOutboundDelivery = new HashSet<CarOutboundDelivery>();
            CarsInternal1 = new HashSet<CarsInternal>();
        }

        public int id { get; set; }

        public int id_sostav { get; set; }

        public int id_arrival { get; set; }

        public int num { get; set; }

        public DateTime? dt_uz { get; set; }

        public DateTime? dt_inp_amkr { get; set; }

        public DateTime? dt_out_amkr { get; set; }

        public int? natur_kis_inp { get; set; }

        public int? natur_kis_out { get; set; }

        public int? natur_rw { get; set; }

        [StringLength(50)]
        public string user_create { get; set; }

        public DateTime dt_create { get; set; }

        [StringLength(50)]
        public string user_close { get; set; }

        public DateTime? dt_close { get; set; }

        public int? parent_id { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CarInboundDelivery> CarInboundDelivery { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CarOperations> CarOperations { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CarOutboundDelivery> CarOutboundDelivery { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CarsInternal> CarsInternal1 { get; set; }

        public virtual CarsInternal CarsInternal2 { get; set; }

        public virtual Directory_Cars Directory_Cars { get; set; }
    }
}
