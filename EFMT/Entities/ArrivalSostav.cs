namespace MT.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MT.ArrivalSostav")]
    public partial class ArrivalSostav
    {
        public ArrivalSostav()
        {
            ArrivalCars = new HashSet<ArrivalCars>();
        }

        public int ID { get; set; }

        public int IDArrival { get; set; }

        [Required]
        [StringLength(50)]
        public string FileName { get; set; }

        [Required]
        [StringLength(50)]
        public string CompositionIndex { get; set; }

        public DateTime DateTime { get; set; }

        public int Operation { get; set; }

        public DateTime Create { get; set; }

        public DateTime? Close { get; set; }

        public DateTime? Arrival { get; set; }

        public int? ParentID { get; set; }

        public virtual ICollection<ArrivalCars> ArrivalCars { get; set; }
    }
}
