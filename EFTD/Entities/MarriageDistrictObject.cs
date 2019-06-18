namespace EFTD.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TD.MarriageDistrictObject")]
    public partial class MarriageDistrictObject
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MarriageDistrictObject()
        {
            MarriageWork = new HashSet<MarriageWork>();
        }

        public int id { get; set; }

        public int id_district { get; set; }

        [Column("district_object")]
        [Required]
        [StringLength(250)]
        public string district_object { get; set; }

        public int type_object { get; set; }

        public virtual MarriageDistrict MarriageDistrict { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MarriageWork> MarriageWork { get; set; }
    }
}
