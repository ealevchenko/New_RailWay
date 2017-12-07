namespace EFMT.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MT.ApproachesSostav")]
    public partial class ApproachesSostav
    {
        public ApproachesSostav()
        {
            ApproachesCars = new HashSet<ApproachesCars>();
        }

        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string FileName { get; set; }

        [Required]
        [StringLength(50)]
        public string CompositionIndex { get; set; }

        public DateTime DateTime { get; set; }

        public DateTime Create { get; set; }

        public DateTime? Close { get; set; }

        public DateTime? Approaches { get; set; }

        public int? ParentID { get; set; }

        public virtual ICollection<ApproachesCars> ApproachesCars { get; set; }
    }
}
