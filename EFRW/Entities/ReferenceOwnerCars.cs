namespace EFRW.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RailWay.Reference_OwnerCars")]
    public partial class ReferenceOwnerCars
    {
        public int id { get; set; }

        public int num { get; set; }

        public int id_owner { get; set; }

        public DateTime? start_lease { get; set; }

        public DateTime? end_lease { get; set; }

        public int? id_arrival { get; set; }

        public DateTime create_dt { get; set; }

        [Required]
        [StringLength(50)]
        public string create_user { get; set; }

        public DateTime? change_dt { get; set; }

        [StringLength(50)]
        public string change_user { get; set; }

        public virtual ReferenceCars ReferenceCars { get; set; }

        public virtual ReferenceOwners ReferenceOwners { get; set; }
    }
}
