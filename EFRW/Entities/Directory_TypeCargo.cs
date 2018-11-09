namespace EFRW.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RW.Directory_TypeCargo")]
    public partial class Directory_TypeCargo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Directory_TypeCargo()
        {
            Directory_Cargo = new HashSet<Directory_Cargo>();
        }

        public int id { get; set; }

        public int id_group { get; set; }

        [Required]
        [StringLength(50)]
        public string type_name_ru { get; set; }

        [Required]
        [StringLength(50)]
        public string type_name_en { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Directory_Cargo> Directory_Cargo { get; set; }

        public virtual Directory_GroupCargo Directory_GroupCargo { get; set; }
    }
}
