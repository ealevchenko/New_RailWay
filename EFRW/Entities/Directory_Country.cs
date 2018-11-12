namespace EFRW.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RW.Directory_Country")]
    public partial class Directory_Country
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Directory_Country()
        {
            CarInboundDelivery = new HashSet<CarInboundDelivery>();
            CarOutboundDelivery = new HashSet<CarOutboundDelivery>();
            Directory_Cars = new HashSet<Directory_Cars>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(100)]
        public string country_ru { get; set; }

        [Required]
        [StringLength(100)]
        public string country_en { get; set; }

        public int code { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CarInboundDelivery> CarInboundDelivery { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CarOutboundDelivery> CarOutboundDelivery { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Directory_Cars> Directory_Cars { get; set; }
    }
}
