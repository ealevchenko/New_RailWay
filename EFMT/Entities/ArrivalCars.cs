namespace MT.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MT.ArrivalCars")]
    public partial class ArrivalCars
    {
        public int ID { get; set; }

        public int IDSostav { get; set; }

        public int Position { get; set; }

        public int Num { get; set; }

        public int CountryCode { get; set; }

        public float Weight { get; set; }

        public int CargoCode { get; set; }

        [Required]
        [StringLength(50)]
        public string Cargo { get; set; }

        public int StationCode { get; set; }

        [Required]
        [StringLength(50)]
        public string Station { get; set; }

        public int Consignee { get; set; }

        [Required]
        [StringLength(50)]
        public string Operation { get; set; }

        [Required]
        [StringLength(50)]
        public string CompositionIndex { get; set; }

        public DateTime DateOperation { get; set; }

        public int TrainNumber { get; set; }

        public int? NumDocArrival { get; set; }

        public DateTime? Arrival { get; set; }

        public int? ParentID { get; set; }

        public virtual ArrivalSostav ArrivalSostav { get; set; }
    }
}
