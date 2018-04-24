using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMT.Entities
{
    public class CountNaturList
    {
        public int year { get; set; }
        public int month { get; set; }
        public int count { get; set; }
        public DateTime start_dt { get; set; }
        public DateTime stop_dt { get; set; }
    }
}
