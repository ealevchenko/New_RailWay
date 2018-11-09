namespace EFRW.Entities1
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RailWay.Reference_Cargo")]
    public partial class ReferenceCargo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ReferenceCargo()
        {
            CarsInpDelivery = new HashSet<CarsInpDelivery>();
            CarsOutDelivery = new HashSet<CarsOutDelivery>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(200)]
        public string name_ru { get; set; }

        [Required]
        [StringLength(200)]
        public string name_en { get; set; }

        [Required]
        [StringLength(500)]
        public string name_full_ru { get; set; }

        [Required]
        [StringLength(500)]
        public string name_full_en { get; set; }

        public int etsng { get; set; }

        public int id_type { get; set; }

        public DateTime create_dt { get; set; }

        [Required]
        [StringLength(50)]
        public string create_user { get; set; }

        public DateTime? change_dt { get; set; }

        [StringLength(50)]
        public string change_user { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CarsInpDelivery> CarsInpDelivery { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CarsOutDelivery> CarsOutDelivery { get; set; }

        public virtual ReferenceTypeCargo ReferenceTypeCargo { get; set; }
    }
    //public partial class ReferenceCargo
    //{
    //    [Key]
    //    public int id { get; set; }

    //    [Required]
    //    [StringLength(200)]
    //    public string name_ru { get; set; }

    //    [Required]
    //    [StringLength(200)]
    //    public string name_en { get; set; }

    //    [Required]
    //    [StringLength(500)]
    //    public string name_full_ru { get; set; }

    //    [Required]
    //    [StringLength(500)]
    //    public string name_full_en { get; set; }

    //    public int etsng { get; set; }

    //    public int id_type { get; set; }

    //    public DateTime? datetime { get; set; }
    //}
}
