namespace EFRC.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class NeighborStations
    {
        public int id { get; set; }

        public int? id_stat1 { get; set; }

        public int? id_stat2 { get; set; }

        public int? side_stat1 { get; set; }

        public int? side_stat2 { get; set; }
    }
}
