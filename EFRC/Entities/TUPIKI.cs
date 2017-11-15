namespace EFRC.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TUPIKI")]
    public partial class TUPIKI
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TUPIKI()
        {
            VAGON_OPERATIONS = new HashSet<VAGON_OPERATIONS>();
        }

        [Key]
        public int id_tupik { get; set; }

        [StringLength(50)]
        public string name { get; set; }

        public int? id_ora { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VAGON_OPERATIONS> VAGON_OPERATIONS { get; set; }
    }
}
