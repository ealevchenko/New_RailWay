namespace EFRW.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RW.Directory_Owners")]
    public partial class Directory_Owners
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Directory_Owners()
        {
            Directory_OwnerCars = new HashSet<Directory_OwnerCars>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(300)]
        public string owner_name { get; set; }

        [StringLength(50)]
        public string owner_abr { get; set; }

        [Required]
        [StringLength(50)]
        public string user_create { get; set; }

        public DateTime dt_create { get; set; }

        [StringLength(50)]
        public string user_edit { get; set; }

        public DateTime? dt_edit { get; set; }

        public int? id_kis { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Directory_OwnerCars> Directory_OwnerCars { get; set; }
    }
}
