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
    public class ReferenceCargo : ISerializable
    {
        public int id { get; set; }
        public string name_ru { get; set; }
        public string name_en { get; set; }
        public string name_full_ru { get; set; }
        public string name_full_en { get; set; }
        public int etsng { get; set; }
        public int id_type { get; set; }
        public DateTime? datetime { get; set; }

        public ReferenceCargo(SerializationInfo info, StreamingContext context)
        {
            this.id = (int)info.GetValue("id", typeof(int));
            this.name_ru = (string)info.GetValue("name_ru", typeof(string));
            this.name_en = (string)info.GetValue("name_en", typeof(string));
            this.name_full_ru = (string)info.GetValue("name_full_ru", typeof(string));
            this.name_full_en = (string)info.GetValue("name_full_en", typeof(string));
            this.etsng = (int)info.GetValue("etsng", typeof(int));
            this.id_type = (int)info.GetValue("id_type", typeof(int));
            this.datetime = ((string)info.GetValue("datetime", typeof(string))).DateNullConversion();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }
    
    public class RWReference : ClientWebAPI
    {
        private eventID eventID = eventID.RWWebAPI_RWReference;

        public RWReference():base(){
        
        }

        public ReferenceCargo GetReferenceCargoOfCodeETSNG(int code_etsng) {
            return GetJSONSelect<ReferenceCargo>(@"rw/reference/cargo/code/" + code_etsng.ToString());
        }

    }
}
