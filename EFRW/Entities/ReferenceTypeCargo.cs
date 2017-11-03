namespace EFRW.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RailWay.Reference_TypeCargo")]
    public partial class ReferenceTypeCargo
    {
        [Key]
        public int id { get; set; }

        [Required]
        [StringLength(50)]
        public string type_name_ru { get; set; }

        [Required]
        [StringLength(50)]
        public string type_name_en { get; set; }
    }
}
