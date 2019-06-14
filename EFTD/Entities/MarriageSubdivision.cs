namespace EFTD.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TD.MarriageSubdivision")]
    public partial class MarriageSubdivision
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MarriageSubdivision()
        {
            MarriageWork = new HashSet<MarriageWork>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(50)]
        public string subdivision { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MarriageWork> MarriageWork { get; set; }
    }
}
