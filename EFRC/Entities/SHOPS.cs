namespace EFRC.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SHOPS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SHOPS()
        {
            VAGON_OPERATIONS = new HashSet<VAGON_OPERATIONS>();
        }

        [Key]
        public int id_shop { get; set; }

        [StringLength(250)]
        public string name { get; set; }

        [StringLength(500)]
        public string name_full { get; set; }

        public int? id_stat { get; set; }

        public int? id_ora { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VAGON_OPERATIONS> VAGON_OPERATIONS { get; set; }
    }
}
