namespace EFTD.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TD.MarriageCause")]
    public partial class MarriageCause
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MarriageCause()
        {
            MarriageWork = new HashSet<MarriageWork>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(250)]
        public string cause { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MarriageWork> MarriageWork { get; set; }
    }
}
