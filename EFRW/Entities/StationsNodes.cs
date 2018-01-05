namespace EFRW.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RailWay.StationsNodes")]
    public partial class StationsNodes
    {
        public int id { get; set; }

        [Required]
        [StringLength(200)]
        public string nodes { get; set; }

        public int id_station_from { get; set; }

        public bool side_station_from { get; set; }

        public int id_station_on { get; set; }

        public bool side_station_on { get; set; }

        public int transfer_type { get; set; }
    }
}
