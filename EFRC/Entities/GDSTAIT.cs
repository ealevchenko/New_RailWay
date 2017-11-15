namespace EFRC.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GDSTAIT")]
    public partial class GDSTAIT
    {
        [Key]
        public int id_gdstait { get; set; }

        [StringLength(50)]
        public string name { get; set; }

        public int? id_ora { get; set; }
    }
}
