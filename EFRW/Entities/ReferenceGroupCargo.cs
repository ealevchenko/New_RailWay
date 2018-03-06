namespace EFRW.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RailWay.Reference_GroupCargo")]
    public partial class ReferenceGroupCargo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ReferenceGroupCargo()
        {
            ReferenceTypeCargo = new HashSet<ReferenceTypeCargo>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(50)]
        public string group_name_ru { get; set; }

        [Required]
        [StringLength(50)]
        public string group_name_en { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReferenceTypeCargo> ReferenceTypeCargo { get; set; }
    }
    //public partial class ReferenceGroupCargo
    //{
    //    [Key]
    //    public int id { get; set; }

    //    [Required]
    //    [StringLength(50)]
    //    public string group_name_ru { get; set; }

    //    [Required]
    //    [StringLength(50)]
    //    public string group_name_en { get; set; }
    //}
}
