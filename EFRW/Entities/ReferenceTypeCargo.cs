namespace EFRW.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RailWay.Reference_TypeCargo")]
    public partial class ReferenceTypeCargo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ReferenceTypeCargo()
        {
            ReferenceCargo = new HashSet<ReferenceCargo>();
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
        public virtual ICollection<ReferenceCargo> ReferenceCargo { get; set; }

        public virtual ReferenceGroupCargo ReferenceGroupCargo { get; set; }
    }
    //public partial class ReferenceTypeCargo
    //{
    //    [Key]
    //    public int id { get; set; }

    //    [Required]
    //    [StringLength(50)]
    //    public string type_name_ru { get; set; }

    //    [Required]
    //    [StringLength(50)]
    //    public string type_name_en { get; set; }
    //}
}
