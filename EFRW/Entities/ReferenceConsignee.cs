namespace EFRW.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RailWay.Reference_Consignee")]
    public partial class ReferenceConsignee
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ReferenceConsignee()
        {
            CarsInpDelivery = new HashSet<CarsInpDelivery>();
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

        public DateTime create_dt { get; set; }

        [Required]
        [StringLength(50)]
        public string create_user { get; set; }

        public DateTime? change_dt { get; set; }

        [StringLength(50)]
        public string change_user { get; set; }

        public int? id_kis { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CarsInpDelivery> CarsInpDelivery { get; set; }

        public virtual Shops Shops { get; set; }
    }
}
