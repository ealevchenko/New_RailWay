namespace EFRW.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RailWay.Reference_Cargo")]
    public partial class ReferenceCargo
    {
        [Key]
        public int id { get; set; }

        [Required]
        [StringLength(200)]
        public string name_ru { get; set; }

        [Required]
        [StringLength(200)]
        public string name_en { get; set; }

        [Required]
        [StringLength(500)]
        public string name_full_ru { get; set; }

        [Required]
        [StringLength(500)]
        public string name_full_en { get; set; }

        public int etsng { get; set; }

        public int id_type { get; set; }

        public DateTime? datetime { get; set; }
    }
}
