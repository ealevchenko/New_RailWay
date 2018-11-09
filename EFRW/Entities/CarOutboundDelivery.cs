namespace EFRW.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RW.CarOutboundDelivery")]
    public partial class CarOutboundDelivery
    {
        public int id { get; set; }

        public int id_car_internal { get; set; }

        [StringLength(35)]
        public string num_nakl_sap { get; set; }

        public int? id_tupik { get; set; }

        public int? id_country_out { get; set; }

        public int? id_station_out { get; set; }

        [StringLength(500)]
        public string note { get; set; }

        public int? cargo_code { get; set; }

        public int? id_cargo { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? weight_cargo { get; set; }

        public int? num_doc_reweighing_sap { get; set; }

        public DateTime? dt_doc_reweighing_sap { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? weight_reweighing_sap { get; set; }

        public DateTime? dt_reweighing_sap { get; set; }

        public int? post_reweighing_sap { get; set; }

        public virtual CarsInternal CarsInternal { get; set; }

        public virtual Directory_Cargo Directory_Cargo { get; set; }

        public virtual Directory_Country Directory_Country { get; set; }

        public virtual Directory_ExternalStations Directory_ExternalStations { get; set; }
    }
}
