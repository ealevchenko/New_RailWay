namespace EFRW.Entities1
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RailWay.Reference_GroupCars")]
    public partial class ReferenceGroupCars
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ReferenceGroupCars()
        {
            ReferenceTypeCars = new HashSet<ReferenceTypeCars>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(50)]
        public string group_cars_ru { get; set; }

        [Required]
        [StringLength(50)]
        public string group_cars_en { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReferenceTypeCars> ReferenceTypeCars { get; set; }
    }
}
