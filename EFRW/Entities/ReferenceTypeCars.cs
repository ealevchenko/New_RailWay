namespace EFRW.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RailWay.Reference_TypeCars")]
    public partial class ReferenceTypeCars
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ReferenceTypeCars()
        {
            ReferenceCars = new HashSet<ReferenceCars>();
        }

        public int id { get; set; }

        public int id_group { get; set; }

        [Required]
        [StringLength(50)]
        public string type_cars_ru { get; set; }

        [Required]
        [StringLength(50)]
        public string type_cars_en { get; set; }

        [StringLength(5)]
        public string type_cars_abr_ru { get; set; }

        [StringLength(5)]
        public string type_cars_abr_en { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReferenceCars> ReferenceCars { get; set; }

        public virtual ReferenceGroupCars ReferenceGroupCars { get; set; }
    }
}
