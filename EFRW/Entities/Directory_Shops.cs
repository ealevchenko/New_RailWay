namespace EFRW.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RW.Directory_Shops")]
    public partial class Directory_Shops
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Directory_Shops()
        {
            Directory_Consignee = new HashSet<Directory_Consignee>();
            Directory_Shops1 = new HashSet<Directory_Shops>();
            Directory_Ways = new HashSet<Directory_Ways>();
        }

        public int id { get; set; }

        public int? id_station { get; set; }

        [Required]
        [StringLength(250)]
        public string name_ru { get; set; }

        [Required]
        [StringLength(250)]
        public string name_en { get; set; }

        [Required]
        [StringLength(500)]
        public string name_full_ru { get; set; }

        [Required]
        [StringLength(500)]
        public string name_full_en { get; set; }

        public int? code_amkr { get; set; }

        [Required]
        [StringLength(50)]
        public string user_create { get; set; }

        public DateTime dt_create { get; set; }

        [StringLength(50)]
        public string user_edit { get; set; }

        public DateTime? dt_edit { get; set; }

        public int? id_kis { get; set; }

        public int? parent_id { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Directory_Consignee> Directory_Consignee { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Directory_Shops> Directory_Shops1 { get; set; }

        public virtual Directory_Shops Directory_Shops2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Directory_Ways> Directory_Ways { get; set; }
    }
}
