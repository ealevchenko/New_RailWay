namespace EFRW.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RailWay.Stations")]
    public partial class Stations
    {
        public int id { get; set; }

        [Required]
        [StringLength(200)]
        public string name_ru { get; set; }

        [Required]
        [StringLength(200)]
        public string name_en { get; set; }

        public bool view { get; set; }

        public bool exit_uz { get; set; }

        public bool station_uz { get; set; }

        public int? id_rs { get; set; }

        public int? id_kis { get; set; }
    }
}
