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
    public class Cargo: ISerializable
    {
        public int id { get; set; }
        public int code_etsng { get; set; }
        public string name_etsng { get; set; }
        public int code_gng { get; set; }
        public string name_gng { get; set; }
        public int? id_sap { get; set; }

        public Cargo(SerializationInfo info, StreamingContext context)
        {
            this.id = (int)info.GetValue("id", typeof(int));
            this.code_etsng = (int)info.GetValue("code_etsng", typeof(int));
            this.name_etsng = (string)info.GetValue("name_etsng", typeof(string));
            this.code_gng = (int)info.GetValue("code_etsng", typeof(int));
            this.name_gng = (string)info.GetValue("name_etsng", typeof(string));
            this.id_sap = (int?)info.GetValue("id_sap", typeof(int?));
        }

    
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
 	        throw new NotImplementedException();
        }
    }
    
    public class Reference : ClientWebAPI
    {
        private eventID eventID = eventID.RWWebAPI_RWReference;

        public Reference():base(){
        
        }

        public Cargo GetCargoOfCodeETSNG(int code_etsng) {
            return GetJSONSelect<Cargo>(@"reference/cargo/etsng_code/" + code_etsng.ToString());
        }

    }
}
