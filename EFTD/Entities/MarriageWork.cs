namespace EFTD.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TD.MarriageWork")]
    public partial class MarriageWork
    {
        public int id { get; set; }

        public DateTime date_start { get; set; }

        public DateTime? date_stop { get; set; }

        public int id_place { get; set; }

        [Required]
        [StringLength(200)]
        public string site { get; set; }

        public int id_classification { get; set; }

        public int? id_nature { get; set; }

        [Required]
        [StringLength(200)]
        public string num { get; set; }

        public int id_cause { get; set; }

        public int id_type_cause { get; set; }

        public int id_subdivision { get; set; }

        public int akt { get; set; }

        [Required]
        [StringLength(100)]
        public string locomotive_series { get; set; }

        [StringLength(100)]
        public string driver { get; set; }

        [StringLength(100)]
        public string helper { get; set; }

        [StringLength(500)]
        public string measures { get; set; }

        [StringLength(500)]
        public string note { get; set; }

        public DateTime create { get; set; }

        [Required]
        [StringLength(50)]
        public string create_user { get; set; }

        public DateTime change { get; set; }

        [Required]
        [StringLength(50)]
        public string change_user { get; set; }

        public virtual MarriageCause MarriageCause { get; set; }

        public virtual MarriageClassification MarriageClassification { get; set; }

        public virtual MarriageNature MarriageNature { get; set; }

        public virtual MarriagePlace MarriagePlace { get; set; }

        public virtual MarriageSubdivision MarriageSubdivision { get; set; }
    }
}
