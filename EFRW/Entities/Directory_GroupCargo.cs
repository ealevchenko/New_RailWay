namespace EFRW.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RW.Directory_GroupCargo")]
    public partial class Directory_GroupCargo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Directory_GroupCargo()
        {
            Directory_TypeCargo = new HashSet<Directory_TypeCargo>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(50)]
        public string group_name_ru { get; set; }

        [Required]
        [StringLength(50)]
        public string group_name_en { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Directory_TypeCargo> Directory_TypeCargo { get; set; }
    }
}
