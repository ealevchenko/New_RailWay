namespace EFRC.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class STATIONS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public STATIONS()
        {
            VAGON_OPERATIONS = new HashSet<VAGON_OPERATIONS>();
        }

        [Key]
        public int id_stat { get; set; }

        [StringLength(200)]
        public string name { get; set; }

        public int? id_ora { get; set; }

        public int? outer_side { get; set; }

        public int? is_uz { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VAGON_OPERATIONS> VAGON_OPERATIONS { get; set; }
    }
}
