using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMT.Entities
{
    public class RouteWagonTracking
    {
        public int nvagon { get; set; }
        public int cycle { get; set; }    
        public int route { get; set; }    
        public int id_wt_min { get; set; }
        public int id_wt_max { get; set; }
        public int station_from { get; set; }
        public int station_disl { get; set; }
        public int station_end { get; set; }
        public int station_group { get; set; }
        public string name_station_from { get; set; }
        public string name_station_disl { get; set; }
        public string name_station_end { get; set; }
        public string name_station_group { get; set; }
        public DateTime dt_start { get; set; }
        public DateTime dt_stop { get; set; }
        public int dt_difference { get; set; }
        public int? time_limit { get; set; }
        public int? time_left { get; set; }
        public int? km { get; set; }
        public int? km_distance { get; set; }
        public int  id_cargo { get; set; }
        public string type_cargo { get; set; }
    }

    public class CurentWagonTracking
    {
        public string name_station { get; set; }
        public int? count_1 { get; set; }
        public int? count_surplus_1 { get; set; }
        public int? count_norma_1 { get; set; }
        public decimal? time_limit_1 { get; set; }
        public decimal? time_avg_1 { get; set; }
        public int? count_2 { get; set; }
        public int? count_surplus_2 { get; set; }
        public int? count_norma_2 { get; set; }
        public decimal? time_limit_2 { get; set; }
        public decimal? time_avg_2 { get; set; }
        public int? count_3 { get; set; }
        public int? count_surplus_3 { get; set; }
        public int? count_norma_3 { get; set; }
        public decimal? time_limit_3 { get; set; }
        public decimal? time_avg_3 { get; set; }
        public int? count_4 { get; set; }
        public int? count_surplus_4 { get; set; }
        public int? count_norma_4 { get; set; }
        public decimal? time_limit_4 { get; set; }
        public decimal? time_avg_4 { get; set; }
    }

}
