namespace EFRW.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RW.CarOperations")]
    public partial class CarOperations
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CarOperations()
        {
            CarOperations1 = new HashSet<CarOperations>();
        }

        public int id { get; set; }

        public int id_car_internal { get; set; }

        public int? id_car_conditions { get; set; }

        public int? id_car_status { get; set; }

        public DateTime? dt_inp { get; set; }

        public DateTime? dt_out { get; set; }

        public int? id_way { get; set; }

        public int? position { get; set; }

        public int? train1 { get; set; }

        public int? train2 { get; set; }

        public int? side { get; set; }

        [Required]
        [StringLength(50)]
        public string user_create { get; set; }

        public DateTime dt_create { get; set; }

        [StringLength(50)]
        public string user_edit { get; set; }

        public DateTime? dt_edit { get; set; }

        public int? parent_id { get; set; }

        public virtual CarConditions CarConditions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CarOperations> CarOperations1 { get; set; }

        public virtual CarOperations CarOperations2 { get; set; }

        public virtual CarsInternal CarsInternal { get; set; }

        public virtual CarStatus CarStatus { get; set; }

        public virtual Directory_Ways Directory_Ways { get; set; }
    }
}
