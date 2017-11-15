namespace EFRC.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Loading_data_SAP
    {
        public int id { get; set; }

        public DateTime? dat_dsdu { get; set; }

        public int? VAGU { get; set; }

        [StringLength(50)]
        public string CEHU { get; set; }

        [StringLength(50)]
        public string RODU { get; set; }

        [StringLength(50)]
        public string grvu { get; set; }

        [StringLength(50)]
        public string stnu { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? tonu5 { get; set; }

        [StringLength(150)]
        public string ngru { get; set; }
    }
}
