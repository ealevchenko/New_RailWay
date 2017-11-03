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
        public int Code { get; set; }

        [Required]
        [StringLength(200)]
        public string CodeDescription { get; set; }

        [Column("Consignee")]
        public int Consignee1 { get; set; }
    }
}
