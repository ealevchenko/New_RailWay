namespace EFRW.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RW.Directory_TypeWays")]
    public partial class Directory_TypeWays
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Directory_TypeWays()
        {
            Directory_Ways = new HashSet<Directory_Ways>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(50)]
        public string type_way_ru { get; set; }

        [Required]
        [StringLength(50)]
        public string type_way_en { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Directory_Ways> Directory_Ways { get; set; }
    }
}
