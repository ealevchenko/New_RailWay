using MessageLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WebAPI;

namespace RWWebAPI
{
    [Serializable]    
    public class KometaVagonSob: ISerializable
    {
        public int N_VAGON { get; set; }
        public int SOB { get; set; }
        public DateTime DATE_AR { get; set; }
        public DateTime? DATE_END { get; set; }
        public string ROD { get; set; }
        public DateTime? DATE_REM { get; set; }
        public string PRIM { get; set; }
        public int? CODE { get; set; }

        public KometaVagonSob(SerializationInfo info, StreamingContext context)
        {
            this.N_VAGON = (int)info.GetValue("N_VAGON", typeof(int));
            this.SOB = (int)info.GetValue("SOB", typeof(int));
            this.DATE_AR = ((string)info.GetValue("DATE_AR", typeof(string))).DateConversion();
            this.DATE_END = ((string)info.GetValue("DATE_END", typeof(string))).DateNullConversion();
            this.ROD = (string)info.GetValue("ROD", typeof(string));
            this.DATE_REM = ((string)info.GetValue("DATE_REM", typeof(string))).DateNullConversion();
            this.PRIM = (string)info.GetValue("PRIM", typeof(string));
            this.CODE = (int?)info.GetValue("CODE", typeof(int?));
        }        
        
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }

    [Serializable]  
    public class KometaSobstvForNakl: ISerializable
    {
        public string NPLAT { get; set; }
        public int SOBSTV { get; set; }
        public string ABR { get; set; }
        public int? SOD_PLAT { get; set; }
        public int? ID { get; set; }
        public int? ID2 { get; set; }

        public KometaSobstvForNakl(SerializationInfo info, StreamingContext context)
        {
            this.NPLAT = (string)info.GetValue("NPLAT", typeof(string));
            this.SOBSTV = (int)info.GetValue("SOBSTV", typeof(int));
            this.ABR = (string)info.GetValue("ABR", typeof(string));   
            this.SOD_PLAT = (int?)info.GetValue("SOD_PLAT", typeof(int?));         
            this.ID = (int?)info.GetValue("ID", typeof(int?)); 
            this.ID2 = (int?)info.GetValue("ID2", typeof(int?)); 
        }        

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }

    [Serializable] 
    public class PromGruzSP: ISerializable
    {
        public int K_GRUZ { get; set; }
        public string NAME_GR { get; set; }
        public string ABREV_GR { get; set; }
        public int GRUP_P { get; set; }
        public string NGRUP_P { get; set; }
        public int GRUP_O { get; set; }
        public int GROUP_OSV { get; set; }
        public string NGRUP_O { get; set; }
        public int? TAR_GR { get; set; }
        public int KOD1 { get; set; }
        public int KOD2 { get; set; }
        public int? K_GR { get; set; }

        public PromGruzSP(SerializationInfo info, StreamingContext context)
        {
            this.K_GRUZ = (int)info.GetValue("K_GRUZ", typeof(int));    
            this.NAME_GR = (string)info.GetValue("NAME_GR", typeof(string));        
            this.ABREV_GR = (string)info.GetValue("ABREV_GR", typeof(string));     
            this.GRUP_P = (int)info.GetValue("GRUP_P", typeof(int));   
            this.NGRUP_P = (string)info.GetValue("NGRUP_P", typeof(string)); 
            this.GRUP_O = (int)info.GetValue("GRUP_O", typeof(int));   
            this.GROUP_OSV = (int)info.GetValue("GROUP_OSV", typeof(int));   
            this.NGRUP_O = (string)info.GetValue("NGRUP_O", typeof(string)); 
            this.TAR_GR = (int?)info.GetValue("TAR_GR", typeof(int?)); 
            this.KOD1 = (int)info.GetValue("KOD1", typeof(int)); 
            this.KOD2 = (int)info.GetValue("KOD2", typeof(int));   
            this.K_GR = (int?)info.GetValue("K_GR", typeof(int?));           
        }    
    
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }

    [Serializable] 
    public class NumVagStpr1Gr: ISerializable
    {
        public int KOD_GR { get; set; }
        public string GR { get; set; }
        public int? OLD { get; set; }

        public NumVagStpr1Gr(SerializationInfo info, StreamingContext context)
        {
            this.KOD_GR = (int)info.GetValue("KOD_GR", typeof(int));
            this.GR = (string)info.GetValue("GR", typeof(string));
            this.OLD = (int?)info.GetValue("OLD", typeof(int?));           
        } 

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }

    public class Wagons : ClientWebAPI
    {
        private eventID eventID = eventID.RWWebAPI_Reference;

        public Wagons()
            : base()
        {
        
        }

        public List<KometaVagonSob> GetKometaVagonSob(int num)
        {
            return GetJSONSelect<List<KometaVagonSob>>(@"kis/kometa/vagon_sob/num_vag/" + num.ToString());
        }

        public KometaVagonSob GetKometaVagonSob(int num, DateTime dt)
        {
            return GetJSONSelect<KometaVagonSob>(String.Format(@"kis/kometa/vagon_sob/num_vag/{0}/{1}/{2}/{3}/{4}/{5}/{6}", num, dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second));
        }

        public List<KometaSobstvForNakl> GetSobstvForNakl()
        {
            return GetJSONSelect<List<KometaSobstvForNakl>>(@"kis/kometa/sobstv_for_nakl/");
        }

        public KometaSobstvForNakl GetSobstvForNakl(int kod)
        {
            return GetJSONSelect<KometaSobstvForNakl>(String.Format(@"kis/kometa/sobstv_for_nakl/sob/{0}",kod.ToString()));
        }

        public List<PromGruzSP> GetGruzSP()
        {
            return GetJSONSelect<List<PromGruzSP>>(@"kis/prom/gruz_sp/");
        }

        public PromGruzSP GetGruzSP(int kod)
        {
            return GetJSONSelect<PromGruzSP>(String.Format(@"kis/prom/gruz_sp/kod_gr/{0}", kod.ToString()));
        }

        public PromGruzSP GetGruzSPToTarGR(int? kod, bool corect)
        {
            return GetJSONSelect<PromGruzSP>(String.Format(@"kis/prom/gruz_sp/tar_gr/{0}/{1}", kod.ToString(), corect.ToString()));
        }

        public List<NumVagStpr1Gr> GetSTPR1GR()
        {
            return GetJSONSelect<List<NumVagStpr1Gr>>(@"kis/num_vag/stpr1gr/");
        }

        public NumVagStpr1Gr GetSTPR1GR(int kod)
        {
            return GetJSONSelect<NumVagStpr1Gr>(String.Format(@"kis/num_vag/stpr1gr/kod/{0}", kod.ToString()));
        }

    }
}
