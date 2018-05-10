namespace EFRW.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RailWay.Deadlock")]
    public partial class Deadlock
    {
        public int id { get; set; }

        public int? id_shop { get; set; }

        public int? id_way { get; set; }

        [Required]
        [StringLength(50)]
        public string name { get; set; }

        [Required]
        [StringLength(250)]
        public string description { get; set; }

        public int? id_kis { get; set; }

        public virtual Shops Shops { get; set; }

        public virtual Ways Ways { get; set; }
    }
}
