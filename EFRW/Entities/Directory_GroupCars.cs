namespace EFRW.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RW.Directory_GroupCars")]
    public partial class Directory_GroupCars
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Directory_GroupCars()
        {
            Directory_TypeCars = new HashSet<Directory_TypeCars>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(50)]
        public string group_cars_ru { get; set; }

        [Required]
        [StringLength(50)]
        public string group_cars_en { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Directory_TypeCars> Directory_TypeCars { get; set; }
    }
}
