namespace EFRW.Entities1
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RailWay.CarStatus")]
    public partial class CarStatus
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CarStatus()
        {
            CarOperations = new HashSet<CarOperations>();
        }

        public int id { get; set; }

        [StringLength(50)]
        public string status_ru { get; set; }

        [StringLength(50)]
        public string status_en { get; set; }

        public int? id_status_next { get; set; }

        public int? order { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CarOperations> CarOperations { get; set; }
    }
}
