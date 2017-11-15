namespace EFRC.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class NAZN_COUNTRIES
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NAZN_COUNTRIES()
        {
            OWNERS = new HashSet<OWNERS>();
        }

        [Key]
        public int id_country { get; set; }

        [StringLength(250)]
        public string name { get; set; }

        [StringLength(10)]
        public string id_ora { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OWNERS> OWNERS { get; set; }
    }
}
