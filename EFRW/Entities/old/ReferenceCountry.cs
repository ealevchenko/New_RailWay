namespace EFRW.Entities1
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RailWay.Reference_Country")]
    public partial class ReferenceCountry
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ReferenceCountry()
        {
            CarsOutDelivery = new HashSet<CarsOutDelivery>();
            ReferenceCars = new HashSet<ReferenceCars>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(100)]
        public string country_ru { get; set; }

        [Required]
        [StringLength(100)]
        public string country_en { get; set; }

        public int code { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CarsOutDelivery> CarsOutDelivery { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReferenceCars> ReferenceCars { get; set; }
    }
    //public partial class ReferenceCountry
    //{
    //    [Key]
    //    public int id { get; set; }

    //    [Required]
    //    [StringLength(100)]
    //    public string country { get; set; }

    //    public int code { get; set; }
    //}
}
