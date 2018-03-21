using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFKIS.Abstract
{
    public interface IBufferArrivalSostav
    {
        int id { get; set; }
        DateTime datetime { get; set; }
        int day { get; set; }
        int month { get; set; }
        int year { get; set; }
        int hour { get; set; }
        int minute { get; set; }
        int natur { get; set; }
        int id_station_kis { get; set; }
        int? way_num { get; set; }
        int? napr { get; set; }
        int? count_wagons { get; set; }
        int? count_nathist { get; set; }
        int? count_set_wagons { get; set; }
        int? count_set_nathist { get; set; }
        DateTime? close { get; set; }
        string close_user { get; set; }
        int? status { get; set; }
        string list_wagons { get; set; }
        string list_no_set_wagons { get; set; }
        string list_no_update_wagons { get; set; }
        string message { get; set; }
    }
}
