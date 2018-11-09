namespace EFRW.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RW.Directory_Consignee")]
    public partial class Directory_Consignee
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Directory_Consignee()
        {
            CarInboundDelivery = new HashSet<CarInboundDelivery>();
        }

        public int id { get; set; }

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

        [Required]
        [StringLength(20)]
        public string name_abr_ru { get; set; }

        [Required]
        [StringLength(20)]
        public string name_abr_en { get; set; }

        public int? id_shop { get; set; }

        [StringLength(50)]
        public string user_create { get; set; }

        public DateTime dt_create { get; set; }

        [StringLength(50)]
        public string user_edit { get; set; }

        public DateTime? dt_edit { get; set; }

        public int? id_kis { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CarInboundDelivery> CarInboundDelivery { get; set; }

        public virtual Directory_Shops Directory_Shops { get; set; }
    }
}
