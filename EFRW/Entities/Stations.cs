namespace EFRW.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Runtime.Serialization;

    [Table("RailWay.Stations")]
    //[Serializable]
    public partial class Stations //: ISerializable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Stations()
        {
            CarOperations = new HashSet<CarOperations>();
            StationsNodes = new HashSet<StationsNodes>();
            StationsNodes1 = new HashSet<StationsNodes>();
            Ways = new HashSet<Ways>();
        }

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

        public bool? default_side { get; set; }

        public int? code_uz { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CarOperations> CarOperations { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StationsNodes> StationsNodes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StationsNodes> StationsNodes1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ways> Ways { get; set; }

        //public Stations(SerializationInfo info, StreamingContext context)
        //{
        //    this.id = (int)info.GetValue("id", typeof(int));
        //    this.name_ru = (string)info.GetValue("name_ru", typeof(string));   
        //    this.name_en = (string)info.GetValue("name_en", typeof(string));  
        //    this.view = (bool)info.GetValue("view", typeof(bool));            
        //    this.exit_uz = (bool)info.GetValue("exit_uz", typeof(bool));   
        //    this.station_uz = (bool)info.GetValue("station_uz", typeof(bool));    
        //    this.id_rs = (int?)info.GetValue("id_rs", typeof(int?));        
        //    this.id_kis = (int?)info.GetValue("id_kis", typeof(int?)); 
        //    this.default_side = (bool?)info.GetValue("default_side", typeof(bool?));   
        //    this.code_uz = (int?)info.GetValue("code_uz", typeof(int?));
        //    //this.Ways = (List<Ways>)info.GetValue("Ways", typeof(List<Ways>));
        //}

        //public void GetObjectData(SerializationInfo info, StreamingContext context)
        //{
        //    info.AddValue("id", this.id);
        //    info.AddValue("name_ru", this.name_ru);
        //    info.AddValue("name_en", this.name_en);
        //    info.AddValue("view", this.view);
        //    info.AddValue("exit_uz", this.exit_uz);
        //    info.AddValue("station_uz", this.station_uz);
        //    info.AddValue("id_rs", this.id_rs);
        //    info.AddValue("id_kis", this.id_kis);
        //    info.AddValue("default_side", this.default_side);
        //    info.AddValue("code_uz", this.code_uz);
        //    info.AddValue("Ways", this.Ways);

        //}

    }
}
