namespace EFRW.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RailWay.CarOperations")]
    public partial class CarOperations
    {
        public int id { get; set; }

        public int id_car { get; set; }

        public int? id_car_conditions { get; set; }

        public int? id_car_status { get; set; }

        public int? id_station { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? dt_inp_station { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? dt_out_station { get; set; }

        public int? id_way { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? dt_inp_way { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? dt_out_way { get; set; }

        public int? position { get; set; }

        public int? send_id_station { get; set; }

        public int? send_id_overturning { get; set; }

        public int? send_id_shop { get; set; }

         [Column(TypeName = "datetime")]
        public DateTime? send_dt_inp_way { get; set; }

         [Column(TypeName = "datetime")]
        public DateTime? send_dt_out_way { get; set; }

        public int? send_id_position { get; set; }

        public int? send_train1 { get; set; }

        public int? send_train2 { get; set; }

        public int? send_side { get; set; }

        [StringLength(50)]
        public string edit_user { get; set; }

         [Column(TypeName = "datetime")]
        public DateTime? edit_dt { get; set; }

        public int? parent_id { get; set; }

        public virtual CarConditions CarConditions { get; set; }

        public virtual Cars Cars { get; set; }

        public virtual CarStatus CarStatus { get; set; }

        public virtual Stations Stations { get; set; }

        public virtual Ways Ways { get; set; }
    }
}
