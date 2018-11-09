namespace EFRW.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RW.Directory_OwnerCars")]
    public partial class Directory_OwnerCars
    {
        public int id { get; set; }

        public int num { get; set; }

        public int id_owner { get; set; }

        public DateTime? start_lease { get; set; }

        public DateTime? end_lease { get; set; }

        public int? id_arrival { get; set; }

        [Required]
        [StringLength(50)]
        public string user_create { get; set; }

        public DateTime dt_create { get; set; }

        [StringLength(50)]
        public string user_edit { get; set; }

        public DateTime? dt_edit { get; set; }

        public virtual Directory_Cars Directory_Cars { get; set; }

        public virtual Directory_Owners Directory_Owners { get; set; }
    }
}
