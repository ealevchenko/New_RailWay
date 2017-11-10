namespace MT.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MT.Consignee")]
    public partial class Consignee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int code { get; set; }

        [Required]
        [StringLength(200)]
        public string description { get; set; }

        [Column("consignee")]
        public int consignee1 { get; set; }

        public bool send { get; set; }
    }
}
