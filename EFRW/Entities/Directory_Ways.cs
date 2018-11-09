namespace EFRW.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RW.Directory_Ways")]
    public partial class Directory_Ways
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Directory_Ways()
        {
            CarOperations = new HashSet<CarOperations>();
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

        public int id_type_way { get; set; }

        public int? id_car_status { get; set; }

        public int? id_station_end { get; set; }

        public int? id_shop_end { get; set; }

        public int? id_overturn_end { get; set; }

        [Required]
        [StringLength(50)]
        public string user_create { get; set; }

        public DateTime dt_create { get; set; }

        [StringLength(50)]
        public string user_edit { get; set; }

        public DateTime? dt_edit { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CarOperations> CarOperations { get; set; }

        public virtual Directory_InternalStations Directory_InternalStations { get; set; }

        public virtual Directory_InternalStations Directory_InternalStations1 { get; set; }

        public virtual Directory_Overturn Directory_Overturn { get; set; }

        public virtual Directory_Shops Directory_Shops { get; set; }

        public virtual Directory_TypeWays Directory_TypeWays { get; set; }
    }
}
