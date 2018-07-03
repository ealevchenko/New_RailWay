using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFKIS.Entities
{
    [Table("PROM.SOSTAV")]
    public partial class PromSostav
    {
        [Key, Column(Order = 1)]
        public int N_NATUR { get; set; }
        public int? D_PR_DD { get; set; }
        public int? D_DD { get; set; }
        public int? D_PR_MM { get; set; }
        public int? D_MM { get; set; }
        public int? D_PR_YY { get; set; }
        public int? D_YY { get; set; }
        public int? T_PR_HH { get; set; }
        public int? T_HH { get; set; }
        public int? T_PR_MI { get; set; }
        public int? T_MI { get; set; }
        public int? K_ST { get; set; }
        public int? N_PUT { get; set; }
        public int? NAPR { get; set; }
        public int? P_OT { get; set; }
        public int? V_P { get; set; }
        public int? K_ST_OTPR { get; set; }
        public int? K_ST_PR { get; set; }
        public int? N_VED_PR { get; set; }
        public int? N_SOST_OT { get; set; }
        public int? N_SOST_PR { get; set; }
        public DateTime? DAT_VVOD { get; set; }
        [NotMapped]
        public DateTime? DT
        {
            get
            {
                try
                {
                    return new DateTime((int)D_YY, (int)D_MM, (int)D_DD, (int)T_HH, (int)T_MI, 00);
                }
                catch (Exception e)
                {
                    return null;
                }
            }
            private set { }
        }
        [NotMapped]
        public DateTime? DT_PR
        {
            get
            {
                try
                {
                    return new DateTime((int)D_PR_YY, (int)D_PR_MM, (int)D_PR_DD, (int)T_PR_HH, (int)T_PR_MI, 00);
                }
                catch (Exception e)
                {
                    return null;
                }
            }
            private set { }
        }
        [NotMapped]
        public DateTime DT_PR_Key
        {
            get
            {
                try
                {
                    return new DateTime((int)D_PR_YY, (int)D_PR_MM, (int)D_PR_DD, (int)T_PR_HH, (int)T_PR_MI, 00);
                }
                catch (Exception e)
                {
                    return DateTime.MinValue;
                }
            }
            private set { }
        }
    }

}
