namespace EFRW.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RailWay.Reference_Country")]
    public partial class ReferenceCountry
    {
        [Key]
        public int id { get; set; }

        [Required]
        [StringLength(100)]
        public string country { get; set; }

        public int code { get; set; }
    }
}
