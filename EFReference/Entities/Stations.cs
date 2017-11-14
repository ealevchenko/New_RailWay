namespace EFReference.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Reference.Stations")]
    public partial class Stations
    {
        public int id { get; set; }

        public int code { get; set; }

        public int? code_cs { get; set; }

        [Required]
        [StringLength(50)]
        public string station { get; set; }

        public int? id_ir { get; set; }

        public virtual InternalRailroad InternalRailroad { get; set; }
    }
}
