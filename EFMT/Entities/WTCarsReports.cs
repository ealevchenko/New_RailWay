namespace EFMT.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MT.WTCarsReports")]
    public partial class WTCarsReports
    {
        public int id { get; set; }

        public int id_wtreport { get; set; }

        public int nvagon { get; set; }

        public virtual ListWagonsTracking ListWagonsTracking { get; set; }

        public virtual WTReports WTReports { get; set; }
    }
}
