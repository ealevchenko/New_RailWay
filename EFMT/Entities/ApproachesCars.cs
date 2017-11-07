namespace MT.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MT.ApproachesCars")]
    public partial class ApproachesCars
    {
        public ApproachesCars() { }

        public int ID { get; set; }

        public int IDSostav { get; set; }

        [Required]
        [StringLength(50)]
        public string CompositionIndex { get; set; }

        public int Num { get; set; }

        public int CountryCode { get; set; }

        public float Weight { get; set; }

        public int CargoCode { get; set; }

        public int TrainNumber { get; set; }

        [Required]
        [StringLength(50)]
        public string Operation { get; set; }

        public DateTime DateOperation { get; set; }

        public int CodeStationFrom { get; set; }

        public int CodeStationOn { get; set; }

        public int CodeStationCurrent { get; set; }

        public int CountWagons { get; set; }

        public int SumWeight { get; set; }

        public int FlagCargo { get; set; }

        public int Route { get; set; }

        public int Owner { get; set; }

        public int? NumDocArrival { get; set; }

        public DateTime? Arrival { get; set; }

        //public int? ParentID { get; set; }

        public virtual ApproachesSostav ApproachesSostav { get; set; }
    }
}
