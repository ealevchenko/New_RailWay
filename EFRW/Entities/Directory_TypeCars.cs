namespace EFRW.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RW.Directory_TypeCars")]
    public partial class Directory_TypeCars
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Directory_TypeCars()
        {
            Directory_Cars = new HashSet<Directory_Cars>();
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
        public virtual ICollection<Directory_Cars> Directory_Cars { get; set; }

        public virtual Directory_GroupCars Directory_GroupCars { get; set; }
    }
}
