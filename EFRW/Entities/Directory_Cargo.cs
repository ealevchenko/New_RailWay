namespace EFRW.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RW.Directory_Cargo")]
    public partial class Directory_Cargo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Directory_Cargo()
        {
            CarInboundDelivery = new HashSet<CarInboundDelivery>();
            CarOutboundDelivery = new HashSet<CarOutboundDelivery>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(200)]
        public string name_ru { get; set; }

        [Required]
        [StringLength(200)]
        public string name_en { get; set; }

        [Required]
        [StringLength(500)]
        public string name_full_ru { get; set; }

        [Required]
        [StringLength(500)]
        public string name_full_en { get; set; }

        public int etsng { get; set; }

        public int id_type { get; set; }

        [Required]
        [StringLength(50)]
        public string user_create { get; set; }

        public DateTime dt_create { get; set; }

        [StringLength(50)]
        public string user_edit { get; set; }

        public DateTime? dt_edit { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CarInboundDelivery> CarInboundDelivery { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CarOutboundDelivery> CarOutboundDelivery { get; set; }

        public virtual Directory_TypeCargo Directory_TypeCargo { get; set; }
    }
}
