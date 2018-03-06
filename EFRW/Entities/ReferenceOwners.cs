namespace EFRW.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RailWay.Reference_Owners")]
    public partial class ReferenceOwners
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ReferenceOwners()
        {
            ReferenceOwnerCars = new HashSet<ReferenceOwnerCars>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(300)]
        public string owner_name { get; set; }

        [StringLength(50)]
        public string owner_abr { get; set; }

        public int? id_kis { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReferenceOwnerCars> ReferenceOwnerCars { get; set; }
    }
}
