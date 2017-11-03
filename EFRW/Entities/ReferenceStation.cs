namespace EFRW.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RailWay.Reference_Station")]
    public partial class ReferenceStation
    {
        [Key]
        public int id { get; set; }

        [Required]
        [StringLength(100)]
        public string name { get; set; }

        [Required]
        [StringLength(50)]
        public string station { get; set; }

        [Required]
        [StringLength(250)]
        public string internal_railroad { get; set; }

        [Required]
        [StringLength(10)]
        public string ir_abbr { get; set; }

        [Required]
        [StringLength(250)]
        public string name_network { get; set; }

        [Required]
        [StringLength(10)]
        public string nn_abbr { get; set; }

        public int code_cs { get; set; }
    }
}
