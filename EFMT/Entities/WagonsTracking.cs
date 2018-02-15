namespace EFMT.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Runtime.Serialization;
    using RWConversionFunctions;

    [Table("MT.WagonsTracking")]
    [Serializable]
    public partial class WagonsTracking : ISerializable
    {
        public int id { get; set; }

        public int nvagon { get; set; }

        public int? st_disl { get; set; }

        [StringLength(50)]
        public string nst_disl { get; set; }

        public int? kodop { get; set; }

        [StringLength(50)]
        public string nameop { get; set; }

        [StringLength(100)]
        public string full_nameop { get; set; }

        public DateTime? dt { get; set; }

        public int? st_form { get; set; }

        [StringLength(50)]
        public string nst_form { get; set; }

        public int? idsost { get; set; }

        [StringLength(50)]
        public string nsost { get; set; }

        public int? st_nazn { get; set; }

        [StringLength(50)]
        public string nst_nazn { get; set; }

        public int? ntrain { get; set; }

        public int? st_end { get; set; }

        [StringLength(50)]
        public string nst_end { get; set; }

        public int? kgr { get; set; }

        [StringLength(500)]
        public string nkgr { get; set; }

        public int? kgrp { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? ves { get; set; }

        public DateTime? updated { get; set; }

        public int? kgro { get; set; }

        public int? km { get; set; }

        public WagonsTracking() { 
        
        }
        
        public WagonsTracking(SerializationInfo info, StreamingContext context)
        {
            this.nvagon = (int)info.GetValue("nvagon", typeof(int));//
            this.st_disl = (int?)info.GetValue("st_disl", typeof(int?));//
            this.nst_disl = (string)info.GetValue("nst_disl", typeof(string));
            this.kodop = (int?)info.GetValue("kodop", typeof(int?));//
            this.nameop = (string)info.GetValue("nameop", typeof(string));//
            this.dt = ((string)info.GetValue("dt", typeof(string))).DateNullConversion();//
            this.nst_form = (string)info.GetValue("nst_form", typeof(string));//
            this.st_form = (int?)info.GetValue("st_form", typeof(int?));
            this.nsost = (string)info.GetValue("nsost", typeof(string));//
            this.st_nazn = (int?)info.GetValue("st_nazn", typeof(int?));//
            this.nst_nazn = (string)info.GetValue("nst_nazn", typeof(string));//
            this.ntrain = (int?)info.GetValue("ntrain", typeof(int?));//
            this.st_end = (int?)info.GetValue("st_end", typeof(int?));//
            this.nst_end = (string)info.GetValue("nst_end", typeof(string));//
            this.idsost = (int?)info.GetValue("idsost", typeof(int?));//
            this.kgr = (int?)info.GetValue("kgr", typeof(int?));//
            this.nkgr = (string)info.GetValue("nkgr", typeof(string));//
            this.kgrp = (int?)info.GetValue("kgrp", typeof(int?));
            this.ves = (decimal?)info.GetValue("ves", typeof(decimal?));
            this.updated = ((string)info.GetValue("updated", typeof(string))).DateNullConversion();
            //this.note = (string)info.GetValue("note", typeof(string));
            this.full_nameop = (string)info.GetValue("full_nameop", typeof(string));
            //this.nquest = (string)info.GetValue("nquest", typeof(string));
            this.kgro = (int?)info.GetValue("kgro", typeof(int?));
            this.km = (int?)info.GetValue("km", typeof(int?));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }
}
