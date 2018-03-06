namespace EFRW.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RailWay.Stations")]
    public partial class Stations
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Stations()
        {
            CarOperations = new HashSet<CarOperations>();
            StationsNodes = new HashSet<StationsNodes>();
            StationsNodes1 = new HashSet<StationsNodes>();
            Ways = new HashSet<Ways>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(200)]
        public string name_ru { get; set; }

        [Required]
        [StringLength(200)]
        public string name_en { get; set; }

        public bool view { get; set; }

        public bool exit_uz { get; set; }

        public bool station_uz { get; set; }

        public int? id_rs { get; set; }

        public int? id_kis { get; set; }

        public bool? default_side { get; set; }

        public int? code_uz { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CarOperations> CarOperations { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StationsNodes> StationsNodes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StationsNodes> StationsNodes1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ways> Ways { get; set; }
    }
}
