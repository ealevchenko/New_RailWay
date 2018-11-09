namespace EFRW.Entities1
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RailWay.Reference_Station")]
    public partial class ReferenceStation
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ReferenceStation()
        {
            CarsOutDelivery = new HashSet<CarsOutDelivery>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(100)]
        public string name { get; set; }

        [Required]
        [StringLength(50)]
        public string station { get; set; }

        [Required]
        [StringLength(250)]
        public string internal_railroad { get; set; }

        [Required]
        [StringLength(10)]
        public string ir_abbr { get; set; }

        [Required]
        [StringLength(250)]
        public string name_network { get; set; }

        [Required]
        [StringLength(10)]
        public string nn_abbr { get; set; }

        public int code_cs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CarsOutDelivery> CarsOutDelivery { get; set; }
    }
    //public partial class ReferenceStation
    //{
    //    [Key]
    //    public int id { get; set; }

    //    [Required]
    //    [StringLength(100)]
    //    public string name { get; set; }

    //    [Required]
    //    [StringLength(50)]
    //    public string station { get; set; }

    //    [Required]
    //    [StringLength(250)]
    //    public string internal_railroad { get; set; }

    //    [Required]
    //    [StringLength(10)]
    //    public string ir_abbr { get; set; }

    //    [Required]
    //    [StringLength(250)]
    //    public string name_network { get; set; }

    //    [Required]
    //    [StringLength(10)]
    //    public string nn_abbr { get; set; }

    //    public int code_cs { get; set; }
    //}
}
