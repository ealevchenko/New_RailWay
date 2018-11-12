namespace EFRW.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RW.Directory_Cars")]
    public partial class Directory_Cars
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Directory_Cars()
        {
            CarsInternal = new HashSet<CarsInternal>();
            Directory_OwnerCars = new HashSet<Directory_OwnerCars>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int num { get; set; }

        public int? id_type { get; set; }

        [StringLength(50)]
        public string sap { get; set; }

        [StringLength(250)]
        public string note { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? lifting_capacity { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? tare { get; set; }

        public int? id_country { get; set; }

        public int? count_axles { get; set; }

        public bool? is_output_uz { get; set; }

        [Required]
        [StringLength(50)]
        public string user_create { get; set; }

        public DateTime dt_create { get; set; }

        [StringLength(50)]
        public string user_edit { get; set; }

        public DateTime? dt_edit { get; set; }

        public virtual Directory_Country Directory_Country { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CarsInternal> CarsInternal { get; set; }

        public virtual Directory_TypeCars Directory_TypeCars { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Directory_OwnerCars> Directory_OwnerCars { get; set; }
    }
}
