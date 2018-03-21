namespace EFKIS.Entities
{
    using EFKIS.Abstract;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KIS.BufferArrivalSostav")]
    public partial class RCBufferArrivalSostav : IBufferArrivalSostav
    {
        public int id { get; set; }

        public DateTime datetime { get; set; }

        public int day { get; set; }

        public int month { get; set; }

        public int year { get; set; }

        public int hour { get; set; }

        public int minute { get; set; }

        public int natur { get; set; }

        public int id_station_kis { get; set; }

        public int? way_num { get; set; }

        public int? napr { get; set; }

        public int? count_wagons { get; set; }

        public int? count_nathist { get; set; }

        public int? count_set_wagons { get; set; }

        public int? count_set_nathist { get; set; }

        public DateTime? close { get; set; }

        [StringLength(100)]
        public string close_user { get; set; }

        public int? status { get; set; }

        [StringLength(1000)]
        public string list_wagons { get; set; }

        [StringLength(1000)]
        public string list_no_set_wagons { get; set; }

        [StringLength(1000)]
        public string list_no_update_wagons { get; set; }

        [StringLength(1000)]
        public string message { get; set; }
    }
}
