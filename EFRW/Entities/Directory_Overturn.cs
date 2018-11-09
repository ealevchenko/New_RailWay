namespace EFRW.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RW.Directory_Overturn")]
    public partial class Directory_Overturn
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Directory_Overturn()
        {
            Directory_Ways = new HashSet<Directory_Ways>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(250)]
        public string name_ru { get; set; }

        [Required]
        [StringLength(250)]
        public string name_en { get; set; }

        [Required]
        [StringLength(50)]
        public string user_create { get; set; }

        public DateTime dt_create { get; set; }

        [StringLength(50)]
        public string user_edit { get; set; }

        public DateTime? dt_edit { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Directory_Ways> Directory_Ways { get; set; }
    }
}
