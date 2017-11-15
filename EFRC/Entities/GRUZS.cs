namespace EFRC.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GRUZS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GRUZS()
        {
            VAGON_OPERATIONS = new HashSet<VAGON_OPERATIONS>();
        }

        [Key]
        public int id_gruz { get; set; }

        [StringLength(200)]
        public string name { get; set; }

        [StringLength(500)]
        public string name_full { get; set; }

        public int? id_ora { get; set; }

        public int? id_ora2 { get; set; }

        public int? ETSNG { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VAGON_OPERATIONS> VAGON_OPERATIONS { get; set; }
    }
}
