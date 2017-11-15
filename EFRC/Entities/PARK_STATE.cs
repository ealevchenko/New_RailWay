namespace EFRC.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PARK_STATE
    {
        public double? ID { get; set; }

        [Key]
        [Column(Order = 0, TypeName = "numeric")]
        public decimal N_DOC { get; set; }

        [Key]
        [Column(Order = 1, TypeName = "datetime2")]
        public DateTime DATE_DOC { get; set; }

        [StringLength(3)]
        public string K_STAN { get; set; }

        [StringLength(150)]
        public string NM_STAN { get; set; }

        [StringLength(3)]
        public string RAIL { get; set; }

        [StringLength(10)]
        public string ORDER_RAIL { get; set; }

        [Key]
        [Column(Order = 2, TypeName = "numeric")]
        public decimal N_VAG { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? ID_STRAN { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? DATE_PR { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? ID_SOB { get; set; }

        [StringLength(150)]
        public string NM_SOB { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? ID_GODN { get; set; }

        [StringLength(10)]
        public string NM_GODN { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? ID_GRUZ { get; set; }

        [StringLength(150)]
        public string NM_GRUZ { get; set; }

        public double? ID_MAIL { get; set; }

        [StringLength(10)]
        public string N_MAIL { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? DATE_MAIL { get; set; }

        public double? ID_MR { get; set; }

        [StringLength(150)]
        public string TEXT_MR { get; set; }

        [StringLength(150)]
        public string PRIM { get; set; }

        [StringLength(2)]
        public string NN { get; set; }

        [StringLength(150)]
        public string STAN_MAIL { get; set; }

        [StringLength(100)]
        public string TEXT { get; set; }

        [StringLength(100)]
        public string ST_OTPR { get; set; }

        [StringLength(20)]
        public string PR_GRUZ { get; set; }

        [StringLength(10)]
        public string TYPE_VAG { get; set; }

        [StringLength(20)]
        public string CEH_NAZN { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? STATUS { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? DT_STATUS { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? ID_GRUZ2 { get; set; }

        [StringLength(150)]
        public string NM_GRUZ2 { get; set; }

        [StringLength(25)]
        public string REM_TUP { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PR_V { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PR_S { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? DT_REFRESH { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PR_GR { get; set; }

        [StringLength(3)]
        public string OTCEP { get; set; }

        [StringLength(15)]
        public string NM_TP { get; set; }
    }
}
