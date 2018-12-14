namespace EFMT.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MT.BufferArrivalSostav")]
    public partial class BufferArrivalSostav
    {
        public int id { get; set; }

        public DateTime datetime { get; set; }

        public int id_sostav { get; set; }

        public int id_arrival { get; set; }

        public int? count_wagons { get; set; }

        public int? count_set_wagons { get; set; }

        public DateTime? close { get; set; }

        [StringLength(100)]
        public string close_user { get; set; }

        [StringLength(250)]
        public string close_comment { get; set; }

        [StringLength(1000)]
        public string list_wagons { get; set; }

        [StringLength(1000)]
        public string list_no_set_wagons { get; set; }

        [StringLength(2000)]
        public string message { get; set; }
    }
}
