using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMT.Entities
{
    public class OperationWagonsTracking
    {
        public int id { get; set; }
        public int nvagon { get; set; }
        public string type_car_ru { get; set; }
        public string type_car_en { get; set; }
        public DateTime? dt { get; set; }
        public int? st_disl { get; set; }
        public string nst_disl { get; set; }
        public int? kodop { get; set; }
        public string nameop { get; set; }
        public string full_nameop { get; set; }
        public int? st_form { get; set; }
        public string nst_form { get; set; }
        public int? idsost { get; set; }
        public string nsost { get; set; }
        public string index { get; set; }
        public int? st_nazn { get; set; }
        public string nst_nazn { get; set; }
        public int? ntrain { get; set; }
        public int? st_end { get; set; }
        public string nst_end { get; set; }
        public int? kgr { get; set; }
        public string nkgr { get; set; }
        public int id_cargo { get; set; }
        public string cargo_ru { get; set; }
        public string cargo_en { get; set; }
        public string type_cargo_ru { get; set; }
        public string type_cargo_en { get; set; }
        public string group_cargo_ru { get; set; }
        public string group_cargo_en { get; set; }
        public int? kgrp { get; set; }
        public decimal? ves { get; set; }
        public DateTime? updated { get; set; }
        public int? kgro { get; set; }
        public int? km { get; set; }
    }

    public class CargoOperationWagonsTracking
    {
        //public int nvagon { get; set; }
        //public string type_car_ru { get; set; }
        //public string type_car_en { get; set; }
        public int? st_disl { get; set; }
        public string nst_disl { get; set; }
        public DateTime? dt { get; set; }
        public string nameop { get; set; }
        //public string full_nameop { get; set; }
        public int? st_nazn { get; set; }
        public string nst_nazn { get; set; }
        public int? st_form { get; set; }
        public string nst_form { get; set; }
        //public int id_cargo { get; set; }
        //public string cargo_ru { get; set; }
        //public string cargo_en { get; set; }
        public string type_cargo_ru { get; set; }
        public string type_cargo_en { get; set; }
        //public string group_cargo_ru { get; set; }
        //public string group_cargo_en { get; set; }
        public decimal? ves_cargo { get; set; }
        public int count_car { get; set; }
    }
}
