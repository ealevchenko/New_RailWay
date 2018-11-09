namespace EFRW.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RW.CarInboundDelivery")]
    public partial class CarInboundDelivery
    {
        public int id { get; set; }

        public int id_car_internal { get; set; }

        public DateTime? datetime { get; set; }

        [Required]
        [StringLength(50)]
        public string composition_index { get; set; }

        public int id_arrival { get; set; }

        public int num_car { get; set; }

        public int position { get; set; }

        [StringLength(250)]
        public string note { get; set; }

        [StringLength(35)]
        public string num_nakl_sap { get; set; }

        public int? country_code { get; set; }

        public int? id_country { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? weight_cargo { get; set; }

        public int? num_doc_reweighing_sap { get; set; }

        public DateTime? dt_doc_reweighing_sap { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? weight_reweighing_sap { get; set; }

        public DateTime? dt_reweighing_sap { get; set; }

        public int? post_reweighing_sap { get; set; }

        public int? cargo_code { get; set; }

        public int? id_cargo { get; set; }

        [StringLength(18)]
        public string material_code_sap { get; set; }

        [StringLength(50)]
        public string material_name_sap { get; set; }

        [StringLength(50)]
        public string station_shipment { get; set; }

        [StringLength(3)]
        public string station_shipment_code_sap { get; set; }

        [StringLength(50)]
        public string station_shipment_name_sap { get; set; }

        public int consignee { get; set; }

        public int? id_consignee { get; set; }

        [StringLength(4)]
        public string shop_code_sap { get; set; }

        [StringLength(50)]
        public string shop_name_sap { get; set; }

        [StringLength(4)]
        public string new_shop_code_sap { get; set; }

        [StringLength(50)]
        public string new_shop_name_sap { get; set; }

        public bool? permission_unload_sap { get; set; }

        public bool? step1_sap { get; set; }

        public bool? step2_sap { get; set; }

        public virtual CarsInternal CarsInternal { get; set; }

        public virtual Directory_Cargo Directory_Cargo { get; set; }

        public virtual Directory_Consignee Directory_Consignee { get; set; }

        public virtual Directory_Country Directory_Country { get; set; }
    }
}
