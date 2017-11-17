namespace EFRC.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RailWay.TypeCargo")]
    public partial class TypeCargo
    {
        [Key]
        public int ID { get; set; }

        [Column("TypeCargo")]
        [Required]
        [StringLength(50)]
        public string TypeCargoRU { get; set; }

        [Required]
        [StringLength(50)]
        public string TypeCargoEN { get; set; }
    }
}
