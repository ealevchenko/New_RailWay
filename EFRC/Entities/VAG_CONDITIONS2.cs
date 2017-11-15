namespace EFRC.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class VAG_CONDITIONS2
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public VAG_CONDITIONS2()
        {
            VAG_CONDITIONS21 = new HashSet<VAG_CONDITIONS2>();
            VAGON_OPERATIONS = new HashSet<VAGON_OPERATIONS>();
            WAYS = new HashSet<WAYS>();
        }

        [Key]
        public int id_cond { get; set; }

        [StringLength(50)]
        public string name { get; set; }

        public int? id_cond_after { get; set; }

        public int? order { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VAG_CONDITIONS2> VAG_CONDITIONS21 { get; set; }

        public virtual VAG_CONDITIONS2 VAG_CONDITIONS22 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VAGON_OPERATIONS> VAGON_OPERATIONS { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WAYS> WAYS { get; set; }
    }
}
