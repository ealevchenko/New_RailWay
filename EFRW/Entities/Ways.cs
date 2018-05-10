namespace EFRW.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RailWay.Ways")]
    public partial class Ways
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Ways()
        {
            CarOperations = new HashSet<CarOperations>();
            Deadlock = new HashSet<Deadlock>();
        }

        public int id { get; set; }

        public int id_station { get; set; }

        [Required]
        [StringLength(20)]
        public string num { get; set; }

        [Required]
        [StringLength(250)]
        public string name_ru { get; set; }

        [Required]
        [StringLength(250)]
        public string name_en { get; set; }

        public int? position { get; set; }

        public int? capacity { get; set; }

        public int? id_car_status { get; set; }

        public bool? tupik { get; set; }

        public bool? dissolution { get; set; }

        public bool? defrosting { get; set; }

        public bool? overturning { get; set; }

        public bool? pto { get; set; }

        public bool? cleaning { get; set; }

        public bool? rest { get; set; }

        public int? id_rc { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CarOperations> CarOperations { get; set; }

        public virtual Stations Stations { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Deadlock> Deadlock { get; set; }
    }
}
