namespace EFTD.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TD.MarriageDistrict")]
    public partial class MarriageDistrict
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MarriageDistrict()
        {
            MarriageDistrictObject = new HashSet<MarriageDistrictObject>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(250)]
        public string district { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MarriageDistrictObject> MarriageDistrictObject { get; set; }
    }
}
