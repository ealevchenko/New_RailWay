namespace EFRW.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RailWay.Reference_Cars")]
    public partial class ReferenceCars
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ReferenceCars()
        {
            Cars = new HashSet<Cars>();
            ReferenceOwnerCars = new HashSet<ReferenceOwnerCars>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int num { get; set; }

        public int? id_type { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? lifting_capacity { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? tare { get; set; }

        public int? id_country { get; set; }

        public int? count_axles { get; set; }

        public bool? is_output_uz { get; set; }
         [Column(TypeName = "datetime")]
        public DateTime create_dt { get; set; }

        [Required]
        [StringLength(50)]
        public string create_user { get; set; }
         [Column(TypeName = "datetime")]
        public DateTime? change_dt { get; set; }

        [StringLength(50)]
        public string change_user { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cars> Cars { get; set; }

        public virtual ReferenceCountry ReferenceCountry { get; set; }

        public virtual ReferenceTypeCars ReferenceTypeCars { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReferenceOwnerCars> ReferenceOwnerCars { get; set; }
    }
}
