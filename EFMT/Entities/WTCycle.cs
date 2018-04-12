namespace EFMT.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MT.WTCycle")]
    public partial class WTCycle
    {
        public int id { get; set; }

        public int id_wt { get; set; }

        public int cycle { get; set; }

        public int route { get; set; }

        public int station_from { get; set; }

        public int station_end { get; set; }

        public virtual WagonsTracking WagonsTracking { get; set; }
    }
}
