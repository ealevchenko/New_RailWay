namespace EFReference.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Reference.Countrys")]
    public partial class Countrys
    {
        public int id { get; set; }

        [Required]
        [StringLength(100)]
        public string country { get; set; }

        [Required]
        [StringLength(2)]
        public string Alpha_2 { get; set; }

        [Required]
        [StringLength(3)]
        public string alpha_3 { get; set; }

        public int code { get; set; }

        [Required]
        [StringLength(20)]
        public string iso3166_2 { get; set; }

        public int? id_state { get; set; }

        public int? code_europe { get; set; }

        public virtual States States { get; set; }
    }
}
